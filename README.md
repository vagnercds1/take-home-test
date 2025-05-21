# **Take-Home Test: Backend-Focused Full-Stack Developer (.NET C# & Angular)**
 
## **Instructions**

✔ - To run the .NET 8 application locally, open the solution file take-home-test\backend\src\src.sln

 1 - Right-click on the solution and select 'Configure startup projects...'
 
 2 - Select 'Multiple startup projects' and choose 'Fundo.Applications.WebApi' and 'Fundo.Applications.WebApiSecurity' projects. 

 3 - Update the connection string in the appsettings.Development.json file of both Fundo projects.Applications.WebApi and Fundo.Applications.WebApiSecurity


 4 - Open the windows package Manager Console for execute migrations and check if the database was successfully created on Sql Management studio.
    
    command: Update-Database 

 5- - Press run and Visual Studio will open them in your selected browser.


✔ - For APIs tests, open the Postman and import both files of the folder take-home-test\Postman

    Fundo.postman_collection.json
    
    LocalHost.postman_environment.json

✔ - The user for authentication was created by Entity Framework Seed and can be used in the login route previously imported in postman

    user: john@test.com

    password: 1234


✔ - To run the application in Docker, open the folder take-home-test\backend and update the files Fundo.Applications.WebApi\Startup.cs and Fundo.Applications.WebApiSecurity\Startup.cs for retrive the connection string from the environment variable.

Execute the command on terminal.

	docker-compose up --build

	Open the browser and access the URL: http://localhost:5000/

	The Swagger interface will be available at: http://localhost:5000/swagger/index.html


✔ - For start the Front End Angular application, open the folder take-home-test\frontend and run the command:

	npm install

	npm start

    Open the browser and access the URL: http://localhost:4200/

 
## **Additional Information**

With the goal of contributing to Fundo’s development team, I developed the loan management API following best development practices and a DDD architecture. I implemented the repository layer, request model validation using FluentValidation, standardized HTTP responses using appropriate HttpStatus Codes and HTTP Verbs, and a well-documented Swagger interface. I also implemented standardized logging to be captured by tools like FileBeat and stored in databases such as Elastic.

For security, I developed a new JWT-based authentication service, designed to run in a separate container, enabling individual scalability and enhanced protection against login-related attacks.

I wrote unit and integration tests for the most important features, but I didn't achieve full line coverage. My focus was to demonstrate how everything works.

During development, I saw the need to review some Entity Framework Core concepts, Angular JS basic concepts and Docker-Compose.

 
