(function () {
    var preferredCityCookieName = "preferred_city";

    function setPreferredCity(city) {
        var value = (city || "").trim();
        var expires = new Date();
        expires.setFullYear(expires.getFullYear() + 1);

        document.cookie =
            preferredCityCookieName + "=" + encodeURIComponent(value) +
            "; expires=" + expires.toUTCString() +
            "; path=/; SameSite=Lax";
    }

    document.addEventListener("click", function (e) {
        var link = e.target.closest("[data-city-slug]");
        if (!link) {
            return;
        }

        var city = link.getAttribute("data-city-slug");
        if (!city) {
            return;
        }

        setPreferredCity(city);
    });
})();