import http from 'http';
import Express, {Application, request, Request,Response} from 'express';
import path from 'path';
import fs from 'fs';
import { Pool, ScoreVO, InventoryVO, UserVO } from './DB';
import { FieldPacket, ResultSetHeader, RowDataPacket } from 'mysql2';
import JWT from "jsonwebtoken"
import { Key } from "./Secret"

const app : Application = Express();

app.use(Express.json()); // 들어오는 post 데이터를 json 으로 변환 해줘서 body에 넣어줌
app.use(Express.urlencoded({extended:true})); // 한글 때문에 사용

app.get("/", (req: Request, res: Response) =>
{
    
});

app.post("/inven", async (req:Request,res:Response)=>
{
    const {user_id, json} : {user_id:number,json:string} = req.body;
    let sql = `SELECT * FROM inventories WHERE user_id = ?`;
    let [row, infos] : [InventoryVO[], FieldPacket[]] = await Pool.query(sql, [user_id]);

    try 
    {
        if(row.length == 0)
        {
            sql = `INSERT INTO inventories (user_id, json) VALUES (?, ?)`;
            let [result, info] : [ResultSetHeader, FieldPacket[]] = await Pool.query(sql, [user_id, json]);

            if(result.affectedRows != 1) throw `Not Affected`;
        }

        else
        {
            sql = `UPDATE inventories SET json=? where user_id= ?`;
            let [result, info] : [ResultSetHeader, FieldPacket[]] = await Pool.query(sql, [json, user_id]);
         
            if(result.affectedRows != 1) throw `Not Affected`;
        }

    res.json({success:true, msg: "저장 완료"});

    }
    catch(e)
    {
        console.log(e);
        res.json({success:false, msg: "데이터 베이스 저장 중 오류 발생 "});
    }

});


app.get("/inven", async (req:Request, res:Response) =>
{
    const user_id:number = 1; // 나중에 토큰에서 값을 빼서 인증 하는 식으로 변경
    const sql = `SELECT * FROM inventories WHERE user_id = ?`;
    let[rows,fieldInfos]: [InventoryVO[], FieldPacket[]] = await Pool.query(sql, [user_id]);

    if(rows.length == 0)
    {
        res.json({msg:"로딩 완료", data:""});
    }
    else
    {
        res.json({msg:'로딩 완료', data:rows[0].json});
    }
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

app.post("/user", async (req:Request, res:Response) =>
{
    let sql = "INSERT INTO users (account, name, pass) VALUE (?,?,PASSWORD(?))";
    const {account, name, pass} : {account:string, name:string, pass:string} = req.body;
    
    try
    {
        let [result, info] : [ResultSetHeader, FieldPacket[]] = await Pool.query(sql, [account, name, pass]);

        if(result.affectedRows == 0) throw "Error";

        res.json({success:true, msg:"성공적으로 회원가입"}) ;
    }
    catch(e)
    {   
        console.log(e);
        res.json({ success: false, msg:"회원가입중 오류 발생"}) ;

    }


});

app.post("/login", async (req:Request, res:Response) =>
{
    const {account, pass} : {account:string, pass:string} = req.body;

    console.log(account, pass);


    let token:string = JWT.sign(   
    {
        account:account,
        name:"유하준"    
    },
    Key.secret,
    {
        algorithm:"HS256",
        expiresIn:"30 days"
    });

    res.json({msg:"로그인 성공", token});
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



