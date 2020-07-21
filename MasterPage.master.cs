using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class MasterPage : System.Web.UI.MasterPage
{
    public List<UserPermissions> UserPermissions
    {
        get
        {
            if (Session["UserPermissions"] != null && Session["UserPermissions"].ToString() != string.Empty)
                return global::UserPermissions.DeSerializePermissionsList(Session["UserPermissions"].ToString());
            return new List<UserPermissions>();
        }
        set { Session["UserPermissions"] = global::UserPermissions.SerializePermissionsList(value); }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["User"] != null)
                ltUsername.Text = UserDetails.DeSerializeUserDetails(Session["User"].ToString()).Name;
            else
                Response.Redirect("login.aspx");
            using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
            {
                Project p = db.Projects.FirstOrDefault();
                if (p != null)
                {
                    lnkArchiving.HRef = "project-files.aspx?id=" + EncryptString.Encrypt(p.id.ToString()) + "&g=" + EncryptString.Encrypt(p.governmentalEntityId.ToString()) + "&c=" + EncryptString.Encrypt(p.companyId.ToString());
                }
            }
        }
    }
    protected void lnkLogout_ServerClick(object sender, EventArgs e)
    {
        Session.Remove("User");
        Session.Remove("UserPermissions");
        Session.Remove("StateTransitionPermissions");
        Session.Remove("PrerequisitesPermissions");
        Response.Redirect("login.aspx");
    }


    protected void lnkChangePassword_ServerClick(object sender, EventArgs e)
    {
        mpeProject.Show();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            User u = db.Users.FirstOrDefault(x => x.id == UserDetails.DeSerializeUserDetails(Session["User"].ToString()).ID);
            if (u != null)
            {
                if (u.password ==EncryptString.Encrypt(txtOldPassword.Text))
                {
                    u.password = EncryptString.Encrypt(txtNewPassword.Text);
                    db.SubmitChanges();
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertUser", "alert('تم الحفظ بنجاح');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertUser", "alert('عفوا، كلمة السر القديمة غير صحيحة');", true);
                    mpeProject.Show();
                }
            }
        }
    }

    protected void lnkClosePoject_Click(object sender, EventArgs e)
    {
        mpeProject.Hide();
    }

    protected void lnkHistory_Click(object sender, EventArgs e)
    {

    }
}
