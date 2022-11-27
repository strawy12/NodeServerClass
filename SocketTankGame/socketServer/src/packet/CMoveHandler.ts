import SocketSession from "../socketSession";
import { PacketHandler } from "./packetHandler";
import { tankio } from "./packet";

export class CMoveHandler implements PacketHandler
{
    constructor(){}
        handleMsg(session:SocketSession, buffer:Buffer):void
        {
            let cMove = tankio.C_Move.deserialize(buffer);

            session.pos = cMove.position;


        }
}