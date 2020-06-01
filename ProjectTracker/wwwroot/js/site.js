// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function GetMemeType(filename) {

    var ext = filename.split('.').pop();

    switch (ext.toLowerCase()) {
        case "txt":
            return "text/plain";
        case "pdf":
            return "application/pdf";
        case "doc":
            return "application/vnd.ms-word";
        case "docx":
            return "application/vnd.ms-word";
        case "png":
            return "image/png";
        case "jpg":
            return "image/jpeg";
        default:
            return "application/octet-stream";
    }
}

function ConfirmDialog() {
    if (window.confirm("Are you sure you want to proceed with this action?")) {
        return true;
    } else {
        return false;
    }
}