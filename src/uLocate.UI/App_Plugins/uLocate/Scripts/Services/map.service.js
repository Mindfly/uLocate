(function(uLocateServices) {

    uLocateServices.MapService = function ($http, $q) {

        var mapFactory = {};

        mapFactory.loadMap = function (elem, options) {
            var mapOptions;
            if (!options) {
                mapOptions = {
                    center: new google.MapService.latLng(0, 0),
                    zoom: 15
                };
            } else {
                mapOptions = new uLocate.Models.MapOptions(options);
            }
            var element = document.querySelector(elem);
            var map = new google.maps.Map(element, mapOptions);
            return map;
        };

        return mapFactory;

    };

    angular.module('umbraco.resources').factory('uLocateMapService', ['$http', '$q', uLocate.Services.MapService]);

}(window.uLocate.Services = window.uLocate.Services || {}));