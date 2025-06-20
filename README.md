# 📚 BookSaw\.Api

**BookSaw\.Api** is a RESTful API built with ASP.NET Core for managing an online book store. It allows listing, filtering, adding, and administrating books and categories. It is designed to be consumed by frontend applications such as React + Vite.

## 🚀 Features

* Full CRUD operations for books
* Book-to-category association
* Search and filter by title, author, category, and price
* Auto-seeding with real or fake data
* Support for multiple environments: Staging and Production
* Docker containerization
* PostgreSQL integration

## 🛠️ Tech Stack

* ASP.NET Core 8
* Entity Framework Core
* PostgreSQL
* Docker
* NGINX (for proxy)
* GitHub Container Registry (ghcr.io)

## 📦 Local Setup

### 1. Clone the repository

```bash
git clone https://github.com/ITSulik/BookSaw.Api.git
cd BookSaw.Api
```

### 2. Environment settings

Update `appsettings.Development.json` or set environment variables:

```json
"ConnectionStrings": {
  "BookSawDb": "Host=localhost;Port=5432;Database=BookSawDb;Username=BookSaw;Password=BookSaw123"
}
```

### 3. Run with .NET CLI

```bash
dotnet restore
dotnet ef database update
dotnet run --project BookSaw.Api
```

## 🐳 Docker Setup

### 1. Prerequisites

* [Docker](https://www.docker.com/)
* [Docker Compose](https://docs.docker.com/compose/)

### 2. Run

```bash
docker compose up --build
```

### 3. Access

* API: [http://localhost/api/books](http://localhost/api/books)
* DB: PostgreSQL at `localhost:5432`

## 🔄 Seeder

Seeder runs automatically inside the container. To run manually:

```bash
dotnet run --project tools/BookSaw.Seeder
```

Two seeders are available:

* `StagingSeeder`: for fake/testing data
* `ProductionSeeder`: for real data


## 🔐 NGINX Proxy Configuration

Located in `nginx/booksaw-proxy.conf`:

```nginx
location /api/ {
    proxy_pass http://booksaw-api:8080;
    rewrite ^/api/(.*)$ /$1 break;
}
```

## 🧪 Testing

Test the API using tools like Postman or from the integrated frontend application.

## 🤝 Contributions

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## 📄 License

[MIT](LICENSE)
