
using CMS.IRepository;
using CMS.Reporitory;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Web.Model;
using Web.Model.CustomModel;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Controllers
{
    public class CartController : Controller
    {
        private const string CartSession = "CartSession";
        private IProductRepository productRepository = new ProductRepository();
        private ICartRepository cartRepository = new CartRepository();
        // GET: Cart
        public ActionResult Index()
        {
            var cart = Session[CartSession];
            var lst = new List<CartItem>();
            if (cart != null)
            {
                lst = (List<CartItem>)Session[CartSession];
                Session["CoutCart"]= lst.Count;
            }else
            {
                Session["CoutCart"] = 0;
            }
            return View(lst);
        }
        public ActionResult AddProduct(int id,int quantity)
        {
            var product = productRepository.Find(id);
            var cart = Session[CartSession];
            if(cart != null)
            {
                var lst = (List<CartItem>)Session[CartSession];
                if(lst.Exists(x=>x.Product.ID == id))
                {
                    foreach (var item in lst)
                    {
                        if (item.Product == product)
                        {
                            item.Quantity += quantity;
                        }
                    }
                }else
                {
                    var item = new CartItem();
                    item.Product = product;
                    item.Quantity = quantity;
                    lst.Add(item);
                }
               
            }
            else
            {
                var item = new CartItem();
                item.Product = product;
                item.Quantity = quantity;
                var lst =new  List<CartItem>();
                lst.Add(item);
                Session[CartSession]= lst;
            }
            return RedirectToAction("Index");
        }
        public void EditProduct(string Products)
        {
            var lst = new JavaScriptSerializer().Deserialize<List<ProductCustom>>(Products);
            var lstCartItem = new List<CartItem>();
            foreach (var item in lst)
            {
                var product = new Product
                {
                    Images = item.Images,
                    Name = item.Name,
                    Price = item.Price
                };
                var quantity = item.Quantity;
                var cartItem = new CartItem
                {
                    Product = product,
                    Quantity = quantity
                };
                lstCartItem.Add(cartItem);
            }
            Session[CartSession] = lstCartItem;
        }
        public void DeleteSession()
        {
            Session.Remove(CartSession);
        }
        [HttpPost]
        public ActionResult Order(string customerOrder, string orderDetail)
        {
            var order = new JavaScriptSerializer().Deserialize<tbl_Order>(customerOrder);
            var details = new JavaScriptSerializer().Deserialize<List<OrderDetail>>(orderDetail);
            try
            {
                if (order.CustomerName.Length > 100)
                {
                    return Json(new
                    {
                        Success = false,
                        Message="Họ tên không hợp lệ"
                    }, JsonRequestBehavior.AllowGet);
                }
                if (order.CustomerAddress.Length > 250)
                {
                    return Json(new
                    {
                        Success = false,
                        Message ="Địa chỉ không hợp lệ"
                    }, JsonRequestBehavior.AllowGet);
                }
                if (order.CustomerPhone.Length > 20)
                {
                    return Json(new
                    {
                        Success = false,
                        Message ="Số điện thoại không hợp lệ"
                    }, JsonRequestBehavior.AllowGet);
                }
                if (order.CustomerEmail.Length > 250)
                {
                    return Json(new
                    {
                        Success = false,
                        Message ="Email không hợp lệ"
                    }, JsonRequestBehavior.AllowGet);
                }
                order.Status = 1;
                order.CreatedDate = DateTime.Now;
               int id = cartRepository.AddOrder(order);
                foreach (var item in details)
                {
                    item.OrderID = id;
                    cartRepository.AddOrderDetail(item);
                }
                Session.Clear();
                return Json(new
                {
                    Success = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    Success = false
                }, JsonRequestBehavior.AllowGet);
            }
           
        }
        public ActionResult CancelOrder()
        {
            Session.Clear();
            return Json(new
            {
                Success = true
            }, JsonRequestBehavior.AllowGet);
        }
        class ProductCustom:Product
        {
            public int Quantity { get; set; }
        }
    }
}