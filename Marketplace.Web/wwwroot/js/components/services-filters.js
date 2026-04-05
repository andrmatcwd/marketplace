window.Marketplace = window.Marketplace || {};

window.Marketplace.ServicesFilters = (function ($) {
    function initSidebarToggle() {
        const $sidebar = $('#desktopFiltersSidebar');
        const $toggle = $('#desktopFiltersToggle');
        const $toggleText = $toggle.find('.services-toggle-text');

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