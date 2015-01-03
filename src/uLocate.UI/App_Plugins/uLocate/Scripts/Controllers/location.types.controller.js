(function(controllers, undefined) {

    controllers.LocationTypesController = function ($scope, $location, $routeParams, treeService, assetsService, dialogService, navigationService, notificationsService) {

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
         * @name getCurrentNode
         * @function
         * 
         * @description - Get the node for this page from the treeService for use with opening a create dialog later.
         */
        $scope.getCurrentNode = function () {
            var promise = treeService.getTree({ section: 'uLocate' });
            promise.then(function (tree) {
                _.each(tree.root.children, function (node) {
                    if (node.id == '1') {
                        $scope.currentNode = node;
                    }
                });
            });
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
            $scope.selectedView = $routeParams.id;
            $scope.getCurrentNode();
            $scope.icon = 'icon-store color-blue';
            $scope.newLocationType = new uLocate.Models.LocationType();
            if (($location.search()).name) {
                $scope.newLocationType.name = ($location.search()).name;
            }
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

        /**
         * @ngdoc method
         * @name isTypeNameProvided
         * @function
         * 
         * @param {string} typeName - A string that should contain a name for a location type.
         * @returns {boolean}
         * @description - Returns true if the provided string isn't empty.
         */
        $scope.isTypeNameProvided = function (typeName) {
            var result = false;
            if (typeName !== '') {
                result = true;
            }
            return result;
        };

        function populateIcon(locationType) {
            $scope.newLocationType.icon = locationType;
        }

        /*-------------------------------------------------------------------*/

        $scope.init();

    };

    angular.module('umbraco').controller('uLocate.Controllers.LocationTypesController', ['$scope', '$location', '$routeParams', 'treeService', 'assetsService', 'dialogService', 'navigationService', 'notificationsService', uLocate.Controllers.LocationTypesController]);

}(window.uLocate.Controllers = window.uLocate.Controllers || {}));