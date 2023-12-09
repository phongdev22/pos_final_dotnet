const { origin, pathname } = window.location;
const url = origin;

const searchInput = document.querySelector("#search");

// Debounce function
function debounce_1(func, delay) {
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
         const { code, data } = await res.json();

         if (data.products.length == 1) {
            const { _id, productName, price, quantity } = data.products[0];

            const product = {
               id: _id,
               name: productName,
               price: price,
               quantity: 1,
               subtotal: price,
               max: quantity,
            };

            const cart = JSON.parse(localStorage.getItem("cart"));
            const existingProduct = cart.find((item) => item.id === _id);

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

         if (code === 0 && data.products.length > 0) {
            document.querySelector("#list-product").innerHTML = "";
            data.products.forEach((item) => {
               const wrapper = document.createElement("div");
               wrapper.classList.add("p-0", "mb-3", "mr-3");
               wrapper.style.width = "calc(100% / 3 - 20px)";
               const productCard = `
               <img src="${item.imagePath}" class="col p-0 rounded" width="100%" height="200px" alt="Image" />
               <div class="card-body">
                  <h5
                     class="card-title text-truncate"
                     data-toggle="tooltip"
                     data-placement="top"
                     title="${item.productName}"
                  >${item.productName}</h5>
                  <p class="card-text">
                     ${convertVND(item.price * 1000)}
                     <br />
                     In stock:
                     ${item.quantity}
                  </p>
                  <div class="d-flex justify-content-between">
                     <button
                        class="btn btn-primary col-12"
                        onclick="handleAddToCart(event)"
                        data-id="${item._id}"
                        data-name="${item.productName}"
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
      // const response = await fetch(url + pathname + "/search?keyword=" + searchTerm, {
      //    method: "GET",
      // });
   }
}

const debouncedSearch = debounce_1(performSearch, 300);
searchInput.addEventListener("input", debouncedSearch);

