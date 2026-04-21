(function () {
    var preferredCityCookieName = "preferred_city";

    function getCulture() {
        var segments = window.location.pathname.split("/").filter(Boolean);
        return segments.length ? segments[0] : "uk";
    }

    function getCookie(name) {
        var prefix = name + "=";
        var cookies = document.cookie ? document.cookie.split("; ") : [];

        for (var i = 0; i < cookies.length; i++) {
            if (cookies[i].indexOf(prefix) === 0) {
                return decodeURIComponent(cookies[i].substring(prefix.length));
            }
        }

        return "";
    }

    function hasPreferredCity() {
        return !!getCookie(preferredCityCookieName);
    }

    function setPreferredCity(city) {
        var value = (city || "").trim();
        var expires = new Date();
        expires.setFullYear(expires.getFullYear() + 1);

        document.cookie =
            preferredCityCookieName + "=" + encodeURIComponent(value) +
            "; expires=" + expires.toUTCString() +
            "; path=/; SameSite=Lax";
    }

    function resolveCityFromCoords(culture, lat, lng) {
        var url = "/" + culture + "/api/geo/resolve-city?lat=" +
            encodeURIComponent(lat) +
            "&lng=" +
            encodeURIComponent(lng);

        return fetch(url, {
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        }).then(function (response) {
            return response.json();
        });
    }

    function getLocationBootstrap(culture) {
        return fetch("/" + culture + "/api/location/bootstrap", {
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        }).then(function (response) {
            return response.json();
        });
    }

    function initPreferredCityBootstrap() {
        var citySelect = document.getElementById("catalogGatewayCity");
        var message = document.getElementById("catalogDetectedCityMessage");

        if (!citySelect) {
            return;
        }

        if ((citySelect.value || "").trim()) {
            return;
        }

        if (hasPreferredCity()) {
            return;
        }

        var culture = getCulture();

        function applyCity(city, cityName) {
            if (!city) {
                return;
            }

            citySelect.value = city;
            setPreferredCity(city);

            if (message) {
                message.hidden = false;
                message.textContent = cityName
                    ? "Selected city: " + cityName
                    : "Selected city: " + city;
            }
        }

        function applyDefaultCity() {
            getLocationBootstrap(culture)
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
            resolveCityFromCoords(culture, position.coords.latitude, position.coords.longitude)
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

        if (!form || !citySelect) {
            return;
        }

        citySelect.addEventListener("change", function () {
            setPreferredCity(citySelect.value || "");
        });

        form.addEventListener("submit", function (e) {
            e.preventDefault();

            var culture = getCulture();
            var city = (citySelect.value || "").trim();

            if (!city) {
                citySelect.focus();
                return;
            }

            setPreferredCity(city);
            window.location.href = "/" + culture + "/" + encodeURIComponent(city);
        });
    }

    function initGeoDetection() {
        var button = document.getElementById("catalogDetectLocationBtn");
        var citySelect = document.getElementById("catalogGatewayCity");
        var message = document.getElementById("catalogDetectedCityMessage");

        if (!button || !citySelect || !message) {
            return;
        }

        button.addEventListener("click", function () {
            if (!navigator.geolocation) {
                return;
            }

            button.disabled = true;

            navigator.geolocation.getCurrentPosition(function (position) {
                var culture = getCulture();

                resolveCityFromCoords(culture, position.coords.latitude, position.coords.longitude)
                    .then(function (data) {
                        if (data && data.city) {
                            citySelect.value = data.city;
                            setPreferredCity(data.city);
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