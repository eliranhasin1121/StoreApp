$(document).ready(function () {

    $("#btn-search4").click(function () {
        console.log('here');
        var address = $("#sup-address").val();

        var phone = $("#phone").val();

        var name = $("#prod").val();

        var data =
            {
                address: address,
                phone: phone,
                name: name
            }
        console.log(data);
        $.ajax({
            type: "POST",
            url: "searchBySupplier2",
            data: data,
            success: function (res) {
                console.log('HERE');
                console.log(res);

                if (!res) {
                    console.log('the data is not exist');

                } else {
                    console.log(res);

                    var len = parseInt($("#len").val());
                    console.log(typeof (len));
                    console.log(len);
                    for (let i = 0; i < len; i++) {
                        $("#t12").remove();

                    }
                    console.log(res);
                    for (let i = 0; i < res.length; i++) {

                       

                        let result = $(`<tr id="t12"><th id="t1">${res[i].value.comapnyName}</th><th id="t2">${res[i].value.address}</th><th id="t3">${res[i].value.phone}</th><th>${res[i].value.product}</th><th>${res[i].value.exsist}</th></tr>`);
                        $("#suptable").append(result);


                    }
                    $("#len").val(res.length);
                }




            }

        });

    });

});