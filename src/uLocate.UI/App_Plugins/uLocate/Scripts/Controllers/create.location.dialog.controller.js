(function (controllers, undefined) {

    controllers.CreateLocationDialogController = function ($scope, uLocateLocationTypeApiService) {

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
            $scope.setVariables();
        };

        /**
         * @ngdoc method
         * @name setVariables
         * @function
         * 
         * @description - Sets the initial state for $scope variables.
         */
        $scope.setVariables = function () {
            $scope.defaultKey = uLocate.Constants.DEFAULT_LOCATION_TYPE_KEY;
            $scope.locationName = '';
            $scope.locationTypes = uLocate.Constants.LOCATION_TYPES;
            if ($scope.locationTypes.length < 1) {
                $scope.getLocationTypes().then(function (locationTypes) {
                    $scope.locationTypes = locationTypes;
                });
            }
            $scope.showLocationError = false;
        };

        /*-------------------------------------------------------------------
         * Event Handler Methods
         *-------------------------------------------------------------------*/

        /**
         * @ngdoc method
         * @name confirm
         * @function
         * 
         * @description - Open the Create Location view with the name and key provided.
         */
        $scope.confirm = function (name, key) {
            if (name !== '') {
                $scope.nav.hideNavigation();
                window.location = '/umbraco/#/uLocate/uLocate/locations/create?&name=' + name + '&key=' + key;
            } else {
                $scope.showLocationError = true;
            }
        };

        /*-------------------------------------------------------------------
         * Helper Methods
         * ------------------------------------------------------------------*/

        /**
         * @ngdoc method
         * @name getLocationTypes
         * @function
         * 
         * @description - Gets all location types from the API, if not already stored in constants.
         */
        $scope.getLocationTypes = function () {
            var promise = uLocateLocationTypeApiService.getAllLocationTypes();
            return promise.then(function (response) {
                var locationTypes = _.map(response, function (locationType) {
                    return new uLocate.Models.LocationType(locationType);
                });
                uLocate.Constants.LOCATION_TYPES = locationTypes;
                return uLocate.Constants.LOCATION_TYPES;
            });
        };

        /*-------------------------------------------------------------------*/

        $scope.init();
    };

    angular.module('umbraco').controller('uLocate.Controllers.CreateLocationeDialogController', ['$scope', 'uLocateLocationTypeApiService', uLocate.Controllers.CreateLocationDialogController]);


}(window.uLocate.Controllers = window.uLocate.Controllers || {}));