using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class unit_structure : System.Web.UI.Page
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
    private DataTable dtParent
    {
        get
        {
            return ((DataTable)ViewState["_dtParent"]);
        }
        set
        {
            if (value == null)
            {
                ViewState.Remove("_dtParent");
            }
            else
            {
                ViewState["_dtParent"] = value;
            }
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            HtmlAnchor lnkHistory = (HtmlAnchor)Master.FindControl("lnkHistory");
            lnkHistory.HRef = "log.aspx?t=" + EncryptString.Encrypt("UnitStructure");
            if (Session["User"] != null && Session["User"].ToString() != string.Empty)
            {
                if (UserPermissions.Any(
                    p =>
                        p.PageUrl.ToLower().Equals(Common.UnitStructurePath) &&
                        (p.Show.Equals(true) || p.Add.Equals(true) || p.Edit.Equals(true) || p.Delete.Equals(true))))
                {
                    var per = UserPermissions.FirstOrDefault(p => p.PageUrl.ToLower().Equals(Common.UnitStructurePath));
                    using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
                    {
                        GovernmentalEntity g = db.GovernmentalEntities.FirstOrDefault(x => x.id == int.Parse(EncryptString.Decrypt(Request.QueryString["id"])));
                        ((HtmlGenericControl)Page.Master.FindControl("ulBreadcrumb")).InnerHtml = "<li><i class='ace-icon fa fa-home home-icon'></i><a href ='Default.aspx'> الرئيسية </a></li><li>" + per.ModuleName + "</li>" + (per.ParentForm != string.Empty ? "<li>" + per.ParentForm + "</li>" : string.Empty) + "<li><a class='active'>" + per.PageName + "</a></li><li><strong>"+g.name+"</strong></li>";
                        Page.Title = per.PageName;
                    }
                }
                else
                    Response.Redirect("no-permission.aspx");
            }
            else
                Response.Redirect("Login.aspx?ReturnURL=" + Request.Url.AbsolutePath);
            BindDDL();
            if (UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.UnitStructurePath) && (p.Show.Equals(true) || p.Edit.Equals(true) || p.Delete.Equals(true))))
                BindData();
        }
    }
    private void BindDDL()
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            ddlType.DataSource = db.UnitStructureTypes;
            ddlType.DataTextField = "name";
            ddlType.DataValueField = "id";
            ddlType.DataBind();
            ddlType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- اختر --", "0"));

            dtParent = db.SP_UnitStructure(int.Parse(EncryptString.Decrypt(Request.QueryString["id"]))).CopyToDataTable();
            ddlParent.DataSource = dtParent;
            ddlParent.DataTextField = "name";
            ddlParent.DataValueField = "id";
            ddlParent.DataBind();
            ddlParent.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- اختر --", "0"));
        }
    }
    private void BindData()
    {
        tvMenu.Nodes.Clear();
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            var menu = (from m in db.UnitStructures
                        where m.parentId == 0
                        && m.governmentalEntityId.Equals(int.Parse(EncryptString.Decrypt(Request.QueryString["id"])))
                        select new
                        {
                            m.id,
                            name = m.code + " | " + m.name,
                            m.governmentalEntityId,
                            ChildNodeCount = db.UnitStructures.Where(n => n.parentId.Equals(m.id)).Count()
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
            UnitStructure m =
                db.UnitStructures
                    .FirstOrDefault(i => i.id.Equals(int.Parse(ViewState["id"].ToString())));
            if (m != null)
            {
                if (resetPosition == false)
                {
                    txtName.Text = m.name;
                    txtCode.Text = m.code;
                    ddlParent.SelectedValue = m.parentId.ToString();
                    ddlType.SelectedValue = (m.typeId != null ? m.typeId.ToString() : "0");
                    txtLvl.Text = m.lvl != null ? m.lvl.ToString() : string.Empty;
                }
            }
        }
    }
    private void ClearControls()
    {
        txtName.Text = txtCode.Text =txtLvl.Text= string.Empty;
        ddlType.SelectedValue=ddlParent.SelectedValue = "0";
        ViewState["id"] = null;
        lnkDelete.Visible =  false;
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
                            m.governmentalEntityId,
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
    protected void lnkSubmit_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
            {
                try
                {
                    int? defaultLvl = null;
                    if (ddlParent.SelectedValue != "0")
                    {
                        DataRow[] lvl = dtParent.Select("id =" + int.Parse(ddlParent.SelectedValue));
                        defaultLvl = int.Parse(lvl[0]["RecursionLevel"].ToString())+1;
                    }
                    else
                        defaultLvl = 1;
                    if (ViewState["id"] == null)
                    {
                        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.UnitStructurePath) && p.Add.Equals(true)))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للاضافة');</script>", false);
                            return;
                        }
                        UnitStructure c = new UnitStructure
                        {
                            governmentalEntityId = int.Parse(EncryptString.Decrypt(Request.QueryString["id"])),
                            name = txtName.Text.Trim(),
                            parentId = ddlParent.SelectedValue != "0" ? int.Parse(ddlParent.SelectedValue) : 0,
                            code = txtCode.Text.Trim(),
                            lvl = txtLvl.Text.Trim() != string.Empty ? (int?)int.Parse(txtLvl.Text.Trim()) : defaultLvl,
                            typeId = ddlType.SelectedValue != "0" ? (int?)int.Parse(ddlType.SelectedValue) : null
                        };
                        db.UnitStructures.InsertOnSubmit(c);
                        db.SubmitChanges();
                        LogWriter.LogWrite("UnitStructure", ((int)ActivitiesEnum.Add).ToString(), c.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                    }
                    else
                    {
                        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.UnitStructurePath) && p.Edit.Equals(true)))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للتعديل');</script>", false);
                            return;
                        }
                        UnitStructure c =
                            db.UnitStructures
                                .FirstOrDefault(i => i.id.Equals(int.Parse(ViewState["id"].ToString())));
                        if (c != null)
                        {
                            c.name = txtName.Text.Trim();
                            c.parentId = ddlParent.SelectedValue != "0" ? int.Parse(ddlParent.SelectedValue) : 0;
                            c.code = txtCode.Text.Trim();
                            c.lvl = txtLvl.Text.Trim() != string.Empty ? (int?)int.Parse(txtLvl.Text.Trim()) : defaultLvl;
                            c.typeId = ddlType.SelectedValue != "0" ? (int?)int.Parse(ddlType.SelectedValue) : null;
                            db.SubmitChanges();
                            LogWriter.LogWrite("UnitStructure", ((int)ActivitiesEnum.Update).ToString(), c.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                        }
                    }
                    ClearControls();
                    BindData();
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
        ViewState["id"] = tvMenu.SelectedNode.Value;
        FillControls(false);
        lnkDelete.Visible =  true;
    }
    protected void tvMenu_TreeNodePopulate(object sender, TreeNodeEventArgs e)
    {
        PopulateSubLevel(int.Parse(e.Node.Value), e.Node);
    }
    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.UnitStructurePath) && p.Delete.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للحذف');</script>", false);
            return;
        }
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            try
            {
                if (ViewState["id"] != null)
                {
                    var menus =
                        db.UnitStructures.Where(
                            i =>
                                i.id.Equals(int.Parse(ViewState["id"].ToString())) ||
                                i.parentId.Equals(int.Parse(ViewState["id"].ToString())));
                    db.UnitStructures.DeleteAllOnSubmit(menus);
                    db.SubmitChanges();
                    LogWriter.LogWrite("UnitStructure", ((int)ActivitiesEnum.Delete).ToString(), ViewState["id"].ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                }
                ClearControls();
                BindData();
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
    protected void btnClear_Click(object sender, EventArgs e)
    {
        ClearControls();
    }
    protected void cvCode_ServerValidate(object source, ServerValidateEventArgs args)
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            if (ViewState["id"] == null)
                args.IsValid = !db.UnitStructures.Any(x => x.code.Equals(txtCode.Text.Trim()));
            else
                args.IsValid = !db.UnitStructures.Any(x => x.id != int.Parse(ViewState["id"].ToString()) && x.code.Equals(txtCode.Text.Trim()));
        }
    }
    protected void cvName_ServerValidate(object source, ServerValidateEventArgs args)
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            if (ViewState["id"] == null)
                args.IsValid = !db.UnitStructures.Any(x => x.parentId.Equals(int.Parse(ddlParent.SelectedValue)) && x.name.Equals(txtName.Text.Trim()));
            else
                args.IsValid = !db.UnitStructures.Any(x => x.parentId.Equals(int.Parse(ddlParent.SelectedValue)) && x.id != int.Parse(ViewState["id"].ToString()) && x.name.Equals(txtName.Text.Trim()));
        }
    }
}