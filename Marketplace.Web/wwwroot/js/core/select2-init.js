(function () {
    'use strict';

    document.addEventListener('DOMContentLoaded', function () {
        if (typeof $ === 'undefined' || !$.fn || !$.fn.select2) return;

        $('.app-select').each(function () {
            var $el = $(this);
            var $empty = $el.find('option[value=""]');
            var placeholder = $empty.length ? $empty.first().text() : '';
            var minSearch = $el.data('minSearch') !== undefined ? parseInt($el.data('minSearch'), 10) : 7;

            $el.select2({
                placeholder: placeholder,
                allowClear: !!placeholder,
                width: '100%',
                minimumResultsForSearch: minSearch
            });
        });
    });
})();
