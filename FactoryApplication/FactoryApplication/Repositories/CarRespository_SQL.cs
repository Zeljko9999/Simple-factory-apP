using Microsoft.Data.Sqlite;
using FactoryApplication.Models;
using FactoryApplication.Repositories.Interfaces;

namespace FactoryApplication.Repositories
{
    public class CarRespository_SQL : ICarRepository
    {
        private readonly string _connectionString = "Data Source=C:\\Users\\zeljk\\Desktop\\DIS-labovi\\Zeljko9999-hw-03\\" +
            "FactoryApplication\\FactoryApplication\\SQL\\database.db";
        private readonly string _dbDatetimeFormat = "yyyy-MM-dd hh:mm:ss.fff";
        public void CreateNewCar(Car car)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
            @"
                INSERT INTO Cars (Manufacturer, Model, Price, ManufacturingDate, QuantityInStock, IsAvailable)
                VALUES ($manufacturer, $model, $price, $manufacturingdate, $quantityinstock, $isavailable)";

            command.Parameters.AddWithValue("$manufacturer", car.Manufacturer);
            command.Parameters.AddWithValue("$model", car.Model);
            command.Parameters.AddWithValue("$price", car.Price);
            command.Parameters.AddWithValue("$manufacturingdate", car.ManufacturingDate.ToString(_dbDatetimeFormat));
            command.Parameters.AddWithValue("$quantityinstock", car.QuantityInStock);
            command.Parameters.AddWithValue("$isavailable", car.IsAvailable);
            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected < 1)
            {
                throw new ArgumentException("Could not insert car into database.");
            }
        }

        public void DeleteCar(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
            @"
                DELETE FROM Cars
                WHERE ID == $id";

            command.Parameters.AddWithValue("$id", id);

            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected < 1)
            {
                throw new ArgumentException($"No cars with ID = {id}.");
            }
        }

        public List<Car> GetAllCars()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
               @"SELECT Cars.ID, Cars.Manufacturer, Cars.Model, Cars.Price, 
                 Cars.ManufacturingDate, Cars.QuantityInStock, Cars.IsAvailable, Features.Name
                 FROM Cars
                 LEFT JOIN Indexes ON Cars.ID = Indexes.CarID
                 LEFT JOIN Features ON Indexes.FeatureID = Features.ID";

            using var reader = command.ExecuteReader();

            var carFeaturesDictionary = new Dictionary<int, List<string>>(); // Store features for each car
            var results = new List<Car>();

            while (reader.Read())
            {
                var carId = reader.GetInt32(0);

                if (!carFeaturesDictionary.ContainsKey(carId))
                {
                    carFeaturesDictionary[carId] = new List<string>();
                }

                // Add the feature to the list associated with the car ID
                var feature = reader.IsDBNull(7) ? null : reader.GetString(7);
                if (!string.IsNullOrEmpty(feature))
                {
                    carFeaturesDictionary[carId].Add(feature);
                }

                // Read car information only once
                if (carFeaturesDictionary[carId].Count == 1)
                {
                    var car = new Car
                    {
                        Id = carId,
                        Manufacturer = reader.GetString(1),
                        Model = reader.GetString(2),
                        Price = reader.GetInt32(3),
                        ManufacturingDate = DateTime.ParseExact(reader.GetString(4), _dbDatetimeFormat, null),
                        QuantityInStock = reader.GetInt32(5),
                        IsAvailable = reader.GetInt32(6) != 0,
                        Features = carFeaturesDictionary[carId]
                    };

                    results.Add(car);
                }
            }

            return results;
        }


        public Car? GetCar(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
              @"SELECT Cars.ID, Cars.Manufacturer, Cars.Model, Cars.Price, 
                 Cars.ManufacturingDate, Cars.QuantityInStock, Cars.IsAvailable, Features.Name
                 FROM Cars
                 LEFT JOIN Indexes ON Cars.ID = Indexes.CarID
                 LEFT JOIN Features ON Indexes.FeatureID = Features.ID
                 WHERE Cars.ID == $id";

            command.Parameters.AddWithValue("$id", id);

            var featuresList = new List<string>();
            using var reader = command.ExecuteReader();

            Car result = null;

            while (reader.Read())
            {
                var feature = reader.IsDBNull(7) ? null : reader.GetString(7);
                if (!string.IsNullOrEmpty(feature))
                {
                    featuresList.Add(feature);
                }

                if (featuresList.Count == 1)
                {

                    result = new Car
                    {
                        Id = reader.GetInt32(0),
                        Manufacturer = reader.GetString(1),
                        Model = reader.GetString(2),
                        Price = reader.GetInt32(3),
                        ManufacturingDate = DateTime.ParseExact(reader.GetString(4), _dbDatetimeFormat, null),
                        QuantityInStock = reader.GetInt32(5),
                        IsAvailable = reader.GetInt32(6) != 0,
                        Features = featuresList
                    };
                }
            }

            return result;
        }

        public void UpdateCar(int id, Car updatedCar)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
            @"
                UPDATE Cars
                SET
                    Manufacturer = $manufacturer,
                    Model = $model,
                    Price = $price,
                    ManufacturingDate = $manufacturingdate,
                    QuantityInStock = $quantityinstock,
                    IsAvailable = $isavailable
                WHERE
                    ID == $id";

            command.Parameters.AddWithValue("$id", id);
            command.Parameters.AddWithValue("$manufacturer", updatedCar.Manufacturer);
            command.Parameters.AddWithValue("$model", updatedCar.Model);
            command.Parameters.AddWithValue("$price", updatedCar.Price);
            command.Parameters.AddWithValue("$manufacturingdate", updatedCar.ManufacturingDate.ToString(_dbDatetimeFormat));
            command.Parameters.AddWithValue("$quantityinstock", updatedCar.QuantityInStock);
            command.Parameters.AddWithValue("$isavailable", updatedCar.IsAvailable);

            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected < 1)
            {
                throw new ArgumentException($"Could not update car with ID = {id}.");
            }
        }
    }
}
