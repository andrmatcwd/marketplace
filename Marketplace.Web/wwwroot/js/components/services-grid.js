window.Marketplace = window.Marketplace || {};

window.Marketplace.ServicesGrid = (function ($) {
    function init() {
        const $filtersForm = $('#servicesFiltersForm');
        const $gridContainer = $('#servicesGridContainer');

        if (!$filtersForm.length || !$gridContainer.length) {
            return;
        }

        function loadGrid() {
            const query = $filtersForm.serialize();

            $.get('/Services/List?' + query)
                .done(function (html) {
                    $gridContainer.html(html);
                });
        }

        $(document).on('change', '#servicesSortBy', function () {
            $filtersForm.find('input[name="SortBy"]').val($(this).val());
            $filtersForm.find('input[name="Page"]').val(1);
            loadGrid();
        });

        $(document).on('click', '#applyFiltersAjax', function () {
            $filtersForm.find('input[name="Page"]').val(1);
            loadGrid();
        });

        $(document).on('click', '.services-page-link', function (e) {
            e.preventDefault();

            const page = $(this).data('page');
            $filtersForm.find('input[name="Page"]').val(page);
            loadGrid();
        });
    }

    return {
        init: init
    };
})(jQuery);