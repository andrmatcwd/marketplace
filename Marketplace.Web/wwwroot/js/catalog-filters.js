(function () {
    'use strict';

    document.addEventListener('DOMContentLoaded', function () {
        document.querySelectorAll('.filter-bar').forEach(function (form) {
            form.addEventListener('change', function (e) {
                if (e.target.tagName === 'SELECT') {
                    var pageInput = form.querySelector('input[name="page"]');
                    if (pageInput) pageInput.value = '1';
                }
            });

            form.addEventListener('submit', function () {
                var pageInput = form.querySelector('input[name="page"]');
                if (!pageInput) {
                    var input = document.createElement('input');
                    input.type = 'hidden';
                    input.name = 'page';
                    input.value = '1';
                    form.appendChild(input);
                }
            });
        });
    });
})();
