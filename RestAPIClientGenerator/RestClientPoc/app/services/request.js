(function () {
    'use strict';

    var serviceId = 'requestService';
    angular.module('app').factory(serviceId, ['common', request]);

    function request(common) {
        var $q = common.$q;
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(serviceId);
        var $http = common.$http;
        var $apiUri = common.apiEndpoints.requestService;

        var service = {
            executeRequest: executeRequest
        };

        return service;


        function executeRequest(request) {
            var deferred = $q.defer();
            $http.post($apiUri + "execute", request).success(function (response) {
                deferred.resolve(response);
            }).error(function (err) {
                log(err.message);
                deferred.reject(err);
            });

            return deferred.promise;
        }
            
    }
})();