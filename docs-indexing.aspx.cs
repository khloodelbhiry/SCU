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

public partial class docs_indexing : System.Web.UI.Page
{
    private DataTable DtKeyword
    {
        get { return ((DataTable)ViewState["_dtKeyword"]); }
        set
        {
            if (value == null)
            {
                ViewState.Remove("_dtKeyword");
            }
            else
            {
                if (ViewState["_dtKeyword"] == null)
                {
                    ViewState["_dtKeyword"] = new DataTable();
                }
                ViewState["_dtKeyword"] = value;
                if (((DataTable)ViewState["_dtKeyword"]).Columns.Count == 0)
                {
                    ((DataTable)ViewState["_dtKeyword"]).Columns.Add("id", typeof(int));
                    ((DataTable)ViewState["_dtKeyword"]).Columns.Add("name");
                }
            }
        }
    }

    private DataTable DtReference
    {
        get { return ((DataTable)ViewState["_dtReference"]); }
        set
        {
            if (value == null)
            {
                ViewState.Remove("_dtReference");
            }
            else
            {
                if (ViewState["_dtReference"] == null)
                {
                    ViewState["_dtReference"] = new DataTable();
                }
                ViewState["_dtReference"] = value;
                if (((DataTable)ViewState["_dtReference"]).Columns.Count == 0)
                {
                    ((DataTable)ViewState["_dtReference"]).Columns.Add("id", typeof(int))
                        .AutoIncrement = true;
                    ((DataTable)ViewState["_dtReference"]).Columns.Add("typeId", typeof(int));
                    ((DataTable)ViewState["_dtReference"]).Columns.Add("type");
                    ((DataTable)ViewState["_dtReference"]).Columns.Add("description");
                    ((DataTable)ViewState["_dtReference"]).Columns.Add("code");
                }
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
            fuDoc.Attributes["style"] = "display:none";
            fuDoc.Attributes["onchange"] = "UploadFile(this)";
            HtmlAnchor lnkHistory = (HtmlAnchor)Master.FindControl("lnkHistory");
            lnkHistory.Visible = false;
            if (Session["User"] != null && Session["User"].ToString() != string.Empty)
            {
                if (UserPermissions.Any(
                    p =>
                        p.PageUrl.ToLower().Equals(Common.DocsIndexingPath) && p.Index.Equals(true)))
                {
                    var per = UserPermissions.FirstOrDefault(p => p.PageUrl.ToLower().Equals(Common.DocsIndexingPath));
                    ((HtmlGenericControl)Page.Master.FindControl("ulBreadcrumb")).InnerHtml = "<li><i class='ace-icon fa fa-home home-icon'></i><a href ='Default.aspx'> الرئيسية </a></li><li>" + per.ModuleName + "</li><li><a class='active'>" + per.PageName + "</a></li>";
                    Page.Title = per.PageName;
                }
                else
                    Response.Redirect("no-permission.aspx");
            }
            else
                Response.Redirect("Login.aspx?ReturnURL=" + Request.Url.AbsolutePath);
            DtReference = new DataTable();
            DtKeyword = new DataTable();
            BindControls();
            if (Request.QueryString["f"] != null)
                FillControls();
            else if (Request.QueryString["d"] != null)
                GetDocumentById();
            else
                GetNextDocument();
        }
    }
    private void BindControls()
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            var state = db.StatesTransitions.FirstOrDefault(x => x.moduleFormId == (int)ModuleFormsEnum.DocsIndexing);
            hdfDocsIndexing.Value = state.id.ToString();
            hdfNextState.Value = db.StatesTransitions.OrderBy(x => x.stateOrder).FirstOrDefault(x => x.stateOrder > state.stateOrder).id.ToString();
            var query1 = from q in db.Categories
                         where q.parentId == 0 && q.statusId == (int)StatusEnum.Approved
                         select new
                         {
                             q.id,
                             q.name
                         };
            ddlCategory.DataSource = query1;
            ddlCategory.DataTextField = "name";
            ddlCategory.DataValueField = "id";
            ddlCategory.DataBind();
            ddlCategory.Items.Insert(0, new ListItem("-- اختر --", "0"));
            ddlSubcategory.Items.Insert(0, new ListItem("-- اختر --", "0"));

            var query2 = from q in db.DocTypes
                         where q.statusId == (int)StatusEnum.Approved
                         select new
                         {
                             q.id,
                             q.name
                         };
            ddlDocType.DataSource = query2;
            ddlDocType.DataTextField = "name";
            ddlDocType.DataValueField = "id";
            ddlDocType.DataBind();
            ddlDocType.Items.Insert(0, new ListItem("-- اختر --", "0"));

            var query3 = from q in db.Parties
                         where q.statusId == (int)StatusEnum.Approved
                         select new
                         {
                             q.id,
                             q.name
                         };
            ddlSendingParty.DataSource = query3;
            ddlSendingParty.DataTextField = "name";
            ddlSendingParty.DataValueField = "id";
            ddlSendingParty.DataBind();
            ddlSendingParty.Items.Insert(0, new ListItem("-- اختر --", "0"));

            ddlReceivingParty.DataSource = query3;
            ddlReceivingParty.DataTextField = "name";
            ddlReceivingParty.DataValueField = "id";
            ddlReceivingParty.DataBind();
            ddlReceivingParty.Items.Insert(0, new ListItem("-- اختر --", "0"));

            var query4 = from q in db.BusinessReferences
                         where q.statusId == (int)StatusEnum.Approved
                         select new
                         {
                             q.id,
                             q.name
                         };
            ddlReference.DataSource = query4;
            ddlReference.DataTextField = "name";
            ddlReference.DataValueField = "id";
            ddlReference.DataBind();
            ddlReference.Items.Insert(0, new ListItem("-- اختر --", "0"));

            var query5 = (from q in db.Keywords
                          select new
                          {
                              q.id,
                              q.name
                          });
            ddlKeywords.DataSource = query5;
            ddlKeywords.DataTextField = "name";
            ddlKeywords.DataValueField = "id";
            ddlKeywords.DataBind();
            ddlKeywords.Items.Insert(0, new ListItem("-- اختر --", "0"));
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {

                using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
            {
                try
                {
                    FileDocument q = db.FileDocuments.FirstOrDefault(x => x.id.Equals(int.Parse(hdfDoc.Value)));
                    q.isIndexed = true;
                    q.receivingPartyId = ddlReceivingParty.SelectedValue != "0" ? (int?)int.Parse(ddlReceivingParty.SelectedValue) : null;
                    q.sendingPartyId = ddlSendingParty.SelectedValue != "0" ? (int?)int.Parse(ddlSendingParty.SelectedValue) : null;
                    q.categoryId = ddlSubcategory.SelectedValue != "0" ? (int?)int.Parse(ddlSubcategory.SelectedValue) : null;
                    q.docTypeId = ddlDocType.SelectedValue != "0" ? (int?)int.Parse(ddlDocType.SelectedValue) : null;
                    q.notes = txtNotes.Text;
                    q.reference = txtReference.Text.Trim();
                    q.title = txtTitle.Text.Trim();
                    if (txtDocumentDateYear.Text.Trim() != string.Empty && txtDocumentDateMonth.Text.Trim() != string.Empty && txtDocumentDateDay.Text.Trim() != string.Empty)
                        q.date = new DateTime(int.Parse(txtDocumentDateYear.Text.Trim()), int.Parse(txtDocumentDateMonth.Text.Trim()), int.Parse(txtDocumentDateDay.Text.Trim()));
                    for (int i = 0; i < DtKeyword.Rows.Count; i++)
                    {
                        FileDocumentKeyword k = new FileDocumentKeyword();
                        k.fileDocumentId = q.id;
                        k.keywordId = int.Parse(DtKeyword.Rows[i]["id"].ToString());
                        db.FileDocumentKeywords.InsertOnSubmit(k);
                    }
                    for (int i = 0; i < DtReference.Rows.Count; i++)
                    {
                        FileDocumentReference k = new FileDocumentReference();
                        k.fileDocumentId = q.id;
                        k.referenceId = int.Parse(DtReference.Rows[i]["typeId"].ToString());
                        k.description = DtReference.Rows[i]["description"].ToString();
                        k.code = DtReference.Rows[i]["code"].ToString();
                        db.FileDocumentReferences.InsertOnSubmit(k);
                    }
                    DocumentTransaction l = new DocumentTransaction();
                    l.stateTransitionId = int.Parse(hdfDocsIndexing.Value);
                    l.documentId = q.id;
                    l.actionTypeId = (int)ActionsEnum.Done;
                    l.date = DateTime.Now;
                    l.userId = UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID;
                    db.DocumentTransactions.InsertOnSubmit(l);
                    db.SubmitChanges();
                    if (Request.QueryString["f"] != null)
                        FillControls();
                    else if (Request.QueryString["d"] != null)
                        Response.Redirect("file-documents.aspx?b=" + Request.QueryString["b"] + "&id=" + Request.QueryString["id"] + "&c=" + Request.QueryString["c"] + "&g=" + Request.QueryString["g"]);
                    else
                        GetNextDocument();
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
    private void BindDocument(FileDocument query)
    {
        hdfDoc.Value = query.id.ToString();
        txtDocCode.Text = query.barcode;
        txtNotes.Text = query.notes;
        txtReference.Text = query.reference;
        txtTitle.Text = query.title;
        txtFileTitle.Text = query.BoxFile.title;
        txtFileReference.Text = query.BoxFile.reference;
        if (query.date != null)
        {
            txtDocumentDateDay.Text = query.date.Value.Day.ToString().PadLeft(2, '0');
            txtDocumentDateMonth.Text = query.date.Value.Month.ToString().PadLeft(2, '0');
            txtDocumentDateYear.Text = query.date.Value.Year.ToString();
        }
        ddlDocType.SelectedValue = query.docTypeId != null ? query.docTypeId.ToString() : "0";
        ddlReceivingParty.SelectedValue = query.receivingPartyId != null ? query.receivingPartyId.ToString() : "0";
        ddlSendingParty.SelectedValue = query.sendingPartyId != null ? query.sendingPartyId.ToString() : "0";
        if (query.categoryId != null)
        {
            ddlCategory.SelectedValue = query.Category.parentId.ToString();
            ddlCategory_SelectedIndexChanged(ddlCategory, new EventArgs());
            ddlSubcategory.SelectedValue = query.categoryId.ToString();
        }
        string embed = "<object data = '{0}#toolbar=1'type = 'application/pdf' width = '100%' height = '700px' ></ object >";
        string path = "http://192.168.28.248/scu/PDF/" + query.BoxFile.UnitStructure.code.ToString().PadLeft((int)CodesLengthEnum.Unit, '0') + "/" + query.BoxFile.code.ToString().PadLeft((int)CodesLengthEnum.File, '0') + "/" + query.barcode + ".pdf?" + DateTime.Now;
        ltrPDFEmbed.Text = string.Format(embed, path);
    }
    private void GetNextDocument()
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            try
            {
                ClearControls();
                if (hdfFile.Value == null || hdfFile.Value == string.Empty)
                {
                    GetUserFile();
                }
                var query = db.FileDocuments.Where(t => t.fileId.Equals(int.Parse(hdfFile.Value))
                             && (t.isIndexed ?? false) == false).FirstOrDefault();
                if (query != null)
                {
                    BindDocument(query);
                }
                else
                {
                    BoxFile q = db.BoxFiles.FirstOrDefault(x => x.id.Equals(int.Parse(hdfFile.Value)));
                    if (q != null)
                    {
                        q.nextStateId = int.Parse(hdfNextState.Value);
                        FileTransaction l = new FileTransaction();
                        l.stateTransitionId = int.Parse(hdfDocsIndexing.Value);
                        l.fileId = q.id;
                        l.actionTypeId = (int)ActionsEnum.Done;
                        l.date = DateTime.Now;
                        l.userId = UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID;
                        db.FileTransactions.InsertOnSubmit(l);
                        db.SubmitChanges();
                        hdfFile.Value = null;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('تمت فهرسة وثائق الملف');</script>", false);
                        GetNextDocument();
                    }
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
    private void GetDocumentById()
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            try
            {
                ClearControls();
                var query = db.FileDocuments.Where(t => t.id.Equals(int.Parse(EncryptString.Decrypt(Request.QueryString["d"])))).FirstOrDefault();
                if (query != null)
                {
                    BindDocument(query);
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
    private void FillControls()
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            try
            {
                ClearControls();
                var query = db.FileDocuments.Where(t => t.fileId.Equals(int.Parse(Request.QueryString["f"]))
                             && (t.isIndexed ?? false) == false).FirstOrDefault();
                if (query != null)
                {
                    BindDocument(query);
                }
                else
                {
                    BoxFile q = db.BoxFiles.FirstOrDefault(x => x.id.Equals(int.Parse(Request.QueryString["f"])));
                    if (q != null)
                    {
                        q.nextStateId = int.Parse(hdfNextState.Value);
                        FileTransaction l = new FileTransaction();
                        l.stateTransitionId = int.Parse(hdfDocsIndexing.Value);
                        l.fileId = q.id;
                        l.actionTypeId = (int)ActionsEnum.Done;
                        l.date = DateTime.Now;
                        l.userId = UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID;
                        db.FileTransactions.InsertOnSubmit(l);
                        db.SubmitChanges();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('تم الحفظ بنجاح');location.href='project-files.aspx?id=" + Request.QueryString["id"] + "&g=" + Request.QueryString["g"] + "&c=" + Request.QueryString["c"] + "';</script>", false);
                    }
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
    private void GetUserFile()
    {
        divControls.Visible = true;
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            int batchId = 0;
            var assignedBatch = (from i in db.BoxFiles
                                 where i.nextStateId==int.Parse(hdfDocsIndexing.Value) &&
                                 i.indexedBy.Equals(UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID)
                               //  && i.UnitStructure.GovernmentalEntity.id== UserDetails.DeSerializeUserDetails(Session["User"].ToString()).GovernmentalEntityId
                                 select new { i.id }).FirstOrDefault();
            if (assignedBatch != null)
            {
                batchId = assignedBatch.id;
            }
            else
            {
                var newBatch = (from i in db.BoxFiles
                                where i.nextStateId == int.Parse(hdfDocsIndexing.Value)
                               //   && i.UnitStructure.GovernmentalEntity.id == UserDetails.DeSerializeUserDetails(Session["User"].ToString()).GovernmentalEntityId
                               && i.indexedBy == null
                                select i).FirstOrDefault();
                if (newBatch != null)
                {
                    batchId = newBatch.id;
                    newBatch.indexedBy = UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID;
                    db.SubmitChanges();
                }
                else
                {
                    divControls.Visible = false;
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alertUser",
                        "alert('لا يوجد وثائق للفهرسة');", true);
                }
            }
            hdfFile.Value = batchId.ToString();
            if (batchId == 0)
            {
                divControls.Visible = false;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alertUser",
                    "alert('لا يوجد وثائق للفهرسة');", true);
                return;
            }
        }
    }

    private void ClearControls()
    {
        DtReference.Rows.Clear();
        DtKeyword.Rows.Clear();
        gdvKeywords.DataSource = DtKeyword;
        gdvKeywords.DataBind();
        gdvReferences.DataSource = DtReference;
        gdvReferences.DataBind();
        txtFileReference.Text = txtFileTitle.Text = txtDocumentDateDay.Text = txtDocumentDateMonth.Text = txtDocumentDateYear.Text = string.Empty;
        //txtDocCode.Text =  txtNotes.Text = txtReference.Text = txtTitle.Text = string.Empty;
        //ddlCategory.SelectedValue = "0";
        ddlDocType.SelectedValue = ddlReceivingParty.SelectedValue = ddlSendingParty.SelectedValue = "0";
        //ddlCategory_SelectedIndexChanged(ddlCategory, new EventArgs());
    }
    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            var query1 = from q in db.Categories
                         where q.parentId.Equals(int.Parse(ddlCategory.SelectedValue))
                         && q.parentId != 0 && q.statusId == (int)StatusEnum.Approved
                         select new
                         {
                             q.id,
                             q.name
                         };
            ddlSubcategory.DataSource = query1;
            ddlSubcategory.DataTextField = "name";
            ddlSubcategory.DataValueField = "id";
            ddlSubcategory.DataBind();
            ddlSubcategory.Items.Insert(0, new ListItem("-- اختر --", "0"));
        }
    }

    protected void btnDeleteKeyword_Command(object sender, CommandEventArgs e)
    {
        var rows = DtKeyword.Select("id = " + int.Parse(e.CommandArgument.ToString()));
        foreach (var row in rows)
            row.Delete();
        DtKeyword.AcceptChanges();
        gdvKeywords.DataSource = DtKeyword;
        gdvKeywords.DataBind();
    }

    protected void btnAddReference_Click(object sender, EventArgs e)
    {
        DtReference.Rows.Add(null, int.Parse(ddlReference.SelectedValue), ddlReference.SelectedItem.Text, txtDescription.Text, txtReferenceCode.Text);
        txtDescription.Text = txtReferenceCode.Text = string.Empty;
        ddlReference.SelectedValue = "0";
        gdvReferences.DataSource = DtReference;
        gdvReferences.DataBind();
    }

    protected void btnDeleteReference_Command(object sender, CommandEventArgs e)
    {
        var rows = DtReference.Select("id = " + int.Parse(e.CommandArgument.ToString()) + "");
        foreach (var row in rows)
            row.Delete();
        gdvReferences.DataSource = DtReference;
        gdvReferences.DataBind();
    }

    protected void lnkPrintBarcode_Click(object sender, EventArgs e)
    {
        var uriReportSource = new Telerik.Reporting.UriReportSource { Uri = Server.MapPath("doc.trdp") };
        uriReportSource.Parameters.Add(new Telerik.Reporting.Parameter("barcode", txtDocCode.Text));
        ReportViewer1.ReportSource = uriReportSource;
        ScriptManager.RegisterStartupScript(this, GetType(), "alertUser", "fnPrintReport();", true);
    }

    protected void lnkScan_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "alertUser", "alert('لا يوجد ماسح ضوئي متصل');", true);
    }
    protected void lnkSubmitUpload_Click(object sender, EventArgs e)
    {
        if (fuDoc.HasFile)
        {
            try
            {
                string fileName = Server.MapPath("Docs/") + txtDocCode.Text + ".pdf";
                fuDoc.PostedFile.SaveAs(fileName);
                string embed = "<object data = '{0}#toolbar=1'type = 'application/pdf' width = '100%' height = '700px' ></ object >";
                string path = "http://localhost/SCU/Docs/" + txtDocCode.Text + ".pdf";
                ltrPDFEmbed.Text = string.Format(embed, path);
                lnkUpload.Visible = lnkScan.Visible = lnkPrintBarcode.Visible = false;
            }
            catch (Exception ex) { }
        }
    }

    protected void lnkUpload_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "alertUser", "initFileUpload();", true);
        //fuDoc.Attributes["style"] = "display:block";
    }

    protected void lnkCloseKeywordModal_Click(object sender, EventArgs e)
    {
        mpeKeyword.Hide();
        txtKeyword.Text = string.Empty;
    }

    protected void lnkSaveKeyword_Click(object sender, EventArgs e)
    {
        var rows = DtKeyword.Select("name = '" + txtKeyword.Text + "'");
        if (rows != null && rows.Count() > 0)
        {
            txtKeyword.Text = string.Empty;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('موجوده من قبل');</script>", false);
            return;
        }
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            Keyword keyword = db.Keywords.FirstOrDefault(x => x.name == txtKeyword.Text.Trim());
            if (keyword == null)
            {
                Keyword k = new Keyword();
                k.name = txtKeyword.Text.Trim();
                db.Keywords.InsertOnSubmit(k);
                db.SubmitChanges();
                ddlKeywords.Items.Insert(ddlKeywords.Items.Count, new ListItem(txtKeyword.Text.Trim(), k.id.ToString()));
                DtKeyword.Rows.Add(k.id, txtKeyword.Text);
            }
            else
                DtKeyword.Rows.Add(keyword.id, txtKeyword.Text);
            txtKeyword.Text = string.Empty;
            gdvKeywords.DataSource = DtKeyword;
            gdvKeywords.DataBind();
        }
    }

    protected void ddlKeywords_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlKeywords.SelectedValue != "0")
        {
            var rows = DtKeyword.Select("id = " + int.Parse(ddlKeywords.SelectedValue));
            if (rows != null && rows.Count() > 0)
            {
                ddlKeywords.SelectedValue = "0";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('موجوده من قبل');</script>", false);
                return;
            }
            DtKeyword.Rows.Add(int.Parse(ddlKeywords.SelectedValue), ddlKeywords.SelectedItem.Text); 
            gdvKeywords.DataSource = DtKeyword;
            gdvKeywords.DataBind();
            ddlKeywords.SelectedValue = "0";
        }
    }

    protected void lnkAddKeyword_Click(object sender, EventArgs e)
    {
        mpeKeyword.Show();
    }
}