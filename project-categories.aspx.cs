using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class ministers_departments: System.Web.UI.Page
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
            if (Session["User"] != null && Session["User"].ToString() != string.Empty)
            {
                if (UserPermissions.Any(
                    p =>
                        p.PageUrl.ToLower().Equals(Common.ProjectCategoriesPath) &&
                        (p.Show.Equals(true) || p.Add.Equals(true) || p.Edit.Equals(true) || p.Delete.Equals(true) || p.Approve.Equals(true) || p.Freze.Equals(true))))
                {
                    var per = UserPermissions.FirstOrDefault(p => p.PageUrl.ToLower().Equals(Common.ProjectCategoriesPath));
                    ((HtmlGenericControl)Page.Master.FindControl("ulBreadcrumb")).InnerHtml = "<li><i class='ace-icon fa fa-home home-icon'></i><a href ='Default.aspx'> الرئيسية </a></li><li>" + per.ModuleName + "</li><li><a class='active'>" + per.PageName + "</a></li>";
                    Page.Title = per.PageName;
                }
                else
                    Response.Redirect("no-permission.aspx");
            }
            else
                Response.Redirect("Login.aspx?ReturnURL=" + Request.Url.AbsolutePath);
            if (UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectCategoriesPath) && (p.Show.Equals(true) || p.Edit.Equals(true) || p.Delete.Equals(true) || p.Approve.Equals(true) || p.Freze.Equals(true))))
                PopulateRootLevel();
        }
    }
    private void PopulateRootLevel()
    {
        tvMenu.Nodes.Clear();
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            ddlParent.DataSource = db.SP_CategoriesTreeByProject(int.Parse(EncryptString.Decrypt(Request.QueryString["id"])));
            ddlParent.DataTextField = "Category_Name";
            ddlParent.DataValueField = "id";
            ddlParent.DataBind();
            ddlParent.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- اختر --", "0"));

            var menu = (from m in db.Categories
                        where m.parentId == 0
                        && (m.statusId == (int)StatusEnum.Approved || m.projectId == int.Parse(EncryptString.Decrypt(Request.QueryString["id"])))
                        select new
                        {
                            m.id,
                            name="<span class="+(m.statusId==(int)StatusEnum.Approved?"'green'":(m.statusId == (int)StatusEnum.Freezed ? "'red'" :string.Empty))+">"+m.name+"</span>",
                            ChildNodeCount = db.Categories.Where(n => n.parentId.Equals(m.id)).Count()
                        }).OrderBy(x => x.id);
            DataTable dt = menu.CopyToDataTable();
            PopulateNodes(dt, tvMenu.Nodes);
        }
        tvMenu.ExpandAll();
    }
    private void FillControls(bool resetPosition)
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            Category m =
                db.Categories
                    .FirstOrDefault(i => i.id.Equals(int.Parse(ViewState["ID"].ToString())));
            if (m != null)
            {
                if (resetPosition == false)
                {
                    txtName.Text = m.name;
                    ddlParent.SelectedValue = m.parentId.ToString();
                    btnApprove.Visible = lnkSubmit.Visible = m.statusId == (int)StatusEnum.UnderApprrove;
                    btnFreeze.Visible = m.statusId != (int)StatusEnum.Freezed && m.statusId == null;
                    lnkDelete.Visible = m.statusId == (int)StatusEnum.UnderApprrove;
                }
            }
        }
    }
    private void ClearControls()
    {
        txtName.Text = string.Empty;
        ViewState["ID"] = null;
        lnkDelete.Visible =  btnApprove.Visible = btnFreeze.Visible = false;
        lnkSubmit.Visible = true;
        ddlParent.SelectedValue = "0";
    }
    private void PopulateSubLevel(int parentid, TreeNode parentNode)
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            var menu = (from m in db.Categories
                        where m.parentId == parentid
                         && (m.statusId == (int)StatusEnum.Approved || m.projectId == int.Parse(EncryptString.Decrypt(Request.QueryString["id"])))
                        select new
                        {
                            m.id,
                            name = "<span class=" + (m.statusId == (int)StatusEnum.Approved ? "'green'" : (m.statusId == (int)StatusEnum.Freezed ? "'red'" : string.Empty)) + ">" + m.name + "</span>",
                            ChildNodeCount = db.Categories.Where(n => n.parentId.Equals(m.id)).Count()
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
    protected void lnkSubmit_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
            {
                try
                {
                    if (ViewState["ID"] == null)
                    {
                        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectCategoriesPath) && p.Add.Equals(true)))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للأضافة');</script>", false);
                            return;
                        }
                        Category c = new Category
                        {
                            name = txtName.Text.Trim(),
                            parentId = ddlParent.SelectedValue != "0" ? int.Parse(ddlParent.SelectedValue) : 0,
                            projectId =  int.Parse(EncryptString.Decrypt(Request.QueryString["id"])),
                            statusId=(int)StatusEnum.UnderApprrove
                        };
                        db.Categories.InsertOnSubmit(c);
                    }
                    else
                    {
                        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectCategoriesPath) && p.Edit.Equals(true)))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للتعديل');</script>", false);
                            return;
                        }
                        Category c =
                            db.Categories
                                .FirstOrDefault(i => i.id.Equals(int.Parse(ViewState["ID"].ToString())));
                        if (c != null)
                        {
                            c.name = txtName.Text.Trim();
                            c.parentId = ddlParent.SelectedValue != "0" ? int.Parse(ddlParent.SelectedValue) : 0;
                        }
                    }
                    db.SubmitChanges();
                    PopulateRootLevel();
                    ClearControls();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('تم الحفظ بنجاح');</script>", false);
                }
                catch (Exception ex)
                {
                    Common.InsertException(ex.Message, ex.StackTrace,
                          new System.IO.FileInfo(System.Web.HttpContext.Current.Request.Url.AbsolutePath).Name);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('حدث خطأ اثناء التنفيذ');</script>", false);
                }
            }
        }
    }
    protected void tvMenu_SelectedNodeChanged(object sender, EventArgs e)
    {
        ViewState["ID"] = tvMenu.SelectedNode.Value;
        FillControls(false);
    }
    protected void tvMenu_TreeNodePopulate(object sender, TreeNodeEventArgs e)
    {
        PopulateSubLevel(int.Parse(e.Node.Value), e.Node);
    }
    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectCategoriesPath) && p.Delete.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للحذف');</script>", false);
            return;
        }
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            try
            {
                if (ViewState["ID"] != null)
                {
                    var menus =
                        db.Categories.Where(
                            i =>
                                i.id.Equals(int.Parse(ViewState["ID"].ToString())) ||
                                i.parentId.Equals(int.Parse(ViewState["ID"].ToString())));
                    db.Categories.DeleteAllOnSubmit(menus);
                    db.SubmitChanges();
                }
                PopulateRootLevel();
                ClearControls();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('تم الحفظ بنجاح');</script>", false);
            }
            catch (Exception ex)
            {
                Common.InsertException(ex.Message, ex.StackTrace,
                      new System.IO.FileInfo(System.Web.HttpContext.Current.Request.Url.AbsolutePath).Name);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('حدث خطأ اثناء التنفيذ');</script>", false);
            }
        }
    }
    protected void cvName_ServerValidate(object source, ServerValidateEventArgs args)
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            if (ViewState["ID"] == null)
                args.IsValid = !db.Categories.Any(x =>  x.parentId.Equals(int.Parse(ddlParent.SelectedValue)) && x.name.Equals(txtName.Text.Trim()));
            else
                args.IsValid = !db.Categories.Any(x =>  x.parentId.Equals(int.Parse(ddlParent.SelectedValue)) && x.id != int.Parse(ViewState["ID"].ToString()) && x.name.Equals(txtName.Text.Trim()));
        }
    }
    protected void btnFreeze_Click(object sender, EventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectCategoriesPath) && p.Freze.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للتجميد');</script>", false);
            return;
        }
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            Category c = db.Categories.Where(x => x.id == int.Parse(ViewState["ID"].ToString())).FirstOrDefault();
            try
            {
                c.statusId= (int)StatusEnum.Freezed;
                db.SubmitChanges();
                btnFreeze.Visible = btnApprove.Visible = lnkSubmit.Visible = false;
                PopulateRootLevel();
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
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectCategoriesPath) && p.Approve.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للأعتماد');</script>", false);
            return;
        }
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            Category c = db.Categories.Where(x => x.id == int.Parse(ViewState["ID"].ToString())).FirstOrDefault();
            try
            {
                c.statusId= (int)StatusEnum.Approved;
                db.SubmitChanges(); 
                btnApprove.Visible = lnkSubmit.Visible =lnkDelete.Visible= btnFreeze.Visible =false;
                PopulateRootLevel();
            }
            catch (Exception ex)
            {
                Common.InsertException(ex.Message, ex.StackTrace,
                          new System.IO.FileInfo(System.Web.HttpContext.Current.Request.Url.AbsolutePath).Name);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('حدث خطا اثناء الحفظ');</script>", false);
            }
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        ClearControls();
    }
}