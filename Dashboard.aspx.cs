using Newtonsoft.Json;
using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Dashboard: System.Web.UI.Page
{
    protected string MapCenter = string.Empty, MapMarkers = string.Empty;
    protected int MapZoomLevel = 8;
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
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            HtmlAnchor lnkHistory = (HtmlAnchor)Master.FindControl("lnkHistory");
            lnkHistory.Visible=false;
            using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
            {
                int pages = 0;
                var p = db.Projects;
                ((HtmlGenericControl)Page.Master.FindControl("ulBreadcrumb")).InnerHtml = "<li><i class='ace-icon fa fa-home home-icon'></i><a href ='Default.aspx'> الرئيسية </a></li><li><a class='active'>لوحة متابعة الإنجاز</a></li>";
                Page.Title = "لوحة متابعة الإنجاز";
                foreach (var i in p)
                {
                    pages += i.noOfPages ?? 0;
                }
                ltrProjectFiles.Text = (pages / 500).ToString() + " ملف";
                ltrPages.Text = pages.ToString() + " ورقة";
                ltrDocs.Text = (pages / 5).ToString() + " وثيقة";
                decimal progress = ((db.ProjectPrerequisites.Where(x => x.progress == 100).Sum(x => x.ProcessPrerequisite.relativeWeight) ?? 0)/db.Projects.Count()) / 100;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "noOfExportedPages", " var projectProgress=" + progress + ";var noOfExportedPages=0;", true);
            }
        }
    }
}