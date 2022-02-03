function deleteTodo(i) 
{

    $.ajax({
        url: 'Todo/Delete',
        type: 'POST',
        data: {
            id: i
        },
        success: function(response) {
            alert('success' + response);
            window.location.reload();
        },
        error: function() {
            alert('fail');
        }
    });
}

function populateForm(i) {

    $.ajax({
        url: 'Todo/PopulateForm',
        type: 'GET',
        data: {
            id: i
        },
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        success: function (response) {
            alert('success pop' + response.name);
            $("#Todo_Name").val(response.name);
        },
        error: function () {
            alert('fail');
        }
    });
}