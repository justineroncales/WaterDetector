using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WaterDetector.Code;

namespace WaterDetector.Controllers
{
    public class MobileController : Controller
    {
        // GET: Mobile
        public ActionResult Index()
        {
            return View();
        }

        // GET: Mobile/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Mobile/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Mobile/Create
        [HttpPost]
        public ActionResult Create(UsersDetails collection)
        {
            try
            {
                // TODO: Add insert logic here
                Users user = new Users();
                bool result =  user.Register(collection);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return null;
            }
        }
        
        [HttpPost]
        public ActionResult LocationUpdates(int ID,string status)
        {
            try
            {
                // TODO: Add insert logic here
                Users user = new Users();
                bool result = user.LocationUpdates(ID,status);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return null;
            }
        }

        [HttpPost]
        public ActionResult Login(UsersDetails collection)
        {
            try
            {
                // TODO: Add insert logic here
                Users user = new Users();
                int result = user.Login(collection);
                return Json(Convert.ToString(result), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return null;
            }
        }

        [HttpPost]
        public ActionResult SendReport(UsersDetails collection)
        {
            try
            {
                // TODO: Add insert logic here
                Users user = new Users();
                user.SendReports(collection);
                return Json(new HttpResponseMessage(System.Net.HttpStatusCode.OK));
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return null;
            }
        }
        // GET: Mobile/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Mobile/Edit/5
        [HttpGet]
        public ActionResult GetLocations(int ID)
        {
            try
            {
                Users users = new Users();
                string status= users.GetLocations(ID);
                return Json(status, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return View();
            }
        }

        // GET: Mobile/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Mobile/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
