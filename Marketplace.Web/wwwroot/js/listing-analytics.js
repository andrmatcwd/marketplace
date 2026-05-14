(function () {
    function sendEvent(name, params) {
        if (typeof gtag === "undefined") return;
        gtag("event", name, params);
    }

    document.addEventListener("click", function (e) {
        var phone = e.target.closest(".listing-phone-link");
        if (phone) {
            sendEvent("phone_click", {
                listing_id:    phone.dataset.listingId,
                listing_title: phone.dataset.listingTitle,
                phone_number:  phone.dataset.phone
            });
            return;
        }

        var website = e.target.closest(".listing-website-link");
        if (website) {
            sendEvent("website_click", {
                listing_id:    website.dataset.listingId,
                listing_title: website.dataset.listingTitle,
                website_url:   website.dataset.website
            });
        }
    });
})();
