$(document).ready(function () {

    $("#prod-search").click(function () {
        console.log('here');
        var prodId = $("#prodId").val();
        ;
        var data =
            {
                prodId: prodId
            }
        console.log(data);
        $.ajax({
            type: "POST",
            url: "prodSearch",
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


                    let result = $(`<tr id="t12"><th id="t1">${res.prodID}</th><th id="t2">${res.productName}</th><th id="t3">${res.productType}</th><th id="t4">${res.productAmount}</th><th id="t5">${res.productPrice}</th> <th id="t5">${res.productDescription}</th> <th id="t5">${res.supplierId}</th></tr>`);
                    $("#productable").append(result);
                    console.log('here 2');

                    prodID = query.ID,
                        productName = query.ProductName,
                        productType = query.ProductType,
                        productAmount = query.Amount,
                        productPrice = query.Price,
                        productDescription = query.Description,
                        supplierId = query.SupplierID

                    $("#len").val(res.length);
                }




            },

        });

    });

});