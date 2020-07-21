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

public partial class docs_qa : System.Web.UI.Page
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
                    ((DataTable)ViewState["_dtKeyword"]).Columns.Add("id", typeof(int))
                        .AutoIncrement = true;
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
            HtmlAnchor lnkHistory = (HtmlAnchor)Master.FindControl("lnkHistory");
            lnkHistory.Visible = false;
            //if (Session["User"] != null && Session["User"].ToString() != string.Empty)
            //{
            //    if (UserPermissions.Any(
            //        p =>
            //            p.PageUrl.ToLower().Equals(Common.DocsQaPath) && p.Qa.Equals(true)))
            //    {
            //        var per = UserPermissions.FirstOrDefault(p => p.PageUrl.ToLower().Equals(Common.DocsQaPath));
            //        ((HtmlGenericControl)Page.Master.FindControl("ulBreadcrumb")).InnerHtml = "<li><i class='ace-icon fa fa-home home-icon'></i><a href ='Default.aspx'> الرئيسية </a></li><li>" + per.ModuleName + "</li><li><a class='active'>" + per.PageName + "</a></li>";
            //        Page.Title = per.PageName;
            //    }
            //    else
            //        Response.Redirect("no-permission.aspx");
            //}
            //else
            //    Response.Redirect("Login.aspx?ReturnURL=" + Request.Url.AbsolutePath);
            ((HtmlGenericControl)Page.Master.FindControl("ulBreadcrumb")).InnerHtml = "<li><i class='ace-icon fa fa-home home-icon'></i><a href ='Default.aspx'> الرئيسية </a></li><li>الأرشفة اليومية</li><li><a class='active'>تفاصيل الوثيقة</a></li>";
            Page.Title = "تفاصيل الوثيقة";
            DtReference = new DataTable();
            DtKeyword = new DataTable();
            BindControls();
            if (Request.QueryString["id"] != null)
                GetDocumentById();
        }
    }
    private void BindControls()
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            var state = db.StatesTransitions.FirstOrDefault(x => x.moduleFormId == (int)ModuleFormsEnum.DocsQa);
            hdfDocsQa.Value = state.id.ToString();
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
                         where q.Project.governmentalEntityId == UserDetails.DeSerializeUserDetails(Session["User"].ToString()).GovernmentalEntityId
                       && q.statusId == (int)StatusEnum.Approved
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
        }
    }
    private void GetDocumentById()
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            try
            {
                ClearControls();
                var query = db.FileDocuments.Where(t => t.id.Equals(int.Parse(EncryptString.Decrypt(Request.QueryString["id"])))).FirstOrDefault();
                if (query != null)
                {
                    hdfDoc.Value = query.id.ToString();
                    txtFileCode.Text = query.BoxFile.barcode;
                    txtFileTitle.Text = query.BoxFile.title;
                    txtFileReference.Text = query.BoxFile.reference;
                    txtUnitStructure.Text = query.BoxFile.UnitStructure.name;
                    txtDocCode.Text = query.barcode;
                    txtNotes.Text = query.notes;
                    txtReference.Text = query.reference;
                    txtTitle.Text = query.title;
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
                    var refe = from r in db.FileDocumentReferences
                               where r.fileDocumentId == query.id
                               select new
                               {
                                   id = r.id,
                                   typeId = r.BusinessReference.id,
                                   type = r.BusinessReference.name,
                                   description = r.description,
                                   code = r.code
                               };
                    divReferences.Visible = refe.Any();
                    DtReference = refe.CopyToDataTable();
                    gdvReferences.DataSource = DtReference;
                    gdvReferences.DataBind();
                    var keyw = from k in db.FileDocumentKeywords
                               where k.fileDocumentId == query.id
                               select new
                               {
                                   id = k.id,
                                   name = k.keyword
                               };
                    divKeywords.Visible = keyw.Any();
                    DtKeyword = keyw.CopyToDataTable();
                    gdvKeywords.DataSource = DtKeyword;
                    gdvKeywords.DataBind();
                    string embed = "<object data = '{0}#toolbar=1'type = 'application/pdf' width = '100%' height = '700px' ></ object >";
                    string path = "http://localhost/SCU/Docs/" + query.barcode + ".pdf?" + DateTime.Now;
                    ltrPDFEmbed.Text = string.Format(embed, path);
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
    private void GetNextDocument()
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            try
            {
                ClearControls();
                var query = db.FileDocuments.Where(t => t.fileId.Equals(int.Parse(hdfFile.Value))).FirstOrDefault();
                if (query != null)
                {
                    hdfDoc.Value = query.id.ToString();
                    txtFileCode.Text = query.BoxFile.barcode;
                    txtFileTitle.Text = query.BoxFile.title;
                    txtFileReference.Text = query.BoxFile.reference;
                    txtDocCode.Text = query.barcode;
                    txtUnitStructure.Text= query.BoxFile.UnitStructure.name;
                    txtNotes.Text = query.notes;
                    txtReference.Text = query.reference;
                    txtTitle.Text = query.title;
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
                    var refe = from r in db.FileDocumentReferences
                               where r.fileDocumentId == query.id
                               select new
                               {
                                   id = r.id,
                                   typeId = r.BusinessReference.id,
                                   type = r.BusinessReference.name,
                                   description = r.description,
                                   code = r.code
                               };
                    DtReference = refe.CopyToDataTable();
                    gdvReferences.DataSource = DtReference;
                    gdvReferences.DataBind();
                    var keyw = from k in db.FileDocumentKeywords
                               where k.fileDocumentId == query.id
                               select new
                               {
                                   id = k.id,
                                   name = k.keyword
                               };
                    DtKeyword = keyw.CopyToDataTable();
                    gdvKeywords.DataSource = DtKeyword;
                    gdvKeywords.DataBind();
                    string embed = "<object data = '{0}#toolbar=1'type = 'application/pdf' width = '100%' height = '700px' ></ object >";
                    string path = "http://10.1.20.25/SCU/boxfile-certificate/%23" + query.barcode + "%23.pdf?" + DateTime.Now;
                    ltrPDFEmbed.Text = string.Format(embed, path);
                }
                else
                {
                    BoxFile q = db.BoxFiles.FirstOrDefault(x => x.id.Equals(int.Parse(hdfFile.Value)));
                    if (q != null)
                    {
                        q.nextStateId = int.Parse(hdfNextState.Value);
                        FileTransaction l = new FileTransaction();
                        l.stateTransitionId = int.Parse(hdfDocsQa.Value);
                        l.fileId = q.id;
                        l.actionTypeId = (int)ActionsEnum.Done;
                        l.date = DateTime.Now;
                        l.userId = UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID;
                        db.FileTransactions.InsertOnSubmit(l);
                        db.SubmitChanges();
                        hdfFile.Value = null;
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
    private void FillControls()
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            try
            {
                ClearControls();
                var query = db.FileDocuments.Where(t => t.fileId.Equals(int.Parse(Request.QueryString["id"]))
                             && (t.isReviewed ?? false) == false).FirstOrDefault();
                if (query != null)
                {
                    hdfDoc.Value = query.id.ToString();
                    txtDocCode.Text = query.barcode;
                    txtNotes.Text = query.notes;
                    txtReference.Text = query.reference;
                    txtTitle.Text = query.title;
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
                    var refe = from r in db.FileDocumentReferences
                               where r.fileDocumentId == query.id
                               select new
                               {
                                   id = r.id,
                                   typeId = r.BusinessReference.id,
                                   type = r.BusinessReference.name,
                                   description = r.description,
                                   code = r.code
                               };
                    DtReference = refe.CopyToDataTable();
                    gdvReferences.DataSource = DtReference;
                    gdvReferences.DataBind();
                    var keyw = from k in db.FileDocumentKeywords
                               where k.fileDocumentId == query.id
                               select new
                               {
                                   id = k.id,
                                   name = k.keyword
                               };
                    DtKeyword = keyw.CopyToDataTable();
                    gdvKeywords.DataSource = DtKeyword;
                    gdvKeywords.DataBind();
                    string embed = "<object data = '{0}#toolbar=1'type = 'application/pdf' width = '100%' height = '700px' ></ object >";
                    string path = "http://10.1.20.25/SCU/boxfile-certificate/%23" + query.barcode + "%23.pdf?" + DateTime.Now;
                    ltrPDFEmbed.Text = string.Format(embed, path);
                }
                else
                {
                    BoxFile q = db.BoxFiles.FirstOrDefault(x => x.id.Equals(int.Parse(Request.QueryString["f"])));
                    if (q != null)
                    {
                        q.nextStateId = int.Parse(hdfNextState.Value);
                        FileTransaction l = new FileTransaction();
                        l.stateTransitionId = int.Parse(hdfDocsQa.Value);
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
                                 where i.nextStateId == int.Parse(hdfDocsQa.Value) &&
                                 i.qaBy.Equals(UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID)
                               && i.UnitStructure.GovernmentalEntity.id == UserDetails.DeSerializeUserDetails(Session["User"].ToString()).GovernmentalEntityId
                                 select new { i.id, i.unitStructureId }).FirstOrDefault();
            if (assignedBatch != null)
            {
                batchId = assignedBatch.id;
                hdfUnit.Value = assignedBatch.unitStructureId.ToString();
            }
            else
            {
                var newBatch = (from i in db.BoxFiles
                                where i.nextStateId == int.Parse(hdfDocsQa.Value)
                             && i.UnitStructure.GovernmentalEntity.id == UserDetails.DeSerializeUserDetails(Session["User"].ToString()).GovernmentalEntityId
                                    && i.qaBy == null
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
                        "alert('لا يوجد وثائق للمراجعة');", true);
                }
            }
            hdfFile.Value = batchId.ToString();
            if (batchId == 0)
            {
                divControls.Visible = false;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alertUser",
                    "alert('لا يوجد وثائق للمراجعة');", true);
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
        txtDocCode.Text = txtDocumentDateDay.Text = txtDocumentDateMonth.Text = txtDocumentDateYear.Text = txtNotes.Text = txtReference.Text = txtTitle.Text = string.Empty;
        ddlCategory.SelectedValue = ddlDocType.SelectedValue = ddlReceivingParty.SelectedValue = ddlSendingParty.SelectedValue = "0";
        ddlCategory_SelectedIndexChanged(ddlCategory, new EventArgs());
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
        var rows = DtKeyword.Select("id = " + int.Parse(e.CommandArgument.ToString()) + "");
        foreach (var row in rows)
            row.Delete();
        gdvKeywords.DataSource = DtKeyword;
        gdvKeywords.DataBind();
    }
}