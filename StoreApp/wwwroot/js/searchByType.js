$(document).ready(function () {

    $("#btn-search").click(function () {
        console.log('here');
        var productType = $("#product").val();
        console.log(productType);
        var min = $("#min").val();
        console.log(min);
        var max = $("#max").val();
        console.log(max);
        var data =
        {
            productType: productType,
            min: min,
            max: max
        }
        console.log(data);
        $.ajax({
            type: "POST",
            url: "searchByPriceAndType",
            data: data,
            success: function (res) {
 

                if (!res) {
                    console.log('the data is not exist');

                } else {

                    var len = parseInt($("#len").val());
                  
                    for (let i = 0; i < len; i++) {
                        $("#t12").remove();

                    }

                    for (let i = 0; i < res.length; i++) {

                        console.log(res[i].productName);

                        let result = $(`<tr id="t12"><th id="t1">${res[i].productName}</th><th id="t2">${res[i].productType}</th><th id="t3">${res[i].price.toString()}</th><td><img src="~/images/products/Meat/1.jpg" alt="23" style="width:150px; height:150px /></th></tr>`);
                        $("#productable").append(result);


                    }
                    $("#len").val(res.length);
                }




            },

        });

    });

});