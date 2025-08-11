# GymCollab

A gym management app I built for my ASP.NET Core class project.

## What it does

- Track gym equipment (treadmills, weights, etc.)
- Schedule gym classes
- Manage gym members
- User login system

## API Stuff

I added Swagger to document the API endpoints. Pretty cool right?

### How to see the docs

1. Run the app in Visual Studio (make sure it's in debug mode)
2. Go to `/swagger` in your browser
3. Check out all the API endpoints

### Main APIs

- `/api/equipment` - Add/remove/edit gym equipment
- `/api/gymclasses` - Create and manage classes
- `/api/members` - Handle member info

### Login required

Some endpoints need you to be logged in first (the ones with [Authorize])

## Tech stuff

- .NET 8.0
- Entity Framework Core
- SQLite database
- Swagger for API docs

## Notes

This was my first time using Swagger - it's actually pretty useful for testing APIs!
The app uses cookies for login which I learned about in class.