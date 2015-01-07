(function (uLocateServices) {

    uLocateServices.LocationTypeApiService = function ($http, $q) {

        var locationTypeApiFactory = {};

        /**
         * @ngdoc method
         * @name getAllLocationTypes
         * @function
         * 
         * @returns {Array of uLocate.Models.LocationType}
         * @description - Get a list of all location types.
         */
        locationTypeApiFactory.getAllLocationTypes = function() {
            return $http.get('/umbraco/backoffice/uLocate/LocationTypeApi/GetAll').then(function(response) {
                if (response.data) {
                    var locationTypes = locationTypeApiFactory.downCaseProperties(response.data);
                    var data = _.map(locationTypes, function(locationType) {
                        return new uLocate.Models.LocationType(locationType);
                    });
                    return data;
                } else {
                    return false;
                }
            });
        };

        locationTypeApiFactory.getByKey = function (key) {
            var config = { params: { key: key } };
            return $http.get('/umbraco/backoffice/uLocate/LocationTypeApi/GetByKey', config).then(function (response) {
                if (response.data) {
                    var locationType = new uLocate.Models.LocationType(locationTypeApiFactory.downCaseProperties(response.data));
                    return locationType;
                } else {
                    return false;
                }
            });
        };


        locationTypeApiFactory.setupDb = function() {
            $http.get('/umbraco/backoffice/uLocate/InitializationApi/DeleteDb').then(function(response) {
                $http.get('/umbraco/backoffice/uLocate/InitializationApi/InitDb').then(function(initResponse) {
                    $http.get('/umbraco/backoffice/uLocate/TestApi/TestPopulateSomeLocationTypes').then(function(testResponse) {
                        console.info(testResponse);
                    });
                });
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
        locationTypeApiFactory.downCaseProperties = function (object) {
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

        return locationTypeApiFactory;

    };

    angular.module('umbraco.resources').factory('uLocateLocationTypeApiService', ['$http', '$q', uLocate.Services.LocationTypeApiService]);

}(window.uLocate.Services = window.uLocate.Services || {}));
