(function ($) {
    $(function () {
        $(document).on("click", "[data-mobile-filter-open]", function () {
            $("[data-mobile-filter-drawer]").addClass("is-open");
        });

        $(document).on("click", "[data-mobile-filter-close]", function () {
            $("[data-mobile-filter-drawer]").removeClass("is-open");
        });

        $(document).on("click", "[data-mobile-filter-drawer]", function (e) {
            if ($(e.target).is("[data-mobile-filter-drawer]")) {
                $(this).removeClass("is-open");
            }
        });
    });
})(jQuery);