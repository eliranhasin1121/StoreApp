$(document).ready(function () {

	$("#btn-search2").click(function () {
		console.log('here');
		var userId = $("#userId").val();
		console.log(userId);
		var begin = $("#begin").val();
		console.log(begin);
		var end = $("#end").val();
		console.log(end);
		var data =
			{
				userId: userId,
				begin: begin,
				end: end
			}
		console.log(data);
		$.ajax({
			type: "POST",
			url: "searchByUser",
			data: data,
			success: function (res) {

				console.log(res);
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