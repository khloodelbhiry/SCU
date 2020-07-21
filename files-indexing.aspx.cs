using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class files_indexing : System.Web.UI.Page
{
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
                        p.PageUrl.ToLower().Equals(Common.FilesIndexingPath) && p.Index.Equals(true)))
                {
                    var per = UserPermissions.FirstOrDefault(p => p.PageUrl.ToLower().Equals(Common.FilesIndexingPath));
                    ((HtmlGenericControl)Page.Master.FindControl("ulBreadcrumb")).InnerHtml = "<li><i class='ace-icon fa fa-home home-icon'></i><a href ='Default.aspx'> الرئيسية </a></li><li>" + per.ModuleName + "</li><li><a class='active'>" + per.PageName + "</a></li>";
                    Page.Title = per.PageName;
                }
                else
                    Response.Redirect("no-permission.aspx");
            }
            else
                Response.Redirect("Login.aspx?ReturnURL=" + Request.Url.AbsolutePath);
            BindControls();
            if (Request.QueryString["f"] != null)
                FillControls();
            else
                GetNextFile();
        }
    }
    private void BindControls()
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            var state = db.StatesTransitions.FirstOrDefault(x => x.moduleFormId == (int)ModuleFormsEnum.FileIndexing);
            hdfFileIndexing.Value = state.id.ToString();
            hdfNextState.Value = db.StatesTransitions.OrderBy(x => x.stateOrder).FirstOrDefault(x => x.stateOrder > state.stateOrder).id.ToString();
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
                    BoxFile q = db.BoxFiles.FirstOrDefault(x => x.id.Equals(int.Parse(hdfFile.Value)));
                    q.nextStateId = int.Parse(hdfNextState.Value);
                    q.employee = UserDetails.DeSerializeUserDetails(Session["User"].ToString()).FullName;
                    q.notes = txtNotes.Text;
                    q.reference = txtReference.Text.Trim();
                    q.title = txtTitle.Text.Trim();
                    FileTransaction l = new FileTransaction();
                    l.stateTransitionId = int.Parse(hdfFileIndexing.Value);
                    l.fileId = q.id;
                    l.actionTypeId = (int)ActionsEnum.Done;
                    l.date = DateTime.Now;
                    l.userId = UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID;
                    db.FileTransactions.InsertOnSubmit(l);
                    db.SubmitChanges();
                    if (Request.QueryString["f"] == null)
                        GetNextFile();
                    else 
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('تم الحفظ بنجاح');location.href='project-files.aspx?id=" + Request.QueryString["id"] + "&g=" + Request.QueryString["g"] + "&c=" + Request.QueryString["c"] + "';</script>", false);
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
    private void GetNextFile()
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            try
            {
                ClearControls();
                if (hdfBatch.Value == null || hdfBatch.Value == string.Empty)
                {
                    GetUserBatch();
                }
                var query = db.BoxFiles.Where(t => t.batchId.Equals(int.Parse(hdfBatch.Value))
                             && t.nextStateId == int.Parse(hdfFileIndexing.Value)).FirstOrDefault();
                if (query != null)
                {
                    hdfFile.Value = query.id.ToString();
                    txtCode.Text = query.barcode;
                    txtNotes.Text = query.notes;
                    txtReference.Text = query.reference;
                    txtTitle.Text = query.title;
                    string embed = "<object data = '{0}#toolbar=1'type = 'application/pdf' width = '100%' height = '700px' ></ object >";
                    string path = "http://10.1.20.25/SCU/boxfile-certificate/%23" + query.barcode + "%23.pdf?" + DateTime.Now;
                    ltrPDFEmbed.Text = string.Format(embed, path);
                }
                else
                {
                    Batch b = db.Batches.FirstOrDefault(x => x.id.Equals(int.Parse(hdfBatch.Value)));
                    if (b != null)
                    {
                        b.indexed = true;
                        db.SubmitChanges();
                        hdfBatch.Value = null;
                        GetNextFile();
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

    private void GetUserBatch()
    {
        divControls.Visible = true;
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            int batchId = 0;
            var assignedBatch = (from i in db.Batches
                                 where i.indexed == null &&
                                 i.indexedBy.Equals(UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID)
                               //  && i.unitStructureId == UserDetails.DeSerializeUserDetails(Session["User"].ToString()).UnitStructureId
                                 select new { i.id }).FirstOrDefault();
            if (assignedBatch != null)
            {
                batchId = assignedBatch.id;
            }
            else
            {
                var newBatch = (from i in db.Batches
                                where i.BoxFiles.Count() == i.BoxFiles.Where(x => x.nextStateId >= int.Parse(hdfFileIndexing.Value)).Count()
                               // && i.unitStructureId == UserDetails.DeSerializeUserDetails(Session["User"].ToString()).UnitStructureId
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
                        "alert('لا يوجد ملفات للفهرسة');", true);
                }
            }
            hdfBatch.Value = batchId.ToString();
            if (batchId == 0)
            {
                divControls.Visible = false;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alertUser",
                    "alert('لا يوجد ملفات للفهرسة');", true);
                return;
            }
        }
    }

    private void ClearControls()
    {
        txtCode.Text = txtNotes.Text = txtReference.Text = txtTitle.Text = string.Empty;
    }

    private void FillControls()
    {
        if (Request.QueryString["f"] != null)
        {
            using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
            {
                var query = db.BoxFiles.Where(t => t.id.Equals(int.Parse(Request.QueryString["f"].ToString()))).FirstOrDefault();
                hdfFile.Value = query.id.ToString();
                txtCode.Text = query.barcode;
                txtNotes.Text = query.notes;
                txtReference.Text = query.reference;
                txtTitle.Text = query.title;
                string embed = "<object data = '{0}#toolbar=1'type = 'application/pdf' width = '100%' height = '700px' ></ object >";
                string path = "http://10.1.20.25/SCU/boxfile-certificate/%23" + query.barcode + "%23.pdf?" + DateTime.Now;
                ltrPDFEmbed.Text = string.Format(embed, path);
            }
        }
    }
}