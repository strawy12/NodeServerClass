import SocketSession from "../socketSession";
import { PacketHandler } from "./packetHandler";
import { tankio } from "./packet";
import SessionManager from "../SessionManager";
import MapManager from "../MapManager";

export class CHitReportHandler implements PacketHandler
{
    constructor(){
    }
        handleMsg(session:SocketSession, buffer:Buffer):void
        {
           let cReport = tankio.C_Hit_Report.deserialize(buffer);

            let { playerId, fireId, x, y, damage } = cReport;

            let {x:sPosX, y:sPosY} = session.pos;

            let dist = Math.pow(sPosX - x, 2) + Math.pow(sPosY - y, 2);

            let confirm = new tankio.S_Hit_Confirm({playerId,fireId,x,y,damage,isCritical:false, isIgnore:false});

            if(dist > 0.7) // 0.7 은 노가다로 구한 탱크 크기의 반지름의 제곱
            {
                console.log(`충돌이 불가능한 총알입니다. ${fireId}`);
                confirm.isIgnore = true;
            }else if(MapManager.Instance.IsSafeZone({x,y})){
                console.log(`안전 지대에 있음. ${x}, ${y}`);
                confirm.isIgnore = true;
            }
            else{
                if(Math.random() < 0.3)

                confirm.damage *= 2;
                confirm.isCritical = true;
            }

            SessionManager.Instance.BroadCastMessage(confirm.serialize(),
            tankio.MSGID.S_HIT_CONFIRM,
            session.playerID, false);
        }
}