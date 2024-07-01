# BookBazaar

BookBazaar is a web application designed for managing book sales. It features an admin panel for managing books, authors, 
categories, companies, covers, and orders, as well as a customer interface for viewing books, managing the shopping cart
and placing orders.

## Table of Contents

- [Features](#features)
- [Installation](#installation)
- [Usage](#usage)
- [Project Structure](#project-structure)
- Images(#images)

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

## Images
[Register](https://github.com/MohanasundaramNagaraj/BookBazaar/assets/159687200/9832524e-bb66-4b28-b5f9-7aaf623ceaa6)
[Login](https://github.com/MohanasundaramNagaraj/BookBazaar/assets/159687200/831983b7-ab0f-4b08-87c2-a3e501ef68d4)
[Admin Home](https://github.com/MohanasundaramNagaraj/BookBazaar/assets/159687200/6ca87857-6f42-4dc5-b14a-4f51d81fb176)
[Dashboard](https://github.com/MohanasundaramNagaraj/BookBazaar/assets/159687200/11990872-af26-475b-9bdd-5f1ee984aacc)
[Cart](https://github.com/MohanasundaramNagaraj/BookBazaar/assets/159687200/a1b51447-3e1b-4d1b-91f7-9a8b0b679b03)
[Customer Home](https://github.com/MohanasundaramNagaraj/BookBazaar/assets/159687200/91825fd9-0bc0-422b-bde8-9dcbc97a3d63)
