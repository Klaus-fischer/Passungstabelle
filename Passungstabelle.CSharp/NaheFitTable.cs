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

    public bool eventgesteuert = false;
    public bool Event_AfterRegen = false;
    public bool Event_BevorSave = false;

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

    public bool ConnectToSW(object ThisSW, int Cookie)
    {
        bool ConnectToSWRet = default;
        var pt = new PassungstabellenHandler();


        this.ISwApp = (SldWorks)ThisSW;
        this.addinID = Cookie;

        // Ini Datei lesen
        this.macro_pfad = this.GetAppPath();
        this.Setup_pfad = this.GetSetupPath();
        pt.Macro_pfad = this.macro_pfad;
        pt.Setup_pfad = this.Setup_pfad;

        // Nur wenn die Setup-Datei gefunden wird
        if (pt.Check_for_setup())
        {
            // Kann auch der Wert für die Event-Steuerung gesetz werden
            this.eventgesteuert = Conversions.ToBoolean(pt.Attr_generell.Eventgesteuert);
            if (this.eventgesteuert == true)
            {
                this.Event_AfterRegen = Conversions.ToBoolean(pt.Attr_generell.Event_AfterRegen);
                this.Event_BevorSave = Conversions.ToBoolean(pt.Attr_generell.Event_BevorSave);
            }
        }

        pt = null;

        // Setup callbacks
        this.ISwApp.SetAddinCallbackInfo(0, this, this.addinID);

        // Setup the Command Manager
        this.iCmdMgr = this.ISwApp.GetCommandManager(Cookie);
        this.AddCommandMgr();

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

        this.RemoveCommandMgr();
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

    #region UI Methods
    public void AddCommandMgr()
    {

        ICommandGroup cmdGroup;

        if (this.iBmp is null)
        {
            this.iBmp = new BitmapHandler();
        }

        Assembly thisAssembly;

        int cmdIndex0;
        int cmdIndex1;
        int cmdIndex2;
        string Title = My.Resources.Passungstabelle;
        string ToolTip = My.Resources.Passungstabelle_Add_In_für_SolidWorks;
        int[] docTypes = new int[] { (int)swDocumentTypes_e.swDocDRAWING };

        thisAssembly = Assembly.GetAssembly(this.GetType());

        int cmdGroupErr = 0;
        bool ignorePrevious = false;

        object registryIDs = null;
        bool getDataResult = this.iCmdMgr.GetGroupDataFromRegistry(mainCmdGroupID, out registryIDs);

        int[] knownIDs = new int[3] { mainItemID1, mainItemID2, mainItemID3 };

        if (getDataResult)
        {
            if (!this.CompareIDs((int[])registryIDs, knownIDs)) // if the IDs don't match, reset the commandGroup
            {
                ignorePrevious = true;
            }
        }

        cmdGroup = this.iCmdMgr.CreateCommandGroup2(mainCmdGroupID, Title, ToolTip, "", -1, ignorePrevious, ref cmdGroupErr);
        if (cmdGroup is null | thisAssembly is null)
        {
            throw new NullReferenceException();
        }

        cmdGroup.LargeIconList = this.iBmp.CreateFileFromResourceBitmap("Passungstabellen.MainLarge.png", thisAssembly);
        cmdGroup.SmallIconList = this.iBmp.CreateFileFromResourceBitmap("Passungstabellen.MainSmall.png", thisAssembly);

        int menuToolbarOption = (int)(swCommandItemType_e.swMenuItem | swCommandItemType_e.swToolbarItem);

        // cmdIndex0 = cmdGroup.AddCommandItem2("PassungstabellenHandler", -1, "PassungstabellenHandler", "PassungstabellenHandler", 0, "ErstelleTabelle", "", mainItemID1, menuToolbarOption)
        cmdIndex0 = cmdGroup.AddCommandItem2(My.Resources.Passungstabelle, -1, My.Resources.Passungstabelle, My.Resources.Passungstabelle, 0, "ErstelleTabelle", "", mainItemID1, menuToolbarOption);
        // cmdIndex1 = cmdGroup.AddCommandItem2("PassungstabellenHandler Setup", -1, "PassungstabellenHandler Setup", "PassungstabellenHandler Setup", 1, "PassungsTabelleSetup", "", mainItemID2, menuToolbarOption)
        cmdIndex1 = cmdGroup.AddCommandItem2(My.Resources.Passungstabelle_Setup, -1, My.Resources.Passungstabelle_Setup, My.Resources.Passungstabelle_Setup, 1, "PassungsTabelleSetup", "", mainItemID2, menuToolbarOption);
        // cmdIndex2 = cmdGroup.AddCommandItem2("PassungstabellenHandler Hilfe", -1, "PassungstabellenHandler Hilfe", "PassungstabellenHandler Hilfe", 2, "PassungsTabelleHilfe", "", mainItemID3, menuToolbarOption)
        cmdIndex2 = cmdGroup.AddCommandItem2(My.Resources.Passungstabelle_Hilfe, -1, My.Resources.Passungstabelle_Hilfe, My.Resources.Passungstabelle_Hilfe, 2, "PassungsTabelleHilfe", "", mainItemID3, menuToolbarOption);

        cmdGroup.HasToolbar = true;
        cmdGroup.HasMenu = true;
        cmdGroup.Activate();

        foreach (int docType in docTypes)
        {
            ICommandTab cmdTab = this.iCmdMgr.GetCommandTab(docType, Title);
            bool bResult;

            if (cmdTab is not null & !getDataResult | ignorePrevious) // if tab exists, but we have ignored the registry info, re-create the tab.  Otherwise the ids won't matchup and the tab will be blank
            {
                bool res = this.iCmdMgr.RemoveCommandTab((CommandTab)cmdTab);
                cmdTab = null;
            }

            if (cmdTab is null)
            {
                cmdTab = this.iCmdMgr.AddCommandTab(docType, Title);

                var cmdBox = cmdTab.AddCommandTabBox();

                var cmdIDs = new int[3];
                var TextType = new int[3];

                cmdIDs[0] = cmdGroup.get_CommandID(cmdIndex0);
                TextType[0] = (int)swCommandTabButtonTextDisplay_e.swCommandTabButton_TextHorizontal;

                cmdIDs[1] = cmdGroup.get_CommandID(cmdIndex1);
                TextType[1] = (int)swCommandTabButtonTextDisplay_e.swCommandTabButton_TextHorizontal;

                cmdIDs[2] = cmdGroup.get_CommandID(cmdIndex2);
                TextType[2] = (int)swCommandTabButtonTextDisplay_e.swCommandTabButton_TextHorizontal;

                bResult = cmdBox.AddCommands(cmdIDs, TextType);

                var cmdBox1 = cmdTab.AddCommandTabBox();
                cmdIDs = new int[2];
                TextType = new int[2];

                bResult = cmdBox1.AddCommands(cmdIDs, TextType);
            }
        }

        thisAssembly = null;
    }

    public void RemoveCommandMgr()
    {
        try
        {
            this.iBmp.Dispose();
            this.iCmdMgr.RemoveCommandGroup(mainCmdGroupID);
            this.iCmdMgr.RemoveFlyoutGroup(flyoutGroupID);
        }
        catch (Exception e)
        {
        }
    }

    public bool CompareIDs(int[] storedIDs, int[] addinIDs)
    {

        var storeList = new List<int>(storedIDs);
        var addinList = new List<int>(addinIDs);

        addinList.Sort();
        storeList.Sort();

        if (!(addinList.Count == storeList.Count))
        {
            return false;
        }
        else
        {
            for (int i = 0, loopTo = addinList.Count - 1; i <= loopTo; i++)
            {
                if (!(addinList[i] == storeList[i]))
                {

                    return false;
                }
            }
        }

        return true;
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
    public void PassungsTabelleSetup()
    {
        //// Thread.CurrentThread.CurrentCulture = New CultureInfo("en-US")

        //// CultureInfo.DefaultThreadCurrentCulture = New CultureInfo("en-US")
        //var cinfo = Thread.CurrentThread.CurrentUICulture;
        //// MsgBox("Aktuell info: " + cinfo.Name, vbOKOnly, "Meldung")

        //if (cinfo.TwoLetterISOLanguageName != "de")
        //{
        //    Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        //}
        //else
        //{
        //    Thread.CurrentThread.CurrentUICulture = new CultureInfo("de-DE");
        //}

        //var setupdlg = new SetupDialog() { Swapp = SwApp };
        //// cinfo = Thread.CurrentThread.CurrentUICulture
        //// MsgBox("info: " + cinfo.Name, vbOKOnly, "Meldung")
        //Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
        //setupdlg.ShowDialog();
        //setupdlg.Close();

        //setupdlg = (SetupDialog)null;
    }

    public void PassungsTabelleHilfe()
    {
        var p = new Process();
        // Dim psi As New ProcessStartInfo(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.Location) & "\Help\HtmlHelp\Willkommen.html")


        var cinfo = Thread.CurrentThread.CurrentUICulture;

        if (cinfo.TwoLetterISOLanguageName != "de")
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        }
        else
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("de-DE");
        }

        // Dim psi As New ProcessStartInfo(macro_pfad & "\Help\HtmlHelp\Willkommen.html")
        var psi = new ProcessStartInfo(this.macro_pfad + My.Resources.HtmlHelpPfad);

        Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");

        psi.Verb = "open";
        p.StartInfo = psi;
        p.Start();
    }

    public void ErstelleTabelle()
    {
        var fittable = new PassungstabellenHandler();
        var cinfo = Thread.CurrentThread.CurrentUICulture;

        if (cinfo.TwoLetterISOLanguageName != "de")
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        }
        else
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("de-DE");
        }

        Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");

        fittable.Main(this.ISwApp);
    }

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

    #endregion

}