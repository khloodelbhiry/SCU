using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class login : Page
{
    public List<UserPermissions> UserPermissions
    {
        get
        {
            if (Session["UserPermissions"] != null && Session["UserPermissions"].ToString() != string.Empty)
                return global::UserPermissions.DeSerializePermissionsList(Session["UserPermissions"].ToString());
            return new List<UserPermissions>();
        }
        set { Session["UserPermissions"] = global::UserPermissions.SerializePermissionsList(value); }
    }

    public List<PrerequisitesPermissions> PrerequisitesPermissions
    {
        get
        {
            if (Session["PrerequisitesPermissions"] != null && Session["PrerequisitesPermissions"].ToString() != string.Empty)
                return global::PrerequisitesPermissions.DeSerializePermissionsList(Session["PrerequisitesPermissions"].ToString());
            return new List<PrerequisitesPermissions>();
        }
        set { Session["PrerequisitesPermissions"] = global::PrerequisitesPermissions.SerializePermissionsList(value); }
    }

    public List<StateTransitionPermissions> StateTransitionPermissions
    {
        get
        {
            if (Session["StateTransitionPermissions"] != null && Session["StateTransitionPermissions"].ToString() != string.Empty)
                return global::StateTransitionPermissions.DeSerializePermissionsList(Session["StateTransitionPermissions"].ToString());
            return new List<StateTransitionPermissions>();
        }
        set { Session["StateTransitionPermissions"] = global::StateTransitionPermissions.SerializePermissionsList(value); }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["User"] != null)
            {
                if (Session["Back"] != null)
                    Response.Redirect(Session["Back"].ToString());
                else
                    Response.Redirect("default.aspx");
            }
        }
    }
    protected void lnkLogin_Click(object sender, EventArgs e)
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            var query = (from q in db.Users
                         where q.email.Equals(txtEmail.Text.Trim())
                         && q.password.Equals(EncryptString.Encrypt(txtPassword.Text))
                         select q).FirstOrDefault();
            if (query != null)
            {
                if (query.statusId != (int)StatusEnum.Freezed)
                {
                    Session["User"] = UserDetails.SerializeUserDetails(new UserDetails(query.id, query.fullName, string.Empty, query.mobile, query.email, query.governmentalEntityId ?? 0, query.unitStructureId ?? 0, query.companyId ?? 0, query.groupId ?? 0));
                    var per = db.SP_SelectUserPermission(query.groupId).ToList();
                    List<UserPermissions> tempList = UserPermissions;
                    tempList.AddRange(
                        per.Select(
                            i =>
                    new UserPermissions(i.moduleId, i.module, i.form, i.url, i.parentForm,
                       i.create??false, i.read??false, i.update??false, i.remove??false, i.print??false, i.export??false, i.index??false, i.qa??false, i.approve??false, i.freeze??false)));
                    UserPermissions = tempList;

                    var trans = db.SP_SelectStateTransitionPermission(query.id).ToList();
                    List<StateTransitionPermissions> tempListStateTransitionPermissions = StateTransitionPermissions;
                    tempListStateTransitionPermissions.AddRange(
                        trans.Select(
                            i =>
                    new StateTransitionPermissions(i.StateID, i.StateName, bool.Parse(i.Done), bool.Parse(i.Restart))));
                    StateTransitionPermissions = tempListStateTransitionPermissions;

                    var preq = db.SP_SelectPrerequisitesPermission(query.id).ToList();
                    List<PrerequisitesPermissions> tempListPrerequisitesPermissions = PrerequisitesPermissions;
                    tempListPrerequisitesPermissions.AddRange(
                        preq.Select(
                            i =>
                    new PrerequisitesPermissions(i.processPrerequisiteId)));
                    PrerequisitesPermissions = tempListPrerequisitesPermissions;

                    if (UserDetails.DeSerializeUserDetails(Session["User"].ToString()).CompanyId == 0
                        && UserPermissions.Any(
                   p =>
                       p.PageUrl.ToLower().Equals(Common.SearchPath) && p.Show.Equals(true)))
                        Response.Redirect(Common.SearchPath + ".aspx");
                    else
                        Response.Redirect("Default.aspx");
                }
            }
            else
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alertUser",
                    "alert('عفوا، المستخدم غير موجود');", true);
        }
    }
}