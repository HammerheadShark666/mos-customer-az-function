# mos-registered-user-add-customer-func

Microservice Order System - Customer Microservice

[See Wiki for details about the Registered User Add Customer Function](https://github.com/HammerheadShark666/mos-registered-user-add-customer-func/wiki)

This project is an **Azure Function** designed to add customer details. It is built using **.NET 8**, interacts with an **SQL Server** database for storage, and processes messages from **Azure Service Bus** for customer registration. The function is set up with a **CI/CD pipeline** for seamless deployment.

## Features

- **Customer Addition**: Listens for messages from Azure Service Bus to add customer details to the SQL Server database.
- **SQL Server Database**: Stores customer information such as names, emails, and contact details.
- **Azure Service Bus Integration**: Consumes messages from the service bus to trigger customer additions.
- **Scalable Serverless Architecture**: Utilizes Azure Functions for on-demand execution and scaling.
- **CI/CD Pipeline**: Automated build and deployment using **GitHub Actions**.

---

## Technologies Used

- **.NET 8**
- **C#**
- **Azure Functions**
- **SQL Server** (Azure SQL)
- **Azure Service Bus**
- **GitHub Actions** for CI/CD

---
