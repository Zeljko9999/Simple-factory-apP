using FactoryApplication.Models;

namespace FactoryApplication.Logic
{
    public interface ICarLogic
    {
        void CreateNewCar(Car? car);
        void DeleteCar(int id);
        Car? GetCar(int id);
        IEnumerable<Car> GetCars();
        void UpdateCar(int id, Car? car);
    }
}
