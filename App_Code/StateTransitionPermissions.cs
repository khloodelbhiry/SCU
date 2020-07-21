using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

/// <summary>
/// Summary description for UserPermissions
/// </summary>
public class StateTransitionPermissions
{
    public StateTransitionPermissions()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    private int _StateID;
    private string _StateName;
    private bool _done;
    private bool _restart;



    #region Public Variables
    public bool Done
    {
        get { return _done; }
        set { _done = value; }
    }
    public bool Restart
    {
        get { return _restart; }
        set { _restart = value; }
    }
    public int StateID
    {
        get { return _StateID; }
        set { _StateID = value; }
    }
    public string StateName
    {
        get { return _StateName; }
        set { _StateName = value; }
    }
    #endregion
    public StateTransitionPermissions(int stateID, string stateName, bool done, bool restart)
    {
        _StateID = stateID;
        _StateName = stateName;
        _done = done;
        _restart = restart;
    }
    private static string SerializePermissions(StateTransitionPermissions up)
    {
        StringBuilder value = new StringBuilder();
        value.Append(up.StateID);
        value.Append(";" + up.StateName);
        value.Append(";" + up.Done);
        value.Append(";" + up.Restart);
        return value.ToString();
    }
    private static StateTransitionPermissions DeSerializePermissions(string up)
    {
        string[] details = up.Split(';');
        return new StateTransitionPermissions(int.Parse(details[0])
            , details[1]
            , bool.Parse(details[2])
            , bool.Parse(details[3]));
    }
    public static string SerializePermissionsList(List<StateTransitionPermissions> its)
    {
        StringBuilder value = new StringBuilder();
        try
        {
            foreach (StateTransitionPermissions item in its)
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

    public static List<StateTransitionPermissions> DeSerializePermissionsList(string its)
    {
        string[] details = its.Split('*');
        List<StateTransitionPermissions> items = new List<StateTransitionPermissions>();
        foreach (string item in details)
        {
            items.Add(DeSerializePermissions(item));
        }
        return items;
    }
}