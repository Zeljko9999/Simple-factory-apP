using FactoryApplication.Models;
using Microsoft.AspNetCore.Http;


namespace FactoryApplication.Controllers.DTO
{
    public class NewCarDTo
    {
        public string? Manufacturer { get; set; }
        public string? Model { get; set; }
        public decimal? Price { get; set; }

        public string? ManufacturingDate { get; set; }
        public int? QuantityInStock { get; set; }
        public string? IsAvailable { get; set; }
        public List<string>? Features { get; set; }

        public Car ToModel()
        {
            return new Car
            {
                Id = -1,
                Manufacturer = Manufacturer,
                Model = Model,
                Price = Price,
                ManufacturingDate = DateOnly.ParseExact(ManufacturingDate, "yyyy-MM-dd", null),
                QuantityInStock = QuantityInStock,
                IsAvailable = (IsAvailable == "Yes") ? true : false,
                Features = Features
            };
        }
    }
}
