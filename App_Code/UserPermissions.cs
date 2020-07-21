using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

/// <summary>
/// Summary description for UserPermissions
/// </summary>
public class UserPermissions
{
    public UserPermissions()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    private int _moduleID;
    private string _moduleName;
    private string _pageName;
    private string _pageUrl;
    private string _parentForm;
    private bool _show;
    private bool _add;
    private bool _edit;
    private bool _delete;
    private bool _index;
    private bool _qa;
    private bool _export;
    private bool _print;
    private bool _approve;
    private bool _freze;


    #region Public Variables
    public bool Qa
    {
        get { return _qa; }
        set { _qa = value; }
    }
    public bool Export
    {
        get { return _export; }
        set { _export = value; }
    }
    public bool Print
    {
        get { return _print; }
        set { _print = value; }
    }
    public int ModuleID
    {
        get { return _moduleID; }
        set { _moduleID = value; }
    }
    public string ModuleName
    {
        get { return _moduleName; }
        set { _moduleName = value; }
    }
    public string ParentForm
    {
        get { return _parentForm; }
        set { _parentForm = value; }
    }
    public string PageName
    {
        get { return _pageName; }
        set { _pageName = value; }
    }
    public string PageUrl
    {
        get { return _pageUrl; }
        set { _pageUrl = value; }
    }
    public bool Show
    {
        get { return _show; }
        set { _show = value; }
    }
    public bool Add
    {
        get { return _add; }
        set { _add = value; }
    }
    public bool Edit
    {
        get { return _edit; }
        set { _edit = value; }
    }
    public bool Delete
    {
        get { return _delete; }
        set { _delete = value; }
    }
    public bool Index
    {
        get { return _index; }
        set { _index = value; }
    }
    public bool Approve
    {
        get { return _approve; }
        set { _approve = value; }
    }
    public bool Freze
    {
        get { return _freze; }
        set { _freze = value; }
    }
    #endregion
    public UserPermissions(int moduleID,string moduleName, string pageName, string pageUrl,string parentForm, bool add, bool show, bool edit, bool delete,bool print,bool export,bool index,bool qa,bool approve,bool freze)
    {
        _moduleID = moduleID;
        _moduleName = moduleName;
        _pageName = pageName;
        _pageUrl = pageUrl;
        _parentForm = parentForm;
        _show = show;
        _add = add;
        _edit = edit;
        _delete = delete;
        _index = index;
        _qa = qa;
        _export = export;
        _print = print;
        _approve = approve;
        _freze = freze;
    }
    private static string SerializePermissions(UserPermissions up)
    {
        StringBuilder value = new StringBuilder();
        value.Append(up.ModuleID);
        value.Append(";" + up.ModuleName);
        value.Append(";" + up.PageName);
        value.Append(";" + up.PageUrl);
        value.Append(";" + up.ParentForm);
        value.Append(";" + up.Add);
        value.Append(";" + up.Show);
        value.Append(";" + up.Edit);
        value.Append(";" + up.Delete);
        value.Append(";" + up.Print);
        value.Append(";" + up.Export);
        value.Append(";" + up.Index);
        value.Append(";" + up.Qa);
        value.Append(";" + up.Approve);
        value.Append(";" + up.Freze);
        return value.ToString();
    }
    private static UserPermissions DeSerializePermissions(string up)
    {
        string[] details = up.Split(';');
        return new UserPermissions(int.Parse(details[0])
            , details[1]
            , details[2]
            , details[3]
            , details[4]
            , bool.Parse(details[5])
            , bool.Parse(details[6])
            , bool.Parse(details[7])
            , bool.Parse(details[8])
            , bool.Parse(details[9])
            , bool.Parse(details[10])
            , bool.Parse(details[11])
            , bool.Parse(details[12])
            , bool.Parse(details[13])
            , bool.Parse(details[14]));
    }
    public static string SerializePermissionsList(List<UserPermissions> its)
    {
        StringBuilder value = new StringBuilder();
        try
        {
            foreach (UserPermissions item in its)
            {
                if (value.ToString() == string.Empty)
                {
                    value.Append(SerializePermissions(item));
                }
                else
                {
                    value.Append("*" + SerializePermissions(item));
                }
            }
        }
        catch (Exception)
        {
            // ignored
        }

        return value.ToString();
    }

    public static List<UserPermissions> DeSerializePermissionsList(string its)
    {
        string[] details = its.Split('*');
        List<UserPermissions> items = new List<UserPermissions>();
        foreach (string item in details)
        {
            items.Add(DeSerializePermissions(item));
        }
        return items;
    }
}