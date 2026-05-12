(function () {
    'use strict';

    document.addEventListener('DOMContentLoaded', function () {
        if (typeof $ === 'undefined' || !$.fn || !$.fn.select2) return;

        $('.admin-select').each(function () {
            var $el = $(this);
            var $empty = $el.find('option[value=""]');
            var placeholder = $empty.length ? $empty.first().text() : '';

            $el.select2({
                placeholder: placeholder,
                allowClear: !!placeholder,
                width: '100%',
                minimumResultsForSearch: 2
            });
        });
    });
})();
