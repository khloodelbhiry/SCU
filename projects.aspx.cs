using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class projects : System.Web.UI.Page
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
            lnkHistory.HRef = "log.aspx?t=" + EncryptString.Encrypt("Projects");
            if (Session["User"] != null && Session["User"].ToString() != string.Empty)
            {
                if (UserPermissions.Any(
                    p =>
                        p.PageUrl.ToLower().Equals(Common.ProjectsPath) &&
                        (p.Show.Equals(true) || p.Add.Equals(true) || p.Edit.Equals(true) || p.Delete.Equals(true) || p.Approve.Equals(true) || p.Freze.Equals(true))))
                {
                    var per = UserPermissions.FirstOrDefault(p => p.PageUrl.ToLower().Equals(Common.ProjectsPath));
                    ((HtmlGenericControl)Page.Master.FindControl("ulBreadcrumb")).InnerHtml = "<li><i class='ace-icon fa fa-home home-icon'></i><a href ='Default.aspx'> الرئيسية </a></li><li><a class='active'>" + per.PageName + "</a></li>";
                    Page.Title = per.PageName;
                }
                else
                    Response.Redirect("no-permission.aspx");
            }
            else
                Response.Redirect("Login.aspx?ReturnURL=" + Request.Url.AbsolutePath);
            BindDDL();
            if (UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectsPath) && (p.Show.Equals(true) || p.Edit.Equals(true) || p.Delete.Equals(true) || p.Approve.Equals(true) || p.Freze.Equals(true))))
                BindData();
        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectsPath) && p.Add.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للاضافة');</script>", false);
            return;
        }
        mpeProject.Show();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
            {
                try
                {
                    var culture = new CultureInfo("en-US");
                    if (ViewState["id"] == null)
                    {
                        Project q = new Project();
                        q.name = txtName.Text;
                        q.governmentalEntityId = int.Parse(ddlGovernmentalEntity.SelectedValue);
                        q.companyId = int.Parse(ddlCompany.SelectedValue);
                        q.noOfPages = int.Parse(txtPagesCount.Text);
                        q.startDate = DateTime.ParseExact(txtStartDateMonth.Text.PadLeft(2, '0') + "/" + txtStartDateDay.Text.PadLeft(2, '0') + "/" + txtStartDateYear.Text, "MM/dd/yyyy", culture);
                        q.period = int.Parse(txtPeriod.Text);
                        q.deliveryDate = DateTime.ParseExact(txtDeliveryDateMonth.Text + "/" + txtDeliveryDateDay.Text + "/" + txtDeliveryDateYear.Text, "MM/dd/yyyy", culture);
                        q.statusId = (int)StatusEnum.UnderApprrove;
                        db.Projects.InsertOnSubmit(q);
                        db.SubmitChanges();
                        LogWriter.LogWrite("Projects", ((int)ActivitiesEnum.Add).ToString(), q.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                    }
                    else
                    {
                        Project q = db.Projects.FirstOrDefault(x => x.id.Equals(int.Parse(ViewState["id"].ToString())));
                        q.name = txtName.Text;
                        q.governmentalEntityId = int.Parse(ddlGovernmentalEntity.SelectedValue);
                        q.companyId = int.Parse(ddlCompany.SelectedValue);
                        q.noOfPages = int.Parse(txtPagesCount.Text);
                        q.startDate = DateTime.ParseExact(txtStartDateMonth.Text.PadLeft(2, '0') + "/" + txtStartDateDay.Text.PadLeft(2, '0') + "/" + txtStartDateYear.Text, "MM/dd/yyyy", culture);
                        q.period = int.Parse(txtPeriod.Text);
                        q.deliveryDate = DateTime.ParseExact(txtDeliveryDateMonth.Text.PadLeft(2, '0') + "/" + txtDeliveryDateDay.Text.PadLeft(2, '0') + "/" + txtDeliveryDateYear.Text, "MM/dd/yyyy", culture);
                        db.SubmitChanges();
                        LogWriter.LogWrite("Projects", ((int)ActivitiesEnum.Update).ToString(), q.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
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
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectsPath) && p.Edit.Equals(true)))
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
                    var query = db.Projects.Where(t => t.id.Equals(int.Parse(ViewState["id"].ToString()))).FirstOrDefault();
                    ddlGovernmentalEntity.SelectedValue = query.governmentalEntityId.ToString();
                    ddlCompany.SelectedValue = query.companyId.ToString();
                    txtPagesCount.Text = query.noOfPages.ToString();
                    txtStartDateDay.Text = query.startDate.Value.Day.ToString();
                    txtStartDateMonth.Text = query.startDate.Value.Month.ToString();
                    txtStartDateYear.Text = query.startDate.Value.Year.ToString();
                    txtPeriod.Text = query.period.ToString();
                    txtDeliveryDateDay.Text = query.deliveryDate.Value.Day.ToString();
                    txtDeliveryDateMonth.Text = query.deliveryDate.Value.Month.ToString();
                    txtDeliveryDateYear.Text = query.deliveryDate.Value.Year.ToString();
                    txtName.Text = query.name;
                    btnApprove.Visible = btnSave.Visible = query.statusId == (int)StatusEnum.UnderApprrove;
                    btnFreeze.Visible = query.statusId != (int)StatusEnum.Freezed;
                    mpeProject.Show();
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
        mpeProject.Hide();
    }
    private void ClearControls()
    {
        ViewState["id"] = null;
        btnApprove.Visible = btnFreeze.Visible = false;
        btnSave.Visible = true;
        txtDeliveryDateYear.Text = txtDeliveryDateMonth.Text = txtDeliveryDateDay.Text = txtPagesCount.Text = txtPeriod.Text = txtStartDateYear.Text = txtStartDateMonth.Text = txtStartDateDay.Text = txtName.Text = string.Empty;
        ddlCompany.SelectedValue = ddlGovernmentalEntity.SelectedValue = "0";
    }
    protected void btnDelete_Command(object sender, CommandEventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectsPath) && p.Delete.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للحذف');</script>", false);
            return;
        }
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            Project c = db.Projects.Where(x => x.id == int.Parse(e.CommandArgument.ToString())).FirstOrDefault();
            try
            {
                db.Projects.DeleteOnSubmit(c);
                db.SubmitChanges();
                LogWriter.LogWrite("Projects", ((int)ActivitiesEnum.Delete).ToString(), c.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
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
            var query = (from b in db.Projects
                         select new
                         {
                             b.id,
                             b.deliveryDate,
                             governmentalEntity = b.GovernmentalEntity.name,
                             company = b.Company.name,
                             b.noOfPages,
                             b.period,
                             b.startDate,
                             b.name,
                             status=b.Status.name,
                             b.statusId,
                             b.governmentalEntityId,
                             b.companyId
                         });
            if (UserDetails.DeSerializeUserDetails(Session["User"].ToString()).GovernmentalEntityId != 0)
             query = query.Where(x => x.governmentalEntityId == UserDetails.DeSerializeUserDetails(Session["User"].ToString()).GovernmentalEntityId);
            if (txtCompanySrc.Text.Trim() != string.Empty)
                query = query.Where(x => x.company.Contains(txtCompanySrc.Text.Trim()));
            if (txtMinistrySrc.Text.Trim() != string.Empty)
                query = query.Where(x => x.governmentalEntity.Contains(txtMinistrySrc.Text.Trim()));
            if (txtNameSrc.Text.Trim() != string.Empty)
                query = query.Where(x => x.name.Contains(txtNameSrc.Text.Trim()));
            if (txtStartDateFrom.Text.Trim() != string.Empty)
                query = query.Where(x => x.startDate >= Convert.ToDateTime(txtStartDateFrom.Text));
            if (txtEndDateTo.Text.Trim() != string.Empty)
                query = query.Where(x => x.startDate <= Convert.ToDateTime(txtEndDateTo.Text));
            if (txtDeliveryStartDate.Text.Trim() != string.Empty)
                query = query.Where(x => x.deliveryDate >= Convert.ToDateTime(txtDeliveryStartDate.Text));
            if (txtDeliveryEndDate.Text.Trim() != string.Empty)
                query = query.Where(x => x.deliveryDate <= Convert.ToDateTime(txtDeliveryEndDate.Text));
            if (ddlStatusSrc.SelectedValue != "0")
                query = query.Where(x => x.statusId == int.Parse(ddlStatusSrc.SelectedValue));
            dtData = query.CopyToDataTable();
            gdvData.DataSource = dtData;
            gdvData.DataBind();
            lblResult.Text = dtData.Rows.Count.ToString();
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectsPath) && (p.Show.Equals(true) || p.Edit.Equals(true) || p.Delete.Equals(true) || p.Approve.Equals(true) || p.Freze.Equals(true))))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للبحث');</script>", false);
            return;
        }
        BindData();
    }
    protected void btnClearSearch_Click(object sender, EventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectsPath) && (p.Show.Equals(true) || p.Edit.Equals(true) || p.Delete.Equals(true) || p.Approve.Equals(true) || p.Freze.Equals(true))))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للبحث');</script>", false);
            return;
        }
        txtNameSrc.Text = txtMinistrySrc.Text = txtEndDateTo.Text = txtDeliveryStartDate.Text=txtDeliveryEndDate.Text= txtStartDateFrom.Text = string.Empty;
        BindData();
    }

    private void BindDDL()
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            var query = from x in db.GovernmentalEntities
                        where x.statusId == (int)StatusEnum.Approved
                        select new
                        {
                            x.name,
                            x.id
                        };
            ddlGovernmentalEntity.DataSource = query;
            ddlGovernmentalEntity.DataTextField = "name";
            ddlGovernmentalEntity.DataValueField = "id";
            ddlGovernmentalEntity.DataBind();
            ddlGovernmentalEntity.Items.Insert(0, new ListItem("-- اختر --", "0"));

            var query2 = from x in db.Companies
                         where x.statusId==(int)StatusEnum.Approved
                         select new
                         {
                             x.name,
                             x.id
                         };
            ddlCompany.DataSource = query2;
            ddlCompany.DataTextField = "name";
            ddlCompany.DataValueField = "id";
            ddlCompany.DataBind();
            ddlCompany.Items.Insert(0, new ListItem("-- اختر --", "0"));

            ddlStatusSrc.DataSource = db.Status;
            ddlStatusSrc.DataTextField = "name";
            ddlStatusSrc.DataValueField = "id";
            ddlStatusSrc.DataBind();
            ddlStatusSrc.Items.Insert(0, new ListItem("-- اختر --", "0"));
        }
    }

    protected void txtPeriod_TextChanged(object sender, EventArgs e)
    {
        var culture = new CultureInfo("en-US");
        if (txtPeriod.Text.Trim() != string.Empty && txtStartDateDay.Text != string.Empty && txtStartDateMonth.Text != string.Empty && txtStartDateYear.Text != string.Empty)
        {
            txtDeliveryDateDay.Text = DateTime.ParseExact(txtStartDateMonth.Text.PadLeft(2, '0') + "/" + txtStartDateDay.Text.PadLeft(2, '0') + "/" + txtStartDateYear.Text, "MM/dd/yyyy", culture).AddDays(int.Parse(txtPeriod.Text)).Day.ToString().PadLeft(2,'0');
            txtDeliveryDateMonth.Text = DateTime.ParseExact(txtStartDateMonth.Text.PadLeft(2, '0') + "/" + txtStartDateDay.Text.PadLeft(2, '0') + "/" + txtStartDateYear.Text, "MM/dd/yyyy", culture).AddDays(int.Parse(txtPeriod.Text)).Month.ToString().PadLeft(2, '0');
            txtDeliveryDateYear.Text = DateTime.ParseExact(txtStartDateMonth.Text.PadLeft(2, '0') + "/" + txtStartDateDay.Text.PadLeft(2, '0') + "/" + txtStartDateYear.Text, "MM/dd/yyyy", culture).AddDays(int.Parse(txtPeriod.Text)).Year.ToString();
        }
        mpeProject.Show();
    }

    protected void txtStartDate_TextChanged(object sender, EventArgs e)
    {
        txtPeriod_TextChanged(txtPeriod, new EventArgs());
    }
    protected void btnFreeze_Click(object sender, EventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectsPath) && p.Freze.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للتجميد');</script>", false);
            return;
        }
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            Project c = db.Projects.Where(x => x.id == int.Parse(ViewState["id"].ToString())).FirstOrDefault();
            try
            {
                c.statusId = (int)StatusEnum.Freezed;
                db.SubmitChanges();
                LogWriter.LogWrite("Projects", ((int)ActivitiesEnum.Freze).ToString(), c.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                btnFreeze.Visible = btnApprove.Visible = btnSave.Visible = false;
                BindData();
                mpeProject.Show();
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
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectsPath) && p.Approve.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للأعتماد');</script>", false);
            return;
        }
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            Project c = db.Projects.Where(x => x.id == int.Parse(ViewState["id"].ToString())).FirstOrDefault();
            try
            {
                c.statusId = (int)StatusEnum.Approved;
                db.SubmitChanges();
                LogWriter.LogWrite("Projects", ((int)ActivitiesEnum.Approve).ToString(), c.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty); 
                btnApprove.Visible = btnSave.Visible = false;
                btnFreeze.Visible = true;
                BindData();
                mpeProject.Show();
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