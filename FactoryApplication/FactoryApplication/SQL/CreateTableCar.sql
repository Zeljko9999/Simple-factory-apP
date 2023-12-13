CREATE TABLE "Cars" (
	"ID"	INTEGER NOT NULL UNIQUE,
	"Manufacturer"	TEXT,
	"Model"	TEXT,
	"Price"	REAL,
	"ManufacturingDate"	TEXT,
	"QuantityInStock"	INTEGER,
	PRIMARY KEY("ID" AUTOINCREMENT)
);