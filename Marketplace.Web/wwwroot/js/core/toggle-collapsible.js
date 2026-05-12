(function () {
    'use strict';

    function bindToggle(config) {
        var wrappers = document.querySelectorAll(config.wrapperSelector);
        if (!wrappers.length) return;

        wrappers.forEach(function (wrapper) {
            var list = wrapper.querySelector(config.listSelector);
            var button = wrapper.querySelector(config.buttonSelector);

            if (!list || !button) return;

            var expanded = false;
            button.setAttribute('aria-expanded', 'false');

            button.addEventListener('click', function () {
                expanded = !expanded;
                button.setAttribute('aria-expanded', String(expanded));

                if (expanded) {
                    list.classList.remove(config.collapsedClass);
                    list.classList.add(config.expandedClass);
                    button.textContent = config.collapseText;
                } else {
                    list.classList.add(config.collapsedClass);
                    list.classList.remove(config.expandedClass);
                    button.textContent = config.expandText;
                    wrapper.scrollIntoView({ behavior: 'smooth', block: 'start' });
                }
            });
        });
    }

    function initCollapsibles() {
        bindToggle({
            wrapperSelector: '[data-taxonomy-wrapper]',
            listSelector: '[data-taxonomy-grid]',
            buttonSelector: '[data-taxonomy-toggle]',
            collapsedClass: 'taxonomy-grid-collapsed',
            expandedClass: 'taxonomy-grid-expanded',
            expandText: 'Показати всі',
            collapseText: 'Згорнути'
        });

        bindToggle({
            wrapperSelector: '[data-features-wrapper]',
            listSelector: '[data-features-list]',
            buttonSelector: '[data-features-toggle]',
            collapsedClass: 'listing-features-collapsed',
            expandedClass: 'listing-features-expanded',
            expandText: 'Показати всі',
            collapseText: 'Згорнути'
        });

        bindToggle({
            wrapperSelector: '[data-reviews-wrapper]',
            listSelector: '[data-reviews-list]',
            buttonSelector: '[data-reviews-toggle]',
            collapsedClass: 'listing-reviews-list-collapsed',
            expandedClass: 'listing-reviews-list-expanded',
            expandText: 'Показати всі',
            collapseText: 'Згорнути'
        });
    }

    document.addEventListener('DOMContentLoaded', initCollapsibles);
})();

document.addEventListener('DOMContentLoaded', function () {
    document.querySelectorAll('[data-collapsible]').forEach(function (wrapper) {
        var content = wrapper.querySelector('[data-collapsible-content]');
        var button = wrapper.querySelector('[data-collapsible-toggle]');

        if (!content || !button) return;

        button.setAttribute('aria-expanded', 'false');

        button.addEventListener('click', function () {
            var isOpen = wrapper.classList.toggle('is-open');
            button.setAttribute('aria-expanded', String(isOpen));
            button.textContent = isOpen
                ? button.dataset.collapseText || 'Згорнути'
                : button.dataset.expandText || 'Показати більше';
        });
    });
});
