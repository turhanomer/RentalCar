// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function showLoading() {
    document.getElementById('loading').style.display = 'flex';
}

document.addEventListener('DOMContentLoaded', function () {
    document.getElementById('loading').style.display = 'none';
});
