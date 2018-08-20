jQuery(document).ready(function ($) {

    function getProductFromLocalStorage() {
        var i = 0,
            oJson = {},
            sKey;
        for (; sKey = window.localStorage.key(i); i++) {
            oJson[sKey] = window.localStorage.getItem(sKey);
            //Printing my localstorage cart :
            console.log(oJson[sKey]);
        }
        return oJson;
    }    
    //myContainer
    var cartWrapper = $('.cd-cart-container');

    if (cartWrapper.length == 0) {
        var productsArray = getProductsFromLocalStorage;
    }
    var cartBody = cartWrapper.find('.body');



}