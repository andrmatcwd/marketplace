(function () {
    'use strict';

    function initListingContactModal() {
        var form = document.getElementById('contactListingForm');
        var feedback = document.getElementById('contactListingFeedback');
        var submitBtn = document.getElementById('contactListingSubmitBtn');
        var spinner = document.getElementById('contactListingSpinner');
        var submitText = document.getElementById('contactListingSubmitText');

        if (!form || !feedback || !submitBtn) return;

        form.addEventListener('submit', async function (e) {
            e.preventDefault();

            var culture = form.dataset.culture || 'uk';
            var listingId = form.dataset.listingId;

            var payload = {
                listingId: listingId,
                name: (document.getElementById('contactName')?.value || '').trim(),
                phone: (document.getElementById('contactPhone')?.value || '').trim(),
                message: (document.getElementById('contactMessage')?.value || '').trim()
            };

            feedback.hidden = true;
            feedback.className = 'contact-form-feedback';
            feedback.textContent = '';

            submitBtn.disabled = true;
            submitBtn.setAttribute('aria-busy', 'true');
            if (spinner) spinner.classList.remove('d-none');
            if (submitText) submitText.textContent = culture === 'uk' ? 'Надсилання...' : 'Sending...';

            try {
                var response = await fetch('/' + culture + '/api/contact/send', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'Accept': 'application/json'
                    },
                    body: JSON.stringify(payload)
                });

                var result = await response.json();

                feedback.hidden = false;

                if (response.ok && result.success) {
                    feedback.classList.add('is-success');
                    feedback.textContent = result.message || (culture === 'uk'
                        ? 'Повідомлення успішно надіслано.'
                        : 'Message sent successfully.');

                    form.reset();

                    if (window.gtag) {
                        gtag('event', 'contact_form_submit', { listing_id: listingId });
                    }
                } else {
                    feedback.classList.add('is-error');
                    feedback.textContent = result.message || (culture === 'uk'
                        ? 'Не вдалося надіслати повідомлення.'
                        : 'Could not send message.');
                }
            } catch {
                feedback.hidden = false;
                feedback.classList.add('is-error');
                feedback.textContent = culture === 'uk'
                    ? 'Сталася помилка під час надсилання.'
                    : 'An error occurred while sending.';
            } finally {
                submitBtn.disabled = false;
                submitBtn.removeAttribute('aria-busy');
                if (spinner) spinner.classList.add('d-none');
                if (submitText) submitText.textContent = culture === 'uk' ? 'Надіслати' : 'Send';
            }
        });
    }

    document.addEventListener('DOMContentLoaded', initListingContactModal);
})();
