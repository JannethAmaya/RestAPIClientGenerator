(function () {
    'use strict';
    var controllerId = 'dashboard';
    angular.module('app').controller(controllerId, ['common', 'requestService', dashboard]);

    function dashboard(common, requestService) {
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);
        var logSucces = getLogFn(controllerId, 'success');
        var logError = getLogFn(controllerId, 'error');

        var vm = this;

        vm.headers = [];
        vm.parameters = [];

        vm.endpoint = { verb: 'GET' }

        activate();

        function activate() {
            var promises = [];
            common.activateController(promises, controllerId)
                .then(function () { });
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

        vm.executeRequest = function () {
            var request =
                {
                    verb: vm.verb,
                    apiUrl: vm.apiUrl,
                    methodName : vm.methodName,
                    apiKey: vm.endpoint.apiKey,
                    parameters: vm.parameters,
                    headers: vm.headers,
                    user: vm.username,
                    password: vm.password
                }

            requestService.executeRequest(request).then(function (response) {
                    requestService.generateClasses(response).then(function(result) {
                        requestService.downloadFile();
                    });
            },
            function (err) {
                logError('Smoething is wrong')
            });
        }
    }
})();