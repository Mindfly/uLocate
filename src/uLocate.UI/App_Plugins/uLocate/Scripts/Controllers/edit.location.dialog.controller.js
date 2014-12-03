(function (controllers, undefined) {

    controllers.EditLocationDialogController = function($scope, dialogService) {

        $scope.init = function() {
            console.info('You passed in the following data: '+ $scope.dialogData.sampleItem);
        };

        $scope.init();
    };

    angular.module('umbraco').controller('uLocate.Controllers.EditLocationDialogController', ['$scope', 'dialogService', uLocate.Controllers.EditLocationDialogController]);


}(window.uLocate.Controllers = window.uLocate.Controllers || {}));