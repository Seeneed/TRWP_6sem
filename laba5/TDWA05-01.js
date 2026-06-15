const http = require('http');

const nick = process.argv[2];
const port = parseInt(process.argv[3]);
const baseDelay = parseInt(process.argv[4]);

if (!nick || !port || !baseDelay) {
    console.error("Использование: node TDWA05-01.js <Nick> <Port> <Delay>");
    process.exit(1);
}

const server = http.createServer((req, res) => {
    if (req.url.endsWith('/A') || req.url === '/lb') {
        let currentDelay = baseDelay;
        
        switch (req.method) {
            case 'GET':    currentDelay = baseDelay * (1 / 3); break;
            case 'POST':   currentDelay = baseDelay * (2 / 3); break;
            case 'PUT':    currentDelay = baseDelay; break;
            case 'DELETE': currentDelay = baseDelay * (1 / 4); break;
        }

        setTimeout(() => {
            res.setHeader('Content-Type', 'application/json');
            res.writeHead(200);
            res.end(JSON.stringify({
                Nick: nick,
                Method: req.method
            }));
        }, currentDelay);

    } else {
        res.writeHead(404);
        res.end("Not Found");
    }
});

server.listen(port, () => {
    console.log(`Сервер [${nick}] запущен. Порт: ${port}, Базовая задержка: ${baseDelay}мс`);
});