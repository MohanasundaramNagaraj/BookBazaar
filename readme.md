# BookBazaar

BookBazaar is a web application designed for managing book sales. It features an admin panel for managing books, authors, 
categories, companies, covers, and orders, as well as a customer interface for viewing books, managing the shopping cart
and placing orders.

## Table of Contents

- [Features](#features)
- [Installation](#installation)
- [Usage](#usage)
- [Project Structure](#project-structure)

## Features

- Admin panel for managing books, authors, categories, companies, covers, and orders.
- Customer interface for browsing books, managing the shopping cart, and placing orders.
- MVC architecture with separate areas for Admin and Customer functionalities.
- Entity Framework Core for database management.
- Validation and file upload capabilities for book images.

## Installation

1. Clone the repository:
   
   git clone https://github.com/MohanasundaramNagaraj/BookBazaar.git
   cd BookBazaar

2. Change app settings json as your credentials

3. run command update-database



## Project Structure

BookBazaar
├──wwwroot
├── Areas
│   ├── Admin
│   │   ├── Controllers
│   │   │   ├── Api
│   │   │   │   ├── BooksController.cs
│   │   │   │   ├── CompaniesController.cs
│   │   │   │   ├── OrdersController.cs
│   │   │   ├── AuthorController.cs
│   │   │   ├── BookController.cs
│   │   │   ├── CategoryController.cs
│   │   │   ├── CompanyController.cs
│   │   │   ├── CoverController.cs
│   │   │   ├── OrderController.cs
│   │   ├── Views
│   ├── Customer
│   │   ├── Controllers
│   │   │   ├── CartController.cs
│   │   │   ├── HomeController.cs
│   │   ├── Views
├── Data
│   ├── Entities
│   │   ├── ApplicationUser.cs
│   │   ├── Author.cs
│   │   ├── Book.cs
│   │   ├── Category.cs
│   │   ├── Company.cs
│   │   ├── Cover.cs
│   │   ├── OrderDetail.cs
│   │   ├── OrderHeader.cs
│   │   ├── ShoppingCart.cs
│   ├── Profiles
│   ├── Utilities
│   ├── ViewModels
│   │   ├── ApplicationDbContext.cs
│   │   ├── PersistenceContainer.cs
├── Interfaces
├── Migrations
├── Models
├── Repository
│   ├── DbInitializer.cs
│   ├── GenericRepository.cs
│   ├── UnitOfWork.cs
├── ViewComponents
├── Views
├── appsettings.json
├── Program.cs

