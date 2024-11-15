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

```sql
SELECT id, name, email, country, phone_number FROM users WHERE id = 20
```

vs

```csharp
users.GetByID(20);
```

## How to get started with Entity Framework

The latest version of Entity Framework is available as the [EntityFramework NuGet Package](https://www.nuget.org/packages/EntityFramework/)

Microsoft Learn has brilliant tutorials on EF Core - an example of which can be found [here -> Getting Started with EF Core](https://learn.microsoft.com/en-gb/ef/core/get-started/overview/first-app?tabs=netcore-cli).

## Why use Entity Framework?

Entity Framework can simplify the creation of a database tailored to you objects, or you can use it to map objects to an existing database. It can make it easier for developers by allowing data manipulation using .NET Objects and tooling such as [LINQ](https://learn.microsoft.com/en-us/dotnet/csharp/linq/)

Entity Framework can help to generate a database and DbContext from your models

## What can it do for me?

1. *Simplified Data Access*  

    EF abstracts the database interactions, allowing you to work with data as objects. This reduces the amount of boilerplate code you need to write.

    Example: Without EF, fetching data might look like this:  

    ```csharp
    using (SqlConnection conn = new SqlConnection(connectionString))
    {
        string query = "SELECT * FROM Customers WHERE TransactionDate > '2023-01-01'";
        SqlCommand cmd = new SqlCommand(query, conn);
        SqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            // Process rows
        }
    }
    ```  

    Whereas using Entity Framework and LINQ could look like this:  

    ```csharp
    using (var context = new MyDbContext())
    {
        var customers = context.Customers
                            .Where(c => c.TransactionDate > new DateTime(2023, 1, 1))
                            .ToList();
    }
    ```

2. *Strongly Typed Queries*  

   EF allows you to write strongly typed queries using LINQ, which are checked at compile time, reducing runtime errors.  

   Example:

    ```csharp  
    var activeUsers = context.Users
                            .Where(u => u.IsActive)
                            .OrderBy(u => u.LastName)
                            .ThenBy(u => u.FirstName)
                            .ToList();
    ```

3. *Change Tracking*  

   EF automatically tracks changes made to objects and saves them to the database when you call SaveChanges(). This makes it easier to manage updates.  

   Example:

   ```csharp
   var user = context.Users.Find(userId);
   user.LastName = "UpdatedLastName";
   context.SaveChanges();
   ```

4. *Eager Loading*  

   Entity Framework lets you include related data as part of the initial query, reducing the number of  database calls.  

   Example:  

   ```csharp
   var usersWithOrders = context.Users
                             .Include(u => u.Orders)
                             .ToList();  
   ```

5. *Migration Support*  

   Entity Framework lets you manage DB schemas through migrations much like Flyway or other migration tools.  

   Example:

   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

6. *Cross Platform*  

   EF Core - the latest version - is compatible with Windows, macOS, and Linux.

## Which Databases Does Entity Framework Support?

Entity Framework currently supports:  

1. [SQL Server](https://learn.microsoft.com/en-gb/ef/core/providers/sql-server/)
2. [SQLite](https://learn.microsoft.com/en-gb/ef/core/providers/sqlite/)
3. [PostgreSQL](http://www.npgsql.org/efcore/index.html)
4. [MySQL and MariaDB](https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql)
5. [Azure Cosmos DB for NoSQL](https://learn.microsoft.com/en-gb/ef/core/providers/cosmos/)
6. [In memory (testing)](https://learn.microsoft.com/en-gb/ef/core/providers/in-memory/)
7. And others - see a full list of [All current Database Providers](https://learn.microsoft.com/en-gb/ef/core/providers/)

## How do I troubleshoot or write custom SQL?

A tutorial on using Raw SQL can be found [here](https://learn.microsoft.com/en-gb/ef/core/querying/sql-queries?tabs=sqlserver) - please note that it is important to maintain best practices for defending against SQL Injection if you take this approach, and as such it is not recommended for normal interactions with the DB.

## Advantages & Disadvantages

### Advantages

+ Can simplify data access via .NET objects
+ Strongly typed through use of LINQ can help catch errors at compile-time and make code more readable.
+ Can track and automatically save changes to objects.
+ Can reduce number of database calls with Eager Loading.
+ Supports easy-to-use schema migrations.
+ Cross-platform.
+ Integrates well with code-gen and scaffolding features in Visual Studio and JetBrains Rider.
  
### Disadvantages

+ Can add performance overhead due to the added abstraction layer - might not be best suited to performance critical applications
+ May struggle with complex queries by potentially generating less efficient SQL than a suitably knowledgable developer.
+ There may be a considerable learning curve for developers who are not familiar with ORMs or come from a pure/raw SQL background.
+ Limited control due to abstraction of many database controls.
+ If lazy-loading is not managed properly it may cause performance issues with excess database calls.
+ Caution needed to ensure compatibility with more niche DB providers.
