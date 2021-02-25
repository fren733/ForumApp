var modal = document.getElementById("myModal");

// Get the button that opens the modal
var btn = document.getElementById("myBtn");

// Get the <span> element that closes the modal
var span = document.getElementsByClassName("close")[0];

function OpenModal() {

    var postid = $(event.target).attr("value");

    $.ajax({
        url: '/Post/CreatePost',
        type: 'GET',
        data: {
            PostID: postid
        },
        error: function (xhr) {
            alert('Error: ' + xhr.statusText);
        },
        success: function (result) {
            $('#content').html(result);
        }
    });
    modal.style.display = "block";
}

span.onclick = function () {
    modal.style.display = "none";
}

window.onclick = function (event) {
    if (event.target == modal) {
        modal.style.display = "none";
    }
}
