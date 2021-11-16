using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Security;
using System.Web.SessionState;

namespace WebFacturacion
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            ScriptResourceDefinition myScriptResDef = new ScriptResourceDefinition();
            myScriptResDef.Path = "~/Contents/Scripts/jquery-1.4.2.min.js";
            myScriptResDef.DebugPath = "~/Contents/Scripts/jquery-1.4.2.js";
            myScriptResDef.CdnPath = "http://ajax.microsoft.com/ajax/jQuery/jquery-1.12.4.min.js";
            myScriptResDef.CdnDebugPath = "http://ajax.microsoft.com/ajax/jQuery/jquery-1.12.4.min.js";
            ScriptManager.ScriptResourceMapping.AddDefinition("jquery", null, myScriptResDef);

            myScriptResDef.Path = "~/Contents/Scripts/jquery-ui.js";
            myScriptResDef.DebugPath = "~/Contents/Scripts/jquery-ui.js";
            ScriptManager.ScriptResourceMapping.AddDefinition("jquery-ui", null, myScriptResDef);

            myScriptResDef.Path = "~/Contents/Scripts/bootstrap.js";
            myScriptResDef.DebugPath = "~/Contents/Scripts/bootstrap.js";
            ScriptManager.ScriptResourceMapping.AddDefinition("bootstrap", null, myScriptResDef);

            Application["Title"] = "VIVA Aerobus-Facturación";
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}