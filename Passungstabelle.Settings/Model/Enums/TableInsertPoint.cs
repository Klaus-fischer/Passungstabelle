// <copyright file="FormatSettings" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace Passungstabelle.Settings;

using SolidWorks.Interop.swconst;

public enum TableInsertPoint
{
    TopLeft = swBOMConfigurationAnchorType_e.swBOMConfigurationAnchor_TopLeft,
    TopRight = swBOMConfigurationAnchorType_e.swBOMConfigurationAnchor_TopRight,
    BottomLeft = swBOMConfigurationAnchorType_e.swBOMConfigurationAnchor_BottomLeft,
    BottomRight = swBOMConfigurationAnchorType_e.swBOMConfigurationAnchor_BottomRight,
}
