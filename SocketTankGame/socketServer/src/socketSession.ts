import WebSocket, {RawData} from "ws";
import { tankio } from "./packet/packet";
import PacketManager from "./PacketManager";



export default class SocketSession
{
    socket:WebSocket;
    ipAddress:string;
    playerID:number;
    
    name:string = "unknown";
    isEnter:boolean = false;
    beforePos:tankio.Position = new tankio.Position({x:0,y:0,rotate:0});
    pos:tankio.Position = new tankio.Position({x:0,y:0,rotate:0});
    nesxPos:tankio.Position = new tankio.Position({x:0,y:0,rotate:0});


    constructor(socket:WebSocket, ipAddress:string, playerID:number, closeCallback:Function)
    {
        this.socket = socket;
        this.ipAddress = ipAddress;
        this.playerID = playerID;

        this.socket.on("close", (code:number,reason:Buffer) =>
        {
            console.log(`${this.playerID}님이 ${code}로 종료하였습니다.`);
            closeCallback();
        });
    }

    getInt16LEFromBuffer(buffer:Buffer) : number
    {
        return buffer.readUInt16LE();
    }

    recieveMsg(data:RawData):void
    {
        let code:number = this.getInt16LEFromBuffer(data.slice(2,4) as Buffer);
        PacketManager.Instance.handlerMap[code].handleMsg(this,data.slice(4) as Buffer);
    }
    sendData(payload:Uint8Array, msgCode:number):void
    {
        let len:number = payload.length + 4;

        let lenBuffer : Uint8Array = new Uint8Array(2);
        new DataView(lenBuffer.buffer).setUint16(0, len, true);

        let msgCodeBuffer : Uint8Array = new Uint8Array(2);
        new DataView(msgCodeBuffer.buffer).setUint16(0, msgCode, true);

        let sendBuffer:Uint8Array = new Uint8Array(len);
        sendBuffer.set(lenBuffer, 0);
        sendBuffer.set(msgCodeBuffer, 2);
        sendBuffer.set(payload, 4);

        this.socket.send(sendBuffer);
    }
}