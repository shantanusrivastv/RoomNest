# RoomNest Application #

## Setup Instruction ##

1.	Open the solution in Visual Studio.
2.	Confirm the connection string in appsettings.json matches your launch configuration.
3.	Open the Package Manager Console.
4.	**Optional** Run Update-Database — migrations are also applied automatically when the application starts.
5.	Select the RoomNest.API project as the startup project; it uses LocalDB by default.
6.	Launch the application (Ctrl + F5).
	By default, it runs at http://localhost:5117.
	The Swagger UI will automatically open at http://localhost:5117/swagger/index.html.
7.	**Optional** Switch the launch profile to IIS Express or SdkContainer, and update the connection string accordingly.

#### Run from Command Line ####

 1. cd into API proj -> cd RoomNest/RoomNest.API
 2. Run -> dotnet run

#### Run in Release Mode ####

 1. Build in Release mode -> dotnet build --configuration Release
 2. Navigate to release folder -> cd RoomNest/RoomNest.API\bin\Release\net8.0\
 3. Run  ->  dotnet Pressford.News.API.dll --urls "<http://localhost:5117>"

### Login Information ##

No authentication required.

