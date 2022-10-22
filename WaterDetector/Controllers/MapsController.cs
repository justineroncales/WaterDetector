using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WaterDetector.Code;

namespace WaterDetector.Controllers
{
    public class MapsController : Controller
    {
        // GET: Maps
        public ActionResult Index(int ID)
        {
            try
            {
                Locations locations = new Locations();
                Users users = new Users();
                locations = users.GetUserLocations(ID);
                return View(locations);
            }
            catch
            {
                return View();
            }
        }
 
    }
}