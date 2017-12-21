using CheeseMVC.Data;
using CheeseMVC.Models;
using CheeseMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CheeseMVC.Controllers
{
    public class MenuController : Controller
    {
        private readonly CheeseDbContext context;

        public MenuController(CheeseDbContext dbContext)
        {
            context = dbContext;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            List<Menu> menus = context.Menus.ToList();
            return View(menus);
        }

        public IActionResult Add()
        {
            AddMenuViewModel addMenuViewModel = new AddMenuViewModel();
            return View(addMenuViewModel); 
        }

        [HttpPost]
        public IActionResult Add(AddMenuViewModel addMenuViewModel)
        {
            if (ModelState.IsValid)
            {
                Menu newMenu = new Menu
                {
                    Name = addMenuViewModel.Name,

                };

                string menuName = addMenuViewModel.Name;

                context.Menus.Add(newMenu);
                context.SaveChanges();

                return Redirect("/Menu/ViewMenu/" + newMenu.ID);
            }
            return View(addMenuViewModel);
        }

        [HttpGet]
        public IActionResult ViewMenu(int id)
        {
            List<CheeseMenu> items = context
                .CheeseMenus
                .Include(item => item.Cheese)
                .Where(CheeseMenu => CheeseMenu.MenuID == id)
                .ToList();
            Menu theMenu = context.Menus.Single(c => c.ID == id);

            ViewMenuViewModel viewMenuViewModel = new ViewMenuViewModel()
            {
                Items = items,
                Menu = theMenu
            };

            return View(viewMenuViewModel);
        }

        [HttpGet]
        public IActionResult AddItem(int id)
        {
            Menu theMenu = context.Menus.Single(m => m.ID == id);
            List<Cheese> cheeses = context.Cheeses.ToList();

            return View(new AddMenuItemViewModel(theMenu,cheeses)); 
        }
    }
}
