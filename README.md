# BookStore-Library
This is an example of a C# library of a Bookstore Application
It uses text and XML files to store and handle data. 

There is also a login and user level management.

The project is built using three tiers levels 
GUI - for graphic interface (App)

BLL and DAL levels were developted in library project (dll)

BLL - Handles the business level ( all rulles concerned to the business itself)

DAL - Data access layer handles all files access readings and updates. It can be easly modified to handles a database instead of files.

GUI is present in APP project

In order to increase security, the User class stores only the hash of every user pwd
