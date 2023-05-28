
CREATE DATABASE CarGalleryDB;
USE CarGalleryDB;

CREATE TABLE Users (
  id UNIQUEIDENTIFIER PRIMARY KEY,
  role_id INT,
  username VARCHAR(50),
  email VARCHAR(100),
  password VARCHAR(100)
);

CREATE TABLE Roles (
	id INT IDENTITY(1, 1) PRIMARY KEY,
	name VARCHAR(50)
);

CREATE TABLE Fuels (
    id INT IDENTITY(1, 1) PRIMARY KEY,
	type VARCHAR(50)
);

CREATE TABLE Bodies (
    id INT IDENTITY(1, 1) PRIMARY KEY,
	type VARCHAR(50)
);

CREATE TABLE Brands (
    id INT IDENTITY(1, 1) PRIMARY KEY,
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
	imagePath VARCHAR(50)
);
