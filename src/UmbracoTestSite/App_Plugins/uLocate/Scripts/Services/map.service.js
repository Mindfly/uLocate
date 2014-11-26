(function(uLocateServices) {

    uLocateServices.MapService = function ($http, $q) {

        var mapFactory = {};

        mapFactory.loadMap = function (elem, options) {
            var mapOptions;
            if (!options) {
                options = {
                    center: {
                        latitude: 0,
                        longitude: 0
                    },
                    zoom: 15
                };
            }
            console.info(options);
            mapOptions = new uLocate.Models.MapOptions(options);
            console.info(mapOptions);
            var element = document.querySelector(elem);
            var map = new google.maps.Map(element, mapOptions);
            return map;
        };

        return mapFactory;

    };

    angular.module('umbraco.resources').factory('uLocateMapService', ['$http', '$q', uLocate.Services.MapService]);

}(window.uLocate.Services = window.uLocate.Services || {}));