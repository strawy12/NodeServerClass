import Express, { Application } from "express";
import { IncomingMessage } from 'http';
import WS, { RawData } from 'ws';
import { tankio } from './packet/packet';
import Path from 'path';
import MapManager from "./MapManager";
import PacketManager from "./PacketManager";
import SocketSession from "./socketSession";
import SessionManager from "./SessionManager";


const App: Application = Express();

const httpServer = App.listen(50000);


const socketServer: WS.Server = new WS.Server({
    server: httpServer,
    //port:50000
});

console.log("Socket Server is running on 50000 port");

PacketManager.Instance = new PacketManager();
MapManager.Instance = new MapManager(Path.join(__dirname, "Tilemap.txt"));
SessionManager.Instance = new SessionManager();

let playerID:number = 1;

socketServer.on("connection", (soc: WS.WebSocket, req: IncomingMessage) => {

    const id:number = playerID;
    const ip:string = req.connection.remoteAddress as string;
    let session :SocketSession = new SocketSession(soc, ip, id, () => 
    {
        SessionManager.Instance.removeSession(id);
    }); 

    SessionManager.Instance.addSession(session, id);
    console.log(`${ip}에서 ${id} 플레이어 접속함`);

    let spawnPos:tankio.Position = MapManager.Instance.getRandomSafePosition();
    let welcomeMsg = new tankio.S_Init({playerId:id, spawnPosition:spawnPos});
    session.sendData(welcomeMsg.serialize(), tankio.MSGID.S_INIT);

    playerID++;

    soc.on("message", (data: RawData, isBinary: boolean) => {
        
        if(isBinary)
            session.recieveMsg(data);
    });


});