using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for LinkButtonDefault
/// </summary>
namespace App_Code
{
    public class LinkButtonDefault : LinkButton
    {
        protected override void OnLoad(System.EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "addClickFunctionScript",
              _addClickFunctionScript, true);

            string script = string.Format(_addClickScript, ClientID);
            Page.ClientScript.RegisterStartupScript(GetType(), "click_" + ClientID,
              script, true);
            base.OnLoad(e);
        }

        private const string _addClickScript = "addClickFunction('{0}');";

        private const string _addClickFunctionScript =
          @"  function addClickFunction(id) {{
            var b = document.getElementById(id);
            if (b && typeof(b.click) == 'undefined') b.click = function() {{
                var result = true; if (b.onclick) result = b.onclick();
                if (typeof(result) == 'undefined' || result) {{ eval(b.getAttribute('href')); }}
            }}}};";
        public LinkButtonDefault()
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }
}