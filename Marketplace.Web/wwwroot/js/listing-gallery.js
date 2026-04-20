(function ($) {
    $(function () {
        var $main = $(".listing-gallery-main-image");
        var $thumbs = $(".listing-gallery-thumb");

        if (!$main.length || !$thumbs.length) {
            return;
        }

        $thumbs.on("click", function () {
            var src = $(this).attr("src");
            var alt = $(this).attr("alt") || "";

            $main.attr("src", src);
            $main.attr("alt", alt);
        });
    });
})(jQuery);