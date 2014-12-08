(function(uLocateServices) {

    uLocateServices.MapService = function ($http, $q) {

        var mapFactory = {};
        mapFactory.markers = [];
        mapFactory.geocoder = false;

        /**
         * @ngdoc method
         * @name addMarker
         * @function
         * 
         * @param {google.maps.Map} map - The map object to add the marker to.
         * @param {[number, number]} coord - The [lat, lng] coordinates of the marker.
         * @param {uLocate.Models.MapMarkerOptions} options - Optional details for the marker's style and popup.
         * @param {function} callback - An optional callback function when the marker is clicked on.
         * @returns {integer} - The index of the marker created in the mapFactory.markers array.
         * @description - Add a marker to the map.
         */
        mapFactory.addMarker = function(map, coord, options, callback) {
            var setup = {
                map: map,
                position: coord,
            };
            if (options) {
                if (options.title) {
                    setup.title = options.title;
                }
                if (options.icon) {
                    setup.icon = options.icon;
                }
            }
            var markerOptions = new uLocate.Models.MapMarker(setup);
            var index = mapFactory.markers.length;
            var marker = new google.maps.Marker(markerOptions);
            mapFactory.markers.push(marker);
            return index;
        };

        /**
         * @ngdoc method
         * @name changeView
         * @function
         * 
         * @param {Google.maps.Map} map - The map to update.
         * @param {uLocate.Models.MapView} options - The directions on how to change the view.
         * @description - Changes the view of the map.
         */
        mapFactory.changeView = function (map, options) {
            options = new uLocate.Models.MapView(options);
            if (options.coordinates) {
                var coord = new google.maps.LatLng(options.coordinates[0], options.coordinates[1]);
                if (options.smoothAnimation) {
                    map.panTo(coord);
                } else {
                    map.setCenter(coord);
                }
            }
            if (options.zoom) {
                map.setZoom(options.zoom);
            }
        };

        /**
         * @ngdoc method
         * @name deleteAllMarkers
         * @function
         * 
         * @description - Deletes all markers.
         */
        mapFactory.deleteAllMarkers = function () {
            for (var i = (mapFactory.markers.length - 1) ; i > -1; i--) {
                mapFactory.deleteMarker(i);
            }
        };

        /**
         * @ngdoc method
         * @name deleteMarker
         * @function
         * 
         * @param {integer} index - The index of the desired marker to delete.
         * @description - Deletes a marker.
         */
        mapFactory.deleteMarker = function (index) {
            mapFactory.hideMarker(index);
            mapFactory.markers.splice(index, 1);
        };

        /**
         * @ngdoc method
         * @name fitBoundsToMarkers
         * @function
         * 
         * @param {google.maps.Map} map - The map to bind to the markers.
         * @param {array of google.maps.Marker} - Optional array of markers to bind to. If not included, uses mapFactory.markers.
         * @description - Centers the map's view to fit all the markers provided.
         */
        mapFactory.fitBoundsToMarkers = function(map, markers) {
            if (!markers) {
                markers = mapFactory.markers;
            }
            var bounds = new google.maps.LatLngBounds();
            _.each(markers, function(marker) {
                if (marker.getVisible()) {
                    bounds.extend(marker.getPosition());
                }
            });
            map.fitBounds(bounds);
            if (map.getZoom() > 15) {
                map.setZoom(15);
            }
        };

        /**
         * @ngdoc method
         * @name geocode
         * @function
         * 
         * @param {string} address - The address to geocode
         * @returns {[number, number]} - The [lat,lng] of the address.
         * @description - Returns the coordinates for the adddress. A promise wrapper for google's geocoder.
         */
        mapFactory.geocode = function (address) {
            var deferred = $q.defer();
            if (!mapFactory.geocoder) {
                mapFactory.geocoder = new google.maps.Geocoder();
            }
            mapFactory.geocoder.geocode({ 'address': address }, function (results, status) {
                var response = false;
                if (status == google.maps.GeocoderStatus.OK) {
                    response = results[0].geometry.location;
                }
                deferred.resolve(response);
            });
            return deferred.promise;
        }

        /**
         * @ngdoc method
         * @name getAllMarkers
         * @function
         * 
         * @returns {array of google.maps.Marker} - The markers returned.
         * @description - Returns all markers in mapFactory.markers array.
         */
        mapFactory.getAllMarkers = function () {
            return mapFactory.markers;
        };

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
         * @name hideAllMarkers
         * @function
         * 
         * @description - Hides all markers.
         */
        mapFactory.hideAllMarkers = function() {
            for (var i = (mapFactory.markers.length - 1) ; i > -1; i--) {
                mapFactory.hideMarker(i);
            }
        };

        /**
         * @ngdoc method
         * @name hideMarker
         * @function
         * 
         * @param {integer} index - The index of the desired marker to hide.
         * @description - Hides a marker.
         */
        mapFactory.hideMarker = function (index) {
            mapFactory.markers[index].setMap(null);
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
            mapOptions = new uLocate.Models.MapOptions(options);
            var element = document.querySelector(elem);
            var map = new google.maps.Map(element, mapOptions);
            return map;
        };


        /**
         * @ngdoc method
         * @name showAllMarkers
         * @function
         * 
         * @param {google.maps.Map} - The map to show the markers on.
         * @description - Shows all markers.
         */
        mapFactory.showAllMarkers = function (map) {
            for (var i = (mapFactory.markers.length - 1) ; i > -1; i--) {
                mapFactory.showMarker(i, map);
            }
        };

        /**
         * @ngdoc method
         * @name showMarker
         * @function
         * 
         * @param {integer} index - The index of the desired marker to show.
         * @param {google.maps.Map} map - The map to show the marker on.
         * @description - Shows a marker.
         */
        mapFactory.showMarker = function(index, map) {
            mapFactory.markers[index].setMap(map);
        };

        return mapFactory;

    };

    angular.module('umbraco.resources').factory('uLocateMapService', ['$http', '$q', uLocate.Services.MapService]);

}(window.uLocate.Services = window.uLocate.Services || {}));