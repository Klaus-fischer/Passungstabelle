namespace Passungstabelle.CSharp;

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

public class LogFile
{
    public string LogPfad = "";
    public Dictionary<string, dynamic> Attr_generell = new();
    public Dictionary<string, Dictionary<string, bool>> Attr_Meldungen = new();
    public string UserName = "";
    public bool IOAccessToLogPath = false;

    public LogFile(Dictionary<string, dynamic> attr)
    {
        LogPfad = GetLogPath();
        Attr_generell = attr;
        UserName = Environment.UserName;
    }

    public LogFile()
    {
        LogPfad = GetLogPath();
        UserName = Environment.UserName;
    }

    public void WriteInfo(string Info, string Info2, bool Msg)
    {
        bool tempattr;
        bool MsgShow = false;

        if (string.IsNullOrEmpty(LogPfad)) return;

        try
        {
            tempattr = Convert.ToBoolean(Attr_generell["LogDatei"]);
        }
        catch
        {
            tempattr = true;
        }

        if (tempattr)
        {
            string logFilePath = Path.Combine(LogPfad, Definitionen.LOGName);
            using StreamWriter writer = new(logFilePath, true);
            string entry = $"{DateTime.Now}\t";

            if (Info == "Start" || Info == "Fertig")
                entry += $"***{Info}{Info2}*** User: {UserName}";
            else
                entry += $"{Info}{Info2}";

            writer.WriteLine(entry);
        }

        try
        {
            tempattr = Convert.ToBoolean(Attr_generell["Fehlermeldung"]);
        }
        catch
        {
            tempattr = false;
        }

        if (!tempattr && Msg)
        {
            
            MessageBox.Show(Info + Info2, "Meldung", MessageBoxButton.OK);
        }
        else if (Msg)
        {
            try
            {
                if (GetMsgValue(Info))
                    MessageBox.Show(Info + Info2, "Meldung", MessageBoxButton.OK);
            }
            catch { }
        }
    }

    public bool GetMsgValue(string info)
    {
        foreach (var n in Attr_Meldungen)
        {
            var value = n.Value;
            if (value.ContainsKey(info))
                return value[info];
        }
        return false;
    }

    public string GetLogPath()
    {
        string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        if (CheckLogIOAccess(path))
        {
            return Path.Combine(path, Application.CompanyName, Application.ProductName);
        }
        else
        {
            return "";
        }
    }


    private bool CheckLogIOAccess(string pfad)
    {
        if (!CreateDir(Path.Combine(pfad, Application.CompanyName))) return false;
        if (!CreateDir(Path.Combine(pfad, Application.CompanyName, Application.ProductName))) return false;
        return CreateFile(Path.Combine(pfad, Application.CompanyName, Application.ProductName));
    }

    private bool CreateDir(string pfad)
    {
        try
        {
            if (!Directory.Exists(pfad))
                Directory.CreateDirectory(pfad);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private bool CreateFile(string pfad)
    {
        string filePath = Path.Combine(pfad, "temp.txt");
        try
        {
            if (File.Exists(filePath))
                File.Delete(filePath);

            File.WriteAllText(filePath, "hello");
            File.Delete(filePath);
            return true;
        }
        catch
        {
            return false;
        }
    }

    internal void WriteInfo(object hat_den_Wert_0, object value, bool v)
    {
        throw new NotImplementedException();
    }
}