(function () {
    "use strict";

    angular.module(APPNAME)
        .config(["$routeProvider", "$locationProvider", function ($routeProvider, $locationProvider) {

            $routeProvider.when('/', {
                templateUrl: '/assets/themes/bringpro/js/features/dashboard/templates/jobsTabDashboard.html',
                controller: 'userProfileJobsController',
                controllerAs: 'profilejobs'
            }).when('/jobs/:jobStatus', {
                templateUrl: '/assets/themes/bringpro/js/features/dashboard/templates/jobsTabDashboard.html',
                controller: 'userProfileJobsController',
                controllerAs: 'profilejobs'
            }).when('/jobs/:jobId/invoice', {
                templateUrl: '/assets/themes/bringpro/js/features/dashboard/templates/userProfileJobsInfo.html',
                controller: 'userProfileInvoiceController',
                controllerAs: 'jobinvoice'
            }).when('/wallet', {
                templateUrl: '/assets/themes/bringpro/js/features/dashboard/templates/walletTab.html',
                controller: 'walletController',
                controllerAs: 'wallet'
            }).when('/creditcards', {
                templateUrl: '/assets/themes/bringpro/js/features/dashboard/templates/creditCardsTabDashboard.html',
                controller: 'billingController',
                controllerAs: 'bill'
            }).when('/addressbook', {
                templateUrl: '/assets/themes/bringpro/js/features/dashboard/templates/addressBookTab.html',
                controller: 'addressBookController',
                controllerAs: 'addressBook'
            }).when('/referral', {
                templateUrl: '/assets/themes/bringpro/js/features/dashboard/templates/referralsTabDashboard.html',
                controller: 'referralController',
                controllerAs: 'referral'
            }).when('/activity', {
                templateUrl: '/assets/themes/bringpro/js/features/dashboard/templates/activityTabDashboard.html',
                controller: 'activityController',
                controllerAs: 'activity'
            }).when('/account', {
                templateUrl: '/assets/themes/bringpro/js/features/dashboard/templates/tabDashboard.html',
                controller: 'userProfileController',
                controllerAs: 'userProfile'});

            $locationProvider.html5Mode(false).hashPrefix('');

        }]);

})();