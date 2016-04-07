(function (controllers, undefined) {

    controllers.ViewDialogController = function($scope, dialogService) {

        $scope.init = function() {
            console.info('You passed in the following data: '+ $scope.dialogData.sampleItem);
        };

        $scope.init();
    };

    angular.module('umbraco').controller('uLocate.Controllers.ViewDialogController', ['$scope', 'dialogService', uLocate.Controllers.ViewDialogController]);


}(window.uLocate.Controllers = window.uLocate.Controllers || {}));