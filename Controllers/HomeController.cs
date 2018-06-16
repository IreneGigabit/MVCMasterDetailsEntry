using MVCMasterDetailsEntry.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Transactions;

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

        public ActionResult Create() {
            return View();
        }

        //Post action for Save data to database
        [HttpPost]
        public JsonResult SaveOrder(OrderVM O) {
            bool status = false;

            try {
                if (ModelState.IsValid) {
                    using (TransactionScope ts = new TransactionScope()) {
                        using (MyDatabaseEntities dc = new MyDatabaseEntities()) {
                            Order order = new Order { OrderNo = O.OrderNo, OrderDate = O.OrderDate, Description = O.Description };
                            dc.Orders.Add(order);
                            dc.SaveChanges();

                            //int a = 0;
                            //int total = 10 / a;
                            foreach (var i in O.OrderDetails) {
                                order.OrderDetails.Add(i);
                            }
                            dc.SaveChanges();

                            status = true;
                        }

                        ts.Complete();
                    }
                }

            }
            catch {
            }

            return new JsonResult { Data = new { status = status } };
        }
    }
}
