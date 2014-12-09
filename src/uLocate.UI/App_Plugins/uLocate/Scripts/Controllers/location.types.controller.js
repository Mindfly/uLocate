(function(controllers, undefined) {

    controllers.LocationTypesController = function ($scope, $routeParams, treeService, assetsService, dialogService, navigationService, notificationsService) {

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
            $scope.currentNode = false;
            $scope.openMenu = false;
            $scope.getCurrentNode();
        };

        /*-------------------------------------------------------------------
         * Event Handler Methods
         *-------------------------------------------------------------------*/

        /**
         * @ngdoc method
         * @name openCreateDialog
         * @function
         * 
         * @description - Opens the Create Location Type dialog.
         */
        $scope.openCreateDialog = function () {
            navigationService.showDialog({
                node: $scope.currentNode,
                action: {
                    alias: 'create',
                    cssclass: 'add',
                    name: 'Create',
                    seperator: false,
                    metaData: {
                        actionView: '/App_Plugins/uLocate/Dialogs/create.locationType.dialog.html',
                        dialogTitle: 'Create'
                    }
                },
                section: 'uLocate'
            });
        };

        /**
         * @ngdoc method
         * @name openDeleteDialog
         * @function
         * 
         * @param {uLocate.Models.LocationType} locationType - the location type to delete.
         * @description - Opens the Delete Conirmation dialog.
         */
        $scope.openDeleteDialog = function (locationType) {
            navigationService.showDialog({
                node: locationType,
                action: {
                    alias: 'delete',
                    cssclass: 'delete',
                    name: 'Delete',
                    seperator: false,
                    metaData: {
                        actionView: '/App_Plugins/uLocate/Dialogs/delete.confirmation.dialog.html',
                        dialogTitle: 'Delete'
                    }
                },
                section: 'uLocate',
            });
        };

        $scope.openIconPicker = function() {
            dialogService.iconPicker({
                callback: populateIcon
            });
        };



        /*-------------------------------------------------------------------
         * Helper Methods
         * ------------------------------------------------------------------*/

        function populateIcon(locationType) {
            $scope.node = locationType;
            $scope.icon = locationType.icon;
        }

        /*-------------------------------------------------------------------*/

        $scope.init();

    };

    angular.module('umbraco').controller('uLocate.Controllers.LocationTypesController', ['$scope', '$routeParams', 'treeService', 'assetsService', 'dialogService', 'navigationService', 'notificationsService', uLocate.Controllers.LocationTypesController]);

}(window.uLocate.Controllers = window.uLocate.Controllers || {}));