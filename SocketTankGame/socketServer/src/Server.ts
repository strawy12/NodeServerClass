import Express, { Application } from "express";
import { IncomingMessage } from 'http';
import WS, { RawData } from 'ws';
import { tankio } from './packet/packet';
import Path from 'path';
import MapManager from "./MapManager";


const App: Application = Express();

MapManager.Instance = new MapManager(Path.join(__dirname, "Tilemap.txt"));

const httpServer = App.listen(50000, () => {
    console.log("Server is running on 50000 port");
});


const socketServer: WS.Server = new WS.Server({
    server: httpServer,
    //port:50000
});

socketServer.on("connection", (soc: WS.WebSocket, req: IncomingMessage) => {

    soc.on("message", (data: RawData, isBinary: boolean) => {


        let length: number = (data.slice(0, 2) as Buffer).readInt16LE();
        let code: number = (data.slice(2, 4) as Buffer).readInt16LE();
        let payload = data.slice(4) as Buffer; // 4부터 끝까지 잘라내기

        console.log(code);
        if (code == tankio.MSGID.C_POS) {
            let cPos:tankio.C_Pos = tankio.C_Pos.deserialize(payload); 
            console.log(cPos.x, cPos.y);
        }
    });

});