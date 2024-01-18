/*
 **********************************
 * Author: Željko Kalajžić
 * Project Task: Homework 3 - Cars factory - Interface Implementation
 **********************************
 * Description:
 *  Class that implements the `ICarRepository` interface
 *  Implement of 5 CRUD methods

 *
 **********************************
 */


// Implement the `ICarRepository` interface

using Microsoft.AspNetCore.Mvc;
using FactoryApplication.Models;
using FactoryApplication.Repositories;
using FactoryApplication.Repositories.Interfaces;

namespace FactoryApplication.Repositories
{
    public class CarRepository : ICarRepository
    {
        // A list of all cars
        private List<Car> m_lstCar;

        public CarRepository()
        {
            m_lstCar = new List<Car>();
        }


        // Create Operation: Create an car object

        public void CreateNewCar(Car car)
        {

            m_lstCar.Add(car);

        }


        // Read Operation 1 - Get all cars

        public List<Car> GetAllCars()
        {
            return m_lstCar;
        }


        // Read Operation 2 - Get the car with the specified ID

        public Car? GetCar(int id)
        {
            if (!m_lstCar.Any(car => car.Id == id))
            {
                return null;
            }

            var car = m_lstCar.FirstOrDefault(car => car.Id == id);

            return car;
        }


        // Update Operation - Update the car with the specified ID

        public void UpdateCar(int id, Car newCar)
        {
            if (!m_lstCar.Any(car => car.Id == id))
            {
                throw new KeyNotFoundException($"Car with ID '{id}' not found.");
            }

            var curCar = m_lstCar.FirstOrDefault(x => x.Id == id);

            curCar.Manufacturer = newCar.Manufacturer;
            curCar.Model = newCar.Model;
            curCar.Price = newCar.Price;
            curCar.ManufacturingDate = newCar.ManufacturingDate;
            curCar.IsAvailable = newCar.IsAvailable;
            curCar.QuantityInStock = newCar.QuantityInStock;
            curCar.Features = newCar.Features;
        }


        // Delete Operation - Delete the car with the specified ID

        public void DeleteCar(int id)
        {
            var itemToDelete = m_lstCar.FirstOrDefault(car => car.Id == id);
            if (itemToDelete == null)
            {
                throw new KeyNotFoundException($"Car with ID '{id}' not found.");
            }

            m_lstCar.Remove(itemToDelete);
        }

        public bool GetManufacturerAndModel(string manufacturer, string model)
        {
            return false;
        }

    }
}
