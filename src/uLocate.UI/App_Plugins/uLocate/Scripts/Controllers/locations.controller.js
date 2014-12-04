(function(controllers, undefined) {

    controllers.LocationsController = function ($scope, $routeParams, assetsService, dialogService, uLocateMapService, uLocateLocationApiService, notificationsService) {

        /*-------------------------------------------------------------------
         * Initialization Methods
         * ------------------------------------------------------------------*/

        /**
         * @ngdoc method
         * @name init
         * @function
         * 
         * @description - Called when the $scope is initalized.
         */
        $scope.init = function () {
            console.info($routeParams);
            $scope.loadGoogleMapAsset();
        };

        /**
         * @ngdoc method
         * @name loadGoogleMapAsset
         * @function
         * 
         * @description - Load the Google Maps API asset, then load the applicable map starting functionality.
         */
        $scope.loadGoogleMapAsset = function() {
            assetsService.loadJs("//www.google.com/jsapi").then(function () {
                google.load('maps', '3', {
                    callback: $scope.setVariables,
                    other_params: 'sensor=false'
                });
            });
        };

        /**
         * @ngdoc method
         * @name loadMap
         * @function
         * 
         * @description - Loads the Google Map.
         */
        $scope.loadMap = function () {
            var options = $scope.mapOptions;
            $scope.map = uLocateMapService.loadMap('#location-map', options);
            $scope.map.setOptions({ styles: $scope.mapStyles });
            $scope.getLocations();
        };

        /**
         * @ngdoc method
         * @name setVariables
         * @function
         * 
         * @description - Sets the initial state for $scope variables.
         */
        $scope.setVariables = function () {
            $scope.customMarkerIcon = new uLocate.Models.MarkerSymbolIcon(uLocate.Constants.MARKER_ICON);
            $scope.filter = '';
            $scope.locations = [];
            $scope.locationsLoaded = false;
            $scope.map = null;
            $scope.mapOptions = {
                center: {
                    latitude: 0,
                    longitude: 0
                },
                zoom: 12,
                mapTypeControlOptions: {
                    position: google.maps.ControlPosition.LEFT_CENTER,
                    style: google.maps.MapTypeControlStyle.DROPDOWN_MENU
                },
                panControlOptions: {
                    position: google.maps.ControlPosition.LEFT_CENTER
                },
                streetViewControl: false,
                zoomControlOptions: {
                    style: google.maps.ZoomControlStyle.SMALL,
                    position: google.maps.ControlPosition.LEFT_CENTER
                }
            };
            $scope.mapStyles = uLocate.Constants.MAP_STYLES;
            $scope.options = {
                perPage: [25, 50, 100]
            };
            $scope.page = 0;
            $scope.perPage = 100;
            $scope.selected = {
                perPage: $scope.options.perPage[2]
        }
            $scope.sortBy = 'name';
            $scope.sortOrder = 'ascending';
            $scope.totalPages = 0;
            // Load the map now that the required variables have been assigned.
            $scope.loadMap();
        };

        /*-------------------------------------------------------------------
         * Event Handler Methods
         *-------------------------------------------------------------------*/

        /**
         * @ngdoc method
         * @name changeFilter
         * @function
         * 
         * @param {string} filter - The search filter string.
         * @description - changes the search filter and triggers getLocations();
         */
        $scope.changeFilter = function(filter) {
            $scope.filter = filter;
            $scope.page = 0;
            $scope.getLocations();
        };

        /**
         * @ngdoc method
         * @name changePage
         * @function
         * 
         * @param {integer} difference - The amount to change the current page by.
         * @description - changes the current page and triggers getLocations();
         */
        $scope.changePage = function(difference) {
            $scope.page = $scope.page + difference;
            if ($scope.page < 0) {
                $scope.page = 0;
            }
            if ($scope.page >= $scope.totalPages) {
                $scope.page = $scope.totalPages - 1;
            }
            $scope.getLocations();
        };

        $scope.openEditDialog = function () {
            console.info('clicked');
            var dialogData = {};
            dialogData.sampleItem = 'Example';
            dialogService.open({
                template: '/App_Plugins/uLocate/Dialogs/edit.location.dialog.html',
                show: true,
                callback: $scope.processEditDialog,
                dialogData: dialogData
            });
        };

        /**
         * @ngdoc method
         * @name openViewDialog
         * @function
         * 
         * @param {uLocate.Models.Location} location - The location to view.
         * @description - Opens a location view dialog.
         */
        $scope.openViewDialog = function (location) {
            var dialogData = {};
            dialogData.location = new uLocate.Models.Location(location);
            dialogService.open({
                template: '/App_Plugins/uLocate/Dialogs/view.location.dialog.html',
                show: true,
                callback: $scope.openEditDialog,
                dialogData: dialogData
            });
        };

        /**
         * @ngdoc method
         * @name updatePerPage
         * @function
         * 
         * @param {integer} perPage - The number of locations to show per page.
         * @description - Updates the amount of locations to show per page and triggers getLocations();
         */
        $scope.updatePerPage = function(perPage) {
            $scope.perPage = perPage;
            $scope.page = 0;
            $scope.getLocations();
        };

        /**
         * @ngdoc method
         * @name zoomToLocation
         * @function
         * 
         * @param {uLocate.Models.Location} location - The location to zoom to.
         * @description - Zoom to the provided location and bounce a marker if one is present.
         */
        $scope.zoomToLocation = function (location) {
            var view = new uLocate.Models.MapView({
                coordinates: [location.coordinates.latitude, location.coordinates.longitude],
                smoothAnimation: true,
                zoom: 15
            });
            uLocateMapService.changeView($scope.map, view);
            var markers = uLocateMapService.getAllMarkers();
            _.each(markers, function(marker) {
                var position = marker.getPosition();
                var lat = position.lat();
                var lng = position.lng();
                if (lat === location.coordinates.latitude && lng === location.coordinates.longitude) {
                    marker.setAnimation(google.maps.Animation.BOUNCE);
                    setTimeout(function() { marker.setAnimation(null); }, 1500);
                }
            });
        };

        /*-------------------------------------------------------------------
         * Helper Methods
         * ------------------------------------------------------------------*/

        /**
         * @ngdoc method
         * @name getLocations
         * @function
         * 
         * @description - Acquires locations via API call, using the parameters defined by the user.
         */
        $scope.addLocationMarkersToMap = function () {
            uLocateMapService.deleteAllMarkers();
            _.each($scope.locations, function(location) {
                var coord = [location.coordinates.latitude, location.coordinates.longitude];
                uLocateMapService.addMarker($scope.map, coord, { title: location.name, icon: $scope.customMarkerIcon });
            });
            uLocateMapService.fitBoundsToMarkers($scope.map);
        };

        /**
         * @ngdoc method
         * @name getLocations
         * @function
         * 
         * @description - Acquires locations via API call, using the parameters defined by the user.
         */
        $scope.getLocations = function() {
            var request = new uLocate.Models.GetLocationsApiRequst({
                filter: $scope.filter,
                page: $scope.page,
                perPage: $scope.perPage,
                sortBy: $scope.sortBy,
                sortOrder: $scope.sortOrder
            });
            var promise = uLocateLocationApiService.getLocations(request);
            promise.then(function(response) {
                $scope.locations = _.map(response.locations, function(location) {
                    return new uLocate.Models.Location(location);
                });
                $scope.page = response.page;
                $scope.perPage = response.perPage;
                _.each($scope.options.perPage, function(option, index) {
                    if (option == response.perPage) {
                        $scope.selected.perPage = $scope.options.perPage[index];
                    }
                });
                $scope.totalPages = response.totalPages;
                $scope.locationsLoaded = true;
                $scope.addLocationMarkersToMap();
            });
        };

        /**
        * @ngdoc method
        * @name hasLocations
        * @function
        * 
        * @returns {boolean}
        * @description - Returns true if the scope has at least one location.
        */
        $scope.hasLocations = function() {
            var result = false;
            if ($scope.locations) {
                if ($scope.locations.length > 0) {
                    result = true;
                }
            }
            return result;
        };

        $scope.processEditDialog = function(data) {
            console.info(data);
            notificationsService.success("Location edited", "This location has been successfully updated. #h5yr!");
        };

        /*-------------------------------------------------------------------*/

        $scope.init();


    };

    angular.module('umbraco').controller('uLocate.Controllers.LocationsController', ['$scope', '$routeParams', 'assetsService', 'dialogService', 'uLocateMapService', 'uLocateLocationApiService', 'notificationsService', uLocate.Controllers.LocationsController]);

}(window.uLocate.Controllers = window.uLocate.Controllers || {}));