(function () {
    const STORAGE_KEY = "cookie_consent_accepted";

    function initCookieConsent() {
        const banner = document.getElementById("cookieConsent");
        const acceptBtn = document.getElementById("cookieAcceptBtn");

        if (!banner || !acceptBtn) {
            return;
        }

        const accepted = localStorage.getItem(STORAGE_KEY);

        if (!accepted) {
            banner.hidden = false;
        }

        acceptBtn.addEventListener("click", function () {
            localStorage.setItem(STORAGE_KEY, "true");

            banner.classList.add("cookie-consent-hide");

            setTimeout(function () {
                banner.hidden = true;
            }, 300);
        });
    }

    document.addEventListener("DOMContentLoaded", initCookieConsent);
})();