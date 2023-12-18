SELECT Manufacturer, Model, Price, ManufacturingDate, QuantityInStock 
FROM Cars
WHERE Price > 20000 AND Manufacturer LIKE 'B%';
