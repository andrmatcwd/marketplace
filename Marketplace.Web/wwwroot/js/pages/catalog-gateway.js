(function () {
    function initPreferredCityBootstrap() {
        var citySelect = document.getElementById("catalogGatewayCity");
        var message = document.getElementById("catalogDetectedCityMessage");

        if (!citySelect || !window.locationContext) {
            return;
        }

        if ((citySelect.value || "").trim()) {
            return;
        }

        if (window.locationContext.hasPreferredCity()) {
            return;
        }

        var culture = window.locationContext.getCulture();

        function applyCity(city, cityName) {
            if (!city) {
                return;
            }

            citySelect.value = city;
            window.locationContext.setPreferredCity(city);

            if (message) {
                message.hidden = false;
                message.textContent = cityName
                    ? "Selected city: " + cityName
                    : "Selected city: " + city;
            }
        }

        function applyDefaultCity() {
            window.locationContext.getLocationBootstrap(culture)
                .then(function (data) {
                    if (data && data.city) {
                        applyCity(data.city, data.cityName);
                    }
                })
                .catch(function () {
                    // no-op
                });
        }

        if (!navigator.geolocation) {
            applyDefaultCity();
            return;
        }

        navigator.geolocation.getCurrentPosition(function (position) {
            window.locationContext.resolveCityFromCoords(
                culture,
                position.coords.latitude,
                position.coords.longitude
            )
                .then(function (data) {
                    if (data && data.city) {
                        applyCity(data.city, data.cityName);
                    } else {
                        applyDefaultCity();
                    }
                })
                .catch(function () {
                    applyDefaultCity();
                });
        }, function () {
            applyDefaultCity();
        }, {
            enableHighAccuracy: false,
            timeout: 5000,
            maximumAge: 3600000
        });
    }

    function initGatewayForm() {
        var form = document.getElementById("catalogGatewayForm");
        var citySelect = document.getElementById("catalogGatewayCity");

        if (!form || !citySelect || !window.locationContext) {
            return;
        }

        citySelect.addEventListener("change", function () {
            window.locationContext.setPreferredCity(citySelect.value || "");
        });

        form.addEventListener("submit", function (e) {
            e.preventDefault();

            var culture = window.locationContext.getCulture();
            var city = (citySelect.value || "").trim();

            if (!city) {
                citySelect.focus();
                return;
            }

            window.locationContext.setPreferredCity(city);
            window.location.href = "/" + culture + "/" + encodeURIComponent(city);
        });
    }

    function initGeoDetection() {
        var button = document.getElementById("catalogDetectLocationBtn");
        var citySelect = document.getElementById("catalogGatewayCity");
        var message = document.getElementById("catalogDetectedCityMessage");

        if (!button || !citySelect || !message || !window.locationContext) {
            return;
        }

        button.addEventListener("click", function () {
            if (!navigator.geolocation) {
                return;
            }

            button.disabled = true;

            navigator.geolocation.getCurrentPosition(function (position) {
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
                            message.hidden = false;
                            message.textContent = "Detected city: " + (data.cityName || data.city);
                        } else {
                            message.hidden = false;
                            message.textContent = "Could not detect a supported city.";
                        }
                    })
                    .catch(function () {
                        message.hidden = false;
                        message.textContent = "Unable to detect city.";
                    })
                    .finally(function () {
                        button.disabled = false;
                    });
            }, function () {
                button.disabled = false;
                message.hidden = false;
                message.textContent = "Location access was denied.";
            });
        });
    }

    document.addEventListener("DOMContentLoaded", function () {
        initPreferredCityBootstrap();
        initGatewayForm();
        initGeoDetection();
    });
})();