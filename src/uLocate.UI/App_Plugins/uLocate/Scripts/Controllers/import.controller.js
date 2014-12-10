(function (controllers, undefined) {

    controllers.ImportController = function ($scope, $routeParams, treeService, assetsService, dialogService, navigationService, notificationsService) {

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
            $scope.selectedView = $routeParams.id;
        };

        /**
         * @ngdoc method
         * @name setVariables
         * @function
         * 
         * @description - Sets the initial state for $scope variables.
         */
        $scope.setVariables = function () {
            $scope.selectedView = $routeParams.id;
            $scope.currentNode = false;
            $scope.openMenu = false;
            $scope.getCurrentNode();
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

    app.requires.push('angularFileUpload');

    angular.module('umbraco').controller('uLocate.Controllers.ImportController', ['$scope', '$routeParams', 'treeService', 'assetsService', 'dialogService', 'navigationService', 'notificationsService', uLocate.Controllers.ImportController]);

}(window.uLocate.Controllers = window.uLocate.Controllers || {}));