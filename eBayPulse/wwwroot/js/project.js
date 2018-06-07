$(document).ready(function () {
    $(function () {
        $("#btnGetItem").click(
            function () {
                var msg = $("#message").val();
                if (msg == "") {
                    alert("Add item Id");
                    return;
                }
                var message = { msg: msg };
                $.post("/Home/Index", message, addToItemsList);
            });
    });
});
function addToItemsList(str) {
    var data = str.split(';');
    var formatData = '';
    data.forEach(function(element) {
        formatData += "<td>" + element + "</td>";
      });
      formatData = "<tr>"+ formatData + "</tr>"
    $("#ItemsTable > tbody").append(formatData);
};