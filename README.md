# Car Gallery Application Documentation

Welcome to the **Car Gallery** application documentation. 
It is a full stack application with Angular frontend, ASP.NET backend and Microsoft SQL Server database.

## Table of Contents

- [Technologies used](#technologies-used)
- [Getting Started](#getting-started)
- [Overview](#overview)
- [Features](#features)

## Technologies used

Frontend
- Angular v16
- Bootstrap v5.3.0
- Typescript v5.1.6
- https://fontawesome.com/

Backend
- ASP.NET Core
- .NET 6.0
- Entity Framework Core
- Excel Data Reader
- Swagger

Database
- Microsoft SQL Server

## Getting Started

1. Check if you have installed **Node**, **Angular CLI**, **.NET CLI or IDE**, **Microsoft SQL Server**

2. Clone this repository to your local machine using:
- git clone https://github.com/Noppe420/CarGallery

3. **Backend Setup**:
- Navigate to the `Backend\CarGalleryAPI` folder and open the Visual Studio project.
- Configure the MSSQL database connection in the `appsettings.json` file and `Data\DbCreator.cs` (if necessary) default server is `localhost` with windows authentication.
- Run the backend project to start the API server (the database will be created on startup).

4. **Frontend Setup**:
- Navigate to the `Frontend\CarGallery` folder.
- Install the required npm packages using `npm install`.
- Update the API endpoint URL in the frontend code to match your backend server in the `Frontend\CarGallery\src\app\environment.ts`.
- Run the frontend project using `ng serve`.

The default admin account has both username and password: admin

## Overview

There are 4 main pages:
- **Log in/Sign up**: Displays login/signup form and an error message when it occurs.
- **Home**: Displays cars uploaded by the current user, allowing editing and deletion of them. You can also add cars on this page.
- **All**: Shows all cars uploaded by all users, with an option to filter by brand.
- **Panel**: Accessible only to administrators, this section lets them manage car brands, body types, and users.

## Features

- Creating a database from a .sql file with initialization of basic data imported from an .xlsx file in .NET at first startup.
- Users authorization (hashing passowords, session storage).
- Uploading images to the server and than displaying them on the client side.
- Add and edit buttons open a popup form.
- Clicking on a car's image reveals additional information.
- Filter select shows only brands that are used.
- The **panel** is visible in the navigation bar only to users with administrator privileges.

---