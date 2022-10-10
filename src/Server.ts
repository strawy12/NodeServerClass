import http from 'http';
import Express, {Application, request, Request,Response} from 'express';
import path from 'path';
import fs from 'fs';
import { Pool, ScoreVO, InventoryVO } from './DB';
import { FieldPacket, ResultSetHeader, RowDataPacket } from 'mysql2';

const data = {
    name: "방과후 데이터",
    users:[
        {id:1, name: "김동윤"},
        {id:2, name: "김대현"},
        {id:3, name: "유하준"},
    ]
};

const app : Application = Express();

app.use(Express.json()); // 들어오는 post 데이터를 json 으로 변환 해줘서 body에 넣어줌
app.use(Express.urlencoded({extended:true})); // 한글 때문에 사용

app.get("/", (req: Request, res: Response) =>
{
    res.json(data);
});

app.post("/insert/inventories", async (req:Request,res:Response)=>
{
    const {user_id, json} = req.body;
   
    let [result, info] : [ResultSetHeader, FieldPacket[]] = await Pool.query(`UPDATE inventories SET user_id=?,json=? where id=1`, [user_id, json]);
    res.json({msg: "성공적으로 기록 완료", insertId: result.insertId});
});



app.get("/record/inventories", async (req:Request, res:Response) =>
{
    const sql = `SELECT * FROM inventories`;
    let[rows,fieldInfos]: [InventoryVO[], FieldPacket[]] = await Pool.query(sql);

    res.json({msg:'data list', data:rows});
});

app.post("/insert/scores", async (req:Request,res:Response)=>
{
    const {score, username} = req.body;

    
    let [result, info] : [ResultSetHeader, FieldPacket[]] = await Pool.query(`INSERT INTO scores (score, username, time) VALUES (?, ?, NOW())`, [score, username]);
    res.json({msg: "성공적으로 기록 완료", insertId: result.insertId});
});

app.get("/record/scores", async (req:Request, res:Response) =>
{
    const sql = `SELECT * FROM scores ORDER BY score DESC LIMIT 0,5`;
    let[rows,fieldInfos]: [ScoreVO[], FieldPacket[]] = await Pool.query(sql);

    res.json({msg:'data list', count: rows.length, data:rows});
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



