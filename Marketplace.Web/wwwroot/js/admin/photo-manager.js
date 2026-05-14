(function () {
    var uploadZone = document.getElementById("uploadZone");
    var photoGrid  = document.getElementById("photoGrid");
    var fileInput  = document.getElementById("photoFileInput");
    var statusEl   = document.getElementById("uploadStatus");

    if (!uploadZone || !photoGrid) return;

    var mode       = uploadZone.dataset.mode || "listing";
    var uploadUrl  = uploadZone.dataset.uploadUrl;
    var listingId  = uploadZone.dataset.listingId;
    var roomId     = uploadZone.dataset.roomId;
    var deleteUrl  = photoGrid.dataset.deleteUrl;
    var primaryUrl = photoGrid.dataset.primaryUrl;

    // ── Upload helpers ────────────────────────────────────────

    function showStatus(msg, isError) {
        statusEl.textContent = msg;
        statusEl.className = "photo-upload-status" + (isError ? " photo-upload-status--error" : " photo-upload-status--ok");
        statusEl.hidden = false;
        setTimeout(function () { statusEl.hidden = true; }, 4000);
    }

    function buildCard(id, url, isPrimary) {
        var card = document.createElement("div");
        card.className = "photo-card";
        if (id) card.dataset.id = id;
        card.dataset.url = url;

        var badge = isPrimary ? '<span class="photo-card__badge">Основне</span>' : "";

        card.innerHTML =
            '<div class="photo-card__img-wrap">' +
                '<img src="' + url + '" alt="" loading="lazy" />' +
                badge +
            '</div>' +
            '<div class="photo-card__actions">' +
                (mode === "listing" && !isPrimary
                    ? '<button type="button" class="admin-btn admin-btn--xs admin-btn--secondary js-set-primary">★ Основне</button>'
                    : "") +
                '<button type="button" class="admin-btn admin-btn--xs admin-btn--danger js-delete-photo">Видалити</button>' +
            '</div>';

        return card;
    }

    async function uploadFile(file) {
        var form = new FormData();
        form.append("file", file);
        if (mode === "listing") form.append("listingId", listingId);
        if (mode === "room")    form.append("roomId", roomId);

        var res = await fetch(uploadUrl + "?" + (mode === "listing" ? "listingId=" + listingId : "roomId=" + roomId), {
            method: "POST",
            body: form,
            headers: { "RequestVerificationToken": getAntiForgeryToken() }
        });

        if (!res.ok) {
            var err = await res.json().catch(function () { return { error: "Upload failed." }; });
            throw new Error(err.error || "Upload failed.");
        }

        return res.json();
    }

    async function processFiles(files) {
        for (var i = 0; i < files.length; i++) {
            try {
                var data = await uploadFile(files[i]);
                var isFirst = photoGrid.querySelectorAll(".photo-card").length === 0;
                var card = buildCard(data.id, data.url, isFirst);
                photoGrid.appendChild(card);
                showStatus("Фото завантажено успішно.", false);
            } catch (e) {
                showStatus(e.message, true);
            }
        }
    }

    // ── Drag-and-drop on upload zone ─────────────────────────

    uploadZone.addEventListener("dragover", function (e) {
        e.preventDefault();
        uploadZone.classList.add("photo-upload-zone--active");
    });

    uploadZone.addEventListener("dragleave", function () {
        uploadZone.classList.remove("photo-upload-zone--active");
    });

    uploadZone.addEventListener("drop", function (e) {
        e.preventDefault();
        uploadZone.classList.remove("photo-upload-zone--active");
        processFiles(e.dataTransfer.files);
    });

    fileInput.addEventListener("change", function () {
        processFiles(fileInput.files);
        fileInput.value = "";
    });

    // ── Delete / Set primary via event delegation ────────────

    photoGrid.addEventListener("click", async function (e) {
        var delBtn = e.target.closest(".js-delete-photo");
        var priBtn = e.target.closest(".js-set-primary");

        if (delBtn) {
            var card = delBtn.closest(".photo-card");
            if (!confirm("Видалити це фото?")) return;

            try {
                if (mode === "listing") {
                    await fetch(deleteUrl + "?photoId=" + card.dataset.id, {
                        method: "POST",
                        headers: { "RequestVerificationToken": getAntiForgeryToken() }
                    });
                } else {
                    await fetch(deleteUrl + "?roomId=" + roomId, {
                        method: "POST",
                        headers: {
                            "Content-Type": "application/json",
                            "RequestVerificationToken": getAntiForgeryToken()
                        },
                        body: JSON.stringify({ url: card.dataset.url })
                    });
                }
                card.remove();
                showStatus("Фото видалено.", false);
            } catch {
                showStatus("Помилка при видаленні.", true);
            }
        }

        if (priBtn && mode === "listing") {
            var card = priBtn.closest(".photo-card");
            var photoId = card.dataset.id;

            try {
                await fetch(primaryUrl + "?listingId=" + listingId + "&photoId=" + photoId, {
                    method: "POST",
                    headers: { "RequestVerificationToken": getAntiForgeryToken() }
                });

                // Update badge and buttons in DOM
                photoGrid.querySelectorAll(".photo-card").forEach(function (c) {
                    var badge = c.querySelector(".photo-card__badge");
                    var btn   = c.querySelector(".js-set-primary");
                    if (c === card) {
                        if (!badge) {
                            var wrap = c.querySelector(".photo-card__img-wrap");
                            var b = document.createElement("span");
                            b.className = "photo-card__badge";
                            b.textContent = "Основне";
                            wrap.appendChild(b);
                        }
                        if (btn) btn.remove();
                    } else {
                        if (badge) badge.remove();
                        if (!c.querySelector(".js-set-primary")) {
                            var actions = c.querySelector(".photo-card__actions");
                            var newBtn = document.createElement("button");
                            newBtn.type = "button";
                            newBtn.className = "admin-btn admin-btn--xs admin-btn--secondary js-set-primary";
                            newBtn.textContent = "★ Основне";
                            actions.insertBefore(newBtn, actions.firstChild);
                        }
                    }
                });

                showStatus("Основне фото змінено.", false);
            } catch {
                showStatus("Помилка при зміні основного фото.", true);
            }
        }
    });

    function getAntiForgeryToken() {
        var el = document.querySelector('input[name="__RequestVerificationToken"]');
        return el ? el.value : "";
    }
})();
