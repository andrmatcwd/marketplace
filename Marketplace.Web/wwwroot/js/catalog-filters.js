(function ($) {
    $(function () {
        var $forms = $(".filter-bar");

        if (!$forms.length) {
            return;
        }

        $forms.each(function () {
            var $form = $(this);

            $form.on("change", "select", function () {
                var $pageInput = $form.find('input[name="page"]');

                if ($pageInput.length) {
                    $pageInput.val("1");
                }
            });

            $form.on("submit", function () {
                var $pageInput = $form.find('input[name="page"]');

                if (!$pageInput.length) {
                    $('<input>', {
                        type: 'hidden',
                        name: 'page',
                        value: '1'
                    }).appendTo($form);
                }
            });
        });
    });
})(jQuery);