
$(document).ready(function () {

    $('#login-button').click(function () {
        $('#login-button').fadeOut("slow", function () {
            $("#container").fadeIn();
        });
    });

    $(".close-btn").click(function () {
        $("#container, #forgotten-container").fadeOut(800, function () {
            $("#login-button").fadeIn(800);
        });
    });

    $("#loginbutton").click(function () {
        var user = ($('#username').val());
        var pass =($('#password').val());
        console.log('clicked..');
        if (user && pass) {
            var auth = {
                username: user,
                password: pass
            }
            var stringJson = JSON.stringify(auth);
            console.log(auth);
            $.post("AuthUser", { user: stringJson }, function (result) {
                if (result === "true") {
                    window.location.replace("http://localhost:51220/adminpanel/index");
                } else {
                    alert("invalid input !");
                    window.location.replace("http://localhost:51220/adminpanel/login");
                }
            });
        }
    })

    /* Forgotten Password */
    $('#forgotten').click(function () {
        $("#container").fadeOut(function () {
            $("#forgotten-container").fadeIn();
        });
    });
});