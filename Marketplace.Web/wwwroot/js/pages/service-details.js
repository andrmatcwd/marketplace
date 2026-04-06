window.initServiceMap = async function () {
    const mapElement = document.getElementById('serviceMap');

    if (!mapElement) {
        return;
    }

    const lat = Number(mapElement.dataset.lat);
    const lng = Number(mapElement.dataset.lng);
    const title = mapElement.dataset.title || 'Service location';

    if (Number.isNaN(lat) || Number.isNaN(lng)) {
        return;
    }

    const position = { lat, lng };

    const { Map } = await google.maps.importLibrary("maps");
    const { AdvancedMarkerElement } = await google.maps.importLibrary("marker");

    const map = new Map(mapElement, {
        center: position,
        zoom: 14,
        mapId: "DEMO_MAP_ID"
    });

    new AdvancedMarkerElement({
        map,
        position,
        title
    });
};

window.setMainImage = function (img) {
    const main = document.getElementById('mainServiceImage');
    if (!main) return;

    main.src = img.src;

    document.querySelectorAll('.gallery-thumb')
        .forEach(x => x.classList.remove('active'));

    img.classList.add('active');
};