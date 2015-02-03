(function (controllers, undefined) {

    controllers.ImportController = function ($scope, notificationsService, uLocateFileApiService, uLocateLocationTypeApiService) {

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
            $scope.getLocationTypes();
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
                promise.then(function(response) {
                    $scope.locationTypes = _.map(response, function(locationType) {
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
            $scope.mode = 'import';
            $scope.select = null;
            $scope.selectedLocationType = null;
            $scope.importResults = '';
            $scope.file = false;
            $scope.isUploading = false;
        };

        /*-------------------------------------------------------------------
         * Event Handler Methods
         *-------------------------------------------------------------------*/

        /**
         * @ngdoc method
         * @name fileSelected
         * @function
         * 
         * @param {array of file} files - One or more files selected by the HTML5 File Upload API.
         * @description - Get the file selected and store it in scope.
         */
        $scope.fileSelected = function(files) {
            if (files.length > 0) {
                $scope.file = files[0];
            }
        };

        $scope.updateLocationType = function(locationType) {
            $scope.selectedLocationType = locationType;
        };

        /**
         * @ngdoc method
         * @name uploadFile
         * @function
         * 
         * @description - Uploads a CSV file to the backend to convert the data to locations.
         */
        $scope.uploadFile = function () {
            if (!$scope.isUploading) {
                if ($scope.file) {
                    $scope.isUploading = true;
                    notificationsService.info("File import started. This may take a few moments.");
                    var key = $scope.selectedLocationType.key;
                    var promise = uLocateFileApiService.uploadFileToServer($scope.file);
                    promise.then(function(response) {
                        if (response) {
                            var importPromise = uLocateFileApiService.importLocationsCsv(response, key);
                            importPromise.then(function (importResponse) {
                                notificationsService.success('File successfully imported. #h5yr!');
                                $scope.importResults = importResponse.message;
                                $scope.isUploading = false;
                            });
                        } else {
                            notificationsService.error("File import failed.");
                        }
                    }, function(reason) {
                        notificationsService.error("File import failed.", reason.message);
                        $scope.isUploading = false;
                    });
                } else {
                    notificationsService.error("Must select a file to import.");
                    $scope.isUploading = false;
                }
                
            }
        };

        /*-------------------------------------------------------------------
         * Helper Methods
         * ------------------------------------------------------------------*/

        /*-------------------------------------------------------------------*/

        $scope.init();

    };

    app.requires.push('angularFileUpload');

    angular.module('umbraco').controller('uLocate.Controllers.ImportController', ['$scope', 'notificationsService', 'uLocateFileApiService', 'uLocateLocationTypeApiService', uLocate.Controllers.ImportController]);

}(window.uLocate.Controllers = window.uLocate.Controllers || {}));