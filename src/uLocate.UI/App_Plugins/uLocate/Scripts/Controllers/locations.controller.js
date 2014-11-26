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
            $scope.loadGoogleMapAsset();
        };

        $scope.loadGoogleMapAsset = function() {
            assetsService.loadJs("//maps.googleapis.com/maps/api/js?&sensor=false");
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

    angular.module('umbraco').controller('uLocate.Controllers.LocationsController', ['$scope', 'assetsService', 'dialogService', 'uLocateMapService', 'notificationsService', uLocate.Controllers.LocationsController]);

}(window.uLocate.Controllers = window.uLocate.Controllers || {}));