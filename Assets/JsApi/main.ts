import { initPlayer, replaceText } from "./textalive";
(window as any).initPlayer = initPlayer;
(window as any).replaceText = replaceText;
(window as any).skyway_load = true;

import("./skyway").then(
    (s_module) => {

    }
).catch(
    (res) => {
        console.warn(res);
        (window as any).skyway_load = false;
    }
);

/*
import { createHost, createClient, writeStream, recreateToken, changeJoin, closeRoom, room } from "./skyway";
(window as any).createHost = createHost;
(window as any).createClient = createClient;
(window as any).writeStream = writeStream;
(window as any).recreateToken = recreateToken;
(window as any).changeJoin = changeJoin;
(window as any).closeRoom = closeRoom;
(window as any).room = room;
*/