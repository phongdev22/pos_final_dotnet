// INITIAL DATA
document.addEventListener("DOMContentLoaded", function () {
    const existingCart = localStorage.getItem("cart");

    if (!existingCart) {
        localStorage.setItem("cart", JSON.stringify([]));
    }

    renderCart();
});

const checkout = function () {
    const url = window.location.origin + "/orders/create";

    const phoneNumber = $("#phoneNumber").val();
    const name = $("#name").val();
    const address = $("#address").val();

    if (!phoneNumber || !name || !address) {
        $("#new-order").click();
        return;
    }
    let total = 0;

    const cartData = JSON.parse(localStorage.getItem("cart"));
    cartData.forEach((item) => (total += item.subtotal));

    if (cartData.length != 0) {
        fetch(url, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify({
                customer: {
                    phoneNumber,
                    name,
                    address,
                },
                products: cartData,
                total: total,
            }),
        })
            .then((response) => response.json())
            .then((data) => {
                if (data.code == 0) {
                    localStorage.setItem("cart", JSON.stringify([]));
                    window.location.href = data.returnUrl;
                    return;
                }
                alert(data.message);
            });
    } else {
        alert("Không có dữ liệu giỏ hàng.");
    }
}


document.querySelector("#checkoutButton").addEventListener("click", checkout);

// CONVERT CURRENCY
const convertVND = (x) => x.toLocaleString("it-IT", { style: "currency", currency: "VND" }).replace("VND", "đ");

// RENDER
const renderCart = function () {
    const cart = JSON.parse(localStorage.getItem("cart"));

    clearTable();

    cart.forEach((item, index) => {
        const itemRow = createTrTable(index + 1, item);
        document.querySelector("#cart>tbody").appendChild(itemRow);
    });
    updateTotal();
};

const handleAddToCart = function (event) {
    const { id, name, price, stock, inventory } = event.target.dataset;

    // const stock = parseInt(event.target.dataset.stock);
    if (parseInt(stock) === 0) {
        alert("Out of stock! Please choose another one!");
        return;
    }

    const product = {
        id,
        name,
        price: parseFloat(price),
        quantity: 1,
        subtotal: parseFloat(price),
        inventory: inventory,
        max: stock,
    };

    const cart = JSON.parse(localStorage.getItem("cart"));
    const existingProduct = cart.find((item) => item.id === id && item.inventory === inventory);

    if (existingProduct) {
        existingProduct.quantity = Math.min(existingProduct.quantity + 1, stock);
        existingProduct.subtotal = existingProduct.price * existingProduct.quantity;
    } else {
        cart.push({ ...product, quantity: 1 });
    }
    localStorage.setItem("cart", JSON.stringify(cart));
    renderCart();
    updateTotal();
};

const createTrTable = function (index, product) {
    const trElement = document.createElement("tr");
    const { id, name, quantity, subtotal, inventory } = product;
    trElement.setAttribute("id", "cart-" + product.id + "-" + inventory);

    trElement.innerHTML = `
   <td class="text-center">
      <span data-id="${id}" data-inventory="${inventory}" style="color:red; cursor:pointer" onclick="removeFromCart(this)">
         <i class="ri-delete-bin-line mr-0"></i>
      </span>
   </td>
   <td class="product-name">${name}</td>
   <td data-id="${id}" data-inventory="${inventory}" class="text-center" onclick="editQuantity(this)" >${quantity}</td>
   <td class="subtotal text-center">${convertVND(subtotal)}</td>`;
    return trElement;
};

const clearTable = function () {
    document.querySelector("tbody").innerHTML = "";
};

const removeFromCart = function (button) {
    const cart = JSON.parse(localStorage.getItem("cart"));
    const { id, inventory } = button.dataset;

    const existingProduct = cart.findIndex((item) => {
        if (item.id === id && item.inventory === inventory) {
            return true
        }
        return false;
    });

    if (existingProduct != -1) {
        cart.splice(existingProduct, 1);
        document.querySelector(`#cart-${id}-${inventory}`).remove();
    }

    localStorage.setItem("cart", JSON.stringify(cart));
    updateTotal();
};

const editQuantity = function (button) {
    const cart = JSON.parse(localStorage.getItem("cart"));
    const { id, inventory } = button.dataset;
    const currentContent = button.innerText;

    const newContent = prompt("Edit content:", currentContent);

    if (newContent !== null) {
        button.innerText = newContent;
        const existingProduct = cart.find((item) => item.id === id && item.inventory === inventory);
        existingProduct.quantity = Math.min(parseInt(newContent), existingProduct.max);
        existingProduct.subtotal = existingProduct.price * existingProduct.quantity;
    }
    localStorage.setItem("cart", JSON.stringify(cart));
    renderCart();
    updateTotal();
};

const updateTotal = function () {
    const cart = JSON.parse(localStorage.getItem("cart"));
    let total = 0;
    cart.forEach((item) => (total += item.subtotal));
    document.querySelector("#cart-total").textContent = `Total: ${convertVND(total)}`;
};
