using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Drawing;
using Telerik.Reporting;
using SortDirection = System.Web.UI.WebControls.SortDirection;
using System.IO;
using System.Web;
using iTextSharp.text.pdf;

public partial class project_files : System.Web.UI.Page
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
    public SortDirection dirStatus
    {
        get
        {
            if (ViewState["_dirStatus"] == null)
            {
                ViewState["_dirStatus"] = SortDirection.Ascending;
            }
            return (SortDirection)ViewState["_dirStatus"];
        }
        set
        {
            ViewState["_dirStatus"] = value;
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
            HtmlAnchor lnkHistory = (HtmlAnchor)Master.FindControl("lnkHistory");
            lnkHistory.Visible = false;
            if (Session["User"] != null && Session["User"].ToString() != string.Empty)
            {
                if (UserPermissions.Any(
                    p =>
                        p.PageUrl.ToLower().Equals(Common.ProjectFilesPath) && p.Show.Equals(true)))
                {
                    if (Request.QueryString["id"] != null && Request.QueryString["g"] != null)
                    {
                        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
                        {
                            var per = UserPermissions.FirstOrDefault(p => p.PageUrl.ToLower().Equals(Common.ProjectFilesPath));
                            Project query = db.Projects.FirstOrDefault(x => x.id == int.Parse(EncryptString.Decrypt(Request.QueryString["id"])));
                            ((HtmlGenericControl)Page.Master.FindControl("ulBreadcrumb")).InnerHtml = "<li><i class='ace-icon fa fa-home home-icon'></i><a href ='Default.aspx'> الرئيسية </a></li><li>" + per.ModuleName + "</li><li><strong>المشروع : " + query.name + "</strong></li><li><strong>الجهة : " + query.GovernmentalEntity.name + "</strong></li><li><strong>الشركة المنفذة : " + query.Company.name + "</strong></li><li><a class='active'>" + per.PageName + "</a></li>";
                            Page.Title = per.PageName;
                            ViewState["batch"] = db.Settings.FirstOrDefault(x => x.id == (int)SettingsEnum.BatchManagement).value;
                        }
                    }
                    else
                        Response.Redirect("projects.aspx");
                }
                else
                    Response.Redirect("project-no-permission.aspx?id=" + Request.QueryString["id"] + "&g=" + Request.QueryString["g"] + "&c=" + Request.QueryString["c"]);
            }
            else
                Response.Redirect("Login.aspx?ReturnURL=" + Request.Url.AbsolutePath);
            if (UserDetails.DeSerializeUserDetails(Session["User"].ToString()).CompanyId == 0 &&
                UserDetails.DeSerializeUserDetails(Session["User"].ToString()).UnitStructureId == 0)
                Response.Redirect("project-no-permission.aspx?id=" + Request.QueryString["id"] + "&g=" + Request.QueryString["g"] + "&c=" + Request.QueryString["c"]);
            else if (UserDetails.DeSerializeUserDetails(Session["User"].ToString()).CompanyId == 0)
            {
                gdvData.Columns[3].Visible = false;
                gdvData.Columns[6].Visible = false;
                lnkBoxFiles.Text = "<i class='ace-icon fa fa-plus bigger-110'></i>إضافة ملف";
                divBtnChangeStatus.Visible = false;
            }
            BindDDL();
            if (UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectFilesPath) && p.Show.Equals(true)))
                BindData();
        }
    }

    private void BindDDL()
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            var query = from q in db.UnitStructures
                        where q.governmentalEntityId == int.Parse(EncryptString.Decrypt(Request.QueryString["g"]))
                        select new { q.id, q.name };
            ddlUnit.DataSource = query;
            ddlUnit.DataTextField = "name";
            ddlUnit.DataValueField = "id";
            ddlUnit.DataBind();
            ddlUnit.Items.Insert(0, new ListItem("-- اختر --", "0"));

            var query2 = from q in db.StatesTransitions
                         select new { q.id, q.name };
            ddlStatus.DataSource = query2;
            ddlStatus.DataTextField = "name";
            ddlStatus.DataValueField = "id";
            ddlStatus.DataBind();
            ddlStatus.Items.Insert(0, new ListItem("-- اختر الحالة --", "0"));
        }
    }

    public bool CheckVisible(int state, int action)
    {
        return StateTransitionPermissions.Any(
                 p => p.StateID.Equals(state) &&
                 (action == (int)ActionsEnum.Done ? p.Done.Equals(true) : p.Restart.Equals(true)));
    }
    private void BindData()
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            var query = (from b in db.BoxFiles
                         where b.UnitStructure.governmentalEntityId.Equals(int.Parse(EncryptString.Decrypt(Request.QueryString["g"])))
                         select new
                         {
                             b.barcode,
                             b.id,
                             b.unitStructureId,
                             state = UserDetails.DeSerializeUserDetails(Session["User"].ToString()).CompanyId != 0 ? (b.nextStateId != null ? b.StatesTransition.name : "تم " + db.FileTransactions.Where(x => x.fileId == b.id).OrderByDescending(x => x.id).FirstOrDefault().StatesTransition.name) : "مفتوح",
                             stateOrder = b.nextStateId != null ? b.StatesTransition.stateOrder : 0,
                             noOfPages = b.FileDocuments.Sum(x => x.noOfPages),
                             docs = b.nextStateId > (int)StatesEnum.BarcodeDocuments ? (int?)b.FileDocuments.Count() : null,
                             nextStateId = b.nextStateId ?? 0,
                             batch = b.Batch.barcode,
                             box = b.Box.barcode,
                             unit = b.UnitStructure.name
                         });
            if (UserDetails.DeSerializeUserDetails(Session["User"].ToString()).UnitStructureId != 0)
                query = query.Where(x => x.unitStructureId == UserDetails.DeSerializeUserDetails(Session["User"].ToString()).UnitStructureId);
            if (txtBarcodeSrc.Text.Trim() != string.Empty)
                query = query.Where(x => x.barcode == txtBarcodeSrc.Text.Trim());
            if (ddlStatus.SelectedValue != "0")
                query = query.Where(x => x.nextStateId == int.Parse(ddlStatus.SelectedValue));
            dtData = query.OrderBy(x => x.id).CopyToDataTable();
            gdvData.DataSource = dtData;
            gdvData.DataBind();
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
    protected void lnkLog_Command(object sender, CommandEventArgs e)
    {
        if (UserDetails.DeSerializeUserDetails(Session["User"].ToString()).CompanyId != 0)
        {
            ViewState["id"] = e.CommandArgument.ToString();
            BindLog();
        }
        else
            Response.Redirect("files-indexing.aspx?id=" + Request.QueryString["id"] + "&g=" + Request.QueryString["g"] + "&c=" + Request.QueryString["c"] + "&f=" + e.CommandArgument.ToString());
    }
    private void BindLog()
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            var query = (from x in db.FileTransactions
                         where x.fileId.Equals(int.Parse(ViewState["id"].ToString()))
                         select new
                         {
                             name = x.ActionsType.name + " " + x.StatesTransition.name,
                             x.User.fullName,
                             x.date,
                             x.actionTypeId
                         }).ToList();
            dtStatus = query.CopyToDataTable();
            gdvStatus.DataSource = dtStatus;
            gdvStatus.DataBind();
            mpeLog.Show();
        }
    }
    protected void lnkChangeStatus_Command(object sender, CommandEventArgs e)
    {
        int id = int.Parse(e.CommandArgument.ToString().Split(';')[0]);
        int order = int.Parse(e.CommandArgument.ToString().Split(';')[1]);
        int state = int.Parse(e.CommandArgument.ToString().Split(';')[2]);
        int? nextState = null;
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            StatesTransition s = db.StatesTransitions.OrderBy(x => x.stateOrder).FirstOrDefault(x => x.stateOrder > order);
            if (s != null)
                if (!(s.ProcessPrerequisite.ProjectPrerequisites.Any() && s.ProcessPrerequisite.ProjectPrerequisites.FirstOrDefault().doneDate != null))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertUser",
                                          "alert('لابد الإنتهاء من متطلبات تشغيل المشروع اولا');", true);
                    return;
                }
            nextState = s != null ? (int?)s.id : null;
        }
        if (state == (int)StatesEnum.CompanyReceivesFiles)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert(' عفوا، لا يمكن استلام ملف واحد. ');</script>", false);
            return;
        }
        else if (state != (int)StatesEnum.BarcodeDocuments)
        {
            ChangeStatus(id, order, nextState, null, state);
            BindData();
            ScriptManager.RegisterStartupScript(this, GetType(), "alertUser",
                    "alert('تم الحفظ بنجاح');", true);
        }
        else
        {
            hdfId_Barcode.Value = id.ToString();
            hdfOrder_Barcode.Value = order.ToString();
            hdfState_Barcode.Value = nextState.ToString();
            hdfCurrentState_Barcode.Value = state.ToString();
            mpeBarcodeDocuments.Show();
        }
    }
    private int CreateDeliveryBatch(int unit)
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            Batch batch = db.Batches.Where(x => x.UnitStructure.governmentalEntityId == int.Parse(EncryptString.Decrypt(Request.QueryString["g"]))).OrderByDescending(x => x.id).FirstOrDefault();
            int batchCode = 0;
            if (batch != null)
                batchCode = batch.code ?? 0;
            Batch n = new Batch();
            n.unitStructureId = unit;
            n.code = batchCode + 1;
            n.barcode = "BATCH-" + db.UnitStructures.FirstOrDefault(x => x.id == unit).code + "-" + (batchCode + 1).ToString().PadLeft(6, '0');
            n.date = DateTime.Now;
            db.Batches.InsertOnSubmit(n);
            db.SubmitChanges();
            return n.id;
        }
    }
    protected void lnkChangeAllStatus_Command(object sender, CommandEventArgs e)
    {
        string result = CheckAllFilesStatus((int)ActionsEnum.Done);
        int order = int.Parse(result.Split(';')[0]);
        if (order != -1)
        {
            int? nextState = null;
            using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
            {
                StatesTransition s = db.StatesTransitions.OrderBy(x => x.stateOrder).FirstOrDefault(x => x.stateOrder > order);
                if (!(s != null && s.ProcessPrerequisite.ProjectPrerequisites.Any() && s.ProcessPrerequisite.ProjectPrerequisites.FirstOrDefault().doneDate != null))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertUser",
                                          "alert('لابد الإنتهاء من متطلبات تشغيل المشروع اولا');", true);
                    return;
                }
                nextState = s != null ? (int?)s.id : null;
            }
            int? batch = null;
            int state = int.Parse(result.Split(';')[1]);
            if (state == (int)StatesEnum.CompanyReceivesFiles)
                batch = CreateDeliveryBatch(int.Parse(result.Split(';')[2]));
            foreach (GridViewRow row in gdvData.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    HtmlInputCheckBox chkFile = (row.Cells[0].FindControl("chkFile") as HtmlInputCheckBox);
                    if (chkFile.Checked)
                    {
                        HiddenField hdfId = (row.Cells[0].FindControl("hdfId") as HiddenField);
                        int id = int.Parse(hdfId.Value);
                        ChangeStatus(id, order, nextState, batch, state);
                    }
                }
            }
            if (state == (int)StatesEnum.CompanyReceivesFiles)
            {
                var uriReportSource = new UriReportSource { Uri = Server.MapPath("rptDeliveryReport.trdp") };
                uriReportSource.Parameters.Add(new Telerik.Reporting.Parameter("id", batch));
                ReportViewer1.ReportSource = uriReportSource;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'>fnPrintReport();</script>", false);
            }
            BindData();
            ScriptManager.RegisterStartupScript(this, GetType(), "alertUser",
                    "alert('تم الحفظ بنجاح');", true);
        }
    }
    private void ChangeStatus(int id, int order, int? nextState, int? batch, int currentState)
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            try
            {
                StatesTransition current = db.StatesTransitions.FirstOrDefault(x => x.stateOrder == order);
                if (current.moduleFormId == null)
                {
                    BoxFile q = db.BoxFiles.FirstOrDefault(x => x.id.Equals(id));
                    if (currentState == (int)StatesEnum.Scan)
                    {
                        if (TransferDocuments(q.UnitStructure.code, q.code, q.FileDocuments.Count, q.barcode) != true)
                            return;
                    }
                    q.nextStateId = nextState;
                    if (batch != null)
                        q.batchId = batch;
                    FileTransaction f = new FileTransaction();
                    f.fileId = id;
                    f.stateTransitionId = current.id;
                    f.actionTypeId = (int)ActionsEnum.Done;
                    f.date = DateTime.Now;
                    f.userId = UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID;
                    db.FileTransactions.InsertOnSubmit(f);
                    db.SubmitChanges();
                }
                else
                    Response.Redirect(current.ModuleForm.url + ".aspx?id=" + Request.QueryString["id"] + "&g=" + Request.QueryString["g"] + "&c=" + Request.QueryString["c"] + "&f=" + id);
            }
            catch (Exception ex)
            {
                Common.InsertException(ex.Message, ex.StackTrace,
                      new System.IO.FileInfo(System.Web.HttpContext.Current.Request.Url.AbsolutePath).Name);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('حدث خطا اثناء الحفظ');</script>", false);
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
    protected void lnkPrint_Command(object sender, CommandEventArgs e)
    {
        var uriReportSource = new Telerik.Reporting.UriReportSource { Uri = Server.MapPath("rptBoxfile.trdp") };
        uriReportSource.Parameters.Add(new Telerik.Reporting.Parameter("id", int.Parse(e.CommandArgument.ToString())));
        ReportViewer1.ReportSource = uriReportSource;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'>fnPrintReport();</script>", false);
    }
    protected void btnSaveBoxFiles_Click(object sender, EventArgs e)
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            try
            {
                StatesTransition s = db.StatesTransitions.FirstOrDefault(x => x.stateOrder == 1);
                if (s.ProcessPrerequisite.ProjectPrerequisites.Any() && s.ProcessPrerequisite.ProjectPrerequisites.FirstOrDefault().doneDate != null)
                {
                    string unitCode = db.UnitStructures.FirstOrDefault(x => x.id == int.Parse(ddlUnit.SelectedValue)).code;
                    int nextBoxFile = 0;
                    if (UserDetails.DeSerializeUserDetails(Session["User"].ToString()).CompanyId == 0)
                    {
                        BoxFile b = db.BoxFiles.OrderByDescending(x => x.code).FirstOrDefault(x => x.code > 400000 && x.unitStructureId.Equals(int.Parse(ddlUnit.SelectedValue)));
                        if (b != null)
                            nextBoxFile = b.code + 1;
                        else
                            nextBoxFile = 400001;
                    }
                    else
                    {
                        BoxFile b = db.BoxFiles.OrderByDescending(x => x.code).FirstOrDefault(x => x.code < 400000 && x.unitStructureId.Equals(int.Parse(ddlUnit.SelectedValue)));
                        if (b != null)
                            nextBoxFile = b.code + 1;
                        else
                            nextBoxFile = 1;
                    }
                    for (int i = 0; i < int.Parse(txtBoxFilesCount.Text.Trim()); i++)
                    {
                        BoxFile box = new BoxFile()
                        {
                            unitStructureId = int.Parse(ddlUnit.SelectedValue),
                            code = nextBoxFile,
                            barcode = unitCode.PadLeft(5, '0') + nextBoxFile.ToString().PadLeft(6, '0') + "000",
                            noOfPages = int.Parse(txtPagesCount.Text.Trim()),
                            nextStateId = (int)StatesEnum.second
                        };
                        db.BoxFiles.InsertOnSubmit(box);
                        FileTransaction l = new FileTransaction();
                        l.BoxFile = box;
                        l.stateTransitionId = (int)StatesEnum.first;
                        l.actionTypeId = (int)ActionsEnum.Done;
                        l.userId = UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID;
                        l.date = DateTime.Now;
                        db.FileTransactions.InsertOnSubmit(l);
                        db.SubmitChanges();
                        int nextDoc = 1;
                        for (int j = 0; j < int.Parse(txtDocsCount.Text.Trim()); j++)
                        {
                            FileDocument d = new FileDocument();
                            d.fileId = box.id;
                            d.code = nextDoc;
                            d.barcode = box.barcode.Substring(0, box.barcode.Length - (int)CodesLengthEnum.Doc) + nextDoc.ToString().PadLeft((int)CodesLengthEnum.Doc, '0');
                            db.FileDocuments.InsertOnSubmit(d);
                            nextDoc++;
                        }
                        db.SubmitChanges();
                        nextBoxFile++;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertUser",
                           "alert('لابد الإنتهاء من متطلبات تشغيل المشروع اولا');", true);
                    return;
                }
                BindData();
                txtBoxFilesCount.Text = txtDocsCount.Text = txtPagesCount.Text = string.Empty;
                ddlUnit.SelectedValue = "0";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('تم انشاء الملفات بنجاح');</script>", false);
                mpeCreateBoxfile.Hide();
            }
            catch (Exception ex)
            {
                Common.InsertException(ex.Message, ex.StackTrace,
                          new System.IO.FileInfo(System.Web.HttpContext.Current.Request.Url.AbsolutePath).Name);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('حدث خطا اثناء الحفظ');</script>", false);
            }
        }
    }
    protected void btnCloseBoxFiles_Click(object sender, EventArgs e)
    {
        txtBoxFilesCount.Text = txtDocsCount.Text = txtPagesCount.Text = string.Empty;
        ddlUnit.SelectedValue = "0";
        mpeCreateBoxfile.Hide();
    }
    protected void lnkBoxFiles_Click(object sender, EventArgs e)
    {
        if (!CheckVisible((int)StatesEnum.first, (int)ActionsEnum.Done))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية لتسجيل الملفات');</script>", false);
            return;
        }
        if (UserDetails.DeSerializeUserDetails(Session["User"].ToString()).CompanyId != 0)
            mpeCreateBoxfile.Show();
        else
        {
            ddlUnit.SelectedValue = UserDetails.DeSerializeUserDetails(Session["User"].ToString()).UnitStructureId.ToString();
            txtBoxFilesCount.Text = "1";
            txtDocsCount.Text = txtPagesCount.Text = "0";
            btnSaveBoxFiles_Click(btnSaveBoxFiles, new EventArgs());
        }
    }
    protected void lnkReverse_Command(object sender, CommandEventArgs e)
    {
        ViewState["id"] = e.CommandArgument.ToString();
        ReverseStatus(int.Parse(e.CommandArgument.ToString().Split(';')[1]));
    }
    private void ReverseStatus(int order)
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            var query = from q in db.StatesTransitions
                        where q.stateOrder < order
                        && q.stateOrder != 1
                        select new
                        {
                            q.id,
                            q.name
                        };
            rpStates.DataSource = query;
            rpStates.DataBind();
        }
        mpeReverse.Show();
    }
    protected void lnkCloseReverse_Click(object sender, EventArgs e)
    {
        ViewState["id"] = null;
        mpeReverse.Hide();
    }
    protected void lnkReverseAll_Command(object sender, CommandEventArgs e)
    {
        ViewState["id"] = null;
        int order = int.Parse(CheckAllFilesStatus((int)ActionsEnum.Restart).Split(';')[0]);
        if (order != -1)
            ReverseStatus(order);
    }
    private string CheckAllFilesStatus(int action)
    {
        int checkedRows = 0, state = 0, order = -1, unit = 0;
        foreach (GridViewRow row in gdvData.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                HtmlInputCheckBox chkFile = (row.Cells[0].FindControl("chkFile") as HtmlInputCheckBox);
                if (chkFile.Checked)
                {
                    checkedRows += 1;
                    HiddenField hdfState = (row.Cells[0].FindControl("hdfState") as HiddenField);
                    HiddenField hdfOrder = (row.Cells[0].FindControl("hdfOrder") as HiddenField);
                    HiddenField hdfUnit = (row.Cells[0].FindControl("hdfUnit") as HiddenField);
                    if ((int.Parse(hdfState.Value) != state && state != 0) || (int.Parse(hdfUnit.Value) != unit && unit != 0))
                    {
                        order = -1;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert(' عفوا، لا يمكنك تغير الحالة .. تأكد من اختيار ملفات لها نفس الحالة و فى نفس الإدارة. ');</script>", false);
                        return (order + ";" + state + ";" + unit);
                    }
                    else
                    {
                        state = int.Parse(hdfState.Value);
                        unit = int.Parse(hdfUnit.Value);
                        order = int.Parse(hdfOrder.Value);
                    }
                    if (state == (int)StatesEnum.BarcodeDocuments)
                    {
                        order = -1;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert(' عفوا، لا يمكنك تطبيق هذه الحالة على مجموعة من الملفات في آن واحد. ');</script>", false);
                        return (order + ";" + state + ";" + unit);
                    }
                }
            }
        }
        if (checkedRows == 0)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert(' تأكد من اختيار ملف واحد على الأقل. ');</script>", false);
            return (order + ";" + state + ";" + unit);
        }
        else
        {
            if (state == (int)StatesEnum.CompanyReceivesFiles)
            {
                if (checkedRows == 1)
                {
                    order = -1;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert(' عفوا، لا يمكن استلام ملف واحد. ');</script>", false);
                    return (order + ";" + state + ";" + unit);
                }
            }
        }
        if (!CheckVisible(state, action))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert(' عفو، ليس لديك صلاحية لتغير الحالة ');</script>", false);
            order = -1;
            return (order + ";" + state + ";" + unit);
        }
        return (order + ";" + state + ";" + unit);
    }
    protected void btnSaveReverse_Click(object sender, EventArgs e)
    {
        string val = string.Empty;
        for (int i = 0; i < rpStates.Items.Count; i++)
        {
            HtmlInputRadioButton rdStates = (HtmlInputRadioButton)rpStates.Items[i].FindControl("rdStates");
            if (rdStates.Checked)
            {
                val = rdStates.Value;
            }
        }
        if (val == string.Empty)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alertUser",
                    "alert('يرجى اختيار الحالة اولا');", true);
            mpeReverse.Show();
            return;
        }
        int? form = null;
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            form = db.StatesTransitions.FirstOrDefault(x => x.id == int.Parse(val)).moduleFormId;
        }
        if (ViewState["id"] != null)
            SaveReverse(int.Parse(ViewState["id"].ToString().Split(';')[0]), int.Parse(val), form);
        else
        {
            foreach (GridViewRow row in gdvData.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    HtmlInputCheckBox chkFile = (row.Cells[0].FindControl("chkFile") as HtmlInputCheckBox);
                    if (chkFile.Checked)
                    {
                        HiddenField hdfId = (row.Cells[0].FindControl("hdfId") as HiddenField);
                        int id = int.Parse(hdfId.Value);
                        SaveReverse(id, int.Parse(val), form);
                    }
                }
            }
        }
        ScriptManager.RegisterStartupScript(this, GetType(), "alertUser",
                "alert('تم الحفظ');", true);
        BindData();
    }
    private void SaveReverse(int file, int state, int? form)
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            try
            {
                BoxFile b = db.BoxFiles.FirstOrDefault(x => x.id == file);
                b.nextStateId = state;
                FileTransaction f = new FileTransaction();
                f.actionTypeId = (int)ActionsEnum.Restart;
                f.date = DateTime.Now;
                f.userId = UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID;
                f.fileId = b.id;
                f.stateTransitionId = state;
                db.FileTransactions.InsertOnSubmit(f);
                if (form == (int)ModuleFormsEnum.DocsIndexing || form == (int)ModuleFormsEnum.DocsQa)
                {
                    var query = db.FileDocuments.Where(x => x.fileId == file);
                    foreach (var i in query)
                    {
                        if (form == (int)ModuleFormsEnum.DocsIndexing)
                            i.isIndexed = null;
                        i.isReviewed = null;
                    }
                }
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                Common.InsertException(ex.Message, ex.StackTrace,
                          new System.IO.FileInfo(System.Web.HttpContext.Current.Request.Url.AbsolutePath).Name);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('حدث خطا اثناء الحفظ');</script>", false);
            }
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
    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        int size = ddlPageSize.SelectedValue != "all" ? int.Parse(ddlPageSize.SelectedValue) : dtData.Rows.Count;
        gdvData.PageSize = size;
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

    protected void gdvData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].BackColor = Color.White;
        }
    }

    protected void lnkCloseBarcodeDocuments_Click(object sender, EventArgs e)
    {
        txtDocFrom.Text = "1";
        txtDocTo.Text = string.Empty;
        hdfId_Barcode.Value = hdfState_Barcode.Value = hdfOrder_Barcode.Value = null;
        mpeBarcodeDocuments.Hide();
    }

    protected void btnSaveBarcodeDocuments_Click(object sender, EventArgs e)
    {
        ChangeStatus(int.Parse(hdfId_Barcode.Value), int.Parse(hdfOrder_Barcode.Value), int.Parse(hdfState_Barcode.Value), null, int.Parse(hdfCurrentState_Barcode.Value));
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            var query = db.FileDocuments.Where(x => x.code > int.Parse(txtDocTo.Text.Trim())
                        && x.fileId == int.Parse(hdfId_Barcode.Value));
            db.FileDocuments.DeleteAllOnSubmit(query);
            db.SubmitChanges();
            txtDocFrom.Text = "1";
            txtDocTo.Text = string.Empty;
        }
        BindData();
        ScriptManager.RegisterStartupScript(this, GetType(), "alertUser",
                "alert('تم الحفظ بنجاح');", true);
    }
    protected void lnkPrintSelection_Click(object sender, EventArgs e)
    {
        int checkedRows = 0;
        var reportBook = new ReportBook();
        foreach (GridViewRow row in gdvData.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                HtmlInputCheckBox chkFile = (row.Cells[0].FindControl("chkFile") as HtmlInputCheckBox);
                if (chkFile.Checked)
                {
                    checkedRows++;
                    HiddenField hdfId = (row.Cells[0].FindControl("hdfId") as HiddenField);
                    int id = int.Parse(hdfId.Value);
                    var uriReportSource = new Telerik.Reporting.UriReportSource { Uri = Server.MapPath("rptBoxfile.trdp") };
                    uriReportSource.Parameters.Add(new Telerik.Reporting.Parameter("id", id));
                    reportBook.ReportSources.Add(uriReportSource);
                    chkFile.Checked = false;
                }
            }
        }
        if (checkedRows == 0)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert(' تأكد من اختيار ملف واحد على الأقل. ');</script>", false);
            return;
        }
        ReportViewer1.ReportSource = reportBook;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'>fnPrintReport();</script>", false);
    }

    protected void lnkPrintFileBarcode_Command(object sender, CommandEventArgs e)
    {
        var uriReportSource = new Telerik.Reporting.UriReportSource { Uri = Server.MapPath("file.trdp") };
        uriReportSource.Parameters.Add(new Telerik.Reporting.Parameter("barcode", e.CommandArgument.ToString().Split(';')[0]));
        uriReportSource.Parameters.Add(new Telerik.Reporting.Parameter("branch", e.CommandArgument.ToString().Split(';')[1]));
        ReportViewer1.ReportSource = uriReportSource;
        ScriptManager.RegisterStartupScript(this, GetType(), "alertUser", "fnPrintReport();", true);
    }
    private bool TransferDocuments(string unitCode, int fileCode, int docsCount, string fileBarcode)
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            try
            {
                string basic =@"D:\share\SPLIT"; //db.Settings.FirstOrDefault(x => x.id == (int)SettingsEnum.PdfPath).value;
                string source = basic + @"\" + unitCode + @"\" + fileCode+@"\Output";
                string[] fileEntries = Directory.GetFiles(source, "*.pdf");
                if (fileEntries.Count() != docsCount)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertUser", "alert('كود الأدارة :" + unitCode + " ، كود الملف :" + fileCode + " ،  عدد الوثائق بالمجلد غير مساوى لعدد الوثائق بقاعدة البيانات');", true);
                    return false;
                }
                foreach (string file in fileEntries)
                {
                    string barcode = Path.GetFileName(file).Split('.')[0].Substring(0, Path.GetFileName(file).Split('.')[0].Length - (int)CodesLengthEnum.Doc);
                    if (barcode != fileBarcode.Substring(0, fileBarcode.Length - (int)CodesLengthEnum.Doc))
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "alertUser", "alert('كود الأدارة :" + unitCode + " ، كود الملف :" + fileCode + " ،  يوجد وثيقة بباركود خطأ');", true);
                        return false;
                    }
                }
                Uri originalUrl = new Uri(HttpContext.Current.Request.Url.AbsoluteUri);
                string destPath = Server.MapPath("PDF/" +
                    unitCode.ToString().PadLeft((int)CodesLengthEnum.Unit, '0') + "/" +
                    fileCode.ToString().PadLeft((int)CodesLengthEnum.File, '0') + "/");
                if (!Directory.Exists(destPath))
                    Directory.CreateDirectory(destPath);
                DataTable dtBarcodes = new DataTable();
                dtBarcodes.Columns.Add("barcode");
                dtBarcodes.Columns.Add("source");
                dtBarcodes.Columns.Add("dest");
                dtBarcodes.Columns.Add("pages");
                foreach (string file in fileEntries)
                {
                    int pages = 0;
                    try
                    {
                        PdfReader pdfReader = new PdfReader(file); pages = pdfReader.NumberOfPages;
                    }
                    catch (Exception ex) { }
                    string barcode = Path.GetFileName(file).Split('.')[0];
                    dtBarcodes.Rows.Add(barcode, file, destPath + @"\" + barcode + ".pdf", pages);
                }
                for (int i = 0; i < dtBarcodes.Rows.Count; i++)
                {
                    FileDocument doc = db.FileDocuments.FirstOrDefault(x => x.barcode == dtBarcodes.Rows[i]["barcode"].ToString());
                    if (doc != null)
                    {
                        doc.noOfPages = int.Parse(dtBarcodes.Rows[i]["pages"].ToString());
                    }
                }
                db.SubmitChanges();
                for (int i = 0; i < dtBarcodes.Rows.Count; i++)
                {
                    File.Copy(dtBarcodes.Rows[i]["source"].ToString(), dtBarcodes.Rows[i]["dest"].ToString());
                }
                return true;
            }
            catch (Exception ex2)
            {
                Common.InsertException(ex2.Message, ex2.StackTrace,
                     new System.IO.FileInfo(System.Web.HttpContext.Current.Request.Url.AbsolutePath).Name);
                ScriptManager.RegisterStartupScript(this, GetType(), "alertUser",
                        "alert('Error.');", true);
                return false;
            }
        }
    }
}