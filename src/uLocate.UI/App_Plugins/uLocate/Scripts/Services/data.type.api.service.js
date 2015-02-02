(function (uLocateServices) {

    uLocateServices.DataTypeApiService = function ($http) {

        var dataTypeApiFactory = {};

        /**
         * @ngdoc method
         * @name getAllAvailable
         * @function
         * 
         * @returns {Array of {id: 'x', name: 'x'} pairs} - The data types.
         * @description - Get a list of all valid data types.
         */
        dataTypeApiFactory.getAllAvailable = function () {
            return $http.get('/umbraco/backoffice/uLocate/DataTypeApi/GetAllAvailable').then(function (response) {
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
         * @name getAllDataTypes
         * @function
         * 
         * @returns {Array of {guid: 'x', id: 'y', name: 'z'} } - The data types.
         * @description - Get a list of all valid data types with GUIDs.
         */
        dataTypeApiFactory.getAllDataTypes = function () {
            var dataTypes = [];
            var availablePromise = dataTypeApiFactory.getAllAvailable();
            var guidsPromise = dataTypeApiFactory.getAllDataTypesWithGuids();
            return availablePromise.then(function (availableTypes) {
                if (availableTypes) {
                    return guidsPromise.then(function (typesWithGuids) {
                        if (typesWithGuids) {
                            _.each(availableTypes, function (availableType) {
                                _.each(typesWithGuids, function (typeWithGuid) {
                                    if (typeWithGuid.name === availableType.name) {
                                        dataTypes.push({
                                            guid: typeWithGuid.guid,
                                            id: availableType.id,
                                            name: availableType.name
                                        });
                                    }
                                });
                            });
                            return dataTypes;
                        } else {
                            return false;
                        }
                    });
                } else {
                    return false;
                }
            });
        };

        /**
         * @ngdoc method
         * @name getAllDataTypesWithGuids
         * @function
         * 
         * @returns {Array of {guid: 'x', name: 'y'} pairs} - The data types.
         * @description - Get a list of all available data types with name and guid, via API.
         */
        dataTypeApiFactory.getAllDataTypesWithGuids = function () {
            return $http.get('/umbraco/backoffice/uLocate/DataTypeApi/GetAllDataTypesWithGuids').then(function (response) {
                if (response.data) {
                    return response.data;
                } else {
                    return false;
                }
            });
        };

        /**
         * @ngdoc method
         * @name getByName
         * @function
         * 
         * @param {string} name - the name of the data type to fetch property editor information for.
         * @description - Return property editor details for a data type
         */
        dataTypeApiFactory.getByName = function(name) {
            return $http.get('/umbraco/backoffice/uLocate/DataTypeApi/GetByName?name=' + name).then(function (response) {
                if (response.data) {
                    return response.data;
                } else {
                    return false;
                }
            });
        };

        return dataTypeApiFactory;

    };

    angular.module('umbraco.resources').factory('uLocateDataTypeApiService', ['$http', uLocate.Services.DataTypeApiService]);

}(window.uLocate.Services = window.uLocate.Services || {}));
