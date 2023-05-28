USE CarGalleryDB;
INSERT INTO Fuels (type)
	VALUES ('Diesel'), ('Gasoline'), ('Electric'), 
		('Hybrid'), ('Gasoline+CNG'), ('Gasoline+LPG'), 
		('Ethanol'), ('Hydrogen');

INSERT INTO Bodies (type)
	VALUES ('Hatchback'), ('Sedan'), ('SUV'), 
		('Coupe'), ('Convertible'), ('Wagon'), 
		('Van'), ('Jeep');

INSERT INTO Brands (name)
	VALUES ('Alfa Romeo'), ('Aston Martin'), ('Audi'), ('BMW'), 
		('Bentley'), ('Bugatti'), ('Cadillac'), ('Chevrolet'), 
		('Chrysler'), ('Citroen'), ('Dodge'), ('Ferrari'), 
		('Fiat'), ('Ford'), ('Honda'), ('Hyundai'), 
		('Infiniti'), ('Jaguar'), ('Jeep'), ('Kia'), 
		('Lamborghini'), ('Lexus'), ('Maserati'), ('Mazda'), 
		('Mercedes'), ('Mitsubishi'), ('Nissan'), ('Opel'), 
		('Peugeot'), ('Porsche'), ('Renault'), ('Skoda'), 
		('Seat'), ('Tesla'), ('Toyota'), ('Volkswagen'), ('Volvo');

INSERT INTO Roles (name)
	VALUES ('Admin'), ('User');