using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class DocumentTypes : System.Web.UI.Page
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
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            HtmlAnchor lnkHistory = (HtmlAnchor)Master.FindControl("lnkHistory");
            lnkHistory.Visible = false;
            ((HtmlGenericControl)Page.Master.FindControl("ulBreadcrumb")).InnerHtml = "<li>سجل الحالات</li>";
            Page.Title = "سجل الحالات";
            BindDDL();
            BindData();
        }
    }
    private void BindDDL()
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            ddlOperation.DataSource = db.ActivityTypes;
            ddlOperation.DataTextField = "name";
            ddlOperation.DataValueField = "id";
            ddlOperation.DataBind();
            ddlOperation.Items.Insert(0, new ListItem("-- اختر --", "0"));
        }
    }
    private void BindData()
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            try
            {
                string[] tables = EncryptString.Decrypt(Request.QueryString["t"]).Split(';');
                var query = from t in db.AuditLogs
                            where tables.Contains(t.tableName)
                            select new
                            {
                                t.id,
                                operation = t.ActivityType.name,
                                t.fieldName,
                                t.newValue,
                                t.objectId,
                                t.occurredAt,
                                t.oldValue,
                                t.User.fullName,
                                t.activityId,
                                t.companyId,
                                t.projectId,
                                t.description
                            };
                if (Request.QueryString["c"] != null)
                    query = query.Where(x => x.companyId == int.Parse(EncryptString.Decrypt(Request.QueryString["c"])));
                if (Request.QueryString["p"] != null)
                    query = query.Where(x => x.projectId == int.Parse(EncryptString.Decrypt(Request.QueryString["p"])));
                if (ddlOperation.SelectedValue != "0")
                    query = query.Where(x => x.activityId == int.Parse(ddlOperation.SelectedValue));
                if (txtDateFrom.Text.Trim() != string.Empty)
                    query = query.Where(x => x.occurredAt >= Convert.ToDateTime(txtDateFrom.Text.Trim()));
                if (txtDateTo.Text.Trim() != string.Empty)
                    query = query.Where(x => x.occurredAt <= Convert.ToDateTime(txtDateTo.Text.Trim()));
                lblResult.Text = query.Count().ToString();
                gdvData.DataSource = query.OrderByDescending(x=>x.occurredAt);
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
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindData();
    }
    protected void btnNewSearch_Click(object sender, EventArgs e)
    {
        txtDateTo.Text=txtDateFrom.Text = string.Empty;
        ddlOperation.SelectedValue = "0";
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
    protected void gdvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvData.PageIndex = e.NewPageIndex;
        BindData();
    }

    protected void lnkUpdate_Click(object sender, EventArgs e)
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(HttpContext.Current.Server.MapPath("Log"));
                IEnumerable<FileInfo> fileList = dir.GetFiles("log-*.txt");
                foreach (var i in fileList)
                {
                    string newFile = i.FullName.Replace("log-", "updated_log-");
                    File.Copy(i.FullName, newFile);
                    File.Delete(i.FullName);
                    string[] lines = File.ReadAllLines(newFile);
                    string delimiter = "#;                  ;#";
                    int count = 0;
                    foreach (string line in lines)
                    {
                        if (count > 3)
                        {
                            string[] result = line.Split(new string[] { delimiter }, StringSplitOptions.None);
                            AuditLog l = new AuditLog();
                            l.tableName = result[0];
                            l.activityId = (result[1] != string.Empty ? (int?)int.Parse(result[1]) : null);
                            l.objectId = (result[2] != string.Empty ? result[2] : null);
                            l.occurredAt = (result[3] != string.Empty ? (DateTime?)DateTime.Parse(result[3]) : null);
                            l.performedBy = (result[4] != string.Empty ? (int?)int.Parse(result[4]) : null);
                            l.fieldName = (result[5] != string.Empty ? result[5] : null);
                            l.oldValue = (result[6] != string.Empty ? result[6] : null);
                            l.newValue = (result[7] != string.Empty ? result[7] : null);
                            l.companyId = (result[8] != string.Empty ? (int?)int.Parse(result[8]) : null);
                            l.projectId = (result[9] != string.Empty ? (int?)int.Parse(result[9]) : null);
                            l.description = (result[10] != string.Empty ? result[10] : null);
                            db.AuditLogs.InsertOnSubmit(l);
                        }
                        count++;
                    }
                    db.SubmitChanges();
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertDocumentType", "alert('تم التحديث بنجاح .');", true);
                    BindData();
                }
            }
            catch (Exception ex)
            {
                Common.InsertException(ex.Message, ex.StackTrace,
                          new System.IO.FileInfo(System.Web.HttpContext.Current.Request.Url.AbsolutePath).Name);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('حدث خطا اثناء التحديث');</script>", false);
            }
        }
    }
}