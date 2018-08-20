$(document).ready(function () {

    $("#sup-search").click(function () {
        console.log('here');
        var supId = $("#supId").val();
        ;
        var data =
            {
                supId: supId
            }
        console.log(data);
        $.ajax({
            type: "POST",
            url: "supplierSearch",
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


                    let result = $(`<tr id="t12"><th id="t1">${res.id}</th><th id="t2">${res.companyName}</th><th id="t4">${res.address}</th><th id="t5">${res.phoneNumber}</th></tr>`);
                    $("#productable").append(result);
                    console.log('here 2');

                

                    $("#len").val(res.length);
                }




            },

        });

    });

});