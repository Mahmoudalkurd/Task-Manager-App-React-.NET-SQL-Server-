Full-Stack Task Manager App
Project Description
This project is a Full-Stack Task Manager application that allows users to:
Create, update, delete, and mark tasks as completed.Filter and sort tasks by title, completion status, or due date.Assign tags to tasks for better organization.The frontend is built with React.js + TypeScript, and the backend is built with .NET 8 Web API using Entity Framework Core for database access

Architecture Overview
Frontend (React + TypeScript)
│
├─ Pages: TasksPage.tsx
├─ Components: reusable UI components
├─ Services: Axios API calls (taskService.ts)
└─ State: useState & useEffect

Backend (.NET Web API)
│
├─ Controllers: TasksController.cs
├─ Entities: TaskItem, TaskTag, Tag, User
├─ Infrastructure: AppDbContext (EF Core)
└─ Logging: ILogger

Database (SQL Server / SQLite / PostgreSQL)
│
├─ Tables: Tasks, TaskTags, Users, Tags
└─ Relationships:
    - One User → Many Tasks
    - Many-to-Many: TaskTags


Backend

Navigate to backend folder:
cd backend

Restore dependencies:
dotnet restore

Apply EF Core migrations :
dotnet ef migrations add InitialCreate
dotnet ef database update

Run the backend:
dotnet run

API will run at:
http://localhost:5063/api

Frontend

Navigate to frontend folder:
cd frontend

Install dependencies:
npm install

Start the frontend:
npm start

The app will open at:
http://localhost:3000

Database

Configure connection string in appsettings.json of backend:
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=TaskManagerDb;Trusted_Connection=True;"
}


Run migrations to create tables:
dotnet ef migrations add InitialCreate
dotnet ef database update


Endpoints
This full API list allows the frontend to perform:
CRUD on Tasks, Users, and Tags
Task filtering and sorting
Tasks
Method	Endpoint	Purpose	Parameters / Body
GET	/api/tasks	Get all tasks	Optional query: search (string), sortBy (title/dueDate), sortDir (asc/desc)
GET	/api/tasks/{id}	Get task by ID	id (path)
POST	/api/tasks	Create a new task	JSON body: { title, description, completed?, dueDate?, userId }
PUT	/api/tasks/{id}	Update a task	id (path), JSON body: { title, description, completed, dueDate, userId }
DELETE	/api/tasks/{id}	Delete a task	id (path)


Users
Method	Endpoint	Purpose	Parameters / Body
GET	/api/users	Get all users	None
GET	/api/users/{id}	Get user by ID	id (path)
POST	/api/users	Create a new user	JSON body: { name, email, password }
PUT	/api/users/{id}	Update user	id (path), JSON body: { name?, email?, password? }
DELETE	/api/users/{id}	Delete user	id (path)

Tags
Method	Endpoint	Purpose	Parameters / Body
GET	/api/tags	Get all tags	None
GET	/api/tags/{id}	Get tag by ID	id (path)
POST	/api/tags	Create a new tag	JSON body: { name }
PUT	/api/tags/{id}	Update tag	id (path), JSON body: { name }
DELETE	/api/tags/{id}	Delete tag	id (path)
POST /api/tasks/{id}/tags → Assign tags

GET /api/tasks/{id}/tags → Fetch tags for task

GitHub Actions CI/CD
This workflow automatically builds, tests, and verifies both backend and frontend on push or pull request events on the main branch.
