(function(uLocateServices) {

    uLocateServices.MapService = function ($http, $q) {

        var mapFactory = {};

        /**
         * @ngdoc method
         * @name geocode
         * @function
         * 
         * @param {string} address - a string representation of an address (e.g. '114 W Magnolia Street, Bellingham, WA 98225').
         * @returns {[number, number]} - an object containing the [lat, lng] for the address provided.
         * @description - Return lat/lng coordinates of provided address via promise.
         */
        mapFactory.geocode = function (address) {
            var coord;
            var deferred = $q.defer();
            var geocoder = new google.maps.Geocoder();
            geocoder.geocode({ 'address': address }, function (results, status) {
                if (status === google.maps.GeocoderStatus.OK) {
                    var location = results[0].geometry.location;
                    coord = [location.lat() * 1, location.lng() * 1];
                    deferred.resolve(coord);
                } else {
                    deferred.resolve(false);
                }
            });
            return deferred.promise;
        };

        /**
         * @ngdoc method
         * @name loadMap
         * @function
         * 
         * @param {string} elem - The CSS selector for an HTML element.
         * @param {uLocate.Models.MapOptions} options - The options for configuring the map.
         * @returns {google.maps.Map} - The Google Map that was created.
         * @description - Load and initialize a Google map.
         */
        mapFactory.loadMap = function (elem, options) {
            var mapOptions;
            if (!options) {
                options = {
                    center: {
                        latitude: 0,
                        longitude: 0
                    },
                    zoom: 15
                };
            }
            console.info(options);
            mapOptions = new uLocate.Models.MapOptions(options);
            console.info(mapOptions);
            var element = document.querySelector(elem);
            var map = new google.maps.Map(element, mapOptions);
            return map;
        };

        return mapFactory;

    };

    angular.module('umbraco.resources').factory('uLocateMapService', ['$http', '$q', uLocate.Services.MapService]);

}(window.uLocate.Services = window.uLocate.Services || {}));