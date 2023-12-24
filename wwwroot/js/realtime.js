const hubConnections = [
    { name: "productHub", url: "/productHub" },
    { name: "orderHub", url: "/orderHub" }
];

async function startConnection(connectionInfo) {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl(connectionInfo.url)
        .configureLogging(signalR.LogLevel.Information)
        .build();

    connection.on("ProductAdded", function (product) {
        console.log("New product added:", product);
    });

    try {
        await connection.start();
        console.log(`SignalR Connected to ${connectionInfo.name} at ${connection.baseUrl}`);
    } catch (err) {
        console.log(err);
        setTimeout(() => startConnection(connectionInfo), 5000);
    }

    connection.onclose(async () => {
        await startConnection(connectionInfo);
    });
}

async function startAllConnections() {
    for (const connectionInfo of hubConnections) {
        await startConnection(connectionInfo);
    }
}

startAllConnections();
