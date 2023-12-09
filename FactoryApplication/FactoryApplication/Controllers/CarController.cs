/*
 **********************************
 * Author: Željko Kalajžić
 * Project Task: Homework 3 - Cars factory - Controller Implementation (CRUD operations)
 **********************************
 * Description:
 *  A program that demonstrates a simple CRUD interface.
 *  The program stores a list of cars and provides the user
    with an API to execute CRUD operations:
     - Create - Create a new Car model and insert it into storage
     - Read All - Get a collection of all Car models in storage
     - Read Specific - Get a Car model from storage by providing a unique identifier to the API
     - Update - Apply changes to a specific Car in storage, identified by a unique value
     - Delete - Delete a specific Car from storage, identified by a unique value
 *
 **********************************
 */

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FactoryApplication.Models;
using FactoryApplication.Repositories;

namespace FactoryApplication.Controllers
{

    [ApiController]
    public class CarController : ControllerBase
    {

        private readonly CarRepository m_carRepository;


        public CarController(CarRepository carRepository)
        {
            m_carRepository = carRepository;
        }


        // Create an car object

        [HttpPost("/cars/new")]
        public IActionResult CreateNewCar([FromBody] Car car)
        {
            bool fSuccess = m_carRepository.CreateNewCar(car);

            if (fSuccess)
            {
                return Ok("Car succesfully created!");
            }
            else
            {
                return BadRequest("Error while creating an car!");
            }
        }


         // Read Operation 1 - Get all cars

        [HttpGet("/cars/all")]
        public IActionResult GetAllCars()
        {
            return Ok(m_carRepository.GetAllCars());
        }


        // Read Operation 2 - Get the car with the specified ID

        [HttpGet("/cars/{id}")]
        public IActionResult GetCarById([FromRoute] int id)
        {
            var car = m_carRepository.GetCar(id);

            if (car is null)
            {
                return NotFound($"Could not find an car with ID = {id}");
            }
            else
            {
                return Ok(car);
            }
        }

        
        // Update Operation - Update the car with the specified ID

        [HttpPost("/cars/{id}")]
        public IActionResult UpdateCarById([FromRoute] int id, [FromBody] Car newCar)
        {
            if (m_carRepository.UpdateCar(id, newCar))
            {
                return Ok($"Succesfully updated the car with ID = {id}");
            }
            else
            {
                return NotFound($"Could not find the car with ID = {id}");
            }
        }


        // Delete Operation - Delete the car with the specified ID

        [HttpDelete("/cars/{id}")]
        public IActionResult DeleteCarById([FromRoute] int id)
        {
            if (m_carRepository.DeleteCar(id))
            {
                return Ok($"Succesfully deleted the car with ID = {id}");
            }
            else
            {
                return NotFound($"Could not find an car with ID = {id}");
            }
        }

    }
}
