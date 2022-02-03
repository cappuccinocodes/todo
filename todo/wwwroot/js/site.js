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