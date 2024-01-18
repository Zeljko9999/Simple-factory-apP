using FactoryApplication.Exceptions;
using FactoryApplication.Models;
using FactoryApplication.Repositories;
using FactoryApplication.Repositories.Interfaces;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;


namespace FactoryApplication.Logic
{
    public class CarLogic : ICarLogic
    {
        private readonly ICarRepository _carRepository;

        private const int _manufacturerMaxCharacters = 50;
        private const int _modelMaxCharacters = 40;

        public CarLogic(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        private void ValidateManufacturerField(string? manufacturer)
        {
            if (string.IsNullOrEmpty(manufacturer))
            {
                throw new LogicException("Manufacturer field cannot be empty.");
            }

            if (manufacturer.Length > _manufacturerMaxCharacters)
            {
                throw new LogicException($"Manufacturer field too long. Exceeded {_manufacturerMaxCharacters} characters.");
            }
        }

        private void ValidateModelField(string? model, string? manufacturer)
        {
            if (string.IsNullOrEmpty(model))
            {
                throw new LogicException("Model cannot be empty.");
            }

            if (model.Length > _modelMaxCharacters)
            {
                throw new LogicException($"Model field too long. Exceeded {_modelMaxCharacters} characters");
            }

            if (_carRepository.GetManufacturerAndModel(model, manufacturer))
            {
                throw new LogicException($"Model of same manufacturer already exist");
            }
        }

        private void ValidatePriceField(decimal? price)
        {
            if (price is null)
            {
                throw new LogicException("Price field cannot be empty.");
            }

            if (price < 0)
            {
                throw new LogicException($"Price can not be negative.");
            }
        }

        private void ValidateDManufacturingDateField(DateOnly? date)
        {
            if (date is null)
            {
                throw new LogicException("Manufacturing data field cannot be empty.");
            }
        
            DateOnly minDate = new DateOnly(1900, 1, 1);
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            if (date < minDate || date > today)
            {
                throw new LogicException("Wrong data. Data should be newer than 01.01.1900 and older than today.");
            }
        }

        private void ValidateQuantityInStockField(int? quantity)
        {
            if (quantity < 0)
            {
                throw new LogicException($"Quantity can not be negative.");
            }
        }

        public void CreateNewCar(Car? car)
        {

            // Sanitize inputs
            car.Id = -1;

            // Convert input string fields into first letter as upppercase and all other into lowercase
            if (!(string.IsNullOrEmpty(car.Manufacturer) && string.IsNullOrEmpty(car.Model)))
            {
                car.Manufacturer = char.ToUpper(car.Manufacturer[0]) + car.Manufacturer.Substring(1).ToLower();
                car.Model = char.ToUpper(car.Model[0]) + car.Model.Substring(1).ToLower();
            }

            ValidateManufacturerField(car.Manufacturer);
            ValidateModelField(car.Model, car.Manufacturer);
            ValidatePriceField(car.Price);
            ValidateDManufacturingDateField(car.ManufacturingDate);
            ValidateQuantityInStockField(car.QuantityInStock);

            _carRepository.CreateNewCar(car);
        }

        public void UpdateCar(int id, Car? car)
        {

            // Convert input string fields into first letter as upppercase and all other into lowercase
            if (!(string.IsNullOrEmpty(car.Manufacturer) && string.IsNullOrEmpty(car.Model)))
            {
                car.Manufacturer = char.ToUpper(car.Manufacturer[0]) + car.Manufacturer.Substring(1).ToLower();
                car.Model = char.ToUpper(car.Model[0]) + car.Model.Substring(1).ToLower();
            }

            car.Id = -1;

            ValidateManufacturerField(car.Manufacturer);
            ValidateModelField(car.Model, car.Manufacturer);
            ValidatePriceField(car.Price);
            ValidateDManufacturingDateField(car.ManufacturingDate);
            ValidateQuantityInStockField(car.QuantityInStock);

            _carRepository.UpdateCar(id, car);
        }

        public void DeleteCar(int id)
        {
            _carRepository.DeleteCar(id);
        }

        public Car? GetCar(int id)
        {
            return _carRepository.GetCar(id);
        }

        public IEnumerable<Car> GetCars()
        {
            return _carRepository.GetAllCars();
        }

    }
}
