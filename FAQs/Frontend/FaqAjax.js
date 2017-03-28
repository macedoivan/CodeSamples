App.services.help = App.services.help || {};

App.services.help.insert = function (data, onSuccess, onError) {
    var url = "/api/help/";

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

App.services.help.update = function (data, onSuccess, onError) {
    var url = "/api/help/";
    var settings = {
        cache: false
    , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
    , data: data
    , dataType: "json"
    , success: onSuccess
    , error: onError
    , type: "PUT"
    };

    $.ajax(url, settings);
};

App.services.help.updateOrder = function (data, onSuccess, onError) {
    var url = "/api/help/order";
    var settings = {
        cache: false
    , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
    , data: data
    , dataType: "json"
    , success: onSuccess
    , error: onError
    , type: "PUT"
    };

    $.ajax(url, settings);
};

App.services.help.get = function (onAjaxSuccess, onAjaxError) {
    //var url = App.services.apiPrefix + "/api/help/";
    var url = "/api/help/";
    var settings = {
        cache: false
            , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
            , dataType: "json"
            , success: onAjaxSuccess
            , error: onAjaxError
            , type: "GET"
    };

    $.ajax(url, settings);
};

App.services.help.getById = function (id, onSuccess, onError) {

    var url = "/api/help/" +id;
    var settings = {
        cache: false
    , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
    , dataType: "json"
    , success: onSuccess
    , error: onError
    , type: "GET"
    };

    $.ajax(url, settings);
};

App.services.help.getByWebsiteId = function (websiteId, onSuccess, onError) {

    var url = "/api/help/website/" + websiteId;
    var settings = {
        cache: false
    , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
    , dataType: "json"
    , success: onSuccess
    , error: onError
    , type: "GET"
    };

    $.ajax(url, settings);
};

App.services.help.delete = function (id, onSuccess, onError) {


   // var url = App.services.apiPrefix + "/api/help/" + id;
    var url = "/api/help/" +id;
    var settings = {
        cache: false
    , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
    , dataType: "json"
    , success: onSuccess
    , error: onError
    , type: "DELETE"
    };

    $.ajax(url, settings);
};