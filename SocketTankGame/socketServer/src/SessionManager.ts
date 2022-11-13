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
}