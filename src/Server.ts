import http from 'http';
import Express, {Application, Request,Response} from 'express';
import path from 'path';
import fs from 'fs';

const data = {
    name: "방과후 데이터",
    users:[
        {id:1, name: "김동윤"},
        {id:2, name: "김대현"},
        {id:3, name: "유하준"},
    ]
};

const app : Application = Express();


app.get("/", (req: Request, res: Response) =>
{
    res.json(data);
});

app.get("/image/:filename", (req: Request, res: Response) =>
{
    let filename:string = req.params.filename;

    let filePath = path.join(__dirname, "..", "images", filename);
    if(!fs.existsSync(filePath))
    {
        filePath = path.join(__dirname, "..", "images", "NotFound.jpg");
    }

    res.sendFile(filePath);
});

app.get("/imageList", (req:Request, res:Response) =>
{
    let imagePath = path.join(__dirname, "..", "images");
    let fileList:string[] = fs.readdirSync(imagePath);

    fileList = fileList.filter(x=>x != "NotFound.jpg");

    let msg = {
        text: "성공적으로 로딩",
        count: fileList.length,
        list: fileList
    }

    res.json(msg);
});

const server = http.createServer(app);
server.listen(50000, ()=> 
{
    console.log("서버가 50000번째 포트에서 실행중입니다.");
});



