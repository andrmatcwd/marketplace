(function ($) {
    $(function () {
        var $input = $("#search");
        if (!$input.length) {
            return;
        }

        var $box = $('<div class="search-autocomplete"></div>');
        $input.after($box);

        var timer = null;

        function getCulture() {
            var pathSegments = window.location.pathname.split('/').filter(Boolean);
            return pathSegments.length ? pathSegments[0] : 'uk';
        }

        $input.on("input", function () {
            var query = $(this).val();

            clearTimeout(timer);

            if (!query || query.length < 2) {
                $box.empty().removeClass("is-visible");
                return;
            }

            timer = setTimeout(function () {
                var culture = getCulture();

                $.get('/' + culture + '/api/listings/autocomplete', { search: query })
                    .done(function (items) {
                        if (!items || !items.length) {
                            $box.empty().removeClass("is-visible");
                            return;
                        }

                        var html = items.map(function (item) {
                            var city = item.city
                                ? '<span class="search-autocomplete-city">' + item.city + '</span>'
                                : '';

                            return '<a class="search-autocomplete-item" href="' + item.url + '">' +
                                '<span class="search-autocomplete-title">' + item.label + '</span>' +
                                city +
                                '</a>';
                        }).join("");

                        $box.html(html).addClass("is-visible");
                    })
                    .fail(function () {
                        $box.empty().removeClass("is-visible");
                    });
            }, 180);
        });

        $(document).on("click", function (e) {
            if (!$(e.target).closest(".filter-bar-group").length) {
                $box.removeClass("is-visible");
            }
        });
    });
})(jQuery);