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
        private object m_oLock = new object();

        public CarRepository()
        {       
            m_lstCar = new List<Car>();
        }


        // Generate a random unique id for a new car

        private int GenerateUniqueId()
        {
            int generatedId;

            lock (m_oLock)
            {
                do
                {
                    generatedId = Random.Shared.Next(0, 10);
                }
                while (m_lstCar.Any(car => car.Id == generatedId));
            }

            return generatedId;
        }


        // Create Operation: Create an car object

        public bool CreateNewCar(Car car)
        {
            car.Id = GenerateUniqueId();

            lock (m_oLock)
            {
                m_lstCar.Add(car);
            }

            return true;
        }


        // Read Operation 1 - Get all cars

        public IEnumerable<Car> GetAllCars()
        {
            return m_lstCar;
        }


        // Read Operation 2 - Get the car with the specified ID

        public Car GetCar(int id)
        {
            if (!m_lstCar.Any(car => car.Id == id))
            {
                return null;
            }

            var car = m_lstCar.FirstOrDefault(car => car.Id == id);

            return car;
        }



        // Update Operation - Update the car with the specified ID

        public bool UpdateCar(int id, Car newCar)
        {
            if (!m_lstCar.Any(car => car.Id == id))
            {
                return false;
            }

            lock (m_oLock)
            {
                var curCar= m_lstCar.FirstOrDefault(x => x.Id == id);

                curCar.Manufacturer = newCar.Manufacturer;
                curCar.Model = newCar.Model;
                curCar.Price = newCar.Price;
                curCar.ManufacturingDate = newCar.ManufacturingDate;
                curCar.IsAvailable = newCar.IsAvailable;
                curCar.QuantityInStock = newCar.QuantityInStock;
                curCar.Features = newCar.Features;
            }

            return true;
        }


        // Delete Operation - Delete the car with the specified ID

        public bool DeleteCar(int id)
        {
            var itemToDelete = m_lstCar.FirstOrDefault(car => car.Id == id);
            if (itemToDelete == null)
            {
                return false;
            }

            lock (m_oLock)
            {
                m_lstCar.Remove(itemToDelete);
            }

            return true;
        }

    }
}
