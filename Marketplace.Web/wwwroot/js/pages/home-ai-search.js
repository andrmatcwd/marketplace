(function () {
    const tabs = document.querySelectorAll('.home-search-tab');
    const classicPanel = document.getElementById('homeTabClassic');
    const aiPanel = document.getElementById('homeTabAi');

    tabs.forEach(tab => {
        tab.addEventListener('click', () => {
            tabs.forEach(t => t.classList.remove('is-active'));
            tab.classList.add('is-active');

            const target = tab.dataset.tab;
            classicPanel.hidden = target !== 'classic';
            aiPanel.hidden = target !== 'ai';
        });
    });

    const btn = document.getElementById('aiSearchBtn');
    const input = document.getElementById('aiSearchInput');
    const errorBox = document.getElementById('aiSearchError');

    if (!btn || !input) return;

    btn.addEventListener('click', async () => {
        const description = input.value.trim();
        if (!description) {
            showError('Please describe what you are looking for.');
            return;
        }

        hideError();
        btn.disabled = true;
        btn.textContent = 'Searching…';

        try {
            const culture = window.__culture || 'uk';
            const response = await fetch(`/${culture}/api/ai-search`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ description })
            });

            const data = await response.json();

            if (!response.ok) {
                showError(data.error || 'Something went wrong. Please try again.');
                return;
            }

            window.location.href = data.redirectUrl;
        } catch {
            showError('Network error. Please check your connection and try again.');
        } finally {
            btn.disabled = false;
            btn.textContent = 'Find with AI';
        }
    });

    input.addEventListener('keydown', e => {
        if (e.key === 'Enter' && !e.shiftKey) {
            e.preventDefault();
            btn.click();
        }
    });

    function showError(msg) {
        errorBox.textContent = msg;
        errorBox.hidden = false;
    }

    function hideError() {
        errorBox.hidden = true;
        errorBox.textContent = '';
    }
})();
