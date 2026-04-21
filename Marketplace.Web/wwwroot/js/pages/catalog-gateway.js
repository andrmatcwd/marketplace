document.addEventListener('DOMContentLoaded', () => {
    const continueBtn = document.querySelector('[data-continue]');
    const citySelect = document.querySelector('[data-city-select]');

    if (!continueBtn || !citySelect) return;

    continueBtn.addEventListener('click', () => {
        const city = citySelect.value;

        if (!city) {
            alert('Please select a city first');
            return;
        }

        window.locationContext.setPreferredCity(city);

        const culture = window.locationContext.getCulture();
        window.location.href = `/${culture}/${city}`;
    });
});