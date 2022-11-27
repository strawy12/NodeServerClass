import { CEnterHandler } from "./packet/CEnterHandler";
import { CMoveHandler } from "./packet/CMoveHandler";
import { tankio } from "./packet/packet";
import { PacketHandler } from "./packet/packetHandler";

interface HandlerDictionary {
    [key: number]: PacketHandler
}


export default class PacketManager {
    static Instance: PacketManager;
    handlerMap: HandlerDictionary;

    constructor() {
        console.log("Packet Manager initalize...");
        this.handlerMap = {};
        this.register();
    }

    register(): void {
       this.handlerMap[tankio.MSGID.C_ENTER] = new CEnterHandler();
       this.handlerMap[tankio.MSGID.C_MOVE] = new CMoveHandler();
    };
    }
