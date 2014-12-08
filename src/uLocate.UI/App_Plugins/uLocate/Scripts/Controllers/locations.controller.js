(function(controllers, undefined) {

    controllers.LocationsController = function ($scope, $routeParams, treeService, assetsService, dialogService, navigationService, notificationsService, uLocateMapService, uLocateLocationApiService) {

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
            if ($scope.selectedView === 'view') {
                $scope.loadGoogleMapAsset();
            } else {
                $scope.setVariables();
            }
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
            $scope.locations = [];
            $scope.locationsLoaded = false;
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

        $scope.createLocation = function () {
            $scope.wasFormSubmitted = true;
            if ($scope.createForm.$valid) {
                
            }
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
            if ($scope.selected.country.provinces.length > 1) {
                result = true;
            }
            return result;
        };

        /**
         * @ngdoc method
         * @name openCreateDialog
         * @function
         * 
         * @description - Opens the Create Location dialog.
         */
        $scope.openCreateDialog = function() {
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
         * @description - Opens the Delete Location dialog.
         */
        $scope.openDeleteDialog = function(location) {
            var dialogData = {
                name: location.name,
                location: location,
            };
            dialogService.open({
                template: '/App_Plugins/uLocate/Dialogs/delete.confirmation.dialog.html',
                show: true,
                callback: $scope.processDeleteDialog,
                dialogData: dialogData
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
                coordinates: [location.coordinates.latitude, location.coordinates.longitude],
                smoothAnimation: true,
                zoom: 15
            });
            uLocateMapService.changeView($scope.map, view);
            var markers = uLocateMapService.getAllMarkers();
            _.each(markers, function(marker) {
                var position = marker.getPosition();
                var lat = position.lat();
                var lng = position.lng();
                if (lat === location.coordinates.latitude && lng === location.coordinates.longitude) {
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
                var coord = [location.coordinates.latitude, location.coordinates.longitude];
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
        $scope.buildAddressString = function(address) {
            var string = '';
            string += address.streetAddress;
            if (address.extendedAddress != null && (typeof address.extendedAddress) != 'undefined' && address.extendedAddress != false) {
                string += ' ' + address.extendedAddress;
            }
            string += ', ' + address.locality;
            string += ', ' + address.region;
            string += ' ' + address.postalCode;
            string += ' ' + address.countryName;
            return string;
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
            var request = new uLocate.Models.GetLocationsApiRequst({
                filter: $scope.filter,
                page: $scope.page,
                perPage: $scope.perPage,
                sortBy: $scope.sortBy,
                sortOrder: $scope.sortOrder
            });
            var promise = uLocateLocationApiService.getAllLocations(request);
            promise.then(function(response) {
                $scope.locations = _.map(response.locations, function(location) {
                    return new uLocate.Models.Location(location);
                });
                $scope.page = response.page;
                $scope.perPage = response.perPage;
                _.each($scope.options.perPage, function(option, index) {
                    if (option == response.perPage) {
                        $scope.selected.perPage = $scope.options.perPage[index];
                    }
                });
                $scope.totalPages = response.totalPages;
                $scope.locationsLoaded = true;
                $scope.addLocationMarkersToMap();
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
        * @name processDeleteDialog
        * @function
        * 
        * @param {object} data - Returned object from dialog
        * @param {string} data.name - Name of location.
        * @param {uLocate.Models.Location} data.location - Location to delete.
        * @description - Deletes a location.
        */
        $scope.processDeleteDialog = function(data) {
            if (data) {
                var location = data.location;
                var promise = uLocateLocationApiService.deleteLocation(location.id);
                promise.then(function(response) {
                    if (response.success) {
                        notificationsService.success("Location '" + location.name + "' successfully deleted.");
                        $scope.getLocations();
                    } else {
                        notificationsService.error("Attempt to delete location '" + location.name + "' failed.", reason.message);
                    }
                }, function(reason) {
                    notificationsService.error("Attempt to delete location '" + location.name + "' failed.", reason.message);
                });
            }
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
        $scope.processEditDialog = function (data) {
            if (data) {
                var location = data.location;
                var updatePromise;
                if (data.generateLatLng) {
                    var address = $scope.buildAddressString(location.address);
                    notificationsService.success("Acquiring lat/lng for this address. This may take a moment. Do not leave or reload page.");
                    var geocodePromise = uLocateMapService.geocode(address);
                    geocodePromise.then(function (geocodeResponse) {
                        if (geocodeResponse) {
                            location.coordinates.latitude = geocodeResponse[0];
                            location.coordinates.longitude = geocodeResponse[1];
                            updatePromise = uLocateLocationApiService.updateLocation(location);
                            updatePromise.then(function (response) {
                                if (response) {
                                    notificationsService.success("Location '" + location.name + "' successfully updated. #h5yr!");
                                    $scope.getLocations();
                                }
                            }, function (reason) {
                                notificationsService.error("Attempt to update location '" + location.name + "' failed.", reason.message);
                            });
                        } else {
                            notificationsService.error("Unable to acquire lat/lng for this address.");
                        }
                    });
                } else {
                    updatePromise = uLocateLocationApiService.updateLocation(location);
                    updatePromise.then(function (response) {
                        if (response) {
                            notificationsService.success("Location '" + location.name + "' successfully updated. #h5yr!");
                            $scope.getLocations();
                        }
                    }, function(reason) {
                        notificationsService.error("Attempt to update location '" + location.name + "' failed.", reason.message);
                    });
                }
            }
        };

        /*-------------------------------------------------------------------*/

        $scope.init();


    };

    angular.module('umbraco').controller('uLocate.Controllers.LocationsController', ['$scope', '$routeParams', 'treeService', 'assetsService', 'dialogService', 'navigationService', 'notificationsService', 'uLocateMapService', 'uLocateLocationApiService', uLocate.Controllers.LocationsController]);

}(window.uLocate.Controllers = window.uLocate.Controllers || {}));