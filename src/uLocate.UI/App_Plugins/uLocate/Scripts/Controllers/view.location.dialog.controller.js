(function (controllers, undefined) {

    controllers.ViewLocationDialogController = function($scope, dialogService) {

        $scope.init = function() {
            console.info('You passed in the following data.');
            console.info($scope.dialogData);
        };

        $scope.init();
    };

    angular.module('umbraco').controller('uLocate.Controllers.ViewLocationDialogController', ['$scope', 'dialogService', uLocate.Controllers.ViewLocationDialogController]);


}(window.uLocate.Controllers = window.uLocate.Controllers || {}));