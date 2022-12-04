import SocketSession from "../socketSession";
import { PacketHandler } from "./packetHandler";
import { tankio } from "./packet";
import SessionManager from "../SessionManager";

export class CFireHandler implements PacketHandler
{
    fireId:number;


    constructor(){
        this.fireId = 0;
    }
        handleMsg(session:SocketSession, buffer:Buffer):void
        {
            let cFire = tankio.C_Fire.deserialize(buffer);
            
            this.fireId++;
            
            let {playerId, x,y,z,dirX, dirY} = cFire;
            let sFire = new tankio.S_Fire({
                playerId, x,y,z,dirX,dirY, fireId: this.fireId
            });

            SessionManager.Instance.BroadCastMessage(sFire.serialize(), tankio.MSGID.S_FIRE);
        }
}