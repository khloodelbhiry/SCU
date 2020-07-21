using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for LogWriter
/// </summary>
public class LogWriter
{
    public LogWriter(string tableName, string operation,string id, string occurredAt, string performedBy, string fieldName, string oldValue, string newValue, string companyId, string projectId,string description)
    {
        LogWrite(tableName, operation,id, occurredAt, performedBy, fieldName, oldValue, newValue,companyId,projectId,description);
    }
    public static void LogWrite(string tableName, string operation,string id, string occurredAt, string performedBy, string fieldName, string oldValue, string newValue,string companyId,string projectId, string description)
    {
        try
        {
            DirectoryInfo dir = new DirectoryInfo(HttpContext.Current.Server.MapPath("Log"));
            IEnumerable<FileInfo> fileList = dir.GetFiles("log-*.txt");
            FileInfo fileQuery =  (from file in fileList where file.Length <= 58000000
                                               orderby file.LastWriteTime descending
                                               select file).FirstOrDefault();
            string path = string.Empty;
            if (fileQuery == null)
            {
                path = HttpContext.Current.Server.MapPath("Log") + @"\log-" + DateTime.Now.ToFileTime().ToString() + ".txt";
                File.Create(path).Dispose();
                using (StreamWriter w = File.AppendText(path))
                {
                    Log("tableName", "operation", "id", "occurredAt", "performedBy", "fieldName", "oldValue", "newValue","companyId","projectId", "description", w);
                    w.WriteLine("\r\n-----------------------------------------------------------------------------------------------------------------------------------------------------------");
                }
            }
            else
                path = fileQuery.FullName;
            using (StreamWriter w = File.AppendText(path))
            {
                Log(tableName, operation,id, occurredAt, performedBy, fieldName, oldValue, newValue,companyId,projectId,description, w);
            }
        }
        catch (Exception ex)
        {
        }
    }
    public static void Log(string tableName,string operation,string id,string occurredAt,string performedBy,string fieldName,string oldValue,string newValue, string companyId, string projectId, string description, TextWriter txtWriter)
    {
        try
        {
            txtWriter.Write("\r\n{0}#;                  ;#{1}#;                  ;#{2}#;                  ;#{3}#;                  ;#{4}#;                  ;#{5}#;                  ;#{6}#;                  ;#{7}#;                  ;#{8}#;                  ;#{9}#;                  ;#{10}", tableName, operation,id, occurredAt, performedBy, fieldName, oldValue, newValue,companyId,projectId,description);
        }
        catch (Exception ex)
        {
        }
    }
    public LogWriter()
    {
        //
        // TODO: Add constructor logic here
        //
    }
}