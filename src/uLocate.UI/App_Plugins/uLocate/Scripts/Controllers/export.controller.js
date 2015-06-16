(function (controllers, undefined) {

    controllers.ExportController = function ($scope, notificationsService, uLocateFileApiService, uLocateInitializationApiService) {

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
                locationType: [new uLocate.Models.LocationType({
                    name: 'Default',
                    key: '00001'
                })]
            }
            $scope.selected = {
                locationType: $scope.options.locationType[0]
            };
            $scope.locationType = $scope.options.locationType[0];
            $scope.file = false;
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
            var key = $scope.selected.locationType.key;
            var url = uLocateFileApiService.exportFile(key);
            if (url) {
                window.open(url, '_blank');
            } else {
                notificationsService.error('Unable to initialize the export.');
            }
        };

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

        /*-------------------------------------------------------------------
         * Helper Methods
         * ------------------------------------------------------------------*/

        /*-------------------------------------------------------------------*/

        $scope.init();

    };

    app.requires.push('angularFileUpload');

    angular.module('umbraco').controller('uLocate.Controllers.ExportController', ['$scope', 'notificationsService', 'uLocateFileApiService', 'uLocateInitializationApiService', uLocate.Controllers.ExportController]);

}(window.uLocate.Controllers = window.uLocate.Controllers || {}));