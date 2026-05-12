(function () {
    'use strict';

    function initGatewayForm() {
        var form = document.getElementById('catalogGatewayForm');
        var citySelect = document.getElementById('catalogGatewayCity');

        if (!form || !citySelect || !window.locationContext) return;

        citySelect.addEventListener('change', function () {
            window.locationContext.setPreferredCity(citySelect.value || '');
        });

        form.addEventListener('submit', function (e) {
            e.preventDefault();
            var culture = window.locationContext.getCulture();
            var city = (citySelect.value || '').trim();
            if (!city) {
                citySelect.focus();
                return;
            }
            window.locationContext.setPreferredCity(city);
            window.location.href = '/' + culture + '/' + encodeURIComponent(city);
        });
    }

    document.addEventListener('DOMContentLoaded', function () {
        if (window.cityBootstrap) {
            window.cityBootstrap.initCityBootstrap('catalogGatewayCity', 'catalogDetectedCityMessage');
            window.cityBootstrap.initGeoDetectButton('catalogDetectLocationBtn', 'catalogGatewayCity', 'catalogDetectedCityMessage');
        }
        initGatewayForm();
    });
})();
