const historyList = document.getElementById("historyList");

async function loadHistory() {
    historyList.innerHTML = "Завантаження...";

    const response = await fetch("/api/history");
    const history = await response.json();

    if (history.length === 0) {
        historyList.innerHTML = `<p class="empty">Історія пошуку порожня</p>`;
        return;
    }

    historyList.innerHTML = history
        .map(
            (h) => `
        <div class="list-item history-item">
            <div>
                <h3>${h.query}</h3>
                <p class="muted">${new Date(h.searchedAt).toLocaleString("uk-UA")}</p>
            </div>
            <span class="badge ${h.wasFound ? "badge-ok" : "badge-miss"}">
                ${h.wasFound ? "знайдено" : "не знайдено"}
            </span>
        </div>
    `
        )
        .join("");
}

window.loadHistory = loadHistory;
