using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

/// <summary>
/// Class to save important details for user
/// </summary>
public class UserDetails
{
    private int _ID;
    private string _FullName;
    private string _Name;
    private string _Mobile;
    private string _Email;
    private int _GroupId;
    private int _GovernmentalEntityId;
    private int _UnitStructureId;
    private int _CompanyId;

    public int GovernmentalEntityId
    {
        get { return _GovernmentalEntityId; }
        set { _GovernmentalEntityId = value; }
    }
    public string Mobile
    {
        get { return _Mobile; }
        set { _Mobile = value; }
    }
    public string Email
    {
        get { return _Email; }
        set { _Email = value; }
    }
    public string Name
    {
        get { return _Name; }
        set { _Name = value; }
    }
    public string FullName
    {
        get { return _FullName; }
        set { _FullName = value; }
    }
    public int ID
    {
        get { return _ID; }
        set { _ID = value; }
    }
    public int UnitStructureId
    {
        get { return _UnitStructureId; }
        set { _UnitStructureId = value; }
    }
    public int CompanyId
    {
        get { return _CompanyId; }
        set { _CompanyId = value; }
    }
    public int GroupId
    {
        get { return _GroupId; }
        set { _GroupId = value; }
    }
    public UserDetails()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public UserDetails(int id,
        string fullName,
        string name,
        string mobile,
        string email,
        int governmentalEntityId,
        int unitStructureId,
        int companyId,
        int groupId)
    {
        _ID = id;
        _FullName = fullName;
        _Name = name;
        _Mobile = mobile;
        _Email = email;
        _GovernmentalEntityId = governmentalEntityId;
        _UnitStructureId = unitStructureId;
        _CompanyId = companyId;
        _GroupId = groupId;
    }

    public static string SerializeUserDetails(UserDetails det)
    {
        StringBuilder value = new StringBuilder();
        value.Append(det.ID);
        value.Append(";" + (det.FullName ?? "").Replace(";", ""));
        value.Append(";" + det.Name);
        value.Append(";" + det.Mobile);
        value.Append(";" + det.Email);
        value.Append(";" + det.GovernmentalEntityId);
        value.Append(";" + det.UnitStructureId);
        value.Append(";" + det.CompanyId);
        value.Append(";" + det.GroupId);
        return value.ToString();
    }

    public static UserDetails DeSerializeUserDetails(string det)
    {
        string[] details = det.Split(';');
        if (details.Count() == 9)
        {
            return new UserDetails(int.Parse(details[0])
                        , details[1]
                        , details[2]
                        , details[3]
                        , details[4]
                        , int.Parse(details[5])
                        , int.Parse(details[6])
                        , int.Parse(details[7])
                        , int.Parse(details[8]));
        }
        else
        {
            return null;
        }
    }
}