// <copyright file="PassungsEntryCollection" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.CSharp;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Collections.Generic;
using System.Linq;

internal class PassungEntityCollection
{
    private const double factor = 1000;

    private readonly Dictionary<string, PassungEntity> passungsEntities = new Dictionary<string, PassungEntity>();

    public void Clear() => passungsEntities.Clear();

    public void AddPassungFromDimension(IDimension dimension, bool isHole, string prefix, params string[] zonen)
    {
        var tolerance = dimension.Tolerance;
        var holeFit = tolerance.GetHoleFitValue();
        var shaftFit = tolerance.GetShaftFitValue();

        var passung = isHole ? holeFit : shaftFit;
        double maß = dimension.GetSystemValue2("") * factor;
        if (string.IsNullOrWhiteSpace(passung) ||
            !PassungsRechner.TryGetAbmaß(maß, passung, out var toleranzU, out var toleranzO))
        {
            return;
        }

        this.AddPassung(prefix, maß, passung, toleranzO, toleranzU, isHole, zonen);
    }

    public void AddPassungFromCallOut(ICalloutVariable[] calloutVariables, string prefix, params string[] zonen)
    {
        foreach (ICalloutVariable swCalloutVariable in calloutVariables)
        {
            if (swCalloutVariable.Type != (int)swCalloutVariableType_e.swCalloutVariableType_Length ||
                swCalloutVariable is not CalloutLengthVariable swCalloutLengthVariable)
            {
                continue;
            }

            // Nur wenn es sich auch um eine Passungsangabe handelt, wird ausgewertet
            if (swCalloutVariable.ToleranceType != (int)swTolType_e.swTolFIT &&
                swCalloutVariable.ToleranceType != (int)swTolType_e.swTolFITTOLONLY &&
                swCalloutVariable.ToleranceType != (int)swTolType_e.swTolFITWITHTOL)
            {
                continue;
            }

            var maß = swCalloutLengthVariable.Length * factor;

            this.AddPassungFromCallOut(swCalloutVariable, maß, true, prefix, zonen);
            this.AddPassungFromCallOut(swCalloutVariable, maß, false, prefix, zonen);
        }
    }

    private void AddPassungFromCallOut(ICalloutVariable swCalloutVariable, double maß, bool isHole, string prefix, string[] zonen)
    {
        var holeFit = swCalloutVariable.HoleFit;
        var shaftFit = swCalloutVariable.ShaftFit;

        var passung = isHole ? holeFit : shaftFit;

        if (string.IsNullOrWhiteSpace(passung) ||
            !PassungsRechner.TryGetAbmaß(maß, passung, out var toleranzU, out var toleranzO))
        {
            return;
        }

        this.AddPassung(prefix, maß, passung, toleranzO, toleranzU, isHole, zonen);
    }

    private void AddPassung(string prefix, double maß, string passung, double toleranzO, double toleranzU, bool isHole, string[] zonen)
    {
        if (string.IsNullOrWhiteSpace(passung))
        {
            return;
        }

        var identifier = $"{prefix}{maß:0.###} {passung}";
        if (!this.passungsEntities.TryGetValue(identifier, out var passungsEntity))
        {
            passungsEntity = new PassungEntity()
            {
                Prefix = prefix,
                Maß = maß,
                Passung = passung,
                ToleranzO = toleranzO,
                ToleranzU = toleranzU,
                PassungsType = isHole ? PassungsType.Hole : PassungsType.Shaft
            };
            this.passungsEntities[identifier] = passungsEntity;
        }

        foreach (var zone in zonen)
        {
            if (passungsEntity.Zone.Contains(zone))
            {
                continue;
            }

            passungsEntity.Zone.Add(zone);
        }
    }

    public TabellenZeile[] BuildTable()
    {
        return passungsEntities
            .Values
            .Where(this.ValidatePassung)
            .Order()
            .Select(o => (TabellenZeile)(o))
            .ToArray();
    }

    private bool ValidatePassung(PassungEntity entity)
    {
        if (entity.Passung != "" && entity.ToleranzO == 0.0 && entity.ToleranzU == 0.0)
        {
            //Log.WriteInfo(My.Resources.LeerePassungsWerte, entity.Maß, entity.Passung);

            if (entity.Passung[0] >= 'A' & entity.Passung[0] <= 'Z' & entity.PassungsType == PassungsType.Shaft)
            {
                //Log.WriteInfo(My.Resources.UngültigeWellenpassung, entity.Passung);
            }
            else if (entity.Passung[0] >= 'a' & entity.Passung[0] <= 'Z' & entity.PassungsType == PassungsType.Hole)
            {
                //Log.WriteInfo(My.Resources.UngültigeBohrungspassung, entity.Passung);
            }

            return false;
        }

        if (entity.Passung[0] >= 'A' & entity.Passung[0] <= 'Z' & entity.PassungsType == PassungsType.Shaft)
        {
            // Log.WriteInfo(My.Resources.UngültigeWellenpassung, entity.Passung);
            return false;
        }

        if (entity.Passung[0] >= 'a' & entity.Passung[0] <= 'z' & entity.PassungsType == PassungsType.Hole)
        {
            // Log.WriteInfo(My.Resources.UngültigeBohrungspassung, entity.Passung);
            return false;
        }

        return true;
    }
}
