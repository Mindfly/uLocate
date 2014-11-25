(function(controllers, undefined) {

    controllers.LocationsController = function($scope, dialogService, notificationsService) {

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
        };

        /*-------------------------------------------------------------------
         * Event Handler Methods
         *-------------------------------------------------------------------*/

        $scope.openExampleDialog = function () {
            console.info('clicked');
            var dialogData = {};
            dialogData.sampleItem = 'Example';
            dialogService.open({
                template: '/App_Plugins/uLocate/Dialogs/example.dialog.html',
                show: true,
                callback: $scope.processExampleDialog,
                dialogData: dialogData
            });
        };

        /*-------------------------------------------------------------------
         * Helper Methods
         * ------------------------------------------------------------------*/

        $scope.processExampleDialog = function(data) {
            console.info(data);
        };

        /*-------------------------------------------------------------------*/

        $scope.init();


    };

    angular.module('umbraco').controller('uLocate.Controllers.LocationsController', ['$scope', 'dialogService', 'notificationsService', uLocate.Controllers.LocationsController]);

}(window.uLocate.Controllers = window.uLocate.Controllers || {}));