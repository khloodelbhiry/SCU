using DevExpress.Web.ASPxTreeView;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class project_prerequisites : System.Web.UI.Page
{
    public List<PrerequisitesPermissions> PrerequisitesPermissions
    {
        get
        {
            if (Session["PrerequisitesPermissions"] != null && Session["PrerequisitesPermissions"].ToString() != string.Empty)
                return global::PrerequisitesPermissions.DeSerializePermissionsList(Session["PrerequisitesPermissions"].ToString());
            return new List<PrerequisitesPermissions>();
        }
        set { Session["PrerequisitesPermissions"] = global::PrerequisitesPermissions.SerializePermissionsList(value); }
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
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            HtmlAnchor lnkHistory = (HtmlAnchor)Master.FindControl("lnkHistory");
            lnkHistory.HRef = "log.aspx?t=" + EncryptString.Encrypt("ProcessPrerequisites"+";"+ "ProjectPrerequisites") + "&p=" + Request.QueryString["id"];
            if (Session["User"] != null && Session["User"].ToString() != string.Empty)
            {
                if (UserPermissions.Any(
                    p =>
                        p.PageUrl.ToLower().Equals(Common.ProjectPrerequisitesPath) &&
                        (p.Show.Equals(true) || p.Edit.Equals(true) || p.Approve.Equals(true))))
                {
                    if (Request.QueryString["id"] != null && Request.QueryString["g"] != null)
                    {
                        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
                        {
                            var per = UserPermissions.FirstOrDefault(p => p.PageUrl.ToLower().Equals(Common.ProjectPrerequisitesPath));
                            Project query = db.Projects.FirstOrDefault(x => x.id == int.Parse(EncryptString.Decrypt(Request.QueryString["id"])));
                            ((HtmlGenericControl)Page.Master.FindControl("ulBreadcrumb")).InnerHtml = "<li><i class='ace-icon fa fa-home home-icon'></i><a href ='Default.aspx'> الرئيسية </a></li><li>" + per.ModuleName + "</li><li><strong>المشروع : " + query.name + "</strong></li><li><strong>الجهة : " + query.GovernmentalEntity.name + "</strong></li><li><strong>الشركة المنفذة : " + query.Company.name + "</strong></li><li><a class='active'>" + per.PageName + "</a></li>";
                            Page.Title = per.PageName;
                            GetProjectProgress(query.id, query.noOfPages ?? 0);
                            gdvData.Columns[6].HeaderText = "نسبة الإنجاز من " + query.noOfPages.ToString();
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
            if (UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectPrerequisitesPath) && (p.Show.Equals(true) || p.Edit.Equals(true) || p.Approve.Equals(true))))
                BindData();
        }
    }
    private void GetProjectProgress(int id, int noOfPages)
    {
        dtProgress = new DataTable();
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            string progress = db.fn_ProjectProgress(id, noOfPages);
            string[] result1 = progress.TrimStart(';').Split(';');
            foreach (var i in result1)
            {
                string[] result2 = i.Split(',');
                dtProgress.Rows.Add(double.Parse(result2[0].Split(':')[0]), int.Parse(result2[1]), int.Parse(result2[2]), result2[3]);
            }
        }
    }
    public string GetProgress(int id, string type,double relativeWeight)
    {
        DataRow[] dr = dtProgress.Select(string.Format("id ={0} AND type='{1}' ", id, type));
        double progress = 0;
        foreach (DataRow row in dr)
        {
            double val = (double.Parse(row["progress"].ToString(), NumberStyles.Float) / relativeWeight) * double.Parse("100");
            progress += Double.IsNaN(val) ? 0 : val;
        }
        return progress.ToString("N15").TrimEnd(new char[] { '0' }).TrimEnd(new char[] { '.' });
    }
    public bool CheckVisible(int id)
    {
        return PrerequisitesPermissions.Any(p => p.ID.Equals(id));
    }
    private void BindData()
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            var prerequisites = (from b in db.ProcessPrerequisites
                                 where b.forEveryProject == true
                                 select new
                                 {
                                     type = 'p',
                                     b.id,
                                     b.name,
                                     b.relativeWeight,
                                     StartDate = db.ProjectPrerequisites.Any(x => x.processPrerequisiteId == b.id && x.projectId == int.Parse(EncryptString.Decrypt(Request.QueryString["id"]))) ? db.ProjectPrerequisites.FirstOrDefault(x => x.processPrerequisiteId == b.id && x.projectId == int.Parse(EncryptString.Decrypt(Request.QueryString["id"]))).startDate : null,
                                     EndDate = db.ProjectPrerequisites.Any(x => x.processPrerequisiteId == b.id && x.projectId == int.Parse(EncryptString.Decrypt(Request.QueryString["id"]))) ? db.ProjectPrerequisites.FirstOrDefault(x => x.processPrerequisiteId == b.id && x.projectId == int.Parse(EncryptString.Decrypt(Request.QueryString["id"]))).endDate : null,
                                     DoneDate = db.ProjectPrerequisites.Any(x => x.processPrerequisiteId == b.id && x.projectId == int.Parse(EncryptString.Decrypt(Request.QueryString["id"]))) ? db.ProjectPrerequisites.FirstOrDefault(x => x.processPrerequisiteId == b.id && x.projectId == int.Parse(EncryptString.Decrypt(Request.QueryString["id"]))).doneDate : null,
                                     ImplementerType = b.ImplementerType.name,
                                     ProgressCalculationType = b.ProgressCalculationType.name,
                                     b.progressCalculationTypeId,
                                     stateOrder=0,
                                     Progress = GetProgress(b.id, "p",Convert.ToDouble(b.relativeWeight??0))
                                 }).ToList();
            var states = (from b in db.StatesTransitions
                          select new
                          {
                              type='s',
                              b.id,
                              b.name,
                              b.relativeWeight,
                              StartDate = db.ProjectStateTransitions.Any(x => x.stateTransitionId == b.id && x.projectId == int.Parse(EncryptString.Decrypt(Request.QueryString["id"]))) ? (DateTime?)db.ProjectStateTransitions.FirstOrDefault(x => x.stateTransitionId == b.id && x.projectId == int.Parse(EncryptString.Decrypt(Request.QueryString["id"]))).startDate : null,
                              EndDate = db.ProjectStateTransitions.Any(x => x.stateTransitionId == b.id && x.projectId == int.Parse(EncryptString.Decrypt(Request.QueryString["id"]))) ? (DateTime?)db.ProjectStateTransitions.FirstOrDefault(x => x.stateTransitionId == b.id && x.projectId == int.Parse(EncryptString.Decrypt(Request.QueryString["id"]))).endDate : null,
                              DoneDate = (DateTime?)null,
                              ImplementerType = b.ImplementerType.name,
                              ProgressCalculationType = "تلقائي",
                              progressCalculationTypeId = (int?)1,
                              b.stateOrder,
                              Progress = GetProgress(b.id, "s", Convert.ToDouble(b.relativeWeight??0))
                          }).OrderBy(x=>x.stateOrder).ToList();
            var query = prerequisites.Union(states);
            dtData = query.CopyToDataTable();
            gdvData.DataSource = dtData;
            gdvData.DataBind();
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
    protected void btnSave_Click(object sender, EventArgs e)
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            try
            {
                    var culture = new CultureInfo("en-US");
                if (ViewState["id"].ToString().Split(';')[1] == "p")
                {
                    ProjectPrerequisite t = new ProjectPrerequisite();
                    t.projectId = int.Parse(EncryptString.Decrypt(Request.QueryString["id"]));
                    t.processPrerequisiteId = int.Parse(ViewState["id"].ToString().Split(';')[0]);
                    t.startDate = DateTime.ParseExact(txtStartDateMonth.Text.PadLeft(2, '0') + "/" + txtStartDateDay.Text.PadLeft(2, '0') + "/" + txtStartDateYear.Text, "MM/dd/yyyy", culture);
                    t.endDate = DateTime.ParseExact(txtEndDateMonth.Text.PadLeft(2, '0') + "/" + txtEndDateDay.Text.PadLeft(2, '0') + "/" + txtEndDateYear.Text, "MM/dd/yyyy", culture);
                    db.ProjectPrerequisites.InsertOnSubmit(t);
                    db.SubmitChanges();
                    LogWriter.LogWrite("ProjectPrerequisites", ((int)ActivitiesEnum.Approve).ToString(), t.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, EncryptString.Decrypt(Request.QueryString["id"]), t.ProcessPrerequisite.name);
                }
                else
                {
                    ProjectStateTransition t = new ProjectStateTransition();
                    t.projectId = int.Parse(EncryptString.Decrypt(Request.QueryString["id"]));
                    t.stateTransitionId = int.Parse(ViewState["id"].ToString().Split(';')[0]);
                    t.startDate = DateTime.ParseExact(txtStartDateMonth.Text.PadLeft(2, '0') + "/" + txtStartDateDay.Text.PadLeft(2, '0') + "/" + txtStartDateYear.Text, "MM/dd/yyyy", culture);
                    t.endDate = DateTime.ParseExact(txtEndDateMonth.Text.PadLeft(2, '0') + "/" + txtEndDateDay.Text.PadLeft(2, '0') + "/" + txtEndDateYear.Text, "MM/dd/yyyy", culture);
                    db.ProjectStateTransitions.InsertOnSubmit(t);
                    db.SubmitChanges();
                    LogWriter.LogWrite("ProjectStateTransitions", ((int)ActivitiesEnum.Approve).ToString(), t.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, EncryptString.Decrypt(Request.QueryString["id"]), t.StatesTransition.name);
                }
                txtEndDateDay.Text = txtEndDateMonth.Text = txtEndDateYear.Text = txtStartDateDay.Text = txtStartDateMonth.Text = txtStartDateYear.Text = string.Empty;
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
    protected void lnkCloseModal_Click(object sender, EventArgs e)
    {
        ClearControls();
    }
    private void ClearControls()
    {
        ViewState["id"] = null;
        txtStartDateDay.Text = txtStartDateMonth.Text = txtStartDateYear.Text = txtEndDateDay.Text = txtEndDateMonth.Text = txtEndDateYear.Text = string.Empty;
        mpeObject.Hide();
    }
    protected void btnApprove_Command(object sender, CommandEventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectPrerequisitesPath) && p.Approve.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للاعتماد');</script>", false);
            return;
        }
        ViewState["id"] = e.CommandArgument.ToString();
        mpeObject.Show();
    }
    protected void lnkProgress_Command(object sender, CommandEventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectPrerequisitesPath) && p.Edit.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للتعديل');</script>", false);
            return;
        }
        ViewState["progress"] = e.CommandArgument.ToString();
        txtProgress.Text = decimal.Parse(e.CommandArgument.ToString().Split(';')[1]).ToString("G29");
        mpeProgress.Show();
    }

    protected void lnkCloseProgress_Click(object sender, EventArgs e)
    {
        ViewState["progress"] = null;
        mpeProgress.Hide();
    }

    protected void btnSaveProgress_Click(object sender, EventArgs e)
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            try
            {
                ProjectPrerequisite t = db.ProjectPrerequisites.FirstOrDefault(x => x.projectId == int.Parse(EncryptString.Decrypt(Request.QueryString["id"])) && x.processPrerequisiteId == int.Parse(ViewState["progress"].ToString().Split(';')[0]));
                t.progress = int.Parse(txtProgress.Text);
                db.SubmitChanges();
                LogWriter.LogWrite("ProjectPrerequisites", ((int)ActivitiesEnum.Update).ToString(), t.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, EncryptString.Decrypt(Request.QueryString["id"]), "تعديل نسبة الإنجاز - "+t.ProcessPrerequisite.name); 
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

    protected void lnkDone_Command(object sender, CommandEventArgs e)
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            try
            {
                ProjectPrerequisite t = db.ProjectPrerequisites.FirstOrDefault(x => x.projectId == int.Parse(EncryptString.Decrypt(Request.QueryString["id"])) && x.processPrerequisiteId == int.Parse(e.CommandArgument.ToString()));
                if (t.ProcessPrerequisite.previousPrerequisiteId != 0)
                {
                    ProjectPrerequisite prev = db.ProjectPrerequisites.FirstOrDefault(x => x.projectId == int.Parse(EncryptString.Decrypt(Request.QueryString["id"])) && x.processPrerequisiteId == t.ProcessPrerequisite.previousPrerequisiteId);
                    if (prev == null || prev.doneDate == null)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('لابد من اتمام الخطوه السابقة اولا');</script>", false);
                        return;
                    }
                }
                t.doneDate = DateTime.Now;
                t.doneBy = UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID;
                db.SubmitChanges();
                LogWriter.LogWrite("ProjectPrerequisites", ((int)ActivitiesEnum.Update).ToString(), t.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, EncryptString.Decrypt(Request.QueryString["id"]), "تم - " + t.ProcessPrerequisite.name);
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