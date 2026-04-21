document.addEventListener('DOMContentLoaded', () => {
    document.querySelectorAll('[data-collapsible]').forEach(container => {
        const list = container.querySelector('[data-list]');
        const btn = container.querySelector('[data-toggle]');

        if (!list || !btn) return;

        let expanded = false;

        btn.addEventListener('click', () => {
            expanded = !expanded;

            list.classList.toggle('is-expanded', expanded);

            const expandLabel = btn.dataset.expand || 'Show all';
            const collapseLabel = btn.dataset.collapse || 'Collapse';

            btn.textContent = expanded ? collapseLabel : expandLabel;

            if (!expanded) {
                container.scrollIntoView({ behavior: 'smooth', block: 'start' });
            }
        });
    });
});