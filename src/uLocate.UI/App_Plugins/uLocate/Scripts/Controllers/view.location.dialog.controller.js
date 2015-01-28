(function (controllers, undefined) {

    controllers.ViewLocationDialogController = function($scope, dialogService) {

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
            $scope.location = $scope.dialogData.location;
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

    angular.module('umbraco').controller('uLocate.Controllers.ViewLocationDialogController', ['$scope', 'dialogService', uLocate.Controllers.ViewLocationDialogController]);


}(window.uLocate.Controllers = window.uLocate.Controllers || {}));