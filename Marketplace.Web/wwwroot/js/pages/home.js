(function () {
    function initPreferredCityBootstrap() {
        var citySelect = document.getElementById("homeCitySelect");
        var message = document.getElementById("detectedCityMessage");

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

    function initHomeSearchForm() {
        var form = document.getElementById("homeSearchForm");
        if (!form || !window.locationContext) {
            return;
        }

        var citySelect = document.getElementById("homeCitySelect");
        if (citySelect) {
            citySelect.addEventListener("change", function () {
                window.locationContext.setPreferredCity(citySelect.value || "");
            });
        }

        form.addEventListener("submit", function (e) {
            e.preventDefault();

            var culture = window.locationContext.getCulture();
            var searchInput = document.getElementById("homeSearchInput");
            var citySelect = document.getElementById("homeCitySelect");

            var search = searchInput ? searchInput.value.trim() : "";
            var city = citySelect ? citySelect.value.trim() : "";

            if (!city) {
                alert("Please select a city first");
                if (citySelect) {
                    citySelect.focus();
                }
                return;
            }

            window.locationContext.setPreferredCity(city);

            if (!search) {
                window.location.href = "/" + culture + "/" + encodeURIComponent(city);
                return;
            }

            window.locationContext.resolveDirectRoute(culture, city, search)
                .then(function (data) {
                    if (data && data.canRouteDirect && data.url) {
                        window.location.href = data.url;
                        return;
                    }

                    var fallbackUrl = window.locationContext.buildFallbackUrl(culture, city, search);
                    if (fallbackUrl) {
                        window.location.href = fallbackUrl;
                    }
                })
                .catch(function () {
                    var fallbackUrl = window.locationContext.buildFallbackUrl(culture, city, search);
                    if (fallbackUrl) {
                        window.location.href = fallbackUrl;
                    }
                });
        });
    }

    function initGeoDetection() {
        var button = document.getElementById("detectLocationBtn");
        var citySelect = document.getElementById("homeCitySelect");
        var message = document.getElementById("detectedCityMessage");

        if (!button || !citySelect || !window.locationContext) {
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

                            if (message) {
                                message.hidden = false;
                                message.textContent = "Detected city: " + (data.cityName || data.city);
                            }
                        } else if (message) {
                            message.hidden = false;
                            message.textContent = "Could not detect a supported city.";
                        }
                    })
                    .catch(function () {
                        if (message) {
                            message.hidden = false;
                            message.textContent = "Unable to detect city.";
                        }
                    })
                    .finally(function () {
                        button.disabled = false;
                    });
            }, function () {
                button.disabled = false;

                if (message) {
                    message.hidden = false;
                    message.textContent = "Location access was denied.";
                }
            });
        });
    }

    document.addEventListener("DOMContentLoaded", function () {
        initPreferredCityBootstrap();
        initHomeSearchForm();
        initGeoDetection();
    });
})();