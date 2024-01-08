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
using FactoryApplication.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FactoryApplication.Controllers
{

    [ApiController]
    public class CarController : ControllerBase
    {

        private readonly ICarRepository carRepository;


        public CarController(ICarRepository carRepository)
        {
            this.carRepository = carRepository;
        }


        // Create an car object

        [HttpPost("/cars/new")]
        public IActionResult CreateNewCar([FromBody] Car car)
        {

            if (car != null)
            {
                carRepository.CreateNewCar(car);
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
            return Ok(carRepository.GetAllCars());
        }


        // Read Operation 2 - Get the car with the specified ID

        [HttpGet("/cars/{id}")]
        public IActionResult GetCarById([FromRoute] int id)
        {
            var car = carRepository.GetCar(id);

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

        [HttpPut("/cars/{id}")]
        public IActionResult UpdateCarById([FromRoute] int id, [FromBody] Car updatedCar)
        {

            if (updatedCar == null)
            {
                return BadRequest($"Error while updating an car!");
            }

            var existingCar = carRepository.GetCar(id);
            if (existingCar == null)
            {
                return NotFound($"Could not find the car with ID = {id}");
            }

            carRepository.UpdateCar(id, updatedCar);

            return Ok($"Succesfully updated the car with ID = {id}");

        }

// Delete Operation - Delete the car with the specified ID

[HttpDelete("/cars/{id}")]
        public IActionResult DeleteCarById([FromRoute] int id)
        {
            var car = carRepository.GetCar(id);
            if (car == null)
            {
                return NotFound($"Could not find an car with ID = {id}");
            }

            carRepository.DeleteCar(id);
            return Ok($"Succesfully deleted the car with ID = {id}");

        }
    }
}
