(function(uLocateServices) {

	uLocateServices.LocationApiService = function ($http) {

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
	    locationApiFactory.createLocation = function (name, key) {
	        var config = { params: { LocationName: name, LocationTypeGuid: key } };
	        return $http.get('/umbraco/backoffice/uLocate/LocationApi/Create', config).then(function (response) {
	            if (response.data) {
	                var data = response.data.split('"').join('');
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
		    return $http.get('/umbraco/backoffice/uLocate/LocationApi/GetAll').then(function (response) {
		        if (response.data) {
		            var data = _.map(response.data, function (location) {
		                return new uLocate.Models.Location(locationApiFactory.downCaseProperties(location));
		            });
		            return data;
		        } else {
		            return false;
		        }
		    });
		};

	    /**
         * @ngdoc method
         * @name getAllLocationsPaged
         * @function
         * 
         * @param {uLocate.Models.GetLocationsApiRequest} request
         * @returns {array of uLocate.Models.Location} - Locations retrieved.
         * @description - Get a paged list of locations.
         */
		locationApiFactory.getAllLocationsPaged = function (request) {
		    console.info("bing!");
		    request = new uLocate.Models.GetLocationsApiRequest(request);
		    if (!request.page) {
		        request.page = 0;
		    }
		    if (!request.perPage) {
		        request.perPage = 100;
		    }
		    var params = { pageNum: request.page, itemsPerPage: request.perPage };
		    console.info(request);
            if (request.sortBy || request.filter) {
                if (!request.sortBy) {
                    request.sortBy = "name";
                }
                if (!request.sortOrder) {
                    request.sortOrder = "ASC";
                }
                params.orderBy = request.sortBy;
                if (request.filter) {
                    params.searchTerm = request.filter;
                }
                params.sortOrder = request.sortOrder;
            }
		    console.info(params);
		    var config = { params: params };

		    return $http.get('/umbraco/backoffice/uLocate/LocationApi/GetAllPaged', config).then(function (response) {
		        if (response.data) {
		            var data = new uLocate.Models.LocationsPagedResponse(locationApiFactory.downCaseProperties(response.data));
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
	        return $http.post('/umbraco/backoffice/uLocate/LocationApi/Update', request).then(function (response) {
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

	angular.module('umbraco.resources').factory('uLocateLocationApiService', ['$http', uLocate.Services.LocationApiService]);

}(window.uLocate.Services = window.uLocate.Services || {}));
