(function (controllers, undefined) {

    controllers.CreateLocationTypeDialogController = function ($scope) {

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
            $scope.showLocationError = false;
            $scope.typeName = '';
        };

        /*-------------------------------------------------------------------
         * Event Handler Methods
         *-------------------------------------------------------------------*/

        /**
         * @ngdoc method
         * @name confirm
         * @function
         * 
         * @description - When user confirms dialog, fire message to broadcast service and close dialog.
         */
        $scope.confirm = function () {
            if ($scope.typeName !== '') {
                $scope.nav.hideNavigation();
                window.location = '/umbraco/#/uLocate/uLocate/locationTypes/edit?name=' + $scope.typeName;
            } else {
                $scope.showLocationError = true;
            }
        };

        /*-------------------------------------------------------------------
         * Helper Methods
         * ------------------------------------------------------------------*/

        /*-------------------------------------------------------------------*/

        $scope.init();
    };

    angular.module('umbraco').controller('uLocate.Controllers.CreateLocationTypeDialogController', ['$scope', uLocate.Controllers.CreateLocationTypeDialogController]);


}(window.uLocate.Controllers = window.uLocate.Controllers || {}));