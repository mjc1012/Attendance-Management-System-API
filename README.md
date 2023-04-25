# ATTENDANCE MANAGEMENT SYSTEM API (BACKEND)

## A fully functioning backend project developed using ASP.NET CORE WEB API for the FACIAL RECOGNITION ATTENDANCE MANAGEMENT SYSTEM - FRONTEND

To run this project please follow the steps below:

1. Clone this project to Visual Studio Code 2022 
2. Make sure all the nuget packages are properly installed
3. Change the value of ConnectionStrings.DefaultConnection in appsettings.json to the connection string of your local database
4. Type 'add-migration initialcreate' in Package Manager Console to create a migration to create the existing schema
5. Then type 'update-database' to create database schema
6. Use Smtp Server of Google and change the values in EmailConstants class in Constants.cs to configure the Smtp server of Google (Watch an example on how to use Smtp Server
of Google here: https://www.youtube.com/watch?v=JH5CWFtoG-k&list=PLc2Ziv7051bZhBeJlJaqq5lrQuVmBJL6A&index=10)
7. Make sure the values for the base URLs in FaceRecognitionConstants class in Constants.cs are the same with the URLs in FACE RECOGNITION API
8. Make sure FACE RECOGNITION API is running
9. Run the application
10. Default Account has the following credentials:
* Id Number: ADMIN
* Password: ADMIN
