$(document).ready(function () {

    $("#btn-search3").click(function () {
        console.log('here');
        var productType = $("#product2").val();
        console.log(productType);
        var supplier = $("#supplier").val();
        console.log(min);
        var name = $("#partName").val();

        var data =
        {
            supplierName: supplier,
            prodtype: productType,
            partName: name
        }
        console.log(data);
        $.ajax({
            type: "POST",
            url: "searchBySupplier",
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

                    for (let i = 0; i < res.length; i++) {

                        

                        let result = $(`<tr id="t12"><th id="t1">${res[i].value.productName}</th><th id="t2">${res[i].value.type}</th><th id="t3">${res[i].value.price}</th><th>${res[i].value.supplierName}</th></tr>`);
                        $("#productable").append(result);


                    }
                    $("#len").val(res.length);
                }




            }

        });

    });

});