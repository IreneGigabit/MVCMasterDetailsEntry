using MVCMasterDetailsEntry.Models;
using System.Collections.Generic;
using System.Linq;
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

        #region 取得所有訂單資料 +ActionResult GetOrders()
        public ActionResult GetOrders() {
            using (MyDatabaseEntities db = new MyDatabaseEntities()) {
                //使用SQO
                var result = db.Orders
                            .OrderBy(a => a.OrderID)
                            .Select(o => new
                            {
                                ID = o.OrderID,
                                No = o.OrderNo,
                                Date = o.OrderDate,
                                Description = o.Description,
                                OrderDetails = o.OrderDetails.Select(d => new { itemName = d.ItemName, qty = d.Quantity, rate = d.Rate, TotalAmount = d.TotalAmount })
                            })
                            .ToList();
                //使用Linq
                /*r result = (from g in db.Orders
                              let details = g.OrderDetails.Select(d => new { ItemName = d.ItemName, Quantity = d.Quantity, Rate = d.Rate, TotalAmount = d.TotalAmount })
                              orderby g.OrderID
                              select new { g.OrderID, g.OrderNo, g.OrderDate, g.Description, details }
                            ).ToList();*/

                //使用Linq(對應viewModel)
                /*var result = (from g in db.Orders
                              //let details = g.OrderDetails.Select(d => new { ItemName = d.ItemName, Quantity = d.Quantity, Rate = d.Rate, TotalAmount = d.TotalAmount })
                              orderby g.OrderID
                              select new { g.OrderNo, g.OrderDate, g.Description, g.OrderDetails }
                              ).ToList()
                              .Select(c => new OrderVM
                              {
                                  OrderNo = c.OrderNo,
                                  OrderDate = c.OrderDate,
                                  Description = c.Description,
                                  OrderDetails = c.OrderDetails.Select(d => new OrderDetail
                                  {
                                      OrderItemsID = d.OrderItemsID,
                                      OrderID = d.OrderID,
                                      ItemName = d.ItemName,
                                      Quantity = d.Quantity,
                                      Rate = d.Rate,
                                      TotalAmount = d.TotalAmount
                                  }).ToList()
                              });*/
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }
        }
        #endregion

        #region 編輯畫面 +ActionResult Modify(int id)
        public ActionResult Modify(int id) {
            using (MyDatabaseEntities db = new MyDatabaseEntities()) {
                Order order = db.Orders.Where(o => o.OrderID == id).FirstOrDefault();
                //Order order = (from a in db.Orders where a.OrderID == id select a).FirstOrDefault();

                //使用SQO
                //var result = db.Orders
                //            .OrderBy(a => a.OrderID)
                //            .Select(o => new
                //            {
                //                ID = o.OrderID,
                //                No = o.OrderNo,
                //                Date = o.OrderDate,
                //                Description = o.Description,
                //                OrderDetails = o.OrderDetails.Select(d => new { orderItemID=d.OrderItemsID, itemName = d.ItemName, qty = d.Quantity, rate = d.Rate, TotalAmount = d.TotalAmount })
                //            })
                //            .ToList();
                return View(order);
            }
        }
        #endregion

        #region 新增畫面 +ActionResult Creat()
        public ActionResult Create() {
            return View();
        }
        #endregion

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
