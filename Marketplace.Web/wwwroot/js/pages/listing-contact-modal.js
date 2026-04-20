(function () {
    function initListingContactModal() {
        const form = document.getElementById("contactListingForm");
        const feedback = document.getElementById("contactListingFeedback");
        const submitBtn = document.getElementById("contactListingSubmitBtn");

        if (!form || !feedback || !submitBtn) {
            return;
        }

        form.addEventListener("submit", async function (e) {
            e.preventDefault();

            const culture = form.dataset.culture || "uk";
            const listingId = form.dataset.listingId;

            const payload = {
                listingId: listingId,
                name: document.getElementById("contactName")?.value?.trim() || "",
                phone: document.getElementById("contactPhone")?.value?.trim() || "",
                message: document.getElementById("contactMessage")?.value?.trim() || ""
            };

            feedback.hidden = true;
            feedback.className = "contact-form-feedback";
            feedback.textContent = "";

            submitBtn.disabled = true;
            submitBtn.textContent = culture === "uk" ? "Надсилання..." : "Sending...";

            try {
                const response = await fetch(`/${culture}/api/contact/send`, {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                        "Accept": "application/json"
                    },
                    body: JSON.stringify(payload)
                });

                const result = await response.json();

                feedback.hidden = false;

                if (response.ok && result.success) {
                    feedback.classList.add("is-success");
                    feedback.textContent = result.message || (culture === "uk"
                        ? "Повідомлення успішно надіслано."
                        : "Message sent successfully.");

                    form.reset();

                    if (window.gtag) {
                        gtag("event", "contact_form_submit", {
                            listing_id: listingId
                        });
                    }
                } else {
                    feedback.classList.add("is-error");
                    feedback.textContent = result.message || (culture === "uk"
                        ? "Не вдалося надіслати повідомлення."
                        : "Could not send message.");
                }
            } catch {
                feedback.hidden = false;
                feedback.classList.add("is-error");
                feedback.textContent = culture === "uk"
                    ? "Сталася помилка під час надсилання."
                    : "An error occurred while sending.";
            } finally {
                submitBtn.disabled = false;
                submitBtn.textContent = culture === "uk" ? "Надіслати" : "Send";
            }
        });
    }

    document.addEventListener("DOMContentLoaded", initListingContactModal);
})();