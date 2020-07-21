using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class governmental_entities : System.Web.UI.Page
{
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
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            HtmlAnchor lnkHistory = (HtmlAnchor)Master.FindControl("lnkHistory");
            lnkHistory.HRef = "log.aspx?t=" + EncryptString.Encrypt("GovernmentalEntities");
            if (Session["User"] != null && Session["User"].ToString() != string.Empty)
            {
                if (UserPermissions.Any(
                    p =>
                        p.PageUrl.ToLower().Equals(Common.GovernmentalEntitiesPath) &&
                        (p.Show.Equals(true) || p.Add.Equals(true) || p.Edit.Equals(true) || p.Delete.Equals(true) || p.Approve.Equals(true) || p.Freze.Equals(true))))
                {
                    var per = UserPermissions.FirstOrDefault(p => p.PageUrl.ToLower().Equals(Common.GovernmentalEntitiesPath));
                    ((HtmlGenericControl)Page.Master.FindControl("ulBreadcrumb")).InnerHtml = "<li><i class='ace-icon fa fa-home home-icon'></i><a href ='Default.aspx'> الرئيسية </a></li><li>" + per.ModuleName + "</li><li><a class='active'>" + per.PageName + "</a></li>";
                    Page.Title = per.PageName;
                }
                else
                    Response.Redirect("no-permission.aspx");
            }
            else
                Response.Redirect("Login.aspx?ReturnURL=" + Request.Url.AbsolutePath);
            BindDLL();
            if (UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.GovernmentalEntitiesPath) && (p.Show.Equals(true) || p.Edit.Equals(true) || p.Delete.Equals(true) || p.Approve.Equals(true) || p.Freze.Equals(true))))
                BindData();
        }
    }
    private void BindDLL()
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            ddlStatusSrc.DataSource = db.Status;
            ddlStatusSrc.DataTextField = "name";
            ddlStatusSrc.DataValueField = "id";
            ddlStatusSrc.DataBind();
            ddlStatusSrc.Items.Insert(0, new ListItem("-- اختر --", "0"));
        }
    }
    private void BindData()
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            try
            {
                var query = from t in db.GovernmentalEntities
                            select new
                            {
                                t.id,
                                t.name,
                                t.code,
                                status = t.Status.name,
                                t.statusId
                            };
                if (txtCodeSrc.Text.Trim() != string.Empty)
                    query = query.Where(x => x.code.Equals(txtCodeSrc.Text.Trim()));
                if (txtNameSrc.Text.Trim() != string.Empty)
                    query = query.Where(x => x.name.Contains(txtNameSrc.Text.Trim()));
                if (ddlStatusSrc.SelectedValue != "0")
                    query = query.Where(x => x.statusId == int.Parse(ddlStatusSrc.SelectedValue));
                lblResult.Text = query.Count().ToString();
                gdvData.DataSource = query;
                gdvData.DataBind();
            }
            catch (Exception ex)
            {
                Common.InsertException(ex.Message, ex.StackTrace,
                           new System.IO.FileInfo(System.Web.HttpContext.Current.Request.Url.AbsolutePath).Name);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('حدث خطا اثناء الحفظ');</script>", false);
            }
        }
    }
    private void ClearControls()
    {
        txtName.Text = txtCode.Text = string.Empty;
        btnApprove.Visible = btnFreeze.Visible = false;
        lnkSave.Visible = true;
        ViewState["id"] = null;
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.GovernmentalEntitiesPath) && (p.Show.Equals(true) || p.Edit.Equals(true) || p.Delete.Equals(true) || p.Approve.Equals(true) || p.Freze.Equals(true))))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للبحث');</script>", false);
            return;
        }
        BindData();
    }
    protected void btnNewSearch_Click(object sender, EventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.GovernmentalEntitiesPath) && (p.Show.Equals(true) || p.Edit.Equals(true) || p.Delete.Equals(true) || p.Approve.Equals(true) || p.Freze.Equals(true))))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للبحث');</script>", false);
            return;
        }
        txtCodeSrc.Text=txtNameSrc.Text = string.Empty;
        BindData();
    }
    protected void lnkSave_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
            {
                try
                {
                    if (ViewState["id"] != null)
                    {
                        GovernmentalEntity u =
                            db.GovernmentalEntities.FirstOrDefault(x => x.id.Equals(int.Parse(ViewState["id"].ToString())));
                        if (u != null)
                        {
                            u.name = txtName.Text.Trim();
                            u.code = txtCode.Text.Trim().PadLeft(4, '0');
                            u.statusId = 1;
                            db.SubmitChanges(); 
                            LogWriter.LogWrite("GovernmentalEntities", ((int)ActivitiesEnum.Update).ToString(), u.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                        }
                    }
                    else
                    {
                        GovernmentalEntity u = new GovernmentalEntity();
                        u.name = txtName.Text.Trim();
                        u.code = txtCode.Text.Trim().PadLeft(4, '0');
                        u.statusId = 1;
                        db.GovernmentalEntities.InsertOnSubmit(u);
                        db.SubmitChanges();
                        LogWriter.LogWrite("GovernmentalEntities", ((int)ActivitiesEnum.Add).ToString(), u.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                    }
                    ClearControls();
                    BindData();
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertDocumentType",
                           "alert('تم الحفظ بنجاح .');", true);
                }
                catch (Exception ex)
                {
                    Common.InsertException(ex.Message, ex.StackTrace,
                         new System.IO.FileInfo(System.Web.HttpContext.Current.Request.Url.AbsolutePath).Name);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('حدث خطا اثناء الحفظ');</script>", false);
                }
            }
        }
    }
    protected void gdvData_Sorting(object sender, GridViewSortEventArgs e)
    {
        string SortDir = string.Empty;
        if (dir == SortDirection.Ascending)
        {
            dir = SortDirection.Descending;
            SortDir = "Desc";
        }
        else
        {
            dir = SortDirection.Ascending;
            SortDir = "Asc";
        }
        DataView sortedView = new DataView(dtData);
        sortedView.Sort = e.SortExpression + " " + SortDir;
        gdvData.DataSource = sortedView;
        gdvData.DataBind();
    }
    protected void gdvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvData.PageIndex = e.NewPageIndex;
        BindData();
    }
    protected void lnkEdit_Command(object sender, CommandEventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.GovernmentalEntitiesPath) && p.Edit.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للتعديل');</script>", false);
            return;
        }
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            try
            {
                ViewState["id"] = e.CommandArgument;
                var u = (from t in db.GovernmentalEntities
                         where t.id.Equals(int.Parse(e.CommandArgument.ToString()))
                         select new
                         {
                             t.id,
                             t.code,
                             t.name,
                             t.statusId
                         }).FirstOrDefault();
                if (u != null)
                {
                    txtName.Text = u.name;
                    txtCode.Text = u.code;
                    btnApprove.Visible = lnkSave.Visible = u.statusId == (int)StatusEnum.UnderApprrove;
                    btnFreeze.Visible = u.statusId != (int)StatusEnum.Freezed;
                }
            }
            catch (Exception ex)
            {
                Common.InsertException(ex.Message, ex.StackTrace,
                           new System.IO.FileInfo(System.Web.HttpContext.Current.Request.Url.AbsolutePath).Name);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('حدث خطا اثناء الحفظ');</script>", false);
            }
        }
        mpeObject.Show();
    }
    protected void lnkDelete_Command(object sender, CommandEventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.GovernmentalEntitiesPath) && p.Delete.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للحذف');</script>", false);
            return;
        }
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            try
            {
                ViewState["id"] = e.CommandArgument;
                GovernmentalEntity u = db.GovernmentalEntities.FirstOrDefault(x => x.id.Equals(int.Parse(e.CommandArgument.ToString())));
                if (u != null)
                    db.GovernmentalEntities.DeleteOnSubmit(u);
                db.SubmitChanges();
                LogWriter.LogWrite("GovernmentalEntities", ((int)ActivitiesEnum.Delete).ToString(), u.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                BindData();
                ScriptManager.RegisterStartupScript(this, GetType(), "alertDocumentType", "alert('تم الحذف بنجاح .');", true);
            }
            catch (Exception ex)
            {
                Common.InsertException(ex.Message, ex.StackTrace,
                          new System.IO.FileInfo(System.Web.HttpContext.Current.Request.Url.AbsolutePath).Name);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('حدث خطا اثناء الحفظ');</script>", false);
            }
        }
    }
    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.GovernmentalEntitiesPath) && p.Add.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للاضافة');</script>", false);
            return;
        }
        mpeObject.Show();
    }
    protected void lnkCloseModal_Click(object sender, EventArgs e)
    {
        ClearControls();
        ViewState["id"] = null;
        mpeObject.Hide();
    }
    protected void cvCode_ServerValidate(object source, ServerValidateEventArgs args)
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            if (ViewState["id"] == null)
                args.IsValid = !db.GovernmentalEntities.Any(x =>x.code.Equals(txtCode.Text.Trim()));
            else
                args.IsValid = !db.GovernmentalEntities.Any(x => x.id != int.Parse(ViewState["id"].ToString())  && x.code.Equals(txtCode.Text.Trim()));
        }
    }
    protected void btnFreeze_Click(object sender, EventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.GovernmentalEntitiesPath) && p.Freze.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للتجميد');</script>", false);
            return;
        }
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            GovernmentalEntity c = db.GovernmentalEntities.Where(x => x.id == int.Parse(ViewState["id"].ToString())).FirstOrDefault();
            try
            {
                c.statusId = (int)StatusEnum.Freezed;
                db.SubmitChanges();
                LogWriter.LogWrite("GovernmentalEntities", ((int)ActivitiesEnum.Freze).ToString(), c.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                btnFreeze.Visible = btnApprove.Visible = lnkSave.Visible = false;
                BindData();
                mpeObject.Show();
            }
            catch (Exception ex)
            {
                Common.InsertException(ex.Message, ex.StackTrace,
                          new System.IO.FileInfo(System.Web.HttpContext.Current.Request.Url.AbsolutePath).Name);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('حدث خطا اثناء الحفظ');</script>", false);
            }
        }
    }

    protected void btnApprove_Click(object sender, EventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.GovernmentalEntitiesPath) && p.Approve.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للأعتماد');</script>", false);
            return;
        }
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            GovernmentalEntity c = db.GovernmentalEntities.Where(x => x.id == int.Parse(ViewState["id"].ToString())).FirstOrDefault();
            try
            {
                c.statusId = (int)StatusEnum.Approved;
                db.SubmitChanges();
                LogWriter.LogWrite("GovernmentalEntities", ((int)ActivitiesEnum.Approve).ToString(), c.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                btnApprove.Visible = lnkSave.Visible = false;
                btnFreeze.Visible = true;
                BindData();
                mpeObject.Show();
            }
            catch (Exception ex)
            {
                Common.InsertException(ex.Message, ex.StackTrace,
                          new System.IO.FileInfo(System.Web.HttpContext.Current.Request.Url.AbsolutePath).Name);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('حدث خطا اثناء الحفظ');</script>", false);
            }
        }
    }
}