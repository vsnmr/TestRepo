using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using EBillApp.Models;
using EBillApp.Repository;
using NLog;

namespace EBillApp.Controllers
{
    public class EBillController : Controller
    {
       // GET: EBill
     private static Logger logger = LogManager.GetCurrentClassLogger();
        /*   public ActionResult LoggerFile()
        {
            try
            {
                string studentName = "Vaibhav123";
                ValidateStudent(studentName);
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private static void ValidateStudent(string studentName)
        {
            Regex regex = new Regex("^[a-zA-Z]+$");
            if (!regex.IsMatch(studentName))
                throw new Exception(studentName);
        }*/

        public ActionResult Index()
        {
            Data data = new Data();
            var list = data.GetAllDetails();
            return View(list);
        }

        public ActionResult Create()
        {

            return View();
            //new BillDetail { CustomerAddress = "solapur", CustomerName = "shiva", MobileNumber = "7676565676" }
        }
        [HttpPost]
        public ActionResult Create(BillDetail detail)
        {
            Data dt = new Data();
            try
            {
               
                    dt.SaveBillDetails(detail);
                    ModelState.Clear();

                int a = 120;
               int b = 0;
                if (b == 0)
                {
                    var n = a / b;
                }
                else
                {
                    ModelState.AddModelError("", "Division by zero is not allowed.");
                }


            }
            catch(Exception ex){

                logger.Error(ex.Message);
                ModelState.AddModelError("", "An error occurred while creating the bill detail.");
            }
            
            return View();
        }
        
        public ActionResult CreateItem(Items item)
        {
            return PartialView("CreateItem", item);
        }
        public ActionResult ViewBill(int id)
        {
            Data data = new Data();
            var billDetails = data.GetDetail(id);
            return View(billDetails);
        }

    }
}//end the Project 