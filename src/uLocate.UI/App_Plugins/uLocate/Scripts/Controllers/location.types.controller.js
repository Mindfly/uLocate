(function(controllers, undefined) {

    controllers.LocationTypesController = function ($scope, $routeParams, treeService, assetsService, dialogService, navigationService, notificationsService, uLocateMapService, uLocateLocationApiService) {

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
        };

        /*-------------------------------------------------------------------
         * Event Handler Methods
         *-------------------------------------------------------------------*/

        /*-------------------------------------------------------------------
         * Helper Methods
         * ------------------------------------------------------------------*/

        /*-------------------------------------------------------------------*/

        $scope.init();

    };

    angular.module('umbraco').controller('uLocate.Controllers.LocationTypesController', ['$scope', '$routeParams', 'treeService', 'assetsService', 'dialogService', 'navigationService', 'notificationsService', 'uLocateMapService', 'uLocateLocationApiService', uLocate.Controllers.LocationTypesController]);

}(window.uLocate.Controllers = window.uLocate.Controllers || {}));