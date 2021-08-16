var loadFile = function (event) {
    var output = document.getElementById('img-preview');
    output.src = URL.createObjectURL(event.target.files[0]);
    output.className = "img-thumbnail img-responsive img-preview"; 
    output.onload = function () {
        URL.revokeObjectURL(output.src)
    }
}