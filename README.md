# uLocate v.2 for Umbraco 7#

----------


uLocate is a collaborative Umbraco package which aims to provide Geography data types for Umbraco based on the Microsoft SQL Server Geography type.

## Project Status
UNDER ACTIVE DEVELOPMENT - EARLY STAGES

## Requirements
Supports umbraco 7.x websites.

In order to use uLocate in your umbraco site, you will need to be using a full SQL Server database (not the SQL CE database which is installed by default). This is because the uLocate custom data tables use the "Geography" datatype, which is not available in SQL CE (Info:
[Data Types in SQL Server Compact 4.0](http://msdn.microsoft.com/en-us/library/ms172424%28SQL.110%29.aspx))


## Solution Contents

The Visual Studio solution includes several projects:

### uLocate
The Business Logic and Data access code

Caching

Configuration

**Data** - Includes DTOs for the uLocate tables, database install/uninstall, and static Data Helper functions.

Helpers

**Models** - Interfaces and class models for all uLocate objects

Persistence

Providers

**WebApi** - Api Controllers

### uLocate.Package
Umbraco Package components

### uLocate.Plugins.Geocode.GoogleMaps
A GoogleMaps Provider Plugin

### uLocate.UI

The code needed for the back-office UI

### UmbracoTestSite

Included for testing & development purposes is an Umbraco website folder. However, due to the requirement for a full SQL Server database, a .sdf file is NOT included. In order to use the demo site on your local environment, you will need to run the umbraco install wizard to setup a fresh database. 

Setup your empty database on your local environment with a new login user as the DB Owner.

Copy the web.config file from the /copy-files/ folder to the folder "/src/UmbracoTestSite" and run the project in a web browser.

On the install screen, fill in your desired login info and click the "Customize" button. Select the database type "MS SQL Server" and fill in your local database credentials. 


## Version Changes (from uLocate v.1)
* Back-office interface created for umbraco 7
* Removed the dependency of the SQL CLR Types requirement to be installed on the webserver.

