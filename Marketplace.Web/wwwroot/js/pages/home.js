document.addEventListener('DOMContentLoaded', () => {
    const detectBtn = document.querySelector('[data-detect-city]');

    if (!detectBtn) return;

    detectBtn.addEventListener('click', async () => {
        detectBtn.disabled = true;

        const city = await window.locationContext.detectCity();

        if (!city) {
            alert('Unable to detect city');
            detectBtn.disabled = false;
            return;
        }

        window.locationContext.setPreferredCity(city);
        window.location.reload();
    });
});