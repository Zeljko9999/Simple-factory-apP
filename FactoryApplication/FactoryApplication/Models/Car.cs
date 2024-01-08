/*
 **********************************
 * Author: Željko Kalajžić
 * Project Task: Homework 3 - Cars factory
 **********************************
 * Description:
 *  
 * Defining class Car and it's properties: 
 *      - Id for unique identification
 *      - Manufacturer's name (e.g. Toyota, Ford, etc.)
 *      - Car model (e.g., Camry, Mustang, etc.)
 *      - Car price
 *      - Manufacturing date
 *      - Boolean that indicates whether the car is available for purchase
 *      - Number of cars in stock
 *      - List of car's features (e.g. aircondition, ABS, etc.)
 *
 **********************************
 */

namespace FactoryApplication.Models
{
    public class Car
    {
        public int Id { get; set; }
        public string? Manufacturer { get; set; }
        public string? Model { get; set; }
        public decimal? Price { get; set; }

        public DateTime ManufacturingDate { get; set; }
        public int QuantityInStock { get; set; }
        public bool IsAvailable { get; set; }
        public List<string> Features { get; set; }
    }
}
