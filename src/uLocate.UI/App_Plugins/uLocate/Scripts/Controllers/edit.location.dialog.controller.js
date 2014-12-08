(function (controllers, undefined) {

    controllers.EditLocationDialogController = function($scope, dialogService) {

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
            if (location.address.countryName != '' && location.address.countryName != undefined) {
                _.each($scope.options.countries, function(country, index) {
                    if (location.address.countryName === country.name) {
                        $scope.selected.country = $scope.options.countries[index];
                        if (location.address.region != '' && location.address.region != undefined) {
                            _.each($scope.selected.country.provinces, function(region, index) {
                                if (location.address.region === region.name || location.address.region === region.code) {
                                    $scope.selected.region = $scope.selected.country.provinces[index];
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
        $scope.setVariables = function() {
            $scope.options = {
                countries: [{ name: 'Select a Country', provinceLabel: 'Province/State', provinces: [{ name: 'Select A Province/State', code: '' }] }],
                locationTypes: []
            };
            $scope.provinceLabel = 'Province/State';
            $scope.selected = {
                country: $scope.options.countries[0],
                locationTypes: [],
            };
            $scope.selected.region = $scope.selected.country.provinces[0];
            $scope.shouldHideCoordinatesEditor = true;
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
                $scope.dialogData.location.address.countryName = $scope.selected.country.name;
                if ($scope.hasRegions) {
                    if ($scope.selected.region.code != '') {
                        $scope.dialogData.location.address.region = $scope.selected.region.code;
                    } else {
                        $scope.dialogData.location.address.region = $scope.selected.region.name;
                    }
                }
                $scope.dialogData.generateLatLng = $scope.shouldHideCoordinatesEditor;
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
         * @name getLocationTypes
         * @function
         * 
         * @description - Load a list of location types to populate to $scope.options.locationTypes;
         */
        $scope.getLocationTypes = function () {
            // TODO: Wire in functionality to get a list of location types.    
        }

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
        }

        /*-------------------------------------------------------------------*/

        $scope.init();

    };

    angular.module('umbraco').controller('uLocate.Controllers.EditLocationDialogController', ['$scope', 'dialogService', uLocate.Controllers.EditLocationDialogController]);


}(window.uLocate.Controllers = window.uLocate.Controllers || {}));