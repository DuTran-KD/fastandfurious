(function () {
    "use strict";
    const chars = 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789';
    const ws_url = location.origin.replace(/^http/, 'ws');
    var player;
    var peer;
    var conn;
    var curPeer = 0;

    function getParameterByName(name, url) {
        if (!url) url = window.location.href;
        name = name.replace(/[\[\]]/g, '\\$&');
        let regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'), results = regex.exec(url);
        if (!results) return null;
        if (!results[2]) return '';
        return decodeURIComponent(results[2].replace(/\+/g, ' '));
    }

    function disableF5(e) { if ((e.which || e.keyCode) == 116 || (e.which || e.keyCode) == 82) e.preventDefault(); };

    function detectDevices(callback) {
        let md = navigator.mediaDevices;
        if (!md || !md.enumerateDevices) return callback(false);
        md.enumerateDevices().then(devices => {
            callback(devices.some(device => 'audioinput' === device.kind));
        })
    }

    function openAudio() {
        if (navigator.mediaDevices.getUserMedia) {
            const config = { audio: true, video: false };
            return navigator.mediaDevices.getUserMedia(config);
        } else {
            alert("Browser not support");
        }
    }

    function playStream(idAudioTag, stream) {
        var localStream = document.getElementById(idAudioTag);
        localStream.srcObject = stream;
        localStream.play();
    }

    function removeAttr(dom) {
        var DOM = document.getElementById(dom);
        DOM.removeAttribute("hidden");

    }

    function addAttr(dom) {
        var DOM = document.getElementById(dom);
        DOM.setAttribute("hidden", true);

    }

    function randomToken() {
        var res = '', i = 10, len = chars.length;
        while (i--) { res += chars[Math.floor(Math.random() * len)]; } return res;
    }

    function status(data, call, remoteAudio) {
        if (typeof data === 'string') {
            var decoded = JSON.parse(data);
            switch (decoded.type) {
                case "OPEN":
                    conn = call;
                    console.log("OPEN");
                    console.log(decoded.payload.msg);
                    break;
                case "NOTFOUND":
                    console.log(decoded.payload.msg);
                    addAttr(`streamVideo_${call.peer}`);
                    if (decoded.payload.flag) {
                        removeAttr(`remoteAudio_${call.peer}`);
                        playStream(`remoteAudio_${call.peer}`, remoteAudio);
                    } else {
                        alert("WB run");
                    }
                    break;
                case "ERROR":
                    conn = call;
                    console.log(decoded.payload.msg);
                    call.close();
                    break;
                default: break;
            }
        }
    }

    var course_id = getParameterByName('p');

    function makePeerHeartbeater(peer) {
        var timeoutId = 0;
        function heartbeat() {
            timeoutId = setTimeout(heartbeat, 20000);
            if (peer.socket._wsOpen()) {
                peer.socket.send({ type: 'HEARTBEAT' });
            }
        }
        // return
        return {
            start: function () {
                if (timeoutId === 0) { heartbeat(); }
            },
            stop: function () {
                clearTimeout(timeoutId);
                timeoutId = 0;
            }
        };
    }

    const initAjax = (call) => {
        var canvas = document.getElementById(`streamVideo_${call.peer}`);
        openAudio()
            .then(oAudio => {
                call.answer(oAudio);
                playStream(`localAudio_${call.peer}`, oAudio);
                call.on('stream', async remoteAudio => {
                    player = new JSMpeg.Player(`ws://www.bigprotech.vn:7000/stream?id=${course_id}${randomToken()}&course=${course_id}${call.peer}&type=tutor&token=${peer.options.token}`, { canvas: canvas });
                    setTimeout(function () {
                        var thisIs = player.source;
                        thisIs.socket.onmessage = function (evt) {
                            var data = evt.data;
                            var I = !thisIs.established;
                            thisIs.established = !0,
                                I && thisIs.onEstablishedCallback && thisIs.onEstablishedCallback(thisIs),
                                thisIs.destination && thisIs.destination.write(data)
                            status(data, call, remoteAudio);
                        }
                    }, 500);
                });
            })
            .catch(err => console.log(err));
        $(".stream_zoom").click(function () {
            $(this).toggleClass("show");
            $(this).children("canvas").toggleClass("zoom");
        });
    };

    function initialize() {

        peer = new Peer(course_id, {
            host: "peerjsserver7.herokuapp.com",
            port: "443",
            path: "/",
            secure: true
        });

        peer.on('open', id => makePeerHeartbeater(peer).start());

        //Answer
        peer.on('call', call => {
            curPeer++;
            console.log(call);
            console.log(peer);
            //create canvas
            $(document).ready(function () {
                var child_HD = `<div id="cam_${call.peer}" data-id="${curPeer}"><h3>
                                Hình ảnh
                                <span id="current_connect" class="circle__item circle__green"></span>
                                <span id="current_connect_text">  (Online) </span>
                            </h3>
                            <div class="stream_zoom mr-5">
                                <canvas id="streamVideo_${call.peer}"></canvas>
                            </div></div>`;
                var child_Audio = `<div id="audio_${call.peer}" data-id="${curPeer}"><audio id="localAudio_${call.peer}" autoplay></audio>
                 <video id="remoteAudio_${call.peer}" hidden autoplay></video></div>`;
                $("#container-cam").append(child_HD);
                $("#container-audio").append(child_Audio);
                initAjax(call);
            });
            call.on('close', () => {
                $(`#cam_${call.peer}`).remove();
                $(`#audio_${call.peer}`).remove();
                makePeerHeartbeater(peer).stop();
            });
        });
        peer.on('disconnected', function () {
            console.log('Connection lost. Please reconnect');
        });
        peer.on('close', function () {
            console.log(conn.peer + ' has left the room');
        });
        peer.on('error', function (err) {
            console.log('Err ' + err);
        });

        if (window.navigator.onLine) {
            console.log("online");
        } else {
            console.log("offline");
        }
    };

    $(document).ready(function () {
        $(document).on("keydown", disableF5);
    });

    window.onunload = window.onbeforeunload = function (e) {
        if (!!peer && !peer.destroyed) {
            conn.close();
            peer.destroy();
            player.stop();
        }
        e.preventDefault();
        return "Reload ?"
    };

    detectDevices(function (hasAudio) {
        if (hasAudio) {
            initialize();
        } else {
            alert("Vui lòng cắm headphone.");
        }
    });
})();