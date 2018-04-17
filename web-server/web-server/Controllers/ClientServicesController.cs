using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using web_server.Models;
using web_server.Model;
using System.Data.Entity;


namespace web_server.Controllers
{
    public class ClientServicesController : Controller
    {
        DatabaseModelContainer db;
        ApplicationDbContext context;
        int numPerPage = 4;


        public ClientServicesController() : this(null, null)
        {
            // Default constructor
        }

        public ClientServicesController(DatabaseModelContainer _db = null, ApplicationDbContext _cntxt = null)
        {
            if (_db == null)
                db = new DatabaseModelContainer();
            else
                db = _db;

            if (_cntxt == null)
                context = new ApplicationDbContext();
            else
                context = _cntxt;
        }

        // GET: ClientServices
        public ActionResult Index(int page=1)
        {

            string id = User.Identity.GetUserId();
            User user = db.UserSet.Where(u => u.NetUserId == id).FirstOrDefault();

            IEnumerable<ServiceChoosed> servicesChoosedToReturn = db.ServiceChoosedSet.Where(s => s.CustomerId == user.Id && s.FinishedByCustomer != true && s.FinishedByProvider != true).OrderBy(s => !s.Accepted).ThenBy(s => s.FinishedByProvider).ToList();

            int skip = (page - 1) * numPerPage;
            var count = Math.Ceiling((double)servicesChoosedToReturn.Count() / numPerPage);


            servicesChoosedToReturn = servicesChoosedToReturn.Skip(skip).Take(numPerPage).ToList();

            ViewBag.Page = page;
            ViewBag.NumOfPages = count;

            return View(servicesChoosedToReturn);
        }

        public ActionResult PreviousServices(int page=1)
        {
            string id = User.Identity.GetUserId();
            User user = db.UserSet.Where(u => u.NetUserId == id).FirstOrDefault();

            IEnumerable<ServiceChoosed> servicesChoosedToReturn = db.ServiceChoosedSet.Where(s => s.CustomerId == user.Id && s.FinishedByCustomer == true).ToList();

            int skip = (page - 1) * numPerPage;
            var count = Math.Ceiling((double)servicesChoosedToReturn.Count() / numPerPage);

            servicesChoosedToReturn = servicesChoosedToReturn.Skip(skip).Take(numPerPage).ToList();

            ViewBag.Page = page;
            ViewBag.NumOfPages = count;

            return View(servicesChoosedToReturn);
        }

        public ActionResult ToRateServices(int page=1)
        {
            string id = User.Identity.GetUserId();
            User user = db.UserSet.Where(u => u.NetUserId == id).FirstOrDefault();

            IEnumerable<ServiceChoosed> servicesChoosedToReturn = db.ServiceChoosedSet.Where(s => s.CustomerId == user.Id && s.FinishedByProvider == true && s.FinishedByCustomer != true).ToList();

            int skip = (page - 1) * numPerPage;
            var count = Math.Ceiling((double)servicesChoosedToReturn.Count() / numPerPage);

            servicesChoosedToReturn = servicesChoosedToReturn.Skip(skip).Take(numPerPage).ToList();

            ViewBag.Page = page;
            ViewBag.NumOfPages = count;

            return View(servicesChoosedToReturn);
        }

        [HttpPost]
        public ActionResult RateService(int serviceID, int mark)
        {
            string id = User.Identity.GetUserId();
            User user = db.UserSet.Where(u => u.NetUserId == id).FirstOrDefault();
            var service = db.ServiceChoosedSet.FirstOrDefault(s => s.Id == serviceID);

            service.Rate = new Rate {
                Mark = mark,
                Provider = service.Provider,
                Customer = user as Customer
            };
            service.FinishedByCustomer = true;  
            db.SaveChanges();

            return new HttpStatusCodeResult(200);
        }

    }
}      