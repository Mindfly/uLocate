angular.module("umbraco.directives").directive('ulocateLocalize', function (ulocateLocalizationService) {
    var linker = function (scope, element, attrs) {

        var key = scope.key;

        ulocateLocalizationService.localize(key).then(function (value) {
            if (value) {
                element.html(value);
            }
        });
    }

    return {
        restrict: "E",
        replace: true,
        link: linker,
        scope: {
            key: '@'
        }
    }
});