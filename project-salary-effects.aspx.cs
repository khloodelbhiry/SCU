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

public partial class project_users : System.Web.UI.Page
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
            lnkHistory.HRef = "log.aspx?t=" + EncryptString.Encrypt("SalaryEffects") + "&p=" + Request.QueryString["id"];
            if (Session["User"] != null && Session["User"].ToString() != string.Empty)
            {
                if (UserPermissions.Any(
                    p =>
                        p.PageUrl.ToLower().Equals(Common.ProjectSalaryEffectsPath) &&
                    (p.Show.Equals(true) || p.Add.Equals(true) || p.Edit.Equals(true) || p.Delete.Equals(true) || p.Approve.Equals(true) || p.Freze.Equals(true))))
                {
                    if (Request.QueryString["id"] != null && Request.QueryString["g"] != null)
                    {
                        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
                        {
                            var per = UserPermissions.FirstOrDefault(p => p.PageUrl.ToLower().Equals(Common.ProjectSalaryEffectsPath));
                            Project query = db.Projects.FirstOrDefault(x => x.id == int.Parse(EncryptString.Decrypt(Request.QueryString["id"])));
                            ((HtmlGenericControl)Page.Master.FindControl("ulBreadcrumb")).InnerHtml = "<li><i class='ace-icon fa fa-home home-icon'></i><a href ='Default.aspx'> الرئيسية </a></li><li>" + per.ModuleName + "</li><li><strong>المشروع : " + query.name + "</strong></li><li><strong>الجهة : " + query.GovernmentalEntity.name + "</strong></li><li><strong>الشركة المنفذة : " + query.Company.name + "</strong></li><li><a class='active'>" + per.PageName + "</a></li>";
                            Page.Title = per.PageName;
                        }
                    }
                    else
                        Response.Redirect("projects.aspx");
                }
                else
                    Response.Redirect("project-users-no-permission.aspx?id="+Request.QueryString["id"] + "&g=" + Request.QueryString["g"] + "&c=" + Request.QueryString["c"]);
            }
            else
                Response.Redirect("Login.aspx?ReturnURL=" + Request.Url.AbsolutePath);
            BindDDL();
            if (UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectSalaryEffectsPath) && (p.Show.Equals(true) || p.Edit.Equals(true) || p.Delete.Equals(true) || p.Approve.Equals(true) || p.Freze.Equals(true))))
                BindData();
        }
    }
    private void BindDDL()
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            var query1 = from q in db.Groups
                         select new
                         {
                             q.id,
                             q.name
                         };
            ddlGroupSrc.DataSource = query1;
            ddlGroupSrc.DataTextField = "name";
            ddlGroupSrc.DataValueField = "id";
            ddlGroupSrc.DataBind();
            ddlGroupSrc.Items.Insert(0, new ListItem("-- اختر --", "0"));

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

            var query3 = from q in db.Users
                         where q.projectId==int.Parse(EncryptString.Decrypt(Request.QueryString["id"]))
                         select new
                         {
                             q.id,
                             q.fullName
                         };
            ddlUsers.DataSource = query3;
            ddlUsers.DataTextField = "fullName";
            ddlUsers.DataValueField = "id";
            ddlUsers.DataBind();
            ddlUsers.Items.Insert(0, new ListItem("-- اختر --", "0"));
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
            try
            {
                var query = (from b in db.SalaryEffects
                             where b.projectId == int.Parse(EncryptString.Decrypt(Request.QueryString["id"]))
                             && b.salaryEffectTypeId == int.Parse(Request.QueryString["t"])
                             select new
                             {
                                 b.id,
                                 b.User.fullName,
                                 b.User.email,
                                 b.User.groupId,
                                 b.statusId,
                                 Group = b.User.Group.name,
                                 Status = b.Status.name,
                                 b.date,
                                 b.timeFrom,
                                 b.timeTo
                             });
                if (ddlStatusSrc.SelectedValue != "0")
                    query = query.Where(x => x.statusId.Equals(int.Parse(ddlStatusSrc.SelectedValue)));
                if (ddlGroupSrc.SelectedValue != "0")
                    query = query.Where(x => x.groupId.Equals(int.Parse(ddlGroupSrc.SelectedValue)));
                if (txtElecMSrc.Text.Trim() != string.Empty)
                    query = query.Where(x => x.email.Equals(txtElecMSrc.Text.Trim()));
                if (txtNameSrc.Text.Trim() != string.Empty)
                    query = query.Where(x => x.fullName.Contains(txtNameSrc.Text.Trim()));
                if (txtDateFrom.Text.Trim() != string.Empty)
                    query = query.Where(x => x.date >= Convert.ToDateTime(txtDateFrom.Text.Trim()));
                if (txtDateTo.Text.Trim() != string.Empty)
                    query = query.Where(x => x.date <= Convert.ToDateTime(txtDateTo.Text.Trim()));
                dtData = query.OrderByDescending(x => x.statusId).CopyToDataTable();
                gdvData.DataSource = dtData;
                gdvData.DataBind();
                lblResult.Text = dtData.Rows.Count.ToString();
            }
            catch (Exception ex)
            {
                Common.InsertException(ex.Message, ex.StackTrace,
                          new System.IO.FileInfo(System.Web.HttpContext.Current.Request.Url.AbsolutePath).Name);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('حدث خطا اثناء الحفظ');</script>", false);
            }
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectSalaryEffectsPath) && p.Show.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للبحث');</script>", false);
            return;
        }
        BindData();
    }
    protected void btnClearSearch_Click(object sender, EventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectSalaryEffectsPath) && p.Show.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للبحث');</script>", false);
            return;
        }
        txtNameSrc.Text = txtDateTo.Text = txtDateFrom.Text = txtElecMSrc.Text = string.Empty;
        ddlStatusSrc.SelectedValue = ddlGroupSrc.SelectedValue = "0";
        BindData();
    }
    protected void btnClose_Click(object sender, EventArgs e)
    {
        ViewState["id"] = null;
        ClearControls();
        mpeUser.Hide();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            try
            {
                if (TimeSpan.Parse(txtTimeFromH.Text + ":" + txtTimeFromM.Text) >= TimeSpan.Parse(txtTimeToH.Text + ":" + txtTimeToM.Text))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('الوقت من يجب ان يكون اقل من (الوقت الى)');</script>", false);
                    mpeUser.Show();
                    return;
                }
                if (ViewState["id"] != null)
                {
                    SalaryEffect q = db.SalaryEffects.FirstOrDefault(x => x.id.Equals(int.Parse(ViewState["id"].ToString())));
                    q.date = new DateTime(int.Parse(txtDateYear.Text.Trim()), int.Parse(txtDateMonth.Text.Trim()), int.Parse(txtDateDay.Text.Trim()));
                    q.reason = txtReason.Text.Trim();
                    q.timeFrom = TimeSpan.Parse(txtTimeFromH.Text + ":" + txtTimeFromM.Text);
                    q.timeTo = TimeSpan.Parse(txtTimeToH.Text + ":" + txtTimeToM.Text);
                    db.SubmitChanges();
                    LogWriter.LogWrite("SalaryEffects", ((int)ActivitiesEnum.Update).ToString(), q.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, EncryptString.Decrypt(Request.QueryString["id"]), string.Empty);
                }
                else
                {
                    if (rdAll.Checked)
                    {
                        var query = db.Users.Where(x => x.projectId == int.Parse(EncryptString.Decrypt(Request.QueryString["id"]))).Select(x => x.id);
                        foreach (var i in query)
                        {
                            Save(int.Parse(i.ToString()));
                        }
                    }
                    else
                    {
                        if (ddlUsers.SelectedValue == "0")
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('يرجى اختيار الموظف اولا');</script>", false);
                            mpeUser.Show();
                            return;
                        }
                        Save(int.Parse(ddlUsers.SelectedValue));
                    }
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('تم الحفظ بنجاح');</script>", false);
                ClearControls();
                mpeUser.Hide();
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
    private void Save(int userId)
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            SalaryEffect q = new SalaryEffect();
            q.userId = userId;
            q.salaryEffectTypeId = int.Parse(Request.QueryString["t"]);
            q.projectId = int.Parse(EncryptString.Decrypt(Request.QueryString["id"]));
            q.date = new DateTime(int.Parse(txtDateYear.Text.Trim()), int.Parse(txtDateMonth.Text.Trim()), int.Parse(txtDateDay.Text.Trim()));
            q.reason = txtReason.Text.Trim();
            q.timeFrom = TimeSpan.Parse(txtTimeFromH.Text + ":" + txtTimeFromM.Text);
            q.timeTo = TimeSpan.Parse(txtTimeToH.Text + ":" + txtTimeToM.Text);
            q.statusId = (int)StatusEnum.UnderApprrove;
            db.SalaryEffects.InsertOnSubmit(q);
            db.SubmitChanges();
            LogWriter.LogWrite("SalaryEffects", ((int)ActivitiesEnum.Add).ToString(), q.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, EncryptString.Decrypt(Request.QueryString["id"]), string.Empty);
        }
    }
    private void ClearControls()
    {
        btnApprove.Visible = btnFreeze.Visible = false;
        btnSave.Visible = true;
        txtDateDay.Text = txtDateYear.Text = txtDateMonth.Text = txtReason.Text = txtTimeFromH.Text = txtTimeFromM.Text = txtTimeToH.Text = txtTimeToM.Text = string.Empty;
        ViewState["id"] = null;
        rdAll.Checked = true;
        ddlUsers.SelectedValue = "0";
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectSalaryEffectsPath) && p.Add.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للاضافة');</script>", false);
            return;
        }
        mpeUser.Show();
    }

    protected void btnDelete_Command(object sender, CommandEventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectSalaryEffectsPath) && p.Delete.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للحذف');</script>", false);
            return;
        }
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            SalaryEffect c = db.SalaryEffects.Where(x => x.id == int.Parse(e.CommandArgument.ToString())).FirstOrDefault();
            try
            {
                db.SalaryEffects.DeleteOnSubmit(c);
                db.SubmitChanges();
                LogWriter.LogWrite("SalaryEffects", ((int)ActivitiesEnum.Delete).ToString(), c.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, EncryptString.Decrypt(Request.QueryString["id"]), string.Empty);
                BindData();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('تم الحذف بنجاح'); </script>", false);
            }
            catch (Exception ex)
            {
                Common.InsertException(ex.Message, ex.StackTrace,
                      new System.IO.FileInfo(System.Web.HttpContext.Current.Request.Url.AbsolutePath).Name);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('حدث خطأ اثناء الحذف'); </script>", false);
            }
        }
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectSalaryEffectsPath) && p.Edit.Equals(true)))
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
                    var query = db.SalaryEffects.Where(t => t.id.Equals(int.Parse(ViewState["id"].ToString()))).FirstOrDefault();
                    txtDateDay.Text = query.date.Day.ToString().PadLeft(2,'0');
                    txtDateMonth.Text = query.date.Month.ToString().PadLeft(2, '0');
                    txtDateYear.Text = query.date.Year.ToString().PadLeft(4, '0');
                    txtReason.Text = query.reason;
                    txtTimeFromH.Text=query.timeFrom.Hours.ToString().PadLeft(2, '0');
                    txtTimeToH.Text = query.timeTo.Hours.ToString().PadLeft(2, '0');
                    txtTimeFromM.Text = query.timeFrom.Minutes.ToString().PadLeft(2, '0');
                    txtTimeToM.Text = query.timeTo.Minutes.ToString().PadLeft(2, '0');
                    btnApprove.Visible = btnSave.Visible = query.statusId == (int)StatusEnum.UnderApprrove;
                    btnFreeze.Visible = query.statusId != (int)StatusEnum.Freezed;
                    mpeUser.Show();
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
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectSalaryEffectsPath) && p.Freze.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للتجميد');</script>", false);
            return;
        }
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            SalaryEffect c = db.SalaryEffects.Where(x => x.id == int.Parse(ViewState["id"].ToString())).FirstOrDefault();
            try
            {
                c.statusId = (int)StatusEnum.Freezed;
                db.SubmitChanges();
                LogWriter.LogWrite("SalaryEffects", ((int)ActivitiesEnum.Freze).ToString(), c.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, EncryptString.Decrypt(Request.QueryString["id"]), string.Empty);
                btnFreeze.Visible = btnApprove.Visible = btnSave.Visible = false;
                BindData();
                mpeUser.Show();
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
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectSalaryEffectsPath) && p.Approve.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للأعتماد');</script>", false);
            return;
        }
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            SalaryEffect c = db.SalaryEffects.Where(x => x.id == int.Parse(ViewState["id"].ToString())).FirstOrDefault();
            try
            {
                c.statusId = (int)StatusEnum.Approved;
                db.SubmitChanges();
                LogWriter.LogWrite("SalaryEffects", ((int)ActivitiesEnum.Approve).ToString(), c.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, EncryptString.Decrypt(Request.QueryString["id"]), string.Empty);
                btnApprove.Visible = btnSave.Visible = false;
                btnFreeze.Visible = true;
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
}