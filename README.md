# Student Records API

A clean, modular Web API that handles student data management. It replaces a traditional console user interface with a professional web-based architecture, allowing you to interact with your data via standard HTTP requests.

## What It Does
This system acts as a backend service for your student records:
*   **Data Persistence**: It acts as a "manager" that keeps your student data safe. You can choose to save data to a **CSV file**, a **JSON file**, or keep it in **computer memory** (RAM) for fast, temporary testing.
*   **Business Logic**: It ensures your data remains valid. For example, it prevents you from creating two students with the same ID and handles "Not Found" errors gracefully if you try to access a record that doesn't exist.
*   **Web Receptionist**: Instead of a terminal screen, the API uses "Controllers" to listen for incoming web requests. It translates these requests into actions, handles any errors, and sends back data in standard JSON format.
*   **Full Control**: You can **Create** (POST), **Read** (GET), **Update** (PUT), and **Delete** (DELETE) student records using any standard web tool.

## How to Run
1. Open `StudentRecordsSolution.sln` in Visual Studio.
2. Right-click **StudentRecords.Api** and select **"Set as Startup Project"**.
3. Press **F5** to start.

## Configuration
Change where data is saved in `appsettings.json` choose from json, csv, in-memory:
```
{
  "DatabaseType": "CSV"
}