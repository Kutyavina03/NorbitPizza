let cart = JSON.parse(localStorage.getItem('cart')) || [];

async function renderCartPage() {
    const container = document.getElementById('cart-page-items');
    const total = document.getElementById('cart-page-total');

    container.innerHTML = '';
    let sum = 0;

    cart.forEach((item, index) => {
        sum += item.price * item.quantity;

        const div = document.createElement('div');
        div.className = 'cart-item';
        div.innerHTML = `
            <span>${item.name} — ${item.price} руб.</span>
            <div class="qty-controls">
                <button onclick="changeQuantity(${index}, -1)">-</button>
                <span>${item.quantity}</span>
                <button onclick="changeQuantity(${index}, 1)">+</button>
            </div>
            <button class="remove-btn" onclick="removeFromCart(${index})">×</button>
        `;
        container.appendChild(div);
    });


    try {
        const res = await fetch('https://api.coingecko.com/api/v3/simple/price?ids=solana&vs_currencies=rub');
        const data = await res.json();
        const solRate = data.solana.rub;
        const sumSol = sum / solRate; 

        const solDisplay = document.getElementById('total-sol');
        if (solDisplay) {
            solDisplay.textContent = `≈ ${sumSol.toFixed(4)} SOL`;
        } else {
            const solDiv = document.createElement('div');
            total.textContent = `≈ ${sumSol.toFixed(4)} SOL`;
            container.appendChild(solDiv);
        }
    } catch (err) {
        console.error('Ошибка при получении курса SOL:', err);
    }
}


function changeQuantity(index, delta) {
    cart[index].quantity += delta;
    if (cart[index].quantity < 1) cart.splice(index, 1);
    localStorage.setItem('cart', JSON.stringify(cart));
    renderCartPage();
}

function removeFromCart(index) {
    cart.splice(index, 1);
    localStorage.setItem('cart', JSON.stringify(cart));
    renderCartPage();
}

function submitOrder() {
    const name = document.getElementById('userName').value.trim();
    const phone = document.getElementById('userPhone').value.trim();
    const address = document.getElementById('userAddress').value.trim();

    if (!name || !phone || !address) {
        alert('Заполните все поля!');
        return;
    }
    if (cart.length === 0) {
        alert('Корзина пуста!');
        return;
    }

    document.getElementById("orderData").value = JSON.stringify(cart);
    document.getElementById("orderForm").submit();
    localStorage.removeItem('cart');
    cart = [];
}

renderCartPage();