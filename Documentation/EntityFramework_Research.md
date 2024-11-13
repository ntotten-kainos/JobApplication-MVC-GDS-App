# Entity Framework Spike

Author(s) : Nathan Totten  
Project : "Gov Yourself a Job" Internal .NET Learning Project

## What is Entity Framework?
Entity Framework is an ORM (Object-Relational Mapper) that helps .NET devs with relational data using domain-specific objects.  
It does a decent job of eliminating most data-access code that developers usually need to write, such as establishing connections, using DAOs, and prepared SQL statments to access and modify data in a relational database.

### But what is an ORM?

Object-Relational Mapping is a layer that connects OOP to relational databases.
Typical ORMs provide a means of performing common CRUD operations with simpler methods.  

Take the following example (Syntax may vary across ORMs):  
`SELECT id, name, email, country, phone_number FROM users WHERE id = 20`  
vs  
`users.GetByID(20);`

## How to get started with Entity Framework

The latest version of Entity Framework is available as the [EntityFramework NuGet Package](https://www.nuget.org/packages/EntityFramework/)

## Why use Entity Framework?

Entity Framework can simplify the creation of a database tailored to you objects, or you can use it to map objects to an existing database.  



## What can it do for me?

## How do I troubleshoot or write custom SQL?

## Advantages & Disadvantages