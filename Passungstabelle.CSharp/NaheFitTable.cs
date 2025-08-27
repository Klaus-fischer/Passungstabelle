namespace Passungstabelle.CSharp;

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using Passungstabelle.Settings;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swpublished;
using SolidWorksTools;

[Guid("09a29164-06dc-4670-bfb9-3243404d59ca")]
[ComVisible(true)]
[SwAddin(Description = "Passungstabelle Add In für SolidWorks", Title = "Passungstabelle", LoadAtStartup = true)]
public class NaheFitTable : ISwAddin
{
    private SldWorks? sldWorksApp;

    private SldWorks ISldWorksApp
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => sldWorksApp ?? throw new InvalidOperationException("Not Initialized yet.");

        [MethodImpl(MethodImplOptions.Synchronized)]
        set => sldWorksApp = value;
    }


    internal CommandHandler? CommandHandler {get; private set;}

    internal EventHandler? EventHandler { get; private set; }

    internal PassungsTabelleGenerator? PassungsTabelleGenerator { get; private set; }

    /// <summary>
    /// Public Properties
    /// </summary>
    public SldWorks SwApp
    {
        get
        {
            return this.ISldWorksApp;
        }
    }

    public GeneralSettings Settings { get; private set; } = new();

    [ComRegisterFunction()]
    public static void RegisterFunction(Type t)
    {

        // Get Custom Attribute: SwAddinAttribute
        object[] attributes;
        SwAddinAttribute SWattr = null;

        attributes = System.Attribute.GetCustomAttributes(typeof(NaheFitTable), typeof(SwAddinAttribute));

        if (attributes.Length > 0)
        {
            SWattr = (SwAddinAttribute)attributes[0];
        }
        try
        {
            var hklm = Microsoft.Win32.Registry.LocalMachine;
            var hkcu = Microsoft.Win32.Registry.CurrentUser;

            string keyname = @"SOFTWARE\SolidWorks\Addins\{" + t.GUID.ToString() + "}";
            var addinkey = hklm.CreateSubKey(keyname);
            addinkey.SetValue(null, 0);
            // addinkey.SetValue("Description", SWattr.Description)
            // addinkey.SetValue("Title", SWattr.Title)
            addinkey.SetValue("Description", ResourceLocater.Current.SwAddinDescription);
            addinkey.SetValue("Title", ResourceLocater.Current.SwAddinTitle);

            keyname = @"Software\SolidWorks\AddInsStartup\{" + t.GUID.ToString() + "}";
            addinkey = hkcu.CreateSubKey(keyname);
            addinkey.SetValue(null, SWattr.LoadAtStartup, Microsoft.Win32.RegistryValueKind.DWord);
        }
        catch (NullReferenceException nl)
        {
            Console.WriteLine(@"There was a problem registering this dll: SWattr is null.\n " + nl.Message);
            MessageBox.Show(@"There was a problem registering this dll: SWattr is null.\n" + nl.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine("There was a problem registering this dll: " + e.Message);
            MessageBox.Show("There was a problem registering this dll: " + e.Message);
        }
    }

    [ComUnregisterFunction()]
    public static void UnregisterFunction(Type t)
    {
        try
        {
            var hklm = Microsoft.Win32.Registry.LocalMachine;
            var hkcu = Microsoft.Win32.Registry.CurrentUser;

            string keyname = @"SOFTWARE\SolidWorks\Addins\{" + t.GUID.ToString() + "}";
            hklm.DeleteSubKey(keyname);

            keyname = @"Software\SolidWorks\AddInsStartup\{" + t.GUID.ToString() + "}";
            hkcu.DeleteSubKey(keyname);
        }
        catch (NullReferenceException nl)
        {
            Console.WriteLine(@"There was a problem unregistering this dll: SWattr is null.\n " + nl.Message);
            MessageBox.Show(@"There was a problem unregistering this dll: SWattr is null.\n" + nl.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine("There was a problem unregistering this dll: " + e.Message);
            MessageBox.Show("There was a problem unregistering this dll: " + e.Message);
        }

    }

    public bool ConnectToSW(object ThisSW, int cookie)
    {
        var settingsLoader = new SettingsLoader();
        settingsLoader.ReloadSettings();
        this.Settings = settingsLoader.Settings;

       this.PassungsTabelleGenerator = new PassungsTabelleGenerator(settingsLoader);

        this.ISldWorksApp = (SldWorks)ThisSW;

        // Setup the commands.
        this.CommandHandler = new CommandHandler(this.ISldWorksApp, cookie, this);
        this.CommandHandler.AddCommands();

        // Setup the Event Handlers
        this.EventHandler = new EventHandler(this.ISldWorksApp, this);
        this.EventHandler.AttachEventHandlers();

        return true;
    }

    public bool DisconnectFromSW()
    {
        bool DisconnectFromSWRet = default;

        this.CommandHandler?.RemoveCommandMgr();
        this.CommandHandler?.Dispose();
        this.CommandHandler = null;

        this.EventHandler?.DetachEventHandlers();
        this.EventHandler = null;

        Marshal.ReleaseComObject(this.ISldWorksApp);
        this.ISldWorksApp = null;

        GC.Collect();
        GC.WaitForPendingFinalizers();

        GC.Collect();
        GC.WaitForPendingFinalizers();

        DisconnectFromSWRet = true;
        return DisconnectFromSWRet;
    }
    
    internal void ExecuteOnCurrentSheet(DrawingDoc drawing)
    {
        if (drawing.GetCurrentSheet() is ISheet sheet)
        {
            this.PassungsTabelleGenerator?.Execute(drawing, sheet);
            return;
        }

        this.PassungsTabelleGenerator?.Execute(drawing);
    }

    internal void ExecuteOnSaveNotify(DrawingDoc drawing)
    {
        if (this.Settings.Eventgesteuert && this.Settings.Event_BevorSave)
        {
            this.Execute(drawing);
        }
    }

    internal void ExecuteOnRegenPostNotify(DrawingDoc drawing)
    {
        if (this.Settings.Eventgesteuert && this.Settings.Event_AfterRegen)
        {
            this.Execute(drawing);
        }
    }
    private void Execute(DrawingDoc drawing) 
        => this.PassungsTabelleGenerator?.Execute(drawing);

}