(function () {
    'use strict';

    document.addEventListener('DOMContentLoaded', function () {
        var main = document.querySelector('.listing-gallery-main-image');
        var thumbs = document.querySelectorAll('.listing-gallery-thumb');

        if (!main || !thumbs.length) return;

        thumbs.forEach(function (thumb) {
            thumb.style.cursor = 'pointer';
            thumb.addEventListener('click', function () {
                main.src = thumb.src;
                main.alt = thumb.alt || '';
            });
        });
    });
})();
