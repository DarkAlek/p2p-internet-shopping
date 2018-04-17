using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web;
using web_server.Model;
using web_server.Models;

namespace web_server.Filters
{
    public class AdminApiAuthenticationAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext) {
            var context = new ApplicationDbContext();
            var db = new DatabaseModelContainer();

            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            }
            else
            {
                string authToken = actionContext.Request.Headers.Authorization.Parameter;
                string decodedToken = Encoding.UTF8.GetString(Convert.FromBase64String(authToken));
                string username = decodedToken.Substring(0, decodedToken.IndexOf(":"));
                string password = decodedToken.Substring(decodedToken.IndexOf(":") + 1);

                var user = context.Users.FirstOrDefault(u => u.UserName == username);
                var admin = db.Set<Admin>().FirstOrDefault(u => u.NetUserId == user.Id);

                if (!(admin is Admin))
                {
                    actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
                    return;
                }

                var hasher = new PasswordHasher();

                if (hasher.VerifyHashedPassword(user.PasswordHash, password) == PasswordVerificationResult.Success)
                    HttpContext.Current.User = new GenericPrincipal(new System.Security.Principal.GenericIdentity(username), null);
                else
                    actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            }


        }
    }
}