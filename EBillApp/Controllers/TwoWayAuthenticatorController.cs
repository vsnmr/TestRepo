using EBillApp.Models;
using Google.Authenticator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;

namespace EBillApp.Controllers
{
    public class TwoWayAuthenticatorController : Controller
    {
        // GET: TwoWayAuthenticator
        public ActionResult Index()
        {
            if (Session["Username"] == null || Session["IsValidTwoFactorAuthentication"] == null || !(bool)Session["IsValidTwoFactorAuthentication"])
            {
                return RedirectToAction("Login");
            }
            return View();
        }
        public ActionResult Login()
        {
            Session["UserName"] = null;
            Session["IsValidTwoFactorAuthentication"] = null;
            return View();

        }
       
        [HttpPost]
        public ActionResult Login(LoginModel login)
        {
            bool status = false;

            if (Session["Username"] == null || Session["IsValidTwoFactorAuthentication"] == null || !(bool)Session["IsValidTwoFactorAuthentication"])
            {
                string googleAuthKey = WebConfigurationManager.AppSettings["GoogleAuthKey"];
                string UserUniqueKey = (login.UserName + googleAuthKey);

                //Take UserName And Password As Static - Admin As User And 12345 As Password
                if (login.UserName == "Admin" && login.Password == "12345")
                {
                    Session["UserName"] = login.UserName;

                    //Two Factor Authentication Setup
                    TwoFactorAuthenticator TwoFacAuth = new TwoFactorAuthenticator();
                    var setupInfo = TwoFacAuth.GenerateSetupCode("UdayDodiyaAuthDemo.com", login.UserName, ConvertSecretToBytes(UserUniqueKey, false), 300);
                    Session["UserUniqueKey"] = UserUniqueKey;
                    ViewBag.BarcodeImageUrl = setupInfo.QrCodeSetupImageUrl;
                    ViewBag.SetupCode = setupInfo.ManualEntryKey;
                    status = true;
                }
            }
            else
            {
                return RedirectToAction("Index","TwoWayAuthenticator");
            }
            ViewBag.Status = status;
            return View();
        }

        private static byte[] ConvertSecretToBytes(string secret, bool secretIsBase32) =>
            secretIsBase32 ? Base32Encoding.ToBytes(secret) : Encoding.UTF8.GetBytes(secret);


        public ActionResult TwoFactorAuthenticate()
        {
            var token = Request["CodeDigit"];
            TwoFactorAuthenticator TwoFacAuth = new TwoFactorAuthenticator();
            string UserUniqueKey = Session["UserUniqueKey"].ToString();
            bool isValid = TwoFacAuth.ValidateTwoFactorPIN(UserUniqueKey, token, false);
            if (isValid)
            {
                HttpCookie TwoFCookie = new HttpCookie("TwoFCookie");
                string UserCode = Convert.ToBase64String(MachineKey.Protect(Encoding.UTF8.GetBytes(UserUniqueKey)));

                Session["IsValidTwoFactorAuthentication"] = true;
                return RedirectToAction("Index");
            }

            ViewBag.Message = "Google Two Factor PIN is expired or wrong";
            return RedirectToAction("Login");
        }
        public ActionResult Logoff()
        {
            Session["UserName"] = null;
            Session["IsValidTwoFactorAuthentication"] = null;
            return RedirectToAction("Login");
        }
    }
}