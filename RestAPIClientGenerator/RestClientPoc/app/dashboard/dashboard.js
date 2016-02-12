(function () {
    'use strict';
    var controllerId = 'dashboard';
    angular.module('app').controller(controllerId, ['common', '$http', dashboard]);

    function dashboard(common, $http) {
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);
        var logSucces = getLogFn(controllerId, 'success');
        var logError = getLogFn(controllerId, 'error');

        var vm = this;
        
        vm.headers = [];
        vm.parameters = [];

        vm.endpoint = {verb : 'GET'}

        activate();

        function activate() {
            var promises = [];
            common.activateController(promises, controllerId)
                .then(function () {});
        }

        vm.addHeader = function (header) {
            vm.headers.push(header);
            vm.header = {};
        };

        vm.removeHeader = function (index) {
            vm.headers.splice(index, 1);
        };

        vm.addParameter = function (parameter) {
            vm.parameters.push(parameter);
            vm.parameter = {};
        };

        vm.removeParameter = function (index) {
            vm.parameters.splice(index, 1);
        };

        vm.executeRequest = function()
        {
            $http.get('/Requests').then(successCallback, errorCallback);

            function successCallback() {
                logSucces('EVerything Fine')
            }

            function errorCallback() {
                logError('Smoething is wrong')   
            }
        }
    }
})();