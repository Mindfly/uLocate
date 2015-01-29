(function (controllers, undefined) {

    controllers.ImportController = function ($scope, notificationsService, uLocateFileApiService) {

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
        };

        /**
         * @ngdoc method
         * @name setVariables
         * @function
         * 
         * @description - Sets the initial state for $scope variables.
         */
        $scope.setVariables = function () {
            $scope.mode = 'import';
            $scope.options = {
                locationType: [new uLocate.Models.LocationType({
                    name: 'Default'
                })]
            }
            $scope.selected = {
                locationType: $scope.options.locationType[0]
            };
            $scope.importResults = '';
            $scope.locationType = $scope.options.locationType[0];
            $scope.file = false;
            $scope.isUploading = false;
        };

        /*-------------------------------------------------------------------
         * Event Handler Methods
         *-------------------------------------------------------------------*/

        /**
         * @ngdoc method
         * @name locationTypeSelected
         * @function
         * 
         * @param {uLocate.Models.LocationType} locationType - The selected location type.
         * @description - Get the selected location type.
         */
        $scope.locationTypeSelected = function(locationType) {
            $scope.locationType = locationType;
        };

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
                    var promise = uLocateFileApiService.uploadFileToServer($scope.file, $scope.locationType);
                    promise.then(function(response) {
                        if (response) {
                            var importPromise = uLocateFileApiService.importLocationsCsv(response);
                            importPromise.then(function(importResponse) {
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

    angular.module('umbraco').controller('uLocate.Controllers.ImportController', ['$scope', 'notificationsService', 'uLocateFileApiService', uLocate.Controllers.ImportController]);

}(window.uLocate.Controllers = window.uLocate.Controllers || {}));