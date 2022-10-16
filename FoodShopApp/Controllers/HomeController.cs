using FoodShopApp.Models;
using FoodShopApp.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FoodShopApp.Controllers
{
    public class ViewModel
    {
        public IQueryable<Category> Category { get; set; }
        public int totalPage { get; set; }
        public int pageSize { get; set; }
        public string nameSortParm { get; set; }
        public string dateSortParm { get; set; }
        public string currentFilter { get; set; }
        public string currentSortOrder { get; set; }
    }

    [Authorize]
    public class HomeController : Controller
    {
        private IGenericRepository<Category> _categoryRepository = null;
        private IGenericRepository<Product> _productRepository = null;

        public HomeController(IGenericRepository<Category> categoryRepository, IGenericRepository<Product> productRepository)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }

        public IActionResult Index(string sortOrder, string searchString, int? pageNo, int? pageSize)
        {
            var vm = new ViewModel();
            int pgNo = (pageNo ?? 1);
            int pgSize = vm.pageSize = (pageSize ?? 5);
            IQueryable<Category> categories = null;
            int? totalPage = null;
            vm.currentSortOrder = sortOrder;

            if (!String.IsNullOrEmpty(searchString))
            {
                vm.currentFilter = searchString;
                categories = _categoryRepository.Search((x => x.IsDeleted == false && x.CategoryName.ToLower().Contains(searchString.ToLower())));
                totalPage = (int)Math.Ceiling((decimal)categories.Count() / pgSize);
            }
            else
            {
                categories = _categoryRepository.Search(x => x.IsDeleted == false);
                totalPage = (int)Math.Ceiling((decimal)categories.Count() / pgSize);
            }
            switch (sortOrder)
            {
                case "name_desc":
                    categories = categories.OrderByDescending(s => s.CategoryName);
                    break;
                case "Date":
                    categories = categories.OrderBy(s => s.CreatedOn);
                    break;
                case "date_desc":
                    categories = categories.OrderByDescending(s => s.CreatedOn);
                    break;
                default:
                    categories = categories.OrderBy(s => s.CategoryName);
                    break;
            }
            categories = categories.Skip((pgNo - 1) * pgSize).Take(pgSize);

            vm.Category = categories;
            vm.totalPage = (int)totalPage;
            vm.nameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            vm.dateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            return View("Index", vm);
        }

        [Authorize(Roles = ("Admin"))]
        [HttpPost]
        public IActionResult Index(Category model)
        {
            if(model.CategoryName != null)
            {
                _categoryRepository.Insert(model);
                _categoryRepository.Save();
                
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles =("Admin"))]
        public IActionResult Edit(int CategoryId)
        {
            Category category = _categoryRepository.GetById(CategoryId);
            return View(category);
        }

        [Authorize(Roles = ("Admin"))]
        [HttpPost]
        public IActionResult Edit(Category model)
        {
            if (ModelState.IsValid)
            {
                model.UpdatedOn = DateTime.Now;
                _categoryRepository.Update(model);
                _categoryRepository.Save();
                return RedirectToAction("Index","Home");
            }
            return View();
        }

        public IActionResult Details(int CategoryId)
        {
            var category = _categoryRepository.GetById(CategoryId);
            ViewData["Product"] = _productRepository.Search(x => x.CategoryId == CategoryId);
            return View(category);
        }

        [Authorize(Roles = ("Admin"))]
        [HttpPost]
        public IActionResult Delete(int CategoryId)
        {
            var category = _categoryRepository.GetById(CategoryId);
            category.IsDeleted = true;
            _categoryRepository.Update(category);
            _categoryRepository.Save();
            var list = _productRepository.Search(x => x.IsDeleted == false && x.CategoryId == CategoryId);
            foreach (var p in list)
            {
                p.IsDeleted = true;
                _productRepository.Update(p);
                _productRepository.Save();
            }
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public IActionResult ShowUnAccessError()
        {
            return View();
        }


    }
}
