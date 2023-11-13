using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using EBillApp.Models;

namespace EBillApp.Controllers
{
    public class EmailSetupController : Controller
    {
        // GET: EmailSetup
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(MvcGmail _mvcGmail )
        {
           
                MailMessage mail = new MailMessage("vsnmrgoud@gmail.com",_mvcGmail.To);
                
                mail.Subject = _mvcGmail.Subject;
               
                mail.Body = _mvcGmail.Body;
                mail.IsBodyHtml = false;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;

                NetworkCredential nc= new NetworkCredential("vsnmrgoud@gmail.com", "muccgwntcwfqdjom"); // Enter seders User name and password  
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = nc;

                smtp.Send(mail);

                return View();
           
            
        }
    }
}