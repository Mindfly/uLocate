(function(uLocateServices) {

	uLocateServices.ManagementApiService = function ($http) {

	    var managementApiFactory = {};

	    /**
         * @ngdoc method
         * @name reindexExamine
         * @function
         * 
         * @returns {bool}
         * @description - Reindex examine.
         */
	    managementApiFactory.reindexExamine = function () {
	        return $http.get('/umbraco/backoffice/uLocate/MaintenanceApi/ReIndexAll/').then(function (response) {
	            return response.data;
	        });
	    };


	    return managementApiFactory;

	};

	angular.module('umbraco.resources').factory('uLocateManagementApiService', ['$http', uLocate.Services.ManagementApiService]);

}(window.uLocate.Services = window.uLocate.Services || {}));
