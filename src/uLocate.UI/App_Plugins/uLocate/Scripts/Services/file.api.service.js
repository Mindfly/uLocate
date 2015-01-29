(function (uLocateServices) {

    uLocateServices.FileApiService = function ($http, $q) {

        var fileApiFactory = {};


        /**
         * @ngdoc method
         * @name exportFile
         * @function
         * 
         * @param {string} locationType - string key for location type desired for export.
         * @param {strong} fileFormat - Optional. Desired format of resulting file. Defaults to CSV.
         * @param {boolean} shouldCompress - Optional. Whether to compress file to a zip or not. Defaults to false.
         * @returns {string} - The url that the desired file can be downloaded from.
         * @description - Export all locations of a specific type to a file on the end user's system.
         */
        fileApiFactory.exportFile = function(locationType, fileFormat, shouldCompress) {
            if (!fileFormat) {
                fileFormat = 'csv';
            }
            if (!shouldCompress) {
                shouldCompress = false;
            }
            if (locationType) {
                return '/urlgoeshere/filedownload.axd?format=' + fileFormat + '&type=' + locationType + '&compress=' + shouldCompress;
            } else {
                return false;
            }
        };

        fileApiFactory.importLocationsCsv = function(name) {
            return $http.get('/umbraco/backoffice/ulocate/ImportExportApi/ImportLocationsCSV?FileName=' + name).then(function (response) {
                if (response.data) {
                    console.info(response.data);
                    return response.data;
                } else {
                    return false;
                }
            });
        };

        /**
         * @ngdoc method
         * @name importFile
         * @function
         * 
         * @param {file} file - File object acquired via File Upload API.
         * @description - Upload a file to the server.
         */
        fileApiFactory.uploadFileToServer = function (file) {
            var request = {
                file: file
            };
            return $http({
                method: 'POST',
                url: "/umbraco/backoffice/ulocate/ImportExportApi/UploadFileToServer",
                // TODO: Change Content-Type to undefined in Umbraco 7.5 (or whenever the Angular version is bumped to 1.2 or higher)
                headers: { 'Content-Type': false },
                transformRequest: function(data) {
                    var formData = new FormData();
                    formData.append("file", data.file);
                    return formData;
                },
                //Create an object that contains the model and files which will be transformed in the above transformRequest method
                data: request
            }).then(function (response) {
                if (response) {
                    var fileName = response.data.split('"').join('');
                    fileName = fileName.split(',')[1];
                    fileName = fileName.split('\\\\').join('/');
                    fileName = fileName.split('\App_Data')[1];
                    fileName = '~/App_Data' + fileName;
                    console.info(fileName);
                    return fileName;
                } else {
                    return false;
                }
            });
        };

        /**
         * @ngdoc method
         * @name downCaseProperties
         * @function
         * 
         * @param {object} object - Any object.
         * @description - Converts CamelCase properties to camelCase properties.
         */
        fileApiFactory.downCaseProperties = function (object) {
            var newObject = {};
            for (var prop in object) {
                if (object.hasOwnProperty(prop)) {
                    var propertyName = prop;
                    var propertyValue = object[prop];
                    var newPropertyName = propertyName.charAt(0).toLowerCase() + propertyName.slice(1);
                    if ((typeof propertyValue) === "object") {
                        propertyValue = fileApiFactory.downCaseProperties(propertyValue);
                    }
                    newObject[newPropertyName] = propertyValue;
                }
            };
            return newObject;
        };

        return fileApiFactory;

    };

    angular.module('umbraco.resources').factory('uLocateFileApiService', ['$http', '$q', uLocate.Services.FileApiService]);

}(window.uLocate.Services = window.uLocate.Services || {}));
