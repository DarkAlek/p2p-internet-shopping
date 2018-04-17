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
    public class ServicesController : Controller
    {
        DatabaseModelContainer db;
        ApplicationDbContext context;
        int numPerPage = 4;

        public ServicesController() : this(null, null)
        {
            // Default constructor
        }

        public ServicesController(DatabaseModelContainer _db = null, ApplicationDbContext _cntxt = null)
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

        // GET: Services
        public ActionResult Index(string region="", string sort="", int price_low=int.MinValue, int price_high=int.MaxValue, int page=1)
        {
            region = region.ToLower();

            int skip = (page - 1) * numPerPage;
            var services = db.ServiceSet.Where(s => s.Region.Name.ToLower().Contains(region) && s.Price >= price_low && s.Price <= price_high);
            var count = Math.Ceiling((double)services.Count() / numPerPage);

            IEnumerable<Service> servicesToReturn;

            if (sort == "price_desc")
                servicesToReturn = services.OrderByDescending(s => s.Price);
            else if(sort == "price_asc")
                servicesToReturn = services.OrderBy(s => s.Price);
            else if (sort == "rate_desc")
                servicesToReturn = services.OrderByDescending(s => s.Price);
            else if (sort == "rate_asc")
                servicesToReturn = services.OrderBy(s => s.Price);
            else
                servicesToReturn = services.OrderBy(s => s.Price);

            servicesToReturn = servicesToReturn.Skip(skip).Take(numPerPage).ToList();

            ViewBag.Region = region;
            ViewBag.Page = page;
            ViewBag.NumOfPages = count;

            return View(servicesToReturn);
        }

        public ActionResult Service(int id)
        {
            var service = db.ServiceSet.Find(id);
            ViewBag.Id = id;

            return View(service);
        }

        public ActionResult Order(int? id)
        {
            var service = db.ServiceSet.Find(id);
            ViewBag.Id = id;

            return View(service);
        }

        public ActionResult OrderSummary(int id, string note, System.Security.Principal.IIdentity mockedIdentity = null)
        {
            string userId;

            if (mockedIdentity != null)
                userId = mockedIdentity.GetUserId();
            else
                userId = User.Identity.GetUserId();

            var customer = db.UserSet.FirstOrDefault(u => u.NetUserId == userId) as Customer;

            var service = db.ServiceSet.Find(id);

            var ret = new
            {
                ProviderFirstName = service.Provider.FirstName,
                ProviderSecondName = service.Provider.SecondName,
                Price = service.Price,
                Location = service.Region.Name,
                Note = note,
            };

            return Json(ret);
        }

        public ActionResult OrderFinish(int id, string note)
        {
            var userId = User.Identity.GetUserId();
            var customer = db.UserSet.FirstOrDefault(u => u.NetUserId == userId) as Customer;
            if (customer == null)
                throw new HttpException(403, "You are not allowed to see that page");

            var service = db.ServiceSet.Find(id);
            var serviceChoosed = new ServiceChoosed()
            {
                Accepted = false,
                Customer = customer,
                CustomerNote = note,
                Provider = service.Provider,
                Service = service,
                FinishedByCustomer = false,
                FinishedByProvider = false,
            };

            try
            {
                db.ServiceChoosedSet.Add(serviceChoosed);
                db.SaveChanges();
            }
            catch
            {
                var fail = new
                {
                    Success = false
                };

                return Json(fail);
            }

            var pass = new
            {
                Success = true
            };

            return Json(pass);
        }
    }
}