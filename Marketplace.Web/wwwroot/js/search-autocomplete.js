(function () {
    'use strict';

    document.addEventListener('DOMContentLoaded', function () {
        var input = document.getElementById('search');
        if (!input) return;

        var box = document.createElement('div');
        box.className = 'search-autocomplete';
        box.setAttribute('role', 'listbox');
        box.setAttribute('aria-label', 'Search suggestions');
        input.insertAdjacentElement('afterend', box);

        input.setAttribute('autocomplete', 'off');
        input.setAttribute('aria-autocomplete', 'list');
        input.setAttribute('aria-haspopup', 'listbox');

        var timer = null;

        function getCulture() {
            var segments = window.location.pathname.split('/').filter(Boolean);
            return segments.length ? segments[0] : 'uk';
        }

        function getCurrentCity() {
            var segments = window.location.pathname.split('/').filter(Boolean);
            return segments.length >= 2 ? segments[1] : '';
        }

        function escapeHtml(str) {
            var d = document.createElement('div');
            d.textContent = str;
            return d.innerHTML;
        }

        function clearBox() {
            box.innerHTML = '';
            box.classList.remove('is-visible');
            input.removeAttribute('aria-expanded');
        }

        function renderItems(items) {
            box.innerHTML = items.map(function (item) {
                var badge = item.category
                    ? '<span class="search-autocomplete-city">' + escapeHtml(item.category) + '</span>'
                    : '';
                return '<a class="search-autocomplete-item" href="' + escapeHtml(item.url) + '" role="option">' +
                    '<span class="search-autocomplete-title">' + escapeHtml(item.label) + '</span>' +
                    badge +
                    '</a>';
            }).join('');
            box.classList.add('is-visible');
            input.setAttribute('aria-expanded', 'true');
        }

        input.addEventListener('input', function () {
            clearTimeout(timer);
            var query = input.value;

            if (!query || query.length < 2) {
                clearBox();
                return;
            }

            timer = setTimeout(function () {
                var culture = getCulture();
                var city = getCurrentCity();
                var url = '/' + culture + '/api/listings/autocomplete' +
                    '?search=' + encodeURIComponent(query) +
                    '&city=' + encodeURIComponent(city);

                fetch(url)
                    .then(function (r) { return r.json(); })
                    .then(function (items) {
                        if (!items || !items.length) { clearBox(); return; }
                        renderItems(items);
                    })
                    .catch(clearBox);
            }, 180);
        });

        document.addEventListener('click', function (e) {
            if (!e.target.closest('.filter-bar-group')) clearBox();
        });

        input.addEventListener('keydown', function (e) {
            if (e.key === 'Escape') clearBox();
        });
    });
})();
