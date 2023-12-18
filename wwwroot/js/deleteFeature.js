async function handleDelete(button) {
   const { id, name } = button.dataset;
   const { origin, pathname } = window.location;
   const isConfirmed = confirm(`Are you sure you want to delete item: ${name} ?`);
   if (isConfirmed) {
       await fetch(`${origin}${pathname}/delete/${id}`, { method: "DELETE" })
         .then(async (res) => {
            const result = await res.json();
            if (result.code === 0) {
                alert(`Delete ${name} successfully.`);
               document.querySelector(`#data-${id}`).remove();
            }
            if (result.code === 1) {
               alert(`${result.message}`);
            }
         })
         .catch((error) => console.log(error));
   }
}
