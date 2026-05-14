(function () {
    'use strict';

    function initFaq() {
        var items = document.querySelectorAll('.fb-faq-item');
        items.forEach(function (item) {
            var btn = item.querySelector('.fb-faq-question');
            if (!btn) return;
            btn.addEventListener('click', function () {
                var isOpen = item.classList.contains('open');
                items.forEach(function (i) { i.classList.remove('open'); });
                if (!isOpen) item.classList.add('open');
            });
        });
    }

    function initInquiryForm() {
        var form = document.getElementById('businessInquiryForm');
        var feedback = document.getElementById('fbFormFeedback');
        var submitBtn = document.getElementById('fbSubmitBtn');
        var spinner = document.getElementById('fbSubmitSpinner');
        var submitText = document.getElementById('fbSubmitText');

        if (!form || !feedback || !submitBtn) return;

        form.addEventListener('submit', async function (e) {
            e.preventDefault();

            var culture = form.dataset.culture || 'uk';

            var payload = {
                name: (document.getElementById('fbName')?.value || '').trim(),
                phone: (document.getElementById('fbPhone')?.value || '').trim(),
                email: (document.getElementById('fbEmail')?.value || '').trim() || null,
                companyName: (document.getElementById('fbCompany')?.value || '').trim() || null,
                message: (document.getElementById('fbMessage')?.value || '').trim()
            };

            feedback.className = 'fb-form-feedback';
            feedback.textContent = '';

            submitBtn.disabled = true;
            submitBtn.setAttribute('aria-busy', 'true');
            if (spinner) spinner.classList.remove('d-none');
            if (submitText) {
                submitText.textContent = culture === 'uk' ? 'Надсилання...' : 'Отправка...';
            }

            try {
                var response = await fetch('/' + culture + '/api/business-inquiry/send', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'Accept': 'application/json'
                    },
                    body: JSON.stringify(payload)
                });

                var result = await response.json();

                if (response.ok && result.success) {
                    feedback.className = 'fb-form-feedback success';
                    feedback.textContent = result.message || (culture === 'uk'
                        ? 'Вашу заявку отримано. Ми зв\'яжемося з вами найближчим часом.'
                        : 'Ваша заявка получена. Мы свяжемся с вами в ближайшее время.');
                    form.reset();
                } else {
                    feedback.className = 'fb-form-feedback error';
                    feedback.textContent = result.message || (culture === 'uk'
                        ? 'Не вдалося надіслати заявку. Спробуйте ще раз.'
                        : 'Не удалось отправить заявку. Попробуйте ещё раз.');
                }
            } catch {
                feedback.className = 'fb-form-feedback error';
                feedback.textContent = culture === 'uk'
                    ? 'Сталася помилка під час надсилання. Перевірте з\'єднання і спробуйте знову.'
                    : 'Произошла ошибка при отправке. Проверьте соединение и попробуйте снова.';
            } finally {
                submitBtn.disabled = false;
                submitBtn.removeAttribute('aria-busy');
                if (spinner) spinner.classList.add('d-none');
                if (submitText) {
                    submitText.textContent = culture === 'uk' ? 'Надіслати заявку' : 'Отправить заявку';
                }
            }
        });
    }

    document.addEventListener('DOMContentLoaded', function () {
        initFaq();
        initInquiryForm();
    });
})();
