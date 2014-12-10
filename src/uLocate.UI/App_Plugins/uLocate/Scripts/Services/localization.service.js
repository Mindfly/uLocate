﻿angular.module('umbraco.services').factory('ulocateLocalizationService', function ($http, $q, userService) {
    var service = {
        resourceFileLoaded: false,
        dictionary: {},
        localize: function (key) {
            var deferred = $q.defer();

            if (service.resourceFileLoaded) {
                var value = service._lookup(key);
                deferred.resolve(value);
            }
            else {
                service.initLocalizedResources().then(function (dictionary) {
                    var value = service._lookup(key);
                    deferred.resolve(value);
                });
            }

            return deferred.promise;
        },
        _lookup: function (key) {
            return service.dictionary[key];
        },
        initLocalizedResources: function () {
            var deferred = $q.defer();
            userService.getCurrentUser().then(function (user) {
                $http.get("/App_Plugins/uLocate/langs/" + user.locale + ".js", { cache: true })
                    .then(function (response) {
                        service.resourceFileLoaded = true;
                        service.dictionary = response.data;

                        return deferred.resolve(service.dictionary);
                    }, function (err) {
                        return deferred.reject("Lang file is missing");
                    });
            });
            return deferred.promise;
        }
    }

    return service;
});