(function(uLocateServices) {

	uLocateServices.LocationApiService = function ($http, $q) {

		var locationApiFactory = {};

		locationApiFactory.getLocations = function (page, perPage, sortBy, sortOrder) {
		    var request = {
		    	page: page,
		    	perPage: perPage,
		    	soryBy: sortBy,
				sortOrder: sortOrder
		    };
			// TODO: Change out this line for the one below it when not using mocks.
		    /*return $http.post('', request).then(function (response) {*/
		    return $http.get('/App_Plugins/Scripts/ApiMocks/get.locations.js').then(function(response) {
			    if (response.data) {
			    	var data = locationApiFactory.downCaseProperties(response.data);
			        return data;
			    } else {
			        return false;
			    }
			});
		};

	    locationApiFactory.downCaseProperties = function(object) {
	    	var newObject = {};
	    	for (var prop in object) {
	    		if (object.hasOwnProperty(prop)) {
	    			var propertyName = prop;
	    			var propertyValue = object[prop];
	    			var newPropertyName = propertyName.charAt(0).toLowerCase() + propertyName.slice(1);
	    			if ((typeof propertyValue) == "object") {
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
