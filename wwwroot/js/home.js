// INITIAL DATA
document.addEventListener("DOMContentLoaded", function () {
    const existingCart = localStorage.getItem("cart");

    if (!existingCart) {
        localStorage.setItem("cart", JSON.stringify([]));
    }

    renderCart();
});

document.querySelector("#checkoutButton").addEventListener("click", function () {
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
});

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
    const { id, name, price, stock } = event.target.dataset;

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
        max: stock,
    };

    const cart = JSON.parse(localStorage.getItem("cart"));
    const existingProduct = cart.find((item) => item.id === id);

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

function existingInCart(id) {
    const cart = JSON.parse(localStorage.getItem("cart"));
    const existingProduct = cart.find((item) => item.id === id);
    return existingProduct != null;
}

const createTrTable = function (index, product) {
    const trElement = document.createElement("tr");
    trElement.setAttribute("id", "cart-" + product.id);
    const { id, price, name, quantity, subtotal } = product;

    trElement.innerHTML = `
   <td class="text-center">
      <span data-id="${id}" style="color:red; cursor:pointer" onclick="removeFromCart(this)">
         <i class="ri-delete-bin-line mr-0"></i>
      </span>
   </td>
   <td class="product-name">${name}</td>
   <td data-id="${id}" class="text-center" onclick="editQuantity(this)" >${quantity}</td>
   <td class="subtotal text-center">${convertVND(subtotal)}</td>`;
    return trElement;
};

const clearTable = function () {
    document.querySelector("tbody").innerHTML = "";
};

const removeFromCart = function (button) {
    let cart = JSON.parse(localStorage.getItem("cart"));
    const { id } = button.dataset;

    const existingProduct = cart.find((item) => item.id === id );

    console.log(existingProduct)

    if (existingProduct) {
        cart = cart.filter((item) => item.id !== button.dataset.id);
        document.querySelector(`#cart-${button.dataset.id}`).remove();
    }

    localStorage.setItem("cart", JSON.stringify(cart));
    updateTotal();
};

const editQuantity = function (button) {
    const cart = JSON.parse(localStorage.getItem("cart"));
    const { id } = button.dataset;
    const currentContent = button.innerText;

    const newContent = prompt("Edit content:", currentContent);

    if (newContent !== null) {
        button.innerText = newContent;
        const existingProduct = cart.find((item) => item.id === id);
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

function debounce(func, delay) {
    let timerId;

    return function () {
        const context = this;
        const args = arguments;

        clearTimeout(timerId);
        timerId = setTimeout(function () {
            func.apply(context, args);
        }, delay);
    };
}

async function getCusInfo(inputField) {
    const url = window.location.origin + "/customers/search?keyword=";
    const phone = inputField.value;
    if (phone) {
        await fetch(url + phone).then(async (res) => {
            const data = await res.json();
            console.log(data)
            if (data.code === 0) {
                document.querySelector("#name").value = data.customer.name;
                document.querySelector("#address").value = data.customer.address;
            }
        });
    }
}

const handleGetCusInfoDebounced = debounce(getCusInfo, 400);
