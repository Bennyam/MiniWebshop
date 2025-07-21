const API_BASE = "http://localhost:5273/api/Cart";
const PRODUCT_API = "http://localhost:5273/api/Product";

async function laadProducten() {
  const res = await fetch(PRODUCT_API);
  const producten = await res.json();

  const lijst = document.getElementById("producten");
  lijst.innerHTML = "";

  producten.forEach((product) => {
    const li = document.createElement("li");
    li.innerHTML = `
      <span>${product.naam} - €${product.prijs.toFixed(2)}</span>
      <button onclick="voegToe(${product.id})">Voeg toe</button>
    `;
    lijst.appendChild(li);
  });
}

async function voegToe(productId) {
  await fetch(`${API_BASE}/add`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ productId, aantal: 1 }),
  });
  updateCart();
}

async function updateCart() {
  const resItems = await fetch(`${API_BASE}/items`);
  const items = await resItems.json();

  const resTotal = await fetch(`${API_BASE}/total`);
  const total = await resTotal.json();

  const resFinal = await fetch(`${API_BASE}/final`);
  const final = await resFinal.json();

  const lijst = document.getElementById("cart-items");
  lijst.innerHTML = "";
  items.forEach((i) => {
    const li = document.createElement("li");
    li.textContent = `${i.product.naam} x${i.aantal} = €${(
      i.product.prijs * i.aantal
    ).toFixed(2)}`;
    lijst.appendChild(li);
  });

  document.getElementById("totaal").textContent =
    total.totaalZonderKorting.toFixed(2);
  document.getElementById("final").textContent =
    final.totaalMetKorting.toFixed(2);
}

document.getElementById("leeg-cart").addEventListener("click", async () => {
  await fetch(`${API_BASE}/clear`, { method: "POST" });
  updateCart();
});

laadProducten();
updateCart();

const kortingTypeSelect = document.getElementById("korting-type");
const kortingFields = document.getElementById("korting-fields");
const kortingBtn = document.getElementById("korting-btn");
const kortingFeedback = document.getElementById("korting-feedback");

kortingTypeSelect.addEventListener("change", updateKortingUI);
kortingBtn.addEventListener("click", pasKortingToe);

function updateKortingUI() {
  const type = kortingTypeSelect.value;
  kortingFields.innerHTML = "";

  if (type === "percentage") {
    kortingFields.innerHTML = `
      <input type="number" id="percentage" placeholder="Percentage (%)" />
    `;
  } else if (type === "vast") {
    kortingFields.innerHTML = `
      <input type="number" id="bedrag" placeholder="Bedrag (€)" />
      <input type="number" id="minTotaal" placeholder="Minimumbedrag (€)" />
    `;
  } else if (type === "percategorie") {
    kortingFields.innerHTML = `
      <input type="text" id="categorie" placeholder="Categorie (bijv. Boeken)" />
      <input type="number" id="percentage" placeholder="Percentage (%)" />
    `;
  }
}

async function pasKortingToe() {
  const type = kortingTypeSelect.value;
  const body = { type };

  if (type === "percentage") {
    body.percentage = parseFloat(document.getElementById("percentage").value);
  } else if (type === "vast") {
    body.bedrag = parseFloat(document.getElementById("bedrag").value);
    body.minTotaal = parseFloat(document.getElementById("minTotaal").value);
  } else if (type === "percategorie") {
    body.categorie = document.getElementById("categorie").value;
    body.percentage = parseFloat(document.getElementById("percentage").value);
  }

  const res = await fetch(`${API_BASE}/discount`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(body),
  });

  const text = await res.text();
  kortingFeedback.textContent = text;

  updateCart();
}

updateKortingUI(); // Initieel inladen
