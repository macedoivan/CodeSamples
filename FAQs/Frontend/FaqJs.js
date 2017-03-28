  (function () {
            "use strict";

            angular.module(APPNAME)
                .controller('helpController', HelpController);

            HelpController.$inject = ['$scope', '$baseController', '$helpService', '$uibModal'];

            function HelpController(
                $scope
                , $baseController
                , $helpService
                , $uibModal) {

                //  controllerAs with vm syntax: https://github.com/johnpapa/angular-styleguide#style-y032
                var vm = this;//this points to a new {}
                vm.items = null;
                vm.websiteId = null;

                vm.faqForm = null;
                vm.faqFormVisible = false;
                vm.sortOrder = 0;
                vm.selectedFaq = null;
                vm.$helpService = $helpService;
                vm.$scope = $scope;
                vm.$uibModal = $uibModal;

                //  bindable members (functions) always go up top while the "meat" of the functions go below: https://github.com/johnpapa/angular-styleguide#style-y033
                vm.receiveItems = _receiveItems;
                vm.onError = _onError;
                vm.showFaqForm = _showFaqForm;
                vm.resetFaqForm = _resetFaqForm;
                vm.submitFaq = _submitFaq;
                vm.onSuccess = _onSuccess
                vm.selectFaq = _selectFaq;
                vm.onSort = _onSort;
                vm.orderSuccess = _orderSuccess;
                vm.orderError = _orderError;
                vm.openModal = _openModal;
                vm.openAnswerModal = _openAnswerModal;
                vm.logEvent = _logEvent;
                //-- this line to simulate inheritance
                $baseController.merge(vm, $baseController);

                //this is a wrapper for our small dependency on $scope
                vm.notify = vm.$helpService.getNotifier($scope);

                //this is like the App.startUp function
                initialize();

                function initialize() {
                    //  defer data operations into an external service: https://github.com/johnpapa/angular-styleguide#style-y035
                    vm.websiteId = $("#hiddenWebsiteId").val();
                    vm.$helpService.getByWebsiteId(vm.websiteId, vm.receiveItems, vm.onError);

                };

                function _receiveItems(data) {
                    //this receives the data and calls the special
                    //notify method that will trigger ng to refresh UI
                    vm.notify(function () {
                        vm.items = data.items;
                        console.log("Showing these items:", vm.items);
                    });
                };

                function _onError(jqXhr, error) {
                    console.error(error);
                };

                function _showFaqForm() {
                    console.log("show form!");
                    vm.faqFormVisible = !vm.faqFormVisible;
                };

                function _resetFaqForm() {
                    console.log("reset form");
                    vm.faqFormVisible = false;
                    vm.selectedFaq = null;
                    vm.faqForm.$setPristine();
                    vm.faqForm.$setUntouched();

                };

                function _submitFaq(isValid) {

                    if (isValid) {

                        var payLoad = vm.selectedFaq;
                        payLoad.websiteId = vm.websiteId;
                        payLoad.sortOrder = vm.sortOrder;


                        if (vm.selectedFaq.id > 0) {

                            payLoad.id = vm.selectedFaq.id;
                            vm.$helpService.update(payLoad, _onEditSuccess, vm.onError);

                        } else {

                            vm.$helpService.insert(payLoad, vm.onSuccess, vm.onError);
                        }

                        console.log("My Payload", payLoad);
                        console.log("data is valid! go save this object -> ");
                    } else {

                        console.log("form submission invalid");
                    }
                };

                function _onSuccess(data) {
                    //this receives the data and calls the special
                    //notify method that will trigger ng to refresh UI
                    vm.notify(function () {
                        vm.items = data.items;
                    });
                    vm.resetFaqForm();
                    vm.$helpService.getByWebsiteId(vm.websiteId, vm.receiveItems, vm.onError);
                    vm.$alertService.success("FAQ Added")

                };

                function _onEditSuccess(data) {
                    vm.notify(function () {
                        vm.items = data.items;
                    });
                    vm.resetFaqForm();
                    vm.$helpService.getByWebsiteId(vm.websiteId, vm.receiveItems, vm.onError);
                    vm.$alertService.success("FAQ Edited")
                }

                function _selectFaq(aFaq) {
                    console.log("Selected FAQ: ", aFaq);
                    vm.selectedFaq = aFaq;
                    vm.faqFormVisible = !vm.faqFormVisible;

                    window.scrollTo(0, 0);
                };

                function _logEvent(message) {
                    console.log("Message:", message);
                };

                function _onSort($index) {

                    vm.items.splice($index, 1);

                    var sortOrder = [];

                    for (var i = 0; i < vm.items.length; i++) {
                        console.log("you're inside the loop");

                        sortOrder.push(vm.items[i].id);

                    }

                    var orderObject = { sortOrder }

                    console.log("IDs in array: ", orderObject);

                    vm.$helpService.updateOrder(orderObject, vm.orderSuccess, vm.orderError);
                };

                function _orderSuccess() {
                    console.log("You successfully updated the order");
                    vm.$alertService.success("Order Updated")

                };

                function _orderError() {
                    console.log("Error. Something went wrong :(")
                    vm.$alertService.error("Something went wrong.")
                };

                function _openModal(aFaq) {
                    console.log("running");
                    var modalInstance = vm.$uibModal.open({
                        animation: true,
                        templateUrl: 'modalContent.html',       //  this tells it what html template to use. it must exist in a script tag OR external file
                        controller: 'modalController as mc',    //  this controller must exist and be registered with angular for this to work
                        size: 'sm',
                        resolve: {  //  anything passed to resolve can be injected into the modal controller as shown below
                            currentFaq: function () {
                                return aFaq;
                            }
                        }
                    });

                    //  when the modal closes it returns a promise
                    modalInstance.result.then(function () {
                        vm.$helpService.getByWebsiteId(vm.websiteId, vm.receiveItems, vm.onError);

                        //  if the user closed the modal by clicking Save
                    }, function () {
                        console.log('Modal dismissed at: ' + new Date());   //  if the user closed the modal by clicking cancel
                    });
                }

                function _openAnswerModal(aFaq) {
                    console.log("running");
                    var modalInstance = vm.$uibModal.open({
                        animation: true,
                        templateUrl: 'answerModalContent.html',       //  this tells it what html template to use. it must exist in a script tag OR external file
                        controller: 'answerModalController as amc',    //  this controller must exist and be registered with angular for this to work
                        size: 'lg',
                        resolve: {  //  anything passed to resolve can be injected into the modal controller as shown below
                            currentFaq: function () {
                                return aFaq;
                            }
                        }
                    });

                    //  when the modal closes it returns a promise
                    modalInstance.result.then(function () {

                        //  if the user closed the modal by clicking Save
                    }, function () {
                        console.log('Modal dismissed at: ' + new Date());   //  if the user closed the modal by clicking cancel
                    });
                }
            }
        })();

        //  wrap this whole controller in an IIFE like always
        (function () {
            "use strict";

            angular.module(APPNAME)
                .controller('modalController', ModalController);

            //  $uibModalInstance is coming from the UI Bootstrap library and is a reference to the modal window itself so we can work with it
            //  items is the array passed in from the main controller above through the resolve property
            ModalController.$inject = ['$scope', '$baseController', '$helpService', '$uibModalInstance', 'currentFaq']

            function ModalController(
                $scope
                , $baseController
                , $helpService
                , $uibModalInstance
                , currentFaq) {

                var vm = this;

                $baseController.merge(vm, $baseController);
                vm.$helpService = $helpService;
                vm.$scope = $scope;
                vm.$uibModalInstance = $uibModalInstance;
                vm.currentFaq = currentFaq;

                vm.triggerDelete = _triggerDelete;
                vm.cancel = _cancel;

                //  $uibModalInstance is used to communicate and send data back to main controller
                function _triggerDelete() {

                    vm.$helpService.delete(vm.currentFaq.id, _onDeleteSuccess, _onError);
                };

                function _onDeleteSuccess() {
                    console.log("Successfully deleted");
                    vm.$uibModalInstance.close();
                    vm.$alertService.success("FAQ Deleted")
                };

                function _onError(jqXhr, error) {
                    console.error(error);
                    vm.$alertService.error("Something went wrong.")
                };

                function _cancel() {
                    vm.$uibModalInstance.dismiss('cancel');
                };
            }
        })();

        (function () {
            "use strict";

            angular.module(APPNAME)
                .controller('answerModalController', AnswerModalController);

            //  $uibModalInstance is coming from the UI Bootstrap library and is a reference to the modal window itself so we can work with it
            //  items is the array passed in from the main controller above through the resolve property
            AnswerModalController.$inject = ['$scope', '$baseController', '$uibModalInstance', 'currentFaq']

            function AnswerModalController(
                $scope
                , $baseController
                , $uibModalInstance
                , currentFaq) {

                var vm = this;

                $baseController.merge(vm, $baseController);

                vm.currentFaq = currentFaq;

                vm.$scope = $scope;
                vm.$uibModalInstance = $uibModalInstance;

                //  $uibModalInstance is used to communicate and send data back to main controller
                vm.cancel = function () {
                    vm.$uibModalInstance.dismiss('cancel');
                };
            }
        })();