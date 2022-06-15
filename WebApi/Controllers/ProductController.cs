using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product 
        public ActionResult Index(string searchString, int page = 1, int pageSize = 5)
        {
            IEnumerable<ProductViewModel> productList = new List<ProductViewModel>();
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Products").Result;
            productList = response.Content.ReadAsAsync<IEnumerable<ProductViewModel>>().Result;
            IQueryable<ProductViewModel> prodList = productList.AsQueryable();
            if (!string.IsNullOrEmpty(searchString))
            {
                productList = productList.Where(x => x.Name != null && x.Name.ToUpper().Contains(searchString.ToUpper()) || x.Code != null && x.Code.ToUpper().Contains(searchString.ToUpper()));
            }
            int totalProduct = productList.Count();
            float numberPage = (float)totalProduct / pageSize;
            ViewBag.pageCurrent = page;
            ViewBag.numberPage = (int)Math.Ceiling(numberPage);
            ViewBag.SearchString = searchString;
            int start = (page - 1) * pageSize;
            var model = productList.OrderByDescending(x => x.ID).Skip(start).Take(pageSize).ToList();
            return View(model);
        }

        [HttpGet]
        public ActionResult CreateNewProduct()
        {
            ProductViewModel cate = new ProductViewModel();
            return PartialView("CreateProduct", cate);
        }

        [HttpPost]
        public ActionResult CreateProduct(ProductViewModel data)
        {
            if (ModelState.IsValid)
            {
                IEnumerable<ProductViewModel> productList = new List<ProductViewModel>();
                HttpResponseMessage res = GlobalVariables.WebApiClient.GetAsync("Products").Result;
                productList = res.Content.ReadAsAsync<IEnumerable<ProductViewModel>>().Result;

                if (productList.Where(x => x.Name != null).Select(x => x.Name.ToLower()).Contains(data.Name.ToLower()))
                {
                    ModelState.AddModelError("", "Duplicate product name!");
                }
                else if (productList.Where(x => x.Code != null).Select(x => x.Code.ToLower()).Contains(data.Code.ToLower()))
                {
                    ModelState.AddModelError("", "Duplicate product code!");
                }
                else if (data.Price == 0)
                {
                    ModelState.AddModelError("", "Please enter the price!");
                }
                else if (data.Quantity == 0)
                {
                    ModelState.AddModelError("", "Please enter the quantity!");
                }
                else
                {
                    HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Products", data).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = "Product " + data.Name + " Created Successfully!";
                        return Json(true, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        TempData["SuccessMessage"] = "Product " + data.Name + " Created Failed!";
                        return Json(true, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return PartialView("CreateProduct", data);
        }

        [HttpGet]
        public ActionResult DetailProduct(int id)
        {
            ProductViewModel data = new ProductViewModel();
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Products/" + id).Result;
            data = response.Content.ReadAsAsync<ProductViewModel>().Result;
            return PartialView("DetailProduct", data);
        }

        [HttpGet]
        public ActionResult EditProduct(int id)
        {
            ProductViewModel data = new ProductViewModel();
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Products/" + id).Result;
            data = response.Content.ReadAsAsync<ProductViewModel>().Result;
            return PartialView("EditProduct", data);
        }

        [HttpPost]
        public ActionResult UpdateProduct(ProductViewModel data)
        {
            if (ModelState.IsValid)
            {
                IEnumerable<ProductViewModel> productList = new List<ProductViewModel>();
                HttpResponseMessage res = GlobalVariables.WebApiClient.GetAsync("Products").Result;
                productList = res.Content.ReadAsAsync<IEnumerable<ProductViewModel>>().Result;

                if (productList.Where(x => x.Name != null).Select(x => x.Name.ToLower()).Contains(data.Name.ToLower()))
                {
                    ModelState.AddModelError("", "Duplicate product name!");
                }
                else if (productList.Where(x => x.Code != null).Select(x => x.Code.ToLower()).Contains(data.Code.ToLower()))
                {
                    ModelState.AddModelError("", "Duplicate product code");
                }
                else if (data.Price == 0)
                {
                    ModelState.AddModelError("", "Please enter the price!");
                }
                else if (data.Quantity == 0)
                {
                    ModelState.AddModelError("", "Please enter the quantity!");
                }
                else
                {
                    HttpResponseMessage response = GlobalVariables.WebApiClient.PutAsJsonAsync("Products/" + data.ID, data).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = "Product " + data.Name + " Saved Successfully";
                        return Json(true, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Update product fail!");
                    }
                }
            }
            return PartialView("EditProduct", data);
        }

        [HttpPost]
        public JsonResult DeleteProduct(int id)
        {
            if (id != 0)
            {
                HttpResponseMessage response = GlobalVariables.WebApiClient.DeleteAsync("Products/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    return Json(new { status = true });
                }
                else
                {
                    return Json(new { status = false });
                }
            }
            return Json(new { status = false });
        }
    }
}