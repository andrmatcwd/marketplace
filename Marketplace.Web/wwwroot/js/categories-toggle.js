(function () {
    function initTaxonomyToggles() {
        const wrappers = document.querySelectorAll("[data-taxonomy-wrapper]");

        if (!wrappers.length) {
            return;
        }

        wrappers.forEach(function (wrapper) {
            const grid = wrapper.querySelector("[data-taxonomy-grid]");
            const button = wrapper.querySelector("[data-taxonomy-toggle]");

            if (!grid || !button) {
                return;
            }

            let expanded = false;

            button.addEventListener("click", function () {
                expanded = !expanded;

                if (expanded) {
                    grid.classList.remove("taxonomy-grid-collapsed");
                    grid.classList.add("taxonomy-grid-expanded");
                    button.textContent = "Згорнути";
                } else {
                    grid.classList.add("taxonomy-grid-collapsed");
                    grid.classList.remove("taxonomy-grid-expanded");
                    button.textContent = "Показати всі";

                    wrapper.scrollIntoView({
                        behavior: "smooth",
                        block: "start"
                    });
                }
            });
        });
    }

    document.addEventListener("DOMContentLoaded", initTaxonomyToggles);
})();