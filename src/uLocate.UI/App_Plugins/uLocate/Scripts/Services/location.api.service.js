(function(uLocateServices) {

	uLocateServices.LocationApiService = function ($http, $q) {

		var locationApiFactory = {};

	    /**
         * @ngdoc method
         * @name deleteLocation
         * @function
         * 
         * @param {integer} id - ID of the location.
         * @returns {object}
         * @description - Delete indicated location.
         */
		locationApiFactory.deleteLocation = function (id) {
		    var config = { params: { id: id } };
	        // TODO: Change out the url in this line for the live one below when not using mocks.
	        return $http.get('/App_Plugins/uLocate/Scripts/ApiMocks/delete.location.js', config).then(function(response) {
	            if (response.data) {
	                var data = locationApiFactory.downCaseProperties(response.data);
	                return data;
	            } else {
	                return false;
	            }
	        });
	    };

	    /**
         * @ngdoc method
         * @name getAllLocations
         * @function
         * 
         * @param {integer} id - The id of the desired location.
         * @returns {uLocate.Models.Location} - Location retrieved.
         * @description - Get a specific location.
         */
	    locationApiFactory.getLocation = function(id) {
	        var config = { params: { id: id } };
	        // TODO: Change out the url in this line for the live one below when not using mocks.
	        return $http.get('/App_Plugins/uLocate/Scripts/ApiMocks/get.location.js', config).then(function (response) {
	            if (response.data) {
	                var data = locationApiFactory.downCaseProperties(response.data);
	                return data;
	            } else {
	                return false;
	            }
	        });
	    };

	    /**
         * @ngdoc method
         * @name getAllLocations
         * @function
         * 
         * @param {uLocate.Models.GetLocationsApiRequest} options - The desired options for the request.
         * @returns {array of uLocate.Models.Location} - List of locations.
         * @description - Get all the locations that match the options.
         */
		locationApiFactory.getAllLocations = function (options) {
		    var request;
		    if (!options) {
		        request = new uLocate.Models.GetLocationsApiRequst();
		    } else {
		        request = new uLocate.Models.GetLocationsApiRequst(options);
		    }
		    // TODO: Change out this line for the one below it when not using mocks.
		    /*return $http.post('', request).then(function (response) {*/
		    return $http.get('/App_Plugins/uLocate/Scripts/ApiMocks/get.locations.js').then(function(response) {
			    if (response.data) {
			    	var data = locationApiFactory.downCaseProperties(response.data);
			        return data;
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
	    locationApiFactory.downCaseProperties = function(object) {
	    	var newObject = {};
	    	for (var prop in object) {
	    		if (object.hasOwnProperty(prop)) {
	    			var propertyName = prop;
	    			var propertyValue = object[prop];
	    			var newPropertyName = propertyName.charAt(0).toLowerCase() + propertyName.slice(1);
	    			if ((typeof propertyValue) === "object") {
	    				propertyValue = locationApiFactory.downCaseProperties(propertyValue);
	    			}
	    			newObject[newPropertyName] = propertyValue;
	    		}
	    	};
	    	return newObject;
	    };

	    return locationApiFactory;

	};

	angular.module('umbraco.resources').factory('uLocateLocationApiService', ['$http', '$q', uLocate.Services.LocationApiService]);

}(window.uLocate.Services = window.uLocate.Services || {}));
