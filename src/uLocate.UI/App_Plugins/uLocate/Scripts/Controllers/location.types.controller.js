(function(controllers, undefined) {

    controllers.LocationTypesController = function ($scope, $location, $routeParams, treeService, assetsService, dialogService, navigationService, notificationsService, uLocateDataTypeApiService, uLocateLocationTypeApiService) {

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
            var promise = uLocateLocationTypeApiService.getEmptyLocationType();
            promise.then(function(response) {
                console.info(response);
            });
            $scope.setVariables();
            $scope.getLocationTypesIfNeeded();
            $scope.getDataTypesIfNeeded();
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

        $scope.getDataTypesIfNeeded = function() {
            if ($scope.selectedView == 'edit') {
                var promise = uLocateDataTypeApiService.getAllDataTypes();
                promise.then(function (response) {
                    $scope.options.type = _.map(response, function(dataType) {
                        return {
                            id: dataType.id,
                            name: dataType.name
                        };
                    });
                    $scope.getLocationTypeToEditByKey();
                });
            }
        };

        /**
         * @ngdoc method
         * @name getLocationTypesIfNeeded
         * @function
         * 
         * @description - Get a list of location types from the API if the page is in "view" mode.
         */
        $scope.getLocationTypesIfNeeded = function() {
            if ($scope.selectedView == 'view') {
                var promise = uLocateLocationTypeApiService.getAllLocationTypes();
                promise.then(function(response) {
                    $scope.locationTypes = _.map(response, function(locationType) {
                        return new uLocate.Models.LocationType(locationType);
                    });
                });
            }
        };

        /**
         * @ngdoc method
         * @name getLocationTypeToEditByKey
         * @function
         * 
         * @description - If on edit mode, and a key was provided, load the associated location type from the API. Otherwise, if a name was provided, populate the name.
         */
        $scope.getLocationTypeToEditByKey = function () {
            if ($scope.selectedView == 'edit') {
                if (($location.search()).key) {
                    var key = ($location.search()).key;
                    var promise = uLocateLocationTypeApiService.getByKey(key);
                    promise.then(function(response) {
                        $scope.newLocationType = new uLocate.Models.LocationType(response);
                        _.each($scope.newLocationType.properties, function (property) {
                            _.each($scope.options.type, function(dataType, index) {
                                if (dataType.id == property.propType) {
                                    property.selectedType = $scope.options.type[index];
                                }
                            });
                        });
                });
                } else if (($location.search()).name) {
                    $scope.newLocationType.name = ($location.search()).name;
                }
            }
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
            $scope.options = {
                type: [{id: 0, name: 'null'}]
            };
            $scope.selectedView = $routeParams.id;
            $scope.getCurrentNode();
            $scope.icon = 'icon-store color-blue';
            $scope.locationTypes = [];
            $scope.newLocationType = new uLocate.Models.LocationType();
        };

        /*-------------------------------------------------------------------
         * Event Handler Methods
         *-------------------------------------------------------------------*/

        /**
         * @ngdoc method
         * @name addNewProperty
         * @function
         * 
         * @description - Add a new property to the location type.
         */
        $scope.addNewProperty = function () {
            var newProperty = new uLocate.Models.LocationTypeProperty();
            newProperty.selectedType = $scope.options.type[0];
            $scope.newLocationType.properties.push(newProperty);
           
        };

        /**
         * @ngdoc method
         * @name deleteProperty
         * @function
         * 
         * @param {integer} index - The index of the property to delete.
         * @description - Deletes a property from the location type.
         */
        $scope.deleteProperty = function(index) {
            $scope.newLocationType.properties.splice(index, 1);
        };

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

        /**
         * @ngdoc method
         * @name openIconPicker
         * @function
         * 
         * @description - Opens the Icon Picker dialog.
         */
        $scope.openIconPicker = function() {
            dialogService.iconPicker({
                callback: populateIcon
            });
        };

        $scope.saveNewLocationType = function (locationType) {
            var promise = uLocateLocationTypeApiService.updateLocationType(locationType);
            promise.then(function(response) {
                console.info(response);
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
        $scope.isTypeNameProvided = function(typeName) {
            var result = false;
            if (typeName !== '') {
                result = true;
            }
            return result;
        };

        /**
         * @ngdoc method
         * @name updatePropertyTypeDropdown
         * @function
         * 
         * @param {uLocate.Models.LocationTypeProperty} property - A property to compare.
         * @returns {boolean}
         * @description - Returns true if the provided string isn't empty.
         */
        $scope.updatePropertyTypeDropdown = function(property) {
            var result = $scope.options.type[0];
            _.each($scope.options.type, function(option) {
                if (option.value === property.propertyEditorAlias) {
                    result = option;
                }
            });
            return result;
        };

        /**
         * @ngdoc method
         * @name populateIcon
         * @function
         * 
         * @param {string} icon - Icon class name
         * @description - Sets the icon 
         */
        function populateIcon(icon) {
            $scope.newLocationType.icon = icon;
        };

        /*-------------------------------------------------------------------*/

        $scope.init();

    };

    angular.module('umbraco').controller('uLocate.Controllers.LocationTypesController', ['$scope', '$location', '$routeParams', 'treeService', 'assetsService', 'dialogService', 'navigationService', 'notificationsService', 'uLocateDataTypeApiService','uLocateLocationTypeApiService', uLocate.Controllers.LocationTypesController]);

}(window.uLocate.Controllers = window.uLocate.Controllers || {}));