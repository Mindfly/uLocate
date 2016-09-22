(function (controllers, undefined) {

    controllers.EditLocationDialogController = function ($scope, dialogService, uLocateDataTypeApiService, uLocateLocationTypeApiService) {

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
            $scope.buildCountriesList();
            $scope.attemptToSelectCountryAndProvince();
        };

        /**
         * @ngdoc method
         * @name attemptToSelectCountryAndProvince
         * @function
         * 
         * @description - Attempt to select the country and province for the address, if one already exists.
         */
        $scope.attemptToSelectCountryAndProvince = function () {
            var location = $scope.dialogData.location;
            if (location.countryCode != '' && location.countryCode != undefined) {
                _.each($scope.options.countries, function (country, index) {
                    if (location.countryCode === country.countryCode) {
                        $scope.selected.country = $scope.options.countries[index];
                        if (location.region != '' && location.region != undefined) {
                            _.each($scope.selected.country.provinces, function(region, regionIndex) {
                                if (location.region === region.name || location.region === region.code) {
                                    $scope.selected.region = $scope.selected.country.provinces[regionIndex];
                                }
                            });
                        }
                    }
                });
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
                    countryCode: country.countryCode,
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
        };

        /**
         * @ngdoc method
         * @name setVariables
         * @function
         * 
         * @description - Sets the initial state for $scope variables.
         */
        $scope.setVariables = function () {
            $scope.options = {
                countries: [{ name: 'Select a Country', provinceLabel: 'Province/State', provinces: [{ name: 'Select A Province/State', code: '' }] }]
            };
            $scope.provinceLabel = 'Province/State';
            $scope.selected = {
                country: $scope.options.countries[0]
            };
            $scope.selected.region = $scope.selected.country.provinces[0];
            $scope.shouldHideCoordinatesEditor = true;
            $scope.getLocationTypes().then(function (locationTypes) {
                $scope.locationTypes = locationTypes;
                $scope.buildNewLocationEditors();
            });
        };

        /*-------------------------------------------------------------------
         * Event Handler Methods
         *-------------------------------------------------------------------*/

        /**
         * @ngdoc method
         * @name save
         * @function
         * 
         * @description - Make necessary location model modifications before submitting.
         */
        $scope.save = function () {
            $scope.formSubmitted = true;
            if ($scope.editLocationDialogForm.$valid) {
                $scope.dialogData.location.countryCode = $scope.selected.country.countryCode;
                if ($scope.hasProvinces()) {
                    if ($scope.selected.region.code != '') {
                        $scope.dialogData.location.region = $scope.selected.region.code;
                    } else {
                        $scope.dialogData.location.region = $scope.selected.region.name;
                    }
                }
                $scope.dialogData.generateLatLng = $scope.shouldHideCoordinatesEditor;

                if ($scope.dialogData.location.locationTypeKey !== uLocate.Constants.DEFAULT_LOCATION_TYPE_KEY) {
                    _.each($scope.dialogData.location.editors, function (editor) {

                        var hasMatch = false;
                        _.each($scope.dialogData.location.customPropertyData, function (property) {
                            if (property.propAlias == editor.propAlias) {
                                property.propData = editor.value;
                                hasMatch = true;
                            }
                        });

                        if (!hasMatch) {
                            var newProperty = new uLocate.Models.LocationProperty({
                                key: '00000000-0000-0000-0000-000000000000',
                                propAlias: editor.propAlias,
                                propData: editor.value
                            });
                            if (typeof newProperty.propData === 'undefined') {
                                newProperty.propData = '';
                            }
                            $scope.dialogData.location.customPropertyData.push(newProperty);
                        }
                    });
                }
                $scope.submit($scope.dialogData);
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
        $scope.updateCountry = function(country) {
            $scope.selected.region = country.provinces[0];
            if (country.provinceLabel !== '') {
                $scope.provinceLabel = country.provinceLabel;
            } else {
                $scope.provinceLabel = 'Province/State';
            }
        };

        /*-------------------------------------------------------------------
         * Helper Methods
         * ------------------------------------------------------------------*/

        /**
         * @ngdoc method
         * @name buildNewLocationEditors
         * @function
         * 
         * @description - Build a list of property editors for the new location being created.
         */
        $scope.buildNewLocationEditors = function () {
            if ($scope.dialogData.location.locationTypeKey !== uLocate.Constants.DEFAULT_LOCATION_TYPE_KEY) {
                var results = [];
                var editors = [];
                var dataTypePromise = uLocateDataTypeApiService.getAllDataTypes();
                dataTypePromise.then(function(dataTypes) {
                    _.each(dataTypes, function(dataType) {
                        var getByNamePromise = uLocateDataTypeApiService.getByName(dataType.name);
                        getByNamePromise.then(function(data) {
                            editors.push({
                                id: dataType.id,
                                alias: data.propertyEditorAlias,
                                label: dataType.name,
                                view: data.view,
                                config: data.config,
                            });
                            if (editors.length == dataTypes.length) {
                                var key = $scope.dialogData.location.locationTypeKey;
                                if ($scope.locationTypes.length > 0) {
                                    _.each($scope.locationTypes, function(type) {
                                        if (type.key == key) {
                                            _.each(type.properties, function(property) {
                                                _.each(editors, function(editor) {
                                                    if (editor.id == property.propType) {
                                                        var editorToReturn = {
                                                            id: editor.id,
                                                            alias: editor.alias,
                                                            label: property.propName,
                                                            view: editor.view,
                                                            config: editor.config,
                                                            propAlias: property.propAlias,
                                                        };
                                                        results.push(editorToReturn);
                                                    }
                                                });
                                            });
                                        }
                                    });
                                }
                                _.each(results, function(editor) {
                                    _.each($scope.dialogData.location.customPropertyData, function(property) {
                                        if (property.propAlias == editor.propAlias) {
                                            if (editor.view == "checkboxlist") {
                                                editor.value = property.propData.split(",");
                                            } else {
                                                editor.value = property.propData;
                                            }
                                        }
                                    });
                                });
                                $scope.dialogData.location.editors = results;
                            } else {
                                $scope.dialogData.location.editors = [];
                            }
                        });
                    });
                });
            }
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
                $scope.areLocationTypesLoaded = true;
                return uLocate.Constants.LOCATION_TYPES;
            });
        };

        /**
         * @ngdoc method
         * @name hasProvinces
         * @function
         * 
         * @returns {boolean} - true or false
         * @description - Returns true if the currently selected country has provinces.
         */
        $scope.hasProvinces = function() {
            var result = false;
            if ($scope.selected.country.provinces.length > 1) {
                result = true;
            }
            return result;
        };

        /**
         * @ngdoc method
         * @name isInvalidField
         * @function
         * 
         * @param {object} field - The field in the form to verify.
         * @returns {boolean} - true or false
         * @description - Returns true if the field is invalid and the form has been submitted.
         */
        $scope.isInvalidField = function(field) {
            var result = false;
            if ($scope.formSubmitted) {
                if (field.$invalid) {
                    result = true;
                }
            }
            return result;
        };

        /*-------------------------------------------------------------------*/

        $scope.init();

    };

    angular.module('umbraco').controller('uLocate.Controllers.EditLocationDialogController', ['$scope', 'dialogService', 'uLocateDataTypeApiService', 'uLocateLocationTypeApiService', uLocate.Controllers.EditLocationDialogController]);


}(window.uLocate.Controllers = window.uLocate.Controllers || {}));
