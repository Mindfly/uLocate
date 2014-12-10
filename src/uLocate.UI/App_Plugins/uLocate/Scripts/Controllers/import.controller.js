(function (controllers, undefined) {

    controllers.ImportController = function ($scope, $routeParams, $http, treeService, assetsService, dialogService, navigationService, notificationsService) {

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
            $scope.selectedView = $routeParams.id;
            $scope.dataType = 'Default';
            $scope.file = false;
        };

        /*-------------------------------------------------------------------
         * Event Handler Methods
         *-------------------------------------------------------------------*/

        $scope.fileSelected = function (files) {
            if (files.length > 0) {
                $scope.file = files[0];
            }
        };


        $scope.uploadFile = function () {
            $http({
                method: 'POST',
                url: "/Api/PostStuff",
                // TODO: Change Content-Type to undefined in Umbraco 7.5 (or whenever the Angular version is bumped to 1.2 or higher)
                headers: { 'Content-Type': false },
                transformRequest: function (data) {
                    var formData = new FormData();
                    formData.append("dataType", angular.toJson(data.dataType));
                    formData.append("file", data.file);
                    return formData;
                },
                //Create an object that contains the model and files which will be transformed in the above transformRequest method
                data: { dataType: $scope.dataType, file: $scope.file }
            }).success(function (data, status, headers, config) {
                notificationsService.success("File imported.");
            }).error(function (data, status, headers, config) {
                notificationsService.error("File import failed.");
            });
        };

        /*-------------------------------------------------------------------
         * Helper Methods
         * ------------------------------------------------------------------*/

        /*-------------------------------------------------------------------*/

        $scope.init();

    };

    app.requires.push('angularFileUpload');

    angular.module('umbraco').controller('uLocate.Controllers.ImportController', ['$scope', '$routeParams', '$http', 'treeService', 'assetsService', 'dialogService', 'navigationService', 'notificationsService', uLocate.Controllers.ImportController]);

}(window.uLocate.Controllers = window.uLocate.Controllers || {}));