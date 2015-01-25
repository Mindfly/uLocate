(function(uLocateServices) {

	uLocateServices.LocationApiService = function ($http, $q) {

	    var locationApiFactory = {};

	    /**
         * @ngdoc method
         * @name createLocation
         * @function
         * 
         * @param {string} name - The name of the location
         * @returns {uLocate.Models.Location}
         * @description - Creates a new location with the provided name.
         */
	    locationApiFactory.createLocation = function (name) {
	        var config = { params: { locationName: name} };
	        return $http.get('/umbraco/backoffice/uLocate/LocationApi/Create', config).then(function (response) {
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
         * @name deleteLocation
         * @function
         * 
         * @param {string} key - GUID of the location.
         * @returns {object}
         * @description - Delete indicated location.
         */
		locationApiFactory.deleteLocation = function (key) {
		    var config = { params: { key: key } };
	        // TODO: Change out the url in this line for the live one below when not using mocks.
		    return $http.get('/umbraco/backoffice/uLocate/LocationApi/Delete', config).then(function (response) {
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
         * @name getLocation
         * @function
         * 
         * @returns {array of uLocate.Models.Location} - Locations retrieved.
         * @description - Get a specific location.
         */
		locationApiFactory.getAllLocations = function () {
		    console.info("In the proper location API getAllLocations function.");
		    return $http.get('/umbraco/backoffice/uLocate/LocationApi/GetAll').then(function (response) {
		        if (response.data) {
		            var data = _.map(response.data, function (location) {
		                console.info(locationApiFactory.downCaseProperties(location));
		                return new uLocate.Models.Location(locationApiFactory.downCaseProperties(location));
		            });
		            console.info(data);
		            return data;
		        } else {
		            return false;
		        }
		    });
		};

	    /**
         * @ngdoc method
         * @name getLocation
         * @function
         * 
         * @param {string} key - The GUID of the desired location.
         * @returns {uLocate.Models.Location} - Location retrieved.
         * @description - Get a specific location.
         */
	    locationApiFactory.getLocation = function(key) {
	        var config = { params: { key: key } };
	        return $http.get('/umbraco/backoffice/uLocate/LocationApi/GetByKey', config).then(function (response) {
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
         * @name updateLocation
         * @function
         * 
         * @param {uLocate.Models.Location} location - The location to update.
         * @param {integer} location.id - This must match an existing location in the db.
         * @returns {object}
         * @description - Delete indicated location.
         */
		locationApiFactory.updateLocation = function (location) {
	        var request = new uLocate.Models.Location(location);
	        return $http.post('/umbraco/backoffice/uLocate/LocationTypeApi/Update', request).then(function (response) {
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
