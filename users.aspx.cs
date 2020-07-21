using System;
using System.Collections.Generic;
using System.Data;
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
            lnkHistory.HRef = "log.aspx?t=" + EncryptString.Encrypt("Users");
            if (Session["User"] != null && Session["User"].ToString() != string.Empty)
            {
                if (UserPermissions.Any(
                    p =>
                        p.PageUrl.ToLower().Equals(Common.UsersPath) &&
                    (p.Show.Equals(true) || p.Add.Equals(true) || p.Edit.Equals(true) || p.Delete.Equals(true) || p.Approve.Equals(true) || p.Freze.Equals(true))))
                {
                        var per = UserPermissions.FirstOrDefault(p => p.PageUrl.ToLower().Equals(Common.UsersPath));
                        ((HtmlGenericControl)Page.Master.FindControl("ulBreadcrumb")).InnerHtml = "<li><i class='ace-icon fa fa-home home-icon'></i><a href ='Default.aspx'> الرئيسية </a></li><li>" + per.ModuleName + "</li><li><a class='active'>" + per.PageName + "</a></li>";
                        Page.Title = per.PageName;
                }
                else
                    Response.Redirect("no-permission.aspx");
            }
            else
                Response.Redirect("Login.aspx?ReturnURL=" + Request.Url.AbsolutePath);
            BindDDL();
            if (UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.UsersPath) && (p.Show.Equals(true) || p.Edit.Equals(true) || p.Delete.Equals(true) || p.Approve.Equals(true) || p.Freze.Equals(true))))
                BindData();
        }
    }
    private void BindDDL()
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
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

            var query4 = from q in db.Groups
                         select new
                         {
                             q.id,
                             q.name
                         };
            ddlGroup.DataSource = query4;
            ddlGroup.DataTextField = "name";
            ddlGroup.DataValueField = "id";
            ddlGroup.DataBind();
            ddlGroup.Items.Insert(0, new ListItem("-- اختر --", "0"));

            var query5 = from q in db.UnitStructures
                         where q.governmentalEntityId==8
                         select new
                         {
                             q.id,
                             q.name
                         };
            ddlUnit.DataSource = query5;
            ddlUnit.DataTextField = "name";
            ddlUnit.DataValueField = "id";
            ddlUnit.DataBind();
            ddlUnit.Items.Insert(0, new ListItem("-- اختر --", "0"));
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
                var query = (from b in db.Users
                             where b.governmentalEntityId != null 
                             select new
                             {
                                 b.id,
                                 b.fullName,
                                 b.email,
                                 b.mobile,
                                 b.groupId,
                                 b.statusId,
                                 Group = b.Group.name,
                                 Status = b.Status.name,
                                 IsCheckBoxVisible = !(b.hasWorkLicense ?? false),
                                 unit=b.UnitStructure.name
                             });
                if (ddlStatusSrc.SelectedValue != "0")
                    query = query.Where(x => x.statusId.Equals(int.Parse(ddlStatusSrc.SelectedValue)));
                if (txtElecMSrc.Text.Trim() != string.Empty)
                    query = query.Where(x => x.email.Equals(txtElecMSrc.Text.Trim()));
                if (txtMobileSrc.Text.Trim() != string.Empty)
                    query = query.Where(x => x.mobile.Equals(txtMobileSrc.Text.Trim()));
                if (txtNameSrc.Text.Trim() != string.Empty)
                    query = query.Where(x => x.fullName.Contains(txtNameSrc.Text.Trim()));
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
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.UsersPath) && p.Show.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للبحث');</script>", false);
            return;
        }
        BindData();
    }
    protected void btnClearSearch_Click(object sender, EventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.UsersPath) && p.Show.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للبحث');</script>", false);
            return;
        }
        txtNameSrc.Text = txtMobileSrc.Text = txtElecMSrc.Text = string.Empty;
        ddlStatusSrc.SelectedValue= "0";
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
                if (ViewState["id"] != null)
                {
                    User q = db.Users.FirstOrDefault(x => x.id.Equals(int.Parse(ViewState["id"].ToString())));
                    q.fullName = txtFullName.Text.Trim();
                    q.email = txtElecM.Text.Trim();
                    q.mobile = txtMobileReg.Text.Trim();
                    q.groupId = ddlGroup.SelectedValue != "0" ? (int?)int.Parse(ddlGroup.SelectedValue) : null;
                    q.unitStructureId = int.Parse(ddlUnit.SelectedValue);
                    db.SubmitChanges();
                    LogWriter.LogWrite("Users", ((int)ActivitiesEnum.Update).ToString(), q.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                }
                else
                {
                    User q = new User();
                    q.fullName = txtFullName.Text.Trim();
                    q.email = txtElecM.Text.Trim();
                    q.password = EncryptString.Encrypt("123");
                    q.mobile = txtMobileReg.Text.Trim();
                    q.groupId = ddlGroup.SelectedValue != "0" ? (int?)int.Parse(ddlGroup.SelectedValue) : null;
                    q.statusId = (int)StatusEnum.UnderApprrove;
                    q.governmentalEntityId = 8;
                    q.unitStructureId = int.Parse(ddlUnit.SelectedValue);
                    db.Users.InsertOnSubmit(q);
                    db.SubmitChanges();
                    LogWriter.LogWrite("Users", ((int)ActivitiesEnum.Add).ToString(), q.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
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

    private void ClearControls()
    {
        btnApprove.Visible = btnFreeze.Visible = false;
        btnSave.Visible = true;
        txtElecM.Text = txtFullName.Text = txtMobileReg.Text = string.Empty;
       ddlGroup.SelectedValue= ddlUnit.SelectedValue = "0";
        ViewState["id"] = null;
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.UsersPath) && p.Add.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للاضافة');</script>", false);
            return;
        }
        mpeUser.Show();
    }

    protected void btnDelete_Command(object sender, CommandEventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.UsersPath) && p.Delete.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للحذف');</script>", false);
            return;
        }
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            User c = db.Users.Where(x => x.id == int.Parse(e.CommandArgument.ToString())).FirstOrDefault();
            try
            {
                db.Users.DeleteOnSubmit(c);
                db.SubmitChanges();
                LogWriter.LogWrite("Users", ((int)ActivitiesEnum.Delete).ToString(), c.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
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
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.UsersPath) && p.Edit.Equals(true)))
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
                    var query = db.Users.Where(t => t.id.Equals(int.Parse(ViewState["id"].ToString()))).FirstOrDefault();
                    txtFullName.Text = query.fullName;
                    txtElecM.Text = query.email;
                    txtMobileReg.Text = query.mobile;
                    ddlGroup.SelectedValue = query.groupId != null ? query.groupId.ToString() : "0";
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
    protected void cvMobileReg_ServerValidate(object source, ServerValidateEventArgs args)
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            args.IsValid = !db.Users.Any(
                u =>
                    u.mobile.Equals(txtMobileReg.Text.Trim()));
        }
    }

    protected void lnkWorkLicense_Click(object sender, EventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.UsersPath) && p.Edit.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للتعديل');</script>", false);
            return;
        }
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            try
            {
                int checkedRows = 0;
                foreach (GridViewRow row in gdvData.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        HtmlInputCheckBox chkRow = (row.Cells[1].FindControl("chkRow") as HtmlInputCheckBox);
                        if (chkRow.Checked)
                        {
                            checkedRows += 1;
                            HiddenField hdfID = (row.Cells[0].FindControl("hdfID") as HiddenField);
                            User u = db.Users.FirstOrDefault(x => x.id == int.Parse(hdfID.Value));
                            u.hasWorkLicense = true;
                            db.SubmitChanges();
                            LogWriter.LogWrite("Users", ((int)ActivitiesEnum.Update).ToString(), u.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                        }
                    }
                }
                if (checkedRows == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert(' تأكد من اختيار المستخدمين. ');</script>", false);
                    return;
                }
                BindData();
                ScriptManager.RegisterStartupScript(this, GetType(), "alertUser",
                   "alert('تم الحفظ بنجاح');", true);
            }
            catch (Exception ex)
            {
                Common.InsertException(ex.Message, ex.StackTrace,
                          new System.IO.FileInfo(System.Web.HttpContext.Current.Request.Url.AbsolutePath).Name);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('حدث خطا اثناء الحفظ');</script>", false);
            }
        }
    }
    protected void btnFreeze_Click(object sender, EventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.UsersPath) && p.Freze.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للتجميد');</script>", false);
            return;
        }
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            User c = db.Users.Where(x => x.id == int.Parse(ViewState["id"].ToString())).FirstOrDefault();
            try
            {
                c.statusId = (int)StatusEnum.Freezed;
                db.SubmitChanges();
                LogWriter.LogWrite("Users", ((int)ActivitiesEnum.Freze).ToString(), c.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
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
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.UsersPath) && p.Approve.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للأعتماد');</script>", false);
            return;
        }
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            User c = db.Users.Where(x => x.id == int.Parse(ViewState["id"].ToString())).FirstOrDefault();
            try
            {
                if (c.groupId != null)
                {
                    c.statusId = (int)StatusEnum.Approved;
                    db.SubmitChanges();
                    LogWriter.LogWrite("Users", ((int)ActivitiesEnum.Approve).ToString(), c.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                    btnApprove.Visible = btnSave.Visible = false;
                    btnFreeze.Visible = true;
                    BindData();
                }
                else
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، يرجى اختيار مجموعة الصلاحيات اولا');</script>", false);
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