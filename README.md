#  Simple Task Management System (3-tier architecture)

A simple **3-tier architecture** project built with **C# .NET 6 Web API** and **SQL Server**, designed to manage tasks with full CRUD operations, paging, and conflict detection.

## 🔹 Architecture
- **DAL (Data Access Layer)** – Interacts with SQL Server using stored procedures and functions.  
- **BL (Business Layer)** – Handles validation and business rules (task conflicts, past tasks).  
- **API (Web API Layer)** – Exposes RESTful endpoints for task operations.  

## 🔹 Features
- Add, update, delete tasks  
- Get task by ID  
- Pagination (all, past, upcoming tasks)  
- Conflict detection (overlapping tasks)  

## 🔹 API Endpoints
| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/api/TaskManagement/AddTask` | Add a new task |
| `PUT` | `/api/TaskManagement/{id}` | Update a task |
| `DELETE` | `/api/TaskManagement/{id}` | Delete a task |
| `GET` | `/api/TaskManagement/{id}` | Get task by ID |
| `GET` | `/api/TaskManagement/GetTasksAsPages` | Get paginated tasks |
| `GET` | `/api/TaskManagement/GetPastTaskAsPages` | Get past tasks |
| `GET` | `/api/TaskManagement/GetActiveTasksAsPages` | Get active tasks |

## 🔹 How to Run
1. Clone the repo  
2. Run the SQL script to create DB & procedures  
3. Update `appsettings.json` with your connection string  
4. Start `TaskManagementWepAPI` project  
5. Test with Swagger or Postman  

---

⚡ **A simple and clean task manager built with .NET & SQL Server.**
