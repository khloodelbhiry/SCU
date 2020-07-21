using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class project_dashboard : System.Web.UI.Page
{
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
                    ((DataTable)ViewState["_dtProgress"]).Columns.Add("pages", typeof(double));
                }
            }
        }
    }
    private DataTable dtOperation
    {
        get
        {
            return ((DataTable)ViewState["_dtOperation"]);
        }
        set
        {
            if (value == null)
            {
                ViewState.Remove("_dtOperation");
            }
            else
            {
                ViewState["_dtOperation"] = value;
            }
        }
    }
    public DataTable dtColors
    {
        get
        {
            return ((DataTable)ViewState["_dtColors"]);
        }
        set
        {
            if (value == null)
            {
                ViewState.Remove("_dtColors");
            }
            else
            {

                if (ViewState["_dtColors"] == null)
                {
                    ViewState["_dtColors"] = new DataTable();
                }
                ViewState["_dtColors"] = value;
                if (((DataTable)ViewState["_dtColors"]).Columns.Count == 0)
                {
                    ((DataTable)ViewState["_dtColors"]).Columns.Add("color");
                    ((DataTable)ViewState["_dtColors"]).Columns.Add("hexa");
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
                        p.PageUrl.ToLower().Equals(Common.ProjectDashboardPath) &&
                        (p.Show.Equals(true))))
                {
                    if (Request.QueryString["id"] != null && Request.QueryString["g"] != null)
                    {
                        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
                        {
                            var per = UserPermissions.FirstOrDefault(p => p.PageUrl.ToLower().Equals(Common.ProjectDashboardPath));
                            Project query = db.Projects.FirstOrDefault(x => x.id == int.Parse(EncryptString.Decrypt(Request.QueryString["id"])));
                            ((HtmlGenericControl)Page.Master.FindControl("ulBreadcrumb")).InnerHtml = "<li><i class='ace-icon fa fa-home home-icon'></i><a href ='Default.aspx'> الرئيسية </a></li><li>" + per.ModuleName + "</li><li><strong>المشروع : " + query.name + "</strong></li><li><strong>الجهة : " + query.GovernmentalEntity.name + "</strong></li><li><strong>الشركة المنفذة : " + query.Company.name + "</strong></li><li><a class='active'>" + per.PageName + "</a></li>";
                            Page.Title = per.PageName;
                            GetProjectProgress(query.id, query.noOfPages ?? 0);
                            ViewState["noOfPages"] = query.noOfPages;
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
            daterange.Value = DateTime.Now.ToString("yyyy/MM/01") + " : " + DateTime.Now.ToString("yyyy/MM/dd");
            FillColors();
            BindProgress();
            BindTotalProgressValue();
            BindPrepareChart();
            BindDigitizationChart();
            BindOperationChart();
            BindPrerequisites();
            BindEmployeesCount();
            BindIssues();
            BindStatesForUnits();
            BindUnits();
            if (UserDetails.DeSerializeUserDetails(Session["User"].ToString()).UnitStructureId != 0)
                ddlUnits.SelectedValue = UserDetails.DeSerializeUserDetails(Session["User"].ToString()).UnitStructureId.ToString();
            lnkSearchUnits_Click(lnkSearchUnits, new EventArgs());
        }
    }      
    private void BindEmployeesCount()
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            ltrGovernorateEmp.Text = db.Users.Where(x => x.governmentalEntityId == int.Parse(EncryptString.Decrypt(Request.QueryString["g"]))).Count().ToString();
            ltrCompanyEmp.Text = db.Users.Where(x => x.companyId == int.Parse(EncryptString.Decrypt(Request.QueryString["c"]))).Count().ToString();
        }
    }
    private void BindTotalProgressValue()
    {
        double progress = 0;
        for (int i = 0; i < dtOperation.Rows.Count; i++)
        {
            double val = ((double.Parse(dtOperation.Rows[i]["progress"].ToString().Replace(",",""), NumberStyles.Float) / double.Parse("100")) * double.Parse(dtOperation.Rows[i]["relativeWeight"].ToString(), NumberStyles.Float));
            progress += Double.IsNaN(val) ? 0d : val;
        }
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "progress222", " var projectProgress=" + progress+ ";var noOfPages=" + ViewState["noOfPages"].ToString() + ";", true);
    }
    private void BindPrepareChart()
    {
        DataView view = dtOperation.DefaultView;
        view.RowFilter = string.Format("digitization ={0} AND type='{1}' AND relativeWeight <> 0 ", false, "s");
        rpPrepare.DataSource = view;
        rpPrepare.DataBind();
        string labels = string.Empty, data = string.Empty, colors = string.Empty;
        int index = 0;
        foreach (DataRowView rowView in view)
        {
            DataRow row = rowView.Row;
            labels += (labels != string.Empty ? "," : string.Empty) + "'" + row["name"] + "'";
            data += (data != string.Empty ? "," : string.Empty) + row["progress"];
            colors += (colors != string.Empty ? "," : string.Empty) + "'" + dtColors.Rows[index]["hexa"] + "'";
            index++;
        }
        StringBuilder sb = new StringBuilder();
        sb.Append("var preparedata = {");
        sb.Append("labels: [" + labels + "],");
        sb.Append("datasets: [{");
        sb.Append("data: [" + data + "],");
        sb.Append("backgroundColor: [" + colors + "]");
        sb.Append("}]");
        sb.Append("};");
        sb.Append("var chartPrepare = document.getElementById('chartPrepare');");
        sb.Append("var prepareChart = new Chart(chartPrepare, {");
        sb.Append("type: 'doughnut',");
        sb.Append("data: preparedata,");
        sb.Append("options: optionpie");
        sb.Append("});");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "prepare", sb.ToString(), true);
    }
    private void BindDigitizationChart()
    {
        DataView view = dtOperation.DefaultView;
        view.RowFilter = string.Format("digitization ={0} AND type='{1}' ", true, "s");
        rpDigitization.DataSource = view;
        rpDigitization.DataBind();
        string labels = string.Empty, data = string.Empty, colors = string.Empty;
        int index = 0;
        foreach (DataRowView rowView in view)
        {
            DataRow row = rowView.Row;
            labels += (labels != string.Empty ? "," : string.Empty) + "'" + row["name"] + "'";
            data += (data != string.Empty ? "," : string.Empty) + row["progress"];
            colors += (colors != string.Empty ? "," : string.Empty) + "'" + dtColors.Rows[index]["hexa"] + "'";
            index++;
        }
        StringBuilder sb = new StringBuilder();
        sb.Append("var digitizationdata = {");
        sb.Append("labels: [" + labels + "],");
        sb.Append("datasets: [{");
        sb.Append("data: [" + data + "],");
        sb.Append("backgroundColor: [" + colors + "]");
        sb.Append("}]");
        sb.Append("};");
        sb.Append("var chartDigitization = document.getElementById('chartDigitization');");
        sb.Append("var digitizationChart = new Chart(chartDigitization, {");
        sb.Append("type: 'doughnut',");
        sb.Append("data: digitizationdata,");
        sb.Append("options: optionpie");
        sb.Append("});");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "digitization", sb.ToString(), true);
    }
    private void BindPrerequisites()
    {
        DataView view = dtOperation.DefaultView;
        view.RowFilter = string.Format("type='{0}'", "p");
        rpPrerequisites.DataSource = view;
        rpPrerequisites.DataBind();
        decimal progress = 0;
        int count = 0;
        foreach (DataRowView rowView in view)
        {
            DataRow row = rowView.Row;
            progress += decimal.Parse(row["progress"].ToString());
            count++;
        }
        progress = progress / count;
        ltrPrerequisites.Text = "<div class='easy-pie-chart percentage' data-percent='" + Math.Round(progress, 2) + "' data-color='#87B87F'><span class='percent'>" + Math.Round(progress, 2) + "</span>%</div>";
    }
    private void BindOperationChart()
    {
        DataView view = dtOperation.DefaultView;
        view.RowFilter = string.Format("type='{0}'", "s");
        string labels = string.Empty, data = string.Empty;
        foreach (DataRowView rowView in view)
        {
            DataRow row = rowView.Row;
            labels += (labels != string.Empty ? "," : string.Empty) + "'" + row["name"] + "'";
            data += (data != string.Empty ? "," : string.Empty) + "" + row["progress"] + "";
        }
        StringBuilder sb = new StringBuilder();
        sb.Append("var data = {");
        sb.Append("labels: ["+labels+"],");
        sb.Append("series: [");
        sb.Append("{");
        sb.Append("data: ["+data+"]");
        sb.Append("}");
        sb.Append("]");
        sb.Append("};");
        sb.Append("var options = {");
        sb.Append("axisX: {");
        sb.Append("labelInterpolationFnc: function (value) {");
        sb.Append("return value;");
        sb.Append("}");
        sb.Append("}");
        sb.Append("};");
        sb.Append("var responsiveOptions = [");
        sb.Append("['screen and (min-width: 641px) and (max-width: 1024px)', {");
        sb.Append("showPoint: false,");
        sb.Append("axisX: {");
        sb.Append("labelInterpolationFnc: function(value) {");
        sb.Append("return 'Week ' + value;");
        sb.Append("}");
        sb.Append("}");
        sb.Append("}],");
        sb.Append("['screen and (max-width: 640px)', {");
        sb.Append("showLine: false,");
        sb.Append("axisX: {");
        sb.Append("labelInterpolationFnc: function(value) {");
        sb.Append("return 'W' + value;");
        sb.Append("}");
        sb.Append("}");
        sb.Append("}]");
        sb.Append("];");
        sb.Append("new Chartist.Line('.ct-chart', data, options, responsiveOptions);");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "operation", sb.ToString(), true);
    }
    private void BindProgress()
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            var prerequisites = (from b in db.ProcessPrerequisites
                                 where b.forEveryProject == true
                                 select new
                                 {
                                     type = 'p',
                                     b.id,
                                     name = b.name.Replace("\r\n", string.Empty),
                                     b.relativeWeight,
                                     Progress = GetProgress(b.id, "p", Convert.ToDouble(b.relativeWeight ?? 0)),
                                     digitization = false,
                                     pages = GetPages(b.id, "p"),
                                     objectId = 0
                                 }).ToList();
            var states = (from b in db.StatesTransitions
                          where b.relativeWeight > 0
                          && b.stateOrder != null
                          select new
                          {
                              type = 's',
                              b.id,
                              name = b.dashboardAliase.Replace("\r\n", string.Empty).Replace(" للجهة", "").Replace("في الصناديق", ""),
                              b.relativeWeight,
                              Progress = GetProgress(b.id, "s", Convert.ToDouble(b.relativeWeight ?? 0)),
                              digitization = b.digitization ?? false,
                              pages = GetPages(b.id, "s"),
                              objectId = b.objectId ?? 0
                          }).ToList();
            var query = prerequisites.Union(states);
            dtOperation = query.CopyToDataTable();
        }
    }
    private void GetProjectProgress(int id, int noOfPages)
    {
        dtProgress = new DataTable();
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            string progress = db.fn_ProjectProgress(id, noOfPages);
            string[] result1 = progress.TrimStart(';').Split(';');
            if (result1[0] != string.Empty)
            {
                foreach (var i in result1)
                {
                    string[] result2 = i.Split(',');
                    dtProgress.Rows.Add(double.Parse(result2[0].Split(':')[0]), int.Parse(result2[1]), int.Parse(result2[2]), result2[3], double.Parse(result2[0].IndexOf(':') > -1 ? result2[0].Split(':')[1] : "0"));
                }
            }
        }
    }
    public string GetProgress(int id, string type, double relativeWeight)
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
    public string GetPages(int id, string type)
    {
        DataRow[] dr = dtProgress.Select(string.Format("id ={0} AND type='{1}' ", id, type));
        double pages = 0;
        foreach (DataRow row in dr)
        {
            double val = double.Parse(row["pages"].ToString(), NumberStyles.Float);
            pages += Double.IsNaN(val) ? 0 : val;
        }
        return pages.ToString("N15").TrimEnd(new char[] { '0' }).TrimEnd(new char[] { '.' });
    }
    private void BindIssues()
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            var query=from x in db.Issues
                      where x.projectId == int.Parse(EncryptString.Decrypt(Request.QueryString["id"]))
                      group x by x.statusId into issue
                      select new { status = issue.Key, count = issue.Count() };
            decimal underApprove = 0;
            if (query.Where(x => x.status == (int)IssueStatusEnum.UnderApprove).Count()>0)
             underApprove = (decimal.Parse(query.Where(x => x.status == (int)IssueStatusEnum.UnderApprove).FirstOrDefault().count.ToString()) /decimal.Parse(query.Sum(x => x.count).ToString())) * decimal.Parse("100");
            decimal open = 0;
            if (query.Where(x => x.status == (int)IssueStatusEnum.Open).Count() > 0)
                open = (decimal.Parse(query.Where(x => x.status == (int)IssueStatusEnum.Open).Any()?query.Where(x => x.status == (int)IssueStatusEnum.Open).FirstOrDefault().count.ToString():"0") / decimal.Parse(query.Sum(x => x.count).ToString())) * decimal.Parse("100");
            decimal closed = 0;
            if (query.Where(x => x.status == (int)IssueStatusEnum.Closed).Count() > 0)
                closed = (decimal.Parse(query.Where(x => x.status == (int)IssueStatusEnum.Closed).Any()?query.Where(x => x.status == (int)IssueStatusEnum.Closed).FirstOrDefault().count.ToString():"0") / decimal.Parse(query.Sum(x => x.count).ToString())) * decimal.Parse("100");
            ltrIssuesUnderApprove.Text = "<div class='easy-pie-chart percentage' data-percent='"+Math.Round(underApprove,2)+"' data-color='#87CEEB'><span class='percent'>"+Math.Round(underApprove,2)+"</span>%</div>";
            ltrOpen.Text = "<div class='easy-pie-chart percentage' data-percent='" + Math.Round(open,2) + "' data-color='#D15B47'><span class='percent'>" + Math.Round(open,2) + "</span>%</div>";
            ltrClosed.Text = "<div class='easy-pie-chart percentage' data-percent='" + Math.Round(closed,2) + "' data-color='#87B87F'><span class='percent'>" + Math.Round(closed,2)  + "</span>%</div>";
            var issues = (from x in db.Issues
                          where x.projectId == int.Parse(EncryptString.Decrypt(Request.QueryString["id"]))
                          select new
                          {
                              month = x.date.Month,
                              year = x.date.Year
                          }).OrderByDescending(x => x.year).OrderByDescending(x => x.month).Distinct().Take(10).ToList();
            rpIssuesByDate.DataSource = issues;
            rpIssuesByDate.DataBind();
        }
    }
    protected void rpIssuesByDate_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        var rpIssues = (Repeater)e.Item.FindControl("rpIssues");
        HiddenField hdfMonth = (HiddenField)e.Item.FindControl("hdfMonth");
        HiddenField hdfYear = (HiddenField)e.Item.FindControl("hdfYear");
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            var query = from x in db.Issues
                        where x.date.Month == int.Parse(hdfMonth.Value)
                        && x.date.Year == int.Parse(hdfYear.Value)
                        select new
                        {
                            x.id,
                            MinutesAndHours = x.date.Hour.ToString().PadLeft(2, '0') + ":" + x.date.Minute.ToString().PadLeft(2, '0'),
                            Issue_Date = x.date.Date,
                            x.issue1,
                            x.User.fullName,
                            x.IssueSeverity.name,
                            x.severityId,
                            x.statusId,
                            status = x.IssueStatus.name
                        };
            rpIssues.DataSource = query;
            rpIssues.DataBind();
        }
    }
    public string GetMonthName(int month)
    {
        return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
    }
    private void FillColors()
    {
        dtColors = new DataTable();
        dtColors.Rows.Add("bg-pink", "#c6487e");
        dtColors.Rows.Add("bg-primary", "#0168fa");
        dtColors.Rows.Add("bg-teal", "#00cccc");
        dtColors.Rows.Add("bg-purple", "#6f42c1");
        dtColors.Rows.Add("bg-indigo", "#5b47fb");
        dtColors.Rows.Add("bg-orange", "#fd7e14");
        dtColors.Rows.Add("bg-lightblue", "#a5d7fd");
        dtColors.Rows.Add("bg-gray-900", "#1c273c");
        dtColors.Rows.Add("bg-light-red", "#b76e79");
        dtColors.Rows.Add("bg-pink", "#f10075");
        dtColors.Rows.Add("bg-pink", "#f10075");
        dtColors.Rows.Add("bg-pink", "#f10075");
        dtColors.Rows.Add("bg-pink", "#f10075");
        dtColors.Rows.Add("bg-pink", "#f10075");
        dtColors.Rows.Add("bg-pink", "#f10075");
        dtColors.Rows.Add("bg-pink", "#f10075");
        dtColors.Rows.Add("bg-pink", "#f10075");
        dtColors.Rows.Add("bg-pink", "#f10075");
    }
    private void BindStatesForUnits()
    {
        DataView view = dtOperation.DefaultView;
        view.RowFilter = string.Format("type='{0}'", "s");
        rpStates.DataSource = view;
        rpStates.DataBind();
    }
    private void BindUnits()
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            var query = from q in db.UnitStructures
                        where q.governmentalEntityId == int.Parse(EncryptString.Decrypt(Request.QueryString["g"]))
                        select new
                        {
                            q.id,
                            q.name
                        };
            rpUnits.DataSource = query;
            rpUnits.DataBind();
            ddlUnits.DataSource = query;
            ddlUnits.DataTextField = "name";
            ddlUnits.DataValueField = "id";
            ddlUnits.DataBind();
        }
    }
    protected void rpUnits_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        var rpUnitProgress = (Repeater)e.Item.FindControl("rpUnitProgress");
        HiddenField hdfUnitId = (HiddenField)e.Item.FindControl("hdfUnitId");
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("progress");
            DataView view = dtOperation.DefaultView;
            view.RowFilter = string.Format("type='{1}' ", true, "s");
            foreach (DataRowView rowView in view)
            {
                DataRow row = rowView.Row;
                dt.Rows.Add(double.Parse(db.fn_CalcStateProgressByUnit(int.Parse(row["id"].ToString()), int.Parse(hdfUnitId.Value), int.Parse(row["objectId"].ToString()), null, null).ToString()).ToString("N15").TrimEnd(new char[] { '0' }).TrimEnd(new char[] { '.' }));
            }
            rpUnitProgress.DataSource = dt;
            rpUnitProgress.DataBind();
        }
    }
    private void BindUnitProgress(int unitId, DateTime? from, DateTime? to)
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            DataView view = dtOperation.DefaultView;
            view.RowFilter = string.Format("type='{1}'", false, "s");
            string series = string.Empty,colors=string.Empty;
            int index = 0;
            foreach (DataRowView rowView in view)
            {
                DataRow row = rowView.Row;
                series += (series != string.Empty ? "," : string.Empty) + "{name:'" + row["name"] + "',"
                + "data:[";
                colors += (colors != string.Empty ? "," : string.Empty) + "'" + dtColors.Rows[index]["hexa"] + "'";
                string data = string.Empty;
                var query = db.SP_CalcStateProgressByUnitGroupByDate(int.Parse(row["id"].ToString()), unitId, int.Parse(row["objectId"].ToString()), from, to);
                foreach (var i in query)
                {
                    data += (data != string.Empty ? "," : string.Empty) + "[new Date('" + i.date.ToString("dd MMMM yyyy") + "').getTime()," + i.progess + "]";
                }
                series += data + "]}";
                index++;
            }
            series = "[" + series + "]";
            StringBuilder sb = new StringBuilder();
            sb.Append("if ($('#chartUnits').length) {");
            sb.Append("var options = {");
            sb.Append("chart: {");
            sb.Append("height: 300,");
            sb.Append("type: 'area',");
            sb.Append("stacked: true,");
            sb.Append("events: ");
            sb.Append("{");
            sb.Append("selection: function(chart, e) {");
            sb.Append("} ");
            sb.Append("}, ");
            sb.Append("toolbar: ");
            sb.Append("{ ");
            sb.Append("show: false, ");
            sb.Append("} ");
            sb.Append("}, ");
            sb.Append("colors: ["+colors+"], ");
            sb.Append("dataLabels: ");
            sb.Append("{ ");
            sb.Append("enabled: false ");
            sb.Append("}, ");
            sb.Append("stroke: ");
            sb.Append("{  ");
            sb.Append("curve: 'smooth', ");
            sb.Append("width: 1 ");
            sb.Append("}, ");
            sb.Append("series: " + series + ", ");
            sb.Append("fill: ");
            sb.Append("{");
            sb.Append("type: 'gradient', ");
            sb.Append("gradient: ");
            sb.Append("{");
            sb.Append("opacityFrom: 0.9, ");
            sb.Append("opacityTo: 0.4, ");
            sb.Append("}");
            sb.Append("}, ");
            sb.Append("legend: ");
            sb.Append("{");
            sb.Append("show: false, ");
            sb.Append("position: 'top',showForSingleSeries: false,showForZeroSeries: false,position: 'top',horizontalAlign: 'right' ");
            sb.Append("}, ");
            sb.Append("xaxis: ");
            sb.Append("{");
            sb.Append("type: 'datetime', ");
            sb.Append("labels:  {offsetX: -5,} ");
            sb.Append("}, ");
            sb.Append("yaxis: ");
            sb.Append("{");
            sb.Append("labels: ");
            sb.Append("{");
            sb.Append("show: false, ");
            sb.Append("}, ");
            sb.Append("}, ");
            sb.Append("responsive: [{");
            sb.Append("breakpoint: 480, ");
            sb.Append("options: ");
            sb.Append("{");
            sb.Append("xaxis: ");
            sb.Append("{");
            sb.Append("type: 'datetime', ");
            sb.Append("labels: ");
            sb.Append("{");
            sb.Append("offsetX: 0, ");
            sb.Append("}");
            sb.Append("}, ");
            sb.Append("}, ");
            sb.Append("}] ");
            sb.Append("}; ");
            sb.Append("var chart = new ApexCharts(");
            sb.Append("document.querySelector('#chartUnits'), ");
            sb.Append("options ");
            sb.Append("); ");
            sb.Append("chart.render(); ");
            sb.Append("}");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "units", sb.ToString(), true);
        }
    }

    protected void lnkSearchUnits_Click(object sender, EventArgs e)
    {
        string[] date = daterange.Value.Split(':');
        DateTime from = Convert.ToDateTime(date[0].Trim());
        DateTime to = Convert.ToDateTime(date[1].Trim());
        if (ddlUnits.SelectedValue != string.Empty)
            BindUnitProgress(int.Parse(ddlUnits.SelectedValue), from, to);
        BindPrepareChart();
        BindDigitizationChart();
        BindOperationChart();
    }
}