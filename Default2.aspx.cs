using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Reporting;

public partial class Default2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            var uriReportSource = new UriReportSource { Uri = Server.MapPath("rptDeliveryReport.trdp") };
            uriReportSource.Parameters.Add(new Telerik.Reporting.Parameter("id", 2013));
            ReportViewer1.ReportSource = uriReportSource;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'>fnPrintReport();</script>", false);
        }
    }
}