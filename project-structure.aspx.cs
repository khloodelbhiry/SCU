using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class structure : System.Web.UI.Page
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
                        p.PageUrl.ToLower().Equals(Common.ProjectStructurePath) &&p.Show.Equals(true)))
                {
                    if (Request.QueryString["id"] != null && Request.QueryString["g"] != null)
                    {
                        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
                        {
                            var per = UserPermissions.FirstOrDefault(p => p.PageUrl.ToLower().Equals(Common.ProjectStructurePath));
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
            if (UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectStructurePath) && p.Show.Equals(true)))
                BindData();
        }
    }

    private void BindData()
    {
        tvMenu.Nodes.Clear();
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            var menu = (from m in db.UnitStructures
                        where m.parentId== 0
                        && m.governmentalEntityId.Equals(int.Parse(EncryptString.Decrypt(Request.QueryString["g"])))
                        select new
                        {
                            m.id,
                            m.name,
                            ChildNodeCount = db.UnitStructures.Where(n => n.parentId.Equals(m.id)).Count()
                        }).OrderBy(x => x.id);
            DataTable dt = menu.CopyToDataTable();
            PopulateNodes(dt, tvMenu.Nodes);
        }
        tvMenu.ExpandAll();
    }
    private void PopulateSubLevel(int parentid, TreeNode parentNode)
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            var menu = (from m in db.UnitStructures
                        where m.parentId == parentid
                        select new
                        {
                            m.id,
                            name = m.code + " | " + m.name,
                            ChildNodeCount = db.UnitStructures.Where(n => n.parentId.Equals(m.id)).Count()
                        }).OrderBy(x => x.id);
            DataTable dt = menu.CopyToDataTable();
            PopulateNodes(dt, parentNode.ChildNodes);
        }
    }
    private void PopulateNodes(DataTable dt, TreeNodeCollection nodes)
    {
        foreach (DataRow dr in dt.Rows)
        {
            TreeNode tn = new TreeNode();
            tn.Text = dr["name"].ToString();
            tn.Value = dr["id"].ToString();
            nodes.Add(tn);
            tn.PopulateOnDemand = (int.Parse(dr["ChildNodeCount"].ToString()) > 0);
        }
    }
    protected void tvMenu_TreeNodePopulate(object sender, TreeNodeEventArgs e)
    {
        PopulateSubLevel(int.Parse(e.Node.Value), e.Node);
    }
}