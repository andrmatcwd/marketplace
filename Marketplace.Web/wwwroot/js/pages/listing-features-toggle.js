(function () {
    function initFeaturesToggle() {
        const wrappers = document.querySelectorAll("[data-features-wrapper]");

        if (!wrappers.length) {
            return;
        }

        wrappers.forEach(function (wrapper) {
            const list = wrapper.querySelector("[data-features-list]");
            const button = wrapper.querySelector("[data-features-toggle]");

            if (!list || !button) {
                return;
            }

            let expanded = false;

            button.addEventListener("click", function () {
                expanded = !expanded;

                if (expanded) {
                    list.classList.remove("listing-features-collapsed");
                    list.classList.add("listing-features-expanded");
                    button.textContent = "Згорнути";
                } else {
                    list.classList.add("listing-features-collapsed");
                    list.classList.remove("listing-features-expanded");
                    button.textContent = "Показати всі";

                    wrapper.scrollIntoView({
                        behavior: "smooth",
                        block: "start"
                    });
                }
            });
        });
    }

    document.addEventListener("DOMContentLoaded", initFeaturesToggle);
})();