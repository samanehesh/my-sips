using Microsoft.EntityFrameworkCore;
using Sips.Data;
using Sips.SipsModels;
using Sips.ViewModels;

namespace Sips.Repositories
{
    public class MenuRepo
    {
        private readonly SipsdatabaseContext _db;



        public MenuRepo(SipsdatabaseContext db)
        {
            _db = db;
        }

        public IEnumerable<Item> GetMilkTeas()
        {
            List<Item> items = _db.Items.Where(item => item.ItemTypeId == 1).Include(p => p.Image).ToList();

            return items;
        }

        public IEnumerable<Item> GetFruitTeas()
        {
            List<Item> items = _db.Items.Where(item => item.ItemTypeId == 2).Include(p => p.Image).ToList();

            return items;
        }

        public IEnumerable<Item> GetSlushes()
        {
            List<Item> items = _db.Items.Where(item => item.ItemTypeId == 3).Include(p => p.Image).ToList();

            return items;
        }

        public IEnumerable<Ice> GetIceOptions()
        {
            return _db.Ices.ToList();
        }

        public IEnumerable<Sweetness> GetSweetnessOptions()
        {
            return _db.Sweetnesses.ToList();
        }

        public IEnumerable<MilkChoice> GetMilkOptions()
        {
            return _db.MilkChoices.ToList();
        }

        public IEnumerable<AddIn> GetAddInOptions()
        {
            return _db.AddIns.ToList();
        }
    }
}
