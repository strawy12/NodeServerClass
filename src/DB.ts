
import MySQL, {RowDataPacket} from 'mysql2/promise'

const poolOption : MySQL.PoolOptions =
{
    host: 'gondr.asuscomm.com',
    user: 'yy_40103',
    password: '1234',
    database:'yy_40103',
    connectionLimit: 10
}

export interface ScoreVO extends RowDataPacket
{
    id:number,
    score:number,
    username:string,
    time:Date
}

export interface InventoryVO extends RowDataPacket
{
    id:number,
    user_id:number,
    json:string
}

export const Pool : MySQL.Pool = MySQL.createPool(poolOption);