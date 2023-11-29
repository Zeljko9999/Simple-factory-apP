

using FactoryApplication.Models;

namespace FactoryApplication.Repositories.Interfaces
{
    public interface ICarRepository
    {
        public bool CreateNewCar(Car car);


        public IEnumerable<Car> GetAllCars();

 
        public Car GetCar(int id);


        public bool UpdateCar(int id, Car newCar);


        public bool DeleteCar(int id);
    }
}
