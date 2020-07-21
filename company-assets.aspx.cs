using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class company_assets : System.Web.UI.Page
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
            lnkHistory.HRef = "log.aspx?t=" + EncryptString.Encrypt("Assets") + "&c=" + Request.QueryString["id"];
            if (Session["User"] != null && Session["User"].ToString() != string.Empty)
            {
                if (UserPermissions.Any(
                    p =>
                        p.PageUrl.ToLower().Equals(Common.CompanyAssetsPath) &&
                        (p.Show.Equals(true) || p.Edit.Equals(true) || p.Approve.Equals(true))))
                {
                    if (Request.QueryString["id"] != null)
                    {
                        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
                        {
                            var per = UserPermissions.FirstOrDefault(p => p.PageUrl.ToLower().Equals(Common.CompanyAssetsPath));
                            Company query = db.Companies.FirstOrDefault(x => x.id == int.Parse(EncryptString.Decrypt(Request.QueryString["id"])));
                            ((HtmlGenericControl)Page.Master.FindControl("ulBreadcrumb")).InnerHtml = "<li><i class='ace-icon fa fa-home home-icon'></i><a href ='Default.aspx'> الرئيسية </a></li><li>" + per.ModuleName + "</li><li>" + per.ParentForm + "</li><li><strong>"+ query.name + "</strong></li><li><a class='active'>" + per.PageName + "</a></li>";
                            Page.Title = per.PageName;
                        }
                    }
                    else
                        Response.Redirect("projects.aspx");
                }
                else
                    Response.Redirect("company-no-permission.aspx?id=" + Request.QueryString["id"]);
            }
            else
                Response.Redirect("Login.aspx?ReturnURL=" + Request.Url.AbsolutePath);
            BindDDL();
            if (UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.CompanyAssetsPath) && (p.Show.Equals(true) || p.Edit.Equals(true) || p.Approve.Equals(true))))
                BindData();
        }
    }
    private void BindDDL()
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            var query1 = from q in db.AssetsStatus
                         select new
                         {
                             q.id,
                             q.name
                         };
            ddlStatusSrc.DataSource = query1;
            ddlStatusSrc.DataTextField = "name";
            ddlStatusSrc.DataValueField = "id";
            ddlStatusSrc.DataBind();
            ddlStatusSrc.Items.Insert(0, new ListItem("-- اختر --", "0"));

            var query2 = from q in db.Projects
                         where q.companyId == int.Parse(EncryptString.Decrypt(Request.QueryString["id"]))
                         && q.statusId==(int)StatusEnum.Approved
                         select new
                         {
                             q.id,
                             q.name
                         };
            ddlProject.DataSource = query2;
            ddlProject.DataTextField = "name";
            ddlProject.DataValueField = "id";
            ddlProject.DataBind();
            ddlProject.Items.Insert(0, new ListItem("-- اختر --", "0"));

            ddlProjectSrc.DataSource = query2;
            ddlProjectSrc.DataTextField = "name";
            ddlProjectSrc.DataValueField = "id";
            ddlProjectSrc.DataBind();
            ddlProjectSrc.Items.Insert(0, new ListItem("-- اختر --", "0"));
        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.CompanyAssetsPath) && p.Add.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للاضافة');</script>", false);
            return;
        }
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
                    if (ViewState["ID"] == null)
                    {
                        Asset q = new Asset()
                        {
                            companyId = int.Parse(EncryptString.Decrypt(Request.QueryString["id"])),
                            model = txtModel.Text,
                            name = txtName.Text.Trim(),
                            serialNo = txtSerNo.Text.Trim(),
                            assetsStatusId = (int)AssetsStatusEnum.UnderSupply
                        };
                        db.Assets.InsertOnSubmit(q);
                        db.SubmitChanges();
                        LogWriter.LogWrite("Assets", ((int)ActivitiesEnum.Add).ToString(), q.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty,EncryptString.Decrypt(Request.QueryString["id"]), string.Empty, string.Empty);
                    }
                    else
                    {
                        Asset q = db.Assets.FirstOrDefault(x => x.id.Equals(int.Parse(ViewState["ID"].ToString())));
                        q.model = txtModel.Text;
                        q.name = txtName.Text.Trim();
                        q.serialNo = txtSerNo.Text.Trim();
                        db.SubmitChanges();
                        LogWriter.LogWrite("Assets", ((int)ActivitiesEnum.Update).ToString(), q.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, EncryptString.Decrypt(Request.QueryString["id"]), string.Empty, string.Empty);
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
    protected void lnkEdit_Command(object sender, CommandEventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.CompanyAssetsPath) && p.Edit.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للتعديل');</script>", false);
            return;
        }
        ViewState["ID"] = int.Parse(e.CommandArgument.ToString());
        FillControls();
    }
    private void FillControls()
    {
        if (ViewState["ID"] != null)
        {
            using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
            {
                var query = db.Assets.Where(t => t.id.Equals(int.Parse(ViewState["ID"].ToString()))).FirstOrDefault();
                txtName.Text = query.name;
                txtModel.Text = query.model;
                txtSerNo.Text = query.serialNo;
                mpeObject.Show();
            }
        }
    }
    protected void btnClose_Click(object sender, EventArgs e)
    {
        ClearControls();
        ViewState["ID"] = null;
        mpeObject.Hide();
    }
    private void ClearControls()
    {
        ViewState["ID"] = null;
        txtModel.Text = txtName.Text = txtSerNo.Text = string.Empty;
    }
    protected void lnkDelete_Command(object sender, CommandEventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.CompanyAssetsPath) && p.Delete.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للحذف');</script>", false);
            return;
        }
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            Asset c = db.Assets.Where(x => x.id == int.Parse(e.CommandArgument.ToString())).FirstOrDefault();
            try
            {
                db.Assets.DeleteOnSubmit(c);
                db.SubmitChanges();
                LogWriter.LogWrite("Assets", ((int)ActivitiesEnum.Delete).ToString(), c.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, EncryptString.Decrypt(Request.QueryString["id"]), string.Empty, string.Empty);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('تم الحذف بنجاح');</script>", false);
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
            var query = (from b in db.Assets
                         where b.companyId == int.Parse(EncryptString.Decrypt(Request.QueryString["id"]))
                         select new
                         {
                             b.id,
                             b.model,
                             b.name,
                             b.projectId,
                             b.serialNo,
                             b.assetsStatusId,
                             project=b.Project.name,
                             status=b.AssetsStatus.name,
                             IsCheckBoxVisible = b.projectId == null
                         });
            if (ddlStatusSrc.SelectedValue != "0")
                query = query.Where(x => x.assetsStatusId.Equals(int.Parse(ddlStatusSrc.SelectedValue)));
            if (ddlProjectSrc.SelectedValue != "0")
                query = query.Where(x => x.projectId.Equals(int.Parse(ddlProjectSrc.SelectedValue)));
            if (txtModelSrc.Text.Trim() != string.Empty)
                query = query.Where(x => x.model.Equals(txtModelSrc.Text.Trim()));
            if (txtNameSrc.Text.Trim() != string.Empty)
                query = query.Where(x => x.name.Contains(txtNameSrc.Text.Trim()));
            if (txtSerNoSrc.Text.Trim() != string.Empty)
                query = query.Where(x => x.serialNo.Equals(txtSerNoSrc.Text.Trim()));
            dtData = query.CopyToDataTable();
            gdvData.DataSource = dtData;
            gdvData.DataBind();
            lblResult.Text = dtData.Rows.Count.ToString();
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.CompanyAssetsPath) && (p.Show.Equals(true) || p.Edit.Equals(true) || p.Delete.Equals(true) || p.Approve.Equals(true) || p.Freze.Equals(true))))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للبحث');</script>", false);
            return;
        }
        BindData();
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        ClearControls();
        mpeObject.Show();
    }
    protected void btnClearSearch_Click(object sender, EventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.CompanyAssetsPath) && (p.Show.Equals(true) || p.Edit.Equals(true) || p.Delete.Equals(true) || p.Approve.Equals(true) || p.Freze.Equals(true))))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للبحث');</script>", false);
            return;
        }
        txtNameSrc.Text = txtSerNoSrc.Text = txtModelSrc.Text = string.Empty;
        ddlStatusSrc.SelectedValue = ddlProjectSrc.SelectedValue = "0";
        BindData();
    }

    protected void lnkReceived_Command(object sender, CommandEventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.CompanyAssetsPath) && p.Edit.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للتعديل');</script>", false);
            return;
        }
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            Asset c = db.Assets.Where(x => x.id == int.Parse(e.CommandArgument.ToString())).FirstOrDefault();
            try
            {
                c.assetsStatusId = (int)AssetsStatusEnum.Asset;
                db.SubmitChanges();
                LogWriter.LogWrite("Assets", ((int)ActivitiesEnum.Update).ToString(), c.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, EncryptString.Decrypt(Request.QueryString["id"]), string.Empty,"توريد الأصل");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('تم الحفظ بنجاح');</script>", false);
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
    protected void btnAssetsReg_Click(object sender, EventArgs e)
    {
        int checkedRows = 0;
        foreach (GridViewRow row in gdvData.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkRow = (row.Cells[0].FindControl("chkRow") as CheckBox);
                if (chkRow.Checked)
                {
                    checkedRows += 1;
                }
            }
        }
        if (checkedRows == 0)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert(' تأكد من اختيار الأصل. ');</script>", false);
            return;
        }
        mpeProject.Show();
    }

    protected void lnkClosePoject_Click(object sender, EventArgs e)
    {
        mpeProject.Hide();
        BindData();
    }

    protected void btnAssign_Click(object sender, EventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.CompanyAssetsPath) && p.Edit.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للتعديل');</script>", false);
            return;
        }
        int checkedRows = 0;
        foreach (GridViewRow row in gdvData.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                HtmlInputCheckBox chkProjectRow = (row.Cells[0].FindControl("chkProjectRow") as HtmlInputCheckBox);
                if (chkProjectRow.Checked)
                {
                    checkedRows += 1;
                }
            }
        }
        if (checkedRows == 0)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert(' تأكد من اختيار الأصول. ');</script>", false);
            return;
        }
        mpeProject.Show();
    }
    protected void btnSaveProject_Click(object sender, EventArgs e)
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            try
            {
                int checkedRows = 0;
                foreach (GridViewRow row in gdvData.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        HtmlInputCheckBox chkProjectRow = (row.Cells[0].FindControl("chkProjectRow") as HtmlInputCheckBox);
                        if (chkProjectRow.Checked)
                        {
                            checkedRows += 1;
                            HiddenField hdfID = (row.Cells[0].FindControl("hdfID") as HiddenField);
                            Asset u = db.Assets.FirstOrDefault(x => x.id == int.Parse(hdfID.Value));
                            u.projectId = ddlProject.SelectedValue != "0" ? (int?)int.Parse(ddlProject.SelectedValue) : null;
                            AssetsAssignment a = new AssetsAssignment();
                            a.assetsId = u.id;
                            a.projectId= ddlProject.SelectedValue != "0" ? (int?)int.Parse(ddlProject.SelectedValue) : null;
                            a.assignedBy = UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID;
                            a.assignmentDate = DateTime.Now;
                            db.AssetsAssignments.InsertOnSubmit(a);
                            db.SubmitChanges();
                            LogWriter.LogWrite("Assets", ((int)ActivitiesEnum.Update).ToString(), u.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty,EncryptString.Decrypt(Request.QueryString["id"]), string.Empty,(ddlProject.SelectedValue != "0" ? "تخصيص أصل لمشروع":"ارجاع الأصل للشركة"));
                        }
                    }
                }
                ddlProject.SelectedValue = "0";
                BindData();
                ScriptManager.RegisterStartupScript(this, GetType(), "alertUser","alert('تم الحفظ بنجاح');", true);
            }
            catch (Exception ex)
            {
                Common.InsertException(ex.Message, ex.StackTrace,
                          new System.IO.FileInfo(System.Web.HttpContext.Current.Request.Url.AbsolutePath).Name);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('حدث خطا اثناء الحفظ');</script>", false);
            }
        }
    }
    protected void lnkLog_Command(object sender, CommandEventArgs e)
    {
        ViewState["id"] = e.CommandArgument.ToString();
        BindLog();
    }
    private void BindLog()
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            var query = (from x in db.AssetsAssignments
                         where x.assetsId.Equals(int.Parse(ViewState["id"].ToString()))
                         select new
                         {
                             project=x.Project.name,
                             assignedBy=x.User1.fullName,
                             x.assignmentDate,
                             x.deliveryDate,
                             deliveredBy=x.User.fullName
                         }).ToList();
            dtStatus = query.CopyToDataTable();
            gdvStatus.DataSource = dtStatus;
            gdvStatus.DataBind();
            mpeLog.Show();
        }
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
    protected void gdvStatus_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvStatus.PageIndex = e.NewPageIndex;
        BindLog();
    }
}