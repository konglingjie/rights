﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace App
{
    public class MvcApplication : System.Web.HttpApplication
    { 

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_Error(object sender, EventArgs e)
        {
            ExceptionHandlerStarter();
        }
        public void ExceptionHandlerStarter()
        {

            string s = HttpContext.Current.Request.Url.ToString();
            HttpServerUtility server = HttpContext.Current.Server;
            if (server.GetLastError() != null)
            {
                Exception lastError = server.GetLastError();
                Application["LastError"] = lastError;
                int statusCode = HttpContext.Current.Response.StatusCode;
                string exceptionOperator = System.Configuration.ConfigurationManager.AppSettings["ExceptionUrl"];
                try
                {
                    exceptionOperator = new System.Web.UI.Control().ResolveUrl(exceptionOperator);
                    if (!String.IsNullOrEmpty(exceptionOperator) && !s.Contains(exceptionOperator))
                    {
                        string url = string.Format("{0}?ErrorUrl={1}", exceptionOperator, server.UrlEncode(s));
                        string script = String.Format("<script language='javascript' type='text/javascript'>window.top.location='{0}';</script>", url);
                        Response.Write(script);
                        Response.End();
                    }
                }
                catch (Exception)
                {
                }

            }
        }
    }
}

