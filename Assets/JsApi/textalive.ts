import { Player } from "textalive-app-api";
import { sendMessage } from "./unity";

const ReadyMethod = "OnReady";
const PlayReadyMethod = "OnPlayReady";
const OnPauseMethod = "OnPause";
const OnStopMethod = "OnStop";
const OnFinalChorus = "OnFinalChorus";

var player:Player;
var final_chorus:number;
var is_final_send:boolean;

export function initPlayer() {
    player = new Player({app:{token:""}});
    (window as any).player = player;

    player.addListener({
        onAppReady(app) {
            sendMessage(ReadyMethod);
        },
        onMediaElementSet(el) {
            
        },
        onTimerReady(timer) {
            sendMessage(PlayReadyMethod);
            is_final_send = false;   
        },
        onStop() {
            console.log("stop");
            sendMessage(OnStopMethod);
        },
        onPause() {
            console.log("pause");
            sendMessage(OnStopMethod);
        },
        onTimeUpdate(position) {
            final_chorus = player.getChoruses().at(-1).startTime;
            (window as any).final_chorus = final_chorus;
            if (final_chorus < position && (!is_final_send)) {
                is_final_send = true;
                sendMessage(OnFinalChorus);
            }
        },
    });
    
}

export function replaceText(str) {
    str.replaceAll("\u{301C}", " ");
    return str;
}
