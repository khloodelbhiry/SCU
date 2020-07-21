using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class issues: System.Web.UI.Page
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
    public SortDirection dirNotes
    {
        get
        {
            if (ViewState["dirNotesState"] == null)
            {
                ViewState["dirNotesState"] = SortDirection.Ascending;
            }
            return (SortDirection)ViewState["dirNotesState"];
        }
        set
        {
            ViewState["dirNotesState"] = value;
        }
    }
    private DataTable dtData
    {
        get
        {
            return ((DataTable)ViewState["_dtData"]);
        }
        set
        {
            if (value == null)
            {
                ViewState.Remove("_dtData");
            }
            else
            {
                ViewState["_dtData"] = value;
            }
        }
    }
    public SortDirection dirNotesAttachments
    {
        get
        {
            if (ViewState["dirNotesAttachmentsState"] == null)
            {
                ViewState["dirNotesAttachmentsState"] = SortDirection.Ascending;
            }
            return (SortDirection)ViewState["dirNotesAttachmentsState"];
        }
        set
        {
            ViewState["dirNotesAttachmentsState"] = value;
        }
    }
    private DataTable dtNotesAttachments
    {
        get
        {
            return ((DataTable)ViewState["_dtNotesAttachments"]);
        }
        set
        {
            if (value == null)
            {
                ViewState.Remove("_dtNotesAttachments");
            }
            else
            {
                ViewState["_dtNotesAttachments"] = value;
            }
        }
    }
    public SortDirection dirIssueAttachments
    {
        get
        {
            if (ViewState["dirIssueAttachmentsState"] == null)
            {
                ViewState["dirIssueAttachmentsState"] = SortDirection.Ascending;
            }
            return (SortDirection)ViewState["dirIssueAttachmentsState"];
        }
        set
        {
            ViewState["dirIssueAttachmentsState"] = value;
        }
    }
    private DataTable dtIssueAttachments
    {
        get
        {
            return ((DataTable)ViewState["_dtIssueAttachments"]);
        }
        set
        {
            if (value == null)
            {
                ViewState.Remove("_dtIssueAttachments");
            }
            else
            {
                ViewState["_dtIssueAttachments"] = value;
            }
        }
    }
    private DataTable dtNotes
    {
        get
        {
            return ((DataTable)ViewState["_dtNotes"]);
        }
        set
        {
            if (value == null)
            {
                ViewState.Remove("_dtNotes");
            }
            else
            {
                ViewState["_dtNotes"] = value;
            }
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
            lnkHistory.HRef = "log.aspx?t=" + EncryptString.Encrypt("ProjectTargets") + "&p=" + Request.QueryString["id"];
            if (Session["User"] != null && Session["User"].ToString() != string.Empty)
            {
                if (UserPermissions.Any(
                    p =>
                        p.PageUrl.ToLower().Equals(Common.ProjectTargetPath) &&
                        (p.Show.Equals(true) || p.Add.Equals(true) || p.Edit.Equals(true) || p.Delete.Equals(true) || p.Approve.Equals(true) || p.Freze.Equals(true))))
                {
                    if (Request.QueryString["id"] != null && Request.QueryString["g"] != null)
                    {
                        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
                        {
                            var per = UserPermissions.FirstOrDefault(p => p.PageUrl.ToLower().Equals(Common.ProjectTargetPath));
                            Project query = db.Projects.FirstOrDefault(x => x.id == int.Parse(EncryptString.Decrypt(Request.QueryString["id"])));
                            ((HtmlGenericControl)Page.Master.FindControl("ulBreadcrumb")).InnerHtml = "<li><i class='ace-icon fa fa-home home-icon'></i><a href ='Default.aspx'> الرئيسية </a></li><li>" + per.ModuleName + "</li><li><strong>المشروع : " + query.name + "</strong></li><li><strong>الجهة : " + query.GovernmentalEntity.name + "</strong></li><li><strong>الشركة المنفذة : " + query.Company.name + "</strong></li><li><a class='active'>" + per.PageName + "</a></li>";
                            Page.Title = per.PageName;
                        }
                    }
                    else
                        Response.Redirect("projects.aspx");
                }
                else
                    Response.Redirect("project-no-permission.aspx?id="+Request.QueryString["id"] + "&g=" + Request.QueryString["g"] + "&c=" + Request.QueryString["c"]);
            }
            else
                Response.Redirect("Login.aspx?ReturnURL=" + Request.Url.AbsolutePath);
            if (UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectTargetPath) && (p.Show.Equals(true) || p.Edit.Equals(true) || p.Delete.Equals(true) || p.Approve.Equals(true) || p.Freze.Equals(true))))
                BindData();
        }
    }
    protected void lnkAddTarget_Command(object sender, CommandEventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectTargetPath) && p.Add.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للاضافة');</script>", false);
            return;
        }
        ViewState["state"] = e.CommandArgument.ToString();
        mpeObject.Show();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
            {
                try
                {
                    if (ViewState["id"] == null)
                    {
                        ProjectTarget q = new ProjectTarget()
                        {
                            projectId = int.Parse(EncryptString.Decrypt(Request.QueryString["id"])),
                            stateTransitionId = int.Parse(ViewState["state"].ToString()),
                            statusId = (int)IssueStatusEnum.UnderApprove,
                            startDate = new DateTime(int.Parse(txtDateYear.Text.Trim()), int.Parse(txtDateMonth.Text.Trim()), int.Parse(txtDateDay.Text.Trim())),
                            overTarget = decimal.Parse(txtOverTarget.Text),
                            reachTarget = decimal.Parse(txtReachTarget.Text),
                            error = decimal.Parse(txtError.Text),
                            count = int.Parse(txtCount.Text),
                            timeMeasurement = rdDay.Checked ? char.Parse(rdDay.Value) : char.Parse(rdHour.Value),
                            isActive = false
                        };
                        db.ProjectTargets.InsertOnSubmit(q);
                        db.SubmitChanges();
                        LogWriter.LogWrite("ProjectTargets", ((int)ActivitiesEnum.Add).ToString(), q.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, EncryptString.Decrypt(Request.QueryString["id"]), string.Empty);
                    }
                    else
                    {
                        ProjectTarget q = db.ProjectTargets.FirstOrDefault(x => x.id.Equals(int.Parse(ViewState["id"].ToString())));
                        q.startDate = new DateTime(int.Parse(txtDateYear.Text.Trim()), int.Parse(txtDateMonth.Text.Trim()), int.Parse(txtDateDay.Text.Trim()));
                        q.overTarget = decimal.Parse(txtOverTarget.Text);
                        q.reachTarget = decimal.Parse(txtReachTarget.Text);
                        q.error = decimal.Parse(txtError.Text);
                        q.count = int.Parse(txtCount.Text);
                        q.timeMeasurement = rdDay.Checked ? char.Parse(rdDay.Value) : char.Parse(rdHour.Value);
                        db.SubmitChanges();
                        LogWriter.LogWrite("ProjectTargets", ((int)ActivitiesEnum.Update).ToString(), q.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, EncryptString.Decrypt(Request.QueryString["id"]), string.Empty);
                    }
                    ClearControls();
                    BindTarget();
                    mpeTarget.Show();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('تم الحفظ بنجاح');$('html, body').animate({scrollTop: $('#divAdd').offset().top}, 2000);</script>", false);
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
    protected void lnkEdit_Command(object sender, CommandEventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectTargetPath) && p.Edit.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للتعديل');</script>", false);
            return;
        }
        ViewState["id"] = int.Parse(e.CommandArgument.ToString());
        FillControls();
    }
    private void FillControls()
    {
        if (ViewState["id"] != null)
        {
            using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
            {
                try
                {
                    var query = db.ProjectTargets.Where(t => t.id.Equals(int.Parse(ViewState["id"].ToString()))).FirstOrDefault();
                    txtDateDay.Text = query.startDate.Date.Day.ToString();
                    txtDateMonth.Text = query.startDate.Date.Month.ToString();
                    txtDateYear.Text = query.startDate.Date.Year.ToString();
                    txtOverTarget.Text = query.overTarget.ToString("G29");
                    txtReachTarget.Text = query.reachTarget.ToString("G29");
                    txtError.Text = query.error.ToString("G29");
                    txtCount.Text = query.count.ToString();
                    rdDay.Checked = query.timeMeasurement == 'd';
                    rdHour.Checked = query.timeMeasurement == 'h';
                    btnApprove.Visible = lnkSave.Visible = query.statusId == (int)StatusEnum.UnderApprrove;
                    btnFreeze.Visible = query.statusId != (int)StatusEnum.Freezed;
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
    private void ClearControls()
    {
        ViewState["id"]= null;
        txtCount.Text = txtDateDay.Text = txtDateMonth.Text = txtDateYear.Text = txtError.Text = txtOverTarget.Text = txtReachTarget.Text = string.Empty;
        rdDay.Checked = true;
        btnApprove.Visible = btnFreeze.Visible = false;
        lnkSave.Visible = true;
    }
    protected void lnkDelete_Command(object sender, CommandEventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectTargetPath) && p.Delete.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للحذف');</script>", false);
            return;
        }
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            ProjectTarget c = db.ProjectTargets.Where(x => x.id == int.Parse(e.CommandArgument.ToString())).FirstOrDefault();
            try
            {
                db.ProjectTargets.DeleteOnSubmit(c);
                db.SubmitChanges();
                LogWriter.LogWrite("ProjectTargets", ((int)ActivitiesEnum.Delete).ToString(), c.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, EncryptString.Decrypt(Request.QueryString["id"]), string.Empty);
                ViewState["state"] = c.stateTransitionId;
                BindTarget();
                mpeTarget.Show();
            }
            catch (Exception ex)
            {
                Common.InsertException(ex.Message, ex.StackTrace,
                      new System.IO.FileInfo(System.Web.HttpContext.Current.Request.Url.AbsolutePath).Name);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('حدث خطا اثناء الحفظ');</script>", false);
            }
        }
    }
    protected void gdvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvData.PageIndex = e.NewPageIndex;
        BindData();
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
    private void BindData()
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            var query = (from b in db.StatesTransitions
                         where b.relativeWeight > 0
                         from p in db.ProjectTargets
                         .Where(o => b.id == o.stateTransitionId
                         && o.projectId == int.Parse(EncryptString.Decrypt(Request.QueryString["id"]))
                         && o.isActive == true)
                         .DefaultIfEmpty()
                         select new
                         {
                             b.id,
                             b.name,
                             b.stateOrder,
                             startDate=(DateTime?)p.startDate,
                             error=(decimal?)p.error,
                             overTarget=(decimal?)p.overTarget,
                             reachTarget=(decimal?)p.reachTarget,
                             status=p.Status.name,
                             Target = p.count + " / " + (p.timeMeasurement == 'd' ? "اليوم" : "الساعة")
                         }).ToList();
            dtData = query.OrderBy(x => x.stateOrder).CopyToDataTable();
            gdvData.DataSource = dtData;
            gdvData.DataBind();
        }
    }
    protected void btnFreeze_Click(object sender, EventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectTargetPath) && p.Freze.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للتجميد');</script>", false);
            return;
        }
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            ProjectTarget c = db.ProjectTargets.Where(x => x.id == int.Parse(ViewState["id"].ToString())).FirstOrDefault();
            try
            {
                c.statusId = (int)StatusEnum.Freezed; 
                if (c.isActive)
                {
                    LogWriter.LogWrite("ProjectTargets", ((int)ActivitiesEnum.Update).ToString(), c.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, EncryptString.Decrypt(Request.QueryString["id"]), "إلغاء تنشيط التارجت");
                    c.isActive = false;
                }
                db.SubmitChanges();
                LogWriter.LogWrite("ProjectTargets", ((int)ActivitiesEnum.Freze).ToString(), c.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                btnFreeze.Visible = btnApprove.Visible = lnkSave.Visible = false;
                ViewState["state"] = c.stateTransitionId;
                BindTarget();
                mpeTarget.Show();
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
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectTargetPath) && p.Approve.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للأعتماد');</script>", false);
            return;
        }
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            ProjectTarget c = db.ProjectTargets.Where(x => x.id == int.Parse(ViewState["id"].ToString())).FirstOrDefault();
            try
            {
                c.statusId = (int)StatusEnum.Approved;
                db.SubmitChanges();
                LogWriter.LogWrite("ProjectTargets", ((int)ActivitiesEnum.Approve).ToString(), c.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                btnApprove.Visible = lnkSave.Visible = false;
                btnFreeze.Visible = true;
                ViewState["state"] = c.stateTransitionId;
                BindTarget();
                mpeTarget.Show();
            }
            catch (Exception ex)
            {
                Common.InsertException(ex.Message, ex.StackTrace,
                          new System.IO.FileInfo(System.Web.HttpContext.Current.Request.Url.AbsolutePath).Name);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('حدث خطا اثناء الحفظ');</script>", false);
            }
        }
    }
    protected void lnkTarget_Command(object sender, CommandEventArgs e)
    {
        ViewState["state"] = e.CommandArgument;
        BindTarget();
        mpeTarget.Show();
    }
    private void BindTarget()
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            try
            {
                var query = from q in db.ProjectTargets
                            where q.stateTransitionId == int.Parse(ViewState["state"].ToString())
                            select new
                            {
                                q.id,
                                q.isActive,
                                q.overTarget,
                                q.reachTarget,
                                q.startDate,
                                status = q.Status.name,
                                q.error,
                                q.statusId,
                                Target = q.count + " / " + (q.timeMeasurement == 'd' ? "اليوم" : "الساعة"),
                                IsCheckBoxVisible = q.statusId == (int)StatusEnum.Approved
                            };
                gdvTarget.DataSource = query;
                gdvTarget.DataBind();
            }
            catch (Exception ex)
            {
                Common.InsertException(ex.Message, ex.StackTrace,
                          new System.IO.FileInfo(System.Web.HttpContext.Current.Request.Url.AbsolutePath).Name);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('حدث خطا اثناء الحفظ');</script>", false);
            }
        }
    }
    protected void lnkCloseTarget_Click(object sender, EventArgs e)
    {
        ViewState["state"] = null;
        mpeTarget.Hide();
    }


    protected void lnkCloseModal_Click(object sender, EventArgs e)
    {
        ClearControls();
        ViewState["state"] = null;
        mpeObject.Hide();
    }
    protected void gdvTarget_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvTarget.PageIndex = e.NewPageIndex;
        BindTarget();
    }

    protected void btnActivation_Click(object sender, EventArgs e)
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            try
            {
                ProjectTarget t = db.ProjectTargets.FirstOrDefault(x => x.id == int.Parse(hdfId.Value));
                if (!t.isActive)
                {
                    ProjectTarget other = db.ProjectTargets.FirstOrDefault(x => x.isActive == true && x.stateTransitionId == t.stateTransitionId && x.projectId == t.projectId);
                    if (other != null)
                    {
                        other.isActive = false;
                        LogWriter.LogWrite("ProjectTargets", ((int)ActivitiesEnum.Update).ToString(), other.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, EncryptString.Decrypt(Request.QueryString["id"]), "إلغاء تنشيط التارجت");
                    }
                    LogWriter.LogWrite("ProjectTargets", ((int)ActivitiesEnum.Update).ToString(), t.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, EncryptString.Decrypt(Request.QueryString["id"]), " تنشيط التارجت");
                }
                else
                    LogWriter.LogWrite("ProjectTargets", ((int)ActivitiesEnum.Update).ToString(), t.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, EncryptString.Decrypt(Request.QueryString["id"]), "إلغاء تنشيط التارجت");
                t.isActive = !t.isActive;
                db.SubmitChanges();
                BindTarget();
                BindData();
                mpeTarget.Show();
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