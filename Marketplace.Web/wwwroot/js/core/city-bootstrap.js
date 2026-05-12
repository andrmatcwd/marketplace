(function () {
    'use strict';

    function initCityBootstrap(selectId, messageId) {
        var citySelect = document.getElementById(selectId);
        var message = messageId ? document.getElementById(messageId) : null;

        if (!citySelect || !window.locationContext) return;
        if ((citySelect.value || '').trim()) return;
        if (window.locationContext.hasPreferredCity()) return;

        var culture = window.locationContext.getCulture();

        function applyCity(city, cityName) {
            if (!city) return;
            citySelect.value = city;
            window.locationContext.setPreferredCity(city);
            if (message) {
                message.hidden = false;
                message.textContent = 'Selected city: ' + (cityName || city);
            }
        }

        function applyDefaultCity() {
            window.locationContext.getLocationBootstrap(culture)
                .then(function (data) {
                    if (data && data.city) applyCity(data.city, data.cityName);
                })
                .catch(function () {});
        }

        if (!navigator.geolocation) {
            applyDefaultCity();
            return;
        }

        navigator.geolocation.getCurrentPosition(
            function (position) {
                window.locationContext.resolveCityFromCoords(
                    culture,
                    position.coords.latitude,
                    position.coords.longitude
                )
                    .then(function (data) {
                        if (data && data.city) applyCity(data.city, data.cityName);
                        else applyDefaultCity();
                    })
                    .catch(applyDefaultCity);
            },
            applyDefaultCity,
            { enableHighAccuracy: false, timeout: 5000, maximumAge: 3600000 }
        );
    }

    function initGeoDetectButton(buttonId, selectId, messageId) {
        var button = document.getElementById(buttonId);
        var citySelect = document.getElementById(selectId);
        var message = messageId ? document.getElementById(messageId) : null;

        if (!button || !citySelect || !window.locationContext) return;

        button.addEventListener('click', function () {
            if (!navigator.geolocation) return;
            button.disabled = true;

            navigator.geolocation.getCurrentPosition(
                function (position) {
                    var culture = window.locationContext.getCulture();
                    window.locationContext.resolveCityFromCoords(
                        culture,
                        position.coords.latitude,
                        position.coords.longitude
                    )
                        .then(function (data) {
                            if (data && data.city) {
                                citySelect.value = data.city;
                                window.locationContext.setPreferredCity(data.city);
                                if (message) {
                                    message.hidden = false;
                                    message.textContent = 'Detected city: ' + (data.cityName || data.city);
                                }
                            } else if (message) {
                                message.hidden = false;
                                message.textContent = 'Could not detect a supported city.';
                            }
                        })
                        .catch(function () {
                            if (message) {
                                message.hidden = false;
                                message.textContent = 'Unable to detect city.';
                            }
                        })
                        .finally(function () {
                            button.disabled = false;
                        });
                },
                function () {
                    button.disabled = false;
                    if (message) {
                        message.hidden = false;
                        message.textContent = 'Location access was denied.';
                    }
                }
            );
        });
    }

    window.cityBootstrap = {
        initCityBootstrap: initCityBootstrap,
        initGeoDetectButton: initGeoDetectButton
    };
})();
