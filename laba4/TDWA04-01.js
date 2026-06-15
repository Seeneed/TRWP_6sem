const http = require('http');

const nick = process.argv[2];
const port = parseInt(process.argv[3]);

if (!nick || !port) {
    console.error("Использование: node TDWA04-01.js <Nick> <Port>");
    process.exit(1);
}

const server = http.createServer((req, res) => {
    const expectedPath = `/A`;

    if (req.url === expectedPath) {
        res.setHeader('Content-Type', 'application/json');
        res.writeHead(200);
        
        res.end(JSON.stringify({
            Nick: nick,
            Method: req.method
        }));
    } else {
        res.writeHead(404);
        res.end("Not Found");
    }
});

server.listen(port, () => {
    console.log(`Сервер [${nick}] запущен на http://localhost:${port}`);
});