using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;

namespace XCalibre.Models.Helpers
{
    public static class Extensions
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        public static string GetFullName(this IIdentity user)
        {
            var claimsUser = (ClaimsIdentity)user;
            var claim = claimsUser.Claims.FirstOrDefault(c => c.Type == "Name");
            if (claim != null)
            {
                return claim.Value;
            }
            else
            {
                return null;
            }
        }

        public static string GetPic(this IIdentity user)
        {
            var claimsUser = (ClaimsIdentity)user;
            var claim = claimsUser.Claims.FirstOrDefault(c => c.Type == "Picture");
            if (claim != null)
            {
                return claim.Value;
            }
            else
            {
                return null;
            }
        }

        public static async Task RefreshAuthentication(this HttpContextBase context, ApplicationUser user)
        {
            context.GetOwinContext().Authentication.SignOut();
            await context.GetOwinContext().Get<ApplicationSignInManager>().SignInAsync(user, isPersistent: false, rememberBrowser: false);
            
        }

        public static bool In(this string source, params string[] list)
        {
            if (source == null) throw new ArgumentNullException("source");
            return list.Contains(source, StringComparer.OrdinalIgnoreCase);
        }
        //public static async Task<ApplicationUser> GetPmIdName(this Project project, string userId)
        //{ 
        //    var pm = await Task.Run(() => db.Users.Find(userId));
        //    return pm;
            
        //}
    }
}