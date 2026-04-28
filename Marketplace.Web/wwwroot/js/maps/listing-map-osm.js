(function () {
    function initListingMap() {
        const mapElement = document.getElementById("listingMap");

        if (!mapElement || typeof L === "undefined") {
            return;
        }

        const lat = Number(mapElement.dataset.lat);
        const lng = Number(mapElement.dataset.lng);
        const title = mapElement.dataset.title || "Location";
        const address = mapElement.dataset.address || "";

        if (Number.isNaN(lat) || Number.isNaN(lng)) {
            return;
        }

        const map = L.map(mapElement, {
            scrollWheelZoom: false
        }).setView([lat, lng], 14);

        L.tileLayer("https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png", {
            maxZoom: 19,
            attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
        }).addTo(map);

        const marker = L.marker([lat, lng]).addTo(map);

        if (title || address) {
            marker.bindPopup(
                `<strong>${escapeHtml(title)}</strong>${address ? `<br>${escapeHtml(address)}` : ""}`
            );
        }

        setTimeout(function () {
            map.invalidateSize();
        }, 100);
    }

    function escapeHtml(value) {
        return String(value)
            .replaceAll("&", "&amp;")
            .replaceAll("<", "&lt;")
            .replaceAll(">", "&gt;")
            .replaceAll('"', "&quot;")
            .replaceAll("'", "&#039;");
    }

    document.addEventListener("DOMContentLoaded", initListingMap);
})();