(function () {
    function initCatalogMap() {
        var mapEl = document.getElementById("catalogMap");
        if (!mapEl || typeof L === "undefined") return;

        var raw = mapEl.dataset.markers;
        if (!raw) return;

        var markers;
        try { markers = JSON.parse(raw); } catch { return; }

        var valid = markers.filter(function (m) {
            return typeof m.lat === "number" && typeof m.lng === "number" &&
                   !isNaN(m.lat) && !isNaN(m.lng);
        });

        if (valid.length === 0) return;

        var map = L.map(mapEl, { scrollWheelZoom: false });

        L.tileLayer("https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png", {
            maxZoom: 19,
            attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
        }).addTo(map);

        var group = L.featureGroup();

        valid.forEach(function (m) {
            var popup = "<strong>" + escapeHtml(m.title) + "</strong>";
            if (m.url) {
                popup += '<br><a href="' + escapeHtml(m.url) + '">View listing</a>';
            }
            L.marker([m.lat, m.lng]).bindPopup(popup).addTo(group);
        });

        group.addTo(map);

        if (valid.length === 1) {
            map.setView([valid[0].lat, valid[0].lng], 14);
        } else {
            map.fitBounds(group.getBounds(), { padding: [32, 32] });
        }

        setTimeout(function () { map.invalidateSize(); }, 100);
    }

    function escapeHtml(value) {
        return String(value)
            .replaceAll("&", "&amp;")
            .replaceAll("<", "&lt;")
            .replaceAll(">", "&gt;")
            .replaceAll('"', "&quot;")
            .replaceAll("'", "&#039;");
    }

    document.addEventListener("DOMContentLoaded", initCatalogMap);
})();
