$(document).ready(function () {
    $('#upload_btn').attr("disabled", true);
    $('#upload_btn').attr("title", "Please select a XML/CSV file");

    $('#file').change(function () {
        var value = $('#file[type=file]').val();
        if (value) {
            $('#upload_btn').css("background-color", "green");
            $('#upload_btn').attr("disabled", false);
            $('.input_file').css("background-color", "green");
        }
    });
});  

$('#export_btn').click(function () {
    alert("File has been exported. Please note that u have to change the file name or it will be overwritten.");
});