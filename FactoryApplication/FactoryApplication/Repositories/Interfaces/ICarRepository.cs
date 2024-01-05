

using FactoryApplication.Models;

namespace FactoryApplication.Repositories.Interfaces
{
    public interface ICarRepository
    {
        void CreateNewCar(Car car);


        List<Car> GetAllCars();

 
        Car? GetCar(int id);


        void UpdateCar(int id, Car updatedCar);


        void DeleteCar(int id);
    }
}
