(function (uLocateServices) {

    uLocateServices.LocationTypeApiService = function ($http, umbRequestHelper) {

        var locationTypeApiFactory = {};

        locationTypeApiFactory.getAllDataTypes = function() {
            // Hack - grab DataTypes from Tree API, as `dataTypeService.getAll()` isn't implemented yet
            return umbRequestHelper.resourcePromise(
                $http.get("/umbraco/backoffice/uLocate/DataTypeApi/GetAll", { cache: true }), 'Failed to retrieve datatypes from tree service'
            );
        };

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

        /**
         * @ngdoc method
         * @name getByKey
         * @function
         * 
         * @param {string} key - GUID of desired location type.
         * @returns {uLocate.Models.LocationType}
         * @description - Get a list of all location types.
         */
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

        /**
         * @ngdoc method
         * @name updateLocationType
         * @function
         * 
         * @param {string} key - GUID of desired location type.
         * @returns {uLocate.Models.LocationType}
         * @description - Get a list of all location types.
         */
        // TODO: Test this!
        locationTypeApiFactory.updateLocationType = function (locationType) {
            var updatedLocationType = new uLocate.Models.LocationType(locationType);
            return $http.post('/umbraco/backoffice/ulocate/LocationTypeApi/Update', updatedLocationType).then(function(response) {
                if (response.data) {
                    var data = locationTypeApiFactory.downCaseProperties(response.data);
                    return data;
                } else {
                    return false;
                }
            });

        };

        // TODO: Remove this when the time comes to move live.
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

    angular.module('umbraco.resources').factory('uLocateLocationTypeApiService', ['$http', 'umbRequestHelper', uLocate.Services.LocationTypeApiService]);

}(window.uLocate.Services = window.uLocate.Services || {}));
