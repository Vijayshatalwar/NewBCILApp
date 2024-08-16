using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Text;

namespace GreenplyLocalToCentralWebApi
{
    public class AuthenticationFilter:System.Web.Http.Filters.ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            }
            else
            {
                string AuthenticationToken = actionContext.Request.Headers.Authorization.Parameter;
                string DocodeToken = Encoding.UTF8.GetString(Convert.FromBase64String(AuthenticationToken));
                string UserName = DocodeToken.Substring(0, DocodeToken.IndexOf(":"));
                string Password = DocodeToken.Substring(DocodeToken.IndexOf(":") + 1);
                if (UserName.Trim() == "BARCODE" && Password.Trim() == "BARCODE@2020")
                {

                }
                else
                {
                    actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
                }
            }

            base.OnActionExecuting(actionContext);
        }
    }
}