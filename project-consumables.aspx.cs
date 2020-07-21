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
            HtmlAnchor lnkHistory = (HtmlAnchor)Master.FindControl("lnkHistory");
            lnkHistory.HRef = "log.aspx?t=" + EncryptString.Encrypt("Consumables") + "&p=" + Request.QueryString["id"];
            if (Session["User"] != null && Session["User"].ToString() != string.Empty)
            {
                if (UserPermissions.Any(
                    p =>
                        p.PageUrl.ToLower().Equals(Common.ProjectConsumablesPath) &&
                        (p.Show.Equals(true) || p.Approve.Equals(true))))
                {
                    if (Request.QueryString["id"] != null && Request.QueryString["g"] != null)
                    {
                        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
                        {
                            var per = UserPermissions.FirstOrDefault(p => p.PageUrl.ToLower().Equals(Common.ProjectConsumablesPath));
                            Project query = db.Projects.FirstOrDefault(x => x.id == int.Parse(EncryptString.Decrypt(Request.QueryString["id"])));
                            ((HtmlGenericControl)Page.Master.FindControl("ulBreadcrumb")).InnerHtml = "<li><i class='ace-icon fa fa-home home-icon'></i><a href ='Default.aspx'> الرئيسية </a></li><li>" + per.ModuleName + "</li><li><strong>المشروع : " + query.name + "</strong></li><li><strong>الجهة : " + query.GovernmentalEntity.name + "</strong></li><li><strong>الشركة المنفذة : " + query.Company.name + "</strong></li><li><a class='active'>" + per.PageName + "</a></li>";
                            Page.Title = per.PageName;
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
            BindDDL();
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
            var query = (from b in db.ConsumableDistributions
                         where b.projectId == int.Parse(EncryptString.Decrypt(Request.QueryString["id"]))
                         select new
                         {
                             b.id,
                             type = b.Consumable.ConsumableType.name,
                             b.Consumable.description,
                             b.quantity,
                             b.assignmentDate,
                             assignedBy = b.User.fullName,
                             b.deliveryDate,
                             deliveredBy = b.User1.fullName,
                             b.Consumable.typeId
                         });
            if (ddlTypeSrc.SelectedValue != "0")
                query = query.Where(x => x.typeId.Equals(int.Parse(ddlTypeSrc.SelectedValue)));
            if (ddlStatusSrc.SelectedValue == "1")
                query = query.Where(x => x.deliveryDate==null);
            if (ddlStatusSrc.SelectedValue == "2")
                query = query.Where(x => x.deliveryDate != null);
            dtData = query.OrderByDescending(x=>x.id).CopyToDataTable();
            gdvData.DataSource = dtData;
            gdvData.DataBind();
            lblResult.Text = dtData.Rows.Count.ToString();
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindData();
    }
    protected void btnClearSearch_Click(object sender, EventArgs e)
    {
        ddlTypeSrc.SelectedValue=ddlStatusSrc.SelectedValue = "0";
        BindData();
    }

    protected void lnkDelivered_Command(object sender, CommandEventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectConsumablesPath) && p.Approve.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للأعتماد');</script>", false);
            return;
        }
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            ConsumableDistribution c = db.ConsumableDistributions.Where(x =>x.id==int.Parse(e.CommandArgument.ToString())).FirstOrDefault();
            try
            {
                c.deliveryDate = DateTime.Now;
                c.deliveredBy = UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID;
                db.SubmitChanges();
                LogWriter.LogWrite("Consumables", ((int)ActivitiesEnum.Update).ToString(), c.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, EncryptString.Decrypt(Request.QueryString["id"]), c.Consumable.ConsumableType.name + "تم استلام ");
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