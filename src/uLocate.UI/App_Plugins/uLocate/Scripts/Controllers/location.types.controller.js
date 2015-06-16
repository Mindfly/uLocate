(function(controllers, undefined) {

    controllers.LocationTypesController = function ($scope, $location, $routeParams, treeService, assetsService, dialogService, navigationService, notificationsService, uLocateBroadcastService, uLocateDataTypeApiService, uLocateInitializationApiService, uLocateLocationTypeApiService) {

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
            uLocateInitializationApiService.initDatabaseIfNeeded().then(function() {
                if ($scope.selectedView === 'view') {
                    $scope.addDeleteLocationTypeListener();
                }
                $scope.setVariables();
                $scope.getLocationTypesIfNeeded();
                $scope.getDataTypesIfNeeded();
            });
        };

        /**
         * @ngdoc method
         * @name addDeleteLocationTypeListener
         * @function
         * 
         * @description - If a broadcast event in the deleteLocation channel is dictated, retrieve the message stored and use it to delete a location.
         */
        $scope.addDeleteLocationTypeListener = function () {
            $scope.$on('deleteLocationType', function () {
                var id = uLocateBroadcastService.message;
                $scope.deleteLocationType(id);
            });
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
         * @name getDataTypesIfNeeded
         * @function
         * 
         * @description - Load a list of available data types. Once done, see if a location type needs to be loaded in the editor.
         */
        $scope.getDataTypesIfNeeded = function() {
            if ($scope.selectedView == 'edit') {
                var promise = uLocateDataTypeApiService.getAllDataTypes();
                promise.then(function (response) {
                    console.info(response);
                    _.each(response, function(dataType) {
                        $scope.options.type.push({
                            id: dataType.id,
                            name: dataType.name
                        });
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
                    $scope.locationTypes = _.map(response, function (locationType) {
                        return new uLocate.Models.LocationType(locationType);
                    });
                    uLocate.Constants.LOCATION_TYPES = $scope.locationTypes;
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
                        var newLocationType = new uLocate.Models.LocationType(response);
                        _.each(newLocationType.properties, function (property) {
                            property.selectedType = $scope.options.type[0];
                            _.each($scope.options.type, function (dataType) {
                                if (dataType.id == property.propType) {
                                    property.selectedType = dataType;
                                }
                            });
                        });
                        $scope.newLocationType = newLocationType;
                        if ($scope.newLocationType.properties.length < 1) {
                            $scope.addNewProperty();
                        }
                    });
                } else if (($location.search()).name) {
                    $scope.newLocationType.name = ($location.search()).name;
                    $scope.newLocationType.properties[0].selectedType = $scope.options.type[0];
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
                type: [{id: 0, name: 'Select Data Type'}]
            };
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
         * @name flagPropertyAliasAsEdited
         * @function
         * 
         * @param {uLocate.Models.Property} property - the property
         * @description - Flag the property's alias as edited.
         */
        $scope.flagPropertyAliasAsEdited = function(property) {
            property.hasAliasBeenEdited = true;
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
            var currentNode = locationType;
            currentNode.deleteId = locationType.key;
            currentNode.deleteChannel = 'deleteLocationType';
            navigationService.showDialog({
                node: currentNode,
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

        /**
         * @ngdoc method
         * @name saveNewLocationType
         * @function
         * 
         * @param {uLocate.Models.LocationType} locationType
         * @description - Saves a new LocationType via API.
         */
        $scope.saveNewLocationType = function (locationType) {
            var mayProceed = true;
            var isPropertyError = false;
            if (locationType.icon === '') {
                mayProceed = false;
            }
            _.each(locationType.properties, function(property) {
                if (property.propName === '') {
                    mayProceed = false;
                    isPropertyError = true;
                }
                if (property.propAlias === '') {
                    mayProceed = false;
                    isPropertyError = true;
                } else {
                    property.propAlias = property.propAlias.split(' ').join('');
                }
                if (property.selectedType) {
                    if (property.selectedType.id === 0) {
                        mayProceed = false;
                        isPropertyError = true;
                    } else {
                        property.isDefaultProp = false;
                        property.propType = property.selectedType.id;
                    }
                } else {
                    mayProceed = false;
                    isPropertyError = true;
                }
            });
            if (mayProceed) {
                if ((($location.search()).name)) {
                    var promise = uLocateLocationTypeApiService.createLocationType(locationType.name);
                    promise.then(function(guid) {
                        if (guid) {
                            locationType.key = guid;
                            $scope.updateLocationType(locationType);
                        }
                    });
                } else {
                    $scope.updateLocationType(locationType);
                }
            } else {
                if (isPropertyError) {
                    notificationsService.error('All properties must have names, aliases, and a selected data type.');
                } else {
                    notificationsService.error('The location type requires an icon.');
                }
            }
        };

        $scope.updateLocationType = function (type) {
            var promise = uLocateLocationTypeApiService.updateLocationType(type);
            promise.then(function (response) {
                if (response) {
                    notificationsService.success('Location type "' + type.name + '" saved.');
                    window.location = '#/uLocate/uLocate/locationTypes/view';
                } else {
                    notificationsService.success('Attempt to save location type "' + type.name + '" failed.');
                }
            });
        };

        /*-------------------------------------------------------------------
         * Helper Methods
         * ------------------------------------------------------------------*/

        /**
        * @ngdoc method
        * @name deleteLocationType
        * @function
        * 
        * @param {string} key
        * @description - Deletes a location type, then redirect to viewing all location types.
        */
        $scope.deleteLocationType = function (key) {
            if (key) {
                var promise = uLocateLocationTypeApiService.deleteLocationType(key);
                promise.then(function (response) {
                    console.info(response);
                    if (response.success) {
                        notificationsService.success("Location type successfully deleted.");
                        $scope.getLocationTypesIfNeeded();
                    } else {
                        notificationsService.error("Attempt to delete location type failed.");
                    }
                }, function (reason) {
                    notificationsService.error("Attempt to delete location type failed.", reason.message);
                });
            }
        };

        /**
         * @ngdoc method
         * @name isDefaultLocationType
         * @function
         * 
         * @param {uLocate.Models.LocationType} type
         * @returns {boolean}
         * @description - Returns true if the location type's key matches the default location type's key.
         */
        $scope.isDefaultLocationType = function(type) {
            var result = false;
            if (type.key === uLocate.Constants.DEFAULT_LOCATION_TYPE_KEY) {
                result = true;
            }
            return result;
        };

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

    angular.module('umbraco').controller('uLocate.Controllers.LocationTypesController', ['$scope', '$location', '$routeParams', 'treeService', 'assetsService', 'dialogService', 'navigationService', 'notificationsService', 'uLocateBroadcastService', 'uLocateDataTypeApiService', 'uLocateInitializationApiService', 'uLocateLocationTypeApiService', uLocate.Controllers.LocationTypesController]);

}(window.uLocate.Controllers = window.uLocate.Controllers || {}));