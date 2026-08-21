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
    const data = await response.json();

    if (data.Response === "False") {
        resultDiv.innerHTML = `<p class="error">Фільм не знайдено</p>`;
        return;
    }

    resultDiv.innerHTML = `
        <div class="movie-card">
            <img src="${data.Poster}" alt="${data.Title}" />
            <div class="movie-info">
                <h2>${data.Title} (${data.Year})</h2>
                <p><b>Рейтинг:</b> ${data.imdbRating}</p>
                <p><b>Тривалість:</b> ${data.Runtime}</p>
                <p><b>Режисер:</b> ${data.Director}</p>
                <p><b>У ролях:</b> ${data.Actors}</p>
                <p><b>Опис:</b> ${data.Plot}</p>
            </div>
        </div>
    `;
}
