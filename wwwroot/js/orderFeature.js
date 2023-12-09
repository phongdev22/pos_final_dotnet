const convertVND = (x) => x.toLocaleString("it-IT", { style: "currency", currency: "VND" }).replace("VND", "đ");

function handleCalcCash(button) {
   const { value } = button.dataset;
   const cash = document.querySelector("#cash");

   document.querySelector("#btn-complete").disabled = false;

   if (cash.value) {
      cash.value = parseInt(cash.value) * parseInt(value * 1000);

      const orderTotal = parseInt(document.querySelector("#total").value);

      document.querySelector("#return-money").textContent =
         "Return: " + convertVND(parseInt(cash.value) - orderTotal * 1000);
   }
}

document.querySelector("#cash").oninput = function (event) {
   if (!this.value) {
      document.querySelector("#btn-complete").disabled = true;
      document.querySelector("#return-money").textContent = "Return: 0đ";
   }
   document.querySelector("#btn-complete").disabled = false;
};

async function handleComplete(button) {
   const { orderid } = button.dataset;
   const customerMoney = parseInt(document.querySelector("#cash").value) / 1000;

   await fetch("/orders/complete-order", {
      method: "POST",
      headers: {
         "Content-Type": "application/json",
      },
      body: JSON.stringify({ orderID: orderid, status: "paid", customerMoney }),
   }).then(async (res) => {
      const result = await res.json();

      if (result.code === 0) {
         printInvoice("#invoice-pdf", () => {
            localStorage.setItem("cart", JSON.stringify([]));
            window.location.href = result.returnUrl;
         });
      } else {
         alert("Complete failed!");
      }
   });
}

function printInvoice(selector, callback) {
   const content = document.querySelector(selector);

   const head = document.querySelector("head").cloneNode(true);
   const body = document.createElement("body");
   const html = document.createElement("html");

   head.querySelector("title").innerHTML = "invoice.pdf";

   body.innerHTML = content.innerHTML;

   html.appendChild(head);
   html.appendChild(body);

   const printWindow = window.open("", "_blank");
   printWindow.document.write(html.innerHTML);

   setTimeout(function () {
      printWindow.print();
      printWindow.close();
      callback();
   }, 1000);
}
