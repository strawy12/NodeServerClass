import SocketSession from "../socketSession";

export interface PacketHandler
{
    handleMsg(session : SocketSession, buffer:Buffer): void;
}