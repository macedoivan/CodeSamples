(function () {
    "use strict";

    angular.module(APPNAME)
        .controller('walletController', WalletController)
        .filter('utcToLocal', Filter);

    WalletController.$inject = ['$scope', '$baseController', "$dashboardService"];

    function WalletController(
        $scope
        , $baseController
        , $dashboardService) {

        var vm = this;
        vm.creditsInfo = null;

        vm.$dashboardService = $dashboardService;
        vm.$scope = $scope;

        $baseController.merge(vm, $baseController);

        vm.notify = vm.$dashboardService.getNotifier($scope);

        initialize();

        function initialize() {
            vm.$dashboardService.getUserCredits(_receiveData, _onError);
        };

        function _receiveData(data) {
            vm.notify(function () {
                vm.creditsInfo = data;
            })
            console.log("Items Recieved: ", vm.creditsInfo);
        };

        function _onError() {
            console.log("Something went wrong.");
        };

    
    }
})();