using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class file_documents : System.Web.UI.Page
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
    public SortDirection dirStatus
    {
        get
        {
            if (ViewState["dirStateStatus"] == null)
            {
                ViewState["dirStateStatus"] = SortDirection.Ascending;
            }
            return (SortDirection)ViewState["dirStateStatus"];
        }
        set
        {
            ViewState["dirStateStatus"] = value;
        }
    }
    public SortDirection dirUsers
    {
        get
        {
            if (ViewState["dirUsers"] == null)
            {
                ViewState["dirUsers"] = SortDirection.Ascending;
            }
            return (SortDirection)ViewState["dirUsers"];
        }
        set
        {
            ViewState["dirUsers"] = value;
        }
    }
    public SortDirection dirIssues
    {
        get
        {
            if (ViewState["dirIssues"] == null)
            {
                ViewState["dirIssues"] = SortDirection.Ascending;
            }
            return (SortDirection)ViewState["dirIssues"];
        }
        set
        {
            ViewState["dirIssues"] = value;
        }
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

    private DataTable dtStatus
    {
        get
        {
            return ((DataTable)ViewState["_dtStatus"]);
        }
        set
        {
            if (value == null)
            {
                ViewState.Remove("_dtStatus");
            }
            else
            {
                ViewState["_dtStatus"] = value;
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
            lnkHistory.Visible = false;
            if (Session["User"] != null && Session["User"].ToString() != string.Empty)
            {
                if (UserPermissions.Any(
                    p =>
                        p.PageUrl.ToLower().Equals(Common.FileDocumentsPath) && (p.Show.Equals(true) || p.Add.Equals(true) || p.Print.Equals(true))))
                {
                    if (Request.QueryString["id"] != null && Request.QueryString["g"] != null)
                    {
                        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
                        {
                            var per = UserPermissions.FirstOrDefault(p => p.PageUrl.ToLower().Equals(Common.FileDocumentsPath));
                            Project query = db.Projects.FirstOrDefault(x => x.id == int.Parse(EncryptString.Decrypt(Request.QueryString["id"])));
                            ((HtmlGenericControl)Page.Master.FindControl("ulBreadcrumb")).InnerHtml = "<li><i class='ace-icon fa fa-home home-icon'></i><a href ='Default.aspx'> الرئيسية </a></li><li>" + per.ModuleName + "</li><li><strong>المشروع : " + query.name + "</strong></li><li><strong>الجهة : " + query.GovernmentalEntity.name + "</strong></li><li><strong>الشركة المنفذة : " + query.Company.name + "</strong></li><li><a class='active'>" + per.PageName + "</a></li>";
                            Page.Title = per.PageName;
                        }
                    }
                    else
                        Response.Redirect("projects.aspx");
                }
                else
                    Response.Redirect("no-permission.aspx");
            }
            else
                Response.Redirect("Login.aspx?ReturnURL=" + Request.Url.AbsolutePath);
            if (UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.FileDocumentsPath) && (p.Show.Equals(true) || p.Print.Equals(true))))
            {
                if (Request.QueryString["b"] != null)
                {
                    using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
                    {
                        BoxFile box = db.BoxFiles.FirstOrDefault(x => x.id == int.Parse(EncryptString.Decrypt(Request.QueryString["b"].ToString())));
                        if (box != null)
                        {
                            lblFileBarcode.Text = box.barcode;
                            lblFileReference.Text = box.reference;
                            lblFileTitle.Text = box.title;
                            lblUnit.Text = box.UnitStructure.name;
                        }
                    }
                    if (UserDetails.DeSerializeUserDetails(Session["User"].ToString()).CompanyId == 0)
                    {
                        gdvData.Columns[2].Visible = false;
                        gdvData.Columns[4].Visible = false;
                    }
                    BindData();
                }
                else
                    Response.Redirect("project-files.aspx?id=" + Request.QueryString["id"] + "&g=" + Request.QueryString["g"] + "&c=" + Request.QueryString["c"]);
            }
            lnkBack.PostBackUrl = "project-files.aspx?id=" + Request.QueryString["id"] + "&g=" + Request.QueryString["g"] + "&c=" + Request.QueryString["c"];
        }
    }
    private void BindData()
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            var query = (from b in db.FileDocuments
                         where b.fileId.Equals(int.Parse(EncryptString.Decrypt(Request.QueryString["b"].ToString())))
                         select new
                         {
                             b.barcode,
                             b.id,
                             b.noOfPages,
                             box = b.BoxFile.Box.barcode,
                             batch = b.BoxFile.Batch.barcode,
                             b.fileId,
                             b.BoxFile.nextStateId,
                             isIndexed=b.isIndexed??false,
                             isReviewed=b.isReviewed??false,
                             unit=b.BoxFile.UnitStructure.name,
                             state = UserDetails.DeSerializeUserDetails(Session["User"].ToString()).CompanyId != 0?( b.BoxFile.nextStateId != null ? (b.BoxFile.nextStateId == (int)StatesEnum.DocumentsIndexing ? ((b.isIndexed ?? false) == false ? "الفهرسة" : "تمت فهرسة الوثيقة") : (b.BoxFile.nextStateId == (int)StatesEnum.DocumentsQa ? ((b.isReviewed ?? false) == false ? "المراجعة" : "تمت المراجعة") : b.BoxFile.StatesTransition.name)) : "تم " + db.FileTransactions.Where(x => x.fileId == b.fileId).OrderByDescending(x => x.id).FirstOrDefault().StatesTransition.name):((b.isIndexed ?? false) == false?"قيد الفهرسة":((b.isReviewed??false)==false?"قيد المراجعة":"تمت المراجعة")),
                         });
            if (txtBarcodeSrc.Text.Trim() != string.Empty)
                query = query.Where(x => x.barcode == txtBarcodeSrc.Text.Trim());
            dtData = query.CopyToDataTable();
            gdvData.DataSource = dtData;
            gdvData.DataBind();
            lblResult.Text = dtData.Rows.Count.ToString();
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
    protected void lnkPrint_Command(object sender, CommandEventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.FileDocumentsPath) && p.Print.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للطباعة');</script>", false);
            return;
        }
        var uriReportSource = new Telerik.Reporting.UriReportSource { Uri = Server.MapPath("rptDocument.trdp") };
        uriReportSource.Parameters.Add(new Telerik.Reporting.Parameter("pBarcode", e.CommandArgument.ToString()));
        ReportViewer1.ReportSource = uriReportSource;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'>fnPrintReport();</script>", false);
    }
    protected void btnSaveDocuments_Click(object sender, EventArgs e)
    {
        var reportBook = new Telerik.Reporting.ReportBook();
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            int nextDoc = 1;
            BoxFile box = db.BoxFiles.FirstOrDefault(x => x.id == int.Parse(EncryptString.Decrypt(Request.QueryString["b"].ToString())));
            FileDocument query = db.FileDocuments.Where(x => x.fileId.Equals(box.id)).OrderByDescending(x => x.code).FirstOrDefault();
            if (query != null)
                nextDoc = query.code + 1;
            for (int i = 0; i < int.Parse(txtDocumentsCount.Text); i++)
            {
                string barcode = box.barcode.Substring(0, box.barcode.Length - (int)CodesLengthEnum.Doc) + nextDoc.ToString().PadLeft((int)CodesLengthEnum.Doc, '0');
                FileDocument d = new FileDocument();
                d.fileId = box.id;
                d.code = nextDoc;
                d.barcode = barcode;
                db.FileDocuments.InsertOnSubmit(d);
                db.SubmitChanges();
                for (int j = 0; j < int.Parse(txtCopies.Text); j++)
                {
                    var uriReportSource = new Telerik.Reporting.UriReportSource { Uri = Server.MapPath("document.trdp") };
                    uriReportSource.Parameters.Add(new Telerik.Reporting.Parameter("barcode", barcode));
                    reportBook.ReportSources.Add(uriReportSource);
                }
                nextDoc++;
            }
        }
        ReportViewer1.ReportSource = reportBook;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'>fnPrintReport();</script>", false);
        BindData();
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('تم انشاء الوثائق بنجاح');</script>", false);
        txtDocumentsCount.Text = string.Empty;
        txtCopies.Text = "1";
        mpeCreateDocuments.Hide();
    }
    protected void btnCloseDocuments_Click(object sender, EventArgs e)
    {
        txtDocumentsCount.Text = string.Empty;
        mpeCreateDocuments.Hide();
    }

    protected void lnkBoxFiles_Click(object sender, EventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.FileDocumentsPath) && p.Add.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للاضافة');</script>", false);
            return;
        }
        mpeCreateDocuments.Show();
    }
    protected void lnkLog_Command(object sender, CommandEventArgs e)
    {
        if (UserDetails.DeSerializeUserDetails(Session["User"].ToString()).CompanyId != 0)
        {
            ViewState["id"] = e.CommandArgument.ToString();
            BindLog();
        }
        else
        {
            using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
            {
                FileDocument d = db.FileDocuments.FirstOrDefault(x => x.id == int.Parse(e.CommandArgument.ToString().Split(';')[0]));
                if ((d.isIndexed ?? false) == false)
                    Response.Redirect("docs-indexing.aspx?b=" + Request.QueryString["b"] + "&id=" + Request.QueryString["id"] + "&c=" + Request.QueryString["c"] + "&g=" + Request.QueryString["g"] + "&d=" +EncryptString.Encrypt(e.CommandArgument.ToString().Split(';')[0]));
                else if ((d.isReviewed ?? false) == false)
                    Response.Redirect("docs-qa.aspx?b=" + Request.QueryString["b"] + "&id=" + Request.QueryString["id"] + "&c=" + Request.QueryString["c"] + "&g=" + Request.QueryString["g"] + "&d=" + EncryptString.Encrypt(e.CommandArgument.ToString().Split(';')[0]));
                else
                    Response.Redirect("doc-details.aspx?id=" +EncryptString.Encrypt(e.CommandArgument.ToString().Split(';')[0]));
            }
        }
    }
    private void BindLog()
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            var query = from x in db.FileTransactions
                         where x.fileId.Equals(int.Parse(ViewState["id"].ToString().Split(';')[1]))
                         select new
                         {
                             name = x.ActionsType.name + " " + x.StatesTransition.name,
                             x.User.fullName,
                             x.date,
                             actionTypeId=x.actionTypeId??0,
                             x.StatesTransition.stateOrder
                         };
            var doc = from x in db.DocumentTransactions
                      where x.documentId == int.Parse(ViewState["id"].ToString().Split(';')[0])
                      select new
                      {
                          name=x.ActionsType.name + " " + x.StatesTransition.name.Replace("الوثائق","الوثيقة"),
                          x.User.fullName,
                          x.date,
                          x.actionTypeId,
                          x.StatesTransition.stateOrder
                      };
            dtStatus = query.Union(doc).OrderBy(x=>x.date).CopyToDataTable();
            gdvStatus.DataSource = dtStatus;
            gdvStatus.DataBind();
            mpeLog.Show();
        }
    }
    protected void gdvStatus_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HiddenField hdfAction = (HiddenField)e.Row.FindControl("hdfAction");
            if (hdfAction != null && int.Parse(hdfAction.Value) == (int)ActionsEnum.Restart)
            {
                e.Row.BackColor = Color.PaleVioletRed;
            }
        }
    }
    protected void btnClose_Click(object sender, EventArgs e)
    {
        ViewState["id"] = null;
        mpeLog.Hide();
    }
    protected void gdvStatus_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvStatus.PageIndex = e.NewPageIndex;
        BindLog();
    }
    protected void gdvStatus_Sorting(object sender, GridViewSortEventArgs e)
    {
        string SortDir = string.Empty;
        if (dirStatus == SortDirection.Ascending)
        {
            dirStatus = SortDirection.Descending;
            SortDir = "Desc";
        }
        else
        {
            dirStatus = SortDirection.Ascending;
            SortDir = "Asc";
        }
        DataView sortedView = new DataView(dtStatus);
        sortedView.Sort = e.SortExpression + " " + SortDir;
        gdvStatus.DataSource = sortedView;
        gdvStatus.DataBind();
    }
    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        BindData();
    }
    protected void lnkClearSearch_Click(object sender, EventArgs e)
    {
        txtBarcodeSrc.Text = string.Empty;
        BindData();
    }
    protected void txtBarcodeSrc_TextChanged(object sender, EventArgs e)
    {
        BindData();
    }
    public bool CheckVisible(int state, int action)
    {
        return StateTransitionPermissions.Any(
                 p => p.StateID.Equals(state) &&
                 (action == (int)ActionsEnum.Done ? p.Done.Equals(true) : p.Restart.Equals(true)));
    }

    protected void lnkChangeStatus_Command(object sender, CommandEventArgs e)
    {
        string path = string.Empty;
        if (int.Parse(e.CommandArgument.ToString().Split(';')[1]) == (int)StatesEnum.DocumentsIndexing)
            path = "docs-indexing";
        else
            path = "docs-qa";
        path += ".aspx?d=" + EncryptString.Encrypt(e.CommandArgument.ToString().Split(';')[0]) + "&b=" + Request.QueryString["b"] + "&id=" + Request.QueryString["id"] + "&c=" + Request.QueryString["c"] + "&g=" + Request.QueryString["g"];
        Response.Redirect(path);
    }
}