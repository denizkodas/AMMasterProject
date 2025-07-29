$(document).ready(function () {
    $('#ListingLoad').DataTable({
        "order": [[0, 'asc']]
    });
});

$(document).ready(function () {
    $('#ListingLoad1').DataTable({
        "order": [[0, 'asc']]
    });
});



$(document).ready(function () {
    $('#ListingLoad2').DataTable({
        "order": [[0, 'asc']]
    });
});

$(document).ready(function () {
    $('#ListingLoad3').DataTable({
        "order": [[0, 'asc']]
    });
});

$(document).ready(function () {
    $('#ListingLoaddesc').DataTable({
        "order": [[0, 'desc']]
    });
});




$(document).ready(function () {
    $('#ListingLoad1desc').DataTable({
        "order": [[0, 'desc']]
    });
});




$(document).ready(function () {
    $('#ListingLoad2desc').DataTable({
        "order": [[0, 'desc']]
    });
});



$(document).ready(function () {
    $('#ListingLoad3desc').DataTable({
        "order": [[0, 'desc']]
    });
});


//if need to change order
//$(document).ready(function () {
//    var table = $('#ListingLoad').DataTable();

//    if ($.fn.dataTable.isDataTable('#ListingLoad')) {
//        table.destroy();
//    }

//    table.DataTable({
//        "order": [[0, 'desc']]
//        // Other options...
//    });
//});