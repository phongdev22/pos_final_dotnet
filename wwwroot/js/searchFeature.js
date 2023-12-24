const { origin, pathname } = window.location;
const url = origin;

const searchInput = document.querySelector("#search");

// Debounce function
function debounce(func, delay) {
    let timeoutId;
    return function () {
        const context = this;
        const args = arguments;

        clearTimeout(timeoutId);

        timeoutId = setTimeout(function () {
            func.apply(context, args);
        }, delay);
    };
}

async function performSearch() {
    const searchTerm = encodeURIComponent(searchInput.value);

    if (!searchTerm) {
        return;
    }

    if (pathname !== "/products") {
        await fetch(url + "/products" + "/search?keyword=" + searchTerm, {
            method: "GET",
        }).then(async (res) => {
            const products = await res.json();
            console.log(products)
            if (products.length == 1) {
                const { id, name, price, quantity } = products[0];

                const product = {
                    id: id,
                    name: name,
                    price: price,
                    quantity: 1,
                    subtotal: price,
                    max: quantity,
                };

                const cart = JSON.parse(localStorage.getItem("cart"));
                const existingProduct = cart.find((item) => item.id === id);

                if (existingProduct) {
                    existingProduct.quantity = Math.min(existingProduct.quantity + 1, product.max);
                    existingProduct.subtotal = existingProduct.price * existingProduct.quantity;
                } else {
                    cart.push({ ...product, quantity: 1 });
                }
                localStorage.setItem("cart", JSON.stringify(cart));
                renderCart();
                updateTotal();
            }

            if (products.length > 0) {
                document.querySelector("#list-product").innerHTML = "";
                products.forEach((item) => {
                    const wrapper = document.createElement("div");
                    wrapper.classList.add("p-0", "mb-3", "mr-3");
                    wrapper.style.width = "calc(100% / 3 - 20px)";
                    const productCard = `
               <img src="${item.imagePath}" class="col p-0 rounded" width="100%" height="200px" alt="Image" style="object-fit: contain;" />
               <div class="card-body">
                  <h5
                     class="card-title text-truncate"
                     data-toggle="tooltip"
                     data-placement="top"
                     title="${item.name}"
                  >${item.name}</h5>
                  <p class="card-text">
                     ${convertVND(item.price)}
                     <br />
                     In stock:
                     ${item.quantity}
                  </p>
                  <div class="d-flex justify-content-between">
                     <button
                        class="btn btn-primary col-12"
                        onclick="handleAddToCart(event)"
                        data-id="${item.id}"
                        data-name="${item.name}"
                        data-price="${item.price}"
                        data-stock="${item.quantity}"
                     >Buy</button>
                  </div>
               </div>
               `;
                    wrapper.innerHTML = productCard;
                    document.querySelector("#list-product").appendChild(wrapper);
                });
            }
        });
    } else {
    }
}

const debouncedSearch = debounce(performSearch, 300);
searchInput.addEventListener("input", debouncedSearch);

async function getCusInfo(inputField) {
    const btnHistory = document.querySelector("#history");
    const url = window.location.origin + "/customers/search?keyword=";
    const phone = inputField.value;
    if (phone) {
        await fetch(url + phone).then(async (res) => {
            const data = await res.json();
            console.log(data.customer)
            if (data.code === 0) {
                document.querySelector("#name").value = data.customer.Name;
                document.querySelector("#address").value = data.customer.Address;
                btnHistory.classList.remove("disabled");
                btnHistory.setAttribute("href", "/orders/history/" + phone);
            } else {
                btnHistory.classList.add("disabled");
            }
        });
    } else {
        btnHistory.classList.add("disabled");
    }
}

const handleGetCusInfoDebounced = debounce(getCusInfo, 400);
