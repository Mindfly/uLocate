(function (controllers, undefined) {

    controllers.EditDialogController = function($scope, dialogService) {

        $scope.init = function() {
            console.info('You passed in the following data: '+ $scope.dialogData.sampleItem);
        };

        $scope.init();
    };

    angular.module('umbraco').controller('uLocate.Controllers.EditDialogController', ['$scope', 'dialogService', uLocate.Controllers.EditDialogController]);


}(window.uLocate.Controllers = window.uLocate.Controllers || {}));