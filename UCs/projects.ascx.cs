using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UCs_projects : System.Web.UI.UserControl
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
            using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
            {
                if (UserDetails.DeSerializeUserDetails(Session["User"].ToString()).CompanyId != 0)
                {
                    ltrFiles.Text = db.BoxFiles.Where(b => b.UnitStructure.governmentalEntityId.Equals(int.Parse(EncryptString.Decrypt(Request.QueryString["g"])))).Count().ToString();
                    ltrIssues.Text = db.Issues.Count(x => x.projectId.Equals(int.Parse(EncryptString.Decrypt(Request.QueryString["id"])))).ToString();
                    ltrSites.Text = db.WorkSites.Count(x => x.projectId == int.Parse(EncryptString.Decrypt(Request.QueryString["id"]))).ToString();
                    ltrStocks.Text = db.Stocks.Count(x => x.projectId == int.Parse(EncryptString.Decrypt(Request.QueryString["id"]))).ToString();
                    ltrUsers.Text = db.Users.Count(x => x.governmentalEntityId == int.Parse(EncryptString.Decrypt(Request.QueryString["g"]))).ToString();
                    ltrAssets.Text = db.Assets.Count(x => x.projectId == int.Parse(EncryptString.Decrypt(Request.QueryString["id"]))).ToString();
                }
                else
                    ltrFiles2.Text = db.BoxFiles.Where(b => b.unitStructureId.Equals(UserDetails.DeSerializeUserDetails(Session["User"].ToString()).UnitStructureId)).Count().ToString();
            }
        }
    }
}