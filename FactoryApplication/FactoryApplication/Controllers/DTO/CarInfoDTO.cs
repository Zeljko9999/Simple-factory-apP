using FactoryApplication.Models;

namespace FactoryApplication.Controllers.DTO
{
    public class CarInfoDTO
    {
        public int ID { get; set; }
        public string? Manufacturer { get; set; }
        public string? Model { get; set; }
        public decimal? Price { get; set; }

        public string? ManufacturingDate { get; set; }
        public int? QuantityInStock { get; set; }
        public string? IsAvailable { get; set; }
        public List<string>? Features { get; set; }

        public static CarInfoDTO FromModel(Car model)
        {
            return new CarInfoDTO
            {
                ID = model.Id,
                Manufacturer = model.Manufacturer,
                Model = model.Model,
                Price = model.Price,
                ManufacturingDate = model.ManufacturingDate.ToLongDateString(),
                QuantityInStock = model.QuantityInStock,
                IsAvailable = (model.IsAvailable == true) ? "Yes" : "No",
                Features = model.Features
            };
        }
    }

}
