async function handleUpdateStatus(event) {
    const { origin, pathname } = window.location;
    const url = `${origin}${pathname}`;


    if (event && event.target.tagName === "SPAN") {
        event.stopPropagation();
        const id = event.currentTarget.getAttribute("data-id");

        await fetch(`${url}/ChangeStatus?id=${id}`, {
            method: "POST",
        })
            .then(async function (res) {
                const data = await res.json();
                console.log(data)
                if (data.code == 0) {
                    if (data.status) {
                        event.target.classList.remove("bg-danger");
                        event.target.classList.add("bg-success");
                        event.target.textContent = "Active";
                    } else {
                        event.target.classList.remove("bg-success");
                        event.target.classList.add("bg-danger");
                        event.target.textContent = "Locked";
                        event.target.setAttribute("class", "btnStatus badge bg-danger");
                    }
                }
            })
            .catch((err) => {
                console.log(err);
            });
    }
}
