$(function () {
    $('#TestItem').submit(function () {
        $(this).correlatedSubmit();
        return false;
    });
});