using System.Collections.Generic;
using SolidWorks.Interop.sldworks;

namespace Passungstabelle.CSharp;

/// <summary>
/// Was wir alles von den Ansichten wissen wollen
/// </summary>
public class Ansicht
{
    public string ansichtsName;           // Ansichtsname
    public View ViewRef;                  // Verweis auf Ansicht
    public ModelDoc2 arefernz;            // Referenz auf die die Ansicht verweist (Teil, Baugruppe, ...)
    public int doctype;               // Dockumenttyp der Ansichtsreferenz
    public List<HoleTable> holetab;    // Bohrungstabelle
    public List<Dictionary<string, List<MathPoint>>> HoletableTags { get; set; } // Tags der Ansichten
    public List<Dictionary<string, List<string>>> HoletableZones { get; set; }

    /// <summary>
    /// Intialisierungsfuntkion für die Strukur "Ansicht"
    /// </summary>
    /// <param name="iansichtsName"></param>
    /// <param name="iarefernz"></param>
    /// <param name="idoctype"></param>
    /// <param name="iViewRef"></param>
    public Ansicht(string iansichtsName, ModelDoc2 iarefernz, int idoctype, View iViewRef)
    {
        this.ansichtsName = iansichtsName;
        this.ViewRef = iViewRef;
        this.arefernz = iarefernz;
        this.doctype = idoctype;
        this.holetab = null;
        this.HoletableTags = new();
        this.HoletableZones = new();
    }
}
