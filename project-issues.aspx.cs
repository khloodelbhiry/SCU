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
            lnkHistory.HRef = "log.aspx?t=" + EncryptString.Encrypt("Issues"+";"+ "IssueNotes"+";"+ "NoteAttachments"+";"+ "IssueAttachments") + "&p=" + Request.QueryString["id"];
            if (Session["User"] != null && Session["User"].ToString() != string.Empty)
            {
                if (UserPermissions.Any(
                    p =>
                        p.PageUrl.ToLower().Equals(Common.ProjectIssuesPath) &&
                        (p.Show.Equals(true) || p.Add.Equals(true) || p.Edit.Equals(true) || p.Delete.Equals(true) || p.Approve.Equals(true) || p.Freze.Equals(true))))
                {
                    if (Request.QueryString["id"] != null && Request.QueryString["g"] != null)
                    {
                        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
                        {
                            var per = UserPermissions.FirstOrDefault(p => p.PageUrl.ToLower().Equals(Common.ProjectIssuesPath));
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
            BindDDL();
            if (UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectIssuesPath) && (p.Show.Equals(true) || p.Edit.Equals(true) || p.Delete.Equals(true) || p.Approve.Equals(true) || p.Freze.Equals(true))))
                BindData();
        }
    }

    #region Issues
    private void BindDDL()
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            var query1 = from q in db.IssueSeverities
                         select new
                         {
                             q.id,
                             q.name
                         };
            ddlSeverity.DataSource = query1;
            ddlSeverity.DataTextField = "name";
            ddlSeverity.DataValueField = "id";
            ddlSeverity.DataBind();
            ddlSeverity.Items.Insert(0, new ListItem("-- اختر --", "0"));

            ddlSeveritySrc.DataSource = query1;
            ddlSeveritySrc.DataTextField = "name";
            ddlSeveritySrc.DataValueField = "id";
            ddlSeveritySrc.DataBind();
            ddlSeveritySrc.Items.Insert(0, new ListItem("-- اختر --", "0"));

            var query2 = from q in db.IssueStatus
                         select new
                         {
                             q.id,
                             q.name
                         };
            ddlStatusSrc.DataSource = query2;
            ddlStatusSrc.DataTextField = "name";
            ddlStatusSrc.DataValueField = "id";
            ddlStatusSrc.DataBind();
            ddlStatusSrc.Items.Insert(0, new ListItem("-- اختر --", "0"));
        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectIssuesPath) && p.Add.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للاضافة');</script>", false);
            return;
        }
        mpeIssue.Show();
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
                        Issue q = new Issue()
                        {
                            projectId = int.Parse(EncryptString.Decrypt(Request.QueryString["id"])),
                            issue1 = txtIssue.Text,
                            statusId = (int)IssueStatusEnum.UnderApprove,
                            date = DateTime.Now,
                            severityId = int.Parse(ddlSeverity.SelectedValue),
                            userId = UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID
                        };
                        db.Issues.InsertOnSubmit(q);
                        db.SubmitChanges();
                        LogWriter.LogWrite("Issues", ((int)ActivitiesEnum.Add).ToString(), q.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, EncryptString.Decrypt(Request.QueryString["id"]), string.Empty);
                    }
                    else
                    {
                        Issue q = db.Issues.FirstOrDefault(x => x.id.Equals(int.Parse(ViewState["id"].ToString())));
                        q.issue1 = txtIssue.Text;
                        q.statusId = (int)IssueStatusEnum.UnderApprove;
                        q.severityId = int.Parse(ddlSeverity.SelectedValue);
                        db.SubmitChanges();
                        LogWriter.LogWrite("Issues", ((int)ActivitiesEnum.Update).ToString(), q.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, EncryptString.Decrypt(Request.QueryString["id"]), string.Empty);
                    }
                    ClearControls();
                    BindData();
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
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectIssuesPath) && p.Edit.Equals(true)))
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
                    var query = db.Issues.Where(t => t.id.Equals(int.Parse(ViewState["id"].ToString()))).FirstOrDefault();
                    txtIssue.Text = query.issue1;
                    ddlSeverity.SelectedValue = query.severityId.ToString();
                    btnApprove.Visible = btnSave.Visible = query.statusId == (int)IssueStatusEnum.UnderApprove;
                    btnCloseIssue.Visible = query.statusId != (int)IssueStatusEnum.Closed;
                    lblAge.Text = (DateTime.Now - query.date).Days.ToString() + " يوم، " + (DateTime.Now - query.date).Hours.ToString() + " ساعه، " + (DateTime.Now - query.date).Minutes.ToString() + " دقيقة ";
                    divAge.Visible = true;
                    mpeIssue.Show();
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
    protected void btnClose_Click(object sender, EventArgs e)
    {
        ClearControls();
        ViewState["id"] = null;
    }
    private void ClearControls()
    {
        ViewState["id"] = null;
        lblAge.Text=  txtIssue.Text =string.Empty;
        btnCloseIssue.Visible = btnApprove.Visible=divAge.Visible = false;
        ddlSeverity.SelectedValue = "0";
    }
    protected void btnDelete_Command(object sender, CommandEventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectIssuesPath) && p.Delete.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للحذف');</script>", false);
            return;
        }
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            Issue c = db.Issues.Where(x => x.id == int.Parse(e.CommandArgument.ToString())).FirstOrDefault();
            try
            {
                db.Issues.DeleteOnSubmit(c);
                db.SubmitChanges();
                LogWriter.LogWrite("Issues", ((int)ActivitiesEnum.Delete).ToString(), c.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, EncryptString.Decrypt(Request.QueryString["id"]), string.Empty);
                BindData();
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
            var query = (from b in db.Issues
                         where b.projectId==int.Parse(EncryptString.Decrypt(Request.QueryString["id"]))
                         select new
                         {
                             b.id,
                             b.issue1,
                             b.statusId,
                             b.severityId,
                             b.date,
                             b.User.fullName,
                             IssueSeverity=b.IssueSeverity.name,
                             IssueStatus=b.IssueStatus.name,
                             Age=(DateTime.Now-b.date).Hours,
                             NotesCount=b.IssueNotes.Count(),
                             AttachmentCount=b.IssueAttachments.Count()
                         });
            if (ddlOperator.SelectedValue != "-1")
            {
                if (ddlOperator.SelectedValue == "equal")
                    query = query.Where(x => x.Age.Equals(int.Parse(txtAge.Text)));
                else if (ddlOperator.SelectedValue == "larger")
                    query = query.Where(x => x.Age > int.Parse(txtAge.Text));
                else if (ddlOperator.SelectedValue == "smaller")
                    query = query.Where(x => x.Age < int.Parse(txtAge.Text));
            }
            if (ddlStatusSrc.SelectedValue != "0")
                query = query.Where(x => x.statusId.Equals(int.Parse(ddlStatusSrc.SelectedValue)));
            if (ddlSeveritySrc.SelectedValue != "0")
                query = query.Where(x => x.severityId.Equals(int.Parse(ddlSeveritySrc.SelectedValue)));
            if (txtBySrc.Text.Trim() != string.Empty)
                query = query.Where(x => x.fullName.Contains(txtBySrc.Text.Trim()));
            dtData = query.OrderByDescending(x => x.id).CopyToDataTable();
            gdvData.DataSource = dtData;
            gdvData.DataBind();
            lblResult.Text = dtData.Rows.Count.ToString();
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectIssuesPath) && p.Show.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للبحث');</script>", false);
            return;
        }
        BindData();
    }
    protected void btnClearSearch_Click(object sender, EventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectIssuesPath) && p.Show.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للبحث');</script>", false);
            return;
        }
        txtBySrc.Text = string.Empty;
        ddlSeveritySrc.SelectedValue = ddlStatusSrc.SelectedValue = "0";
    }
    protected void btnApprove_Click(object sender, EventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectIssuesPath) && p.Approve.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للاعتماد');</script>", false);
            return;
        }
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            Issue c = db.Issues.Where(x => x.id == int.Parse(ViewState["id"].ToString())).FirstOrDefault();
            try
            {
                c.statusId = (int)IssueStatusEnum.Open;
                db.SubmitChanges();
                LogWriter.LogWrite("Issues", ((int)ActivitiesEnum.Approve).ToString(), c.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, EncryptString.Decrypt(Request.QueryString["id"]), string.Empty);
                FillControls();
                BindData();
            }
            catch (Exception ex)
            {
                Common.InsertException(ex.Message, ex.StackTrace,
                      new System.IO.FileInfo(System.Web.HttpContext.Current.Request.Url.AbsolutePath).Name);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('حدث خطا اثناء الحفظ');</script>", false);
            }
        }
    }

    protected void btnCloseIssue_Click(object sender, EventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectIssuesPath) && p.Freze.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للأغلاق المشكلة');</script>", false);
            return;
        }
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            Issue c = db.Issues.Where(x => x.id == int.Parse(ViewState["id"].ToString())).FirstOrDefault();
            try
            {
                c.statusId = (int)IssueStatusEnum.Closed;
                db.SubmitChanges();
                LogWriter.LogWrite("Issues", ((int)ActivitiesEnum.Freze).ToString(), c.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, EncryptString.Decrypt(Request.QueryString["id"]), "اغلاق المشكلة");
                FillControls();
                BindData();
            }
            catch (Exception ex)
            {
                Common.InsertException(ex.Message, ex.StackTrace,
                      new System.IO.FileInfo(System.Web.HttpContext.Current.Request.Url.AbsolutePath).Name);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('حدث خطا اثناء الحفظ');</script>", false);
            }
        }
    }
    #endregion

    #region Issue Notes
    protected void lnkNotes_Command(object sender, CommandEventArgs e)
    {
        ViewState["id"] = e.CommandArgument;
        BindNotes();
        mpeNotes.Show();
    }

    protected void lnkCloseNotes_Click(object sender, EventArgs e)
    {
        ViewState["id"] = null;
        mpeNotes.Hide();
    }

    protected void btnSaveNotes_Click(object sender, EventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectIssuesPath) && p.Add.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للاضافة');</script>", false);
            return;
        }
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            try
            {
                if (ViewState["NoteID"] == null)
                {
                    IssueNote q = new IssueNote();
                    q.issueId = int.Parse(ViewState["id"].ToString());
                    q.note = txtNotes.Text;
                    q.statusId = (int)IssueStatusEnum.UnderApprove;
                    q.date = DateTime.Now;
                    q.userId = UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID;
                    db.IssueNotes.InsertOnSubmit(q);
                    db.SubmitChanges();
                    LogWriter.LogWrite("IssueNotes", ((int)ActivitiesEnum.Add).ToString(), q.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, EncryptString.Decrypt(Request.QueryString["id"]), "اضافة ملاحظة (المشكلات)");
                }
                else
                {
                    IssueNote q = db.IssueNotes.FirstOrDefault(x => x.id.Equals(int.Parse(ViewState["NotesID"].ToString())));
                    q.note = txtNotes.Text;
                    db.SubmitChanges();
                    LogWriter.LogWrite("IssueNotes", ((int)ActivitiesEnum.Update).ToString(), q.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, EncryptString.Decrypt(Request.QueryString["id"]), "تعديل ملاحظة (المشكلات)");
                }
                ClearNotesControls();
                mpeNotes.Show();
                BindNotes();
                BindData();
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
    private void ClearNotesControls()
    {
        txtNotes.Text= string.Empty;
        btnApproveNotes.Visible = btnFreezeNotes.Visible = false;
        ViewState["NoteID"] = null;
        btnSaveNotes.Visible = true;
    }
    protected void btnApproveNotes_Click(object sender, EventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectIssuesPath) && p.Approve.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للاعتماد');</script>", false);
            return;
        }
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            IssueNote c = db.IssueNotes.Where(x => x.id == int.Parse(ViewState["NoteID"].ToString())).FirstOrDefault();
            try
            {
                c.statusId = (int)StatusEnum.Approved;
                db.SubmitChanges();
                LogWriter.LogWrite("IssueNotes", ((int)ActivitiesEnum.Approve).ToString(), c.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, EncryptString.Decrypt(Request.QueryString["id"]), "اعتماد ملاحظة (المشكلات)");
                ClearNotesControls();
                BindNotes();
                mpeNotes.Show();
            }
            catch (Exception ex)
            {
                Common.InsertException(ex.Message, ex.StackTrace,
                      new System.IO.FileInfo(System.Web.HttpContext.Current.Request.Url.AbsolutePath).Name);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('حدث خطا اثناء الحفظ');</script>", false);
            }
        }
    }

    protected void btnFreezeNotes_Click(object sender, EventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectIssuesPath) && p.Freze.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للتجميد');</script>", false);
            return;
        }
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            IssueNote c = db.IssueNotes.Where(x => x.id == int.Parse(ViewState["NoteID"].ToString())).FirstOrDefault();
            try
            {
                c.statusId = (int)StatusEnum.Freezed;
                db.SubmitChanges();
                LogWriter.LogWrite("IssueNotes", ((int)ActivitiesEnum.Freze).ToString(), c.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, EncryptString.Decrypt(Request.QueryString["id"]), "تجميد ملاحظة (المشكلات)");
                ClearNotesControls();
                BindNotes();
                mpeNotes.Show();
            }
            catch (Exception ex)
            {
                Common.InsertException(ex.Message, ex.StackTrace,
                      new System.IO.FileInfo(System.Web.HttpContext.Current.Request.Url.AbsolutePath).Name);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('حدث خطا اثناء الحفظ');</script>", false);
            }
        }
    }

    protected void gdvNotes_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvNotes.PageIndex = e.NewPageIndex;
        BindNotes();
        mpeNotes.Show();
    }
    protected void gdvNotes_Sorting(object sender, GridViewSortEventArgs e)
    {
        string SortDir = string.Empty;
        if (dirNotes == SortDirection.Ascending)
        {
            dirNotes = SortDirection.Descending;
            SortDir = "Desc";
        }
        else
        {
            dirNotes = SortDirection.Ascending;
            SortDir = "Asc";
        }
        DataView sortedView = new DataView(dtNotes);
        sortedView.Sort = e.SortExpression + " " + SortDir;
        gdvNotes.DataSource = sortedView;
        gdvNotes.DataBind();
        mpeNotes.Show();
    }

    protected void btnEditNote_Command(object sender, CommandEventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectIssuesPath) && p.Edit.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للتعديل');</script>", false);
            return;
        }
        ViewState["NoteID"] = e.CommandArgument;
        BindNotesControls();
        mpeNotes.Show();
    }
    private void BindNotesControls()
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            try
            {
                IssueNote q = db.IssueNotes.Where(x => x.id == int.Parse(ViewState["NoteID"].ToString())).FirstOrDefault();
                txtNotes.Text = q.note;
                btnApproveNotes.Visible = btnFreezeNotes.Visible = btnSaveNotes.Visible = q.statusId == (int)StatusEnum.UnderApprrove;
            }
            catch (Exception ex)
            {
                Common.InsertException(ex.Message, ex.StackTrace,
                          new System.IO.FileInfo(System.Web.HttpContext.Current.Request.Url.AbsolutePath).Name);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('حدث خطا اثناء الحفظ');</script>", false);
            }
        }
    }
    private void BindNotes()
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            var query = from q in db.IssueNotes
                        where q.issueId.Equals(int.Parse(ViewState["id"].ToString()))
                        select new
                        {
                            q.id,
                            q.date,
                            q.User.fullName,
                            q.note,
                            status=q.Status.name,
                            AttachmentCount=q.NoteAttachments.Count()
                        };
            dtNotes = query.CopyToDataTable();
            gdvNotes.DataSource = dtNotes;
            gdvNotes.DataBind();
        }
    }
    #endregion

    #region Notes Attachments
    protected void lnkCloseNotesAttachments_Click(object sender, EventArgs e)
    {
        ViewState["NoteID"] = null;
        mpeNotesAttachments.Hide();
        mpeNotes.Show();
    }

    protected void btnSaveNotesAttachments_Click(object sender, EventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectIssuesPath) && p.Add.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للاضافة');</script>", false);
            return;
        }
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            try
            {
                if (fuNotesAttachments.HasFile)
                {
                    string path = Server.MapPath("NotesAttachments/");
                    while (File.Exists(path + fuNotesAttachments.FileName))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('يوجد مرفق بنفس الاسم');</script>", false);
                    }
                    fuNotesAttachments.SaveAs(path + fuNotesAttachments.FileName);
                    NoteAttachment q = new NoteAttachment();
                    q.fileName = "NotesAttachments/" + fuNotesAttachments.FileName;
                    q.date = DateTime.Now;
                    q.issueNoteId = int.Parse(ViewState["NoteID"].ToString());
                    q.userId = UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID;
                    db.NoteAttachments.InsertOnSubmit(q);
                    db.SubmitChanges();
                    LogWriter.LogWrite("NoteAttachments", ((int)ActivitiesEnum.Add).ToString(), q.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, EncryptString.Decrypt(Request.QueryString["id"]), "اضافة مرفق على ملاحظة (المشكلات)");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('تم الحفظ بنجاح');</script>", false);
                    BindNotesAttachments();
                    mpeNotesAttachments.Show();
                }
            }
            catch (Exception ex)
            {
                Common.InsertException(ex.Message, ex.StackTrace,
                          new System.IO.FileInfo(System.Web.HttpContext.Current.Request.Url.AbsolutePath).Name);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('حدث خطا اثناء الحفظ');</script>", false);
            }
        }
    }

    protected void lnkNotesAttachments_Command(object sender, CommandEventArgs e)
    {
        ViewState["NoteID"] = e.CommandArgument;
        BindNotesAttachments();
        mpeNotesAttachments.Show();
    }
    private void BindNotesAttachments()
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            var query = from q in db.NoteAttachments
                        where q.issueNoteId.Equals(int.Parse(ViewState["NoteID"].ToString()))
                        select new
                        {
                            q.fileName,
                            q.id,
                            q.date
                        };
            dtNotesAttachments = query.CopyToDataTable();
            gdvNotesAttachments.DataSource = dtNotesAttachments;
            gdvNotesAttachments.DataBind();
        }
    }

    protected void gdvNotesAttachments_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvNotesAttachments.PageIndex = e.NewPageIndex;
        BindNotesAttachments();
    }
    protected void gdvNotesAttachments_Sorting(object sender, GridViewSortEventArgs e)
    {
        string SortDir = string.Empty;
        if (dirNotesAttachments == SortDirection.Ascending)
        {
            dirNotesAttachments = SortDirection.Descending;
            SortDir = "Desc";
        }
        else
        {
            dirNotesAttachments = SortDirection.Ascending;
            SortDir = "Asc";
        }
        DataView sortedView = new DataView(dtNotesAttachments);
        sortedView.Sort = e.SortExpression + " " + SortDir;
        gdvNotesAttachments.DataSource = sortedView;
        gdvNotesAttachments.DataBind();
    }

    protected void btnDeleteNoteAttachment_Command(object sender, CommandEventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectIssuesPath) && p.Delete.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للحذف');</script>", false);
            return;
        }
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            NoteAttachment c = db.NoteAttachments.Where(x => x.id == int.Parse(e.CommandArgument.ToString())).FirstOrDefault();
            try
            {
                if (File.Exists(Server.MapPath(c.fileName)))
                    File.Delete(Server.MapPath(c.fileName));
            }
            catch (Exception ex)
            {
                Common.InsertException(ex.Message, ex.StackTrace,
                          new System.IO.FileInfo(System.Web.HttpContext.Current.Request.Url.AbsolutePath).Name);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('حدث خطا اثناء الحفظ');</script>", false);
            }
            db.NoteAttachments.DeleteOnSubmit(c);
            db.SubmitChanges();
            LogWriter.LogWrite("NoteAttachments", ((int)ActivitiesEnum.Delete).ToString(), c.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, EncryptString.Decrypt(Request.QueryString["id"]), "حذف مرفق لملاحظة (المشكلات)");
            BindNotesAttachments();
            mpeNotesAttachments.Show();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('تم الحذف بنجاح'); </script>", false);
        }
    }
    #endregion

    #region Issue Attachments
    protected void lnkCloseIssueAttachment_Click(object sender, EventArgs e)
    {
        ViewState["id"] = null;
        mpeIssueAttachment.Hide();
    }

    protected void gdvIssueAttachment_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvIssueAttachment.PageIndex = e.NewPageIndex;
        BindIssueAttachments();
    }

    protected void gdvIssueAttachment_Sorting(object sender, GridViewSortEventArgs e)
    {
        string SortDir = string.Empty;
        if (dirIssueAttachments == SortDirection.Ascending)
        {
            dirIssueAttachments = SortDirection.Descending;
            SortDir = "Desc";
        }
        else
        {
            dirIssueAttachments = SortDirection.Ascending;
            SortDir = "Asc";
        }
        DataView sortedView = new DataView(dtIssueAttachments);
        sortedView.Sort = e.SortExpression + " " + SortDir;
        gdvIssueAttachment.DataSource = sortedView;
        gdvIssueAttachment.DataBind();
    }

    protected void btnDeleteIssueAttachment_Command(object sender, CommandEventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectIssuesPath) && p.Delete.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للحذف');</script>", false);
            return;
        }
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            IssueAttachment c = db.IssueAttachments.Where(x => x.id == int.Parse(e.CommandArgument.ToString())).FirstOrDefault();
            try
            {
                if (File.Exists(Server.MapPath(c.fileName)))
                    File.Delete(Server.MapPath(c.fileName));
                db.IssueAttachments.DeleteOnSubmit(c);
                db.SubmitChanges();
                LogWriter.LogWrite("IssueAttachments", ((int)ActivitiesEnum.Delete).ToString(), c.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, EncryptString.Decrypt(Request.QueryString["id"]), "حذف مرفق (المشكلات)");
                BindIssueAttachments();
                BindData();
                mpeIssueAttachment.Show();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('تم الحذف بنجاح'); </script>", false);
            }
            catch (Exception ex)
            {
                Common.InsertException(ex.Message, ex.StackTrace,
                          new System.IO.FileInfo(System.Web.HttpContext.Current.Request.Url.AbsolutePath).Name);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('حدث خطا اثناء الحفظ');</script>", false);
            }
        }
    }

    protected void btnSaveIssueAttachment_Click(object sender, EventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectIssuesPath) && p.Add.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للاضافة');</script>", false);
            return;
        }
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            try
            {
                if (fuIssueAttachment.HasFile)
                {
                    string path = Server.MapPath("IssuesAttachments/");
                    while (File.Exists(path + fuIssueAttachment.FileName))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('يوجد مرفق بنفس الاسم');</script>", false);
                    }
                    fuIssueAttachment.SaveAs(path + fuIssueAttachment.FileName);
                    IssueAttachment q = new IssueAttachment();
                    q.fileName = "IssuesAttachments/" + fuIssueAttachment.FileName;
                    q.date = DateTime.Now;
                    q.issueId = int.Parse(ViewState["id"].ToString());
                    q.userId = UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID;
                    db.IssueAttachments.InsertOnSubmit(q);
                    db.SubmitChanges();
                    LogWriter.LogWrite("IssueAttachments", ((int)ActivitiesEnum.Add).ToString(), q.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, EncryptString.Decrypt(Request.QueryString["id"]), "اضافة مرفق (المشكلات)");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('تم الحفظ بنجاح');</script>", false);
                    BindIssueAttachments();
                    BindData();
                    mpeIssueAttachment.Show();
                }
            }
            catch (Exception ex)
            {
                Common.InsertException(ex.Message, ex.StackTrace,
                          new System.IO.FileInfo(System.Web.HttpContext.Current.Request.Url.AbsolutePath).Name);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('حدث خطا اثناء الحفظ');</script>", false);
            }
        }
    }
    private void BindIssueAttachments()
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            var query = from q in db.IssueAttachments
                        where q.issueId.Equals(int.Parse(ViewState["id"].ToString()))
                        select new
                        {
                            q.fileName,
                            q.id,
                            q.date
                        };
            dtIssueAttachments = query.CopyToDataTable();
            gdvIssueAttachment.DataSource = dtIssueAttachments;
            gdvIssueAttachment.DataBind();
        }
    }

    protected void lnkIssueAttachments_Command(object sender, CommandEventArgs e)
    {
        ViewState["id"] = e.CommandArgument;
        BindIssueAttachments();
        mpeIssueAttachment.Show();
    }
    #endregion
}