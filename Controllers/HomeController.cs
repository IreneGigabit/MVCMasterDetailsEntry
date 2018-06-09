using MVCMasterDetailsEntry.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCMasterDetailsEntry.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }


        //Post action for Save data to database
        [HttpPost]
        public JsonResult SaveOrder(OrderVM O) {
            bool status = false;

            if (ModelState.IsValid) {
                using (MyDatabaseEntities dc = new MyDatabaseEntities()) {
                    Order order = new Order { OrderNo = O.OrderNo, OrderDate = O.OrderDate, Description = O.Description };
                    foreach (var i in O.OrderDetails) {
                        order.OrderDetails.Add(i);
                    }

                    dc.Orders.Add(order);
                    dc.SaveChanges();
                    status = true;
                }
            }

            return new JsonResult { Data = new { status = status } };
        }
    }
}
