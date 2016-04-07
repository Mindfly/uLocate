(function (controllers, undefined) {

    controllers.ExportController = function ($scope, notificationsService, uLocateFileApiService, uLocateInitializationApiService, uLocateLocationTypeApiService) {

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
            uLocateInitializationApiService.initDatabaseIfNeeded().then(function () {
                $scope.setVariables();
                $scope.getLocationTypes();
            });
        };


        /**
         * @ngdoc method
         * @name getLocationTypes
         * @function
         * 
         * @description - Loads the location types via API.
         */
        $scope.getLocationTypes = function () {
            if ($scope.locationTypes.length < 1) {
                var promise = uLocateLocationTypeApiService.getAllLocationTypes();
                promise.then(function (response) {
                    $scope.locationTypes = _.map(response, function (locationType) {
                        return new uLocate.Models.LocationType(locationType);
                    });
                    uLocate.Constants.LOCATION_TYPES = $scope.locationTypes;
                    $scope.select = $scope.locationTypes[0];
                    $scope.selectedLocationType = $scope.locationTypes[0];
                });
            } else {
                $scope.select = $scope.locationTypes[0];
                $scope.selectedLocationType = $scope.locationTypes[0];
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
            $scope.locationTypes = uLocate.Constants.LOCATION_TYPES;
            $scope.mode = 'export';
            $scope.select = null;
            $scope.selectedLocationType = null;
            $scope.importResults = '';
            $scope.file = false;
            $scope.isUploading = false;
            $scope.isValidFileType = true;
        };

        /*-------------------------------------------------------------------
         * Event Handler Methods
         *-------------------------------------------------------------------*/

        /**
         * @ngdoc method
         * @name exportFile
         * @function
         * 
         * @description - Generate a URL to open in a new window to trigger the export file download.
         */
        $scope.exportFile = function () {

            $scope.isExporting = true;
            notificationsService.info("File export started. This may take a few moments.");
            var key = $scope.selectedLocationType.key;
            var promise = uLocateFileApiService.exportFile(key);
            promise.then(function (response) {
                if (response) {
                    if (response.success) {
                        window.open(response.objectName, '_blank');
                        notificationsService.success('File successfully exported!');
                        $scope.importResults = response.message;
                        $scope.isExporting = false;
                    } else {
                        notificationsService.error("File Export failed. ", response.exceptionMessage);
                        $scope.isExporting = false;
                    }
                } else {
                    notificationsService.error("Export Export failed.");
                    $scope.isExporting = false;
                }
            }, function (reason) {
                notificationsService.error("File Export failed. " + reason.message, reason.message);
                $scope.isExporting = false;
            });
        };

        /**
         * @ngdoc method
         * @name locationTypeSelected
         * @function
         * 
         * @param {uLocate.Models.LocationType} locationType - The selected location type.
         * @description - Get the selected location type.
         */
        $scope.updateLocationType = function (locationType) {
            $scope.selectedLocationType = locationType;
        };

        /*-------------------------------------------------------------------
         * Helper Methods
         * ------------------------------------------------------------------*/

        /*-------------------------------------------------------------------*/

        $scope.init();

    };

    app.requires.push('angularFileUpload');

    angular.module('umbraco').controller('uLocate.Controllers.ExportController', ['$scope', 'notificationsService', 'uLocateFileApiService', 'uLocateInitializationApiService', 'uLocateLocationTypeApiService', uLocate.Controllers.ExportController]);

}(window.uLocate.Controllers = window.uLocate.Controllers || {}));