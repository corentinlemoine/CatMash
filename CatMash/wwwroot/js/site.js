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
    $.ajax({
        type: "POST",
        url: '/Home/AddScoreToCat',
        contentType: "application/json",
        dataType: "json",
        data: JSON.stringify(catId),
        success: function (data) {
            console.log(data["nextCat1"].url);
            console.log(data["nextCat1"].url);

            $('#imgGauche').attr("src", data["nextCat1"].url);
            $('#imgGauche').attr("val", data["nextCat1"].id);

            $('#imgDroite').attr("src", data["nextCat2"].url);
            $('#imgDroite').attr("val", data["nextCat2"].id);

            $('#votes').text(data["votes"] + " votes");
        }
    });
}
