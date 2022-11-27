export default class JobTimer
{
    action: () => void;
    time:number = 0;
    intervalTime?:NodeJS.Timer;

    constructor(time:number, action:()=>void)
    {
        this.time = time;
        this.action = action;
    }

    stopTimer() : void{
        clearInterval(this.intervalTime);
    }

    startTimer() : void{
        this.intervalTime = setInterval(this.action, this.time);
    }
}