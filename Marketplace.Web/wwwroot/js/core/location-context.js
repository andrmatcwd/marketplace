(function (window) {
    const COOKIE_NAME = 'preferredCity';

    function getCulture() {
        const path = window.location.pathname.split('/');
        return path.length > 1 && path[1] ? path[1] : 'uk';
    }

    function getCookie(name) {
        const match = document.cookie.match(new RegExp('(^| )' + name + '=([^;]+)'));
        return match ? decodeURIComponent(match[2]) : null;
    }

    function setPreferredCity(slug) {
        document.cookie = `${COOKIE_NAME}=${encodeURIComponent(slug)};path=/;max-age=31536000`;
    }

    function hasPreferredCity() {
        return !!getCookie(COOKIE_NAME);
    }

    async function resolveCityFromCoords(lat, lng) {
        const culture = getCulture();

        const res = await fetch(`/${culture}/api/resolve-route?lat=${lat}&lng=${lng}`);
        if (!res.ok) return null;

        const data = await res.json();
        return data?.citySlug || null;
    }

    async function detectCity() {
        return new Promise((resolve) => {
            if (!navigator.geolocation) {
                resolve(null);
                return;
            }

            navigator.geolocation.getCurrentPosition(async (pos) => {
                const city = await resolveCityFromCoords(
                    pos.coords.latitude,
                    pos.coords.longitude
                );
                resolve(city);
            }, () => resolve(null));
        });
    }

    window.locationContext = {
        getCulture,
        getCookie,
        setPreferredCity,
        hasPreferredCity,
        resolveCityFromCoords,
        detectCity
    };

})(window);