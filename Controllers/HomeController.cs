using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using XCalibre.Models.Helpers;

namespace XCalibre.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            Email model = new Email();
            return View(model);
        }
        
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        ////public async Task<ActionResult> Contact(Email model)
        ////{
        ////    if (ModelState.IsValid)
        ////    {
        ////        try
        ////        {
        ////            var body = "<p>Email From: <bold>{0}</bold>({1})</p><p>Message:</p><p>{2}</p>";
        ////            var from = "XCalibre<XCalibreSystem@gmail.com>";
        ////            model.Body = "This is a placeholder body that needs to be replaced later.";
        ////            var email = new MailMessage(from, ConfigurationManager.AppSettings["emailto"])
        ////            {
        ////                Subject = "XCalibre Email",
        ////                Body = string.Format(body, model.FromName, model.FromEmail, model.Body),
        ////                IsBodyHtml = true
        ////            };
        ////            var svc = new PersonalEmail();
        ////            await svc.SendAsync(email);
        ////            return View(new Email());
        ////        }
        ////        catch (Exception ex)
        ////        {
        ////            Console.WriteLine(ex.Message);
        ////            await Task.FromResult(0);
        ////        }
        ////        return View(model);
        ////    }
        ////}
    }
}