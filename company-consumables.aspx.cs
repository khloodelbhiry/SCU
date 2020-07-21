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
            lnkHistory.HRef = "log.aspx?t=" + EncryptString.Encrypt("Consumables"+";"+ "ConsumableDistributions") + " & c=" + Request.QueryString["id"];
            if (Session["User"] != null && Session["User"].ToString() != string.Empty)
            {
                if (UserPermissions.Any(
                    p =>
                        p.PageUrl.ToLower().Equals(Common.CompanyConsumablesPath) &&
                        (p.Show.Equals(true) || p.Edit.Equals(true) || p.Approve.Equals(true))))
                {
                    if (Request.QueryString["id"] != null)
                    {
                        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
                        {
                            var per = UserPermissions.FirstOrDefault(p => p.PageUrl.ToLower().Equals(Common.CompanyConsumablesPath));
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
            if (UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.CompanyConsumablesPath) && (p.Show.Equals(true) || p.Edit.Equals(true) || p.Approve.Equals(true))))
                BindData();
        }
    }
    private void BindDDL()
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            var query1 = from q in db.ConsumableTypes
                         select new
                         {
                             q.id,
                             q.name
                         };
            ddlTypeSrc.DataSource = query1;
            ddlTypeSrc.DataTextField = "name";
            ddlTypeSrc.DataValueField = "id";
            ddlTypeSrc.DataBind();
            ddlTypeSrc.Items.Insert(0, new ListItem("-- اختر --", "0"));

            ddlType.DataSource = query1;
            ddlType.DataTextField = "name";
            ddlType.DataValueField = "id";
            ddlType.DataBind();
            ddlType.Items.Insert(0, new ListItem("-- اختر --", "0"));

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

            ddlStatusSrc.DataSource = db.Status;
            ddlStatusSrc.DataTextField = "name";
            ddlStatusSrc.DataValueField = "id";
            ddlStatusSrc.DataBind();
            ddlStatusSrc.Items.Insert(0, new ListItem("-- اختر --", "0"));
        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.CompanyConsumablesPath) && p.Add.Equals(true)))
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
                        Consumable q = new Consumable()
                        {
                            companyId = int.Parse(EncryptString.Decrypt(Request.QueryString["id"])),
                            description =txtDescription.Text,
                            quantity = int.Parse(txtQuantity.Text.Trim()),
                            typeId = int.Parse(ddlType.SelectedValue),
                            statusId=(int)StatusEnum.UnderApprrove
                        };
                        db.Consumables.InsertOnSubmit(q);
                        db.SubmitChanges();
                        LogWriter.LogWrite("Consumables", ((int)ActivitiesEnum.Add).ToString(), q.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty,EncryptString.Decrypt(Request.QueryString["id"]), string.Empty, string.Empty);
                    }
                    else
                    {
                        Consumable q = db.Consumables.FirstOrDefault(x => x.id.Equals(int.Parse(ViewState["ID"].ToString())));
                        q.description = txtDescription.Text;
                        q.quantity = int.Parse(txtQuantity.Text.Trim());
                        q.typeId = int.Parse(ddlType.SelectedValue);
                        db.SubmitChanges();
                        LogWriter.LogWrite("Consumables", ((int)ActivitiesEnum.Update).ToString(), q.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, EncryptString.Decrypt(Request.QueryString["id"]), string.Empty, string.Empty);
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
    protected void btnFreeze_Click(object sender, EventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.CompanyConsumablesPath) && p.Freze.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للتجميد');</script>", false);
            return;
        }
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            Consumable c = db.Consumables.Where(x => x.id == int.Parse(ViewState["ID"].ToString())).FirstOrDefault();
            try
            {
                c.statusId = (int)StatusEnum.Freezed;
                db.SubmitChanges();
                LogWriter.LogWrite("Consumables", ((int)ActivitiesEnum.Freze).ToString(), c.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, EncryptString.Decrypt(Request.QueryString["id"]), string.Empty, string.Empty);
                btnFreeze.Visible = btnApprove.Visible = btnSave.Visible = false;
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
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.CompanyConsumablesPath) && p.Approve.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للأعتماد');</script>", false);
            return;
        }
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            Consumable c = db.Consumables.Where(x => x.id == int.Parse(ViewState["ID"].ToString())).FirstOrDefault();
            try
            {
                c.statusId = (int)StatusEnum.Approved;
                db.SubmitChanges();
                LogWriter.LogWrite("Consumables", ((int)ActivitiesEnum.Approve).ToString(), c.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, EncryptString.Decrypt(Request.QueryString["id"]), string.Empty, string.Empty);
                btnApprove.Visible = btnSave.Visible = false;
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
    protected void lnkEdit_Command(object sender, CommandEventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.CompanyConsumablesPath) && p.Edit.Equals(true)))
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
                var query = db.Consumables.Where(t => t.id.Equals(int.Parse(ViewState["ID"].ToString()))).FirstOrDefault();
                txtQuantity.Text = query.quantity.ToString();
                txtDescription.Text = query.description;
                ddlType.SelectedValue= query.typeId.ToString();
                btnApprove.Visible = btnSave.Visible = query.statusId == (int)StatusEnum.UnderApprrove;
                btnFreeze.Visible = query.statusId != (int)StatusEnum.Freezed;
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
        txtDescription.Text = txtQuantity.Text = string.Empty;
        ddlType.SelectedValue = "0";
        btnApprove.Visible = btnFreeze.Visible = false;
        btnSave.Visible = true;
    }
    protected void lnkDelete_Command(object sender, CommandEventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.CompanyConsumablesPath) && p.Delete.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للحذف');</script>", false);
            return;
        }
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            Consumable c = db.Consumables.Where(x => x.id == int.Parse(e.CommandArgument.ToString())).FirstOrDefault();
            try
            {
                db.Consumables.DeleteOnSubmit(c);
                db.SubmitChanges();
                LogWriter.LogWrite("Consumables", ((int)ActivitiesEnum.Delete).ToString(), c.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, EncryptString.Decrypt(Request.QueryString["id"]), string.Empty, string.Empty);
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
            var query = (from b in db.Consumables
                         where b.companyId == int.Parse(EncryptString.Decrypt(Request.QueryString["id"]))
                         select new
                         {
                             b.id,
                             b.quantity,
                             b.typeId,
                             b.description,
                             type=b.ConsumableType.name,
                             status=b.Status.name,
                             b.statusId,
                             remaining = b.quantity-((int?)b.ConsumableDistributions.Sum(x=>x.quantity)??0)
                         });
           if (ddlTypeSrc.SelectedValue != "0")
                query = query.Where(x => x.typeId.Equals(int.Parse(ddlTypeSrc.SelectedValue)));
            if (ddlStatusSrc.SelectedValue != "0")
                query = query.Where(x => x.statusId.Equals(int.Parse(ddlStatusSrc.SelectedValue)));
            dtData = query.CopyToDataTable();
            gdvData.DataSource = dtData;
            gdvData.DataBind();
            lblResult.Text = dtData.Rows.Count.ToString();
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.CompanyConsumablesPath) && (p.Show.Equals(true) || p.Edit.Equals(true) || p.Delete.Equals(true) || p.Approve.Equals(true) || p.Freze.Equals(true))))
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
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.CompanyConsumablesPath) && (p.Show.Equals(true) || p.Edit.Equals(true) || p.Delete.Equals(true) || p.Approve.Equals(true) || p.Freze.Equals(true))))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للبحث');</script>", false);
            return;
        }
        ddlTypeSrc.SelectedValue = ddlStatusSrc.SelectedValue = "0";
        BindData();
    }

    protected void lnkClosePoject_Click(object sender, EventArgs e)
    {
        mpeProject.Hide();
        BindData();
    }

    protected void btnAssign_Click(object sender, EventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.CompanyConsumablesPath) && p.Edit.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للتعديل');</script>", false);
            return;
        }
        int checkedRows = 0, remaining = 0;
        foreach (GridViewRow row in gdvData.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                HtmlInputCheckBox chkProjectRow = (row.Cells[0].FindControl("chkProjectRow") as HtmlInputCheckBox);
                if (chkProjectRow.Checked)
                {
                    HiddenField hdfRemaining = (row.Cells[0].FindControl("hdfRemaining") as HiddenField);
                    remaining = int.Parse(hdfRemaining.Value);
                    checkedRows += 1;
                }
            }
        }
        if (checkedRows == 0)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert(' تأكد من اختيار المستهلك. ');</script>", false);
            return;
        }
        else if (checkedRows > 1)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert(' يرجى اختيار مستهلك واحد فقط ');</script>", false);
            return;
        }
        rvProjectQuantity.MaximumValue = remaining.ToString();
        rvProjectQuantity.ErrorMessage = "يجب أن لا تزيد الكمية عن " + remaining.ToString();
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
                            int remaining = db.Consumables.Where(x => x.id == int.Parse(hdfID.Value)).Select(x => x.quantity - ((int?)x.ConsumableDistributions.Sum(y => y.quantity)??0)).FirstOrDefault();
                            if (remaining >= int.Parse(txtProjectQuantity.Text))
                            {
                                ConsumableDistribution a = new ConsumableDistribution();
                                a.consumableId = int.Parse(hdfID.Value);
                                a.projectId = int.Parse(ddlProject.SelectedValue);
                                a.quantity = int.Parse(txtProjectQuantity.Text);
                                a.assignedBy = UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID;
                                a.assignmentDate = DateTime.Now;
                                db.ConsumableDistributions.InsertOnSubmit(a);
                                db.SubmitChanges();
                                LogWriter.LogWrite("ConsumableDistributions", ((int)ActivitiesEnum.Add).ToString(), a.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, EncryptString.Decrypt(Request.QueryString["id"]), string.Empty, ddlProject.SelectedItem.Text + " تخصيص كمية لمشروع ");
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "alertUser", "alert('عفوا، لا يمكنك الحفظ.. الكمية المتبقة اقل من الكمية المطلوبة');", true);
                                return;
                            }
                        }
                    }
                }
                ddlProject.SelectedValue = "0";
                txtProjectQuantity.Text = string.Empty;
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
            var query = (from x in db.ConsumableDistributions
                         where x.consumableId.Equals(int.Parse(ViewState["id"].ToString()))
                         select new
                         {
                             project=x.Project.name,
                             assignedBy=x.User.fullName,
                             x.assignmentDate,
                             x.deliveryDate,
                             deliveredBy=x.User1.fullName,
                             x.quantity
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