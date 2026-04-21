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

        function getCurrentCity() {
            var pathSegments = window.location.pathname.split('/').filter(Boolean);
            return pathSegments.length >= 2 ? pathSegments[1] : '';
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
                var city = getCurrentCity();

                $.get('/' + culture + '/api/listings/autocomplete', {
                    search: query,
                    city: city
                })
                    .done(function (items) {
                        if (!items || !items.length) {
                            $box.empty().removeClass("is-visible");
                            return;
                        }

                        var html = items.map(function (item) {
                            var badge = item.category
                                ? '<span class="search-autocomplete-city">' + item.category + '</span>'
                                : '';

                            return '<a class="search-autocomplete-item" href="' + item.url + '">' +
                                '<span class="search-autocomplete-title">' + item.label + '</span>' +
                                badge +
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