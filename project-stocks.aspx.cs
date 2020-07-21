using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class poject_stocks : System.Web.UI.Page
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
            lnkHistory.HRef = "log.aspx?t=" + EncryptString.Encrypt("Stocks") + "&p=" + Request.QueryString["id"];
            if (Session["User"] != null && Session["User"].ToString() != string.Empty)
            {
                if (UserPermissions.Any(
                    p =>
                        p.PageUrl.ToLower().Equals(Common.ProjectStocksPath) &&
                        (p.Show.Equals(true) || p.Add.Equals(true) || p.Edit.Equals(true) || p.Delete.Equals(true) || p.Approve.Equals(true) || p.Freze.Equals(true))))
                {
                    if (Request.QueryString["id"] != null && Request.QueryString["g"] != null)
                    {
                        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
                        {
                            var per = UserPermissions.FirstOrDefault(p => p.PageUrl.ToLower().Equals(Common.ProjectStocksPath));
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
            if (UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectStocksPath) && (p.Show.Equals(true) || p.Edit.Equals(true) || p.Delete.Equals(true) || p.Approve.Equals(true) || p.Freze.Equals(true))))
                BindData();
        }
    }
    private void BindDDL()
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            var query1 = from q in db.Governorates
                         select new
                         {
                             q.id,
                             q.name
                         };
            ddlGovernorate.DataSource = query1;
            ddlGovernorate.DataTextField = "name";
            ddlGovernorate.DataValueField = "id";
            ddlGovernorate.DataBind();
            ddlGovernorate.Items.Insert(0, new ListItem("-- اختر --", "0"));

            ddlGovernorateSrc.DataSource = query1;
            ddlGovernorateSrc.DataTextField = "name";
            ddlGovernorateSrc.DataValueField = "id";
            ddlGovernorateSrc.DataBind();
            ddlGovernorateSrc.Items.Insert(0, new ListItem("-- اختر --", "0"));
            ddlGovernorateSrc_SelectedIndexChanged(ddlGovernorateSrc, new EventArgs());

            var query2 = from q in db.Status
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
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectStocksPath) && p.Add.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للاضافة');</script>", false);
            return;
        }
        mpeStock.Show();
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
                        Stock s = db.Stocks.OrderByDescending(x => Convert.ToInt16(x.code)).FirstOrDefault();
                        Stock q = new Stock()
                        {
                            code = (s != null ? Convert.ToInt16(s.code) + 1 : 0).ToString().PadLeft(4, '0'),
                            notes = txtNotes.Text,
                            address = txtAddress.Text,
                            administrator = txtAdmin.Text.Trim(),
                            cityId = ddlCity.SelectedValue != "0" ? (int?)int.Parse(ddlCity.SelectedValue) : null,
                            mobile = txtMobile.Text.Trim(),
                            name = txtName.Text.Trim(),
                            statusId = (int)StatusEnum.UnderApprrove,
                            projectId = int.Parse(EncryptString.Decrypt(Request.QueryString["id"])),
                            lat = hdfLatitude.Value,
                            lng = hdfLongitude.Value
                        };
                        db.Stocks.InsertOnSubmit(q);
                        db.SubmitChanges();
                        LogWriter.LogWrite("Stocks", ((int)ActivitiesEnum.Add).ToString(), q.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, EncryptString.Decrypt(Request.QueryString["id"]), string.Empty);
                    }
                    else
                    {
                        Stock q = db.Stocks.FirstOrDefault(x => x.id.Equals(int.Parse(ViewState["ID"].ToString())));
                        q.notes = txtNotes.Text;
                        q.address = txtAddress.Text;
                        q.administrator = txtAdmin.Text.Trim();
                        q.cityId = ddlCity.SelectedValue != "0" ? (int?)int.Parse(ddlCity.SelectedValue) : null;
                        q.mobile = txtMobile.Text.Trim();
                        q.name = txtName.Text.Trim();
                        q.lat = hdfLatitude.Value;
                        q.lng = hdfLongitude.Value;
                        db.SubmitChanges();
                        LogWriter.LogWrite("Stocks", ((int)ActivitiesEnum.Update).ToString(), q.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, EncryptString.Decrypt(Request.QueryString["id"]), string.Empty);
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
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectStocksPath) && p.Edit.Equals(true)))
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
                try
                {
                    var query = db.Stocks.Where(t => t.id.Equals(int.Parse(ViewState["ID"].ToString()))).FirstOrDefault();
                    txtNotes.Text = query.notes;
                    txtName.Text = query.name;
                    txtMobile.Text = query.mobile;
                    txtCode.Text = query.code;
                    txtAdmin.Text = query.administrator;
                    txtAddress.Text = query.address;
                    hdfLatitude.Value = query.lat;
                    hdfLongitude.Value = query.lng;
                    ddlGovernorate.SelectedValue = query.cityId != null ? query.City.governorateId.ToString() : "0";
                    ddlGovernorate_SelectedIndexChanged(ddlGovernorate, new EventArgs());
                    ddlCity.SelectedValue = query.cityId != null ? query.cityId.ToString() : "0";
                    btnApprove.Visible = btnSave.Visible = query.statusId == (int)StatusEnum.UnderApprrove;
                    btnFreeze.Visible = query.statusId != (int)StatusEnum.Freezed;
                    mpeStock.Show();
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
        ViewState["ID"] = null;
        mpeStock.Hide();
    }
    private void ClearControls()
    {
        ViewState["ID"] = null;
        txtAddress.Text = txtAdmin.Text = txtMobile.Text = txtNotes.Text = txtName.Text = string.Empty;
        ddlGovernorate.SelectedValue = "0";
        ddlGovernorate_SelectedIndexChanged(ddlGovernorate, new EventArgs());
        btnApprove.Visible = btnFreeze.Visible = false;
        btnSave.Visible = true;
    }
    protected void btnDelete_Command(object sender, CommandEventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectStocksPath) && p.Delete.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للحذف');</script>", false);
            return;
        }
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            Stock c = db.Stocks.Where(x => x.id == int.Parse(e.CommandArgument.ToString())).FirstOrDefault();
            try
            {
                db.Stocks.DeleteOnSubmit(c);
                db.SubmitChanges();
                LogWriter.LogWrite("Stocks", ((int)ActivitiesEnum.Delete).ToString(), c.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, EncryptString.Decrypt(Request.QueryString["id"]), string.Empty);
                BindData();
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
            var query = (from b in db.Stocks
                         where b.projectId==int.Parse(EncryptString.Decrypt(Request.QueryString["id"]))
                         select new
                         {
                             b.id,
                             b.statusId,
                             b.notes,
                             b.name,
                             b.mobile,
                             b.code,
                             b.cityId,
                             b.administrator,
                             b.address,
                             b.City.governorateId,
                             governorate = b.City.Governorate.name,
                             city = b.City.name,
                             status = b.Status.name
                         });
            if (ddlStatusSrc.SelectedValue != "0")
                query = query.Where(x => x.statusId.Equals(int.Parse(ddlStatusSrc.SelectedValue)));
            if (ddlGovernorateSrc.SelectedValue != "0")
                query = query.Where(x => x.governorateId.Equals(int.Parse(ddlGovernorateSrc.SelectedValue)));
            if (ddlCitySrc.SelectedValue != "0")
                query = query.Where(x => x.cityId.Equals(int.Parse(ddlCitySrc.SelectedValue)));
            if (txtCodeSrc.Text.Trim() != string.Empty)
                query = query.Where(x => x.code.Equals(txtCodeSrc.Text.Trim()));
            if (txtAdminSrc.Text.Trim() != string.Empty)
                query = query.Where(x => x.administrator.Contains(txtAdminSrc.Text.Trim()));
            if (txtMobileSrc.Text.Trim() != string.Empty)
                query = query.Where(x => x.mobile.Equals(txtMobileSrc.Text.Trim()));
            if (txtNameSrc.Text.Trim() != string.Empty)
                query = query.Where(x => x.name.Contains(txtNameSrc.Text.Trim()));
            dtData = query.CopyToDataTable();
            gdvData.DataSource = dtData;
            gdvData.DataBind();
            lblResult.Text = dtData.Rows.Count.ToString();
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectStocksPath) && (p.Show.Equals(true) || p.Edit.Equals(true) || p.Delete.Equals(true) || p.Approve.Equals(true) || p.Freze.Equals(true))))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للبحث');</script>", false);
            return;
        }
        BindData();
    }
    protected void btnClearSearch_Click(object sender, EventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectStocksPath) && (p.Show.Equals(true) || p.Edit.Equals(true) || p.Delete.Equals(true) || p.Approve.Equals(true) || p.Freze.Equals(true))))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للبحث');</script>", false);
            return;
        }
        txtNameSrc.Text = txtMobileSrc.Text = txtCodeSrc.Text = txtAdminSrc.Text = string.Empty;
        ddlStatusSrc.SelectedValue = ddlGovernorateSrc.SelectedValue = "0";
        ddlGovernorateSrc_SelectedIndexChanged(ddlGovernorateSrc, new EventArgs());
        BindData();
    }

    protected void ddlGovernorateSrc_SelectedIndexChanged(object sender, EventArgs e)
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            var query = from q in db.Cities
                        where q.governorateId.Equals(int.Parse(ddlGovernorateSrc.SelectedValue))
                        select new
                        {
                            q.id,
                            q.name
                        };
            ddlCitySrc.DataSource = query;
            ddlCitySrc.DataTextField = "name";
            ddlCitySrc.DataValueField = "id";
            ddlCitySrc.DataBind();
            ddlCitySrc.Items.Insert(0, new ListItem("-- اختر --", "0"));
        }
    }
    protected void ddlGovernorate_SelectedIndexChanged(object sender, EventArgs e)
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            var query = from q in db.Cities
                        where q.governorateId.Equals(int.Parse(ddlGovernorate.SelectedValue))
                        select new
                        {
                            q.id,
                            q.name
                        };
            ddlCity.DataSource = query;
            ddlCity.DataTextField = "name";
            ddlCity.DataValueField = "id";
            ddlCity.DataBind();
            ddlCity.Items.Insert(0, new ListItem("-- اختر --", "0"));
            mpeStock.Show();
        }
    }
    protected void btnFreeze_Click(object sender, EventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectStocksPath) && p.Freze.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للتجميد');</script>", false);
            return;
        }
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            Stock c = db.Stocks.Where(x => x.id == int.Parse(ViewState["ID"].ToString())).FirstOrDefault();
            try
            {
                c.statusId = (int)StatusEnum.Freezed;
                db.SubmitChanges();
                LogWriter.LogWrite("Stocks", ((int)ActivitiesEnum.Freze).ToString(), c.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, EncryptString.Decrypt(Request.QueryString["id"]), string.Empty);
                btnFreeze.Visible = btnApprove.Visible = btnSave.Visible = false;
                BindData();
                mpeStock.Show();
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
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectStocksPath) && p.Approve.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للأعتماد');</script>", false);
            return;
        }
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            Stock c = db.Stocks.Where(x => x.id == int.Parse(ViewState["ID"].ToString())).FirstOrDefault();
            try
            {
                c.statusId = (int)StatusEnum.Approved;
                db.SubmitChanges();
                LogWriter.LogWrite("Stocks", ((int)ActivitiesEnum.Approve).ToString(), c.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, EncryptString.Decrypt(Request.QueryString["id"]), string.Empty);
                btnApprove.Visible = btnSave.Visible = false;
                btnFreeze.Visible = true;
                BindData();
                mpeStock.Show();
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