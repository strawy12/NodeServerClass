const http = require('http');
const Express = require('express');


const data = {
    name: "방과후 데이터",
    users:[
        {id:1, name: "김동윤"},
        {id:2, name: "김대현"},
        {id:3, name: "유하준"},
    ]
};

const app = new Express();

app.get("/", (req, res) =>
{
    res.json(data);
});

app.get("/iamge", (req, res) =>
{
    // 아직 미구현
});

const server = http.createServer(app);
server.listen(50000, ()=>
{
    console.log("서버가 50000번째 포트에서 실행중입니다.");
});


