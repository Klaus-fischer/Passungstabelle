<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class SetupDialog
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SetupDialog))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.OK_Button = New System.Windows.Forms.Button()
        Me.Cancel_Button = New System.Windows.Forms.Button()
        Me.FontDialog1 = New System.Windows.Forms.FontDialog()
        Me.TabPage4 = New System.Windows.Forms.TabPage()
        Me.DataGridÜbersetzungen = New System.Windows.Forms.DataGridView()
        Me.KürzelDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.SpracheBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.Data = New Passungstabellen.Data()
        Me.Maß = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.PassungDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.MaßePassung = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ToleranzDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.AbmaßDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.VorbearbeitungsAbmaßeDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.VorbearbeitungsToleranzMitteDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Anzahl = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Zone = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ÜbersetzungenIdDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ÜbersetzungBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.AlleFormateGleichBT = New System.Windows.Forms.Button()
        Me.Label32 = New System.Windows.Forms.Label()
        Me.HeaderLanguage = New System.Windows.Forms.ComboBox()
        Me.TabellenAttributeBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.SprachkombinationBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.GroupBox12 = New System.Windows.Forms.GroupBox()
        Me.Label33 = New System.Windows.Forms.Label()
        Me.KursivKopfZeile = New System.Windows.Forms.CheckBox()
        Me.FettKopfZeile = New System.Windows.Forms.CheckBox()
        Me.FarbeKopfZeile = New System.Windows.Forms.TextBox()
        Me.Label28 = New System.Windows.Forms.Label()
        Me.DurchgestrichenKopfZeile = New System.Windows.Forms.CheckBox()
        Me.UnterstrichenKopfZeile = New System.Windows.Forms.CheckBox()
        Me.TexthöheKopfZeile = New System.Windows.Forms.TextBox()
        Me.Label29 = New System.Windows.Forms.Label()
        Me.SchriftstilKopfZeile = New System.Windows.Forms.TextBox()
        Me.Label30 = New System.Windows.Forms.Label()
        Me.SchriftartKopfZeile = New System.Windows.Forms.TextBox()
        Me.Label31 = New System.Windows.Forms.Label()
        Me.Zeilenparameter = New System.Windows.Forms.GroupBox()
        Me.Label34 = New System.Windows.Forms.Label()
        Me.KursivZeile = New System.Windows.Forms.CheckBox()
        Me.FettZeile = New System.Windows.Forms.CheckBox()
        Me.FarbeZeile = New System.Windows.Forms.TextBox()
        Me.Label27 = New System.Windows.Forms.Label()
        Me.DurchgestrichenZeile = New System.Windows.Forms.CheckBox()
        Me.UnterstrichenZeile = New System.Windows.Forms.CheckBox()
        Me.TexthöheZeile = New System.Windows.Forms.TextBox()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.SchriftstilZeile = New System.Windows.Forms.TextBox()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.SchriftartZeile = New System.Windows.Forms.TextBox()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.HeaderGRP = New System.Windows.Forms.GroupBox()
        Me.HeaderUnten = New System.Windows.Forms.RadioButton()
        Me.HeaderOben = New System.Windows.Forms.RadioButton()
        Me.GroupBox8 = New System.Windows.Forms.GroupBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.RasterStrichStärke = New System.Windows.Forms.ComboBox()
        Me.RasterLinien = New System.Windows.Forms.BindingSource(Me.components)
        Me.Label14 = New System.Windows.Forms.Label()
        Me.RahmenStrichStärke = New System.Windows.Forms.ComboBox()
        Me.RahmenLinien = New System.Windows.Forms.BindingSource(Me.components)
        Me.TabellenZeilenBT = New System.Windows.Forms.Button()
        Me.TabellenKopfZeileBT = New System.Windows.Forms.Button()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Offset_Y = New System.Windows.Forms.TextBox()
        Me.FormatAttributeBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.Offset_X = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.EinfügepunktGRP = New System.Windows.Forms.GroupBox()
        Me.Einfügepunkt_RU = New System.Windows.Forms.RadioButton()
        Me.Einfügepunkt_LU = New System.Windows.Forms.RadioButton()
        Me.Einfügepunkt_RO = New System.Windows.Forms.RadioButton()
        Me.Einfügepunkt_LO = New System.Windows.Forms.RadioButton()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.AbmessungenVomBlatt = New System.Windows.Forms.Button()
        Me.Höhe = New System.Windows.Forms.TextBox()
        Me.Breite = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ListBoxFormate = New System.Windows.Forms.ListBox()
        Me.FormatBindingSource1 = New System.Windows.Forms.BindingSource(Me.components)
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.RB_BeforeSave = New System.Windows.Forms.RadioButton()
        Me.GenerelleAttributeBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.RB_AfterRegen = New System.Windows.Forms.RadioButton()
        Me.CB_EventDriven = New System.Windows.Forms.CheckBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Schichtstärke = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.GroupBox7 = New System.Windows.Forms.GroupBox()
        Me.NurAufErstemBlatt = New System.Windows.Forms.CheckBox()
        Me.AnsichtsTypBaugruppen = New System.Windows.Forms.CheckBox()
        Me.AnsichtsTypTeile = New System.Windows.Forms.CheckBox()
        Me.AnsichtsTypSkizzen = New System.Windows.Forms.CheckBox()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.ReaktionAufLeerePassung = New System.Windows.Forms.CheckBox()
        Me.LöschenAufRestlichenBlättern = New System.Windows.Forms.CheckBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.RundenAuf = New System.Windows.Forms.TextBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.LogDatei = New System.Windows.Forms.CheckBox()
        Me.Fehlermeldung = New System.Windows.Forms.CheckBox()
        Me.NeuPositionieren = New System.Windows.Forms.CheckBox()
        Me.PlusZeichen = New System.Windows.Forms.CheckBox()
        Me.SchichtStärkeAbfragenGrp = New System.Windows.Forms.GroupBox()
        Me.SchichtStärkeFix = New System.Windows.Forms.RadioButton()
        Me.RadioButton7 = New System.Windows.Forms.RadioButton()
        Me.SchichtStärkeKeine = New System.Windows.Forms.RadioButton()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage5 = New System.Windows.Forms.TabPage()
        Me.SP10_TB2 = New System.Windows.Forms.TextBox()
        Me.SP10_TB1 = New System.Windows.Forms.TextBox()
        Me.CB_Spalte10 = New System.Windows.Forms.CheckBox()
        Me.SP9_TB2 = New System.Windows.Forms.TextBox()
        Me.SP9_TB1 = New System.Windows.Forms.TextBox()
        Me.CB_Spalte9 = New System.Windows.Forms.CheckBox()
        Me.GroupBox10 = New System.Windows.Forms.GroupBox()
        Me.BreiteSpalte10 = New System.Windows.Forms.TextBox()
        Me.Label36 = New System.Windows.Forms.Label()
        Me.BreiteSpalte9 = New System.Windows.Forms.TextBox()
        Me.Label35 = New System.Windows.Forms.Label()
        Me.BreiteSpalte8 = New System.Windows.Forms.TextBox()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.BreiteSpalte7 = New System.Windows.Forms.TextBox()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.BreiteSpalte6 = New System.Windows.Forms.TextBox()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.BreiteSpalte5 = New System.Windows.Forms.TextBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.CB_AutomatischeSpaltenBreite = New System.Windows.Forms.CheckBox()
        Me.BreiteSpalte4 = New System.Windows.Forms.TextBox()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.BreiteSpalte3 = New System.Windows.Forms.TextBox()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.BreiteSpalte2 = New System.Windows.Forms.TextBox()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.BreiteSpalte1 = New System.Windows.Forms.TextBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.TextBox23 = New System.Windows.Forms.TextBox()
        Me.SP8_TB2 = New System.Windows.Forms.TextBox()
        Me.SP7_TB4 = New System.Windows.Forms.TextBox()
        Me.SP7_TB3 = New System.Windows.Forms.TextBox()
        Me.SP6_TB2 = New System.Windows.Forms.TextBox()
        Me.SP3_TB2 = New System.Windows.Forms.TextBox()
        Me.SP2_TB2 = New System.Windows.Forms.TextBox()
        Me.SP5_TB4 = New System.Windows.Forms.TextBox()
        Me.SP5_TB3 = New System.Windows.Forms.TextBox()
        Me.SP4_TB4 = New System.Windows.Forms.TextBox()
        Me.SP4_TB3 = New System.Windows.Forms.TextBox()
        Me.SP1_TB2 = New System.Windows.Forms.TextBox()
        Me.SP8_TB1 = New System.Windows.Forms.TextBox()
        Me.CB_Spalte8 = New System.Windows.Forms.CheckBox()
        Me.SP7_TB2 = New System.Windows.Forms.TextBox()
        Me.SP7_TB1 = New System.Windows.Forms.TextBox()
        Me.CB_Spalte7 = New System.Windows.Forms.CheckBox()
        Me.CB_Spalte6 = New System.Windows.Forms.CheckBox()
        Me.CB_Spalte5 = New System.Windows.Forms.CheckBox()
        Me.CB_Spalte4 = New System.Windows.Forms.CheckBox()
        Me.CB_Spalte3 = New System.Windows.Forms.CheckBox()
        Me.CB_Spalte2 = New System.Windows.Forms.CheckBox()
        Me.CB_Spalte1 = New System.Windows.Forms.CheckBox()
        Me.SP6_TB1 = New System.Windows.Forms.TextBox()
        Me.SP3_TB1 = New System.Windows.Forms.TextBox()
        Me.SP2_TB1 = New System.Windows.Forms.TextBox()
        Me.SP5_TB2 = New System.Windows.Forms.TextBox()
        Me.SP5_TB1 = New System.Windows.Forms.TextBox()
        Me.SP4_TB2 = New System.Windows.Forms.TextBox()
        Me.SP4_TB1 = New System.Windows.Forms.TextBox()
        Me.SP1_TB1 = New System.Windows.Forms.TextBox()
        Me.TabPage6 = New System.Windows.Forms.TabPage()
        Me.MeldungenDataGridView = New System.Windows.Forms.DataGridView()
        Me.Meldung_Text = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn4 = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.MeldungenBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.StatLab1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripProgressBar1 = New System.Windows.Forms.ToolStripProgressBar()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.BindingSource1 = New System.Windows.Forms.BindingSource(Me.components)
        Me.TabPage4.SuspendLayout()
        CType(Me.DataGridÜbersetzungen, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SpracheBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Data, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ÜbersetzungBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage3.SuspendLayout()
        CType(Me.TabellenAttributeBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SprachkombinationBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox12.SuspendLayout()
        Me.Zeilenparameter.SuspendLayout()
        Me.HeaderGRP.SuspendLayout()
        Me.GroupBox8.SuspendLayout()
        CType(Me.RasterLinien, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RahmenLinien, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage2.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.FormatAttributeBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.EinfügepunktGRP.SuspendLayout()
        CType(Me.FormatBindingSource1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.GenerelleAttributeBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox7.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        Me.SchichtStärkeAbfragenGrp.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPage5.SuspendLayout()
        Me.GroupBox10.SuspendLayout()
        Me.TabPage6.SuspendLayout()
        CType(Me.MeldungenDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MeldungenBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.StatusStrip1.SuspendLayout()
        CType(Me.BindingSource1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'OK_Button
        '
        resources.ApplyResources(Me.OK_Button, "OK_Button")
        Me.OK_Button.Name = "OK_Button"
        '
        'Cancel_Button
        '
        resources.ApplyResources(Me.Cancel_Button, "Cancel_Button")
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Name = "Cancel_Button"
        '
        'TabPage4
        '
        Me.TabPage4.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage4.Controls.Add(Me.DataGridÜbersetzungen)
        resources.ApplyResources(Me.TabPage4, "TabPage4")
        Me.TabPage4.Name = "TabPage4"
        '
        'DataGridÜbersetzungen
        '
        Me.DataGridÜbersetzungen.AutoGenerateColumns = False
        Me.DataGridÜbersetzungen.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridÜbersetzungen.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.KürzelDataGridViewTextBoxColumn, Me.Maß, Me.PassungDataGridViewTextBoxColumn, Me.MaßePassung, Me.ToleranzDataGridViewTextBoxColumn, Me.AbmaßDataGridViewTextBoxColumn, Me.DataGridViewTextBoxColumn1, Me.VorbearbeitungsAbmaßeDataGridViewTextBoxColumn, Me.VorbearbeitungsToleranzMitteDataGridViewTextBoxColumn, Me.Anzahl, Me.Zone, Me.ÜbersetzungenIdDataGridViewTextBoxColumn})
        Me.DataGridÜbersetzungen.DataSource = Me.ÜbersetzungBindingSource
        resources.ApplyResources(Me.DataGridÜbersetzungen, "DataGridÜbersetzungen")
        Me.DataGridÜbersetzungen.Name = "DataGridÜbersetzungen"
        Me.ToolTip1.SetToolTip(Me.DataGridÜbersetzungen, resources.GetString("DataGridÜbersetzungen.ToolTip"))
        '
        'KürzelDataGridViewTextBoxColumn
        '
        Me.KürzelDataGridViewTextBoxColumn.DataPropertyName = "Kürzel"
        Me.KürzelDataGridViewTextBoxColumn.DataSource = Me.SpracheBindingSource
        Me.KürzelDataGridViewTextBoxColumn.DisplayMember = "Kürzel"
        resources.ApplyResources(Me.KürzelDataGridViewTextBoxColumn, "KürzelDataGridViewTextBoxColumn")
        Me.KürzelDataGridViewTextBoxColumn.Name = "KürzelDataGridViewTextBoxColumn"
        Me.KürzelDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.KürzelDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.KürzelDataGridViewTextBoxColumn.ValueMember = "Kürzel"
        '
        'SpracheBindingSource
        '
        Me.SpracheBindingSource.DataMember = "Sprache"
        Me.SpracheBindingSource.DataSource = Me.Data
        '
        'Data
        '
        Me.Data.DataSetName = "Data"
        Me.Data.Locale = New System.Globalization.CultureInfo("en-US")
        Me.Data.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'Maß
        '
        Me.Maß.DataPropertyName = "Maß"
        resources.ApplyResources(Me.Maß, "Maß")
        Me.Maß.Name = "Maß"
        '
        'PassungDataGridViewTextBoxColumn
        '
        Me.PassungDataGridViewTextBoxColumn.DataPropertyName = "Passung"
        resources.ApplyResources(Me.PassungDataGridViewTextBoxColumn, "PassungDataGridViewTextBoxColumn")
        Me.PassungDataGridViewTextBoxColumn.Name = "PassungDataGridViewTextBoxColumn"
        '
        'MaßePassung
        '
        Me.MaßePassung.DataPropertyName = "MaßePassung"
        resources.ApplyResources(Me.MaßePassung, "MaßePassung")
        Me.MaßePassung.Name = "MaßePassung"
        '
        'ToleranzDataGridViewTextBoxColumn
        '
        Me.ToleranzDataGridViewTextBoxColumn.DataPropertyName = "Toleranz"
        resources.ApplyResources(Me.ToleranzDataGridViewTextBoxColumn, "ToleranzDataGridViewTextBoxColumn")
        Me.ToleranzDataGridViewTextBoxColumn.Name = "ToleranzDataGridViewTextBoxColumn"
        '
        'AbmaßDataGridViewTextBoxColumn
        '
        Me.AbmaßDataGridViewTextBoxColumn.DataPropertyName = "Abmaß"
        resources.ApplyResources(Me.AbmaßDataGridViewTextBoxColumn, "AbmaßDataGridViewTextBoxColumn")
        Me.AbmaßDataGridViewTextBoxColumn.Name = "AbmaßDataGridViewTextBoxColumn"
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.DataPropertyName = "AbmaßToleranzMitte"
        resources.ApplyResources(Me.DataGridViewTextBoxColumn1, "DataGridViewTextBoxColumn1")
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        '
        'VorbearbeitungsAbmaßeDataGridViewTextBoxColumn
        '
        Me.VorbearbeitungsAbmaßeDataGridViewTextBoxColumn.DataPropertyName = "VorbearbeitungsAbmaße"
        resources.ApplyResources(Me.VorbearbeitungsAbmaßeDataGridViewTextBoxColumn, "VorbearbeitungsAbmaßeDataGridViewTextBoxColumn")
        Me.VorbearbeitungsAbmaßeDataGridViewTextBoxColumn.Name = "VorbearbeitungsAbmaßeDataGridViewTextBoxColumn"
        '
        'VorbearbeitungsToleranzMitteDataGridViewTextBoxColumn
        '
        Me.VorbearbeitungsToleranzMitteDataGridViewTextBoxColumn.DataPropertyName = "VorbearbeitungsToleranzMitte"
        resources.ApplyResources(Me.VorbearbeitungsToleranzMitteDataGridViewTextBoxColumn, "VorbearbeitungsToleranzMitteDataGridViewTextBoxColumn")
        Me.VorbearbeitungsToleranzMitteDataGridViewTextBoxColumn.Name = "VorbearbeitungsToleranzMitteDataGridViewTextBoxColumn"
        '
        'Anzahl
        '
        Me.Anzahl.DataPropertyName = "Anzahl"
        resources.ApplyResources(Me.Anzahl, "Anzahl")
        Me.Anzahl.Name = "Anzahl"
        '
        'Zone
        '
        Me.Zone.DataPropertyName = "Zone"
        resources.ApplyResources(Me.Zone, "Zone")
        Me.Zone.Name = "Zone"
        '
        'ÜbersetzungenIdDataGridViewTextBoxColumn
        '
        Me.ÜbersetzungenIdDataGridViewTextBoxColumn.DataPropertyName = "Übersetzungen_Id"
        resources.ApplyResources(Me.ÜbersetzungenIdDataGridViewTextBoxColumn, "ÜbersetzungenIdDataGridViewTextBoxColumn")
        Me.ÜbersetzungenIdDataGridViewTextBoxColumn.Name = "ÜbersetzungenIdDataGridViewTextBoxColumn"
        '
        'ÜbersetzungBindingSource
        '
        Me.ÜbersetzungBindingSource.DataMember = "Übersetzung"
        Me.ÜbersetzungBindingSource.DataSource = Me.Data
        '
        'TabPage3
        '
        Me.TabPage3.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage3.Controls.Add(Me.AlleFormateGleichBT)
        Me.TabPage3.Controls.Add(Me.Label32)
        Me.TabPage3.Controls.Add(Me.HeaderLanguage)
        Me.TabPage3.Controls.Add(Me.GroupBox12)
        Me.TabPage3.Controls.Add(Me.Zeilenparameter)
        Me.TabPage3.Controls.Add(Me.HeaderGRP)
        Me.TabPage3.Controls.Add(Me.GroupBox8)
        Me.TabPage3.Controls.Add(Me.TabellenZeilenBT)
        Me.TabPage3.Controls.Add(Me.TabellenKopfZeileBT)
        resources.ApplyResources(Me.TabPage3, "TabPage3")
        Me.TabPage3.Name = "TabPage3"
        '
        'AlleFormateGleichBT
        '
        resources.ApplyResources(Me.AlleFormateGleichBT, "AlleFormateGleichBT")
        Me.AlleFormateGleichBT.Name = "AlleFormateGleichBT"
        Me.ToolTip1.SetToolTip(Me.AlleFormateGleichBT, resources.GetString("AlleFormateGleichBT.ToolTip"))
        Me.AlleFormateGleichBT.UseVisualStyleBackColor = True
        '
        'Label32
        '
        resources.ApplyResources(Me.Label32, "Label32")
        Me.Label32.Name = "Label32"
        '
        'HeaderLanguage
        '
        Me.HeaderLanguage.DataBindings.Add(New System.Windows.Forms.Binding("SelectedValue", Me.TabellenAttributeBindingSource, "HeaderLanguage", True))
        Me.HeaderLanguage.DataSource = Me.SprachkombinationBindingSource
        Me.HeaderLanguage.DisplayMember = "Name"
        Me.HeaderLanguage.FormattingEnabled = True
        resources.ApplyResources(Me.HeaderLanguage, "HeaderLanguage")
        Me.HeaderLanguage.Name = "HeaderLanguage"
        Me.ToolTip1.SetToolTip(Me.HeaderLanguage, resources.GetString("HeaderLanguage.ToolTip"))
        Me.HeaderLanguage.ValueMember = "Name"
        '
        'TabellenAttributeBindingSource
        '
        Me.TabellenAttributeBindingSource.DataMember = "TabellenAttribute"
        Me.TabellenAttributeBindingSource.DataSource = Me.Data
        '
        'SprachkombinationBindingSource
        '
        Me.SprachkombinationBindingSource.DataMember = "Sprachkombination"
        Me.SprachkombinationBindingSource.DataSource = Me.Data
        '
        'GroupBox12
        '
        Me.GroupBox12.Controls.Add(Me.Label33)
        Me.GroupBox12.Controls.Add(Me.KursivKopfZeile)
        Me.GroupBox12.Controls.Add(Me.FettKopfZeile)
        Me.GroupBox12.Controls.Add(Me.FarbeKopfZeile)
        Me.GroupBox12.Controls.Add(Me.Label28)
        Me.GroupBox12.Controls.Add(Me.DurchgestrichenKopfZeile)
        Me.GroupBox12.Controls.Add(Me.UnterstrichenKopfZeile)
        Me.GroupBox12.Controls.Add(Me.TexthöheKopfZeile)
        Me.GroupBox12.Controls.Add(Me.Label29)
        Me.GroupBox12.Controls.Add(Me.SchriftstilKopfZeile)
        Me.GroupBox12.Controls.Add(Me.Label30)
        Me.GroupBox12.Controls.Add(Me.SchriftartKopfZeile)
        Me.GroupBox12.Controls.Add(Me.Label31)
        Me.GroupBox12.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        resources.ApplyResources(Me.GroupBox12, "GroupBox12")
        Me.GroupBox12.Name = "GroupBox12"
        Me.GroupBox12.TabStop = False
        '
        'Label33
        '
        resources.ApplyResources(Me.Label33, "Label33")
        Me.Label33.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label33.Name = "Label33"
        '
        'KursivKopfZeile
        '
        resources.ApplyResources(Me.KursivKopfZeile, "KursivKopfZeile")
        Me.KursivKopfZeile.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.TabellenAttributeBindingSource, "KursivKopfZeile", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.KursivKopfZeile.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.KursivKopfZeile.Name = "KursivKopfZeile"
        Me.KursivKopfZeile.UseVisualStyleBackColor = True
        '
        'FettKopfZeile
        '
        resources.ApplyResources(Me.FettKopfZeile, "FettKopfZeile")
        Me.FettKopfZeile.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.TabellenAttributeBindingSource, "FettKopfZeile", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.FettKopfZeile.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.FettKopfZeile.Name = "FettKopfZeile"
        Me.FettKopfZeile.UseVisualStyleBackColor = True
        '
        'FarbeKopfZeile
        '
        Me.FarbeKopfZeile.BackColor = System.Drawing.Color.Black
        Me.FarbeKopfZeile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.FarbeKopfZeile.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.TabellenAttributeBindingSource, "FarbeKopfZeile", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        resources.ApplyResources(Me.FarbeKopfZeile, "FarbeKopfZeile")
        Me.FarbeKopfZeile.Name = "FarbeKopfZeile"
        Me.FarbeKopfZeile.ReadOnly = True
        '
        'Label28
        '
        resources.ApplyResources(Me.Label28, "Label28")
        Me.Label28.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label28.Name = "Label28"
        '
        'DurchgestrichenKopfZeile
        '
        resources.ApplyResources(Me.DurchgestrichenKopfZeile, "DurchgestrichenKopfZeile")
        Me.DurchgestrichenKopfZeile.DataBindings.Add(New System.Windows.Forms.Binding("CheckState", Me.TabellenAttributeBindingSource, "DurchgestrichenKopfZeile", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.DurchgestrichenKopfZeile.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.DurchgestrichenKopfZeile.Name = "DurchgestrichenKopfZeile"
        Me.DurchgestrichenKopfZeile.UseVisualStyleBackColor = True
        '
        'UnterstrichenKopfZeile
        '
        resources.ApplyResources(Me.UnterstrichenKopfZeile, "UnterstrichenKopfZeile")
        Me.UnterstrichenKopfZeile.DataBindings.Add(New System.Windows.Forms.Binding("CheckState", Me.TabellenAttributeBindingSource, "UnterstrichenKopfZeile", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.UnterstrichenKopfZeile.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.UnterstrichenKopfZeile.Name = "UnterstrichenKopfZeile"
        Me.UnterstrichenKopfZeile.UseVisualStyleBackColor = True
        '
        'TexthöheKopfZeile
        '
        Me.TexthöheKopfZeile.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.TabellenAttributeBindingSource, "TexthöheKopfZeile", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        resources.ApplyResources(Me.TexthöheKopfZeile, "TexthöheKopfZeile")
        Me.TexthöheKopfZeile.Name = "TexthöheKopfZeile"
        '
        'Label29
        '
        resources.ApplyResources(Me.Label29, "Label29")
        Me.Label29.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label29.Name = "Label29"
        '
        'SchriftstilKopfZeile
        '
        Me.SchriftstilKopfZeile.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.TabellenAttributeBindingSource, "SchriftstilKopfZeile", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        resources.ApplyResources(Me.SchriftstilKopfZeile, "SchriftstilKopfZeile")
        Me.SchriftstilKopfZeile.Name = "SchriftstilKopfZeile"
        '
        'Label30
        '
        resources.ApplyResources(Me.Label30, "Label30")
        Me.Label30.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label30.Name = "Label30"
        '
        'SchriftartKopfZeile
        '
        Me.SchriftartKopfZeile.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.TabellenAttributeBindingSource, "SchriftartKopfZeile", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        resources.ApplyResources(Me.SchriftartKopfZeile, "SchriftartKopfZeile")
        Me.SchriftartKopfZeile.Name = "SchriftartKopfZeile"
        '
        'Label31
        '
        resources.ApplyResources(Me.Label31, "Label31")
        Me.Label31.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label31.Name = "Label31"
        '
        'Zeilenparameter
        '
        Me.Zeilenparameter.Controls.Add(Me.Label34)
        Me.Zeilenparameter.Controls.Add(Me.KursivZeile)
        Me.Zeilenparameter.Controls.Add(Me.FettZeile)
        Me.Zeilenparameter.Controls.Add(Me.FarbeZeile)
        Me.Zeilenparameter.Controls.Add(Me.Label27)
        Me.Zeilenparameter.Controls.Add(Me.DurchgestrichenZeile)
        Me.Zeilenparameter.Controls.Add(Me.UnterstrichenZeile)
        Me.Zeilenparameter.Controls.Add(Me.TexthöheZeile)
        Me.Zeilenparameter.Controls.Add(Me.Label24)
        Me.Zeilenparameter.Controls.Add(Me.SchriftstilZeile)
        Me.Zeilenparameter.Controls.Add(Me.Label25)
        Me.Zeilenparameter.Controls.Add(Me.SchriftartZeile)
        Me.Zeilenparameter.Controls.Add(Me.Label26)
        Me.Zeilenparameter.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        resources.ApplyResources(Me.Zeilenparameter, "Zeilenparameter")
        Me.Zeilenparameter.Name = "Zeilenparameter"
        Me.Zeilenparameter.TabStop = False
        '
        'Label34
        '
        resources.ApplyResources(Me.Label34, "Label34")
        Me.Label34.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label34.Name = "Label34"
        '
        'KursivZeile
        '
        resources.ApplyResources(Me.KursivZeile, "KursivZeile")
        Me.KursivZeile.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.TabellenAttributeBindingSource, "KursivZeile", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.KursivZeile.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.KursivZeile.Name = "KursivZeile"
        Me.KursivZeile.UseVisualStyleBackColor = True
        '
        'FettZeile
        '
        resources.ApplyResources(Me.FettZeile, "FettZeile")
        Me.FettZeile.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.TabellenAttributeBindingSource, "FettZeile", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.FettZeile.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.FettZeile.Name = "FettZeile"
        Me.FettZeile.UseVisualStyleBackColor = True
        '
        'FarbeZeile
        '
        Me.FarbeZeile.BackColor = System.Drawing.Color.Black
        Me.FarbeZeile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.FarbeZeile.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.TabellenAttributeBindingSource, "FarbeZeile", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.FarbeZeile.ForeColor = System.Drawing.Color.Black
        resources.ApplyResources(Me.FarbeZeile, "FarbeZeile")
        Me.FarbeZeile.Name = "FarbeZeile"
        Me.FarbeZeile.ReadOnly = True
        '
        'Label27
        '
        resources.ApplyResources(Me.Label27, "Label27")
        Me.Label27.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label27.Name = "Label27"
        '
        'DurchgestrichenZeile
        '
        resources.ApplyResources(Me.DurchgestrichenZeile, "DurchgestrichenZeile")
        Me.DurchgestrichenZeile.DataBindings.Add(New System.Windows.Forms.Binding("CheckState", Me.TabellenAttributeBindingSource, "DurchgestrichenZeile", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.DurchgestrichenZeile.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.DurchgestrichenZeile.Name = "DurchgestrichenZeile"
        Me.DurchgestrichenZeile.UseVisualStyleBackColor = True
        '
        'UnterstrichenZeile
        '
        resources.ApplyResources(Me.UnterstrichenZeile, "UnterstrichenZeile")
        Me.UnterstrichenZeile.DataBindings.Add(New System.Windows.Forms.Binding("CheckState", Me.TabellenAttributeBindingSource, "UnterstrichenZeile", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.UnterstrichenZeile.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.UnterstrichenZeile.Name = "UnterstrichenZeile"
        Me.UnterstrichenZeile.UseVisualStyleBackColor = True
        '
        'TexthöheZeile
        '
        Me.TexthöheZeile.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.TabellenAttributeBindingSource, "TexthöheZeile", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        resources.ApplyResources(Me.TexthöheZeile, "TexthöheZeile")
        Me.TexthöheZeile.Name = "TexthöheZeile"
        '
        'Label24
        '
        resources.ApplyResources(Me.Label24, "Label24")
        Me.Label24.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label24.Name = "Label24"
        '
        'SchriftstilZeile
        '
        Me.SchriftstilZeile.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.TabellenAttributeBindingSource, "SchriftstilZeile", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        resources.ApplyResources(Me.SchriftstilZeile, "SchriftstilZeile")
        Me.SchriftstilZeile.Name = "SchriftstilZeile"
        '
        'Label25
        '
        resources.ApplyResources(Me.Label25, "Label25")
        Me.Label25.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label25.Name = "Label25"
        '
        'SchriftartZeile
        '
        Me.SchriftartZeile.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.TabellenAttributeBindingSource, "SchriftartZeile", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        resources.ApplyResources(Me.SchriftartZeile, "SchriftartZeile")
        Me.SchriftartZeile.Name = "SchriftartZeile"
        '
        'Label26
        '
        resources.ApplyResources(Me.Label26, "Label26")
        Me.Label26.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label26.Name = "Label26"
        '
        'HeaderGRP
        '
        Me.HeaderGRP.Controls.Add(Me.HeaderUnten)
        Me.HeaderGRP.Controls.Add(Me.HeaderOben)
        resources.ApplyResources(Me.HeaderGRP, "HeaderGRP")
        Me.HeaderGRP.Name = "HeaderGRP"
        Me.HeaderGRP.TabStop = False
        '
        'HeaderUnten
        '
        resources.ApplyResources(Me.HeaderUnten, "HeaderUnten")
        Me.HeaderUnten.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.TabellenAttributeBindingSource, "HeaderUnten", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.HeaderUnten.Name = "HeaderUnten"
        Me.HeaderUnten.TabStop = True
        Me.ToolTip1.SetToolTip(Me.HeaderUnten, resources.GetString("HeaderUnten.ToolTip"))
        Me.HeaderUnten.UseVisualStyleBackColor = True
        '
        'HeaderOben
        '
        resources.ApplyResources(Me.HeaderOben, "HeaderOben")
        Me.HeaderOben.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.TabellenAttributeBindingSource, "HeaderOben", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.HeaderOben.Name = "HeaderOben"
        Me.HeaderOben.TabStop = True
        Me.ToolTip1.SetToolTip(Me.HeaderOben, resources.GetString("HeaderOben.ToolTip"))
        Me.HeaderOben.UseVisualStyleBackColor = True
        '
        'GroupBox8
        '
        Me.GroupBox8.Controls.Add(Me.Label15)
        Me.GroupBox8.Controls.Add(Me.RasterStrichStärke)
        Me.GroupBox8.Controls.Add(Me.Label14)
        Me.GroupBox8.Controls.Add(Me.RahmenStrichStärke)
        resources.ApplyResources(Me.GroupBox8, "GroupBox8")
        Me.GroupBox8.Name = "GroupBox8"
        Me.GroupBox8.TabStop = False
        '
        'Label15
        '
        resources.ApplyResources(Me.Label15, "Label15")
        Me.Label15.Name = "Label15"
        '
        'RasterStrichStärke
        '
        Me.RasterStrichStärke.DataBindings.Add(New System.Windows.Forms.Binding("SelectedValue", Me.TabellenAttributeBindingSource, "RasterStrichStärke", True))
        Me.RasterStrichStärke.DataSource = Me.RasterLinien
        Me.RasterStrichStärke.DisplayMember = "Name"
        Me.RasterStrichStärke.FormattingEnabled = True
        resources.ApplyResources(Me.RasterStrichStärke, "RasterStrichStärke")
        Me.RasterStrichStärke.Name = "RasterStrichStärke"
        Me.ToolTip1.SetToolTip(Me.RasterStrichStärke, resources.GetString("RasterStrichStärke.ToolTip"))
        Me.RasterStrichStärke.ValueMember = "Name"
        '
        'RasterLinien
        '
        Me.RasterLinien.DataMember = "Linienart"
        Me.RasterLinien.DataSource = Me.Data
        '
        'Label14
        '
        resources.ApplyResources(Me.Label14, "Label14")
        Me.Label14.Name = "Label14"
        '
        'RahmenStrichStärke
        '
        Me.RahmenStrichStärke.DataBindings.Add(New System.Windows.Forms.Binding("SelectedValue", Me.TabellenAttributeBindingSource, "RahmenStrichStärke", True))
        Me.RahmenStrichStärke.DataSource = Me.RahmenLinien
        Me.RahmenStrichStärke.DisplayMember = "Name"
        Me.RahmenStrichStärke.FormattingEnabled = True
        resources.ApplyResources(Me.RahmenStrichStärke, "RahmenStrichStärke")
        Me.RahmenStrichStärke.Name = "RahmenStrichStärke"
        Me.ToolTip1.SetToolTip(Me.RahmenStrichStärke, resources.GetString("RahmenStrichStärke.ToolTip"))
        Me.RahmenStrichStärke.ValueMember = "Name"
        '
        'RahmenLinien
        '
        Me.RahmenLinien.DataMember = "Linienart"
        Me.RahmenLinien.DataSource = Me.Data
        '
        'TabellenZeilenBT
        '
        resources.ApplyResources(Me.TabellenZeilenBT, "TabellenZeilenBT")
        Me.TabellenZeilenBT.Name = "TabellenZeilenBT"
        Me.ToolTip1.SetToolTip(Me.TabellenZeilenBT, resources.GetString("TabellenZeilenBT.ToolTip"))
        Me.TabellenZeilenBT.UseVisualStyleBackColor = True
        '
        'TabellenKopfZeileBT
        '
        resources.ApplyResources(Me.TabellenKopfZeileBT, "TabellenKopfZeileBT")
        Me.TabellenKopfZeileBT.Name = "TabellenKopfZeileBT"
        Me.ToolTip1.SetToolTip(Me.TabellenKopfZeileBT, resources.GetString("TabellenKopfZeileBT.ToolTip"))
        Me.TabellenKopfZeileBT.UseVisualStyleBackColor = True
        '
        'TabPage2
        '
        Me.TabPage2.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage2.Controls.Add(Me.GroupBox2)
        Me.TabPage2.Controls.Add(Me.EinfügepunktGRP)
        Me.TabPage2.Controls.Add(Me.Button3)
        Me.TabPage2.Controls.Add(Me.Button2)
        Me.TabPage2.Controls.Add(Me.AbmessungenVomBlatt)
        Me.TabPage2.Controls.Add(Me.Höhe)
        Me.TabPage2.Controls.Add(Me.Breite)
        Me.TabPage2.Controls.Add(Me.Label5)
        Me.TabPage2.Controls.Add(Me.Label4)
        Me.TabPage2.Controls.Add(Me.Label3)
        Me.TabPage2.Controls.Add(Me.Label2)
        Me.TabPage2.Controls.Add(Me.ListBoxFormate)
        Me.TabPage2.Controls.Add(Me.Label1)
        resources.ApplyResources(Me.TabPage2, "TabPage2")
        Me.TabPage2.Name = "TabPage2"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Offset_Y)
        Me.GroupBox2.Controls.Add(Me.Offset_X)
        Me.GroupBox2.Controls.Add(Me.Label6)
        Me.GroupBox2.Controls.Add(Me.Label7)
        Me.GroupBox2.Controls.Add(Me.Label8)
        Me.GroupBox2.Controls.Add(Me.Label9)
        resources.ApplyResources(Me.GroupBox2, "GroupBox2")
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.TabStop = False
        '
        'Offset_Y
        '
        Me.Offset_Y.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.FormatAttributeBindingSource, "Offset_Y", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        resources.ApplyResources(Me.Offset_Y, "Offset_Y")
        Me.Offset_Y.Name = "Offset_Y"
        Me.ToolTip1.SetToolTip(Me.Offset_Y, resources.GetString("Offset_Y.ToolTip"))
        '
        'FormatAttributeBindingSource
        '
        Me.FormatAttributeBindingSource.DataMember = "FormatAttribute"
        Me.FormatAttributeBindingSource.DataSource = Me.Data
        '
        'Offset_X
        '
        Me.Offset_X.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.FormatAttributeBindingSource, "Offset_X", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        resources.ApplyResources(Me.Offset_X, "Offset_X")
        Me.Offset_X.Name = "Offset_X"
        Me.ToolTip1.SetToolTip(Me.Offset_X, resources.GetString("Offset_X.ToolTip"))
        '
        'Label6
        '
        resources.ApplyResources(Me.Label6, "Label6")
        Me.Label6.Name = "Label6"
        '
        'Label7
        '
        resources.ApplyResources(Me.Label7, "Label7")
        Me.Label7.Name = "Label7"
        '
        'Label8
        '
        resources.ApplyResources(Me.Label8, "Label8")
        Me.Label8.Name = "Label8"
        '
        'Label9
        '
        resources.ApplyResources(Me.Label9, "Label9")
        Me.Label9.Name = "Label9"
        '
        'EinfügepunktGRP
        '
        Me.EinfügepunktGRP.Controls.Add(Me.Einfügepunkt_RU)
        Me.EinfügepunktGRP.Controls.Add(Me.Einfügepunkt_LU)
        Me.EinfügepunktGRP.Controls.Add(Me.Einfügepunkt_RO)
        Me.EinfügepunktGRP.Controls.Add(Me.Einfügepunkt_LO)
        resources.ApplyResources(Me.EinfügepunktGRP, "EinfügepunktGRP")
        Me.EinfügepunktGRP.Name = "EinfügepunktGRP"
        Me.EinfügepunktGRP.TabStop = False
        '
        'Einfügepunkt_RU
        '
        resources.ApplyResources(Me.Einfügepunkt_RU, "Einfügepunkt_RU")
        Me.Einfügepunkt_RU.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.FormatAttributeBindingSource, "EinfügepunktRU", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.Einfügepunkt_RU.Name = "Einfügepunkt_RU"
        Me.Einfügepunkt_RU.TabStop = True
        Me.ToolTip1.SetToolTip(Me.Einfügepunkt_RU, resources.GetString("Einfügepunkt_RU.ToolTip"))
        Me.Einfügepunkt_RU.UseVisualStyleBackColor = True
        '
        'Einfügepunkt_LU
        '
        resources.ApplyResources(Me.Einfügepunkt_LU, "Einfügepunkt_LU")
        Me.Einfügepunkt_LU.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.FormatAttributeBindingSource, "EinfügepunktLU", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.Einfügepunkt_LU.Name = "Einfügepunkt_LU"
        Me.Einfügepunkt_LU.TabStop = True
        Me.ToolTip1.SetToolTip(Me.Einfügepunkt_LU, resources.GetString("Einfügepunkt_LU.ToolTip"))
        Me.Einfügepunkt_LU.UseVisualStyleBackColor = True
        '
        'Einfügepunkt_RO
        '
        resources.ApplyResources(Me.Einfügepunkt_RO, "Einfügepunkt_RO")
        Me.Einfügepunkt_RO.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.FormatAttributeBindingSource, "EinfügepunktRO", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.Einfügepunkt_RO.Name = "Einfügepunkt_RO"
        Me.Einfügepunkt_RO.TabStop = True
        Me.ToolTip1.SetToolTip(Me.Einfügepunkt_RO, resources.GetString("Einfügepunkt_RO.ToolTip"))
        Me.Einfügepunkt_RO.UseVisualStyleBackColor = True
        '
        'Einfügepunkt_LO
        '
        resources.ApplyResources(Me.Einfügepunkt_LO, "Einfügepunkt_LO")
        Me.Einfügepunkt_LO.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.FormatAttributeBindingSource, "EinfügepunktLO", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.Einfügepunkt_LO.Name = "Einfügepunkt_LO"
        Me.Einfügepunkt_LO.TabStop = True
        Me.ToolTip1.SetToolTip(Me.Einfügepunkt_LO, resources.GetString("Einfügepunkt_LO.ToolTip"))
        Me.Einfügepunkt_LO.UseVisualStyleBackColor = True
        '
        'Button3
        '
        resources.ApplyResources(Me.Button3, "Button3")
        Me.Button3.Name = "Button3"
        Me.ToolTip1.SetToolTip(Me.Button3, resources.GetString("Button3.ToolTip"))
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Button2
        '
        resources.ApplyResources(Me.Button2, "Button2")
        Me.Button2.Name = "Button2"
        Me.ToolTip1.SetToolTip(Me.Button2, resources.GetString("Button2.ToolTip"))
        Me.Button2.UseVisualStyleBackColor = True
        '
        'AbmessungenVomBlatt
        '
        resources.ApplyResources(Me.AbmessungenVomBlatt, "AbmessungenVomBlatt")
        Me.AbmessungenVomBlatt.Name = "AbmessungenVomBlatt"
        Me.ToolTip1.SetToolTip(Me.AbmessungenVomBlatt, resources.GetString("AbmessungenVomBlatt.ToolTip"))
        Me.AbmessungenVomBlatt.UseVisualStyleBackColor = True
        '
        'Höhe
        '
        Me.Höhe.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.FormatAttributeBindingSource, "Höhe", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        resources.ApplyResources(Me.Höhe, "Höhe")
        Me.Höhe.Name = "Höhe"
        Me.ToolTip1.SetToolTip(Me.Höhe, resources.GetString("Höhe.ToolTip"))
        '
        'Breite
        '
        Me.Breite.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.FormatAttributeBindingSource, "Breite", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        resources.ApplyResources(Me.Breite, "Breite")
        Me.Breite.Name = "Breite"
        Me.ToolTip1.SetToolTip(Me.Breite, resources.GetString("Breite.ToolTip"))
        '
        'Label5
        '
        resources.ApplyResources(Me.Label5, "Label5")
        Me.Label5.Name = "Label5"
        '
        'Label4
        '
        resources.ApplyResources(Me.Label4, "Label4")
        Me.Label4.Name = "Label4"
        '
        'Label3
        '
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.Name = "Label3"
        '
        'Label2
        '
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.Name = "Label2"
        '
        'ListBoxFormate
        '
        Me.ListBoxFormate.DataSource = Me.FormatBindingSource1
        Me.ListBoxFormate.DisplayMember = "Formatname"
        Me.ListBoxFormate.FormattingEnabled = True
        resources.ApplyResources(Me.ListBoxFormate, "ListBoxFormate")
        Me.ListBoxFormate.Name = "ListBoxFormate"
        Me.ToolTip1.SetToolTip(Me.ListBoxFormate, resources.GetString("ListBoxFormate.ToolTip"))
        Me.ListBoxFormate.ValueMember = "Formatname"
        '
        'FormatBindingSource1
        '
        Me.FormatBindingSource1.DataMember = "Format"
        Me.FormatBindingSource1.DataSource = Me.Data
        '
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Name = "Label1"
        '
        'TabPage1
        '
        Me.TabPage1.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage1.Controls.Add(Me.GroupBox1)
        Me.TabPage1.Controls.Add(Me.Label12)
        Me.TabPage1.Controls.Add(Me.Schichtstärke)
        Me.TabPage1.Controls.Add(Me.Label11)
        Me.TabPage1.Controls.Add(Me.GroupBox7)
        Me.TabPage1.Controls.Add(Me.GroupBox5)
        Me.TabPage1.Controls.Add(Me.SchichtStärkeAbfragenGrp)
        resources.ApplyResources(Me.TabPage1, "TabPage1")
        Me.TabPage1.Name = "TabPage1"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.RB_BeforeSave)
        Me.GroupBox1.Controls.Add(Me.RB_AfterRegen)
        Me.GroupBox1.Controls.Add(Me.CB_EventDriven)
        resources.ApplyResources(Me.GroupBox1, "GroupBox1")
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.TabStop = False
        '
        'RB_BeforeSave
        '
        Me.RB_BeforeSave.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.GenerelleAttributeBindingSource, "Event_BevorSave", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        resources.ApplyResources(Me.RB_BeforeSave, "RB_BeforeSave")
        Me.RB_BeforeSave.Name = "RB_BeforeSave"
        Me.RB_BeforeSave.TabStop = True
        Me.RB_BeforeSave.UseVisualStyleBackColor = True
        '
        'GenerelleAttributeBindingSource
        '
        Me.GenerelleAttributeBindingSource.DataMember = "GenerelleAttribute"
        Me.GenerelleAttributeBindingSource.DataSource = Me.Data
        '
        'RB_AfterRegen
        '
        resources.ApplyResources(Me.RB_AfterRegen, "RB_AfterRegen")
        Me.RB_AfterRegen.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.GenerelleAttributeBindingSource, "Event_AfterRegen", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.RB_AfterRegen.Name = "RB_AfterRegen"
        Me.RB_AfterRegen.TabStop = True
        Me.RB_AfterRegen.UseVisualStyleBackColor = True
        '
        'CB_EventDriven
        '
        Me.CB_EventDriven.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.GenerelleAttributeBindingSource, "Eventgesteuert", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        resources.ApplyResources(Me.CB_EventDriven, "CB_EventDriven")
        Me.CB_EventDriven.Name = "CB_EventDriven"
        Me.ToolTip1.SetToolTip(Me.CB_EventDriven, resources.GetString("CB_EventDriven.ToolTip"))
        Me.CB_EventDriven.UseVisualStyleBackColor = True
        '
        'Label12
        '
        resources.ApplyResources(Me.Label12, "Label12")
        Me.Label12.Name = "Label12"
        '
        'Schichtstärke
        '
        Me.Schichtstärke.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.GenerelleAttributeBindingSource, "SchichtStärke", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        resources.ApplyResources(Me.Schichtstärke, "Schichtstärke")
        Me.Schichtstärke.Name = "Schichtstärke"
        Me.ToolTip1.SetToolTip(Me.Schichtstärke, resources.GetString("Schichtstärke.ToolTip"))
        '
        'Label11
        '
        resources.ApplyResources(Me.Label11, "Label11")
        Me.Label11.Name = "Label11"
        '
        'GroupBox7
        '
        Me.GroupBox7.Controls.Add(Me.NurAufErstemBlatt)
        Me.GroupBox7.Controls.Add(Me.AnsichtsTypBaugruppen)
        Me.GroupBox7.Controls.Add(Me.AnsichtsTypTeile)
        Me.GroupBox7.Controls.Add(Me.AnsichtsTypSkizzen)
        resources.ApplyResources(Me.GroupBox7, "GroupBox7")
        Me.GroupBox7.Name = "GroupBox7"
        Me.GroupBox7.TabStop = False
        '
        'NurAufErstemBlatt
        '
        resources.ApplyResources(Me.NurAufErstemBlatt, "NurAufErstemBlatt")
        Me.NurAufErstemBlatt.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.GenerelleAttributeBindingSource, "NurAufErstemBlatt", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.NurAufErstemBlatt.Name = "NurAufErstemBlatt"
        Me.ToolTip1.SetToolTip(Me.NurAufErstemBlatt, resources.GetString("NurAufErstemBlatt.ToolTip"))
        Me.NurAufErstemBlatt.UseVisualStyleBackColor = True
        '
        'AnsichtsTypBaugruppen
        '
        resources.ApplyResources(Me.AnsichtsTypBaugruppen, "AnsichtsTypBaugruppen")
        Me.AnsichtsTypBaugruppen.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.GenerelleAttributeBindingSource, "AnsichtsTypBaugruppen", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.AnsichtsTypBaugruppen.Name = "AnsichtsTypBaugruppen"
        Me.ToolTip1.SetToolTip(Me.AnsichtsTypBaugruppen, resources.GetString("AnsichtsTypBaugruppen.ToolTip"))
        Me.AnsichtsTypBaugruppen.UseVisualStyleBackColor = True
        '
        'AnsichtsTypTeile
        '
        resources.ApplyResources(Me.AnsichtsTypTeile, "AnsichtsTypTeile")
        Me.AnsichtsTypTeile.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.GenerelleAttributeBindingSource, "AnsichtsTypTeile", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.AnsichtsTypTeile.Name = "AnsichtsTypTeile"
        Me.ToolTip1.SetToolTip(Me.AnsichtsTypTeile, resources.GetString("AnsichtsTypTeile.ToolTip"))
        Me.AnsichtsTypTeile.UseVisualStyleBackColor = True
        '
        'AnsichtsTypSkizzen
        '
        resources.ApplyResources(Me.AnsichtsTypSkizzen, "AnsichtsTypSkizzen")
        Me.AnsichtsTypSkizzen.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.GenerelleAttributeBindingSource, "AnsichtsTypSkizzen", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.AnsichtsTypSkizzen.Name = "AnsichtsTypSkizzen"
        Me.ToolTip1.SetToolTip(Me.AnsichtsTypSkizzen, resources.GetString("AnsichtsTypSkizzen.ToolTip"))
        Me.AnsichtsTypSkizzen.UseVisualStyleBackColor = True
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.ReaktionAufLeerePassung)
        Me.GroupBox5.Controls.Add(Me.LöschenAufRestlichenBlättern)
        Me.GroupBox5.Controls.Add(Me.Label10)
        Me.GroupBox5.Controls.Add(Me.RundenAuf)
        Me.GroupBox5.Controls.Add(Me.Label13)
        Me.GroupBox5.Controls.Add(Me.LogDatei)
        Me.GroupBox5.Controls.Add(Me.Fehlermeldung)
        Me.GroupBox5.Controls.Add(Me.NeuPositionieren)
        Me.GroupBox5.Controls.Add(Me.PlusZeichen)
        resources.ApplyResources(Me.GroupBox5, "GroupBox5")
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.TabStop = False
        '
        'ReaktionAufLeerePassung
        '
        resources.ApplyResources(Me.ReaktionAufLeerePassung, "ReaktionAufLeerePassung")
        Me.ReaktionAufLeerePassung.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.GenerelleAttributeBindingSource, "ReaktionAufLeerePassung", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.ReaktionAufLeerePassung.Name = "ReaktionAufLeerePassung"
        Me.ToolTip1.SetToolTip(Me.ReaktionAufLeerePassung, resources.GetString("ReaktionAufLeerePassung.ToolTip"))
        Me.ReaktionAufLeerePassung.UseVisualStyleBackColor = True
        '
        'LöschenAufRestlichenBlättern
        '
        resources.ApplyResources(Me.LöschenAufRestlichenBlättern, "LöschenAufRestlichenBlättern")
        Me.LöschenAufRestlichenBlättern.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.GenerelleAttributeBindingSource, "LöschenAufRestlichenBlättern", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.LöschenAufRestlichenBlättern.Name = "LöschenAufRestlichenBlättern"
        Me.ToolTip1.SetToolTip(Me.LöschenAufRestlichenBlättern, resources.GetString("LöschenAufRestlichenBlättern.ToolTip"))
        Me.LöschenAufRestlichenBlättern.UseVisualStyleBackColor = True
        '
        'Label10
        '
        resources.ApplyResources(Me.Label10, "Label10")
        Me.Label10.Name = "Label10"
        '
        'RundenAuf
        '
        Me.RundenAuf.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.GenerelleAttributeBindingSource, "RundenAuf", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        resources.ApplyResources(Me.RundenAuf, "RundenAuf")
        Me.RundenAuf.Name = "RundenAuf"
        Me.ToolTip1.SetToolTip(Me.RundenAuf, resources.GetString("RundenAuf.ToolTip"))
        '
        'Label13
        '
        resources.ApplyResources(Me.Label13, "Label13")
        Me.Label13.Name = "Label13"
        '
        'LogDatei
        '
        resources.ApplyResources(Me.LogDatei, "LogDatei")
        Me.LogDatei.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.GenerelleAttributeBindingSource, "LogDatei", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.LogDatei.Name = "LogDatei"
        Me.ToolTip1.SetToolTip(Me.LogDatei, resources.GetString("LogDatei.ToolTip"))
        Me.LogDatei.UseVisualStyleBackColor = True
        '
        'Fehlermeldung
        '
        resources.ApplyResources(Me.Fehlermeldung, "Fehlermeldung")
        Me.Fehlermeldung.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.GenerelleAttributeBindingSource, "Fehlermeldung", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.Fehlermeldung.Name = "Fehlermeldung"
        Me.ToolTip1.SetToolTip(Me.Fehlermeldung, resources.GetString("Fehlermeldung.ToolTip"))
        Me.Fehlermeldung.UseVisualStyleBackColor = True
        '
        'NeuPositionieren
        '
        resources.ApplyResources(Me.NeuPositionieren, "NeuPositionieren")
        Me.NeuPositionieren.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.GenerelleAttributeBindingSource, "NeuPositionieren", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.NeuPositionieren.Name = "NeuPositionieren"
        Me.ToolTip1.SetToolTip(Me.NeuPositionieren, resources.GetString("NeuPositionieren.ToolTip"))
        Me.NeuPositionieren.UseVisualStyleBackColor = True
        '
        'PlusZeichen
        '
        resources.ApplyResources(Me.PlusZeichen, "PlusZeichen")
        Me.PlusZeichen.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.GenerelleAttributeBindingSource, "PlusZeichen", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.PlusZeichen.Name = "PlusZeichen"
        Me.ToolTip1.SetToolTip(Me.PlusZeichen, resources.GetString("PlusZeichen.ToolTip"))
        Me.PlusZeichen.UseVisualStyleBackColor = True
        '
        'SchichtStärkeAbfragenGrp
        '
        Me.SchichtStärkeAbfragenGrp.Controls.Add(Me.SchichtStärkeFix)
        Me.SchichtStärkeAbfragenGrp.Controls.Add(Me.RadioButton7)
        Me.SchichtStärkeAbfragenGrp.Controls.Add(Me.SchichtStärkeKeine)
        resources.ApplyResources(Me.SchichtStärkeAbfragenGrp, "SchichtStärkeAbfragenGrp")
        Me.SchichtStärkeAbfragenGrp.Name = "SchichtStärkeAbfragenGrp"
        Me.SchichtStärkeAbfragenGrp.TabStop = False
        '
        'SchichtStärkeFix
        '
        resources.ApplyResources(Me.SchichtStärkeFix, "SchichtStärkeFix")
        Me.SchichtStärkeFix.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.GenerelleAttributeBindingSource, "SchichtStärkeFix", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.SchichtStärkeFix.Name = "SchichtStärkeFix"
        Me.ToolTip1.SetToolTip(Me.SchichtStärkeFix, resources.GetString("SchichtStärkeFix.ToolTip"))
        Me.SchichtStärkeFix.UseVisualStyleBackColor = True
        '
        'RadioButton7
        '
        resources.ApplyResources(Me.RadioButton7, "RadioButton7")
        Me.RadioButton7.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.GenerelleAttributeBindingSource, "SchichtStärkeAbfragen", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.RadioButton7.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.RadioButton7.Name = "RadioButton7"
        Me.ToolTip1.SetToolTip(Me.RadioButton7, resources.GetString("RadioButton7.ToolTip"))
        Me.RadioButton7.UseVisualStyleBackColor = True
        '
        'SchichtStärkeKeine
        '
        resources.ApplyResources(Me.SchichtStärkeKeine, "SchichtStärkeKeine")
        Me.SchichtStärkeKeine.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.GenerelleAttributeBindingSource, "SchichtStärkeKeine", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.SchichtStärkeKeine.Name = "SchichtStärkeKeine"
        Me.ToolTip1.SetToolTip(Me.SchichtStärkeKeine, resources.GetString("SchichtStärkeKeine.ToolTip"))
        Me.SchichtStärkeKeine.UseVisualStyleBackColor = True
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Controls.Add(Me.TabPage3)
        Me.TabControl1.Controls.Add(Me.TabPage5)
        Me.TabControl1.Controls.Add(Me.TabPage4)
        Me.TabControl1.Controls.Add(Me.TabPage6)
        resources.ApplyResources(Me.TabControl1, "TabControl1")
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        '
        'TabPage5
        '
        Me.TabPage5.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage5.Controls.Add(Me.SP10_TB2)
        Me.TabPage5.Controls.Add(Me.SP10_TB1)
        Me.TabPage5.Controls.Add(Me.CB_Spalte10)
        Me.TabPage5.Controls.Add(Me.SP9_TB2)
        Me.TabPage5.Controls.Add(Me.SP9_TB1)
        Me.TabPage5.Controls.Add(Me.CB_Spalte9)
        Me.TabPage5.Controls.Add(Me.GroupBox10)
        Me.TabPage5.Controls.Add(Me.TextBox23)
        Me.TabPage5.Controls.Add(Me.SP8_TB2)
        Me.TabPage5.Controls.Add(Me.SP7_TB4)
        Me.TabPage5.Controls.Add(Me.SP7_TB3)
        Me.TabPage5.Controls.Add(Me.SP6_TB2)
        Me.TabPage5.Controls.Add(Me.SP3_TB2)
        Me.TabPage5.Controls.Add(Me.SP2_TB2)
        Me.TabPage5.Controls.Add(Me.SP5_TB4)
        Me.TabPage5.Controls.Add(Me.SP5_TB3)
        Me.TabPage5.Controls.Add(Me.SP4_TB4)
        Me.TabPage5.Controls.Add(Me.SP4_TB3)
        Me.TabPage5.Controls.Add(Me.SP1_TB2)
        Me.TabPage5.Controls.Add(Me.SP8_TB1)
        Me.TabPage5.Controls.Add(Me.CB_Spalte8)
        Me.TabPage5.Controls.Add(Me.SP7_TB2)
        Me.TabPage5.Controls.Add(Me.SP7_TB1)
        Me.TabPage5.Controls.Add(Me.CB_Spalte7)
        Me.TabPage5.Controls.Add(Me.CB_Spalte6)
        Me.TabPage5.Controls.Add(Me.CB_Spalte5)
        Me.TabPage5.Controls.Add(Me.CB_Spalte4)
        Me.TabPage5.Controls.Add(Me.CB_Spalte3)
        Me.TabPage5.Controls.Add(Me.CB_Spalte2)
        Me.TabPage5.Controls.Add(Me.CB_Spalte1)
        Me.TabPage5.Controls.Add(Me.SP6_TB1)
        Me.TabPage5.Controls.Add(Me.SP3_TB1)
        Me.TabPage5.Controls.Add(Me.SP2_TB1)
        Me.TabPage5.Controls.Add(Me.SP5_TB2)
        Me.TabPage5.Controls.Add(Me.SP5_TB1)
        Me.TabPage5.Controls.Add(Me.SP4_TB2)
        Me.TabPage5.Controls.Add(Me.SP4_TB1)
        Me.TabPage5.Controls.Add(Me.SP1_TB1)
        resources.ApplyResources(Me.TabPage5, "TabPage5")
        Me.TabPage5.Name = "TabPage5"
        '
        'SP10_TB2
        '
        Me.SP10_TB2.BackColor = System.Drawing.SystemColors.ButtonHighlight
        resources.ApplyResources(Me.SP10_TB2, "SP10_TB2")
        Me.SP10_TB2.Name = "SP10_TB2"
        Me.SP10_TB2.ReadOnly = True
        '
        'SP10_TB1
        '
        Me.SP10_TB1.BackColor = System.Drawing.SystemColors.ButtonHighlight
        resources.ApplyResources(Me.SP10_TB1, "SP10_TB1")
        Me.SP10_TB1.Name = "SP10_TB1"
        Me.SP10_TB1.ReadOnly = True
        '
        'CB_Spalte10
        '
        resources.ApplyResources(Me.CB_Spalte10, "CB_Spalte10")
        Me.CB_Spalte10.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.TabellenAttributeBindingSource, "TabSpalteZone", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.CB_Spalte10.Name = "CB_Spalte10"
        Me.ToolTip1.SetToolTip(Me.CB_Spalte10, resources.GetString("CB_Spalte10.ToolTip"))
        Me.CB_Spalte10.UseVisualStyleBackColor = True
        '
        'SP9_TB2
        '
        Me.SP9_TB2.BackColor = System.Drawing.SystemColors.ButtonHighlight
        resources.ApplyResources(Me.SP9_TB2, "SP9_TB2")
        Me.SP9_TB2.Name = "SP9_TB2"
        Me.SP9_TB2.ReadOnly = True
        '
        'SP9_TB1
        '
        Me.SP9_TB1.BackColor = System.Drawing.SystemColors.ButtonHighlight
        resources.ApplyResources(Me.SP9_TB1, "SP9_TB1")
        Me.SP9_TB1.Name = "SP9_TB1"
        Me.SP9_TB1.ReadOnly = True
        '
        'CB_Spalte9
        '
        resources.ApplyResources(Me.CB_Spalte9, "CB_Spalte9")
        Me.CB_Spalte9.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.TabellenAttributeBindingSource, "TabSpalteAnzahl", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.CB_Spalte9.Name = "CB_Spalte9"
        Me.ToolTip1.SetToolTip(Me.CB_Spalte9, resources.GetString("CB_Spalte9.ToolTip"))
        Me.CB_Spalte9.UseVisualStyleBackColor = True
        '
        'GroupBox10
        '
        Me.GroupBox10.Controls.Add(Me.BreiteSpalte10)
        Me.GroupBox10.Controls.Add(Me.Label36)
        Me.GroupBox10.Controls.Add(Me.BreiteSpalte9)
        Me.GroupBox10.Controls.Add(Me.Label35)
        Me.GroupBox10.Controls.Add(Me.BreiteSpalte8)
        Me.GroupBox10.Controls.Add(Me.Label23)
        Me.GroupBox10.Controls.Add(Me.BreiteSpalte7)
        Me.GroupBox10.Controls.Add(Me.Label21)
        Me.GroupBox10.Controls.Add(Me.BreiteSpalte6)
        Me.GroupBox10.Controls.Add(Me.Label19)
        Me.GroupBox10.Controls.Add(Me.BreiteSpalte5)
        Me.GroupBox10.Controls.Add(Me.Label17)
        Me.GroupBox10.Controls.Add(Me.CB_AutomatischeSpaltenBreite)
        Me.GroupBox10.Controls.Add(Me.BreiteSpalte4)
        Me.GroupBox10.Controls.Add(Me.Label22)
        Me.GroupBox10.Controls.Add(Me.BreiteSpalte3)
        Me.GroupBox10.Controls.Add(Me.Label20)
        Me.GroupBox10.Controls.Add(Me.BreiteSpalte2)
        Me.GroupBox10.Controls.Add(Me.Label18)
        Me.GroupBox10.Controls.Add(Me.BreiteSpalte1)
        Me.GroupBox10.Controls.Add(Me.Label16)
        resources.ApplyResources(Me.GroupBox10, "GroupBox10")
        Me.GroupBox10.Name = "GroupBox10"
        Me.GroupBox10.TabStop = False
        '
        'BreiteSpalte10
        '
        Me.BreiteSpalte10.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.TabellenAttributeBindingSource, "BreiteSpalteZone", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        resources.ApplyResources(Me.BreiteSpalte10, "BreiteSpalte10")
        Me.BreiteSpalte10.Name = "BreiteSpalte10"
        Me.ToolTip1.SetToolTip(Me.BreiteSpalte10, resources.GetString("BreiteSpalte10.ToolTip"))
        '
        'Label36
        '
        resources.ApplyResources(Me.Label36, "Label36")
        Me.Label36.Name = "Label36"
        '
        'BreiteSpalte9
        '
        Me.BreiteSpalte9.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.TabellenAttributeBindingSource, "BreiteSpalteAnzahl", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        resources.ApplyResources(Me.BreiteSpalte9, "BreiteSpalte9")
        Me.BreiteSpalte9.Name = "BreiteSpalte9"
        Me.ToolTip1.SetToolTip(Me.BreiteSpalte9, resources.GetString("BreiteSpalte9.ToolTip"))
        '
        'Label35
        '
        resources.ApplyResources(Me.Label35, "Label35")
        Me.Label35.Name = "Label35"
        '
        'BreiteSpalte8
        '
        Me.BreiteSpalte8.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.TabellenAttributeBindingSource, "BreiteSpalteVorbearbeitungsToleranzMitte", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        resources.ApplyResources(Me.BreiteSpalte8, "BreiteSpalte8")
        Me.BreiteSpalte8.Name = "BreiteSpalte8"
        Me.ToolTip1.SetToolTip(Me.BreiteSpalte8, resources.GetString("BreiteSpalte8.ToolTip"))
        '
        'Label23
        '
        resources.ApplyResources(Me.Label23, "Label23")
        Me.Label23.Name = "Label23"
        '
        'BreiteSpalte7
        '
        Me.BreiteSpalte7.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.TabellenAttributeBindingSource, "BreiteSpalteVorbearbeitungsAbmaße", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        resources.ApplyResources(Me.BreiteSpalte7, "BreiteSpalte7")
        Me.BreiteSpalte7.Name = "BreiteSpalte7"
        Me.ToolTip1.SetToolTip(Me.BreiteSpalte7, resources.GetString("BreiteSpalte7.ToolTip"))
        '
        'Label21
        '
        resources.ApplyResources(Me.Label21, "Label21")
        Me.Label21.Name = "Label21"
        '
        'BreiteSpalte6
        '
        Me.BreiteSpalte6.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.TabellenAttributeBindingSource, "BreiteSpalteAbmaßToleranzMitte", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        resources.ApplyResources(Me.BreiteSpalte6, "BreiteSpalte6")
        Me.BreiteSpalte6.Name = "BreiteSpalte6"
        Me.ToolTip1.SetToolTip(Me.BreiteSpalte6, resources.GetString("BreiteSpalte6.ToolTip"))
        '
        'Label19
        '
        resources.ApplyResources(Me.Label19, "Label19")
        Me.Label19.Name = "Label19"
        '
        'BreiteSpalte5
        '
        Me.BreiteSpalte5.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.TabellenAttributeBindingSource, "BreiteSpalteAbmaß", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        resources.ApplyResources(Me.BreiteSpalte5, "BreiteSpalte5")
        Me.BreiteSpalte5.Name = "BreiteSpalte5"
        Me.ToolTip1.SetToolTip(Me.BreiteSpalte5, resources.GetString("BreiteSpalte5.ToolTip"))
        '
        'Label17
        '
        resources.ApplyResources(Me.Label17, "Label17")
        Me.Label17.Name = "Label17"
        '
        'CB_AutomatischeSpaltenBreite
        '
        resources.ApplyResources(Me.CB_AutomatischeSpaltenBreite, "CB_AutomatischeSpaltenBreite")
        Me.CB_AutomatischeSpaltenBreite.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.TabellenAttributeBindingSource, "SpaltenBreiteAutomatisch", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.CB_AutomatischeSpaltenBreite.Name = "CB_AutomatischeSpaltenBreite"
        Me.CB_AutomatischeSpaltenBreite.UseVisualStyleBackColor = True
        '
        'BreiteSpalte4
        '
        Me.BreiteSpalte4.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.TabellenAttributeBindingSource, "BreiteSpalteToleranz", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        resources.ApplyResources(Me.BreiteSpalte4, "BreiteSpalte4")
        Me.BreiteSpalte4.Name = "BreiteSpalte4"
        Me.ToolTip1.SetToolTip(Me.BreiteSpalte4, resources.GetString("BreiteSpalte4.ToolTip"))
        '
        'Label22
        '
        resources.ApplyResources(Me.Label22, "Label22")
        Me.Label22.Name = "Label22"
        '
        'BreiteSpalte3
        '
        Me.BreiteSpalte3.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.TabellenAttributeBindingSource, "BreiteSpalteMaßePassung", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        resources.ApplyResources(Me.BreiteSpalte3, "BreiteSpalte3")
        Me.BreiteSpalte3.Name = "BreiteSpalte3"
        Me.ToolTip1.SetToolTip(Me.BreiteSpalte3, resources.GetString("BreiteSpalte3.ToolTip"))
        '
        'Label20
        '
        resources.ApplyResources(Me.Label20, "Label20")
        Me.Label20.Name = "Label20"
        '
        'BreiteSpalte2
        '
        Me.BreiteSpalte2.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.TabellenAttributeBindingSource, "BreiteSpaltePassung", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        resources.ApplyResources(Me.BreiteSpalte2, "BreiteSpalte2")
        Me.BreiteSpalte2.Name = "BreiteSpalte2"
        Me.ToolTip1.SetToolTip(Me.BreiteSpalte2, resources.GetString("BreiteSpalte2.ToolTip"))
        '
        'Label18
        '
        resources.ApplyResources(Me.Label18, "Label18")
        Me.Label18.Name = "Label18"
        '
        'BreiteSpalte1
        '
        Me.BreiteSpalte1.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.TabellenAttributeBindingSource, "BreiteSpalteMaß", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        resources.ApplyResources(Me.BreiteSpalte1, "BreiteSpalte1")
        Me.BreiteSpalte1.Name = "BreiteSpalte1"
        Me.ToolTip1.SetToolTip(Me.BreiteSpalte1, resources.GetString("BreiteSpalte1.ToolTip"))
        '
        'Label16
        '
        resources.ApplyResources(Me.Label16, "Label16")
        Me.Label16.Name = "Label16"
        '
        'TextBox23
        '
        Me.TextBox23.BackColor = System.Drawing.SystemColors.MenuBar
        Me.TextBox23.BorderStyle = System.Windows.Forms.BorderStyle.None
        resources.ApplyResources(Me.TextBox23, "TextBox23")
        Me.TextBox23.Name = "TextBox23"
        '
        'SP8_TB2
        '
        Me.SP8_TB2.BackColor = System.Drawing.SystemColors.ButtonHighlight
        resources.ApplyResources(Me.SP8_TB2, "SP8_TB2")
        Me.SP8_TB2.Name = "SP8_TB2"
        Me.SP8_TB2.ReadOnly = True
        '
        'SP7_TB4
        '
        Me.SP7_TB4.BackColor = System.Drawing.SystemColors.ButtonHighlight
        resources.ApplyResources(Me.SP7_TB4, "SP7_TB4")
        Me.SP7_TB4.Name = "SP7_TB4"
        Me.SP7_TB4.ReadOnly = True
        '
        'SP7_TB3
        '
        Me.SP7_TB3.BackColor = System.Drawing.SystemColors.ButtonHighlight
        resources.ApplyResources(Me.SP7_TB3, "SP7_TB3")
        Me.SP7_TB3.Name = "SP7_TB3"
        Me.SP7_TB3.ReadOnly = True
        '
        'SP6_TB2
        '
        Me.SP6_TB2.BackColor = System.Drawing.SystemColors.ButtonHighlight
        resources.ApplyResources(Me.SP6_TB2, "SP6_TB2")
        Me.SP6_TB2.Name = "SP6_TB2"
        Me.SP6_TB2.ReadOnly = True
        '
        'SP3_TB2
        '
        Me.SP3_TB2.BackColor = System.Drawing.SystemColors.ButtonHighlight
        resources.ApplyResources(Me.SP3_TB2, "SP3_TB2")
        Me.SP3_TB2.Name = "SP3_TB2"
        Me.SP3_TB2.ReadOnly = True
        '
        'SP2_TB2
        '
        Me.SP2_TB2.BackColor = System.Drawing.SystemColors.ButtonHighlight
        resources.ApplyResources(Me.SP2_TB2, "SP2_TB2")
        Me.SP2_TB2.Name = "SP2_TB2"
        Me.SP2_TB2.ReadOnly = True
        '
        'SP5_TB4
        '
        Me.SP5_TB4.BackColor = System.Drawing.SystemColors.ButtonHighlight
        resources.ApplyResources(Me.SP5_TB4, "SP5_TB4")
        Me.SP5_TB4.Name = "SP5_TB4"
        Me.SP5_TB4.ReadOnly = True
        '
        'SP5_TB3
        '
        Me.SP5_TB3.BackColor = System.Drawing.SystemColors.ButtonHighlight
        resources.ApplyResources(Me.SP5_TB3, "SP5_TB3")
        Me.SP5_TB3.Name = "SP5_TB3"
        Me.SP5_TB3.ReadOnly = True
        '
        'SP4_TB4
        '
        Me.SP4_TB4.BackColor = System.Drawing.SystemColors.ButtonHighlight
        resources.ApplyResources(Me.SP4_TB4, "SP4_TB4")
        Me.SP4_TB4.Name = "SP4_TB4"
        Me.SP4_TB4.ReadOnly = True
        '
        'SP4_TB3
        '
        Me.SP4_TB3.BackColor = System.Drawing.SystemColors.ButtonHighlight
        resources.ApplyResources(Me.SP4_TB3, "SP4_TB3")
        Me.SP4_TB3.Name = "SP4_TB3"
        Me.SP4_TB3.ReadOnly = True
        '
        'SP1_TB2
        '
        Me.SP1_TB2.BackColor = System.Drawing.SystemColors.ButtonHighlight
        resources.ApplyResources(Me.SP1_TB2, "SP1_TB2")
        Me.SP1_TB2.Name = "SP1_TB2"
        Me.SP1_TB2.ReadOnly = True
        '
        'SP8_TB1
        '
        Me.SP8_TB1.BackColor = System.Drawing.SystemColors.ButtonHighlight
        resources.ApplyResources(Me.SP8_TB1, "SP8_TB1")
        Me.SP8_TB1.Name = "SP8_TB1"
        Me.SP8_TB1.ReadOnly = True
        '
        'CB_Spalte8
        '
        resources.ApplyResources(Me.CB_Spalte8, "CB_Spalte8")
        Me.CB_Spalte8.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.TabellenAttributeBindingSource, "TabSpalteVorbearbeitungsToleranzMitte", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.CB_Spalte8.Name = "CB_Spalte8"
        Me.ToolTip1.SetToolTip(Me.CB_Spalte8, resources.GetString("CB_Spalte8.ToolTip"))
        Me.CB_Spalte8.UseVisualStyleBackColor = True
        '
        'SP7_TB2
        '
        Me.SP7_TB2.BackColor = System.Drawing.SystemColors.ButtonHighlight
        resources.ApplyResources(Me.SP7_TB2, "SP7_TB2")
        Me.SP7_TB2.Name = "SP7_TB2"
        Me.SP7_TB2.ReadOnly = True
        '
        'SP7_TB1
        '
        Me.SP7_TB1.BackColor = System.Drawing.SystemColors.ButtonHighlight
        resources.ApplyResources(Me.SP7_TB1, "SP7_TB1")
        Me.SP7_TB1.Name = "SP7_TB1"
        Me.SP7_TB1.ReadOnly = True
        '
        'CB_Spalte7
        '
        resources.ApplyResources(Me.CB_Spalte7, "CB_Spalte7")
        Me.CB_Spalte7.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.TabellenAttributeBindingSource, "TabSpalteVorbearbeitungsAbmaße", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.CB_Spalte7.Name = "CB_Spalte7"
        Me.ToolTip1.SetToolTip(Me.CB_Spalte7, resources.GetString("CB_Spalte7.ToolTip"))
        Me.CB_Spalte7.UseVisualStyleBackColor = True
        '
        'CB_Spalte6
        '
        resources.ApplyResources(Me.CB_Spalte6, "CB_Spalte6")
        Me.CB_Spalte6.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.TabellenAttributeBindingSource, "TabSpalteAbmaßToleranzMitte", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.CB_Spalte6.Name = "CB_Spalte6"
        Me.ToolTip1.SetToolTip(Me.CB_Spalte6, resources.GetString("CB_Spalte6.ToolTip"))
        Me.CB_Spalte6.UseVisualStyleBackColor = True
        '
        'CB_Spalte5
        '
        resources.ApplyResources(Me.CB_Spalte5, "CB_Spalte5")
        Me.CB_Spalte5.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.TabellenAttributeBindingSource, "TabSpalteAbmaß", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.CB_Spalte5.Name = "CB_Spalte5"
        Me.ToolTip1.SetToolTip(Me.CB_Spalte5, resources.GetString("CB_Spalte5.ToolTip"))
        Me.CB_Spalte5.UseVisualStyleBackColor = True
        '
        'CB_Spalte4
        '
        resources.ApplyResources(Me.CB_Spalte4, "CB_Spalte4")
        Me.CB_Spalte4.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.TabellenAttributeBindingSource, "TabSpalteToleranz", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.CB_Spalte4.Name = "CB_Spalte4"
        Me.ToolTip1.SetToolTip(Me.CB_Spalte4, resources.GetString("CB_Spalte4.ToolTip"))
        Me.CB_Spalte4.UseVisualStyleBackColor = True
        '
        'CB_Spalte3
        '
        resources.ApplyResources(Me.CB_Spalte3, "CB_Spalte3")
        Me.CB_Spalte3.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.TabellenAttributeBindingSource, "TabSpalteMaßePassung", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.CB_Spalte3.Name = "CB_Spalte3"
        Me.ToolTip1.SetToolTip(Me.CB_Spalte3, resources.GetString("CB_Spalte3.ToolTip"))
        Me.CB_Spalte3.UseVisualStyleBackColor = True
        '
        'CB_Spalte2
        '
        resources.ApplyResources(Me.CB_Spalte2, "CB_Spalte2")
        Me.CB_Spalte2.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.TabellenAttributeBindingSource, "TabSpaltePassung", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.CB_Spalte2.Name = "CB_Spalte2"
        Me.ToolTip1.SetToolTip(Me.CB_Spalte2, resources.GetString("CB_Spalte2.ToolTip"))
        Me.CB_Spalte2.UseVisualStyleBackColor = True
        '
        'CB_Spalte1
        '
        resources.ApplyResources(Me.CB_Spalte1, "CB_Spalte1")
        Me.CB_Spalte1.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.TabellenAttributeBindingSource, "TabSpalteMaß", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.CB_Spalte1.Name = "CB_Spalte1"
        Me.ToolTip1.SetToolTip(Me.CB_Spalte1, resources.GetString("CB_Spalte1.ToolTip"))
        Me.CB_Spalte1.UseVisualStyleBackColor = True
        '
        'SP6_TB1
        '
        Me.SP6_TB1.BackColor = System.Drawing.SystemColors.ButtonHighlight
        resources.ApplyResources(Me.SP6_TB1, "SP6_TB1")
        Me.SP6_TB1.Name = "SP6_TB1"
        Me.SP6_TB1.ReadOnly = True
        '
        'SP3_TB1
        '
        Me.SP3_TB1.BackColor = System.Drawing.SystemColors.ButtonHighlight
        resources.ApplyResources(Me.SP3_TB1, "SP3_TB1")
        Me.SP3_TB1.Name = "SP3_TB1"
        Me.SP3_TB1.ReadOnly = True
        '
        'SP2_TB1
        '
        Me.SP2_TB1.BackColor = System.Drawing.SystemColors.ButtonHighlight
        resources.ApplyResources(Me.SP2_TB1, "SP2_TB1")
        Me.SP2_TB1.Name = "SP2_TB1"
        Me.SP2_TB1.ReadOnly = True
        '
        'SP5_TB2
        '
        Me.SP5_TB2.BackColor = System.Drawing.SystemColors.ButtonHighlight
        resources.ApplyResources(Me.SP5_TB2, "SP5_TB2")
        Me.SP5_TB2.Name = "SP5_TB2"
        Me.SP5_TB2.ReadOnly = True
        '
        'SP5_TB1
        '
        Me.SP5_TB1.BackColor = System.Drawing.SystemColors.ButtonHighlight
        resources.ApplyResources(Me.SP5_TB1, "SP5_TB1")
        Me.SP5_TB1.Name = "SP5_TB1"
        Me.SP5_TB1.ReadOnly = True
        '
        'SP4_TB2
        '
        Me.SP4_TB2.BackColor = System.Drawing.SystemColors.ButtonHighlight
        resources.ApplyResources(Me.SP4_TB2, "SP4_TB2")
        Me.SP4_TB2.Name = "SP4_TB2"
        Me.SP4_TB2.ReadOnly = True
        '
        'SP4_TB1
        '
        Me.SP4_TB1.BackColor = System.Drawing.SystemColors.ButtonHighlight
        resources.ApplyResources(Me.SP4_TB1, "SP4_TB1")
        Me.SP4_TB1.Name = "SP4_TB1"
        Me.SP4_TB1.ReadOnly = True
        '
        'SP1_TB1
        '
        Me.SP1_TB1.BackColor = System.Drawing.SystemColors.ButtonHighlight
        resources.ApplyResources(Me.SP1_TB1, "SP1_TB1")
        Me.SP1_TB1.Name = "SP1_TB1"
        Me.SP1_TB1.ReadOnly = True
        '
        'TabPage6
        '
        Me.TabPage6.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage6.Controls.Add(Me.MeldungenDataGridView)
        resources.ApplyResources(Me.TabPage6, "TabPage6")
        Me.TabPage6.Name = "TabPage6"
        '
        'MeldungenDataGridView
        '
        Me.MeldungenDataGridView.AllowUserToAddRows = False
        Me.MeldungenDataGridView.AllowUserToDeleteRows = False
        Me.MeldungenDataGridView.AllowUserToOrderColumns = True
        Me.MeldungenDataGridView.AutoGenerateColumns = False
        Me.MeldungenDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.MeldungenDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Meldung_Text, Me.DataGridViewTextBoxColumn4, Me.DataGridViewTextBoxColumn3, Me.DataGridViewTextBoxColumn2})
        Me.MeldungenDataGridView.DataSource = Me.MeldungenBindingSource
        resources.ApplyResources(Me.MeldungenDataGridView, "MeldungenDataGridView")
        Me.MeldungenDataGridView.Name = "MeldungenDataGridView"
        '
        'Meldung_Text
        '
        Me.Meldung_Text.DataPropertyName = "Meldung_Text"
        resources.ApplyResources(Me.Meldung_Text, "Meldung_Text")
        Me.Meldung_Text.Name = "Meldung_Text"
        Me.Meldung_Text.ReadOnly = True
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.DataPropertyName = "Meldung_anzeigen"
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter
        DataGridViewCellStyle1.NullValue = False
        Me.DataGridViewTextBoxColumn4.DefaultCellStyle = DataGridViewCellStyle1
        resources.ApplyResources(Me.DataGridViewTextBoxColumn4, "DataGridViewTextBoxColumn4")
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewTextBoxColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.DataPropertyName = "Meldung"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.DataGridViewTextBoxColumn3.DefaultCellStyle = DataGridViewCellStyle2
        resources.ApplyResources(Me.DataGridViewTextBoxColumn3, "DataGridViewTextBoxColumn3")
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.DataPropertyName = "Meldungen_Id"
        resources.ApplyResources(Me.DataGridViewTextBoxColumn2, "DataGridViewTextBoxColumn2")
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        '
        'MeldungenBindingSource
        '
        Me.MeldungenBindingSource.DataMember = "Meldungen"
        Me.MeldungenBindingSource.DataSource = Me.Data
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StatLab1, Me.ToolStripProgressBar1, Me.ToolStripStatusLabel1})
        resources.ApplyResources(Me.StatusStrip1, "StatusStrip1")
        Me.StatusStrip1.Name = "StatusStrip1"
        '
        'StatLab1
        '
        Me.StatLab1.Name = "StatLab1"
        resources.ApplyResources(Me.StatLab1, "StatLab1")
        '
        'ToolStripProgressBar1
        '
        Me.ToolStripProgressBar1.Name = "ToolStripProgressBar1"
        resources.ApplyResources(Me.ToolStripProgressBar1, "ToolStripProgressBar1")
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        resources.ApplyResources(Me.ToolStripStatusLabel1, "ToolStripStatusLabel1")
        '
        'ToolTip1
        '
        Me.ToolTip1.IsBalloon = True
        Me.ToolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info
        Me.ToolTip1.ToolTipTitle = "Hinweis:"
        '
        'SetupDialog
        '
        Me.AcceptButton = Me.OK_Button
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.Cancel_Button)
        Me.Controls.Add(Me.OK_Button)
        Me.Controls.Add(Me.TabControl1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SetupDialog"
        Me.ShowInTaskbar = False
        Me.ToolTip1.SetToolTip(Me, resources.GetString("$this.ToolTip"))
        Me.TabPage4.ResumeLayout(False)
        CType(Me.DataGridÜbersetzungen, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SpracheBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Data, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ÜbersetzungBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage3.ResumeLayout(False)
        Me.TabPage3.PerformLayout()
        CType(Me.TabellenAttributeBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SprachkombinationBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox12.ResumeLayout(False)
        Me.GroupBox12.PerformLayout()
        Me.Zeilenparameter.ResumeLayout(False)
        Me.Zeilenparameter.PerformLayout()
        Me.HeaderGRP.ResumeLayout(False)
        Me.HeaderGRP.PerformLayout()
        Me.GroupBox8.ResumeLayout(False)
        Me.GroupBox8.PerformLayout()
        CType(Me.RasterLinien, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RahmenLinien, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        CType(Me.FormatAttributeBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.EinfügepunktGRP.ResumeLayout(False)
        Me.EinfügepunktGRP.PerformLayout()
        CType(Me.FormatBindingSource1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.GenerelleAttributeBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox7.ResumeLayout(False)
        Me.GroupBox7.PerformLayout()
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        Me.SchichtStärkeAbfragenGrp.ResumeLayout(False)
        Me.SchichtStärkeAbfragenGrp.PerformLayout()
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage5.ResumeLayout(False)
        Me.TabPage5.PerformLayout()
        Me.GroupBox10.ResumeLayout(False)
        Me.GroupBox10.PerformLayout()
        Me.TabPage6.ResumeLayout(False)
        CType(Me.MeldungenDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MeldungenBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        CType(Me.BindingSource1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents FontDialog1 As Windows.Forms.FontDialog
    Friend WithEvents TabPage4 As Windows.Forms.TabPage
    Friend WithEvents DataGridÜbersetzungen As Windows.Forms.DataGridView
    Friend WithEvents TabPage3 As Windows.Forms.TabPage
    Friend WithEvents HeaderGRP As Windows.Forms.GroupBox
    Friend WithEvents HeaderUnten As Windows.Forms.RadioButton
    Friend WithEvents HeaderOben As Windows.Forms.RadioButton
    Friend WithEvents GroupBox8 As Windows.Forms.GroupBox
    Friend WithEvents Label15 As Windows.Forms.Label
    Friend WithEvents RasterStrichStärke As Windows.Forms.ComboBox
    Friend WithEvents Label14 As Windows.Forms.Label
    Friend WithEvents RahmenStrichStärke As Windows.Forms.ComboBox
    Friend WithEvents TabellenZeilenBT As Windows.Forms.Button
    Friend WithEvents TabellenKopfZeileBT As Windows.Forms.Button
    Friend WithEvents TabPage2 As Windows.Forms.TabPage
    Friend WithEvents GroupBox2 As Windows.Forms.GroupBox
    Friend WithEvents Offset_Y As Windows.Forms.TextBox
    Friend WithEvents Offset_X As Windows.Forms.TextBox
    Friend WithEvents Label6 As Windows.Forms.Label
    Friend WithEvents Label7 As Windows.Forms.Label
    Friend WithEvents Label8 As Windows.Forms.Label
    Friend WithEvents Label9 As Windows.Forms.Label
    Friend WithEvents EinfügepunktGRP As Windows.Forms.GroupBox
    Friend WithEvents Einfügepunkt_RU As Windows.Forms.RadioButton
    Friend WithEvents Einfügepunkt_LU As Windows.Forms.RadioButton
    Friend WithEvents Einfügepunkt_RO As Windows.Forms.RadioButton
    Friend WithEvents Button3 As Windows.Forms.Button
    Friend WithEvents Button2 As Windows.Forms.Button
    Friend WithEvents AbmessungenVomBlatt As Windows.Forms.Button
    Friend WithEvents Höhe As Windows.Forms.TextBox
    Friend WithEvents Breite As Windows.Forms.TextBox
    Friend WithEvents Label5 As Windows.Forms.Label
    Friend WithEvents Label4 As Windows.Forms.Label
    Friend WithEvents Label3 As Windows.Forms.Label
    Friend WithEvents Label2 As Windows.Forms.Label
    Friend WithEvents ListBoxFormate As Windows.Forms.ListBox
    Friend WithEvents Label1 As Windows.Forms.Label
    Friend WithEvents TabPage1 As Windows.Forms.TabPage
    Friend WithEvents GroupBox7 As Windows.Forms.GroupBox
    Friend WithEvents AnsichtsTypBaugruppen As Windows.Forms.CheckBox
    Friend WithEvents AnsichtsTypTeile As Windows.Forms.CheckBox
    Friend WithEvents AnsichtsTypSkizzen As Windows.Forms.CheckBox
    Friend WithEvents GroupBox5 As Windows.Forms.GroupBox
    Friend WithEvents Label10 As Windows.Forms.Label
    Friend WithEvents RundenAuf As Windows.Forms.TextBox
    Friend WithEvents Label13 As Windows.Forms.Label
    Friend WithEvents LogDatei As Windows.Forms.CheckBox
    Friend WithEvents Fehlermeldung As Windows.Forms.CheckBox
    Friend WithEvents NeuPositionieren As Windows.Forms.CheckBox
    Friend WithEvents PlusZeichen As Windows.Forms.CheckBox
    Friend WithEvents SchichtStärkeAbfragenGrp As Windows.Forms.GroupBox
    Friend WithEvents SchichtStärkeFix As Windows.Forms.RadioButton
    Friend WithEvents RadioButton7 As Windows.Forms.RadioButton
    Friend WithEvents SchichtStärkeKeine As Windows.Forms.RadioButton
    Friend WithEvents TabControl1 As Windows.Forms.TabControl
    Friend WithEvents StatusStrip1 As Windows.Forms.StatusStrip
    Friend WithEvents StatLab1 As Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripProgressBar1 As Windows.Forms.ToolStripProgressBar
    Friend WithEvents Einfügepunkt_LO As Windows.Forms.RadioButton
    Friend WithEvents Zeilenparameter As Windows.Forms.GroupBox
    Friend WithEvents TexthöheZeile As Windows.Forms.TextBox
    Friend WithEvents Label24 As Windows.Forms.Label
    Friend WithEvents SchriftstilZeile As Windows.Forms.TextBox
    Friend WithEvents Label25 As Windows.Forms.Label
    Friend WithEvents SchriftartZeile As Windows.Forms.TextBox
    Friend WithEvents Label26 As Windows.Forms.Label
    Friend WithEvents DurchgestrichenZeile As Windows.Forms.CheckBox
    Friend WithEvents UnterstrichenZeile As Windows.Forms.CheckBox
    Friend WithEvents GroupBox12 As Windows.Forms.GroupBox
    Friend WithEvents FarbeKopfZeile As Windows.Forms.TextBox
    Friend WithEvents Label28 As Windows.Forms.Label
    Friend WithEvents DurchgestrichenKopfZeile As Windows.Forms.CheckBox
    Friend WithEvents UnterstrichenKopfZeile As Windows.Forms.CheckBox
    Friend WithEvents TexthöheKopfZeile As Windows.Forms.TextBox
    Friend WithEvents Label29 As Windows.Forms.Label
    Friend WithEvents SchriftstilKopfZeile As Windows.Forms.TextBox
    Friend WithEvents Label30 As Windows.Forms.Label
    Friend WithEvents SchriftartKopfZeile As Windows.Forms.TextBox
    Friend WithEvents Label31 As Windows.Forms.Label
    Friend WithEvents FarbeZeile As Windows.Forms.TextBox
    Friend WithEvents Label27 As Windows.Forms.Label
    Friend WithEvents FettKopfZeile As Windows.Forms.CheckBox
    Friend WithEvents FettZeile As Windows.Forms.CheckBox
    Friend WithEvents LöschenAufRestlichenBlättern As Windows.Forms.CheckBox
    Friend WithEvents ToolTip1 As Windows.Forms.ToolTip
    Friend WithEvents NurAufErstemBlatt As Windows.Forms.CheckBox
    Friend WithEvents ReaktionAufLeerePassung As Windows.Forms.CheckBox
    Friend WithEvents Label12 As Windows.Forms.Label
    Friend WithEvents Schichtstärke As Windows.Forms.TextBox
    Friend WithEvents Label11 As Windows.Forms.Label
    Friend WithEvents BindingSource1 As Windows.Forms.BindingSource
    Friend WithEvents Label32 As Windows.Forms.Label
    Friend WithEvents HeaderLanguage As Windows.Forms.ComboBox
    Friend WithEvents GenerelleAttributeBindingSource As Windows.Forms.BindingSource
    Friend WithEvents FormatBindingSource1 As Windows.Forms.BindingSource
    Friend WithEvents SprachkombinationBindingSource As Windows.Forms.BindingSource
    Friend WithEvents TabellenAttributeBindingSource As Windows.Forms.BindingSource
    Friend WithEvents RasterLinien As Windows.Forms.BindingSource
    Friend WithEvents RahmenLinien As Windows.Forms.BindingSource
    Friend WithEvents SpracheBindingSource As Windows.Forms.BindingSource
    Friend WithEvents ÜbersetzungBindingSource As Windows.Forms.BindingSource
    Friend WithEvents FormatAttributeBindingSource As Windows.Forms.BindingSource
    Friend WithEvents ToolStripStatusLabel1 As Windows.Forms.ToolStripStatusLabel
    Friend WithEvents AlleFormateGleichBT As Windows.Forms.Button
    Friend WithEvents TabPage5 As Windows.Forms.TabPage
    Friend WithEvents TextBox23 As Windows.Forms.TextBox
    Friend WithEvents SP8_TB2 As Windows.Forms.TextBox
    Friend WithEvents SP7_TB4 As Windows.Forms.TextBox
    Friend WithEvents SP7_TB3 As Windows.Forms.TextBox
    Friend WithEvents SP6_TB2 As Windows.Forms.TextBox
    Friend WithEvents SP3_TB2 As Windows.Forms.TextBox
    Friend WithEvents SP2_TB2 As Windows.Forms.TextBox
    Friend WithEvents SP5_TB4 As Windows.Forms.TextBox
    Friend WithEvents SP5_TB3 As Windows.Forms.TextBox
    Friend WithEvents SP4_TB4 As Windows.Forms.TextBox
    Friend WithEvents SP4_TB3 As Windows.Forms.TextBox
    Friend WithEvents SP1_TB2 As Windows.Forms.TextBox
    Friend WithEvents SP8_TB1 As Windows.Forms.TextBox
    Friend WithEvents CB_Spalte8 As Windows.Forms.CheckBox
    Friend WithEvents SP7_TB2 As Windows.Forms.TextBox
    Friend WithEvents SP7_TB1 As Windows.Forms.TextBox
    Friend WithEvents CB_Spalte7 As Windows.Forms.CheckBox
    Friend WithEvents CB_Spalte6 As Windows.Forms.CheckBox
    Friend WithEvents CB_Spalte5 As Windows.Forms.CheckBox
    Friend WithEvents CB_Spalte4 As Windows.Forms.CheckBox
    Friend WithEvents CB_Spalte3 As Windows.Forms.CheckBox
    Friend WithEvents CB_Spalte2 As Windows.Forms.CheckBox
    Friend WithEvents CB_Spalte1 As Windows.Forms.CheckBox
    Friend WithEvents SP6_TB1 As Windows.Forms.TextBox
    Friend WithEvents SP3_TB1 As Windows.Forms.TextBox
    Friend WithEvents SP2_TB1 As Windows.Forms.TextBox
    Friend WithEvents SP5_TB2 As Windows.Forms.TextBox
    Friend WithEvents SP5_TB1 As Windows.Forms.TextBox
    Friend WithEvents SP4_TB2 As Windows.Forms.TextBox
    Friend WithEvents SP4_TB1 As Windows.Forms.TextBox
    Friend WithEvents SP1_TB1 As Windows.Forms.TextBox
    Friend WithEvents CB_EventDriven As Windows.Forms.CheckBox
    Friend WithEvents GroupBox10 As Windows.Forms.GroupBox
    Friend WithEvents BreiteSpalte4 As Windows.Forms.TextBox
    Friend WithEvents Label22 As Windows.Forms.Label
    Friend WithEvents BreiteSpalte3 As Windows.Forms.TextBox
    Friend WithEvents Label20 As Windows.Forms.Label
    Friend WithEvents BreiteSpalte2 As Windows.Forms.TextBox
    Friend WithEvents Label18 As Windows.Forms.Label
    Friend WithEvents BreiteSpalte1 As Windows.Forms.TextBox
    Friend WithEvents Label16 As Windows.Forms.Label
    Friend WithEvents CB_AutomatischeSpaltenBreite As Windows.Forms.CheckBox
    Friend WithEvents Data As Data
    Friend WithEvents AbmaßToleranzmitteDataGridViewTextBoxColumn As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents KursivKopfZeile As Windows.Forms.CheckBox
    Friend WithEvents KursivZeile As Windows.Forms.CheckBox
    Friend WithEvents BreiteSpalte8 As Windows.Forms.TextBox
    Friend WithEvents Label23 As Windows.Forms.Label
    Friend WithEvents BreiteSpalte7 As Windows.Forms.TextBox
    Friend WithEvents Label21 As Windows.Forms.Label
    Friend WithEvents BreiteSpalte6 As Windows.Forms.TextBox
    Friend WithEvents Label19 As Windows.Forms.Label
    Friend WithEvents BreiteSpalte5 As Windows.Forms.TextBox
    Friend WithEvents Label17 As Windows.Forms.Label
    Friend WithEvents Label33 As Windows.Forms.Label
    Friend WithEvents Label34 As Windows.Forms.Label
    Friend WithEvents SP10_TB2 As Windows.Forms.TextBox
    Friend WithEvents SP10_TB1 As Windows.Forms.TextBox
    Friend WithEvents CB_Spalte10 As Windows.Forms.CheckBox
    Friend WithEvents SP9_TB2 As Windows.Forms.TextBox
    Friend WithEvents SP9_TB1 As Windows.Forms.TextBox
    Friend WithEvents CB_Spalte9 As Windows.Forms.CheckBox
    Friend WithEvents BreiteSpalte10 As Windows.Forms.TextBox
    Friend WithEvents Label36 As Windows.Forms.Label
    Friend WithEvents BreiteSpalte9 As Windows.Forms.TextBox
    Friend WithEvents Label35 As Windows.Forms.Label
    Friend WithEvents KürzelDataGridViewTextBoxColumn As Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Maß As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents PassungDataGridViewTextBoxColumn As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents MaßePassung As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ToleranzDataGridViewTextBoxColumn As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents AbmaßDataGridViewTextBoxColumn As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn1 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents VorbearbeitungsAbmaßeDataGridViewTextBoxColumn As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents VorbearbeitungsToleranzMitteDataGridViewTextBoxColumn As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Anzahl As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Zone As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ÜbersetzungenIdDataGridViewTextBoxColumn As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents GroupBox1 As Windows.Forms.GroupBox
    Friend WithEvents RB_BeforeSave As Windows.Forms.RadioButton
    Friend WithEvents RB_AfterRegen As Windows.Forms.RadioButton
    Friend WithEvents TabPage6 As Windows.Forms.TabPage
    Friend WithEvents MeldungenDataGridView As Windows.Forms.DataGridView
    Friend WithEvents MeldungenBindingSource As Windows.Forms.BindingSource
    Friend WithEvents Meldung_Text As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn4 As Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As Windows.Forms.DataGridViewTextBoxColumn
End Class
