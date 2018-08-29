// Wrap your code in a function to avoid global scope and pass in any global objects,
// this sample use the "revealing" module pattern to expose "public" methods.
// You have to expose an object (here named store) and an "init" method with the exact same parameters.
// Those parameters are required to transmit a DNN Service Framework instance and options defined in the module.
// The Store Services module load and initialize your script using "ScriptObjectName" and "Options" settings.

// globals jQuery, window, document, undefined
var store = (function ($, window, document, undefined) {
    "use strict"

    // settings
    var _settings = {};
    // DNN Service Framework
    var _sf = {};
    // store service root URL
    var _root = null;

    // generic function to POST data to a service action
    function _postWebService(action, data, callback) {
        // here you could show a spinner

        // asynchronous call to the service/action
        $.ajax({
            type: "POST",
            url: _root + action,
            data: data,
            beforeSend: _sf.setModuleHeaders
        }).done(callback).fail(function (xhr, result, status) {
            // here you should use a better way to manage errors!
            alert("Error: " + status);
        }).always(function () {
            // here you could hide your spinner
            // remove the always method if not used!
        });
    }

    // generic function to GET data from a service action
    function _getWebService(action, callback) {
        // here you could show a spinner

        // asynchronous call to the service/action
        $.ajax({
            type: "GET",
            url: _root + action,
            dataType: "json",
            beforeSend: _sf.setModuleHeaders
        }).done(function (data, status, xhr) {
            if (data !== undefined && data != null) {
                // call the callback function with data
                callback(data);
            }
        }).fail(function (xhr, result, status) {
            // here you should use a better way to manage errors!
            alert("Error: " + status);
        }).always(function () {
            // here you could hide your spinner,
            // remove the always method if not used!
        });

    }

    // sample function to add a product in the user's cart
    function _addToCart(productId, quantity) {
        var action = "AddToCart/";
        var data = {
            ID: productId,
            Quantity: quantity
        };

        _postWebService(action, data, function () {
            alert("Product added to cart!");
        });
    }

    // sample function to update an item in the user's cart
    function _updateCart(itemId, quantity) {
        var action = "UpdateCart/";
        var data = {
            ID: itemId,
            Quantity: quantity
        };

        _postWebService(action, data, function () {
            alert("Cart item updated!");
        });
    }

    // sample function to verify if a product is in the user's cart
    function _isInCart(productId) {
        var action = "IsInCart/?productID=" + productId;

        _getWebService(action, function (data) {
            if (data == true)
                alert("Product is in cart!");
            else
                alert("Product is NOT in cart!");
        });
    }

    // sample function to retreive user's cart
    function _getCart() {
        var action = "GetCart/";

        _getWebService(action, function (cart) {
            alert("Content cart received!");
        });
    }

    // sample function to retrieve cart url
    function _getCartURL() {
        var action = "GetCartURL/";

        _getWebService(action, function (url) {
            alert("Cart URL: " + url);
        });
    }

    // wire up any plugins or other logic here
    function _setup (options) {
        // your initialization code here...
        // a typical use is for controls binding: $("dom_elem").clic(your_function);
    }

    // wire up the call to _setup function on document ready
    $(document).ready(function () {
        // Wire up the call to _setup function after an update panel request
        window.Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            _setup(_settings);
        });
    });

    // this is the "revealed" part of the module
    // call it from elsewhere: store.init(dnnSF, options);
    var exports = {};

    exports.init = function (dnnSF, options) {
        // define service framework
        _sf = dnnSF;
        // define store service root url
        _root = _sf.getServiceRoot("Store") + "Services/"
        
        // merge options with default values to define settings
        // here "message" and "isSet" are used for demo purpose!
        _settings = $.extend({
            message: null,
            isSet: false
        }, options);

        // setup module
        _setup(_settings);
    };

    return exports;

}(window.jQuery, window, document)); // pass in the globals. Note the safe access of the jQuery object.
