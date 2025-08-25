// <copyright file="CommandHandler" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.CSharp;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using SolidWorksTools.File;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

[Guid("5C8AC6DE-D5A9-4AEB-AA40-7D9F869B0622")]
[ComVisible(true)]
public class CommandHandler : IDisposable
{
    public const int mainCmdGroupID = 3130;
    public const int mainItemID1 = 31300;
    public const int mainItemID2 = 31301;
    public const int mainItemID3 = 31301;
    public const int flyoutGroupID = 91;
    private readonly SldWorks sldWorks;
    private readonly int cookie;
    private readonly NaheFitTable addIn;
    private readonly CommandManager swCommandManager;

    public CommandHandler(SldWorks sldWorks, int cookie, NaheFitTable addIn)
    {
        this.sldWorks = sldWorks;
        this.cookie = cookie;
        this.addIn = addIn;
        sldWorks.SetAddinCallbackInfo(0, this, this.cookie);
        this.swCommandManager = sldWorks.GetCommandManager(this.cookie);
    }

    public void AddCommands()
    {
        ICommandGroup cmdGroup;
        using var bitmapHandler = new BitmapHandler(); ;

        int cmdIndex0;
        int cmdIndex1;
        int cmdIndex2;
        string Title = My.Resources.Passungstabelle;
        string ToolTip = My.Resources.Passungstabelle_Add_In_für_SolidWorks;
        int[] docTypes = [(int)swDocumentTypes_e.swDocDRAWING];

        var thisAssembly = Assembly.GetAssembly(this.GetType());

        int cmdGroupErr = 0;
        bool ignorePrevious = false;

        object registryIDs = null;
        bool getDataResult = this.swCommandManager.GetGroupDataFromRegistry(mainCmdGroupID, out registryIDs);

        int[] knownIDs = [mainItemID1, mainItemID2, mainItemID3];

        if (getDataResult)
        {
            if (!this.CompareIDs(registryIDs.AsArrayOfType<int>(), knownIDs)) // if the IDs don't match, reset the commandGroup
            {
                ignorePrevious = true;
            }
        }

        cmdGroup = this.swCommandManager.CreateCommandGroup2(mainCmdGroupID, Title, ToolTip, "", -1, ignorePrevious, ref cmdGroupErr);
        if (cmdGroup is null || thisAssembly is null)
        {
            throw new NullReferenceException();
        }

        cmdGroup.LargeIconList = bitmapHandler.CreateFileFromResourceBitmap("Passungstabellen.MainLarge.png", thisAssembly);
        cmdGroup.SmallIconList = bitmapHandler.CreateFileFromResourceBitmap("Passungstabellen.MainSmall.png", thisAssembly);

        int menuToolbarOption = (int)(swCommandItemType_e.swMenuItem | swCommandItemType_e.swToolbarItem);

        cmdIndex0 = cmdGroup.AddCommandItem2(
            My.Resources.Passungstabelle,
            -1,
            My.Resources.Passungstabelle,
            My.Resources.Passungstabelle,
            0,
            nameof(ErstelleTabelle),
            "",
            mainItemID1,
            menuToolbarOption);

        cmdIndex1 = cmdGroup.AddCommandItem2(
            My.Resources.Passungstabelle_Setup,
            -1,
            My.Resources.Passungstabelle_Setup,
            My.Resources.Passungstabelle_Setup,
            1,
            nameof(PassungsTabelleSetup),
            "",
            mainItemID2,
            menuToolbarOption);

        cmdIndex2 = cmdGroup.AddCommandItem2(
            My.Resources.Passungstabelle_Hilfe,
            -1,
            My.Resources.Passungstabelle_Hilfe,
            My.Resources.Passungstabelle_Hilfe,
            2,
            nameof(PassungsTabelleHilfe),
            "",
            mainItemID3,
            menuToolbarOption);

        cmdGroup.HasToolbar = true;
        cmdGroup.HasMenu = true;
        cmdGroup.Activate();

        foreach (int docType in docTypes)
        {
            ICommandTab? cmdTab = this.swCommandManager.GetCommandTab(docType, Title);
            bool bResult;

            if (cmdTab is not null && (!getDataResult || ignorePrevious)) // if tab exists, but we have ignored the registry info, re-create the tab.  Otherwise the ids won't matchup and the tab will be blank
            {
                bool res = this.swCommandManager.RemoveCommandTab((CommandTab)cmdTab);
                cmdTab = null;
            }

            if (cmdTab is null)
            {
                cmdTab = this.swCommandManager.AddCommandTab(docType, Title);

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
            this.swCommandManager.RemoveCommandGroup(mainCmdGroupID);
            this.swCommandManager.RemoveFlyoutGroup(flyoutGroupID);
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
        //var p = new Process();
        //// Dim psi As New ProcessStartInfo(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.Location) & "\Help\HtmlHelp\Willkommen.html")


        //var cinfo = Thread.CurrentThread.CurrentUICulture;

        //if (cinfo.TwoLetterISOLanguageName != "de")
        //{
        //    Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        //}
        //else
        //{
        //    Thread.CurrentThread.CurrentUICulture = new CultureInfo("de-DE");
        //}

        //// Dim psi As New ProcessStartInfo(macro_pfad & "\Help\HtmlHelp\Willkommen.html")
        //var psi = new ProcessStartInfo(this.macro_pfad + My.Resources.HtmlHelpPfad);

        //Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");

        //psi.Verb = "open";
        //p.StartInfo = psi;
        //p.Start();
    }

    /// <summary>
    /// 0 - disabled, 1 - enabled, 2 - selected, 3 - selected & enabled.
    /// </summary>
    /// <returns></returns>
    public int CanErstelleTabelle()
    {
        var openDoc = this.sldWorks.ActiveDoc as IModelDoc2;
        if (openDoc is null || openDoc.GetType() == (int)swDocumentTypes_e.swDocDRAWING)
        {
            return 0;
        }

        return 1;
    }

    public void ErstelleTabelle()
    {
        var openDoc = this.sldWorks.ActiveDoc as DrawingDoc;
        if (openDoc is null)
        {
            return;
        }

        this.addIn.Execute(openDoc);
    }

    public void Dispose()
    {
        Marshal.ReleaseComObject(this.swCommandManager);
    }
}
