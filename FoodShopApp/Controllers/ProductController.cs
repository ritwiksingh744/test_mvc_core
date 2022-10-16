using FoodShopApp.Models;
using FoodShopApp.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FoodShopApp.Controllers
{
    public class ViewModel1
    {
        public IEnumerable<Category> Category { get; set; }
        public IQueryable<Product> Product { get; set; }
        public int totalPage { get; set; }
        public int pageSize { get; set; }
        public string nameSortParm { get; set; }
        public string expDateSortParm { get; set; }
        public string createDateSortParm { get; set; }
        public string currentFilter { get; set; }
        public string currentSortOrder { get; set; }
    }
    
    public class ProductController : Controller
    {
        private IGenericRepository<Category> _categoryRepository = null;
        private IGenericRepository<Product> _productRepository = null;

        public ProductController(IGenericRepository<Category> categoryRepository, IGenericRepository<Product> productRepository)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }

        [Authorize(Roles = ("Admin, User"))]
        public IActionResult Index(string sortOrder, string searchString, int? pageNo, int? pageSize)
        {
            var vm = new ViewModel1();
            int pgNo = (pageNo ?? 1);
            int pgSize = vm.pageSize = (pageSize ?? 5);
            IQueryable<Product> products = null;
            int? totalPage = null;
            vm.currentSortOrder = sortOrder;
            if (!String.IsNullOrEmpty(searchString))
            {
                vm.currentFilter = searchString;
                products = _productRepository.Search((x => x.IsDeleted == false && x.ProductName.ToLower().Contains(searchString.ToLower())));
                totalPage = (int)Math.Ceiling((decimal)products.Count() / pgSize);
            }
            else
            {
                products = _productRepository.Search(x => x.IsDeleted == false);
                totalPage = (int)Math.Ceiling((decimal)products.Count() / pgSize);
            }
            switch (sortOrder)
            {
                case "name_desc":
                    products = products.OrderByDescending(p => p.ProductName);
                    break;
                case "Date":
                    products = products.OrderBy(p => p.ExpiryDate);
                    break;
                case "date_desc":
                    products = products.OrderByDescending(s => s.ExpiryDate);
                    break;
                case "createDate":
                    products = products.OrderBy(s => s.CreatedOn);
                    break;
                case "Cdate_desc":
                    products = products.OrderByDescending(s => s.CreatedOn);
                    break;
                default:
                    products = products.OrderBy(s => s.ProductName);
                    break;
            }
            products = products.Skip((pgNo - 1) * pgSize).Take(pgSize);
            vm.Product = products;
            vm.totalPage = (int)totalPage;
            vm.nameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            vm.expDateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            vm.createDateSortParm = sortOrder == "createDate" ? "Cdate_desc" : "createDate";
            vm.Category = _categoryRepository.Search(x => x.IsDeleted == false);
            return View("Index", vm);
        }

        [Authorize(Roles = ("Admin, User"))]
        [HttpPost]
        public IActionResult Index(Product model)
        {
            if(model.ProductName != null)
            {
                _productRepository.Insert(model);
                _productRepository.Save();
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = ("Admin"))]
        public IActionResult Edit(int ProductId)
        {
            ViewData["Category"] = _categoryRepository.Search(x => x.IsDeleted == false);
            var product = _productRepository.GetById(ProductId);
            return View(product);
        }

        [Authorize(Roles = ("Admin"))]
        [HttpPost]
        public IActionResult Edit(Product model)
        {
            ViewData["Category"] = _categoryRepository.Search(x => x.IsDeleted == false);
            if (ModelState.IsValid)
            {
                model.UpdatedOn = DateTime.Now;
                _productRepository.Update(model);
                _productRepository.Save();
                return RedirectToAction("Index");
            }
            return View();
        }

        [Authorize(Roles = ("Admin"))]
        [HttpPost]
        public IActionResult Delete(Product model)
        {
            var product = _productRepository.GetById(model.ProductId);
            product.IsDeleted = true;
            _productRepository.Update(product);
            _productRepository.Save();
            return RedirectToAction("Index");
        }

    }
}
