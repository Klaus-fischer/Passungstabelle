namespace Passungstabelle.Settings;

public class TemplateSettingsViewModel : BaseViewModel
{
    private string tableSchemaName = "Default";
    private string templateNamePattern = "*";
    private string[] formatNames = [];

    public string TemplateNamePattern
    {
        get => templateNamePattern;
        set => this.Set(ref templateNamePattern, value);
    }

    public string TableSchemaName
    {
        get => tableSchemaName;
        set => this.Set(ref tableSchemaName, value);
    }

    public string[] FormatNames
    {
        get => formatNames;
        set => this.Set(ref formatNames, value);
    }

    public static TemplateSettingsViewModel FromModel(TemplateSettings model)
    {
        return new TemplateSettingsViewModel()
        {
            FormatNames = model.FormatNames,
            TableSchemaName = model.TableSchemaName,
            TemplateNamePattern = model.TemplateNamePattern,
        };
    }

    public TemplateSettings ToModel()
    {
        return new TemplateSettings()
        {
            FormatNames = this.FormatNames,
            TableSchemaName = this.TableSchemaName,
            TemplateNamePattern = this.TemplateNamePattern,
        };
    }
}
