/*
 **********************************
 * Author: Željko Kalajžić
 * Project Task: Homework 3 - Cars factory - Controller Implementation (CRUD operations)
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
using FactoryApplication.Controllers.DTO;
using FactoryApplication.Logic;
using FactoryApplication.Filters;

namespace FactoryApplication.Controllers
{
    [ErrorFilter]
    [ApiController]
    [Route("api/[controller]")]
    public class CarController : ControllerBase
    {

        private readonly ICarLogic _carLogic;


        public CarController(ICarLogic carLogic)
        {
            this._carLogic = carLogic;
        }


        // Create an car object

        [HttpPost]
        public IActionResult Post([FromBody] NewCarDTo car)
        {

            if (car != null)
            {
                _carLogic.CreateNewCar(car.ToModel());
                return Ok("Car successfully created!");
            }
            else
            {
                return BadRequest("Error while creating an car!");
            }
        }


         // Read Operation 1 - Get all cars

        [HttpGet]
        public ActionResult<IEnumerable<CarInfoDTO>> Get()
        {
            var allCars = _carLogic.GetCars().Select(x => CarInfoDTO.FromModel(x));
            return Ok(allCars);
        }


        // Read Operation 2 - Get the car with the specified ID

        [HttpGet("{id}")]
        public ActionResult<CarInfoDTO> Get(int id)
        {
            var car = _carLogic.GetCar(id);

            if (car is null)
            {
                return NotFound($"Could not find an car with ID = {id}");
            }
            else
            {
                return Ok(CarInfoDTO.FromModel(car));
            }
        }


        // Update Operation - Update the car with the specified ID

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] NewCarDTo updatedCar)
        {

            if (updatedCar == null)
            {
                return BadRequest($"Error while updating an car!");
            }

            var existingCar = _carLogic.GetCar(id);
            if (existingCar == null)
            {
                return NotFound($"Could not find the car with ID = {id}");
            }

            _carLogic.UpdateCar(id, updatedCar.ToModel());

            return Ok($"Successfully updated the car with ID = {id}");

        }

        // Delete Operation - Delete the car with the specified ID

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var car = _carLogic.GetCar(id);
            if (car == null)
            {
                return NotFound($"Could not find an car with ID = {id}");
            }

            _carLogic.DeleteCar(id);
            return Ok($"Successfully deleted the car with ID = {id}");
        }
    }
}
