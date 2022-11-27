import SocketSession from "../socketSession";
import { PacketHandler } from "./packetHandler";
import { tankio } from "./packet";
import SessionManager from "../SessionManager";

export class CEnterHandler implements PacketHandler
{
    constructor(){}
        handleMsg(session:SocketSession, buffer:Buffer):void
        {
            let enter:tankio.C_Enter = tankio.C_Enter.deserialize(buffer); 
            
            session.isEnter = true;
            session.pos = enter.position;

            session.name =  enter.name;


            let info = new tankio.PlayerInfo({
                playerId:session.playerID,
                name:session.name,
                position:session.pos
            });
            let sEnter : tankio.S_Enter = new tankio.S_Enter({player:info}); 

            SessionManager.Instance.BroadCastMessage(sEnter.serialize(), tankio.MSGID.S_ENTER, session.playerID, true);

            let list = SessionManager.Instance.getPlayerList();
            let initMsg = new tankio.S_InitList({players:list});
            session.sendData(initMsg.serialize(), tankio.MSGID.S_INITLIST);
         }
}