using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UCs_projects : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
            {
                ltrUsers.Text = db.Users.Count(x => x.companyId == int.Parse(EncryptString.Decrypt(Request.QueryString["id"]))).ToString();
                ltrAssets.Text = db.Assets.Count(x => x.companyId == int.Parse(EncryptString.Decrypt(Request.QueryString["id"]))).ToString();
            }
        }
    }
}