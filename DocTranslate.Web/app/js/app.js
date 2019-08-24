(function () {
    'use strict';

    var app = angular.module('app', [
        // Angular modules
        'ngAnimate',
        'ngRoute',
        'ngSanitize',
        'ngTouch',
        'ngResource'
    ]);

    console.log('app starting', navigator.userAgent);

    // config settings
    app.configuration = {
        useLocalData: false
    };

    app.config([
        '$routeProvider',
        '$compileProvider',
        '$httpProvider',
        function ($routeProvider, $compileProvider, $httpProvider) {
            $routeProvider
                .when("/worddocument", {
                    templateUrl: "app/views/worddocument.html"
                })
                .otherwise({
                    redirectTo: "worddocument"
                });
        }
    ]);

    app.service('fileUpload', ['$http', function ($http) {
        this.uploadFileToUrl = function (file, uploadUrl) {
            var fd = new FormData();
            fd.append('file', file);
            var promise = $http.post(uploadUrl, fd, {
                transformRequest: angular.identity,
                headers: { 'Content-Type': undefined }
            })
                .then(function (resp) {
                    var serviceData = resp.data;
                    return serviceData;
                });
            return promise;
        };

        this.getContentDocument = function (docPath, uploadUrl) {
            var promise = $http.get(uploadUrl + '?docPath=' + docPath)
                .then(function (resp) {
                    var serviceData = resp.data;
                    return serviceData;
                });
            return promise;
        };
    }]);
})();