
'use strict';
var appVersion = 'v=1.9.9.7';

require.onError = function (err) {

};
require.config({
    baseUrl: cdnUrl + "/app",
    urlArgs: appVersion,
    waitSeconds: 60,
    enforceDefine: false,
    paths: {
        'angular': 'libs/angular.min',
        'angularAnimate': 'libs/angularPlugin/angular-animate.min',
        'angularCookie': 'libs/angular-cookies.min',
        'ui-router': 'libs/angularPlugin/angular-ui-router',
        'resource': 'libs/angularPlugin/angular-resource.min',
        'sanitize': 'libs/angularPlugin/angular-sanitize.min',
        'ngStorage': 'libs/angularPlugin/ngStorage.min',
        'angularAMD': 'libs/angularAMD.min',
        'bootstrap': 'libs/bootstrap.min',
        'jquery': 'libs/jquery.min',
        'domReady': 'libs/domReady',
        'custom': '../js/custom',
        'underscore': 'libs/underscore-min',
        'modernizr': 'libs/modernizr-2.8.3',
        'respond': 'libs/respond.min',
        'encoder': 'libs/encoder',
        'Base64': 'libs/Base64',
        'jquery.fileDownload': 'libs/jquery.fileDownload',
        'kendo': 'libs/kendo/kendo.all.min',
        'toastr': 'libs/toastr/toastr.min',
        'common': 'libs/common/common',
        'logger': 'libs/common/logger',
        'commonViewModel': 'libs/ViewModels/commonViewModel',
        'route': 'config/router',
        'angularKendoWindowService': 'libs/angular-kendowindow-service',
        'loading-bar': 'libs/loading-bar',
        'bootbox': 'libs/bootbox.min',
        'jquery.cookie': 'libs/jquery.cookie',
        'angular-input-modified': 'libs/angular-input-modified.min',
        'ngmap': 'libs/ng-map.min',
        'mCustomScrollbar': '../js/jquery.mCustomScrollbar.concat.min',
        'angularLoad': 'libs/angular-load.min',
        'dateFormat': 'libs/dateFormat',
        'jquery.maskedinput.min': '../js/jquery.maskedinput.min',
        /* Declare service files */
        'masterfileService': 'services/base/masterfileService',
        'cacheTempleteService': 'services/cacheTempleteService',
        'accountService': 'services/accountService',

        /* Declare controllers */
        'headerController': 'layout/headerController',
        'leftSitebarController': 'layout/leftSitebarController',
        'basePopupController': 'controller/base/basePopupController',

        'roleController': 'controller/Role/roleController',
        'createRoleController': 'controller/Role/createRoleController',
        'updateRoleController': 'controller/Role/updateRoleController',
        'sharedRoleController': 'controller/Role/sharedRoleController',
        'viewDetailRoleController': 'controller/Role/viewDetailRoleController',

        'userControllerForPassword': 'controller/User/userControllerForPassword',
        'setPasswordController': 'controller/User/setPasswordController',
        'changePasswordController': 'controller/User/changePasswordController',

        /*General User*/
        'userController': 'controller/User/generalUserController',
        'createUserController': 'controller/User/createGeneralUserController',
        'updatUserController': 'controller/User/updateGeneralUserController',
        'sharedUserController': 'controller/User/sharedGeneralUserController',

        /*Config*/
        'configController': 'controller/Config/configController',
        'sharedConfigController': 'controller/Config/sharedConfigControllezr',

        //========Import Controller Start========//

        //========Import Controller End========//

        /*Other*/
        'loginController': 'controller/Authentication/loginController',
        'nopermissionController': 'controller/Authentication/nopermissionController',
        'viewMyProfileController': 'controller/MyProfile/viewMyProfileController',
        'userEditAvatarProfileController': 'controller/User/userEditAvatarProfileController',
        'changeAvatarController': 'controller/User/changeAvatarController',

        /*Configuration*/
        'configurationController': 'controller/Configuration/configurationController',
        

        /* Declare directvies */
        'cisGridDirective': 'directive/cisGridDirective',
        'cisLookupDirective': 'directive/cisLookupDirective',
        'cisDropdownlistDirective': 'directive/cisDropdownlistDirective',
        'cisDatetimePickerDirective': 'directive/cisDatetimePickerDirective',
        'cisTimePickerDirective': 'directive/cisTimePickerDirective',
        'cisMultiSelectDirective': 'directive/cisMultiSelectDirective',
        'cisUploadDirective': 'directive/cisUploadDirective',
        'charactersAndNumbersOnlyDirective': 'directive/charactersAndNumbersOnlyDirective',
        'numbersOnlyDirective': 'directive/numbersOnlyDirective',
        'formDirective': 'directive/formDirective',
        'cisEditor': 'directive/cisEditor',
        'enterDirective': 'directive/enterDirective',
        'cisPopupDirective': 'directive/cisPopupDirective',
        'cisAutoCompleteDirective': 'directive/cisAutoCompleteDirective',
        'cisComboBoxAutoCompleteDirective': 'directive/cisComboBoxAutoCompleteDirective',
        /* Declare value */
        'fileTypeValue': 'config/fileTypeValue',
    },

    shim: {
        'angularAMD': {
            exports: 'angularAMD',
            deps: ['angular']
        },
        'angularCookie': {
            deps: ['angular']
        },
        'ui-router': {
            exports: 'ui-router',
            deps: ['angular']
        },
        'custom': {
            exports: 'custom',
            deps: ['jquery', 'bootstrap']
        },
        'angularKendoWindowService': {
            exports: 'angularKendoWindowService',
            deps: ['angular', 'bootstrap']
        },
        'loading-bar': {
            exports: 'loading-bar',
            deps: ['angular', 'jquery']
        },
        'angular-input-modified': {
            exports: 'angular-input-modified',
            deps: ['angular', 'jquery']
        },
        'angularAnimate': {
            'deps': ['angular']
        },
        'ngmap': {
            'deps': ['angular']
        },
        "jquery.fileDownload": { deps: ['jquery'] },
        "jquery.cookie": { deps: ['jquery'] },
        "angular": { deps: ['jquery'] },
        "common": { deps: ['angular'] },
        "resource": { deps: ['angular'] },
        "sanitize": { deps: ['angular'] },
        "bootstrap": { deps: ['jquery'] },
        "toastr": { deps: ['jquery'] },
        "kendo": { deps: ['jquery', 'angular'] },
        "angularLoad": { deps: ['angular'] },
        "mCustomScrollbar": { deps: ['jquery'] },
        "signalr.core": { deps: ["jquery"], exports: "$.connection" },

    },
    deps: ['app']
});
