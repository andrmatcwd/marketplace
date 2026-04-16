(function () {
    function getCulture() {
        var segments = window.location.pathname.split("/").filter(Boolean);
        return segments.length ? segments[0] : "uk";
    }

    function buildTargetUrl(culture, city, search) {
        var trimmedSearch = (search || "").trim();
        var trimmedCity = (city || "").trim();

        if (trimmedCity && trimmedSearch) {
            return "/" + culture + "/catalog?city=" +
                encodeURIComponent(trimmedCity) +
                "&search=" +
                encodeURIComponent(trimmedSearch);
        }

        if (trimmedCity) {
            return "/" + culture + "/" + encodeURIComponent(trimmedCity);
        }

        if (trimmedSearch) {
            return "/" + culture + "/catalog?search=" + encodeURIComponent(trimmedSearch);
        }

        return "/" + culture + "/catalog";
    }

    function initHomeSearchForm() {
        var form = document.getElementById("homeSearchForm");
        if (!form) {
            return;
        }

        form.addEventListener("submit", function (e) {
            e.preventDefault();

            var culture = getCulture();
            var searchInput = document.getElementById("homeSearchInput");
            var citySelect = document.getElementById("homeCitySelect");

            var search = searchInput ? searchInput.value : "";
            var city = citySelect ? citySelect.value : "";

            window.location.href = buildTargetUrl(culture, city, search);
        });
    }

    function initGeoDetection() {
        var button = document.getElementById("detectLocationBtn");
        var citySelect = document.getElementById("homeCitySelect");
        var message = document.getElementById("detectedCityMessage");

        if (!button || !citySelect || !message) {
            return;
        }

        button.addEventListener("click", function () {
            if (!navigator.geolocation) {
                return;
            }

            button.disabled = true;
            button.textContent = "Detecting...";

            navigator.geolocation.getCurrentPosition(function (position) {
                var culture = getCulture();
                var url = "/" + culture + "/api/geo/resolve-city?lat=" +
                    encodeURIComponent(position.coords.latitude) +
                    "&lng=" +
                    encodeURIComponent(position.coords.longitude);

                fetch(url, {
                    method: "GET",
                    headers: {
                        "Accept": "application/json"
                    }
                })
                    .then(function (response) {
                        return response.json();
                    })
                    .then(function (data) {
                        if (data && data.city) {
                            citySelect.value = data.city;
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
                        button.textContent = "Detect my city";
                    });
            }, function () {
                button.disabled = false;
                button.textContent = "Detect my city";
                message.hidden = false;
                message.textContent = "Location access was denied.";
            });
        });
    }

    document.addEventListener("DOMContentLoaded", function () {
        initHomeSearchForm();
        initGeoDetection();
    });
})();