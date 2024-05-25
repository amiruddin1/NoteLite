
# Note-Lite
NoteLite is a minimalistic note-taking web app built with ASP.NET Core MVC. Users can sign up, log in, create, edit, delete, and categorize notes. Admins can manage all notes and categories. Features include user authentication, search/filter, and a clean interface. Ideal for personal and professional use.

## Features

### User Roles
- **User**:  
  - Sign up and log in  
  - Create, edit, and delete notes  
  - Categorize notes (e.g., Work, Personal, Study)  
  - Search and filter notes by title or category  

- **Admin**:  
  - View all users' notes  
  - Delete any inappropriate content  
  - Manage categories globally  

### Note Features
- Add new notes with a title and content body
- Optionally tag notes with categories
- Simple search and filter functionality for easy note retrieval

### Authentication
- Basic user authentication for registration and login

### Database Integration
- Utilizes Entity Framework Core for data management
- Simple schema design supporting note creation, categorization, and user association

### Interface and Usability
- Clean and intuitive web interface using ASP.NET Core MVC
- Easy navigation with a focus on user experience

### Validation and Error Handling
- Basic validation for user inputs (e.g., non-empty notes, character limits for titles and content)
- Clear feedback for errors and successful actions (e.g., note creation or deletion confirmation)

### Optional Enhancements
- Rich text editor for note content (bold, italic, bullet points)
- Note sharing with other users or exporting notes as PDF

## Getting Started

### Prerequisites
- .NET Core SDK
- Visual Studio or any preferred IDE
- SQL Server or any preferred database

### Installation

1. **Clone the repository:**
   ```sh
   git clone https://github.com/your-username/NoteLite.git
   ```
2. **Navigate to the project directory:**
    ```sh
   cd NoteLite
   ```
3. **Restore the dependencies:**
    ```sh
   dotnet restore
   ```
4. **Update the database connection string in appsettings.json to match your database configuration.**
5. **Apply migrations to create the database schema:**
    ```sh
   dotnet ef database update
   ```
6. **Run the application:**
     ```sh
   dotnet run
   ```
