(function(controllers, undefined) {

    controllers.LocationsController = function ($scope, assetsService, dialogService, uLocateMapService, uLocateLocationApiService, notificationsService) {

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
        $scope.init = function() {
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
            //uLocateMapService.addMarker($scope.map, [48, -122], { title: 'Somewhere', icon: $scope.customMarkerIcon });
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
            $scope.page = 0;
            $scope.perPage = 100;
            $scope.sortBy = 'name';
            $scope.sortOrder = 'ascending';
            $scope.loadMap();
        };

        /*-------------------------------------------------------------------
         * Event Handler Methods
         *-------------------------------------------------------------------*/

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
                $scope.locations = _.map(response, function(location) {
                    return new uLocate.Models.Location(location);
                });
                $scope.addLocationMarkersToMap();
            });
        };

        /**
        * @ngdoc method
        * @name haveLocations
        * @function
        * 
        * @returns {boolean}
        * @description - Returns true if the scope has at least one location.
        */
        $scope.haveLocations = function() {
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

    angular.module('umbraco').controller('uLocate.Controllers.LocationsController', ['$scope', 'assetsService', 'dialogService', 'uLocateMapService', 'uLocateLocationApiService', 'notificationsService', uLocate.Controllers.LocationsController]);

}(window.uLocate.Controllers = window.uLocate.Controllers || {}));