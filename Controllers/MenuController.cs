using Microsoft.AspNetCore.Mvc;
using Sips.Interfaces;
using Sips.Repositories;
using Sips.SipsModels;
using Sips.ViewModels;
using Sips.Data;

namespace Sips.Controllers
{
    public class MenuController : Controller
    {
        private readonly SipsdatabaseContext _db;

        public MenuController(SipsdatabaseContext db)
        {
            _db = db;
        }

        public IActionResult MenuIndex()
        {
            return View();
        }

        public IActionResult MilkTea()
        {
            MenuRepo menuRepo = new MenuRepo(_db);

            var milkTeas = menuRepo.GetMilkTeas();
            var iceOptions = menuRepo.GetIceOptions();
            var sweetnessOptions = menuRepo.GetSweetnessOptions();
            var milkOptions = menuRepo.GetMilkOptions();
            var addInOptions = menuRepo.GetAddInOptions();

            var viewModel = new MenuItemVM
            {
                Title = "Milk Tea Menu",
                Items = milkTeas.ToList(),
                IceOptions = iceOptions.ToList(),
                SweetnessOptions = sweetnessOptions.ToList(),
                MilkOptions = milkOptions.ToList(),
                AddInOptions = addInOptions.ToList()
            };

            return View(viewModel);
        }

        public IActionResult FruitTea1()
        {
            MenuRepo menuRepo = new MenuRepo(_db);

            var fruitTeas = menuRepo.GetFruitTeas();
            var iceOptions = menuRepo.GetIceOptions();
            var sweetnessOptions = menuRepo.GetSweetnessOptions();
            var milkOptions = menuRepo.GetMilkOptions();
            var addInOptions = menuRepo.GetAddInOptions();

            var viewModel = new MenuItemVM
            {
                Title = "Fruit Tea Menu",
                Items = fruitTeas.ToList(),
                IceOptions = iceOptions.ToList(),
                SweetnessOptions = sweetnessOptions.ToList(),
                MilkOptions = milkOptions.ToList(),
                AddInOptions = addInOptions.ToList()
            };
            return View(viewModel);
        }

        public IActionResult Slush()
        {
            MenuRepo menuRepo = new MenuRepo(_db);

            var slushes = menuRepo.GetSlushes();
            var iceOptions = menuRepo.GetIceOptions();
            var sweetnessOptions = menuRepo.GetSweetnessOptions();
            var milkOptions = menuRepo.GetMilkOptions();
            var addInOptions = menuRepo.GetAddInOptions();

            var viewModel = new MenuItemVM
            {
                Title = "Milk Tea Menu",
                Items = slushes.ToList(),
                IceOptions = iceOptions.ToList(),
                SweetnessOptions = sweetnessOptions.ToList(),
                MilkOptions = milkOptions.ToList(),
                AddInOptions = addInOptions.ToList()
            };
            return View(viewModel);
        }

    }
}
