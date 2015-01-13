(function (uLocateServices) {

    uLocateServices.DataTypeApiService = function ($http) {

        var dataTypeApiFactory = {};

        /**
         * @ngdoc method
         * @name getAllDataTypes
         * @function
         * 
         * @returns {Array of {id: 'x', name: 'x'} pairs} - The data types.
         * @description - Get a list of all valid data types.
         */
        dataTypeApiFactory.getAllDataTypes = function () {
            return $http.get("/umbraco/backoffice/uLocate/DataTypeApi/GetAllAvailable").then(function (response) {
                if (response.data) {
                    var dataString = JSON.stringify(response.data);
                    dataString = dataString.split('{"')[1].split('"}')[0];
                    var pairs = dataString.split('","');
                    var dataTypes = [];
                    _.each(pairs, function (pair) {
                        var dataType = {
                            id: pair.split('":"')[0],
                            name: pair.split('":"')[1]
                        };
                        dataTypes.push(dataType);
                    });
                    return dataTypes;
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
        dataTypeApiFactory.downCaseProperties = function (object) {
            var newObject = {};
            for (var prop in object) {
                if (object.hasOwnProperty(prop)) {
                    var propertyName = prop;
                    var propertyValue = object[prop];
                    var newPropertyName = propertyName.charAt(0).toLowerCase() + propertyName.slice(1);
                    if ((typeof propertyValue) === "object") {
                        propertyValue = locationTypeApiFactory.downCaseProperties(propertyValue);
                    }
                    newObject[newPropertyName] = propertyValue;
                }
            };
            return newObject;
        };

        return dataTypeApiFactory;

    };

    angular.module('umbraco.resources').factory('uLocateDataTypeApiService', ['$http', uLocate.Services.DataTypeApiService]);

}(window.uLocate.Services = window.uLocate.Services || {}));
