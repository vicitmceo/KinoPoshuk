const searchBtn = document.getElementById("searchBtn");
const titleInput = document.getElementById("movieTitle");
const resultDiv = document.getElementById("result");

searchBtn.addEventListener("click", search);
titleInput.addEventListener("keydown", (e) => {
    if (e.key === "Enter") search();
});

async function search() {
    const title = titleInput.value.trim();
    if (!title) return;

    resultDiv.innerHTML = "Завантаження...";

    const response = await fetch("/api/search?title=" + encodeURIComponent(title));

    if (!response.ok) {
        const error = await response.json().catch(() => ({ Error: "Сталася помилка" }));
        resultDiv.innerHTML = `<p class="error">${error.Error || error.error}</p>`;
        return;
    }

    const movie = await response.json();

    resultDiv.innerHTML = `
        <div class="movie-card">
            <img src="${movie.posterUrl}" alt="${movie.title}" />
            <div class="movie-info">
                <h2>${movie.title} (${movie.year})</h2>
                <p><b>Рейтинг:</b> ${movie.imdbRating}</p>
                <p><b>Тривалість:</b> ${movie.runtime}</p>
                <p><b>Жанр:</b> ${movie.genre}</p>
                <p><b>Режисер:</b> ${movie.director}</p>
                <p><b>У ролях:</b> ${movie.actors}</p>
                <p><b>Опис:</b> ${movie.plot}</p>
                <button class="add-favorite-btn" data-movie='${JSON.stringify(movie).replace(/'/g, "&apos;")}'>+ В обране</button>
            </div>
        </div>
    `;

    resultDiv.querySelector(".add-favorite-btn").addEventListener("click", (e) => {
        const movieData = JSON.parse(e.target.dataset.movie);
        window.addFavorite?.(movieData);
    });
}
