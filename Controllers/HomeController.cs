using MVCMasterDetailsEntry.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Transactions;
using Newtonsoft.Json;

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
        public ActionResult GetOrders() {
            using (MyDatabaseEntities db = new MyDatabaseEntities()) {
                var result = db.Orders
                            .OrderBy(a => a.OrderID)
                            .Select(o => new
                            {
                                ID = o.OrderID,
                                No = o.OrderNo,
                                Date = o.OrderDate,
                                OrderDetails = o.OrderDetails.Select(d => new { itemName = d.ItemName, qty = d.Quantity, rate = d.Rate })
                            })
                            .ToList();
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }
        }

        public ActionResult Create() {
            return View();
        }

        #region 新增存檔[Post] +JsonResult SaveOrder(OrderVM O)
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
        #endregion
    }
}
