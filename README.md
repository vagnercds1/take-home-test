# **Take-Home Test: Backend-Focused Full-Stack Developer (.NET C# & Angular)**
 
## **Instructions**

✔ - To run the .NET 8 application locally, simply open the solution file take-home-test\backend\src\src.sln

 1 - Right-click on the solution and select 'Configure startup projects...'
 
 2 - Select 'Multiple startup projects' and choose 'Fundo.Applications.WebApi' and 'Fundo.Applications.WebApiSecurity' projects. 
 
 3 - Press run and Visual Studio will open them in your selected browser.

✔ - Update the connection string in the appsettings.Development.json file of both Fundo projects.Applications.WebApi and Fundo.Applications.WebApiSecurity.

✔ - Open the windows package Manager Console for execute migrations and check if the database was successfully created
    command: Update-Database 

✔ - For APIs tests, open the Postman and import both files of the folder take-home-test\Postman
    Fundo.postman_collection.json
    LOCAL HOST.postman_environment.json

✔ - The user for authentication was created by Entity Framework Seed and can be used in the login route previously imported in postman 
    user: john@test.com
    password: 1234
 
## **Additional Information**

With the goal of contributing to Fundo’s development team, I developed the loan management API following best development practices and a DDD architecture. I implemented the repository layer, request model validation using FluentValidation, standardized HTTP responses using appropriate HttpStatus Codes and HTTP Verbs, and a well-documented Swagger interface. I also implemented standardized logging to be captured by tools like FileBeat and stored in databases such as Elasticsearch.

For security, I developed a new JWT-based authentication service, designed to run in a separate container, enabling individual scalability and enhanced protection against login-related attacks.

Unfortunately, due to time constraints, I wasn't able to integrate the frontend or create a full Docker Compose setup with the three applications: Frontend, LoanWebApi, and LoginApi.
Due to time constraints, unit tests were created only in the domain layer. However, I believe this is enough to demonstrate that I have the technical knowledge to implement tests across all layers with full line and rule coverage.

Given more time, I would study Angular to implement the frontend integration, although I’m fully capable of working with it and have used it over five years ago.

During development, I saw the need to review some Entity Framework Core concepts and studied them in order to implement the data layer successfully.