using DevExpress.Web.ASPxTreeView;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class groups : System.Web.UI.Page
{
    private DataTable dtData
    {
        get
        {
            return ((DataTable)Session["_dtSelectedData"]);
        }
        set
        {
            if (value == null)
            {
                Session.Remove("_dtSelectedData");
            }
            else
            {
                Session["_dtSelectedData"] = value;
            }
        }
    }
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
            lnkHistory.HRef = "log.aspx?t=" + EncryptString.Encrypt("Groups");
            if (Session["User"] != null && Session["User"].ToString() != string.Empty)
            {
                if (UserPermissions.Any(
                    p =>
                        p.PageUrl.ToLower().Equals(Common.GroupsPath) &&
                        (p.Show.Equals(true) || p.Add.Equals(true) || p.Edit.Equals(true) || p.Delete.Equals(true))))
                {
                    var per = UserPermissions.FirstOrDefault(p => p.PageUrl.ToLower().Equals(Common.GroupsPath));
                    ((HtmlGenericControl)Page.Master.FindControl("ulBreadcrumb")).InnerHtml = "<li><i class='ace-icon fa fa-home home-icon'></i><a href ='Default.aspx'> الرئيسية </a></li><li>" + per.ModuleName + "</li><li><a class='active'>" + per.PageName + "</a></li>";
                    Page.Title = per.PageName;
                }
                else
                    Response.Redirect("no-permission.aspx");
            }
            else
                Response.Redirect("Login.aspx?ReturnURL=" + Request.Url.AbsolutePath);
            DataTable table = GetDataTable();
            CreateTreeViewNodesRecursive(table, tvPermissions.Nodes, "0");
            DataTable tablePrerequisites = GetPrerequisitesDataTable();
            CreatePrerequisitesNodesRecursive(tablePrerequisites, tvPrerequisites.Nodes, "0");
            DataTable tableStatesTransition = GetStatesTransitionDataTable();
            CreateStatesTransitionNodesRecursive(tableStatesTransition, tvStatesTransition.Nodes, "0");
            BindData();
        }
    }
    private DataTable GetPrerequisitesDataTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("id", typeof(String));
        dt.Columns.Add("Name", typeof(String));
        dt.Columns.Add("ParentID", typeof(String));
        dt.Columns.Add("Checked", typeof(Boolean));
        dt.PrimaryKey = new[] { dt.Columns["id"] };
        int groupId = ViewState["id"] != null ? int.Parse(ViewState["id"].ToString()) : 0;
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            var data = (from m in db.ProcessPrerequisites
                           select new
                           {
                               OrderId=m.id,
                               ID = "r" + m.id.ToString(),
                               Name =m.name,
                               ParentID = "0",
                               Checked = db.GroupPrerequisitesCompletions.Any(x => x.processPrerequisiteId.Equals(m.id) && x.groupId.Equals(groupId))
                           });
            dt = data.OrderBy(x=>x.OrderId).CopyToDataTable();
        }
        return dt;
    }

    private DataTable GetStatesTransitionDataTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("id", typeof(String));
        dt.Columns.Add("Name", typeof(String));
        dt.Columns.Add("ParentID", typeof(String));
        dt.Columns.Add("Checked", typeof(Boolean));

        dt.PrimaryKey = new[] { dt.Columns["id"] };
        int groupId = ViewState["id"] != null ? int.Parse(ViewState["id"].ToString()) : 0;
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            var states = (from m in db.StatesTransitions
                          where m.processId==1
                           select new
                           {
                               ID = "s" + m.id.ToString(),
                               Name = m.name,
                               ParentID = "0",
                               m.stateOrder,
                               Checked = db.GroupStateTransitionActions.Count(x => x.StatesTransitionAction.StatesTransition.id.Equals(m.id) && x.groupId.Equals(groupId)) == db.StatesTransitionActions.Where(x => x.statesTransitionId.Equals(m.id)).Count() && db.StatesTransitionActions.Where(x => x.statesTransitionId.Equals(m.id)).Count() > 0
                           });
            var actions = (from m in db.StatesTransitionActions
                         select new
                         {
                             ID = "a" + m.id.ToString(),
                             Name = m.ActionsType.name,
                             ParentID = "s" + (m.statesTransitionId).ToString(),
                             m.StatesTransition.stateOrder,
                             Checked = db.GroupStateTransitionActions.Any(x => x.stateTransitionActionId.Equals(m.id) && x.groupId.Equals(groupId))
                         });
            var data = states.Union(actions).OrderBy(x=>x.stateOrder);
            dt = data.CopyToDataTable();
        }
        return dt;
    }

    private DataTable GetDataTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("id", typeof(String));
        dt.Columns.Add("Name", typeof(String));
        dt.Columns.Add("ParentID", typeof(String));
        dt.Columns.Add("Checked", typeof(Boolean));

        dt.PrimaryKey = new[] { dt.Columns["id"] };
        int groupId = ViewState["id"] != null ? int.Parse(ViewState["id"].ToString()) : 0;
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            var modules = (from m in db.Modules
                           select new
                           {
                               ID = "m" + m.id.ToString(),
                               Name = m.name,
                               ParentID = "0",
                               Checked = db.GroupActivities.Count(x => x.FormActivityType.ModuleForm.moduleId.Equals(m.id) && x.groupId.Equals(groupId)) == db.FormActivityTypes.Where(x => x.ModuleForm.moduleId.Equals(m.id)).Count() && db.FormActivityTypes.Where(x => x.ModuleForm.moduleId.Equals(m.id)).Count() > 0
                           });
            var pages = (from m in db.ModuleForms
                         select new
                         {
                             ID = "p" + m.id.ToString(),
                             Name = m.name,
                             ParentID = "m" + (m.moduleId).ToString(),
                             Checked = db.GroupActivities.Count(x => x.FormActivityType.formId.Equals(m.id) && x.groupId.Equals(groupId)) == m.FormActivityTypes.Count
                         });
            var functions = (from m in db.FormActivityTypes
                             select new
                             {
                                 ID = m.id.ToString(),
                                 Name = m.ActivityType.name,
                                 ParentID = "p" + (m.formId).ToString(),
                                 Checked = db.GroupActivities.Any(x => x.formActivityTypeId.Equals(m.id) && x.groupId.Equals(groupId))
                             });
            var data = modules.Union(pages).Union(functions);
            dt = data.CopyToDataTable();
        }
        return dt;
    }
    private void CreateTreeViewNodesRecursive(DataTable table, TreeViewNodeCollection nodesCollection, string parentId)
    {
        for (int i = 0; i < table.Rows.Count; i++)
        {
            if (table.Rows[i]["ParentID"].ToString() == parentId)
            {
                TreeViewNode node = new TreeViewNode(table.Rows[i]["Name"].ToString(), table.Rows[i]["id"].ToString())
                {
                    Checked = bool.Parse(table.Rows[i]["Checked"].ToString())
                };
                nodesCollection.Add(node);
                CreateTreeViewNodesRecursive(table, node.Nodes, node.Name);
            }
        }
    }
    private void CreatePrerequisitesNodesRecursive(DataTable table, TreeViewNodeCollection nodesCollection, string parentId)
    {
        for (int i = 0; i < table.Rows.Count; i++)
        {
            if (table.Rows[i]["ParentID"].ToString() == parentId)
            {
                TreeViewNode node = new TreeViewNode(table.Rows[i]["Name"].ToString(), table.Rows[i]["ID"].ToString())
                {
                    Checked = bool.Parse(table.Rows[i]["Checked"].ToString())
                };
                nodesCollection.Add(node);
                CreatePrerequisitesNodesRecursive(table, node.Nodes, node.Name);
            }
        }
    }
    private void CreateStatesTransitionNodesRecursive(DataTable table, TreeViewNodeCollection nodesCollection, string parentId)
    {
        for (int i = 0; i < table.Rows.Count; i++)
        {
            if (table.Rows[i]["ParentID"].ToString() == parentId)
            {
                TreeViewNode node = new TreeViewNode(table.Rows[i]["Name"].ToString(), table.Rows[i]["ID"].ToString())
                {
                    Checked = bool.Parse(table.Rows[i]["Checked"].ToString())
                };
                nodesCollection.Add(node);
                CreateStatesTransitionNodesRecursive(table, node.Nodes, node.Name);
            }
        }
    }
    private void BindData()
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            try
            {
                var query = from t in db.Groups
                            select new
                            {
                                t.id,
                                t.name
                            };
                if (txtNameSrch.Text.Trim() != string.Empty)
                    query = query.Where(x => x.name.Contains(txtNameSrch.Text.Trim()));
                dtData = query.CopyToDataTable();
                lblResult.Text = dtData.Rows.Count.ToString();
                gdvData.DataSource = dtData;
                gdvData.DataBind();
            }
            catch (Exception ex)
            {
                Common.InsertException(ex.Message, ex.StackTrace,
                           new System.IO.FileInfo(System.Web.HttpContext.Current.Request.Url.AbsolutePath).Name);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('حدث خطا اثناء الحفظ');</script>", false);
            }
        }
    }
    private void ClearGroupControls()
    {
        txtName.Text = string.Empty;
        tvPermissions.Nodes.Clear();
        tvPrerequisites.Nodes.Clear();
        tvStatesTransition.Nodes.Clear();
        ViewState["id"] = null;
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.GroupsPath) && (p.Show.Equals(true) || p.Edit.Equals(true) || p.Delete.Equals(true))))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للبحث');</script>", false);
            return;
        }
        BindData();
    }
    protected void btnNewSearch_Click(object sender, EventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.GroupsPath) && (p.Show.Equals(true) || p.Edit.Equals(true) || p.Delete.Equals(true))))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للبحث');</script>", false);
            return;
        }
        txtNameSrch.Text = string.Empty;
        BindData();
    }
    protected void btn_NewGroup_Click(object sender, EventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.GroupsPath) && p.Add.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للاضافة');</script>", false);
            return;
        }
        Response.Redirect("group-details.aspx");
        txtName.Text = string.Empty;
        mpeGroupAdd.Show();
    }
    protected void btnClose_Click(object sender, EventArgs e)
    {
        ViewState["GroupID"] = "0";
        tvPermissions.CheckNodesRecursive = false;
    }
    protected void lnkSubmitEditGroup_Click(object sender, EventArgs e)
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            try
            {
                int groupId = 0;
                if (ViewState["id"] != null)
                {
                    Group u =
                        db.Groups.FirstOrDefault(x => x.id.Equals(int.Parse(ViewState["id"].ToString())));
                    if (u != null)
                    {
                        u.name = txtName.Text;
                        groupId = u.id;
                        db.SubmitChanges();
                    }
                }
                else
                {
                    Group u = new Group();
                    u.name = txtName.Text.Trim();
                    u.statusId = (int)StatusEnum.UnderApprrove;
                    db.Groups.InsertOnSubmit(u);
                    db.SubmitChanges();
                    groupId = u.id;
                }
                SavePermissions(groupId);
                SavePrerequisitesPermissions(groupId);
                SaveStatesTransitionPermissions(groupId);
                ClearGroupControls();
                DataTable table = GetDataTable();
                CreateTreeViewNodesRecursive(table, tvPermissions.Nodes, "0");
                DataTable tablePrerequisites = GetPrerequisitesDataTable();
                CreatePrerequisitesNodesRecursive(tablePrerequisites, tvPrerequisites.Nodes, "0");
                DataTable tableStatesTransition = GetStatesTransitionDataTable();
                CreateStatesTransitionNodesRecursive(tableStatesTransition, tvStatesTransition.Nodes, "0");
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

    private void SavePrerequisitesPermissions(int groupId)
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            var pr = db.GroupPrerequisitesCompletions.Where(x => x.groupId.Equals(groupId));
            db.GroupPrerequisitesCompletions.DeleteAllOnSubmit(pr);
            db.SubmitChanges();
            TreeViewNode MainNode = tvPrerequisites.Nodes[0];
            PrintPrerequisitesNodesRecursive(MainNode, groupId);
        }
    }
    int d = 0;
    public void PrintPrerequisitesNodesRecursive(TreeViewNode oParentNode, int group)
    {
        var x = oParentNode.Text;
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
                if (oParentNode.Checked)
                {
                    GroupPrerequisitesCompletion a = new GroupPrerequisitesCompletion
                    {
                        processPrerequisiteId = Convert.ToInt32(oParentNode.Name.Replace('r', ' ').Trim()),
                        groupId = group
                    };
                    db.GroupPrerequisitesCompletions.InsertOnSubmit(a);
                }

                db.SubmitChanges();
            
        }
        ++d;
        if (d < tvPrerequisites.Nodes.Count)
        {
            TreeViewNode MainNode = tvPrerequisites.Nodes[d];
            PrintPrerequisitesNodesRecursive(MainNode, group);
        }
    }
    private void SaveStatesTransitionPermissions(int groupId)
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            var pr = db.GroupStateTransitionActions.Where(x => x.groupId.Equals(groupId));
            db.GroupStateTransitionActions.DeleteAllOnSubmit(pr);
            db.SubmitChanges();
            TreeViewNode MainNode = tvStatesTransition.Nodes[0];
            PrintStatesTransitionNodesRecursive(MainNode, groupId);
        }

    }
        int u = 0;
    public void PrintStatesTransitionNodesRecursive(TreeViewNode oParentNode, int group)
    {
        var x = oParentNode.Text;
        foreach (TreeViewNode SubNode in oParentNode.Nodes)
        {
            using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
            {
                if (SubNode.Checked)
                {
                    if (SubNode.Text == "تم")
                    {
                        var optionsByPage = db.StatesTransitionActions.FirstOrDefault(p => p.statesTransitionId.Equals(Convert.ToInt32(oParentNode.Name.Replace('s', ' ').Trim())) && p.actionTypeId == 1);
                        GroupStateTransitionAction a = new GroupStateTransitionAction
                        {
                            stateTransitionActionId = optionsByPage.id,
                            groupId = group
                        };
                        db.GroupStateTransitionActions.InsertOnSubmit(a);
                    }
                    else if (SubNode.Text == "اعادة تشغيل")
                    {
                        var optionsByPage = db.StatesTransitionActions.FirstOrDefault(p => p.statesTransitionId.Equals(Convert.ToInt32(oParentNode.Name.Replace('s', ' ').Trim())) && p.actionTypeId == 2);
                        GroupStateTransitionAction a = new GroupStateTransitionAction
                        {
                            stateTransitionActionId = optionsByPage.id,
                            groupId = group
                        };
                        db.GroupStateTransitionActions.InsertOnSubmit(a);
                    }

                }

                db.SubmitChanges();
            }
        }
        ++u;
        if (u < tvStatesTransition.Nodes.Count)
        {
            TreeViewNode MainNode = tvStatesTransition.Nodes[u];
            PrintStatesTransitionNodesRecursive(MainNode, group);
        }
    }
    private void SavePermissions(int group)
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            var pr = db.GroupActivities.Where(x => x.groupId.Equals(group));
            db.GroupActivities.DeleteAllOnSubmit(pr);
            db.SubmitChanges();
            TreeViewNode MainNode = tvPermissions.Nodes[0];
            PrintNodesRecursive(MainNode, group);
        }

    }
    int z = 0;
    public void PrintNodesRecursive(TreeViewNode oParentNode, int group)
    {
        var x = oParentNode.Text;
        foreach (TreeViewNode SubNode in oParentNode.Nodes)
        {
            using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
            {
                foreach (TreeViewNode item in SubNode.Nodes)
                {
                    if (item.Checked)
                    {
                        if (item.Text == "عرض")
                        {
                            var optionsByPage = db.FormActivityTypes.FirstOrDefault(p => p.formId.Equals(Convert.ToInt32(SubNode.Name.Replace('p', ' ').Trim())) && p.activityTypeId == (int)ActivitiesEnum.Read);
                            GroupActivity a = new GroupActivity
                            {
                                formActivityTypeId = optionsByPage.id,
                                groupId = group
                            };
                            db.GroupActivities.InsertOnSubmit(a);
                        }
                        else if (item.Text == "اضافة")
                        {
                            var optionsByPage = db.FormActivityTypes.FirstOrDefault(p => p.formId.Equals(Convert.ToInt32(SubNode.Name.Replace('p', ' ').Trim())) && p.activityTypeId == (int)ActivitiesEnum.Add);
                            GroupActivity a = new GroupActivity
                            {
                                formActivityTypeId = optionsByPage.id,
                                groupId = group
                            };
                            db.GroupActivities.InsertOnSubmit(a);
                        }
                        else if (item.Text == "تعديل")
                        {
                            var optionsByPage = db.FormActivityTypes.FirstOrDefault(p => p.formId.Equals(Convert.ToInt32(SubNode.Name.Replace('p', ' ').Trim())) && p.activityTypeId == (int)ActivitiesEnum.Update);
                            GroupActivity a = new GroupActivity
                            {
                                formActivityTypeId = optionsByPage.id,
                                groupId = group
                            };
                            db.GroupActivities.InsertOnSubmit(a);
                        }
                        else if (item.Text == "حذف")
                        {
                            var optionsByPage = db.FormActivityTypes.FirstOrDefault(p => p.formId.Equals(Convert.ToInt32(SubNode.Name.Replace('p', ' ').Trim())) && p.activityTypeId == (int)ActivitiesEnum.Delete);
                            GroupActivity a = new GroupActivity
                            {
                                formActivityTypeId = optionsByPage.id,
                                groupId = group
                            };
                            db.GroupActivities.InsertOnSubmit(a);
                        }
                        else if (item.Text == "طباعة")
                        {
                            var optionsByPage = db.FormActivityTypes.FirstOrDefault(p => p.formId.Equals(Convert.ToInt32(SubNode.Name.Replace('p', ' ').Trim())) && p.activityTypeId == (int)ActivitiesEnum.Print);
                            GroupActivity a = new GroupActivity
                            {
                                formActivityTypeId = optionsByPage.id,
                                groupId = group
                            };
                            db.GroupActivities.InsertOnSubmit(a);
                        }
                        else if (item.Text == "تصدير")
                        {
                            var optionsByPage = db.FormActivityTypes.FirstOrDefault(p => p.formId.Equals(Convert.ToInt32(SubNode.Name.Replace('p', ' ').Trim())) && p.activityTypeId == (int)ActivitiesEnum.Export);
                            GroupActivity a = new GroupActivity
                            {
                                formActivityTypeId = optionsByPage.id,
                                groupId = group
                            };
                            db.GroupActivities.InsertOnSubmit(a);
                        }
                        else if (item.Text == "فهرسة")
                        {
                            var optionsByPage = db.FormActivityTypes.FirstOrDefault(p => p.formId.Equals(Convert.ToInt32(SubNode.Name.Replace('p', ' ').Trim())) && p.activityTypeId == (int)ActivitiesEnum.Index);
                            GroupActivity a = new GroupActivity
                            {
                                formActivityTypeId = optionsByPage.id,
                                groupId = group
                            };
                            db.GroupActivities.InsertOnSubmit(a);
                        }
                        else if (item.Text == "مراجعة")
                        {
                            var optionsByPage = db.FormActivityTypes.FirstOrDefault(p => p.formId.Equals(Convert.ToInt32(SubNode.Name.Replace('p', ' ').Trim())) && p.activityTypeId == (int)ActivitiesEnum.Qa);
                            GroupActivity a = new GroupActivity
                            {
                                formActivityTypeId = optionsByPage.id,
                                groupId = group
                            };
                            db.GroupActivities.InsertOnSubmit(a);
                        }
                        else if (item.Text == "اعتماد")
                        {
                            var optionsByPage = db.FormActivityTypes.FirstOrDefault(p => p.formId.Equals(Convert.ToInt32(SubNode.Name.Replace('p', ' ').Trim())) && p.activityTypeId == (int)ActivitiesEnum.Approve);
                            GroupActivity a = new GroupActivity
                            {
                                formActivityTypeId = optionsByPage.id,
                                groupId = group
                            };
                            db.GroupActivities.InsertOnSubmit(a);
                        }
                        else if (item.Text == "تجميد")
                        {
                            var optionsByPage = db.FormActivityTypes.FirstOrDefault(p => p.formId.Equals(Convert.ToInt32(SubNode.Name.Replace('p', ' ').Trim())) && p.activityTypeId == (int)ActivitiesEnum.Freze);
                            GroupActivity a = new GroupActivity
                            {
                                formActivityTypeId = optionsByPage.id,
                                groupId = group
                            };
                            db.GroupActivities.InsertOnSubmit(a);
                        }
                    }
                }
                db.SubmitChanges();
            }
        }
        ++z;
        if (z < tvPermissions.Nodes.Count)
        {
            TreeViewNode MainNode = tvPermissions.Nodes[z];
            PrintNodesRecursive(MainNode, group);
        }
    }
    protected void lnkEditGroup_Command(object sender, CommandEventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.GroupsPath) && p.Edit.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للتعديل');</script>", false);
            return;
        }
        Response.Redirect("group-details.aspx?id="+EncryptString.Encrypt(e.CommandArgument.ToString()));
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            try
            {
                ViewState["id"] = e.CommandArgument;
                var u = (from t in db.Groups
                         where t.id.Equals(int.Parse(e.CommandArgument.ToString()))
                         select new
                         {
                             t.id,
                             t.name
                         }).FirstOrDefault();
                if (u != null)
                {
                    txtName.Text = u.name;
                }
                tvPermissions.Nodes.Clear();
                DataTable table = GetDataTable();
                CreateTreeViewNodesRecursive(table, tvPermissions.Nodes, "0");
                tvPrerequisites.Nodes.Clear();
                DataTable tablePrerequisites = GetPrerequisitesDataTable();
                CreatePrerequisitesNodesRecursive(tablePrerequisites, tvPrerequisites.Nodes, "0");
                tvStatesTransition.Nodes.Clear();
                DataTable tableStatesTransition = GetStatesTransitionDataTable();
                CreateStatesTransitionNodesRecursive(tableStatesTransition, tvStatesTransition.Nodes, "0");
                mpeGroupAdd.Show();
            }
            catch (Exception ex)
            {
                Common.InsertException(ex.Message, ex.StackTrace,
                           new System.IO.FileInfo(System.Web.HttpContext.Current.Request.Url.AbsolutePath).Name);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('حدث خطا اثناء الحفظ');</script>", false);
            }
        }
    }
    protected void lnkDeleteGroup_Command(object sender, CommandEventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.GroupsPath) && p.Delete.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للحذف');</script>", false);
            return;
        }
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            try
            {
                ViewState["id"] = e.CommandArgument;
                var pr = db.GroupActivities.Where(x => x.groupId.Equals(int.Parse(e.CommandArgument.ToString())));
                db.GroupActivities.DeleteAllOnSubmit(pr);

                var re = db.GroupPrerequisitesCompletions.Where(x => x.groupId.Equals(int.Parse(e.CommandArgument.ToString())));
                db.GroupPrerequisitesCompletions.DeleteAllOnSubmit(re);

                var s = db.GroupStateTransitionActions.Where(x => x.groupId.Equals(int.Parse(e.CommandArgument.ToString())));
                db.GroupStateTransitionActions.DeleteAllOnSubmit(s);

                Group u = db.Groups.FirstOrDefault(x => x.id.Equals(int.Parse(e.CommandArgument.ToString())));
                if (u != null)
                    db.Groups.DeleteOnSubmit(u);
                db.SubmitChanges(); 
                LogWriter.LogWrite("Groups", ((int)ActivitiesEnum.Delete).ToString(), u.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                BindData();
                ScriptManager.RegisterStartupScript(this, GetType(), "alertGroup", "alert('تم الحذف بنجاح .');", true);
            }
            catch (Exception ex)
            {
                Common.InsertException(ex.Message, ex.StackTrace,
                          new System.IO.FileInfo(System.Web.HttpContext.Current.Request.Url.AbsolutePath).Name);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('حدث خطا اثناء الحفظ');</script>", false);
            }
        }
    }
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        ClearGroupControls();
        DataTable table = GetDataTable();
        CreateTreeViewNodesRecursive(table, tvPermissions.Nodes, "0");
        ScriptManager.RegisterStartupScript(this, GetType(), "alertGroup",
             "$('#GroupModal').modal('hide');", true);
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
}