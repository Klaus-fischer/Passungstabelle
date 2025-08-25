using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Runtime.CompilerServices;

namespace Passungstabelle.CSharp;

/// <summary>
/// Base class for model event handlers
/// </summary>
public class DocumentEventHandler
{
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
        this.userAddin.EventHandler?.DetachModelEventHandler(this.iDocument);
        return default;
    }

    /// <summary>
    /// Handler to unregister events on closing documents.
    /// </summary>
    /// <param name="destroyType">See <see cref="swDestroyNotifyType_e"/></param>
    /// <returns></returns>
    protected virtual int DestroyNotify(int destroyType)
    {
        if (destroyType == (int)swDestroyNotifyType_e.swDestroyNotifyDestroy)
        {
            this.DetachEventHandlers();
        }

        return default;
    }
}

/// <summary>
/// Class to listen for Part Events
/// </summary>
public class PartEventHandler : DocumentEventHandler
{
    private PartDoc _IPart;

    private PartDoc IPart
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => _IPart;

        [MethodImpl(MethodImplOptions.Synchronized)]
        set => _IPart = value;
    }

    public PartEventHandler(SldWorks sw, NaheFitTable addin, ModelDoc2 model)
        : base(sw, addin, model)
    {
        this.IPart = (PartDoc)model;
    }

    public override bool AttachEventHandlers()
    {
        IPart.DestroyNotify2 += this.DestroyNotify;
        return default;
    }

    public override bool DetachEventHandlers()
    {
        IPart.DestroyNotify2 -= this.DestroyNotify;
        return base.DetachEventHandlers();
    }
}

/// <summary>
/// Class to listen for Assembly Events
/// </summary>
public class AssemblyEventHandler : DocumentEventHandler
{

    private AssemblyDoc iAssembly;

    private AssemblyDoc IAssembly
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => iAssembly;

        [MethodImpl(MethodImplOptions.Synchronized)]
        set => iAssembly = value;
    }

    public AssemblyEventHandler(SldWorks sw, NaheFitTable addin, ModelDoc2 model)
        : base(sw, addin, model)
    {
        this.iAssembly = (AssemblyDoc)model;

    }

    public override bool AttachEventHandlers()
    {
        IAssembly.DestroyNotify2 += this.DestroyNotify;
        return default;
    }

    public override bool DetachEventHandlers()
    {
        IAssembly.DestroyNotify2 -= this.DestroyNotify;
        return base.DetachEventHandlers();
    }
}

/// <summary>
/// Class to listen for Drawing Events
/// </summary>
public class DrawingEventHandler : DocumentEventHandler
{
    private readonly DrawingDoc iDrawing;
    private readonly NaheFitTable userAddin;

    private DrawingDoc IDrawing
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => iDrawing;
    }


    public DrawingEventHandler(SldWorks sw, NaheFitTable userAddin, ModelDoc2 model)
        : base(sw, userAddin, model)
    {
        this.iDrawing = (DrawingDoc)model;
        this.userAddin = userAddin;
    }

    public override bool AttachEventHandlers()
    {
        this.IDrawing.RegenPostNotify += this.DrawingDoc_RegenPostNotify;
        this.IDrawing.FileSaveNotify += this.DrawingDocEvents_FileSaveNotify;
        this.IDrawing.FileSaveAsNotify2 += this.DrawingDocEvents_FileSaveNotify;
        this.IDrawing.DestroyNotify2 += this.DestroyNotify;

        return default;
    }

    public override bool DetachEventHandlers()
    {
        this.IDrawing.RegenPostNotify -= this.DrawingDoc_RegenPostNotify;
        this.IDrawing.FileSaveNotify -= this.DrawingDocEvents_FileSaveNotify;
        this.IDrawing.FileSaveAsNotify2 -= this.DrawingDocEvents_FileSaveNotify;
        this.IDrawing.DestroyNotify2 -= this.DestroyNotify;
        return base.DetachEventHandlers();
    }

    private int DrawingDoc_RegenPostNotify()
    {
        this.userAddin.ExecuteOnRegenPostNotify(this.IDrawing);
        return 0;
    }

    private int DrawingDocEvents_FileSaveNotify(string FileName)
    {
        this.userAddin.ExecuteOnSaveNotify(this.IDrawing);
        return 0;
    }
}