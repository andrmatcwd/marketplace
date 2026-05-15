(function ($) {
    var $form   = $('#review-form');
    if (!$form.length) return;

    var $alert  = $('#review-alert');
    var $submit = $('#review-submit');
    var $picker = $form.find('[data-star-picker]');
    var $hidden = $form.find('#rating-hidden');
    var selected = 0;

    var msgSuccess = $form.data('msg-success');
    var msgError   = $form.data('msg-error');

    // ── Star picker ──────────────────────────────────────────────
    function paintStars(upTo) {
        $picker.find('.star-pick-btn').each(function () {
            $(this).toggleClass('active', +$(this).data('value') <= upTo);
        });
    }

    $picker.find('.star-pick-btn')
        .on('mouseenter', function () { paintStars(+$(this).data('value')); })
        .on('mouseleave', function () { paintStars(selected); })
        .on('click',      function () {
            selected = +$(this).data('value');
            $hidden.val(selected);
            paintStars(selected);
        });

    // ── Alert helper ─────────────────────────────────────────────
    function showAlert(msg, isSuccess) {
        $alert
            .text(msg)
            .removeClass('review-form-alert-success review-form-alert-error')
            .addClass(isSuccess ? 'review-form-alert-success' : 'review-form-alert-error')
            .show();
    }

    // ── Async submit ─────────────────────────────────────────────
    $form.on('submit', function (e) {
        e.preventDefault();
        $alert.hide();
        $submit.prop('disabled', true);

        $.ajax({
            url:    $form.attr('action'),
            method: 'POST',
            data:   $form.serialize(),
            success: function () {
                showAlert(msgSuccess, true);
                $form[0].reset();
                selected = 0;
                $hidden.val('');
                paintStars(0);
            },
            error: function (xhr) {
                var res = xhr.responseJSON;
                var msg = (res && res.errors && res.errors.length)
                    ? res.errors.join(' ')
                    : msgError;
                showAlert(msg, false);
            },
            complete: function () {
                $submit.prop('disabled', false);
            }
        });
    });
}(jQuery));
