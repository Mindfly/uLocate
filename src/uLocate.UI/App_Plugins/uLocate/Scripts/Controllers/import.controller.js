(function (controllers, undefined) {

    controllers.ImportController = function ($scope, notificationsService, uLocateFileApiService, uLocateInitializationApiService, uLocateLocationTypeApiService) {

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
            uLocateInitializationApiService.initDatabaseIfNeeded().then(function() {
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
            $scope.isValidFileType = true;
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
        $scope.fileSelected = function (files) {
            if (files.length > 0) {
                $scope.file = files[0];
                $scope.isValidFileType = $scope.isFileTypeValid($scope.file.name);
            }
        };

        /**
         * @ngdoc method
         * @name updateLocationType
         * @function
         * 
         * @param locationType
         * @description - Updates $scope.selectedLocationType with the passed in location type.
         */
        $scope.isFileTypeValid = function(fileName) {
            var result = false;
            var extension = fileName.substr(fileName.length - 4).toUpperCase();
            // TODO: Permit XML when future version of uLocate can process such.
            if (extension == ".CSV") {
                result = true;
            }
            return result;
        };

        /**
         * @ngdoc method
         * @name updateLocationType
         * @function
         * 
         * @param locationType
         * @description - Updates $scope.selectedLocationType with the passed in location type.
         */
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
                                if (importResponse.success) {
                                    notificationsService.success('File successfully imported. #h5yr!');
                                    $scope.importResults = importResponse.message;
                                    $scope.isUploading = false;
                                } else {
                                    notificationsService.error("File import failed. ", importResponse.exceptionMessage);
                                    $scope.isUploading = false;
                                }
                            });
                        } else {
                            notificationsService.error("File import failed.");
                            $scope.isUploading = false;
                        }
                    }, function(reason) {
                        notificationsService.error("File import failed. " + reason.message, reason.message);
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

    angular.module('umbraco').controller('uLocate.Controllers.ImportController', ['$scope', 'notificationsService', 'uLocateFileApiService', 'uLocateInitializationApiService', 'uLocateLocationTypeApiService', uLocate.Controllers.ImportController]);

}(window.uLocate.Controllers = window.uLocate.Controllers || {}));