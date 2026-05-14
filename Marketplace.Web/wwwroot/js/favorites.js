(function () {
    'use strict';

    var COOKIE_NAME = 'mp_favorites';
    var COOKIE_MAX_AGE = 60 * 60 * 24 * 365; // 1 year

    // ── Cookie helpers ─────────────────────────────────────────

    function getIds() {
        var match = document.cookie.match(/(?:^|;\s*)mp_favorites=([^;]*)/);
        if (!match || !match[1]) return [];
        return match[1].split(',').map(Number).filter(function (n) { return n > 0; });
    }

    function saveIds(ids) {
        var val = ids.length ? ids.join(',') : '';
        document.cookie = COOKIE_NAME + '=' + val
            + '; path=/; max-age=' + COOKIE_MAX_AGE + '; SameSite=Lax';
    }

    function isSaved(id) {
        return getIds().indexOf(id) >= 0;
    }

    function toggle(id) {
        var ids = getIds();
        var idx = ids.indexOf(id);
        if (idx >= 0) { ids.splice(idx, 1); } else { ids.push(id); }
        saveIds(ids);
        var added = idx < 0;
        dispatchChange(ids.length);
        return added;
    }

    function dispatchChange(count) {
        window.dispatchEvent(new CustomEvent('favoritesChanged', { detail: { count: count } }));
    }

    // ── UI helpers ─────────────────────────────────────────────

    function updateBadges() {
        var count = getIds().length;
        document.querySelectorAll('[data-fav-count]').forEach(function (el) {
            el.textContent = count > 0 ? count : '';
            el.hidden = count === 0;
        });
    }

    function updateButtons() {
        document.querySelectorAll('[data-fav-btn]').forEach(function (btn) {
            var id = parseInt(btn.dataset.favBtn, 10);
            var saved = isSaved(id);
            btn.classList.toggle('is-saved', saved);
            btn.setAttribute('aria-pressed', saved ? 'true' : 'false');
            btn.title = saved ? (btn.dataset.labelRemove || '') : (btn.dataset.labelAdd || '');
        });
    }

    function isUkCulture() {
        var el = document.querySelector('[data-culture]');
        if (el) return el.dataset.culture === 'uk';
        return document.documentElement.lang === 'uk';
    }

    function showToast(added) {
        var prev = document.getElementById('fav-toast');
        if (prev) prev.remove();

        var uk = isUkCulture();
        var msg = added
            ? (uk ? 'Додано до збережених' : 'Добавлено в сохранённые')
            : (uk ? 'Видалено зі збережених' : 'Удалено из сохранённых');

        var t = document.createElement('div');
        t.id = 'fav-toast';
        t.className = 'fav-toast' + (added ? ' fav-toast--added' : '');
        t.textContent = msg;
        t.setAttribute('role', 'status');
        t.setAttribute('aria-live', 'polite');
        document.body.appendChild(t);

        requestAnimationFrame(function () {
            requestAnimationFrame(function () { t.classList.add('is-visible'); });
        });
        setTimeout(function () {
            t.classList.remove('is-visible');
            setTimeout(function () { if (t.parentNode) t.remove(); }, 300);
        }, 2200);
    }

    // ── Favorites page: remove card from DOM on un-save ────────

    function removeCardFromFavPage(btn) {
        var grid = document.getElementById('favs-grid');
        if (!grid) return;

        var card = btn.closest('[data-listing-id]');
        if (!card) return;

        card.style.transition = 'opacity .22s, transform .22s';
        card.style.opacity = '0';
        card.style.transform = 'scale(.97)';

        setTimeout(function () {
            card.remove();
            var remaining = grid.querySelectorAll('[data-listing-id]');
            var countEl = document.getElementById('favs-count');
            if (!remaining.length) {
                grid.style.display = 'none';
                if (countEl) countEl.style.display = 'none';
                var empty = document.getElementById('favs-empty');
                if (empty) empty.style.display = '';
            } else if (countEl) {
                countEl.innerHTML = '<strong>' + remaining.length + '</strong> '
                    + (isUkCulture() ? 'публікацій' : 'объявлений');
            }
        }, 240);
    }

    // ── Init ───────────────────────────────────────────────────

    function init() {
        updateButtons();
        updateBadges();

        document.addEventListener('click', function (e) {
            var btn = e.target.closest('[data-fav-btn]');
            if (!btn) return;
            e.preventDefault();
            e.stopPropagation();

            var id = parseInt(btn.dataset.favBtn, 10);
            if (!id) return;

            var added = toggle(id);
            btn.classList.toggle('is-saved', added);
            btn.setAttribute('aria-pressed', added ? 'true' : 'false');
            btn.title = added ? (btn.dataset.labelRemove || '') : (btn.dataset.labelAdd || '');
            showToast(added);

            if (!added) removeCardFromFavPage(btn);
        });

        window.addEventListener('favoritesChanged', updateBadges);
    }

    document.addEventListener('DOMContentLoaded', init);

    window.Favorites = { getIds: getIds, isSaved: isSaved };

}());
