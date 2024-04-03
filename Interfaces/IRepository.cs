using Sips.SipsModels;

namespace Sips.Interfaces
{
    public interface IRepository
    {
        IEnumerable<Item> GetMilkTeas();
        IEnumerable<Item> GetFruitTeas();
        IEnumerable<Item> GetSlushes();
    }
}
