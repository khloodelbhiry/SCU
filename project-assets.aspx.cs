using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class issues : System.Web.UI.Page
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
            if (Session["User"] != null && Session["User"].ToString() != string.Empty)
            {
                if (UserPermissions.Any(
                    p =>
                        p.PageUrl.ToLower().Equals(Common.ProjectAssetsPath) &&
                        (p.Show.Equals(true) || p.Add.Equals(true) || p.Edit.Equals(true) || p.Delete.Equals(true) || p.Approve.Equals(true) || p.Freze.Equals(true))))
                {
                    if (Request.QueryString["id"] != null && Request.QueryString["g"] != null)
                    {
                        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
                        {
                            var per = UserPermissions.FirstOrDefault(p => p.PageUrl.ToLower().Equals(Common.ProjectAssetsPath));
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
            if (UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectAssetsPath) && (p.Show.Equals(true) || p.Edit.Equals(true) || p.Delete.Equals(true) || p.Approve.Equals(true) || p.Freze.Equals(true))))
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
                         where b.projectId == int.Parse(EncryptString.Decrypt(Request.QueryString["id"]))
                         select new
                         {
                             b.id,
                             b.model,
                             b.name,
                             b.serialNo,
                             b.assetsStatusId,
                             status = b.AssetsStatus.name,
                             deliveryDate = db.AssetsAssignments.Where(x => x.projectId == b.projectId && x.assetsId == b.id).OrderByDescending(x => x.id).FirstOrDefault().deliveryDate
                         });
            if (ddlStatusSrc.SelectedValue != "0")
                query = query.Where(x => x.assetsStatusId.Equals(int.Parse(ddlStatusSrc.SelectedValue)));
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
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectAssetsPath) && (p.Show.Equals(true) || p.Edit.Equals(true) || p.Delete.Equals(true) || p.Approve.Equals(true) || p.Freze.Equals(true))))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للبحث');</script>", false);
            return;
        }
        BindData();
    }
    protected void btnClearSearch_Click(object sender, EventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectAssetsPath) && (p.Show.Equals(true) || p.Edit.Equals(true) || p.Delete.Equals(true) || p.Approve.Equals(true) || p.Freze.Equals(true))))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للبحث');</script>", false);
            return;
        }
        txtNameSrc.Text = txtSerNoSrc.Text = txtModelSrc.Text = string.Empty;
        ddlStatusSrc.SelectedValue = "0";
        BindData();
    }

    protected void lnkDelivered_Command(object sender, CommandEventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectAssetsPath) && p.Approve.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للأعتماد');</script>", false);
            return;
        }
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            AssetsAssignment c = db.AssetsAssignments.Where(x =>x.assetsId==int.Parse(e.CommandArgument.ToString())&& x.projectId == int.Parse(EncryptString.Decrypt(Request.QueryString["id"]))).FirstOrDefault();
            try
            {
                c.deliveryDate = DateTime.Now;
                c.deliveredBy = UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID;
                db.SubmitChanges(); 
                BindData();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('تم الحفظ بنجاح');</script>", false);
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