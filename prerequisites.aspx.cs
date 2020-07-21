using DevExpress.Web.ASPxTreeView;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class prerequisites : System.Web.UI.Page
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
    private DataTable dtProgress
    {
        get
        {
            return ((DataTable)ViewState["_dtProgress"]);
        }
        set
        {
            if (value == null)
            {
                ViewState.Remove("_dtProgress");
            }
            else
            {

                if (ViewState["_dtProgress"] == null)
                {
                    ViewState["_dtProgress"] = new DataTable();
                }
                ViewState["_dtProgress"] = value;
                if (((DataTable)ViewState["_dtProgress"]).Columns.Count == 0)
                {
                    ((DataTable)ViewState["_dtProgress"]).Columns.Add("progress", typeof(double));
                    ((DataTable)ViewState["_dtProgress"]).Columns.Add("id", typeof(int));
                    ((DataTable)ViewState["_dtProgress"]).Columns.Add("projectId", typeof(int));
                    ((DataTable)ViewState["_dtProgress"]).Columns.Add("type", typeof(string));
                }
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
            lnkHistory.Visible = false;
            if (Session["User"] != null && Session["User"].ToString() != string.Empty)
            {
                if (UserPermissions.Any(
                    p =>
                        p.PageUrl.ToLower().Equals(Common.PrerequisitesPath) &&
                        (p.Show.Equals(true) || p.Add.Equals(true) || p.Edit.Equals(true) || p.Delete.Equals(true))))
                {
                    var per = UserPermissions.FirstOrDefault(p => p.PageUrl.ToLower().Equals(Common.PrerequisitesPath));
                    ((HtmlGenericControl)Page.Master.FindControl("ulBreadcrumb")).InnerHtml = "<li><i class='ace-icon fa fa-home home-icon'></i><a href ='Default.aspx'> الرئيسية </a></li><li>" + per.ModuleName + "</li><li><a class='active'>" + per.PageName + "</a></li>";
                    Page.Title = per.PageName;
                }
                else
                    Response.Redirect("no-permission.aspx");
            }
            else
                Response.Redirect("Login.aspx?ReturnURL=" + Request.Url.AbsolutePath);
            BindDDL();
            if (UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.PrerequisitesPath) && (p.Show.Equals(true) || p.Edit.Equals(true) || p.Delete.Equals(true))))
            {
                GetOverallProgress();
                BindData();
            }
        }
    }
    private void GetOverallProgress()
    {
        dtProgress = new DataTable();
        using(SCU_OneTrackDataContext db=new SCU_OneTrackDataContext())
        {
            string progress = db.fn_OverallProgress(null);
            string[] result1 = progress.TrimStart(';').Split(';');
            foreach(var i in result1)
            {
                string[] result2 = i.Split(',');
                dtProgress.Rows.Add(double.Parse(result2[0]), int.Parse(result2[1]), int.Parse(result2[2]), result2[3]);
            }
            gdvData.Columns[5].HeaderText = "نسبة الإنجاز من " + db.Settings.FirstOrDefault(x=>x.id==1).value.ToString("G29");
        }
    }
    public string GetProgress(int id, string type)
    {
        DataRow[] dr = dtProgress.Select(string.Format("id ={0} AND type='{1}' ", id, type));
        double progress = 0;
        foreach (DataRow row in dr)
        {
            progress += double.Parse(row["progress"].ToString(), System.Globalization.NumberStyles.Float);
        }
        return progress.ToString("N15").TrimEnd(new char[] { '0' }).TrimEnd(new char[] { '.' });
    }
    private void BindDDL()
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            var query1 = db.PrerequisiteCategories;
            ddlCategory.DataSource = query1;
            ddlCategory.DataTextField = "name";
            ddlCategory.DataValueField = "id";
            ddlCategory.DataBind();
            ddlCategory.Items.Insert(0, new ListItem("-- اختر --", "0"));

            var query2 = db.ImplementerTypes;
            ddlImplementerType.DataSource = query2;
            ddlImplementerType.DataTextField = "name";
            ddlImplementerType.DataValueField = "id";
            ddlImplementerType.DataBind();
            ddlImplementerType.Items.Insert(0, new ListItem("-- اختر --", "0"));

            var query3 = db.ProgressCalculationTypes;
            ddlCalculationTypes.DataSource = query3;
            ddlCalculationTypes.DataTextField = "name";
            ddlCalculationTypes.DataValueField = "id";
            ddlCalculationTypes.DataBind();
            ddlCalculationTypes.Items.Insert(0, new ListItem("-- اختر --", "0"));

            var query4 = (from q in db.ProcessPrerequisites
                          select new
                          { q.id, q.name });
            ddlStartAfter.DataSource = query4;
            ddlStartAfter.DataTextField = "name";
            ddlStartAfter.DataValueField = "id";
            ddlStartAfter.DataBind();
            ddlStartAfter.Items.Insert(0, new ListItem("-- اختر --", "0"));

        }
    }
    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.PrerequisitesPath) && p.Add.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للاضافة');</script>", false);
            return;
        }
        mpeObject.Show();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
            {
                try
                {
                    if (ViewState["id"] == null)
                    {
                        ProcessPrerequisite q = new ProcessPrerequisite()
                        {
                            name = txtTitle.Text.Trim(),
                            prerequisiteCategoryId = int.Parse(ddlCategory.SelectedValue),
                            implementerTypeId = int.Parse(ddlImplementerType.SelectedValue),
                            previousPrerequisiteId = ddlStartAfter.SelectedValue != "0" ? int.Parse(ddlStartAfter.SelectedValue) : 0,
                            relativeWeight = txtWeight.Text.Trim() != string.Empty ? (decimal?)decimal.Parse(txtWeight.Text.Trim()) : null,
                            progressCalculationTypeId = int.Parse(ddlCalculationTypes.SelectedValue),
                            forEveryProject=chkForEveryProject.Checked
                        };
                        db.ProcessPrerequisites.InsertOnSubmit(q);
                    }
                    else
                    {
                        ProcessPrerequisite q = db.ProcessPrerequisites.FirstOrDefault(x => x.id.Equals(int.Parse(ViewState["id"].ToString())));
                        q.name = txtTitle.Text.Trim();
                        q.prerequisiteCategoryId = int.Parse(ddlCategory.SelectedValue);
                        q.implementerTypeId = int.Parse(ddlImplementerType.SelectedValue);
                        q.previousPrerequisiteId = ddlStartAfter.SelectedValue != "0" ? int.Parse(ddlStartAfter.SelectedValue) : 0;
                        q.relativeWeight = txtWeight.Text.Trim() != string.Empty ? (decimal?)decimal.Parse(txtWeight.Text.Trim()) : null;
                        q.progressCalculationTypeId = int.Parse(ddlCalculationTypes.SelectedValue);
                        q.forEveryProject = chkForEveryProject.Checked;
                    }
                    db.SubmitChanges();
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
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.PrerequisitesPath) && p.Edit.Equals(true)))
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
                var query = db.ProcessPrerequisites.Where(t => t.id.Equals(int.Parse(ViewState["id"].ToString()))).FirstOrDefault();
                txtTitle.Text = query.name;
                ddlCategory.SelectedValue = query.prerequisiteCategoryId.ToString();
                ddlImplementerType.SelectedValue = query.implementerTypeId.ToString();
                txtWeight.Text = query.relativeWeight != null ? query.relativeWeight.ToString() : "0";
                ddlCalculationTypes.SelectedValue = query.progressCalculationTypeId != null ? query.progressCalculationTypeId.ToString() : "0";
                ddlStartAfter.SelectedValue = query.previousPrerequisiteId.ToString();
                chkForEveryProject.Checked = query.forEveryProject??false;
                mpeObject.Show();
            }
        }
    }
    protected void btnClose_Click(object sender, EventArgs e)
    {
        ClearControls();
    }
    private void ClearControls()
    {
        ViewState["id"] = null;
        txtTitle.Text = txtWeight.Text = string.Empty;
        ddlStartAfter.SelectedIndex = ddlCalculationTypes.SelectedIndex = -1;
        chkForEveryProject.Checked = false;
        mpeObject.Hide();
    }
    protected void btnDelete_Command(object sender, CommandEventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.PrerequisitesPath) && p.Delete.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للحذف');</script>", false);
            return;
        }
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            ProcessPrerequisite c = db.ProcessPrerequisites.Where(x => x.id == int.Parse(e.CommandArgument.ToString())).FirstOrDefault();
            try
            {
                db.ProcessPrerequisites.DeleteOnSubmit(c);
                db.SubmitChanges();
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
            var prerequisites = (from b in db.ProcessPrerequisites
                                 select new
                                 {
                                     type="p",
                                     b.id,
                                     b.name,
                                     b.implementerTypeId,
                                     PrerequisiteCategory = b.PrerequisiteCategory.name,
                                     ImplementerType = b.ImplementerType.name,
                                     ProgressCalculationType = b.ProgressCalculationType.name,
                                     relativeWeight = b.relativeWeight ?? 0,
                                     forEveryProject = b.forEveryProject ?? false,
                                     progress=0,
                                     stateOrder=0
                                 });
            var states = (from b in db.StatesTransitions
                          select new
                          {
                              type="s",
                              b.id,
                              b.name,
                              b.implementerTypeId,
                              PrerequisiteCategory = "فني",
                              ImplementerType = b.ImplementerType.name,
                              ProgressCalculationType = "تلقائي",
                              relativeWeight = b.relativeWeight ?? 0,
                              forEveryProject = true,
                              progress=0,
                              b.stateOrder
                          }).OrderBy(x=>x.stateOrder);
            dtData = prerequisites.Union(states).CopyToDataTable();
            gdvData.DataSource = dtData;
            gdvData.DataBind();
        }
    }
    public int RandomNumber(int min, int max)
    {
        Random random = new Random();
        return random.Next(min, max);
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        ClearControls();
        mpeObject.Show();
    }
}