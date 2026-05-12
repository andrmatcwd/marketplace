(function () {
    'use strict';

    function initHomeSearchForm() {
        var form = document.getElementById('homeSearchForm');
        if (!form || !window.locationContext) return;

        var citySelect = document.getElementById('homeCitySelect');
        if (citySelect) {
            citySelect.addEventListener('change', function () {
                window.locationContext.setPreferredCity(citySelect.value || '');
            });
        }

        form.addEventListener('submit', function (e) {
            e.preventDefault();

            var culture = window.locationContext.getCulture();
            var searchInput = document.getElementById('homeSearchInput');
            var select = document.getElementById('homeCitySelect');

            var search = searchInput ? searchInput.value.trim() : '';
            var city = select ? select.value.trim() : '';

            if (!city) {
                alert('Please select a city first');
                if (select) select.focus();
                return;
            }

            window.locationContext.setPreferredCity(city);

            if (!search) {
                window.location.href = '/' + culture + '/' + encodeURIComponent(city);
                return;
            }

            window.locationContext.resolveDirectRoute(culture, city, search)
                .then(function (data) {
                    var url = (data && data.canRouteDirect && data.url)
                        ? data.url
                        : window.locationContext.buildFallbackUrl(culture, city, search);
                    if (url) window.location.href = url;
                })
                .catch(function () {
                    var url = window.locationContext.buildFallbackUrl(culture, city, search);
                    if (url) window.location.href = url;
                });
        });
    }

    document.addEventListener('DOMContentLoaded', function () {
        if (window.cityBootstrap) {
            window.cityBootstrap.initCityBootstrap('homeCitySelect', 'detectedCityMessage');
            window.cityBootstrap.initGeoDetectButton('detectLocationBtn', 'homeCitySelect', 'detectedCityMessage');
        }
        initHomeSearchForm();
    });
})();
