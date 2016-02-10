(function () {
    'use strict';
    var controllerId = 'dashboard';
    angular.module('app').controller(controllerId, ['common', dashboard]);

    function dashboard(common) {
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);
        var logSucces = getLogFn(controllerId, 'success');

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
            logSucces('Pegar con backend de Rodrigo!!')
        }
    }
})();