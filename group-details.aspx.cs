using DevExpress.Web.ASPxTreeView;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class group_details : System.Web.UI.Page
{
    private DataTable dtData
    {
        get
        {
            return ((DataTable)Session["_dtSelectedData"]);
        }
        set
        {
            if (value == null)
            {
                Session.Remove("_dtSelectedData");
            }
            else
            {
                Session["_dtSelectedData"] = value;
            }
        }
    }
    public SortDirection dir
    {
        get
        {
            if (ViewState["dirState"] == null)
            {
                ViewState["dirState"] = SortDirection.Ascending;
            }
            return (SortDirection)ViewState["dirState"];
        }
        set
        {
            ViewState["dirState"] = value;
        }
    }
    public List<UserPermissions> UserPermissions
    {
        get
        {
            if (Session["UserPermissions"] != null && Session["UserPermissions"].ToString() != string.Empty)
                return global::UserPermissions.DeSerializePermissionsList(Session["UserPermissions"].ToString());
            else
            {
                return new List<UserPermissions>();
            }
        }
        set { Session["UserPermissions"] = global::UserPermissions.SerializePermissionsList(value); }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            HtmlAnchor lnkHistory = (HtmlAnchor)Master.FindControl("lnkHistory");
            lnkHistory.HRef = "log.aspx?t=" + EncryptString.Encrypt("Groups");
            if (Session["User"] != null && Session["User"].ToString() != string.Empty)
            {
                if (UserPermissions.Any(
                    p =>
                        p.PageUrl.ToLower().Equals(Common.GroupsPath) &&
                        ( p.Add.Equals(true) || p.Edit.Equals(true))))
                {
                    var per = UserPermissions.FirstOrDefault(p => p.PageUrl.ToLower().Equals(Common.GroupsPath));
                    ((HtmlGenericControl)Page.Master.FindControl("ulBreadcrumb")).InnerHtml = "<li><i class='ace-icon fa fa-home home-icon'></i><a href ='Default.aspx'> الرئيسية </a></li><li>" + per.ModuleName + "</li><li><a class='active'>" + per.PageName + "</a></li>";
                        Page.Title = per.PageName;
                }
                else
                    Response.Redirect("no-permission.aspx");
            }
            else
                Response.Redirect("Login.aspx?ReturnURL=" + Request.Url.AbsolutePath);
            BindData();
        }
    }
    private void BindData()
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            if (Request.QueryString["id"] != null)
            {
                Group data = db.Groups.FirstOrDefault(x => x.id == int.Parse(EncryptString.Decrypt(Request.QueryString["id"])));
                txtName.Text =data.name;
            }
            rpActivities.DataSource = db.ActivityTypes;
            rpActivities.DataBind();

            var query1 = (from x in db.ModuleForms
                          select new
                          {
                              name = x.Module.name + (x.parentId != null ? (" / " + db.ModuleForms.FirstOrDefault(z => z.id == x.parentId).name) : string.Empty) + " / " + x.name,
                              x.id,
                              x.moduleId
                          }).OrderBy(q => q.moduleId);
            rpForms.DataSource = query1;
            rpForms.DataBind();

            int? groupId = Request.QueryString["id"] != null ? (int?)int.Parse(EncryptString.Decrypt(Request.QueryString["id"])) : null;
            var query2 = (from x in db.ProcessPrerequisites
                          where x.relativeWeight > 0
                          select new
                          {
                              x.id,
                              x.name,
                              isChecked = groupId != null ? db.GroupPrerequisitesCompletions.Any(y => y.groupId == groupId && y.processPrerequisiteId == x.id) : false
                          }).OrderBy(x => x.id);
            rpPrerequisites.DataSource = query2;
            rpPrerequisites.DataBind();

            rpActions.DataSource = db.ActionsTypes;
            rpActions.DataBind();

            var query3 = (from x in db.StatesTransitions
                          select new
                          {
                              x.id,
                              name = x.dashboardAliase,
                              x.stateOrder
                          }).OrderBy(x => x.stateOrder);
            rpStates.DataSource = query3;
            rpStates.DataBind();
        }
    }
    protected void rpStates_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            var rpStateActions = (Repeater)e.Item.FindControl("rpStateActions");
            HiddenField hdfStateId = (HiddenField)e.Item.FindControl("hdfStateId");
            int? groupId = Request.QueryString["id"] != null ? (int?)int.Parse(EncryptString.Decrypt(Request.QueryString["id"])) : null;
            var query = from f in db.ActionsTypes
                        select new
                        {
                            f.id,
                            visible = db.StatesTransitionActions.Any(x => x.actionTypeId == f.id && x.statesTransitionId == int.Parse(hdfStateId.Value)),
                            isChecked = groupId != null ? db.GroupStateTransitionActions.Any(x => x.groupId == groupId && x.StatesTransitionAction.statesTransitionId == int.Parse(hdfStateId.Value) && x.StatesTransitionAction.actionTypeId == f.id) : false
                        };
            rpStateActions.DataSource = query;
            rpStateActions.DataBind();
        }
    }
    protected void rpForms_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            var rpFormActivities = (Repeater)e.Item.FindControl("rpFormActivities");
            HiddenField hdfFormId = (HiddenField)e.Item.FindControl("hdfFormId");
            int? groupId = Request.QueryString["id"] != null ? (int?)int.Parse(EncryptString.Decrypt(Request.QueryString["id"])) : null;
            var query = from f in db.ActivityTypes
                        select new
                        {
                            f.id,
                            visible = db.FormActivityTypes.Any(x => x.activityTypeId == f.id && x.formId == int.Parse(hdfFormId.Value)),
                            isChecked = groupId != null ? db.GroupActivities.Any(x => x.groupId == groupId && x.FormActivityType.formId == int.Parse(hdfFormId.Value) && x.FormActivityType.activityTypeId == f.id) : false
                        };
            rpFormActivities.DataSource = query;
            rpFormActivities.DataBind();
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            try
            {
                int groupId = 0;
                if (Request.QueryString["id"] != null)
                {
                    Group u =
                        db.Groups.FirstOrDefault(x => x.id.Equals(int.Parse(EncryptString.Decrypt(Request.QueryString["id"].ToString()))));
                    if (u != null)
                    {
                        u.name = txtName.Text;
                        groupId = u.id;
                        db.SubmitChanges();
                        LogWriter.LogWrite("Groups", ((int)ActivitiesEnum.Update).ToString(), u.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                    }
                }
                else
                {
                    Group u = new Group();
                    u.name = txtName.Text.Trim();
                    u.statusId = (int)StatusEnum.UnderApprrove;
                    db.Groups.InsertOnSubmit(u);
                    db.SubmitChanges();
                    LogWriter.LogWrite("Groups", ((int)ActivitiesEnum.Add).ToString(), u.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                    groupId = u.id;
                }
                SavePriviliges(groupId);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('تم الحفظ بنجاح');window.location='groups.aspx';</script>", false);
            }
            catch (Exception ex)
            {
                Common.InsertException(ex.Message, ex.StackTrace,
                           new System.IO.FileInfo(System.Web.HttpContext.Current.Request.Url.AbsolutePath).Name);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('حدث خطا اثناء الحفظ');</script>", false);
            }
        }
    }
    private void SavePriviliges(int groupId)
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            var query1 = db.GroupActivities.Where(x => x.groupId.Equals(groupId));
            db.GroupActivities.DeleteAllOnSubmit(query1);
            foreach (RepeaterItem ri in rpForms.Items)
            {
                HiddenField hdfFormId = ri.FindControl("hdfFormId") as HiddenField;
                Repeater rpFormActivities = ri.FindControl("rpFormActivities") as Repeater;
                foreach (RepeaterItem a in rpFormActivities.Items)
                {
                    HtmlInputCheckBox chkActivity = a.FindControl("chkActivity") as HtmlInputCheckBox;
                    if (chkActivity.Checked)
                    {
                        HiddenField hdfActivityId = a.FindControl("hdfActivityId") as HiddenField;
                        GroupActivity g = new GroupActivity();
                        g.groupId = groupId;
                        g.formActivityTypeId = db.FormActivityTypes.FirstOrDefault(x => x.formId == int.Parse(hdfFormId.Value) && x.activityTypeId == int.Parse(hdfActivityId.Value)).id;
                        db.GroupActivities.InsertOnSubmit(g);
                    }
                }
            }
            db.SubmitChanges();

            var query2 = db.GroupPrerequisitesCompletions.Where(x => x.groupId.Equals(groupId));
            db.GroupPrerequisitesCompletions.DeleteAllOnSubmit(query2);
            foreach (RepeaterItem ri in rpPrerequisites.Items)
            {
                HtmlInputCheckBox chkPrerequisite = ri.FindControl("chkPrerequisite") as HtmlInputCheckBox;
                if (chkPrerequisite.Checked)
                {
                    HiddenField hdfPrerequisiteId = ri.FindControl("hdfPrerequisiteId") as HiddenField;
                    GroupPrerequisitesCompletion g = new GroupPrerequisitesCompletion();
                    g.groupId = groupId;
                    g.processPrerequisiteId = int.Parse(hdfPrerequisiteId.Value);
                    db.GroupPrerequisitesCompletions.InsertOnSubmit(g);
                }
            }
            db.SubmitChanges();

            var query3 = db.GroupStateTransitionActions.Where(x => x.groupId.Equals(groupId));
            db.GroupStateTransitionActions.DeleteAllOnSubmit(query3);
            foreach (RepeaterItem ri in rpStates.Items)
            {
                HiddenField hdfStateId = ri.FindControl("hdfStateId") as HiddenField;
                Repeater rpStateActions = ri.FindControl("rpStateActions") as Repeater;
                foreach (RepeaterItem a in rpStateActions.Items)
                {
                    HtmlInputCheckBox chkAction = a.FindControl("chkAction") as HtmlInputCheckBox;
                    if (chkAction.Checked)
                    {
                        HiddenField hdfActionId = a.FindControl("hdfActionId") as HiddenField;
                        GroupStateTransitionAction g = new GroupStateTransitionAction();
                        g.groupId = groupId;
                        g.stateTransitionActionId = db.StatesTransitionActions.FirstOrDefault(x => x.statesTransitionId == int.Parse(hdfStateId.Value) && x.actionTypeId == int.Parse(hdfActionId.Value)).id;
                        db.GroupStateTransitionActions.InsertOnSubmit(g);
                    }
                }
            }
            db.SubmitChanges();
        }
    }
}