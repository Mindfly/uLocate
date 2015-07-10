# uLocate v.2 for Umbraco 7#

----------


uLocate is a collaborative Umbraco package which aims to provide Geography data types for Umbraco based on the Microsoft SQL Server Geography type.

*If you are looking for uLocate v.1 for Umbraco 4/6, see [bitbucket.org/rustyswayne/ulocate](https://bitbucket.org/rustyswayne/ulocate)

## Project Status
UNDER ACTIVE DEVELOPMENT

## Requirements
Supports umbraco 7.x websites.

In order to use uLocate in your umbraco site, you will need to be using a full SQL Server database (not the SQL CE database which is installed by default). This is because the uLocate custom data tables use the "Geography" datatype, which is not available in SQL CE (Info:
[Data Types in SQL Server Compact 4.0](http://msdn.microsoft.com/en-us/library/ms172424%28SQL.110%29.aspx))

## Contributing

If you would like to contribute to uLocate, we would love your help! This package has been thoroughly wireframed (for release and future updates), so please follow the patterns and intent displayed in them: [uLocate Moqups](https://moqups.com/mindfly/9TFhrBLu)

## A Note About Names & New Architecture
In July 2015 we were able to dramatically improve performance on querying of Location data by implementing an Examine Index. Similar to how older versions of Umbraco had both a "read-only" "Node" object as well as an editable "Document" object, there are now two sorts of "location" objects in the code:
* **IndexedLocation** is the "read-only" version of a Location - generally this is pulled from the Examine Index and constructed into a strongly-typed object for usage convenience (formerly named "JsonLocation"). Pulling lists of locations using this object type is much faster now.
* **EditableLocation** is the version which is used for creating, deleting, and updating locations. It is actively persisted to the database. (formerly named just "Location") *NOTE: There is a lot of room for improvement in code related to EditableLocation.*

The term "Location" now refers to the concept of a location generically, and in implementation might utilize either or both of the "location" code objects.

Generally, all interaction with locations from outside of uLocate internals should utilize the "LocationService" and the WebApi (which should also be utilizing the "LocationService" for it's various endpoints.) It might take a while for the code to be fully moved over to this architecture, however.

## Solution Contents

The Visual Studio solution includes several projects:

### uLocate
The Business Logic and Data access code

* Caching

* Configuration

* **Data** - Includes DTOs for the uLocate tables, database install/uninstall, and static Data Helper functions.

* Helpers

* **Models** - Interfaces and class models for all uLocate objects

* **Persistence** - Repositories to handle CRUD operations

* Providers

* **WebApi** - Api Controllers

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

