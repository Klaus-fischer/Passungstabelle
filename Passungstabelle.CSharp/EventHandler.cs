using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;

namespace Passungstabelle.CSharp;

public class EventHandler(SldWorks sldWorks, NaheFitTable addIn)
{
    private Dictionary<ModelDoc2, DocumentEventHandler> openDocs = new();
    private readonly SldWorks sldWorks = sldWorks;
    private readonly NaheFitTable addIn = addIn;

    public Dictionary<ModelDoc2, DocumentEventHandler> OpenDocumentsTable
    {
        get
        {
            return this.openDocs;
        }
    }

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
            this.sldWorks.FileNewNotify2 += this.SldWorks_FileNewNotify2;
            this.sldWorks.FileOpenPostNotify += this.SldWorks_FileOpenPostNotify;
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
            this.sldWorks.FileNewNotify2 -= this.SldWorks_FileNewNotify2;
            this.sldWorks.FileOpenPostNotify -= this.SldWorks_FileOpenPostNotify;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public void AttachEventsToAllDocuments()
    {
        ModelDoc2 modDoc;
        modDoc = (ModelDoc2)this.sldWorks.GetFirstDocument();
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

                docHandler = new PartEventHandler(this.sldWorks, this.addIn, modDoc);
                break;

            case (int)swDocumentTypes_e.swDocASSEMBLY:

                docHandler = new AssemblyEventHandler(this.sldWorks, this.addIn, modDoc);
                break;

            case (int)swDocumentTypes_e.swDocDRAWING:

                docHandler = new DrawingEventHandler(this.sldWorks, this.addIn, modDoc);
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
        this.openDocs.Remove(modDoc);
    }

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
}
