'use strict';

define([

    ],function () {
    var logger = {
        log: function(message) {
            console.log('LOG:' + message);
        },
        debug: function(message) {
             console.debug('DEBUG:' + message);
        }
    };

    return logger;

});