USE CarGalleryDB;

CREATE TABLE Roles (
	id INT PRIMARY KEY,
	name VARCHAR(50)
);

CREATE TABLE Users (
  id UNIQUEIDENTIFIER PRIMARY KEY,
  role_id INT,
  username VARCHAR(50),
  email VARCHAR(100),
  password VARCHAR(100)

  CONSTRAINT FK_Users_Roles FOREIGN KEY (role_id)
        REFERENCES Roles (id)
);

CREATE TABLE Fuels (
    id INT PRIMARY KEY,
	type VARCHAR(50)
);

CREATE TABLE Bodies (
    id INT PRIMARY KEY,
	type VARCHAR(50)
);

CREATE TABLE Brands (
    id INT PRIMARY KEY,
	name VARCHAR(50)
);

CREATE TABLE Cars (
	id UNIQUEIDENTIFIER PRIMARY KEY,
	user_id UNIQUEIDENTIFIER,
	fuel_id INT,
	body_id INT,
	brand_id INT,
	model VARCHAR(50),
	productionYear INT,
	engine INT,
	horsePower INT,
	imagePath VARCHAR(250)

	CONSTRAINT FK_Cars_Users FOREIGN KEY (user_id)
        REFERENCES Users (id),
    CONSTRAINT FK_Cars_Fuels FOREIGN KEY (fuel_id)
        REFERENCES Fuels (id),
    CONSTRAINT FK_Cars_Bodies FOREIGN KEY (body_id)
        REFERENCES Bodies (id),
    CONSTRAINT FK_Cars_Brands FOREIGN KEY (brand_id)
        REFERENCES Brands (id)
);
