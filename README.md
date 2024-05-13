# TVMaze Metadata Ingester

This project aims to ingest metadata from the TVMaze database, specifically focusing on retrieving information about TV shows and their cast members. The TVMaze database offers a public REST API that allows querying for such data without authentication. However, it's important to note that the API is rate-limited.

## Assignment

The task at hand is to develop an application that fulfills the following requirements:

1. **Scrapes TVMaze API**: Retrieve show and cast information from the TVMaze API.
2. **Persists Data**: Store the scraped data persistently.
3. **Provides REST API**: Expose the scraped data through a RESTful API.

## Business Requirements

The REST API should meet the following business requirements:

1. **Paginated Show List**: Provide a paginated list of all TV shows, including the show ID and a list of cast members.
2. **Ordered Cast List**: Ensure that the list of cast members for each show is ordered by birthday in descending order.

The REST API should return JSON responses when queried through HTTP endpoints. The specific URI for the API endpoints is left to the developer's discretion.

## Technical Details

- **Technology**: Developed using .NET 8 with MediatR following clean architecture principles.
- **Project Structure**: Organized into API, Application, Domain, and Infrastructure layers.
- **Open Endpoint**: The API offers an open endpoint for fetching all TV shows with pagination support.
- **Background Jobs**: Includes two background jobs. One job, named `ImportAll`, is implemented to import all TV shows and cast members from the TVMaze API and persist them in the database. The other job, `Update`, is not yet implemented but is intended to check for updates on TV shows and cast members daily.
- **Database Entities**: Utilizes Entity Framework Core with two entities: `TvShows` and `CastMembers`. Although initially planned as a many-to-many relationship using EF, due to encountered errors, a custom joint table was created. This aspect is identified as an area for improvement.
- **Missing Features**: Acknowledges the absence of tests, validation, robust logging, and comprehensive exception handling, all of which are recognized as areas for future enhancement.

This README serves as an overview of the project, detailing its objectives, implementation approach, and identified areas for improvement.
