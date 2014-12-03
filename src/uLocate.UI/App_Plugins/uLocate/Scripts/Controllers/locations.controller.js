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
            //uLocateMapService.addMarker($scope.map, [48, -122], { title: 'Somewhere', icon: $scope.customMarkerIcon });
        };

        /**
         * @ngdoc method
         * @name setVariables
         * @function
         * 
         * @description - Sets the initial state for $scope variables.
         */
        $scope.setVariables = function() {
            $scope.map = null;
            $scope.mapOptions = {
                center: {
                    latitude: 48,
                    longitude: -122
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
            $scope.customMarkerIcon = new uLocate.Models.MarkerSymbolIcon({
                path: 'M0-165c-27.618 0-50 21.966-50 49.054C-50-88.849 0 0 0 0s50-88.849 50-115.946C50-143.034 27.605-165 0-165z',
                fillColor: '#ff7100',
                fillOpacity: 1,
                strokeColor: '#000000',
                strokeWeight: 2,
                scale: 1/4
            });
            $scope.mapStyles = [
                {
                    "featureType": "landscape",
                    "stylers": [
                        { "visibility": "on" },
                        { "color": "#ffffff" }
                    ]
                }, {
                    "featureType": "road",
                    "elementType": "geometry",
                    "stylers": [
                        { "visibility": "on" },
                        { "color": "#d9d9d9" }
                    ]
                }, {
                    "featureType": "poi.attraction",
                    "stylers": [
                        { "visibility": "on" },
                        { "color": "#f8f8f8" }
                    ]
                }, {
                    "featureType": "poi.business",
                    "stylers": [
                        { "visibility": "on" },
                        { "color": "#d9d9d9" }
                    ]
                }, {
                    "featureType": "poi.government",
                    "stylers": [
                        { "visibility": "on" },
                        { "color": "#f8f8f8" }
                    ]
                }, {
                    "featureType": "poi.medical",
                    "stylers": [
                        { "visibility": "on" },
                        { "color": "#f8f8f8" }
                    ]
                }, {
                    "featureType": "poi.park",
                    "stylers": [
                        { "visibility": "on" },
                        { "color": "#53a93f" },
                        { "lightness": 37 }
                    ]
                }, {
                    "featureType": "poi.place_of_worship",
                    "stylers": [
                        { "visibility": "off" }
                    ]
                }, {
                    "featureType": "poi.school",
                    "stylers": [
                        { "visibility": "on" },
                        { "color": "#f8f8f8" }
                    ]
                }, {
                    "featureType": "poi.sports_complex",
                    "stylers": [
                        { "visibility": "off" }
                    ]
                }, {
                    "featureType": "water",
                    "stylers": [
                        { "visibility": "on" },
                        { "color": "#049cdb" }
                    ]
                }, {
                    "featureType": "poi.business",
                    "elementType": "labels"
                }, {
                      
                }, {
                    "featureType": "administrative",
                    "stylers": [
                        { "visibility": "on" },
                        { "color": "#f8f8f8" }
                    ]
                }, {
                    "elementType": "labels.text.fill",
                    "stylers": [
                        { "visibility": "on" },
                        { "color": "#1d1d1d" }
                    ]
                }, {
                    "elementType": "labels.text.stroke",
                    "stylers": [
                        { "visibility": "on" },
                        { "color": "#ffffff" }
                    ]
                }, {
                      
                }
            ];
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
                template: '/App_Plugins/uLocate/Dialogs/edit.dialog.html',
                show: true,
                callback: $scope.processEditDialog,
                dialogData: dialogData
            });
        };

        $scope.openViewDialog = function () {
            console.info('clicked');
            var dialogData = {};
            dialogData.sampleItem = 'Example';
            dialogService.open({
                template: '/App_Plugins/uLocate/Dialogs/view.dialog.html',
                show: true,
                callback: $scope.openEditDialog,
                dialogData: dialogData
            });
        };

        /*-------------------------------------------------------------------
         * Helper Methods
         * ------------------------------------------------------------------*/

        $scope.processEditDialog = function(data) {
            console.info(data);
            notificationsService.success("Location edited", "This location has been successfully updated. #h5yr!");
        };

        /*-------------------------------------------------------------------*/

        $scope.init();


    };

    angular.module('umbraco').controller('uLocate.Controllers.LocationsController', ['$scope', 'assetsService', 'dialogService', 'uLocateMapService', 'uLocateLocationApiService', 'notificationsService', uLocate.Controllers.LocationsController]);

}(window.uLocate.Controllers = window.uLocate.Controllers || {}));