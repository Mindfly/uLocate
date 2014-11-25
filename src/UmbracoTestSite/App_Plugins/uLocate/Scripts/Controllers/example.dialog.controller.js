(function (controllers, undefined) {

    controllers.ExampleDialogController = function($scope, dialogService) {

        $scope.init = function() {
            console.info('You passed in the following data: '+ $scope.dialogData.sampleItem);
        };

        $scope.init();
    };

    angular.module('umbraco').controller('uLocate.Controllers.ExampleDialogController', ['$scope', 'dialogService', uLocate.Controllers.ExampleDialogController]);


}(window.uLocate.Controllers = window.uLocate.Controllers || {}));