using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using System.Threading;
using System.Windows;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.Win32;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using SolidWorks.Interop.swpublished;
using SolidWorksTools;
using SolidWorksTools.File;
using My = Passungstabelle.CSharp.My;

namespace Passungstabelle.CSharp;


[Guid("09a29164-06dc-4670-bfb9-3243404d59ca")]
[ComVisible(true)]
[SwAddin(Description = "Passungstabelle Add In für SolidWorks", Title = "Passungstabelle", LoadAtStartup = true)]
public class NaheFitTable : ISwAddin
{

    #region Local Variables
    private SldWorks _ISwApp;

    private SldWorks ISwApp
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _ISwApp;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            _ISwApp = value;
        }
    }
    private ICommandManager iCmdMgr;
    private int addinID;
    private Dictionary<ModelDoc2, DocumentEventHandler> openDocs;
    private SldWorks SwEventPtr;
    // Dim ppage As UserPMPage
    private BitmapHandler iBmp;
    private string macro_pfad = "";
    private string Setup_pfad = "";

    public const int mainCmdGroupID = 3130;
    public const int mainItemID1 = 31300;
    public const int mainItemID2 = 31301;
    public const int mainItemID3 = 31301;
    public const int flyoutGroupID = 91;

    private PassungsTabelleGenerator PassungsTabelleGenerator { get; }
    internal CommandHandler? CommandHandler {get; private set;}

    // Public Properties
    public SldWorks SwApp
    {
        get
        {
            return this.ISwApp;
        }
    }

    public ICommandManager CmdMgr
    {
        get
        {
            return this.iCmdMgr;
        }
    }

    public Dictionary<ModelDoc2, DocumentEventHandler> OpenDocumentsTable
    {
        get
        {
            return this.openDocs;
        }
    }

    public GeneralSettings Settings { get; private set; }
    #endregion

    #region SolidWorks Registration

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
            addinkey.SetValue("Description", My.Resources.Passungstabelle_Add_In_für_SolidWorks);
            addinkey.SetValue("Title", My.Resources.Passungstabelle);

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

    #endregion

    #region ISwAddin Implementation

    public bool ConnectToSW(object ThisSW, int cookie)
    {
        bool ConnectToSWRet = default;
        var settingsLoader = new SettingsLoader();
        settingsLoader.ReloadSettings();
        this.Settings = settingsLoader.Settings;

        var pt = new PassungsTabelleGenerator(settingsLoader.Settings, settingsLoader.TableSettings);


        this.ISwApp = (SldWorks)ThisSW;
        this.addinID = cookie;

        // Ini Datei lesen
        this.macro_pfad = this.GetAppPath();
        this.Setup_pfad = this.GetSetupPath();

        this.CommandHandler = new CommandHandler(this.ISwApp, cookie);
        this.CommandHandler.AddCommands();

        // Setup the Event Handlers
        this.SwEventPtr = this.ISwApp;
        this.openDocs = new();
        this.AttachEventHandlers();

        ConnectToSWRet = true;
        return ConnectToSWRet;
    }

    public bool DisconnectFromSW()
    {
        bool DisconnectFromSWRet = default;

        this.CommandHandler?.RemoveCommandMgr();

        // RemovePMP()
        this.DetachEventHandlers();

        Marshal.ReleaseComObject(this.iCmdMgr);
        this.iCmdMgr = null;
        Marshal.ReleaseComObject(this.ISwApp);
        this.ISwApp = null;
        // The addin _must_ call GC.Collect() here in order to retrieve all managed code pointers 
        GC.Collect();
        GC.WaitForPendingFinalizers();

        GC.Collect();
        GC.WaitForPendingFinalizers();

        DisconnectFromSWRet = true;
        return DisconnectFromSWRet;
    }
    #endregion

    #region Event Methods
    public void AttachEventHandlers()
    {
        this.AttachSWEvents();

        // Listen for events on all currently open docs
        this.AttachEventsToAllDocuments();
    }

    public void DetachEventHandlers()
    {
        this.DetachSWEvents();

        // Close events on all currently open docs
        foreach (var item in this.openDocs)
        {
            item.Value.DetachEventHandlers();
        }

        this.openDocs.Clear();
    }

    public void AttachSWEvents()
    {
        try
        {
            this.ISwApp.FileNewNotify2 += this.SldWorks_FileNewNotify2;
            this.ISwApp.FileOpenPostNotify += this.SldWorks_FileOpenPostNotify;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public void DetachSWEvents()
    {
        try
        {
            this.ISwApp.FileNewNotify2 -= this.SldWorks_FileNewNotify2;
            this.ISwApp.FileOpenPostNotify -= this.SldWorks_FileOpenPostNotify;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public void AttachEventsToAllDocuments()
    {
        ModelDoc2 modDoc;
        modDoc = (ModelDoc2)this.ISwApp.GetFirstDocument();
        while (modDoc is not null)
        {
            if (!this.openDocs.ContainsKey(modDoc))
            {
                this.AttachModelDocEventHandler(modDoc);
            }
            modDoc = (ModelDoc2)modDoc.GetNext();
        }
    }

    public bool AttachModelDocEventHandler(ModelDoc2 modDoc)
    {
        if (modDoc is null)
        {
            return false;
        }

        if (this.openDocs.ContainsKey(modDoc))
        {
            return false;
        }

        DocumentEventHandler? docHandler = null;
        switch (modDoc.GetType())
        {
            case (int)swDocumentTypes_e.swDocPART:

                docHandler = new PartEventHandler(this.ISwApp, this, modDoc);
                break;

            case (int)swDocumentTypes_e.swDocASSEMBLY:

                docHandler = new AssemblyEventHandler(this.ISwApp, this, modDoc);
                break;

            case (int)swDocumentTypes_e.swDocDRAWING:

                docHandler = new DrawingEventHandler(this.ISwApp, this, modDoc);
                break;
            default:
                break;
        }

        if (docHandler is not null)
        {
            docHandler.AttachEventHandlers();
            this.openDocs.Add(modDoc, docHandler);
        }

        return default;
    }

    public void DetachModelEventHandler(ModelDoc2 modDoc)
    {
        DocumentEventHandler docHandler;
        docHandler = (DocumentEventHandler)this.openDocs[modDoc];
        this.openDocs.Remove(modDoc);
    }
    #endregion

    #region Event Handlers

    public int SldWorks_FileNewNotify2(object newDoc, int doctype, string templateName)
    {
        this.AttachEventsToAllDocuments();
        return default;
    }

    public int SldWorks_FileOpenPostNotify(string FileName)
    {
        this.AttachEventsToAllDocuments();
        return default;
    }

    #endregion

    #region UI Callbacks

    public string GetAppPath()
    {
        string GetAppPathRet = default;
        string path;
        path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        GetAppPathRet = path;
        return GetAppPathRet;
    }


    // Function   GetSetupPath
    // Paramter:  keine
    // Ergebnis:  liefert den Pfad der Setup-Datei
    public string GetSetupPath()
    {
        string? path;
        path = Registry.LocalMachine.GetValue(@"Software\nahe", "SetupPfad") as string;
        if (path is null)
        {
            path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        return path;
    }

    internal void ExecuteOnSaveNotify(DrawingDoc drawing)
    {
        throw new NotImplementedException();
    }

    internal void ExecuteOnRegenPostNotify(DrawingDoc drawing)
    {
        throw new NotImplementedException();
    }

    #endregion

}