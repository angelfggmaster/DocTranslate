(function () {
    'use strict';

    angular
        .module('app')
        .controller('wordDocumentController', wordDocumentController);

    function wordDocumentController(fileUpload) {
        console.log('Document controller');
        var vm = this;
        var urlService = 'http://localhost:62530/api/WordDocuments';

        vm.submit = function () {
            var file = vm.myFile;
            fileUpload.uploadFileToUrl(file, urlService)
                .then(function (resp) {
                    vm.data = resp;
                });
        };

        vm.paragraphs = [];
        vm.TranslateDocument = function () {
            fileUpload.getContentDocument(vm.data.FileNames, urlService)
                .then(function (resp) {
                    vm.paragraphs = resp;
                });
        };
    }
}
)();