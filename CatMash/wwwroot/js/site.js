// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$('#imgGauche').click(function () {
    catId = $('#imgGauche').attr("val");

    callAjaxCats(catId);
});

$('#imgDroite').click(function () {
    catId = $('#imgDroite').attr("val");

    callAjaxCats(catId);
});

function callAjaxCats(catId) {
    var data = {
        ID: catId
    };

    $.ajax({
        type: "POST",
        url: '/Home/AddScoreToCat',
        contentType: "application/json",
        dataType: "json",
        data: JSON.stringify(data),
        success: function (data) {
            if (data.length === 3) {
                console.log(data[0].url);
                console.log(data[1].url);

                $('#imgGauche').attr("src", data[0].url);
                $('#imgGauche').attr("val", data[0].id);

                $('#imgDroite').attr("src", data[1].url);
                $('#imgDroite').attr("val", data[1].id);

                $('#votes').text(data[2].score + " votes");
            }
        }
    });
}
