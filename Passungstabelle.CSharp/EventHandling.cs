using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SolidWorks.Interop.sldworks;

namespace Passungstabelle.CSharp;

// Base class for model event handlers
public class DocumentEventHandler
{
    protected readonly Dictionary<ModelView,DocView> openModelViews = new ();
    protected readonly NaheFitTable userAddin;
    protected readonly ModelDoc2 iDocument;
    protected readonly SldWorks iSwApp;

    public DocumentEventHandler(SldWorks sw, NaheFitTable addin, ModelDoc2 model)
    {
        this.iSwApp = sw;
        this.userAddin = addin;
        this.iDocument = model;
    }

    public virtual bool AttachEventHandlers()
    {
        return default;
    }

    public virtual bool DetachEventHandlers()
    {
        return default;
    }

    public bool ConnectModelViews()
    {
        ModelView iModelView;
        iModelView = (ModelView)this.iDocument.GetFirstModelView();

        while (iModelView is not null)
        {
            if (!this.openModelViews.ContainsKey(iModelView))
            {
                var mView = new DocView(this.userAddin, iModelView, this);
                mView.AttachEventHandlers();
                this.openModelViews.Add(iModelView, mView);
            }

            iModelView = (ModelView)iModelView.GetNext();
        }

        return default;
    }

    public bool DisconnectModelViews()
    {
        foreach (var item in this.openModelViews)
        {
            item.Value.DetachEventHandlers();
        }

        this.openModelViews.Clear();

        return default;
    }

    public void DetachModelViewEventHandler(ModelView mView)
    {
        if (this.openModelViews.TryGetValue(mView, out var docView))
        {
            docView.DetachEventHandlers();
            this.openModelViews.Remove(mView);          
        }

    }
}

// Class to listen for Part Events
public class PartEventHandler : DocumentEventHandler
{

    private PartDoc _IPart;

    private PartDoc IPart
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _IPart;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            _IPart = value;
        }
    }

    public PartEventHandler(SldWorks sw, NaheFitTable addin, ModelDoc2 model)
        :base(sw, addin, model) 
    {
        this.IPart = (PartDoc)model;
    }

    public override bool AttachEventHandlers()
    {
        return default;
        // AddHandler iPart.DestroyNotify, AddressOf Me.PartDoc_DestroyNotify
        // AddHandler iPart.NewSelectionNotify, AddressOf Me.PartDoc_NewSelectionNotify
        // ConnectModelViews()
    }

    public override bool DetachEventHandlers()
    {
        return default;
        // RemoveHandler iPart.DestroyNotify, AddressOf Me.PartDoc_DestroyNotify
        // RemoveHandler iPart.NewSelectionNotify, AddressOf Me.PartDoc_NewSelectionNotify

        // DisconnectModelViews()

        // userAddin.DetachModelEventHandler(iDocument)
    }

    public int PartDoc_DestroyNotify()
    {
        this.DetachEventHandlers();
        return default;
    }

    // Function PartDoc_NewSelectionNotify() As Integer

    // End Function
}

// Class to listen for Assembly Events
public class AssemblyEventHandler : DocumentEventHandler
{

    private AssemblyDoc _IAssembly;

    private AssemblyDoc IAssembly
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _IAssembly;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            _IAssembly = value;
        }
    }

    public AssemblyEventHandler(SldWorks sw, NaheFitTable addin, ModelDoc2 model)
        :base(sw, addin, model)
    {
        this._IAssembly = (AssemblyDoc)model;

    }

    public override bool AttachEventHandlers()
    {
        return default;
        // AddHandler iAssembly.DestroyNotify, AddressOf Me.AssemblyDoc_DestroyNotify
        // AddHandler iAssembly.NewSelectionNotify, AddressOf Me.AssemblyDoc_NewSelectionNotify
        // AddHandler iAssembly.ComponentStateChangeNotify, AddressOf Me.AssemblyDoc_ComponentStateChangeNotify
        // AddHandler iAssembly.ComponentStateChangeNotify2, AddressOf Me.AssemblyDoc_ComponentStateChangeNotify2
        // AddHandler iAssembly.ComponentVisualPropertiesChangeNotify, AddressOf Me.AssemblyDoc_ComponentVisiblePropertiesChangeNotify
        // AddHandler iAssembly.ComponentDisplayStateChangeNotify, AddressOf Me.AssemblyDoc_ComponentDisplayStateChangeNotify



        // ConnectModelViews()
    }

    public override bool DetachEventHandlers()
    {
        return default;
        // RemoveHandler iAssembly.DestroyNotify, AddressOf Me.AssemblyDoc_DestroyNotify
        // RemoveHandler iAssembly.NewSelectionNotify, AddressOf Me.AssemblyDoc_NewSelectionNotify
        // RemoveHandler iAssembly.ComponentStateChangeNotify, AddressOf Me.AssemblyDoc_ComponentStateChangeNotify
        // RemoveHandler iAssembly.ComponentStateChangeNotify2, AddressOf Me.AssemblyDoc_ComponentStateChangeNotify2
        // RemoveHandler iAssembly.ComponentVisualPropertiesChangeNotify, AddressOf Me.AssemblyDoc_ComponentVisiblePropertiesChangeNotify
        // RemoveHandler iAssembly.ComponentDisplayStateChangeNotify, AddressOf Me.AssemblyDoc_ComponentDisplayStateChangeNotify

        // DisconnectModelViews()

        // userAddin.DetachModelEventHandler(iDocument)
    }

    public int AssemblyDoc_DestroyNotify()
    {
        this.DetachEventHandlers();
        return default;
    }
}

// Class to listen for Drawing Events
public class DrawingEventHandler : DocumentEventHandler
{
    private DrawingDoc _IDrawing;

    private DrawingDoc IDrawing
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _IDrawing;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            _IDrawing = value;
        }
    }


    public DrawingEventHandler(SldWorks sw, NaheFitTable addin, ModelDoc2 model)
        :base(sw, addin, model)
    {
        this.IDrawing = (DrawingDoc)model;
    }

    public override bool AttachEventHandlers()
    {
        // AddHandler iDrawing.DestroyNotify, AddressOf Me.DrawingDoc_DestroyNotify
        // AddHandler IDrawing.NewSelectionNotify, AddressOf Me.DrawingDoc_NewSelectionNotify
        if (this.userAddin.eventgesteuert == true)
        {
            if (this.userAddin.Event_AfterRegen == true)
            {
                this.IDrawing.RegenPostNotify += this.DrawingDoc_RegenPostNotify;
            }
            if (this.userAddin.Event_BevorSave == true)
            {
                this.IDrawing.FileSaveNotify += this.DrawingDocEvents_FileSaveNotify;
                this.IDrawing.FileSaveAsNotify2 += this.DrawingDocEvents_FileSaveNotify;
            }

        }

        return default;
        // *ConnectModelViews()
    }

    public override bool DetachEventHandlers()
    {
        if (this.userAddin.eventgesteuert == true)
        {
            if (this.userAddin.Event_AfterRegen == true)
            {
                this.IDrawing.RegenPostNotify -= this.DrawingDoc_RegenPostNotify;
            }
            if (this.userAddin.Event_AfterRegen == true)
            {
                this.IDrawing.FileSaveNotify -= this.DrawingDocEvents_FileSaveNotify;
                this.IDrawing.FileSaveAsNotify2 -= this.DrawingDocEvents_FileSaveNotify;
            }
        }
        this.userAddin.DetachModelEventHandler(this.iDocument);
        return default;
    }

    public int DrawingDoc_DestroyNotify()
    {
        this.DetachEventHandlers();
        return default;
    }

    public int DrawingDoc_RegenPostNotify()
    {
        var Fittable = new PassungstabellenHandler();

        Fittable.Main(this.iSwApp, (ModelDoc2)this.IDrawing);
        return 0;
    }

    public int DrawingDocEvents_FileSaveNotify(string FileName)
    {
        var Fittable = new PassungstabellenHandler();

        Fittable.Main(this.iSwApp, (ModelDoc2)this.IDrawing);

        return 0;
    }
}

// Class for handling ModelView events
public class DocView
{

    private ModelView _IModelView;

    private ModelView IModelView
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _IModelView;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            _IModelView = value;
        }
    }
    private readonly NaheFitTable userAddin;
    private readonly DocumentEventHandler parentDoc;

    public DocView(NaheFitTable addin, ModelView mView, DocumentEventHandler parent)
    {
        this.userAddin = addin;
        this.IModelView = mView;
        this.parentDoc = parent;
    }

    public bool AttachEventHandlers()
    {
        return default;
        // AddHandler iModelView.DestroyNotify2, AddressOf Me.ModelView_DestroyNotify2
        // AddHandler iModelView.RepaintNotify, AddressOf Me.ModelView_RepaintNotify
    }

    public bool DetachEventHandlers()
    {
        return default;
        // RemoveHandler iModelView.DestroyNotify2, AddressOf Me.ModelView_DestroyNotify2
        // RemoveHandler iModelView.RepaintNotify, AddressOf Me.ModelView_RepaintNotify

        // parentDoc.DetachModelViewEventHandler(IModelView)
    }

    public int ModelView_DestroyNotify2(int destroyTYpe)
    {
        this.DetachEventHandlers();
        return default;
    }
}