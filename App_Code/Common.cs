using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

public enum StatusEnum
{
    UnderApprrove = 1,
    Approved = 2,
    Freezed = 3
}
public enum AssetsStatusEnum
{
    UnderSupply = 1,
    Asset = 2
}
public enum ModuleFormsEnum
{
    FileIndexing = 1,
    DocsIndexing = 3,
    DocsQa = 4
}
public enum IssueStatusEnum
{
    UnderApprove = 1,
    Open = 2,
    Closed = 3
}
public enum SettingsEnum
{
    ProjectPages = 1,
    BatchManagement = 2,
    PdfPath = 5
}
public enum CodesLengthEnum
{
    Unit = 5,
    File = 6,
    Doc = 3
}
public enum ModulesEnum
{
    Menus = 1,
    Projects = 2,
    Files = 3,
    Issues=4,
    Security=5,
    ControlPanel=6,
    Reports=7,
    Archiving=8
}
public enum StatesEnum
{
    first = 1,
    second = 2,
    BarcodeDocuments = 5,
    CompanyReceivesFiles = 7,
    Scan=11,
    DocumentsIndexing = 13,
    DocumentsQa = 14
}
public enum ActionsEnum
{
    Done = 1,
    Restart = 2
}
public enum ActivitiesEnum
{
    Add = 1,
    Read = 2,
    Update = 3,
    Delete = 4,
    Print = 5,
    Export = 6,
    Index = 8,
    Qa = 9,
    Approve = 10,
    Freze = 11
}
public enum ObjectsEnum
{
    File = 1,
    Document = 2
}
public class Common
{
    public static string CompaniesPath = "companies";
    public static string UnitStructureTypesPath = "unit-structure-types";
    public static string UnitStructurePath = "unit-structure";
    public static string ControlPanelPath = "control-panel";
    public static string GovernmentalEntitiesPath = "governmental-entities";
    public static string GroupsPath = "groups";
    public static string ProjectsPath = "projects";
    public static string PrerequisitesPath = "prerequisites";
    public static string ProjectPrerequisitesPath = "project-prerequisites";
    public static string ProjectDashboardPath = "project-dashboard";
    public static string ProjectFilesPath = "project-files";
    public static string ProjectStructurePath = "project-structure";
    public static string ProjectConsumablesPath = "project-consumables";
    public static string ProjectUsersPath = "project-users";
    public static string ProjectIssuesPath = "project-issues";
    public static string ProjectSitesPath = "project-sites";
    public static string ProjectStocksPath = "project-stocks";
    public static string ProjectAssetsPath = "project-assets";
    public static string ProjectDocTypesPath = "project-doc-types";
    public static string ProjectSalaryEffectsPath = "project-salary-effects";
    public static string ProjectCategoriesPath = "project-categories";
    public static string ProjectPartiesPath = "project-parties";
    public static string ProjectPersonsPath = "project-persons";
    public static string ProjectTargetPath = "project-target";
    public static string ProjectBusinessReferencesPath = "project-references";
    public static string FileDocumentsPath = "file-documents";
    public static string FilesIndexingPath = "files-indexing";
    public static string DocsIndexingPath = "docs-indexing";
    public static string DocsQaPath = "docs-qa";
    public static string CompanyAssetsPath = "company-assets";
    public static string CompanyConsumablesPath = "company-consumables";
    public static string CompanyHrPath = "company-hr";
    public static string CompanyFinancialPath = "company-financial";
    public static string CompanyUsersPath = "company-users";
    public static string DocTypesPath = "doc-types";
    public static string CategoriesPath = "categories";
    public static string UsersPath = "users";
    public static string SearchPath = "search";
    public static string IssuesPath = "issues";
    public Common()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public static string Sub_String(string str, int maxLength)
    {
        if (str.ToString().Length > maxLength)
        {
            return str.ToString().Substring(0, maxLength) + "...";
        }
        else
        {
            return str.ToString();
        }
    }
    public static void InsertException(string message, string stackTrace, string pageName)
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            ExceptionLog exp = new ExceptionLog
            {
                ExceptionLog_Date = DateTime.Now,
                ExceptionLog_Message = message,
                ExceptionLog_StackTrace = stackTrace,
                ExceptionLog_PageName = pageName
            };
            db.ExceptionLogs.InsertOnSubmit(exp);
            db.SubmitChanges();
        }
    }
    public static string GetUniqueFileName(string savePath, string fileName)
    {
        string newFileName = string.Format("{0}{1}", Regex.Replace(RemoveWhitespace(Path.GetFileNameWithoutExtension(fileName)), @"[^0-9a-zA-Z]+", "-"), Path.GetExtension(fileName));
        string pathToCheck = savePath + newFileName;
        string tempfileName = string.Empty;
        if (File.Exists(pathToCheck))
        {
            int counter = 2;
            while (File.Exists(pathToCheck))
            {
                tempfileName = string.Format("{0}-{1}{2}", Path.GetFileNameWithoutExtension(newFileName), counter,
                    Path.GetExtension(newFileName));
                pathToCheck = savePath + tempfileName;
                counter++;
            }
            newFileName = tempfileName;
        }
        return newFileName;
    }
    public static string RemoveWhitespace(string input)
    {
        if (input == null)
            return null;
        return new string(input.ToCharArray()
            .Where(c => !Char.IsWhiteSpace(c))
            .ToArray());
    }
    public static string ToTrimmedString(double target)
    {
        string strValue = target.ToString();
        if (strValue.Contains("."))
        {
            strValue = strValue.TrimEnd('0');
            if (strValue.EndsWith("."))
                strValue = strValue.TrimEnd('.');
        }
        return strValue;
    }
}