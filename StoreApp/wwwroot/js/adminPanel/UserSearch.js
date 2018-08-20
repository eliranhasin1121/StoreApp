$(document).ready(function () {

    $("#usr-search").click(function () {
        console.log('here');
        var usrId = $("#userId").val();
       ;
        var data =
            {
                usrId:usrId
            }
        console.log(data);
        $.ajax({
            type: "POST",
            url: "userSearch",
            data: data,
            success: function (res) {
                console.log('HERE');
                console.log(res);

                if (!res) {
                    console.log('the data is not exist');

                } else {

                    var len = parseInt($("#len").val());
                    console.log(typeof (len));
                    console.log(len);
                        $("#t12").remove();
          

                        let result = $(`<tr id="t12"><th id="t1">${res.userID}</th><th id="t2">${res.userName}</th><th id="t3">${res.firstName}</th><th id="t4">${res.lastName}</th><th id="t5">${res.email}</th></tr>`);
                        $("#productable").append(result);
                        console.log('here 2');


           
                    $("#len").val(res.length);
                }




            },

        });

    });

});