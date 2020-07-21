using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

/// <summary>
/// Summary description for UserPermissions
/// </summary>
public class PrerequisitesPermissions
{
    public PrerequisitesPermissions()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    private int _id;
    #region Public Variables
   public int ID
    {
        get { return _id; }
        set { _id = value; }
    }
    #endregion
    public PrerequisitesPermissions(int id)
    {
        _id = id;
    }
    private static string SerializePermissions(PrerequisitesPermissions up)
    {
        StringBuilder value = new StringBuilder();
        value.Append(up.ID);
        return value.ToString();
    }
    private static PrerequisitesPermissions DeSerializePermissions(string up)
    {
        string[] details = up.Split(';');
        return new PrerequisitesPermissions(int.Parse(details[0]));
    }
    public static string SerializePermissionsList(List<PrerequisitesPermissions> its)
    {
        StringBuilder value = new StringBuilder();
        try
        {
            foreach (PrerequisitesPermissions item in its)
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

    public static List<PrerequisitesPermissions> DeSerializePermissionsList(string its)
    {
        string[] details = its.Split('*');
        List<PrerequisitesPermissions> items = new List<PrerequisitesPermissions>();
        foreach (string item in details)
        {
            items.Add(DeSerializePermissions(item));
        }
        return items;
    }
}