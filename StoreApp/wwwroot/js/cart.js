jQuery(document).ready(function ($) {
var cartWrapper = $('.cd-cart-container');
//product id - you don't need a counter in your real project but you can use your real product id
var productId = 0;
var productsCounter = 0;
var initial = true;
var productList = [];
var quantity = 0;

function initialFromLocalStorageCallback(callback) {
    var i = 0,
        value = {},
        sKey;
    for (; sKey = window.localStorage.key(i); i++) {
        value[sKey] = window.localStorage.getItem(sKey);
        var val = JSON.parse(value[sKey]);
        productList.push(val.productName);
    }
    callback(value);
}

function getUserNameFromCookie() {
    var name = "username=";
    var ca = document.cookie.split(';');
    for(var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

    //store jQuery objects
    var cartBody = cartWrapper.find('.body');
    var cartList = cartBody.find('ul').eq(0);
    var cartTotal = cartWrapper.find('.checkout').find('span');
    var submitOrder = cartWrapper.find('.checkout');
    var cartTrigger = cartWrapper.children('.cd-cart-trigger');
    var toggleMyCart = cartWrapper.children('.wrapButtons').find('.btn');
    var cartCount = cartTrigger.children('.count')
    var addToCartBtn = $('.cd-add-to-cart');
    var undo = cartWrapper.find('.undo');
    var undoTimeoutId;
    var cartIsEmpty = cartWrapper.hasClass('empty');
    
    if (cartIsEmpty) {
        initial = true;
        var json;
        initialFromLocalStorageCallback(function(value) {
            json = value;
        });
        for (var prop in json) {
            var t = JSON.parse(json[prop]);
            cartInitializer(t.productName, t.productPrice, t.productImage, t.productAmount);
        }
    }
    function cartInitializer(productName, productPrice, productImage, productAmount) {
        addProductStorage(productName, productPrice, productImage, productAmount);
        updateCartCount(true, productAmount);
        updateCartTotal(productPrice, true);
        
    }

    function addProductStorage(productName, productPrice, productImage, productAmount) {
        var productAdded = $(`<li class="product"> <div class="product-image"> <a href="#0"><img src= ${productImage}alt="placeholder"></a> </div> <div class="product-details" data-prodname="${productName}" data-prodimage="${productImage}" data-prodprice="${productPrice}" data-prodamount="${productAmount}"><p style="font-size:16px;"><a href="#0">${productName} </a> </p><span class="price">${productPrice}₪</span><div class="actions"><a href="#0" class="delete-item">מחק</a><div class="quantity"><label for="cd-product-'+ productId +'">כמות</label><span class="select"><select id="cd-product-'+ productId +'" name="quantity"><option> ${productAmount}</option><option value="2">2</option><option value="3">3</option><option value="4">4</option><option value="5">5</option><option value="6">6</option><option value="7">7</option><option value="8">8</option><option value="9">9</option></select></span></div></div></div></li>`);
        cartList.prepend(productAdded);
    }
    initial = false;

    //add product to cart
    addToCartBtn.on('click', function(event){
        event.preventDefault();
        addToCart($(this));
        var imageAddress = ($(this).data('image'));
        var price = ($(this).data('price'));
        var prodname = ($(this).data('productname'));
        var prodid = ($(this).data('prodid'));
    });
    
    submitOrder.on('click', function (event) {
        event.preventDefault();
        console.log('sending: ')
        var jsonToServer = submitCart();
        console.log('---------------JSON FILE---------------->' + JSON.stringify(jsonToServer));
        //alert('ההזמנה בוצעה בהצלחה, תודה אח שלי' + getUserNameFromCookie());
        var dataStringToServer = JSON.stringify(jsonToServer);
        $.get("http://localhost:51220/Products/submitOrder", { jsonData: dataStringToServer });
        var d = {
            api_key: 'e01dc417',
            api_secret: 'gfallg6tBRplY1G6',
            to: '972503673664',
            from: "NEXMO",
            text: `New order recieved from: ${getUserNameFromCookie()}.`
        }

        $.ajax({
            type: "POST",
            url: "https://rest.nexmo.com/sms/json",
            data: d
        });

        alert("ההזמנה נקלטה במערכת. תודה");
        productList = []; 
    })


    function submitCart() {
        var i = 0,
        resultAnswer = {};
        var submitJson = {},
        sKey;
        var username = "username";
        submitJson[username] = getUserNameFromCookie();
        for (; sKey = localStorage.key(i); i++) {
            submitJson[sKey] = localStorage.getItem(sKey);
            console.log(sKey +' ' + 'the key of ' + submitJson[sKey]);
        }
        localStorage.clear();
        location.reload();
        
        return (submitJson);
    }
    function addToCart(product) {
        var prodExist = false;
        var name = product.data('productname');
        for (var i = 0 ; i < productList.length ; i ++) {
            if (productList[i] == name) {
                prodExist = true;
                alert("המוצר הוכנס, הזמן כמות.");
                return;
            }
        }
        productList.push(name);
        var cartIsEmpty = cartWrapper.hasClass('empty');
        //update cart product list
        addProduct($(product));
        //update number of items 
        updateCartCount(cartIsEmpty);
        //update total price
        updateCartTotal(product.data('price'), true);
        //show cart
        cartWrapper.removeClass('empty');
    }
    function addProduct(prod) {
        var prodid = (prod.data('prodid'));
        productId = productId + 1;
        var imageAddress = (prod.data('image'));
        var price = (prod.data('price'));
        var prodname = (prod.data('productname'));
        if (initial==false) {
            updateLocalStorage(prodid ,prodname, price, imageAddress, 1);
        }
            var productAdded = $(`<li class="product"> <div class="product-image"> <a href="#0"><img src= ${imageAddress}alt="placeholder"></a> </div> <div class="product-details" data-prodname="${prodname}" data-prodimage="${imageAddress}" data-prodprice="${price}" data-prodamount="1"><p style="font-size:16px;"><a href="#0">${prodname} </a> </p><span class="price">${price}₪</span><div class="actions"><a href="#0" class="delete-item">מחק</a><div class="quantity"><label for="cd-product-'+ productId +'">כמות</label><span class="select"><select id="cd-product-'+ productId +'" name="quantity"><option>1</option><option value="2">2</option><option value="3">3</option><option value="4">4</option><option value="5">5</option><option value="6">6</option><option value="7">7</option><option value="8">8</option><option value="9">9</option></select></span></div></div></div></li>`);
            cartList.prepend(productAdded);
    }

    //open/close cart
    cartTrigger.on('click', function (event) {
        event.preventDefault();
        toggleCart();
    });

    toggleMyCart.on('click', function(event) {
        event.preventDefault();
        cartWrapper.removeClass('empty');
        cartCount.addClass('update-count');
        toggleCart(false);
    });


    //close cart when clicking on the .cd-cart-container::before (bg layer)
    cartWrapper.on('click', function(event){
        if( $(event.target).is($(this)) ) toggleCart(true);
    });

    //delete an item from the cart
    cartList.on('click', '.delete-item', function(event){
        event.preventDefault();
        removeProduct($(event.target).parents('.product'));
        removeFromLocalStorage($(event.target).parents('.product').children('.product-details'));
    });
    
    function removeFromLocalStorage(product) {
        var i = 0,
        value = {},
        sKey;
        productName = product.data('prodname');
    for (; sKey = window.localStorage.key(i); i++) {
        value[sKey] = window.localStorage.getItem(sKey);
        var val = JSON.parse(value[sKey]);
        if (val.productName == productName) {
            localStorage.removeItem(sKey);
            var index = productList.indexOf(productName);
            if (index > -1) {
                productList.splice(index, 1);
                }
            }
        }
    }

    function removeFromLocalArray(productName) {
        for (var i = 0 ; i < productList.length; i++) {
            if (productList[i] == productName) {
                productList.remove(i);
            }
        }
    }
    //update item quantity
    cartList.on('change', 'select', function(event){
        var product = $(event.target).parents('.product').children('.product-details');
        quickUpdateCart(true, product);
        var valu =cartList.children('li:not(.deleted)').find('select quantity').val();
        var id = $(event.target).parents('.product').children('.product-details').data('prodid');
        var name = $(event.target).parents('.product').children('.product-details').data('prodname');
        var price = $(event.target).parents('.product').children('.product-details').data('prodprice');
        var image = $(event.target).parents('.product').children('.product-details').data('prodimage');
        var amount =$(event.target).parents('.product').children('.product-details').data('prodamount');
        var theValue = $(this).find('select').val();
    });
    
    function ChangeValueInLocalStorage(name, amount) {
        var i = 0,
            oJson = {},
            sKey;
        for (; sKey = window.localStorage.key(i); i++) {
            var item = JSON.parse(localStorage.getItem(sKey));
            //product exist in localstorage
            if (item.productName == name) {
                var productPrice = item.productPrice;
                var productImage = item.productImage;
                var productAmount = amount;
                var id = sKey;
                var productName = item.productName;
                var dataToSave = {productName, productPrice, productImage, productAmount};
                localStorage.removeItem(id);
                localStorage.removeItem(id);
                localStorage.setItem(id, JSON.stringify(dataToSave));
                return;
            }
        }
    }

    //reinsert item deleted from the cart
    undo.on('click', 'a', function(event){
        clearInterval(undoTimeoutId);
        event.preventDefault();
        cartList.find('.deleted').addClass('undo-deleted').one('webkitAnimationEnd oanimationend msAnimationEnd animationend', function(){
            $(this).off('webkitAnimationEnd oanimationend msAnimationEnd animationend').removeClass('deleted undo-deleted').removeAttr('style');
            quickUpdateCart();
        });
        undo.removeClass('visible');
    });

function toggleCart(bool) {
    var cartIsOpen = ( typeof bool === 'undefined' ) ? cartWrapper.hasClass('cart-open') : bool;
    
    if( cartIsOpen ) {
        cartWrapper.removeClass('cart-open');
        //reset undo
        clearInterval(undoTimeoutId);
        undo.removeClass('visible');
        cartList.find('.deleted').remove();

        setTimeout(function(){
            cartBody.scrollTop(0);
            //check if cart empty to hide it
            if( Number(cartCount.find('li').eq(0).text()) == 0) cartWrapper.addClass('empty');
        }, 500);
    } else {
        cartWrapper.addClass('cart-open');
    }
}
function updateLocalStorage(productId ,productName, productPrice, productImage, productAmount) {

    var dataToSave = { productName, productPrice, productImage, productAmount };
    var i = 0,
        oJson = {},
        sKey;
    for (; sKey = window.localStorage.key(i); i++) {
        oJson[sKey] = window.localStorage.getItem(sKey);
        if (sKey == productId) {
            var item = JSON.parse(localStorage.getItem(sKey));
            //product exist in localstorage
            var currentAmount = item.productAmount + productAmount;
            productAmount = currentAmount;
            dataToSave = {productName, productPrice, productImage, productAmount};
            localStorage.removeItem(productId);
            localStorage.setItem(productId, JSON.stringify(dataToSave));
            return;
        }
    }
    localStorage.setItem(productId, JSON.stringify(dataToSave));
    productsCounter = productsCounter + 1;
}

function removeProduct(product) {
    clearInterval(undoTimeoutId);
    cartList.find('.deleted').remove();
    var topPosition = product.offset().top - cartBody.children('ul').offset().top ,
        productQuantity = Number(product.find('.quantity').find('select').val()),
        productTotPrice = Number(product.find('.price').text().replace('₪', '')) * productQuantity;
    
    product.css('top', topPosition+'px').addClass('deleted');

    //update items count + total price
    updateCartTotal(productTotPrice, false);
    updateCartCount(true, -productQuantity);
    undo.addClass('visible');

    //wait 8sec before completely remove the item
    undoTimeoutId = setTimeout(function(){
        undo.removeClass('visible');
        cartList.find('.deleted').remove();
    }, 8000);
}

function quickUpdateCart(bool, product) {
    var quantity = 0;
    var price = 0;
    var i = 0;
    if (bool==true) {
        var productName = product.data('prodname');
        cartList.children('li:not(.deleted)').each(function() {
            // catch the value that changed ::
            var singleQuantity = Number($(this).find('select').val());
            var x = $(this).find('product');
            var name = $(x.context).children('.product-details').data('prodname');
            var quantity = Number($(this).find('select').val());
            ChangeValueInLocalStorage(name, quantity);
            i = i + 1;
        });
    }
    cartList.children('li:not(.deleted)').each(function(){
        var singleQuantity = Number($(this).find('select').val());
        var temp = $(this).children('')
        quantity = quantity + singleQuantity;
        price = price + singleQuantity * Number($(this).find('.price').text().replace('₪', ''));
    });

    cartTotal.text(price.toFixed(2));
    cartCount.find('li').eq(0).text(quantity);
    cartCount.find('li').eq(1).text(quantity+1);
}

function updateCartCount(emptyCart, quantity) {
    if( typeof quantity === 'undefined' ) {
        var actual = Number(cartCount.find('li').eq(0).text()) + 1;
        var next = actual + 1;
        
        if( emptyCart ) {
            cartCount.find('li').eq(0).text(actual);
            cartCount.find('li').eq(1).text(next);
        } else {
            cartCount.addClass('update-count');

            setTimeout(function() {
                cartCount.find('li').eq(0).text(actual);
            }, 150);

            setTimeout(function() {
                cartCount.removeClass('update-count');
            }, 200);

            setTimeout(function() {
                cartCount.find('li').eq(1).text(next);
            }, 230);
        }
    } else {
        var actual = Number(cartCount.find('li').eq(0).text()) + quantity;
        var next = actual + 1;
        
        cartCount.find('li').eq(0).text(actual);
        cartCount.find('li').eq(1).text(next);
    }
}

function updateCartTotal(price, bool) {
    var t = price;
    bool ? cartTotal.text( (Number(cartTotal.text().replace('₪','')) +  Number(price.toFixed(2) ) ) ) 
    : cartTotal.text( (Number(cartTotal.text().replace('₪',''))) -  Number(price.toFixed(2) ) );
}
});