(function (uLocateServices) {

    uLocateServices.BroadcastService = function ($rootScope) {

        var broadcastFactory = {};
        broadcastFactory.message = '';

        /**
         * @ngdoc method
         * @name sendMessage
         * @function
         * 
         * @param {string} channel - The channel name to broadcast on, that will be listened to by other listeners.
         * @param {string} message - The message being sent.
         * @description - Broadcast an event using the provided channel name and set the message it is triggering.
         */
        broadcastFactory.sendMessage = function (channel, message) {
            broadcastFactory.message = message;
            $rootScope.$broadcast(channel);
        };

        return broadcastFactory;
    };

    angular.module('umbraco.resources').factory('uLocateBroadcastService', ['$rootScope', uLocate.Services.BroadcastService]);

}(window.uLocate.Services = window.uLocate.Services || {}));