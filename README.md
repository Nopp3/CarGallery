# Car Gallery Application Documentation

Welcome to the **Car Gallery**, a dynamic full-stack application showcasing a seamless fusion of cutting-edge technologies. This project combines the elegance of an Angular frontend, the robustness of an ASP.NET backend, and the scalability of a Microsoft SQL Server database.

## Table of Contents

- [Description](#description)
- [Technologies used](#technologies-used)
- [Getting Started](#getting-started)
- [Overview](#overview)
- [Features](#features)

## Description

The Car Gallery is a web application designed for car enthusiasts to showcase and share their beloved vehicles. Users have the ability to upload photos of their cars along with essential parameters such as engine capacity, model, and brand. The application provides a platform for users to boast about their automotive pride.

## Technologies used

Frontend
- Angular v16
- Bootstrap v5.3.0
- Typescript v5.1.6
- [Font Awesome](https://fontawesome.com/)

Backend
- ASP.NET Core
- .NET 6.0
- Entity Framework Core
- Excel Data Reader

Database
- Microsoft SQL Server

## Getting Started

1. Check if you have installed **Node**, **Angular CLI**, **.NET CLI or IDE**, **Microsoft SQL Server**

2. Clone this repository to your local machine using:
```bash
git clone https://github.com/Noppe420/CarGallery
```
3. **Backend Setup**:
- Navigate to the `Backend\CarGalleryAPI` folder and open the Visual Studio project.
- Configure the MSSQL database connection in the `appsettings.json` (if necessary) default server is `localhost` with windows authentication.
- Run the backend project to start the API server (the database will be created on startup).

4. **Frontend Setup**:
- Navigate to the `Frontend\CarGallery` folder.
- Install the required npm packages using `npm install`.
- Update the API endpoint URL in the frontend code to match your backend server in the `Frontend\CarGallery\src\app\environment.ts`.
- Run the frontend project using `ng serve`.

The default admin account has both username and password: admin

## Overview

There are 4 main pages:
- **Log in/Sign up**: This page presents a user-friendly login/signup form. It also displays an error message when the form is incorrectly completed.
- **Home**: Users can view and manage their uploaded cars on this page. It allows for the editing and deletion of cars, as well as the addition of new ones.
- **All**: Explore a comprehensive display of all cars uploaded by users. The page includes a convenient filter option to sort cars by brand.
- **Panel (Administrators Only)**: Exclusively accessible to administrators, this section facilitates the management of car brands, body types, and user profiles. Administrators can efficiently maintain the integrity of the application's data.

<!-- TO BE DONE -->

## Features

### Database Initialization:
- The application's backend features automatic database initialization upon the first run along with importing basic data from .xlsx file.

### User Profiles:
- Users can create accounts, providing a personalized space to manage and showcase their car collection.
- Their passwords are stored securely thanks to hashing.

### Car Upload and Management:
- Upload photos of cars, including relevant parameters, to the server.
- Edit or delete cars from the user's collection.
- User-friendly pop-up form for editing and adding cars

### Administrative Panel:
- Administrators have a dedicated panel to manage brands and car body types in the database.
- Brands and bodies are maintained to ensure data integrity, preventing users from adding non-existent brands.
- The panel is visible only to administrators

### Car Exploration:
- Clicking on a car unveils supplementary information.
- Brand filtering is available (only if there is a car from that brand).

---