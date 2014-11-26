(function(controllers, undefined) {

    controllers.LocationsController = function($scope, assetsService, dialogService, uLocateMapService, notificationsService) {

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
            console.info('Controller loaded.');
            $scope.setVariables();
            $scope.loadGoogleMapAsset();
        };

        $scope.loadGoogleMapAsset = function() {
            assetsService.loadJs("//www.google.com/jsapi").then(function () {
                google.load('maps', '3', {
                    callback: $scope.loadMap,
                    other_params: 'sensor=false'
                });
            });
        };

        $scope.loadMap = function () {
            var options = {
                center: {
                    latitude: 48,
                    longitude: -122
                },
                zoom: 12
            };
            $scope.map = uLocateMapService.loadMap('#location-map', options);
            $scope.map.setOptions({ styles: $scope.mapStyles });
        };

        $scope.setVariables = function() {
            $scope.map = null;
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

    angular.module('umbraco').controller('uLocate.Controllers.LocationsController', ['$scope', 'assetsService', 'dialogService', 'uLocateMapService', 'notificationsService', uLocate.Controllers.LocationsController]);

}(window.uLocate.Controllers = window.uLocate.Controllers || {}));