using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class project_sites: System.Web.UI.Page
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
            lnkHistory.HRef = "log.aspx?t=" + EncryptString.Encrypt("WorkSites") + "&p=" + Request.QueryString["id"];
            if (Session["User"] != null && Session["User"].ToString() != string.Empty)
            {
                if (UserPermissions.Any(
                    p =>
                        p.PageUrl.ToLower().Equals(Common.ProjectSitesPath) &&
                        (p.Show.Equals(true) || p.Add.Equals(true) || p.Edit.Equals(true) || p.Print.Equals(true) || p.Approve.Equals(true) || p.Print.Equals(true))))
                {
                    if (Request.QueryString["id"] != null && Request.QueryString["g"] != null)
                    {
                        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
                        {
                            var per = UserPermissions.FirstOrDefault(p => p.PageUrl.ToLower().Equals(Common.ProjectSitesPath));
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
            if (UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectSitesPath) && (p.Show.Equals(true) || p.Print.Equals(true) || p.Edit.Equals(true) || p.Approve.Equals(true) || p.Print.Equals(true))))
                BindData();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (ViewState["id"] == null && !UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectSitesPath) && p.Add.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للاضافة');</script>", false);
            return;
        }
        using (SCU_OneTrackDataContext db=new SCU_OneTrackDataContext())
        {
            try
            {
                if (ViewState["id"] == null)
                {
                    WorkSite l = new WorkSite();
                    l.projectId = int.Parse(EncryptString.Decrypt(Request.QueryString["id"]));
                    l.lng = hdfLongitude.Value;
                    l.lat = hdfLatitude.Value;
                    l.internetLineSpeed = txtInternetLineSpeed.Text.Trim();
                    l.maintenancePerson = txtMaintenancePerson.Text;
                    l.maintenancePersonPhone = txtMaintenancePersonPhone.Text;
                    l.notes = txtNotes.Text;
                    l.noOfOffices = int.Parse(txtOffices.Text);
                    l.phone = txtPhone.Text;
                    l.requiredSpace = txtRequiredArea.Text;
                    l.noOfFilingCabinets = int.Parse(txtWheels.Text);
                    l.electricitySource = chkElecSource.Checked;
                    l.closeToBathroom = chkCloseToBathroom.Checked;
                    l.airConditioner = chkAirConditio.Checked;
                    l.noOfChairs = int.Parse(txtChairs.Text);
                    l.address = txtAddress.Text;
                    l.actualArea = txtActualArea.Text;
                    l.noOfTables = int.Parse(txtTables.Text);
                    l.securityPerson = txtSecurityPerson.Text;
                    l.securityPersonPhone = txtSecurityPersonPhone.Text;
                    db.WorkSites.InsertOnSubmit(l);
                    db.SubmitChanges();
                    LogWriter.LogWrite("WorkSites", ((int)ActivitiesEnum.Add).ToString(), l.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, EncryptString.Decrypt(Request.QueryString["id"]), string.Empty);
                }
                else
                {
                    WorkSite l = db.WorkSites.FirstOrDefault(x => x.id == int.Parse(ViewState["id"].ToString()));
                    l.lng = hdfLongitude.Value;
                    l.lat = hdfLatitude.Value;
                    l.internetLineSpeed = txtInternetLineSpeed.Text.Trim();
                    l.maintenancePerson = txtMaintenancePerson.Text;
                    l.maintenancePersonPhone = txtMaintenancePersonPhone.Text;
                    l.notes = txtNotes.Text;
                    l.noOfOffices = int.Parse(txtOffices.Text);
                    l.phone = txtPhone.Text;
                    l.requiredSpace = txtRequiredArea.Text;
                    l.noOfFilingCabinets = int.Parse(txtWheels.Text);
                    l.electricitySource = chkElecSource.Checked;
                    l.closeToBathroom = chkCloseToBathroom.Checked;
                    l.airConditioner = chkAirConditio.Checked;
                    l.noOfChairs = int.Parse(txtChairs.Text);
                    l.address = txtAddress.Text;
                    l.actualArea = txtActualArea.Text;
                    l.noOfTables = int.Parse(txtTables.Text);
                    l.securityPerson = txtSecurityPerson.Text;
                    l.securityPersonPhone = txtSecurityPersonPhone.Text;
                    db.SubmitChanges();
                    LogWriter.LogWrite("WorkSites", ((int)ActivitiesEnum.Update).ToString(), l.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, EncryptString.Decrypt(Request.QueryString["id"]), string.Empty);
                }
                ScriptManager.RegisterStartupScript(this, GetType(), "alertDocumentType", "alert('تم الحفظ بنجاح .');", true);
                BindData();
                ClearControls();
            }
            catch (Exception ex)
            {
                Common.InsertException(ex.Message, ex.StackTrace,
                          new System.IO.FileInfo(System.Web.HttpContext.Current.Request.Url.AbsolutePath).Name);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('حدث خطا اثناء الحفظ');</script>", false);
            }
        }
    }

    private void ClearControls()
    {
      txtSecurityPerson.Text=txtSecurityPersonPhone.Text=  txtActualArea.Text = txtAddress.Text = txtChairs.Text = txtInternetLineSpeed.Text = txtMaintenancePerson.Text = txtMaintenancePersonPhone.Text = txtNotes.Text = txtOffices.Text = txtPhone.Text = txtRequiredArea.Text = txtTables.Text = txtWheels.Text = string.Empty;
        hdfLatitude.Value = hdfLongitude.Value=null;ViewState["id"] = null;
        chkAirConditio.Checked = chkCloseToBathroom.Checked = chkElecSource.Checked = false;
    }

    private void BindData()
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            var query = from q in db.WorkSites
                        where q.projectId == int.Parse(EncryptString.Decrypt(Request.QueryString["id"]))
                        select new
                        {
                            q.id,
                            q.address,
                            q.prepareDate,
                            q.receivedDate,
                            q.lat,
                            q.lng,
                            IsPreparedVisible = q.prepareDate == null,
                            IsReceivedVisible=q.prepareDate!=null&&q.receivedDate==null
                        };
            gdvSites.DataSource = query;
            gdvSites.DataBind();
        }
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        ClearControls();
    }

    protected void lnkPrepared_Command(object sender, CommandEventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectSitesPath) && p.Approve.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للتجهيز');</script>", false);
            return;
        }
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            try
            {
                WorkSite l = db.WorkSites.FirstOrDefault(x => x.id == int.Parse(e.CommandArgument.ToString()));
                l.prepareDate = DateTime.Now;
                l.preparedBy = UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID;
                db.SubmitChanges();
                LogWriter.LogWrite("WorkSites", ((int)ActivitiesEnum.Update).ToString(), l.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, EncryptString.Decrypt(Request.QueryString["id"]), "تجهيز الموقع");
                ScriptManager.RegisterStartupScript(this, GetType(), "alertDocumentType","alert('تم الحفظ بنجاح .');", true);
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

    protected void lnkReceived_Command(object sender, CommandEventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectSitesPath) && p.Approve.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للاستلام');</script>", false);
            return;
        }
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            try
            {
                WorkSite l = db.WorkSites.FirstOrDefault(x => x.id == int.Parse(e.CommandArgument.ToString()));
                l.receivedDate = DateTime.Now;
                l.receivedBy = UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID;
                db.SubmitChanges();
                LogWriter.LogWrite("WorkSites", ((int)ActivitiesEnum.Update).ToString(), l.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, EncryptString.Decrypt(Request.QueryString["id"]), "استلام الموقع");
                ScriptManager.RegisterStartupScript(this, GetType(), "alertDocumentType","alert('تم الحفظ بنجاح .');", true);
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

    protected void lnkImages_Command(object sender, CommandEventArgs e)
    {
        ViewState["id"] = e.CommandArgument.ToString();
        BindImages();
        mpeImages.Show();
    }

    protected void lnkDetails_Command(object sender, CommandEventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectSitesPath) && p.Edit.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للتعديل');</script>", false);
            return;
        }
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            WorkSite l = db.WorkSites.FirstOrDefault(x => x.id == int.Parse(e.CommandArgument.ToString()));
            hdfLongitude.Value = l.lng;
            hdfLatitude.Value = l.lat;
            txtInternetLineSpeed.Text = l.internetLineSpeed;
            txtMaintenancePerson.Text = l.maintenancePerson;
            txtMaintenancePersonPhone.Text = l.maintenancePersonPhone;
            txtNotes.Text = l.notes;
            txtOffices.Text = l.noOfOffices != null ? l.noOfOffices.ToString() : string.Empty;
            txtPhone.Text = l.phone;
            txtRequiredArea.Text = l.requiredSpace;
            txtWheels.Text = l.noOfFilingCabinets != null ? l.noOfFilingCabinets.ToString() : string.Empty;
            chkElecSource.Checked = l.electricitySource ?? false;
            chkCloseToBathroom.Checked = l.closeToBathroom ?? false;
            chkAirConditio.Checked = l.airConditioner ?? false;
            txtChairs.Text = l.noOfChairs != null ? l.noOfChairs.ToString() : string.Empty;
            txtAddress.Text = l.address;
            txtActualArea.Text = l.actualArea;
            txtTables.Text = l.noOfTables != null ? l.noOfTables.ToString() : string.Empty;
            txtSecurityPerson.Text = l.securityPerson;
            txtSecurityPersonPhone.Text = l.securityPersonPhone;
            ViewState["id"] = l.id;
        }
    }

    protected void lnkPrint_Command(object sender, CommandEventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectSitesPath) && p.Print.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للطباعة');</script>", false);
            return;
        }
        LogWriter.LogWrite("WorkSites", ((int)ActivitiesEnum.Print).ToString(), e.CommandArgument.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, EncryptString.Decrypt(Request.QueryString["id"]), string.Empty);
        var uriReportSource = new Telerik.Reporting.UriReportSource { Uri = Server.MapPath("rptSiteLocation.trdp") };
        uriReportSource.Parameters.Add(new Telerik.Reporting.Parameter("id", int.Parse(e.CommandArgument.ToString())));
        ReportViewer1.ReportSource = uriReportSource;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'>fnPrintReport();</script>", false);
    }

    protected void btnSaveImages_Click(object sender, EventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectSitesPath) && p.Add.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للاضافة');</script>", false);
            return;
        }
        SaveImages();
        BindImages();
        mpeImages.Show();
    }

    private void BindImages()
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            var data = (from i in db.WorkSiteImages
                        where i.workSiteId.Equals(int.Parse(ViewState["id"].ToString()))
                        select new
                        {
                            i.id,
                            fileName = "SiteImages/" + i.fileName,
                            i.User.fullName,
                            i.date
                        });
            if (data.Any())
            {
                divNoImages.Visible = false;
                rpData.DataSource = data;
                rpData.DataBind();
            }
            else
                divNoImages.Visible = true;
        }
    }
    private void SaveImages()
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            try
            {
                if (fuImages.HasFile)
                {
                    foreach (HttpPostedFile item in fuImages.PostedFiles)
                    {
                        string fileName = Common.GetUniqueFileName(Server.MapPath("SiteImages/"), item.FileName);
                        item.SaveAs(Server.MapPath("SiteImages/" + fileName));
                        WorkSiteImage ui = new WorkSiteImage
                        {
                            workSiteId = int.Parse(ViewState["id"].ToString()),
                            fileName = fileName
                        };
                        ui.userId = UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID;
                        ui.date = DateTime.Now;
                        db.WorkSiteImages.InsertOnSubmit(ui);
                        db.SubmitChanges();
                        LogWriter.LogWrite("WorkSites", ((int)ActivitiesEnum.Add).ToString(),ui.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, EncryptString.Decrypt(Request.QueryString["id"]), "اضافة صور");
                    }
                }
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

    protected void lnkDeleteImage_Command(object sender, CommandEventArgs e)
    {
        if (!UserPermissions.Any(p => p.PageUrl.ToLower().Equals(Common.ProjectSitesPath) && p.Delete.Equals(true)))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('عفوا، ليس لديك صلاحية للحذف');</script>", false);
            return;
        }
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            try
            {
                WorkSiteImage i =
                    db.WorkSiteImages.FirstOrDefault(
                        m =>
                            m.workSiteId.Equals(int.Parse(ViewState["id"].ToString())) &&
                            m.fileName.Equals(e.CommandArgument.ToString().Replace("SiteImages/", "")));
                if (i != null) db.WorkSiteImages.DeleteOnSubmit(i);
                db.SubmitChanges();
                LogWriter.LogWrite("WorkSites", ((int)ActivitiesEnum.Delete).ToString(), i.id.ToString(), DateTime.Now.ToString(), UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, EncryptString.Decrypt(Request.QueryString["id"]), "حذف صور");
                if (File.Exists(Server.MapPath("SiteImages/" + e.CommandArgument)))
                    File.Delete(Server.MapPath("SiteImages/" + e.CommandArgument));
                BindImages();
                mpeImages.Show();
                ScriptManager.RegisterStartupScript(this, GetType(), "alertUser",
                    "alert('تم الحذف بنجاح');", true);
            }
            catch (Exception ex)
            {
                Common.InsertException(ex.Message, ex.StackTrace,
                          new System.IO.FileInfo(System.Web.HttpContext.Current.Request.Url.AbsolutePath).Name);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Startup", "<script language='javascript'> alert('حدث خطا اثناء الحفظ');</script>", false);
            }
        }
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        ViewState["id"] = null;
        mpeImages.Hide();
    }
}