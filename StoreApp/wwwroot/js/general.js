jQuery(document).ready(function ($) {

    var loginButton = $('.login100-form-btn');
    var user = getUserNameFromCookie();
    var text = $('.intro').find('#introduction').text("ברוכים הבאים " + user + "!");

    loginButton.on('click', function(event){
        removeCookies();
        var username = $('.wrap-login100').find('#username').val();
        console.log('welcome: ' + username+ ' !');
        setCookie('username', username, 1);
    });

    function setCookie(cname, cvalue, exdays) {
        var d = new Date();
        d.setDate(d.getDate() + exdays);
        var expires = "expires="+d.toUTCString();
        document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
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

    function removeCookies() {
        var res = document.cookie;
        var multiple = res.split(";");
        for(var i = 0; i < multiple.length; i++) {
            var key = multiple[i].split("=");
            document.cookie = key[0]+" =; expires = Thu, 01 Jan 1970 00:00:00 UTC";
        }
    }
});    