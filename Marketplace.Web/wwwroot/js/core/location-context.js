(function (window) {
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

    function buildFallbackUrl(culture, city, search) {
        var trimmedSearch = (search || "").trim();
        var trimmedCity = (city || "").trim();

        if (!trimmedCity) {
            return null;
        }

        if (trimmedSearch) {
            return "/" + culture + "/" +
                encodeURIComponent(trimmedCity) +
                "?search=" +
                encodeURIComponent(trimmedSearch);
        }

        return "/" + culture + "/" + encodeURIComponent(trimmedCity);
    }

    function resolveDirectRoute(culture, city, search) {
        var url = "/" + culture + "/api/listings/resolve-route?city=" +
            encodeURIComponent(city) +
            "&search=" +
            encodeURIComponent(search);

        return fetch(url, {
            method: "GET",
            headers: { "Accept": "application/json" }
        }).then(function (response) {
            return response.json();
        });
    }

    function resolveCityFromCoords(culture, lat, lng) {
        var url = "/" + culture + "/api/geo/resolve-city?lat=" +
            encodeURIComponent(lat) +
            "&lng=" +
            encodeURIComponent(lng);

        return fetch(url, {
            method: "GET",
            headers: { "Accept": "application/json" }
        }).then(function (response) {
            return response.json();
        });
    }

    function getLocationBootstrap(culture) {
        return fetch("/" + culture + "/api/location/bootstrap", {
            method: "GET",
            headers: { "Accept": "application/json" }
        }).then(function (response) {
            return response.json();
        });
    }

    window.locationContext = {
        getCulture: getCulture,
        getCookie: getCookie,
        hasPreferredCity: hasPreferredCity,
        setPreferredCity: setPreferredCity,
        buildFallbackUrl: buildFallbackUrl,
        resolveDirectRoute: resolveDirectRoute,
        resolveCityFromCoords: resolveCityFromCoords,
        getLocationBootstrap: getLocationBootstrap
    };
})(window);