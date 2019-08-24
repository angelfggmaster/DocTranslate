(function () {
    'use strict';

    angular
        .module('app')
        .controller('headerController', headerController);

    function headerController() {
        console.log('header controller');
    }
}
)();