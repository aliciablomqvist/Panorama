# Panorama - Movie Management and Social Sharing Platform

Panorama is a dynamic platform for managing, sharing, and discovering movies within groups. Designed for movie enthusiasts, it provides features such as personalized movie lists, group chats, voting systems, and integrations with the TMDB API for recommendations and movie details.

* * *

## Table of Contents

1.  [Project Description](#project-description)
2.  [Features](#features)
3.  [How to Install and Run the Project](#how-to-install-and-run-the-project)
4.  [How to Use the Project](#how-to-use-the-project)
5.  [Creator](#creator)

* * *

## Project Description

**Panorama** is built to solve the challenge of managing movies across groups with collaborative tools for sharing and discussions. It integrates with the TMDB API to fetch movie details, recommendations, and trailers. Users can create movie lists, share them with friends or groups, vote on movies, and engage in group chats.

The project focuses on adhering to SOLID principles, ensuring the architecture is modular, scalable, and testable. Through clean abstractions and services, Panorama is both a learning project and a showcase of best practices in modern web development.

**Technologies and structure:**

*   **C# and ASP.NET Core Razor Pages**: For the backend and frontend.
*   **Entity Framework Core**: For database operations.
*   **Microsoft Identity**: For authentication and user management.
*   **TMDB API**: For movie data and recommendations.
*   **Dependency Injection**: To decouple services and ensure testability.

* * *

## Features

*   **Movie Management**: Create, view, and manage personal or shared movie lists.
*   **Group Collaboration**: Share movie lists, vote for favorites, and discuss them in group chats.
*   **Recommendations**: Fetch recommended movies based on selected titles via the TMDB API.
*   **User-Friendly Interface**: Clean design with features like search, sorting, and filtering.
*   **Authentication and Authorization**: Secure user access with Microsoft Identity.
*   **Modular Architecture**: Adheres to SOLID principles for maintainability and scalability.

* * *

## How to Install and Run the Project

### Prerequisites

*   [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
*   [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
*   TMDB API Key (https://www.themoviedb.org/)

### Installation Steps

1.  **Clone the Repository**:
    
```bash
   git clone https://github.com/yourusername/Panorama.git 
   cd Panorama
```
    
2.  **Set Up the Database**:
    
    *   Update `appsettings.json` with your SQL Server connection string.
    *   Run migrations to set up the database schema:
        
```bash
dotnet ef database update
```
        
3.  **Configure TMDB API**:
    
    *   Add your TMDB API key to the project's user secrets:
        
```bash
dotnet user-secrets set "Tmdb:ApiKey" "YOUR_TMDB_API_KEY"
```
        
4.  **Run the Application**:
    
 ```bash
dotnet run
```
    
5.  Open your browser and navigate to your localhost.
    

* * *

## How to Use the Project

1.  **Sign Up**: Create an account to access features like movie lists and group collaboration.
2.  **Create Movie Lists**: Add your favorite movies to lists and share them with groups.
3.  **Vote and Discuss**: Join group chats to vote and discuss movies with friends.
4.  **Explore Recommendations**: Use TMDB-powered recommendations to discover new movies.
5.  **Authentication**: Securely log in and manage your profile.

* * *

## Credits

### Creator

*   **Alicia Blomqvist**: [GitHub Profile](https://github.com/aliciablomqvist)

### Data Source

*   **TMDB API**: Movie data and recommendations are powered by [The Movie Database (TMDB)](https://www.themoviedb.org/).

* * *
