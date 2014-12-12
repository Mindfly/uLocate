angular.module("umbraco.directives")
    .directive('ulocateLocalize', function (ulocateLocalizationService) {
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
})
.directive('ulocateLocalize', function (ulocateLocalizationService) {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            var keys = attrs.ulocateLocalize.split(',');
            angular.forEach(keys, function (value, key) {
                var attr = element.attr(value);
                if (attr) {
                    ulocateLocalizationService.localize(attr).then(function (val) {
                        element.attr(value, val);
                    });
                }
            });

        }
    };
});