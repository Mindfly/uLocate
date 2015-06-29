(function(controllers, undefined) {

    controllers.LocationsController = function ($scope, $location, $q, $routeParams, treeService, assetsService, dialogService, navigationService, notificationsService, uLocateBroadcastService, uLocateDataTypeApiService, uLocateInitializationApiService, uLocateMapService, uLocateLocationApiService, uLocateLocationTypeApiService) {

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
            $scope.shouldLoadMap = false;
            $scope.selectedView = $routeParams.id;
            uLocateInitializationApiService.initDatabaseIfNeeded().then(function () {
                if ($scope.selectedView === 'view') {
                    $scope.shouldLoadMap = true;
                    $scope.addDeleteLocationListener();
                }
                $scope.loadGoogleMapAsset();
            });
        };

        /**
         * @ngdoc method
         * @name addDeleteLocationListener
         * @function
         * 
         * @description - If a broadcast event in the deleteLocation channel is dictated, retrieve the message stored and use it to delete a location.
         */
        $scope.addDeleteLocationListener = function() {
            $scope.$on('deleteLocation', function() {
                var id = uLocateBroadcastService.message;
                $scope.deleteLocation(id);
            });
        };

        /**
         * @ngdoc method
         * @name buildCountriesList
         * @function
         * 
         * @description - Build the list of countries to populate to $scope.options.countries
         */
        $scope.buildCountriesList = function () {
            _.each(uLocate.Constants.COUNTRIES, function (country) {
                var newCountry = {
                    name: country.name,
                    provinceLabel: country.provinceLabel,
                    countryCode: country.countryCode,
                    provinces: _.map(country.provinces, function (province) {
                        return {
                            name: province.name,
                            code: province.code
                        };
                    })
                };
                var provinceLabel = 'Province';
                if (newCountry.provinceLabel != '') {
                    provinceLabel = newCountry.provinceLabel;
                }
                var provinceSelector = {
                    name: 'Select a ' + provinceLabel,
                    code: ''
                };
                newCountry.provinces.unshift(provinceSelector);
                $scope.options.countries.push(newCountry);
            });
            _.each($scope.options.countries, function(country, index) {
                if (country.name == 'United States') {
                    $scope.selected.country = $scope.options.countries[index];
                    $scope.selected.region = $scope.selected.country.provinces[0];
                }
            });
        };

        /**
         * @ngdoc method
         * @name getCurrentNode
         * @function
         * 
         * @description - Get the node for this page from the treeService for use with opening a create dialog later.
         */
        $scope.getCurrentNode = function() {
            var promise = treeService.getTree({ section: 'uLocate' });
            promise.then(function (tree) {
                _.each(tree.root.children, function(node) {
                    if (node.id == '1') {
                        $scope.currentNode = node;
                    }
                });
            });
        };

        /**
         * @ngdoc method
         * @name loadGoogleMapAsset
         * @function
         * 
         * @description - Load the Google Maps API asset, then load the applicable map starting functionality.
         */
        $scope.loadGoogleMapAsset = function() {
            assetsService.loadJs("//www.google.com/jsapi").then(function () {
                google.load('maps', '3', {
                    callback: $scope.setVariables,
                    other_params: 'sensor=false'
                });
            });
        };

        /**
         * @ngdoc method
         * @name loadMap
         * @function
         * 
         * @description - Loads the Google Map.
         */
        $scope.loadMap = function () {
            var options = $scope.mapOptions;
            $scope.map = uLocateMapService.loadMap('#location-map', options);
            $scope.map.setOptions({ styles: $scope.mapStyles });
            $scope.getLocations();
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
            $scope.customMarkerIcon = new uLocate.Models.MarkerSymbolIcon(uLocate.Constants.MARKER_ICON);
            $scope.filter = '';
            $scope.form = {};
            $scope.isGeocoding = false;
            $scope.locations = [];
            $scope.locationsLoaded = false;
            $scope.locationTypes = uLocate.Constants.LOCATION_TYPES;
            $scope.newLocation = new uLocate.Models.Location();
            if (($location.search()).name) {
                $scope.newLocation.name = ($location.search()).name;
            }
            $scope.openMenu = false;
            $scope.options = {
                countries: [{ name: 'Select a Country', provinceLabel: 'Province/State', provinces: [{ name: 'Select A Province/State', code: '' }] }],
                perPage: [25, 50, 100]
            };
            $scope.page = 0;
            $scope.pages = [];
            $scope.perPage = 25;
            $scope.provinceLabel = 'Province/State';
            $scope.selected = {
                country: $scope.options.countries[0],
                perPage: $scope.options.perPage[0]
            }
            $scope.selected.region = $scope.selected.country.provinces[0];
            $scope.sortBy = 'name';
            $scope.sortOrder = "ASC";
            $scope.totalPages = 0;
            $scope.wasFormSubmitted = false;
            $scope.buildCountriesList();
            $scope.getCurrentNode();
            // Load the map now that the required variables have been assigned.
            if ($scope.selectedView === 'view') {
                $scope.map = null;
                $scope.mapOptions = {
                    center: {
                        latitude: 47.609895,
                        longitude: -122.330259
                    },
                    zoom: 3,
                    mapTypeControlOptions: {
                        position: google.maps.ControlPosition.LEFT_CENTER,
                        style: google.maps.MapTypeControlStyle.DROPDOWN_MENU
                    },
                    panControlOptions: {
                        position: google.maps.ControlPosition.LEFT_CENTER
                    },
                    streetViewControl: false,
                    zoomControlOptions: {
                        style: google.maps.ZoomControlStyle.SMALL,
                        position: google.maps.ControlPosition.LEFT_CENTER
                    }
                };
                $scope.mapStyles = uLocate.Constants.MAP_STYLES;
                $scope.loadMap();
            } else if ($scope.selectedView === 'create') {
                if ($scope.locationTypes.length < 1) {
                    $scope.getLocationTypes().then(function(locationTypes) {
                        $scope.locationTypes = locationTypes;
                        $scope.buildNewLocationEditors();
                    });
                } else {
                    $scope.buildNewLocationEditors();
                }
            }
        };

        /*-------------------------------------------------------------------
         * Event Handler Methods
         *-------------------------------------------------------------------*/

        /**
         * @ngdoc method
         * @name changeFilter
         * @function
         * 
         * @param {string} filter - The search filter string.
         * @description - changes the search filter and triggers getLocations();
         */
        $scope.changeFilter = function(filter) {
            $scope.filter = filter;
            $scope.page = 0;
            $scope.getLocations();
        };

        /**
         * @ngdoc method
         * @name changePage
         * @function
         * 
         * @param {integer} difference - The amount to change the current page by.
         * @description - changes the current page and triggers getLocations();
         */
        $scope.changePage = function (difference) {
            $scope.page = $scope.page + difference;
            if ($scope.page < 0) {
                $scope.page = 0;
            }
            if ($scope.page >= $scope.totalPages) {
                $scope.page = $scope.totalPages - 1;
            }
            $scope.getLocations();
        };

        /**
         * @ngdoc method
         * @name openCreateDialog
         * @function
         * 
         * @description - Opens the Create Location dialog.
         */
        $scope.openCreateDialog = function () {
            var currentNode = $scope.currentNode;
            currentNode.defaultKey = uLocate.Constants.DEFAULT_LOCATION_TYPE_KEY;
            navigationService.showDialog({
                node: $scope.currentNode,
                action: {
                    alias: 'create',
                    cssclass: 'add',
                    name: 'Create',
                    seperator: false,
                    metaData: {
                        actionView: '/App_Plugins/uLocate/Dialogs/create.location.dialog.html',
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
         * @param {uLocate.Models.Location} location - the location to delete.
         * @description - Opens the Delete Confirmation dialog.
         */
        $scope.openDeleteDialog = function (location) {
            var currentNode = location;
            currentNode.deleteId = location.key;
            currentNode.deleteChannel = 'deleteLocation';
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
         * @name openEditDialog
         * @function
         * 
         * @param {uLocate.Models.Location} location - the location to edit.
         * @description - Opens the Edit Location dialog.
         */
        $scope.openEditDialog = function (location) {
            // If the view dialog called this, we'll end up with location nested down in the original object.
            if (location.location) {
                location = location.location;
            }
            var dialogData = {
                location: new uLocate.Models.Location(location)
            };
            dialogService.open({
                template: '/App_Plugins/uLocate/Dialogs/edit.location.dialog.html',
                show: true,
                callback: $scope.processEditDialog,
                dialogData: dialogData
            });
        };

        /**
         * @ngdoc method
         * @name openViewDialog
         * @function
         * 
         * @param {uLocate.Models.Location} location - The location to view.
         * @description - Opens a location view dialog.
         */
        $scope.openViewDialog = function (location) {
            var dialogData = {};
            dialogData.location = new uLocate.Models.Location(location);
            dialogService.open({
                template: '/App_Plugins/uLocate/Dialogs/view.location.dialog.html',
                show: true,
                callback: $scope.openEditDialog,
                dialogData: dialogData
            });
        };

        /**
         * @ngdoc method
         * @name processCreateLocationForm
         * @function
         * 
         * @param {uLocate.Models.Location} location - The location to make.
         * @description - Check if the form is valid, and if so, create a new location.
         */
        $scope.processCreateLocationForm = function(location) {
            $scope.wasFormSubmitted = true;
            var isValid = false;
            if ($scope.createForm.$valid) {
                if ($scope.hasProvinces()) {
                    if ($scope.selected.region.name !== '' && $scope.selected.region.name !== $scope.selected.country.provinces[0].name) {
                        location.region = $scope.selected.region.name;
                        if ($scope.selected.country.name !== '' && $scope.selected.country.name !== $scope.options.countries[0].name) {
                            location.countryCode = $scope.selected.country.countryCode;
                            isValid = true;
                        }
                    }
                } else {
                    if ($scope.selected.country.name !== '' && $scope.selected.country.name !== $scope.options.countries[0].name) {
                        location.countryCode = $scope.selected.country.countryCode;
                        isValid = true;
                    }
                }
            }
            if (isValid) {
                var lat = location.latitude;
                var lng = location.longitude;
                var shouldGeocode = false;
                if ((lat == 0 & lng == 0) || lat == '' || lng == '') {
                    shouldGeocode = true;
                }
                if (($location.search()).key !== uLocate.Constants.DEFAULT_LOCATION_TYPE_KEY) {
                    _.each(location.editors, function(editor) {
                        var newProperty = new uLocate.Models.LocationProperty({
                            key: '00000000-0000-0000-0000-000000000000',
                            propAlias: editor.propAlias,
                            propData: editor.value
                        });
                        if (typeof newProperty.propData === 'undefined') {
                            newProperty.propData = '';
                        }
                        location.customPropertyData.push(newProperty);
                    });
                }
                $scope.createLocation(location, shouldGeocode, true);
            }
        };

        /**
         * @ngdoc method
         * @name toggleSortOrder
         * @function
         * 
         * @param {string} orderBy - "name" or "locationType"
         * @description - Check if the form is valid, and if so, create a new location.
         */
        $scope.toggleSortOrder = function(orderBy) {
            if ($scope.sortBy == orderBy) {
                if ($scope.sortOrder === "ASC") {
                    $scope.sortOrder = "DESC";
                } else {
                    $scope.sortOrder = "ASC";
                }
            } else {
                $scope.sortBy = orderBy;
            }
            $scope.getLocations();
        };

        /**
         * @ngdoc method
         * @name updateCountry
         * @function
         * 
         * @param {object} country - The country to update the information from.
         * @param {string} country.countryCode - The country code of a country.
         * @param {string} country.name - The name of the country.
         * @param {string} country.provinceLabel - The term for the country's provinces.
         * @param {array of object} country.provinces - The provinces inside the country.
         * @param {string} country.provinces[x].name - The name of a province.
         * @param {string} country.provinces[x].code - the code for a province.
         * @description - Update info for province selection based on chosen country.
         */
        $scope.updateCountry = function (country) {
            $scope.selected.region = country.provinces[0];
            if (country.provinceLabel !== '') {
                $scope.provinceLabel = country.provinceLabel;
            } else {
                $scope.provinceLabel = 'Province/State';
            }
        };

        /**
         * @ngdoc method
         * @name updatePerPage
         * @function
         * 
         * @param {integer} perPage - The number of locations to show per page.
         * @description - Updates the amount of locations to show per page and triggers getLocations();
         */
        $scope.updatePerPage = function (perPage) {
            $scope.perPage = perPage;
            $scope.page = 0;
            $scope.getLocations();
        };

        /**
         * @ngdoc method
         * @name zoomToLocation
         * @function
         * 
         * @param {uLocate.Models.Location} location - The location to zoom to.
         * @description - Zoom to the provided location and bounce a marker if one is present.
         */
        $scope.zoomToLocation = function (location) {
            var view = new uLocate.Models.MapView({
                coordinates: [location.latitude, location.longitude],
                smoothAnimation: true,
                zoom: 15
            });
            uLocateMapService.changeView($scope.map, view);
            var markers = uLocateMapService.getAllMarkers();
            _.each(markers, function(marker) {
                var position = marker.getPosition();
                var lat = position.lat();
                var lng = position.lng();
                if (lat === location.latitude && lng === location.longitude) {
                    marker.setAnimation(google.maps.Animation.BOUNCE);
                    setTimeout(function() { marker.setAnimation(null); }, 1500);
                }
            });
        };

        /*-------------------------------------------------------------------
         * Helper Methods
         * ------------------------------------------------------------------*/

        /**
         * @ngdoc method
         * @name addLocationMarkersToMap
         * @function
         * 
         * @description - Acquires locations via API call, using the parameters defined by the user.
         */
        $scope.addLocationMarkersToMap = function () {
            uLocateMapService.deleteAllMarkers();
            _.each($scope.locations, function(location) {
                var coord = [location.latitude, location.longitude];
                uLocateMapService.addMarker($scope.map, coord, { title: location.name, icon: $scope.customMarkerIcon });
            });
            uLocateMapService.fitBoundsToMarkers($scope.map);
        };

        /**
         * @ngdoc method
         * @name buildAddressString
         * @function
         * 
         * @param {uLocate.Models.Address} address - The address to conver to a string.
         * @returns {string} string - The address in string form.
         * @description - Converts a provided address object into a string.
         */
        $scope.buildAddressString = function(location) {
            var string = '';
            string += location.address1;
            if (location.address2 != null && (typeof location.address2) != 'undefined' && location.address2 != false) {
                string += ' ' + location.address2;
            }
            string += ', ' + location.locality;
            string += ', ' + location.region;
            string += ' ' + location.postalCode;
            string += ' ' + location.countryCode;
            return string;
        };

        /**
         * @ngdoc method
         * @name createLocation
         * @function
         * 
         * @param {uLocate.Models.Location} location - The location to create.
         * @param {boolean} shouldGeocode - True if address needs to be geocoded.
         * @param {boolean} shouldReloadLocations - True if locations should be updated after new location is created.
         * @description - Creates a location, then triggers update for location.
         */
        $scope.createLocation = function (location, shouldGeocode, shouldReloadLocations) {
            location = new uLocate.Models.Location(location);
            location.locationTypeKey = uLocate.Constants.DEFAULT_LOCATION_TYPE_KEY;
            if (($location.search()).key) {
                location.locationTypeKey = ($location.search()).key;
            }
            _.each($scope.locationTypes, function (type) {
                if (type.key == location.locationTypeKey) {
                    location.locationTypeName = type.name;
                }
            });
            var createPromise = $scope.createLocationApiCall(location);
            createPromise.then(function(guid) {
                if (guid) {
                    location.key = guid;
                    $scope.updateLocation(location, shouldGeocode, shouldReloadLocations);
                }
            });
        };

        /**
         * @ngdoc method
         * @name createLocationApiCall
         * @function
         * 
         * @param {uLocate.Models.Location} location - The location to create.
         * @returns {string or false}
         * @description - Creates a location with the API.
         */
        $scope.createLocationApiCall = function(location) {
            var createPromise = uLocateLocationApiService.createLocation(location.name, location.locationTypeKey);
            return createPromise.then(function (guid) {
                var result = false;
                if (guid) {
                    result = guid;
                } else {
                    notificationsService.error("Attempt to create location '" + location.name + "' failed.", reason.message);
                }
                return result;
            }, function (reason) {
                notificationsService.error("Attempt to create location '" + location.name + "' failed.", reason.message);
                return false;
            });
        };

        /**
         * @ngdoc method
         * @name buildNewLocationEditors
         * @function
         * 
         * @description - Build a list of property editors for the new location being created.
         */
        $scope.buildNewLocationEditors = function () {
            if (($location.search()).key && ($location.search()).key !== uLocate.Constants.DEFAULT_LOCATION_TYPE_KEY) {
                var results = [];
                var editors = [];
                var dataTypePromise = uLocateDataTypeApiService.getAllDataTypes();
                dataTypePromise.then(function (dataTypes) {
                    _.each(dataTypes, function (dataType) {
                        var getByNamePromise = uLocateDataTypeApiService.getByName(dataType.name);
                        getByNamePromise.then(function (data) {
                            editors.push({
                                id: dataType.id,
                                alias: data.propertyEditorAlias,
                                label: dataType.name,
                                view: data.view,
                                config: data.config,
                            });
                            if (editors.length == dataTypes.length) {
                                var key = ($location.search()).key;
                                if ($scope.locationTypes.length > 0) {
                                    _.each($scope.locationTypes, function(type) {
                                        if (type.key == key) {
                                            _.each(type.properties, function (property) {
                                                _.each(editors, function(editor) {
                                                    if (editor.id == property.propType) {
                                                        var editorToReturn = {
                                                            id: editor.id,
                                                            alias: editor.alias,
                                                            label: property.propName,
                                                            view: editor.view,
                                                            config: editor.config,
                                                            propAlias: property.propAlias
                                                        };
                                                        results.push(editorToReturn);
                                                    }
                                                });
                                            });
                                        }
                                    });
                                }
                                $scope.newLocation.editors = results;
                            } else {
                                $scope.newLocation.editors = [];
                            }
                        });
                    });
                });
            }
        };

        /**
        * @ngdoc method
        * @name deleteLocation
        * @function
        * 
        * @param {integer} id - The id of a location to delete.
        * @description - Deletes a location, then redirect to viewing all locations.
        */
        $scope.deleteLocation = function (id) {
            if (id) {
                var promise = uLocateLocationApiService.deleteLocation(id);
                promise.then(function (response) {
                    if (response.success) {
                        notificationsService.success("Location successfully deleted.");
                        $scope.getLocations();
                    } else {
                        notificationsService.error("Attempt to delete location failed.");
                    }
                }, function (reason) {
                    notificationsService.error("Attempt to delete location failed.", reason.message);
                });
            }
        };

        /**
         * @ngdoc method
         * @name geocodeLocationIfNeeded
         * @function
         * 
         * @param {boolean} shouldGeocode - True if the location's address should be geocode.
         * @param {uLocate.Models.Location} location - A location.
         * @returns {[lat, lng] or false}
         * @description - If required, geocode the location's address, then return the geocoded address.
         */
        $scope.geocodeLocationIfNeeded = function(shouldGeocode, location) {
            if(shouldGeocode) {
                $scope.isGeocoding = true;
                var address = $scope.buildAddressString(location);
                notificationsService.info("Acquiring lat/lng for this address. This may take a moment. Do not leave or reload page.");
                var geocodePromise = uLocateMapService.geocode(address);
                return geocodePromise.then(function (geocodeResponse) {
                    var result;
                    if (geocodeResponse) {
                        result = [geocodeResponse[0], geocodeResponse[1]];
                    } else {
                        notificationsService.error("Unable to acquire lat/lng for this address.");
                        result = false;
                    }
                    $scope.isGeocoding = false;
                    return result;
                });
            } else {
                var deferred = $q.defer();
                deferred.resolve(false);
                return deferred.promise;
            }
        };

        /**
         * @ngdoc method
         * @name getLocations
         * @function
         * 
         * @description - Modify the list of locations by current sort orders and paging options.
         */
        $scope.getLocations = function () {
            $scope.locationsLoaded = false;
            uLocateMapService.deleteAllMarkers();
            var request = new uLocate.Models.GetLocationsApiRequest({
                page: $scope.page,
                perPage: $scope.perPage,
                sortBy: $scope.sortBy,
                sortOrder: $scope.sortOrder,
                filter: $scope.filter
            });
            uLocateLocationApiService.getAllPaged(request).then(function(response) {
                if (response) {
                    $scope.page = response.pageNum;
                    $scope.perPage = response.itemsPerPage;
                    $scope.totalPages = response.totalPages;
                    $scope.locations = [];
                    $scope.locations = _.map(response.locations, function(location) {
                        return new uLocate.Models.Location(location);
                    });
                }
                $scope.locationsLoaded = true;
                $scope.addLocationMarkersToMap();
            });
        };

        /**
         * @ngdoc method
         * @name getLocationTypes
         * @function
         * 
         * @description - Gets all location types from the API, if not already stored in constants.
         */
        $scope.getLocationTypes = function () {
            var promise = uLocateLocationTypeApiService.getAllLocationTypes();
            return promise.then(function (response) {
                var locationTypes = _.map(response, function (locationType) {
                    return new uLocate.Models.LocationType(locationType);
                });
                uLocate.Constants.LOCATION_TYPES = locationTypes;
                return uLocate.Constants.LOCATION_TYPES;
            });
        };

        /**
        * @ngdoc method
        * @name hasLocations
        * @function
        * 
        * @returns {boolean}
        * @description - Returns true if the scope has at least one location.
        */
        $scope.hasLocations = function() {
            var result = false;
            if ($scope.locations) {
                if ($scope.locations.length > 0) {
                    result = true;
                }
            }
            return result;
        };

        /**
         * @ngdoc method
         * @name hasProvinces
         * @function
         * 
         * @returns {boolean} - true or false
         * @description - Returns true if the currently selected country has provinces.
         */
        $scope.hasProvinces = function () {
            var result = false;
            if ($scope.selected) {
                if ($scope.selected.country.provinces.length > 1) {
                    result = true;
                }
            }
            return result;
        };

        /**
        * @ngdoc method
        * @name insertUpdatedLocationBackIntoListing
        * @function
        * 
        * @param {uLocate.Models.Location} location - The location to insert into the listing.
        * @description - Update listing with new version of location.
        */
        $scope.insertUpdatedLocationBackIntoListing = function (updatedLocation) {
            if (updatedLocation) {
                _.each($scope.locations, function(location, index) {
                    if (location.key === updatedLocation.key) {
                        $scope.locations[index] = new uLocate.Models.Location(updatedLocation);
                    }
                });
            }
        };

        /**
        * @ngdoc method
        * @name isFormInvalid
        * @function
        * 
        * @param {object} field - The input field to check.
        * @returns {boolean}
        * @description - Returns true if the field is invalid and the form was submitted.
        */
        $scope.isFieldInvalid = function (field) {
            var result = false;
            if ($scope.wasFormSubmitted) {
                if (field.$invalid) {
                    result = true;
                }
            }
            return result;
        };

        /**
        * @ngdoc method
        * @name isSortAscending
        * @function
        * 
        * @returns {boolean}
        * @description - Returns true if the sortOrder is set to "ASC".
        */
        $scope.isSortAscending = function() {
            var result = false;
            if ($scope.sortOrder) {
                if ($scope.sortOrder.toLowerCase() === "asc") {
                    result = true;
                }
            }
            return result;
        };

        /**
        * @ngdoc method
        * @name isSortingByName
        * @function
        * 
        * @returns {boolean}
        * @description - Returns true if the sortBy is set to "name"
        */
        $scope.isSortingByName = function() {
            var result = false;
            if ($scope.sortBy) {
                if ($scope.sortBy.toLowerCase() === "name") {
                    result = true;
                }
            }
            return result;
        };

        /**
        * @ngdoc method
        * @name processEditDialog
        * @function
        * 
        * @param {object} data - Returned object from dialog
        * @param {uLocate.Models.Location} data.location - Location to update.
        * @description - Update a location via API.
        */
        $scope.processEditDialog = function(data) {
            if (data) {
                var location = data.location;
                $scope.updateLocation(location, data.generateLatLng, true);
            };
        };

        /**
         * @ngdoc method
         * @name updateLocation
         * @function
         * 
         * @param {uLocate.Models.Location} location
         * @param {boolean} shouldGeocode - whether or not to geocode lat lng from address.
         * @param {boolean} shouldReloadLocations - Whether or not to load the locations listing.
         * @description - Updates an existing location.
         */
        $scope.updateLocation = function (location, shouldGeocode, shouldReloadLocations) {
            location = new uLocate.Models.Location(location);
            var geocodePromise = $scope.geocodeLocationIfNeeded(shouldGeocode, location);
            geocodePromise.then(function(coordinates) {
                if (coordinates) {
                    location.latitude = coordinates[0];
                    location.longitude = coordinates[1];
                }
                var updatePromise = $scope.updateLocationApiCall(location);
                updatePromise.then(function (updatedLocation) {
                    if (updatedLocation) {
                        $scope.insertUpdatedLocationBackIntoListing(location);
                    }
                    if (!shouldReloadLocations) {
                        $scope.getLocations();
                    } else {
                        $scope.insertUpdatedLocationBackIntoListing(location);
                        window.location = '/umbraco/#/uLocate/uLocate/locations/view';
                    }
                });
            });
        };

        /**
         * @ngdoc method
         * @name updateLocationApiCall
         * @function
         * 
         * @param {uLocate.Models.Location} location
         * @returns {uLocate.Models.Location or false} 
         * @description - Makes an API call to update a location.
         */
        $scope.updateLocationApiCall = function(location) {
            var updatePromise = uLocateLocationApiService.updateLocation(location);
            return updatePromise.then(function(response) {
                if (response) {
                    var updatedLocation = new uLocate.Models.Location(response);
                    notificationsService.success("Location '" + location.name + "' successfully updated. #h5yr!");
                    return updatedLocation;
                } else {
                    notificationsService.error("Attempt to update location '" + location.name + "' failed.", reason.message);
                    return false;
                }
            });
        };

        /*-------------------------------------------------------------------*/

        $scope.init();


    };

    angular.module('umbraco').controller('uLocate.Controllers.LocationsController', ['$scope', '$location', '$q', '$routeParams', 'treeService', 'assetsService', 'dialogService', 'navigationService', 'notificationsService', 'uLocateBroadcastService', 'uLocateDataTypeApiService', 'uLocateInitializationApiService', 'uLocateMapService', 'uLocateLocationApiService', 'uLocateLocationTypeApiService', uLocate.Controllers.LocationsController]);

}(window.uLocate.Controllers = window.uLocate.Controllers || {}));