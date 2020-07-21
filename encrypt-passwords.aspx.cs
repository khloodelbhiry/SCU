using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class encrypt_passwords : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
            {
                var users = from u in db.Users
                            where u.id!=3
                            select u;
                foreach (var i in users)
                {
                    i.password = EncryptString.Encrypt(i.password);
                    db.SubmitChanges();
                }
            }
        }
    }
}