import SocketSession from "../socketSession";
import { PacketHandler } from "./packetHandler";
import { tankio } from "./packet";
import MapManager,{ MapCategory } from "../MapManager";

export class CEnterHandler implements PacketHandler
{
    constructor(){}
        handleMsg(session:SocketSession, buffer:Buffer):void
        {
            let cEnter:tankio.C_Enter = tankio.C_Enter.deserialize(buffer); 
            
            let mapInfo:MapCategory = MapManager.Instance.getMapData(cEnter.position.x, cEnter.position.y);

            let {x,y} = cEnter.position;
            let sEnter : tankio.S_Enter = new tankio.S_Enter({}); 

            session.sendData(sEnter.serialize(), tankio.MSGID.S_ENTER); 
         }
}