const favoritesList = document.getElementById("favoritesList");

async function loadFavorites() {
    favoritesList.innerHTML = "Завантаження...";

    const response = await fetch("/api/favorites");
    const favorites = await response.json();

    if (favorites.length === 0) {
        favoritesList.innerHTML = `<p class="empty">Список обраного порожній</p>`;
        return;
    }

    favoritesList.innerHTML = favorites
        .map(
            (f) => `
        <div class="list-item" data-id="${f.id}">
            <img src="${f.posterUrl}" alt="${f.title}" />
            <div>
                <h3>${f.title} (${f.year})</h3>
                <p class="muted">Додано: ${new Date(f.addedAt).toLocaleDateString("uk-UA")}</p>
            </div>
            <button class="remove-btn" data-id="${f.id}">Видалити</button>
        </div>
    `
        )
        .join("");

    favoritesList.querySelectorAll(".remove-btn").forEach((btn) => {
        btn.addEventListener("click", async () => {
            await fetch("/api/favorites/" + btn.dataset.id, { method: "DELETE" });
            loadFavorites();
        });
    });
}

async function addFavorite(movie) {
    await fetch("/api/favorites", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
            imdbId: movie.imdbId,
            title: movie.title,
            year: movie.year,
            posterUrl: movie.posterUrl
        })
    });
}

window.loadFavorites = loadFavorites;
window.addFavorite = addFavorite;
