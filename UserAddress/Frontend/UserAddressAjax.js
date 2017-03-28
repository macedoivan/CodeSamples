App.services.userAddress = App.services.userAddress || {};

App.services.userAddress.getUserAddressBook = function (onSuccess, onError) {
    var url = "/api/useraddress/";

    var settings = {
        cache: false
        , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
        , dataType: "json"
        , success: onSuccess
        , error: onError
        , type: "GET"
    }
    $.ajax(url, settings);
}

App.services.userAddress.deleteUserAddress = function (addressId, onSuccess, onError) {
    var url = "/api/useraddress/" + addressId;
    var settings = {
        cache: false
        , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
        , dataType: "json"
        , success: onSuccess
        , error: onError
        , type: "DELETE"
    }
    $.ajax(url, settings);
}

App.services.userAddress.insert = function (data, onSuccess, onError) {
    var url = "/api/useraddress/";

    var settings = {
        cache: false
        , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
        , data: data
        , dataType: "json"
        , success: onSuccess
        , error: onError
        , type: "POST"
    }
    $.ajax(url, settings);
};

App.services.userAddress.updateDefault = function (addressId, onSuccess, onError) {
    var url = "/api/useraddress/" + addressId;
    var settings = {
        cache: false
        , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
        , dataType: "json"
        , success: onSuccess
        , error: onError
        , type: "PUT"
    }
    $.ajax(url, settings);
}

App.services.userAddress.getDefaultAddress = function (onSuccess, onError) {
    var url = "/api/useraddress/default/";

    var settings = {
        cache: false
        , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
        , dataType: "json"
        , success: onSuccess
        , error: onError
        , type: "GET"
    }
    $.ajax(url, settings);
}