import { SkyWayStreamFactory, SkyWayContext, SkyWayRoom, P2PRoom, LocalDataStream, LocalP2PRoomMember, RoomPublication, RemoteRoomMember } from "@skyway-sdk/room";
import { sendMessage } from "./unity";

const changeMemberMethod = "changeMember";
const closeRoomMethod = "onClose";
const retryRoomMethod = "retryRoomName";
const requestTokenMethod = "requestToken";
const connectMethod = "onConnect";
const onDataMethod = "onData";

var stream_data:LocalDataStream;
export var room:P2PRoom;
var me:LocalP2PRoomMember;
let skyway_context:SkyWayContext;
let member_count = 1;
let is_host = false;
let join_mode = true;

export function createHost(token: string, room_name: string) {
    console.log("room_name: %s", room_name);
    createStream();
    is_host = true;
    member_count = 1;
    SkyWayContext.Create(token).then(context => {
        skyway_context = context;
        skyway_context.onTokenUpdateReminder.add(requestToken);
        try {
            let tmp = SkyWayRoom.Find(skyway_context, {
                name: room_name
            }, "p2p");
            
            if (tmp) {
                console.warn("Room already exists");
                /*
                setTimeout(() => {
                sendMessage(retryRoomMethod);
                }, 10);
                return;
                */
            }
        } catch (error) {
            console.log("Room not exist");
        }
        SkyWayRoom.FindOrCreate(skyway_context, {
            type: "p2p",
            name: room_name,
        }).then(r => {
            room = r;
            (window as any).room = room;
            room.join().then(user => {
                me = user;
                me.publish(stream_data);
                sendMessage(connectMethod, me.id);
                room.publications.forEach(onPublished);
                room.onClosed.add(() => sendMessage(closeRoomMethod));
                room.onMemberJoined.add((joinMember) => {
                    // 5人以上入室した場合、退出させる
                    member_count++;
                    if (5 < member_count || !join_mode) {
                        joinMember.member.leave();
                    } else {
                        changeMember(room.members, me.id)
                    }
                });
                room.onMemberLeft.add((leftMember) => {
                    member_count--;
                    changeMember(room.members, me.id)
                })
                room.onStreamPublished.add((e) => onPublished(e.publication));
            });
        })
    })
}

export function createClient(token: string, room_name: string) {
    console.log("room_name: %s", room_name);
    createStream();
    is_host = false;
    SkyWayContext.Create(token).then(context => {
        skyway_context = context;
        skyway_context.onTokenUpdateReminder.add(requestToken);
        SkyWayRoom.Find(skyway_context, {
            name: room_name,
        }, "p2p").then(r => {
            room = r;
            (window as any).room = room;
            room.join().then(user => {
                me = user;
                me.publish(stream_data);
                changeMember(room.members, me.id)
                room.onMemberJoined.add((joinMember) => {
                    member_count++;
                    changeMember(room.members, me.id)
                });
                room.onMemberLeft.add((leftMember) => {
                    member_count--;
                    changeMember(room.members, me.id)
                })
                room.publications.forEach(onPublished);
                room.onClosed.add(() => sendMessage(closeRoomMethod));
                room.onStreamPublished.add((e) => onPublished(e.publication));
            }).catch(() => {
                //sendMessage()
            });
        })
    })
}

export function onPublished(publication: RoomPublication) {
    if (publication.publisher.id === me.id) return;

    me.subscribe(publication.id).then((stream) => {
        if (stream.stream.contentType == "data") {
            stream.stream.onData.add((data) => {
                let from_id = publication.publisher.id;
                sendMessage(onDataMethod, `${from_id};${data.toString()}`);
                //console.log("from: %s, data: %s", from_id, data.toString());
            });
        }
    });
}

export function changeMember(members: RemoteRoomMember[], meId:string) {
    let sendData = "";
    for (const member of members) {
        if (member.id != meId) {
            sendData = member.id + ";";
        }
    }
    sendMessage(changeMemberMethod, sendData);
}

export function createStream() {
    SkyWayStreamFactory.createDataStream().then(data => {
        stream_data = data;
    });
}

export function writeStream(message: string) {
    stream_data.write(message);
}

export function requestToken() {
    sendMessage(requestTokenMethod);
}

export function recreateToken(token: string) {
    skyway_context.updateAuthToken(token);
}

export function changeJoin(mode: boolean) {
    console.log(mode);
    join_mode = mode;
}

export function closeRoom() {
    room.close();
}

(window as any).createHost = createHost;
(window as any).createClient = createClient;
(window as any).writeStream = writeStream;
(window as any).recreateToken = recreateToken;
(window as any).changeJoin = changeJoin;
(window as any).closeRoom = closeRoom;
(window as any).room = room;
