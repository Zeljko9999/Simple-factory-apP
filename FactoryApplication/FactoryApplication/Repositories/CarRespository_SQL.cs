using Microsoft.Data.Sqlite;
using FactoryApplication.Models;
using FactoryApplication.Repositories.Interfaces;
using System.Reflection.PortableExecutable;
using System.Net.Mail;

namespace FactoryApplication.Repositories
{
    public class CarRespository_SQL : ICarRepository
    {
        private readonly string _connectionString = "Data Source=C:\\Users\\zeljk\\Desktop\\DIS-labovi\\Zeljko9999-hw-03\\" +
            "FactoryApplication\\FactoryApplication\\SQL\\database.db";
        private readonly string _dbDateOnlyFormat = "yyyy-MM-dd";


        public int GetIdOfFeature(SqliteConnection connection, string? feature)
        {
            var command = connection.CreateCommand();
            command.CommandText = "SELECT ID FROM Features WHERE Name == $feature";
            command.Parameters.AddWithValue("$feature", feature);
            object? id = command.ExecuteScalar();

            return (Int32)(Int64)id;
        }

        public void CreateNewCar(Car car)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var transaction = connection.BeginTransaction();

            var commandInsertNewCar = connection.CreateCommand();
            commandInsertNewCar.CommandText =
            @"
                INSERT INTO Cars (Manufacturer, Model, Price, ManufacturingDate, QuantityInStock, IsAvailable)
                VALUES ($manufacturer, $model, $price, $manufacturingdate, $quantityinstock, $isavailable);

                SELECT last_insert_rowid();";

            commandInsertNewCar.Parameters.AddWithValue("$manufacturer", car.Manufacturer);
            commandInsertNewCar.Parameters.AddWithValue("$model", car.Model);
            commandInsertNewCar.Parameters.AddWithValue("$price", car.Price);
            commandInsertNewCar.Parameters.AddWithValue("$manufacturingdate", car.ManufacturingDate.ToString(_dbDateOnlyFormat));
            commandInsertNewCar.Parameters.AddWithValue("$quantityinstock", car.QuantityInStock);
            commandInsertNewCar.Parameters.AddWithValue("$isavailable", car.IsAvailable);

            // Take the ID of the last inserted row
            object? last_insert_rowid = commandInsertNewCar.ExecuteScalar();
            if (last_insert_rowid is null)
            {
                transaction.Rollback();
                throw new ArgumentException("Could not insert car into database.");
            }

            Int64 carId = (Int64)last_insert_rowid;

            var commandCarToFeature = connection.CreateCommand();
            commandCarToFeature.CommandText = "INSERT INTO Indexes(CarID, FeatureID) VALUES ($carId, $featureId)";
            commandCarToFeature.Parameters.AddWithValue("$carId", carId);
            commandCarToFeature.Parameters.AddWithValue("$featureId", null);
            try
            {
                foreach (var feature in car.Features)
                {
                    if (feature is not null)
                    {
                        int featureId = GetIdOfFeature(connection, feature);
                        commandCarToFeature.Parameters["$featureId"].Value = featureId;
                        _ = commandCarToFeature.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                transaction.Rollback();
            }

            transaction.Commit();
            connection.Close();
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
                else
                {
                    carFeaturesDictionary[carId].Add("");
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
                        ManufacturingDate = DateOnly.ParseExact(reader.GetString(4), _dbDateOnlyFormat, null),
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
                else
                {
                    featuresList.Add("");
                }


                if (featuresList.Count == 1)
                {

                    result = new Car
                    {
                        Id = reader.GetInt32(0),
                        Manufacturer = reader.GetString(1),
                        Model = reader.GetString(2),
                        Price = reader.GetInt32(3),
                        ManufacturingDate = DateOnly.ParseExact(reader.GetString(4), _dbDateOnlyFormat, null),
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
            command.Parameters.AddWithValue("$manufacturingdate", updatedCar.ManufacturingDate.ToString(_dbDateOnlyFormat));
            command.Parameters.AddWithValue("$quantityinstock", updatedCar.QuantityInStock);
            command.Parameters.AddWithValue("$isavailable", updatedCar.IsAvailable);

            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected < 1)
            {
                throw new ArgumentException($"Could not update car with ID = {id}.");
            }
        }

        // Can not post 2 same pairs of manufacturer & model
        public bool GetManufacturerAndModel(string manufacturer, string model)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
            @"
              SELECT COUNT(*) FROM Cars
              WHERE Manufacturer = $manufacturer AND Model = $model;";

            command.Parameters.AddWithValue("manufacturer", manufacturer);
            command.Parameters.AddWithValue("$model", model);

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                if (reader.GetInt32(0) > 0)
                {
                    return true;
                }
            }
            return false;           
        }
    }
}
