(function () {
    "use strict";

    angular.module(APPNAME)
        .controller('addressBookController', AddressBookController)
        .filter('negToPos', intFilter);

    AddressBookController.$inject = ['$scope', '$baseController', "$userAddressService", "$uibModal"];

    function AddressBookController(
        $scope
        , $baseController
        , $userAddressService
        , $uibModal) {

        var vm = this;
        vm.addresses = null;
        vm.anAddress = null;
        vm.addressComponents = null;
        vm.newAddress = {};
        vm.streetNumber = null;
        vm.streetName = null;
        vm.addressFieldVisible = false;
        vm.map = null;
        vm.mapcood = {
            location: null,
            zoom: 12,
            width: "440",
            height: "80",
            mapType: "roadmap",
            key: $('#staticMapKey').val(),
            markerSize: "mid",
            markerColor: "red",
            format: "png"
        };

        vm.$userAddressService = $userAddressService;
        vm.$scope = $scope;
        vm.$uibModal = $uibModal;

        vm.showAddressField = _showAddressField;
        vm.submitForm = _submitAddress;
        vm.openModal = _openModal;
        vm.makeDefaultAddress = _makeDefaultAddress;

        $baseController.merge(vm, $baseController);

        vm.notify = vm.$userAddressService.getNotifier($scope);

        initialize();

        function initialize() {
            vm.$userAddressService.getUserAddressBook(_receiveData, _onError);
        };

        function _receiveData(data) {
            vm.notify(function () {
                vm.addresses = data.items;
                if(vm.addresses != null) {
                    _mapApi();
                    }
            })
            console.log("Addresses Recieved: ", vm.addresses);
           
        };

        function _onError() {
            console.log("Something went wrong.");
        };

        function _showAddressField() {
            console.log("show form!");
            vm.addressFieldVisible = !vm.addressFieldVisible;
        };

        function _submitAddress() {
            console.log("address components", vm.addressComponents);
            for (var i = 0; i < vm.addressComponents.address_components.length; i++) {
                
                var currentComponent = vm.addressComponents.address_components[i];
               
                if (currentComponent) {

                    if (currentComponent.types && currentComponent.types[0] == "street_number") {
                        vm.streetNumber = currentComponent.short_name;
                    }

                    if (currentComponent.types && currentComponent.types[0] == "route") {
                        vm.streetName = currentComponent.short_name;
                    }

                    if (currentComponent.types && currentComponent.types[0] == "locality") {
                        vm.newAddress.city = currentComponent.long_name;
                    }

                    if (currentComponent.types && currentComponent.types[0] == "administrative_area_level_1") {
                        vm.newAddress.state = currentComponent.short_name;
                    }

                    if (currentComponent.types && currentComponent.types[0] == "country") {
                        vm.newAddress.country = currentComponent.short_name;
                    }

                    if (currentComponent.types && currentComponent.types[0] == "postal_code") {
                        vm.newAddress.zipCode = currentComponent.long_name;
                    }
                }
            }
            vm.newAddress.line1 = vm.streetNumber + " " + vm.streetName;
            vm.newAddress.externalPlaceId = vm.addressComponents.id;

            vm.newAddress.stateId = 0;
            
            console.log("new address: ", vm.newAddress);

            vm.$userAddressService.insert(vm.newAddress, _onSuccess, _onError);
        }

        function _onSuccess() {
            vm.$userAddressService.getUserAddressBook(_receiveData, _onError);
            vm.addressComponents = "";
            vm.addressFieldVisible = !vm.addressFieldVisible;
            vm.$alertService.success("Address Added");
        }

        function _openModal(anAddress) {
            console.log("running");
            var modalInstance = vm.$uibModal.open({
                animation: true,
                templateUrl: '/Assets/Themes/bringpro/js/features/dashboard/templates/addressBookModal.html',       //  this tells it what html template to use. it must exist in a script tag OR external file
                controller: 'addressBookModalController as abmc',    //  this controller must exist and be registered with angular for this to work
                size: 'md',
                resolve: {  //  anything passed to resolve can be injected into the modal controller as shown below
                    currentAddress: function () {
                        return anAddress;
                    }
                }
            });

            //  when the modal closes it returns a promise
            modalInstance.result.then(function () {
                vm.$userAddressService.getUserAddressBook(_receiveData, _onError);

                //  if the user closed the modal by clicking Save
            }, function () {
                console.log('Modal dismissed at: ' + new Date());   //  if the user closed the modal by clicking cancel
            });
        }

        function _makeDefaultAddress(addressId) {
            vm.$userAddressService.updateDefault(addressId, _makeDefaultSuccess, _onError)
        }

        function _makeDefaultSuccess() {
            vm.$alertService.success("Default Updated");
            vm.$systemEventService.broadcast("defaultAddressUpdate");
            vm.$userAddressService.getUserAddressBook(_receiveData, _onError);
        }

        function _mapApi() {
            for (var i = 0; i < vm.addresses.length; i++) {
                vm.addresses[i].googleurl = null;
                var addr = (vm.addresses[i].address.line1 + "+" + vm.addresses[i].address.city + "+" + vm.addresses[i].address.state);
                var waypt = { address: addr, color: "red" };
                vm.mapcood.location = waypt.address;
                vm.map = "http://maps.googleapis.com/maps/api/staticmap?center=" + vm.mapcood.location + "&zoom=" + vm.mapcood.zoom + "&size=" + vm.mapcood.width + "x" + vm.mapcood.height;
                var marker = '&markers=' + '%7Ccolor:' + waypt.color + '%7C' + waypt.address;
                vm.map += marker;
                var key = "&key=" + vm.mapcood.key;
                vm.map += key;
                vm.addresses[i].googleurl = vm.map;
            }
        }
    }
})();