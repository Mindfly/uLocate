(function(controllers, undefined) {

    controllers.LocationsController = function ($scope, $location, $routeParams, treeService, assetsService, dialogService, navigationService, notificationsService, uLocateBroadcastService, uLocateMapService, uLocateLocationApiService, uLocateLocationTypeApiService) {

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
            if ($scope.selectedView === 'view' || $scope.selectedView === 'create') {
                $scope.addDeleteLocationListener();
                $scope.loadGoogleMapAsset();
            } else {
                $scope.setVariables();
            }
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
            $scope.areLocationTypesLoaded = false;
            if ($scope.locationTypes.length > 0) {
                $scope.areLocationTypesLoaded = true;
            }
            $scope.newLocation = new uLocate.Models.Location();
            $scope.openMenu = false;
            $scope.options = {
                countries: [{ name: 'Select a Country', provinceLabel: 'Province/State', provinces: [{ name: 'Select A Province/State', code: '' }] }],
                perPage: [25, 50, 100]
            };
            $scope.page = 0;
            $scope.perPage = 100;
            $scope.provinceLabel = 'Province/State';
            $scope.selected = {
                country: $scope.options.countries[0],
                perPage: $scope.options.perPage[2]
            }
            $scope.selected.region = $scope.selected.country.provinces[0];
            $scope.sortBy = 'name';
            $scope.sortOrder = 'ascending';
            $scope.totalPages = 0;
            $scope.wasFormSubmitted = false;
            $scope.buildCountriesList();
            $scope.getCurrentNode();
            // Load the map now that the required variables have been assigned.
            if ($scope.selectedView === 'view') {
                $scope.map = null;
                $scope.mapOptions = {
                    center: {
                        latitude: 0,
                        longitude: 0
                    },
                    zoom: 12,
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
        $scope.changePage = function(difference) {
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
            currentNode.locationTypes = $scope.locationTypes;
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
                }
            }
            if (isValid) {
                var lat = location.latitude;
                var lng = location.longitude;
                var shouldGeocode = false;
                if ((lat == 0 & lng == 0) || lat == '' || lng == '') {
                    shouldGeocode = true;
                }
                $scope.createLocation(location, shouldGeocode, true);
            }
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
        $scope.updatePerPage = function(perPage) {
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
         * @name getLocations
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
         * @description - Creates a location.
         */
        $scope.createLocation = function (location, shouldGeocode, shouldReloadPageView) {
            var createPromise;
            location.locationTypeKey = uLocate.Constants.DEFAULT_LOCATION_TYPE_KEY;
            if (($location.search()).key) {
                location.locationTypeKey = ($location.search()).key;
            }
            _.each($scope.locationTypes, function (type) {
                console.info(type);
                if (type.key == location.locationTypeKey) {
                    console.info('match!');
                    location.locationTypeName = type.name;
                }
            });
            console.info(location);
            if (shouldGeocode) {
                var address = $scope.buildAddressString(location);
                var geocodePromise = uLocateMapService.geocode(address);
                $scope.isGeocoding = true;
                notificationsService.info("Acquiring lat/lng for this address. This may take a moment. Do not leave or reload page until location is created.");
                geocodePromise.then(function (geocodeResponse) {
                    $scope.isGeocoding = false;
                    if (geocodeResponse) {
                        location.latitude = geocodeResponse[0];
                        location.longitude = geocodeResponse[1];
                    } else {
                        notificationsService.error("Unable to get coordinates for provided location address.");
                    }
                    createPromise = uLocateLocationApiService.createLocation(location.name);
                    createPromise.then(function (guid) {
                        if (guid) {
                            location.key = guid;
                            $scope.updateLocation(location, false, shouldReloadPageView);
                        } else {
                            notificationsService.error("Attempt to create location '" + location.name + "' failed.", reason.message);
                        }
                    }, function (reason) {
                        notificationsService.error("Attempt to create location '" + location.name + "' failed.", reason.message);
                    });
                });
            } else {
                createPromise = uLocateLocationApiService.createLocation(location);
                createPromise.then(function (guid) {
                    if (guid) {
                        location.key = guid;
                        $scope.updateLocation(location, false, shouldReloadPageView);
                    } else {
                        notificationsService.error("Attempt to create location '" + location.name + "' failed.", reason.message);
                    }
                }, function (reason) {
                    notificationsService.error("Attempt to create location '" + location.name + "' failed.", reason.message);
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
         * @name getLocations
         * @function
         * 
         * @description - Acquires locations via API call, using the parameters defined by the user.
         */
        $scope.getLocations = function () {
            uLocateMapService.deleteAllMarkers();
            $scope.locations = [];
            var request = new uLocate.Models.GetLocationsApiRequest({
                filter: $scope.filter,
                page: $scope.page,
                perPage: $scope.perPage,
                sortBy: $scope.sortBy,
                sortOrder: $scope.sortOrder
            });
            var promise = uLocateLocationApiService.getAllLocationsPaged(request);
            promise.then(function (response) {
                if (response.locations) {
                    $scope.locations = _.map(response.locations, function(location) {
                        return new uLocate.Models.Location(location);
                    });
                }
                $scope.page = response.pageNum;
                $scope.perPage = response.itemsPerPage;
                _.each($scope.options.perPage, function(option, index) {
                    if (option == response.perPage) {
                        $scope.selected.perPage = $scope.options.perPage[index];
                    }
                });
                $scope.totalPages = response.totalPages;
                $scope.locationsLoaded = true;
                $scope.addLocationMarkersToMap();
                $scope.getLocationTypes();
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
            if (uLocate.Constants.LOCATION_TYPES.length < 1) {
                var promise = uLocateLocationTypeApiService.getAllLocationTypes();
                promise.then(function (response) {
                    $scope.locationTypes = _.map(response, function (locationType) {
                        return new uLocate.Models.LocationType(locationType);
                    });
                    uLocate.Constants.LOCATION_TYPES = $scope.locationTypes;
                    $scope.areLocationTypesLoaded = true;
                });
            } else {
                $scope.locationTypes = uLocate.Constants.LOCATION_TYPES;
                $scope.areLocationTypesLoaded = true;
            }
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
         * @param {boolean} shouldReloadPageView - Whether or not to load the view of locations.
         * @description - Updates an existing location.
         */
        $scope.updateLocation = function (location, shouldGeocode, shouldReloadPageView) {
            var updatePromise;
            if (shouldGeocode) {
                $scope.isGeocoding = true;
                var address = $scope.buildAddressString(location);
                notificationsService.info("Acquiring lat/lng for this address. This may take a moment. Do not leave or reload page.");
                var geocodePromise = uLocateMapService.geocode(address);
                geocodePromise.then(function (geocodeResponse) {
                    if (geocodeResponse) {
                        location.latitude = geocodeResponse[0];
                        location.longitude = geocodeResponse[1];
                    } else {
                        notificationsService.error("Unable to acquire lat/lng for this address.");
                    }
                    $scope.isGeocoding = false;
                    updatePromise = uLocateLocationApiService.updateLocation(location);
                    updatePromise.then(function (response) {
                        if (response) {
                            notificationsService.success("Location '" + location.name + "' successfully updated. #h5yr!");
                            if (!shouldReloadPageView) {
                                $scope.getLocations();
                            } else {
                                window.location = '/umbraco/#/uLocate/uLocate/locations/view';
                            }
                        }
                    }, function (reason) {
                        notificationsService.error("Attempt to update location '" + location.name + "' failed.", reason.message);
                    });
                });
            } else {
                updatePromise = uLocateLocationApiService.updateLocation(location);
                updatePromise.then(function (response) {
                    if (response) {
                        notificationsService.success("Location '" + location.name + "' successfully updated. #h5yr!");
                        if (!shouldReloadPageView) {
                            $scope.getLocations();
                        } else {
                            window.location = '/umbraco/#/uLocate/uLocate/locations/view';
                        }
                    }
                }, function (reason) {
                    notificationsService.error("Attempt to update location '" + location.name + "' failed.", reason.message);
                });
            }
        };

        /*-------------------------------------------------------------------*/

        $scope.init();


    };

    angular.module('umbraco').controller('uLocate.Controllers.LocationsController', ['$scope', '$location', '$routeParams', 'treeService', 'assetsService', 'dialogService', 'navigationService', 'notificationsService', 'uLocateBroadcastService', 'uLocateMapService', 'uLocateLocationApiService', 'uLocateLocationTypeApiService', uLocate.Controllers.LocationsController]);

}(window.uLocate.Controllers = window.uLocate.Controllers || {}));