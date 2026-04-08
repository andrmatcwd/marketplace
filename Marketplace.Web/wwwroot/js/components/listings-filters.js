window.Marketplace = window.Marketplace || {};

window.Marketplace.ListingsFilters = (function ($) {
    function initSidebarToggle() {
        const $sidebar = $('#desktopFiltersSidebar');
        const $toggle = $('#desktopFiltersToggle');
        const $toggleText = $toggle.find('.listings-toggle-text');

        if (!$sidebar.length || !$toggle.length) {
            return;
        }

        $toggle.on('click', function () {
            $sidebar.toggleClass('collapsed');
            $toggleText.text($sidebar.hasClass('collapsed') ? 'Show filters' : 'Hide filters');
        });
    }

    return {
        initSidebarToggle: initSidebarToggle
    };
})(jQuery);