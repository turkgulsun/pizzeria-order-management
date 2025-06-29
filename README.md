# Pizzeria Order Management

## Overview

**Pizzeria Order Management** is a clean and modular .NET 8 console application that manages and processes pizzeria orders following Clean Architecture and Domain-Driven Design (DDD) principles. The application parses orders from JSON/CSV files, validates them, calculates order totals using dynamic product and ingredient data, and outputs a detailed summary including total raw ingredients required for all orders.

This project demonstrates modern best practices in enterprise .NET development and is designed for clarity, extensibility, and maintainability.

---

## Features

- **Order File Parsing:** Reads and parses orders from JSON or CSV files.
- **Order Validation:** Enforces business rules and data validation using FluentValidation and domain-level rules.
- **Dynamic Product Pricing:** Loads product catalog and prices from an external JSON file.
- **Ingredient Aggregation:** Calculates the total amount of each raw ingredient required, using product-to-ingredient mappings from a JSON file.
- **Summary Output:** Displays all valid orders and a breakdown of ingredient requirements in the console.
- **Layered Architecture:** Clean separation of Domain, Application, Infrastructure and Presentation (Console) layers.

---

## Technologies & Standards Used

- **.NET 8** – Modern, high-performance framework for console and backend apps.
- **C# 12** – Latest language features, records, pattern matching, etc.
- **Clean Architecture** – Separation of concerns, maintainable and scalable structure.
- **Domain-Driven Design (DDD)** – Entity, Aggregate Root, Value Objects, encapsulated domain logic.
- **FluentValidation** – Declarative and reusable validation logic.
- **Mapster** – Lightweight object mapping between DTOs and domain models.
- **Dependency Injection** – For testability and loose coupling.
- **File I/O** – For reading orders, products, and ingredients from JSON/CSV files.

Getting Started
Prerequisites
.NET 8 SDK installed

Clone the repository
bash
Copy
Edit
git clone https://github.com/turkgulsun/pizzeria-order-management.git
cd pizzeria-order-management

Add Sample Data
Place your orders.json, products.json, and ingredients.json files in the designated data folder (see code/configuration for paths).

``` json

orders.json
[
  {
    "OrderId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "ProductId": "4fa85f64-5717-4562-b3fc-2c963f66afb7",
    "Quantity": 2,
    "DeliveryAt": "2023-12-31T18:00:00",
    "CreatedAt": "2023-12-30T12:00:00",
    "Street": "123 Pizza St",
    "City": "Napoli",
    "PostalCode": "80100"
  },
  {
    "OrderId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "ProductId": "5fa85f64-5717-4562-b3fc-2c963f66afc8",
    "Quantity": 1,
    "DeliveryAt": "2023-12-31T18:00:00",
    "CreatedAt": "2023-12-30T12:00:00",
    "Street": "123 Pizza St",
    "City": "Napoli",
    "PostalCode": "80100"
  },
  {
    "OrderId": "4fa85f64-5717-4562-b3fc-2c963f66afb9",
    "ProductId": "4fa85f64-5717-4562-b3fc-2c963f66afb7",
    "Quantity": 3,
    "DeliveryAt": "2023-12-31T19:00:00",
    "CreatedAt": "2023-12-30T13:00:00",
    "Street": "456 Pasta Ave",
    "City": "Roma",
    "PostalCode": "00100"
  }
]

products.json
[
  {
    "ProductId": "4fa85f64-5717-4562-b3fc-2c963f66afb7",
    "ProductName": "Margherita Pizza",
    "Price": 10.50
  },
  {
    "ProductId": "5fa85f64-5717-4562-b3fc-2c963f66afc8",
    "ProductName": "Pepperoni Pizza",
    "Price": 12.00
  }
]

ingredients
[
  {
    "ProductId": "4fa85f64-5717-4562-b3fc-2c963f66afb7",
    "Ingredients": [
      {
        "Name": "Flour",
        "Amount": 200,
        "Unit": "g"
      },
      {
        "Name": "Tomato Sauce",
        "Amount": 100,
        "Unit": "g"
      },
      {
        "Name": "Mozzarella",
        "Amount": 150,
        "Unit": "g"
      }
    ]
  },
  {
    "ProductId": "5fa85f64-5717-4562-b3fc-2c963f66afc8",
    "Ingredients": [
      {
        "Name": "Flour",
        "Amount": 200,
        "Unit": "g"
      },
      {
        "Name": "Tomato Sauce",
        "Amount": 100,
        "Unit": "g"
      },
      {
        "Name": "Mozzarella",
        "Amount": 150,
        "Unit": "g"
      },
      {
        "Name": "Pepperoni",
        "Amount": 80,
        "Unit": "g"
      }
    ]
  }
]

