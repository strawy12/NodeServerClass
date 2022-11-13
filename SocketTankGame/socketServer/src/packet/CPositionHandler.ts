// import SocketSession from "../socketSession";
// import { PacketHandler } from "./packetHandler";
// import { tankio } from "./packet";
// import MapManager,{ MapCategory } from "../MapManager";

// export class CPositionHandler implements PacketHandler
// {
//     constructor(){}
//         handleMsg(session:SocketSession, buffer:Buffer):void
//         {
//             let cPos:tankio.C_Pos = tankio.C_Pos.deserialize(buffer); 
//             console.log(cPos.x, cPos.y);
//             let mapInfo:MapCategory = MapManager.Instance.getMapData(cPos.x, cPos.y);
//             console.log(mapInfo);

//             let {x,y} = cPos;
//             let sPos : tankio.S_Pos = new tankio.S_Pos({x,y}); 

//             session.sendData(sPos.serialize(), tankio.MSGID.S_POS); 
//          }
// }