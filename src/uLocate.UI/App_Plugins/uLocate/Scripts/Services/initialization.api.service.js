(function (uLocateServices) {

    uLocateServices.InitializationApiService = function ($http) {

        var initializationApiFactory = {};

        /**
         * @ngdoc method
         * @name initDatabaseIfNeeded
         * @function
         * 
         * @returns {}
         * @description - Check if the uLocate database has been initialized. If it has not, then do so.
         */
        initializationApiFactory.initDatabaseIfNeeded = function () {
            return $http.get('/umbraco/backoffice/uLocate/InitializationApi/DbTestInit').then(function (response) {
                if (response.data) {
                    return response.data;
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
        initializationApiFactory.downCaseProperties = function (object) {
            var newObject = {};
            for (var prop in object) {
                if (object.hasOwnProperty(prop)) {
                    var propertyName = prop;
                    var propertyValue = object[prop];
                    var newPropertyName = propertyName.charAt(0).toLowerCase() + propertyName.slice(1);
                    if ((typeof propertyValue) === "object") {
                        propertyValue = initializationApiFactory.downCaseProperties(propertyValue);
                    }
                    newObject[newPropertyName] = propertyValue;
                }
            };
            return newObject;
        };

        return initializationApiFactory;

    };

    angular.module('umbraco.resources').factory('uLocateInitializationApiService', ['$http', uLocate.Services.InitializationApiService]);

}(window.uLocate.Services = window.uLocate.Services || {}));
