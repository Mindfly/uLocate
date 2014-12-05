(function (controllers, undefined) {

    controllers.EditLocationDialogController = function($scope, dialogService) {

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
        $scope.setVariables = function() {
            $scope.options = {
                countries: ['Select a Country'],
                locationTypes: [],
                regions: ['Select a State']
            };
            $scope.selected = {
                country: $scope.options.countries[0],
                locationTypes: [],
                region: $scope.options.regions[0]
            };
        };

        /*-------------------------------------------------------------------
         * Event Handler Methods
         *-------------------------------------------------------------------*/

        /**
         * @ngdoc method
         * @name save
         * @function
         * 
         * @description - Make necessary location model modifications before submitting.
         */
        $scope.save = function() {
            $scope.submit();
        };

        /*-------------------------------------------------------------------
         * Helper Methods
         * ------------------------------------------------------------------*/

        /**
         * @ngdoc method
         * @name getCountries
         * @function
         * 
         * @description - Load a list of countries to populate to $scope.options.countries
         */
        $scope.getCountries = function() {
            // TODO: Wire in functionality to get a list of countries.
        };

        /**
         * @ngdoc method
         * @name getLocationTypes
         * @function
         * 
         * @description - Load a list of location types to populate to $scope.options.locationTypes;
         */
        $scope.getLocationTypes = function () {
            // TODO: Wire in functionality to get a list of location types.    
        }

        /**
         * @ngdoc method
         * @name getRegions
         * @function
         * 
         * @param {string} country - name of country to load regions for.
         * @description - Load a list of regions to populate to $scope.options.regions
         */
        $scope.getRegions = function(country) {
            // TODO: Wire in functionality to get a list of regions.
        };

        /*-------------------------------------------------------------------*/

        $scope.init();

    };

    angular.module('umbraco').controller('uLocate.Controllers.EditLocationDialogController', ['$scope', 'dialogService', uLocate.Controllers.EditLocationDialogController]);


}(window.uLocate.Controllers = window.uLocate.Controllers || {}));