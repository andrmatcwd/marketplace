(function () {
    function initReviewsToggle() {
        const wrappers = document.querySelectorAll("[data-reviews-wrapper]");

        if (!wrappers.length) {
            return;
        }

        wrappers.forEach(function (wrapper) {
            const list = wrapper.querySelector("[data-reviews-list]");
            const button = wrapper.querySelector("[data-reviews-toggle]");

            if (!list || !button) {
                return;
            }

            let expanded = false;

            button.addEventListener("click", function () {
                expanded = !expanded;

                if (expanded) {
                    list.classList.remove("listing-reviews-list-collapsed");
                    list.classList.add("listing-reviews-list-expanded");
                    button.textContent = "Згорнути";
                } else {
                    list.classList.add("listing-reviews-list-collapsed");
                    list.classList.remove("listing-reviews-list-expanded");
                    button.textContent = "Показати всі";

                    wrapper.scrollIntoView({
                        behavior: "smooth",
                        block: "start"
                    });
                }
            });
        });
    }

    document.addEventListener("DOMContentLoaded", initReviewsToggle);
})();