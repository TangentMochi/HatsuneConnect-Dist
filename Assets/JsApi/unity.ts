export function sendMessage(method: string, value?: number | string, gameObject = "Master") {
    var unityInstance = (window as any).gameInstance;
    if (!unityInstance) {
        console.error("unityInstance is null");
        return;
    }
    if (value == undefined) {
        unityInstance.SendMessage(gameObject, method);
    } else {
        unityInstance.SendMessage(gameObject, method, value);
    }
}
