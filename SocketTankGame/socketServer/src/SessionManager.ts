import { tankio } from "./packet/packet";
import SocketSession from "./socketSession";

interface SessionDictionary
{
    [key:number] : SocketSession
}

export default class SessionManager 
{
    static Instance : SessionManager;
    sessionMap: SessionDictionary;

    constructor()
    {
        this.sessionMap = {};
    }

    addSession(session:SocketSession, id:number): void 
    {
        this.sessionMap[id] = session;
    }

    removeSession(id:number) : void
    {
        delete this.sessionMap[id];
    }

    BroadCastMessage(payload: Uint8Array, msgCode:number, senderId:number = 0, exceptSender: boolean = false) : void
    {
        for(let idx in this.sessionMap)
        {
            if(exceptSender == true && senderId == this.sessionMap[idx].playerID)
            {
                continue;
            }

            this.sessionMap[idx].sendData(payload, msgCode);
        }
    }

    getPlayerList():tankio.PlayerInfo[]
    {
        let list: tankio.PlayerInfo[] = [];

        for(let idx in this.sessionMap)
        {
            let s = this.sessionMap[idx];
            if(s.isEnter == false) continue;

            list.push(new tankio.PlayerInfo({playerId:s.playerID, name:s.name, position:s.pos}));
        }

        return list;
    }
}