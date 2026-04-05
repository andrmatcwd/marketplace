$(function () {
    const $sidebar = $('#desktopFiltersSidebar');
    const $toggle = $('#desktopFiltersToggle');
    const $toggleText = $toggle.find('.services-toggle-text');

    if ($sidebar.length === 0) return;

    $toggle.on('click', function () {
        $sidebar.toggleClass('collapsed');

        if ($sidebar.hasClass('collapsed')) {
            $toggleText.text('Show filters');
        } else {
            $toggleText.text('Hide filters');
        }
    });
});