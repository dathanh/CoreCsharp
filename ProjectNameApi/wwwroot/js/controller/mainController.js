app.controller('mainController', ['$rootScope', '$scope', '$sce', '$http', function ($rootScope, $scope, $sce, $http) {
    var mainUrl = "http://localhost:57603/api/home/";

    $scope.Order = {
        Gender: "1",
        CityId: 0,
        DistrictId: 0,
        StoreId: 0,
        FullName: "",
        Phone: "",
        Email: "",
        PaymentType: "1",
        ProductId: 2,
        Token: "",
        WinProductId:""
    }
    $scope.SelectedNote = {
        Id: 2,
        Name: "samsung galaxy note10+",
        Price: "27.990.000",

    }
    $scope.selectedNoteId = 1;
    $scope.customerListCount = 0;
    $scope.filerCustomer = "";
    $scope.customers = [];
    $scope.customerRemain = 0;
    $scope.pageCustomer = 1;
    $scope.takeCustomer = 50;
    $scope.isPlayedGame = false;
    $scope.lcdRemain = 0;
    $scope.tiviRemain = 0;
    $scope.init = function () {
        $scope.cities = [];
        $scope.districts = [];
        $scope.stores = [];
        var urlCity = mainUrl + "cities";
        $http.get(urlCity).then(function (response) {
            var result = response.data;
            $scope.cities = result;
        });
        getCustomerData();
        var urlCountDown = mainUrl + "count-down";
        $http.get(urlCountDown).then(function (response) {
            var result = response.data;
            setClock(result);
            myClock.start();
        });
        var urlTiviRemain = mainUrl + "product-remain/3";
        $http.get(urlTiviRemain).then(function (response) {
            var result = response.data;
            $scope.tiviRemain = result;
        });

        var urlLcdRemain = mainUrl + "product-remain/4";
        $http.get(urlLcdRemain).then(function (response) {
            var result = response.data;
            $scope.lcdRemain = result;
        });
    }

    $scope.selectNote = function (id) {
        if (id == 1) {
            $scope.SelectedNote = {
                Id: 1,
                Name: "samsung galaxy note10",
                Price: "23.990.000",
            }
            $scope.Order.ProductId = 5;
        }
        else {
            $scope.SelectedNote = {
                Id: 2,
                Name: "samsung galaxy note10+",
                Price: "27.990.000",
            }
            $scope.Order.ProductId = 2;
        }
    }

    getCustomerData = function () {
        var urlCustomer = mainUrl + "customers";
        var data = {
            Keyword: $scope.filerCustomer,
            Page: $scope.pageCustomer,
            Take: $scope.takeCustomer
        };
        $http.post(urlCustomer, data).then(function (response) {
            var result = response.data;
            $.each(result.Data, function (index, value) {
                $scope.customers.push(value);
            });

            $scope.customerListCount = result.TotalCount;
            $scope.customerRemain = $scope.customerListCount - $scope.customers.length;
        });
    }

    $scope.loadMoreCustomer = function () {
        if ($scope.customerRemain > 0) {
            $scope.pageCustomer = $scope.pageCustomer + 1;
            getCustomerData();
        }
    }
    var isProcessOpenChestInGame = false;
    $scope.openChestInGame = function () {
        if (isProcessOpenChestInGame) {
            return;
        }
        isProcessOpenChestInGame = true;
        var url = mainUrl + "product-win";
        $http.get(url).then(function (response) {
            var result = response.data;
            isProcessOpenChestInGame = false;
            if (result.ProductId != undefined) {
                $('.fs-gilt-box .fs-box').css({ 'display': 'none' });
                if (result.ProductId == 0) {
                    $('#noProductWithGame').fadeIn(300);
                }
                else if (result.ProductId == 4) {
                    $('#gameWinLcd').fadeIn(300);
                }
                else if (result.ProductId == 3) {
                    $('#gameWinTivi').fadeIn(300);
                }
                $scope.isPlayedGame = true;
                $scope.Order.WinProductId = result.ProductId;
                $scope.Order.Token = result.Token;
            } else if (result.ErrorCodes != undefined && result.ErrorCodes.length > 0) {
                showErrorFromServer(result.ErrorCodes);
            }
        });
    }
    $scope.winGame = function () {
        TweenMax.killTweensOf('.fs-answer');
        $(".fs-answer").removeAttr("style");
        $('.fs-answer').remove();

        $('.fs-game-player').removeClass('active');
        $('.fs-game-box, .fs-gilt-box .fs-box').css({ 'display': 'none' });
        $('.is-win, .win-open-first').fadeIn(0);
        $('.fs-game-pop').addClass('active');

    }

    $scope.changeCityOtherAddr = function (id) {
        $scope.Order.CityId = id;
        $("#districtArea").html("<h3>Quận</h3>");
        $scope.Order.DistrictId = 0;
        $scope.districts = [];

        $("#storeArea").html("<h3>Cửa hàng</h3>");
        $scope.Order.StoreId = 0;
        $scope.stores = [];
        if (id != 0) {
            // Bind district
            var urlDistrict = mainUrl + "districts/" + id;
            // $('.loadicon').addClass('is-processing');
            $http.get(urlDistrict).then(function (response) {
                var result = response.data;
                //$('.loadicon').removeClass('is-processing');
                $scope.districts = result;
            });
        }
        changeCityDistrictStoreEvent("cityInOtherAddr" + id);
    }


    $scope.changeDistrictOtherAddr = function (id) {
        $scope.Order.DistrictId = id;
        $("#storeArea").html("<h3>Cửa hàng</h3>");
        $scope.Order.StoreId = 0;
        $scope.stores = [];
        if (id != 0) {
            // Bind store
            var urlStore = mainUrl + "stores/" + id;
            // $('.loadicon').addClass('is-processing');
            $http.get(urlStore).then(function (response) {
                var result = response.data;
                //$('.loadicon').removeClass('is-processing');
                $scope.stores = result;
            });
        }
        changeCityDistrictStoreEvent("districtInOtherAddr" + id);
    }
    $scope.selectProduct = function (id) {
        $scope.Order.ProductId = id;
        var idElement = "selectProduct" + id;
        var that = $("#" + idElement);
        var box = that.parent().parent().parent();
        if (!that.hasClass('selected')) {
            box.find('li').removeClass('selected');
            that.addClass('selected');
            box.removeClass('open');
            target = that.data('target');
        }
    }

    $scope.changeStoreOtherAddr = function (id) {
        $scope.Order.StoreId = id;
        changeCityDistrictStoreEvent("storeInOtherAddr" + id);
    }

    changeCityDistrictStoreEvent = function (idElement) {
        var that = $("#" + idElement);
        var box = that.parent().parent().parent();
        if (!that.hasClass('selected')) {
            box.find('li').removeClass('selected');
            that.addClass('selected');
            box.removeClass('open');
            box.find('.fs-select-header h3').html(that.text());
            target = that.data('target');
        }
    }
    function showErrorFromServer(errorCodes) {
        $.each(errorCodes, function (index, value) {
            if (value == "FullName_Missing") {
                showErrorMessWithControl("fs_txt_name", "*Phải nhập vào họ và tên");
            }
            if (value == "Phone_Missing") {
                showErrorMessWithControl("fs_txt_name", "*Phải nhập vào số điện thoại");
            }
            if (value == "Email_Missing") {
                showErrorMessWithControl("fs_txt_email", "*Phải nhập vào email");
            }
            if (value == "Email_Invalid") {
                showErrorMessWithControl("fs_txt_email", "*Email không hợp lệ");
            }
            if (value == "Store_Missing") {
                $("#storeErrorGroup").addClass("fs-show-error");
                $("#errorPaymentType").html("Bạn phải chọn Cửa hàng!");
            }
            if (value == "Product_Missing") {
                showErrorMessWithControl("fs_txt_name", "*Phải chọn bộ sản phẩm");
            }
        });
    }
    function clearAllData() {
        clearError();
        $scope.Order.FullName = "";
        $scope.Order.Phone = "";
        $scope.Order.Email = "";
        $scope.Order.Token = "";
        $scope.Order.WinProductId = "";
        $scope.isPlayedGame = false;
    }
    var isProcessConfirm = false;
    function confirmOrderInDb(idPrePopup) {
        if (isProcessConfirm) {
            return;
        }
        isProcessConfirm = true;
        var url = mainUrl + "confirm-order/";
        $http.post(url, $scope.Order).then(function (response) {
            var result = response.data;
            isProcessConfirm = false;
            if (result.Success != undefined && result.Success == true) {
                clearAllData();
                $('#' + idPrePopup).fadeOut(300, function () {
                    $('#' + idPrePopup).removeClass('fs-show');
                    $('#completePurchaseOrderPopup').addClass('fs-show');
                    $('#completePurchaseOrderPopup').fadeIn(0);
                });                
            } else if (result.ErrorCodes != undefined && result.ErrorCodes.length > 0) {
                showErrorFromServer(result.ErrorCodes);
            }
        });
    }
    $scope.ignoreOpenChest = function () {
        confirmOrderInDb("popupGameConfirm");
    }

    $scope.confirmOrderStep2 = function () {
        var isError = validateConfirmOrder();
        if (isError) {
            isProcessConfirm = false;
            return;
        }
        
        if ($scope.isPlayedGame == true) {
            confirmOrderInDb('confirmStep2Popup');
        }
        else {
            isProcessConfirm = false;
            $('#confirmStep2Popup').fadeOut(300, function () {
                $('#confirmStep2Popup').removeClass('fs-show');
                $('#popupGameConfirm').addClass('fs-show');
                $('#popupGameConfirm').fadeIn(0);
            });
        }        
    }
    $scope.openChestConfirm = function () {
        $('#popupGameConfirm').fadeOut(300, function () {
            $('#popupGameConfirm').removeClass('fs-show');
            $('#popupGameOpenChestConfirm').addClass('fs-show');
            $('#popupGameOpenChestConfirm').fadeIn(0);
        });
    }


    var isProcessOpenChest = false;
    $scope.openChest = function () {
        if (isProcessOpenChest) {
            return;
        }
        isProcessOpenChest = true;
        if ($scope.isPlayedGame == false) {
            var url = mainUrl + "product-win";
            $http.get(url).then(function (response) {
                var result = response.data;
                isProcessOpenChest = false;
                if (result.ProductId != undefined) {
                    if (result.ProductId == 0) {
                        $('#popupGameOpenChestConfirm').fadeOut(300, function () {
                            $('#popupGameOpenChestConfirm').removeClass('fs-show');
                            $('#popupGameChestNotProductConfirm').addClass('fs-show');
                            $('#popupGameChestNotProductConfirm').fadeIn(0);
                        });
                    }
                    else if (result.ProductId == 4) {
                        $('#popupGameOpenChestConfirm').fadeOut(300, function () {
                            $('#popupGameOpenChestConfirm').removeClass('fs-show');
                            $('#popupGameWinLcdConfirm').addClass('fs-show');
                            $('#popupGameWinLcdConfirm').fadeIn(0);
                        });
                    }
                    else if (result.ProductId == 3) {
                        $('#popupGameOpenChestConfirm').fadeOut(300, function () {
                            $('#popupGameOpenChestConfirm').removeClass('fs-show');
                            $('#popupGameWinTvConfirm').addClass('fs-show');
                            $('#popupGameWinTvConfirm').fadeIn(0);
                        });
                    }
                    $scope.isPlayedGame = true;
                    $scope.Order.WinProductId = result.ProductId;
                    $scope.Order.Token = result.Token;                    
                } else if (result.ErrorCodes != undefined && result.ErrorCodes.length > 0) {
                    showErrorFromServer(result.ErrorCodes);
                }
            });
        }
    }

    $scope.continueAfterOpenChestWinTv = function () {
        
        confirmOrderInDb('popupGameWinTvConfirm');
    }

    $scope.continueAfterOpenChestWinLcd = function () {
        confirmOrderInDb('popupGameWinLcdConfirm');
    }

    $scope.continueAfterOpenChestNoProduct = function () {
        confirmOrderInDb('popupGameChestNotProductConfirm');
    }

    $scope.backToSelectGift = function () {
    }
    function hideAllPopup() {
        $('.is-form, .fs-payment').removeClass('active');
        $('#confirmStep2Popup').hide();  
        $('#confirmStep2Popup').hide();  
        $('#popupGameConfirm').hide();  
        $('#popupGameOpenChestConfirm').hide();  
        $('#popupGameChestNotProductConfirm').hide();  
        $('#popupGameWinLcdConfirm').hide();  
        $('#popupGameWinTvConfirm').hide();  
    }
    $scope.confirmOrder = function () {
        hideAllPopup();
        var isError = validateConfirmOrder();
        if (isError) {
            isProcessConfirm = false;
            return;
        }
        if (isProcessConfirm) {
            return;
        }
        isProcessConfirm = true;
        var url = mainUrl + "validate-order/";
        $http.post(url, $scope.Order).then(function (response) {
            var result = response.data;
            isProcessConfirm = false;
            if (result.Success != undefined && result.Success == true) {
                $('.is-form, .fs-payment').addClass('active');
                $('#confirmStep2Popup').fadeIn(0);               
                ImgLazyLoad();
                var delTop = 80;
                if (window.innerWidth <= 1100) {
                    delTop = 118;
                }
                var top = $('.is-form').offset().top - delTop;
                $('html,body').animate({ scrollTop: top }, 500);
            } else if (result.ErrorCodes != undefined && result.ErrorCodes.length > 0) {
                showErrorFromServer(result.ErrorCodes);
            }
        });
    }

    $scope.closePayment = function () {
        $('.is-form, .fs-payment').removeClass('active');
        setTimeout(function () {
            $('.fs-payment-box').css({ 'display': 'none' });
        }, 300);
    }
    function searchCustomerFunc() {
        $scope.customers = [];
        $scope.pageCustomer = 1;
        getCustomerData();
    }
    $scope.searchCustomer = function () {
        searchCustomerFunc();
    }

    $scope.searchCustomerEnter = function (event) {
        if (event.keyCode === 13) {
            searchCustomerFunc();
        }
    }
    function showErrorMessWithControl(controlId, message) {
        var control = $('#' + controlId);
        control.parent().addClass('fs-show-error');
        control.next().find("p").html(message);
    }

    function clearError() {
        $("#phoneErrorGroup").removeClass("fs-show-error");
        $("#nameErrorGroup").removeClass("fs-show-error");
        $("#emailErrorGroup").removeClass("fs-show-error");
        $("#storeErrorGroup").removeClass("fs-show-error");
        $("#txtNameError").html("");
        $("#txtPhoneError").html("");
        $("#txtEmailError").html("");
        $("#errorPaymentType").html("");
    }
    function validateConfirmOrder() {
        clearError();
        var isError = false;
        if ($scope.Order.FullName == null || $scope.Order.FullName == "") {
            showErrorMessWithControl("fs_txt_name", "*Phải nhập vào họ và tên");
            isError = true;
        }
        if ($scope.Order.Phone == null || $scope.Order.Phone == "") {
            showErrorMessWithControl("fs_txt_phone", "*Phải nhập vào số điện thoại");
            isError = true;
        }
        if ($scope.Order.Email == null || $scope.Order.Email == "") {
            showErrorMessWithControl("fs_txt_email", "*Phải nhập vào email");
            isError = true;
        }
        if ($scope.Order.PaymentType == "1") {
            if ($scope.Order.StoreId == 0) {
                $("#storeErrorGroup").addClass("fs-show-error");
                $("#errorPaymentType").html("Bạn phải chọn Cửa hàng!");
                isError = true;
            }
        }
        if ($scope.Order.ProductId == null || $scope.Order.ProductId == "") {
            showErrorMessWithControl("fs_txt_name", "*Phải chọn bộ sản phẩm");
            isError = true;
        }
        return isError;
    }
}]);