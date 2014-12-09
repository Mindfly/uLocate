(function (controllers, undefined) {

    controllers.DeleteConfirmationDialogController = function ($scope, uLocateBroadcastService, navigationService) {

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

        /**
         * @ngdoc method
         * @name confirm
         * @function
         * 
         * @description - When user confirms dialog, fire message to broadcast service and close dialog.
         */
        $scope.confirm = function () {
            uLocateBroadcastService.sendMessage($scope.currentNode.deleteChannel, $scope.currentNode.deleteId);
            $scope.nav.hideNavigation();
        };

        /*-------------------------------------------------------------------
         * Helper Methods
         * ------------------------------------------------------------------*/

        $scope.isCurrentNodeALocation = function () {
            var result = false;
            if ($scope.currentNode instanceof uLocate.Models.Location) {
                result = true;
            }
            return result;
        };

        /*-------------------------------------------------------------------*/

        $scope.init();
    };

    angular.module('umbraco').controller('uLocate.Controllers.DeleteConfirmationDialogController', ['$scope', 'uLocateBroadcastService', 'navigationService', uLocate.Controllers.DeleteConfirmationDialogController]);


}(window.uLocate.Controllers = window.uLocate.Controllers || {}));