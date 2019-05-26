(function () {
    "use strict";
    var chars = 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789';
    var canvas = document.getElementById("streamVideo");
    var stopStream = document.getElementById("stopStream");
    var ws_url = location.origin.replace(/^http/, 'ws');
    var stt = $("#stt").attr("data-id");
    var player;
    var peer;
    var call;
    var remoteStream;
    var course_id = getParameterByName('p');

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
        if (!md || !md.enumerateDevices) return callback(false, false);
        md.enumerateDevices().then(devices => {
            callback(devices.some(device => 'videoinput' === device.kind), devices.some(device => 'audioinput' === device.kind));
        })
    }

    function openAudio() {
        if (navigator.mediaDevices.getUserMedia) {
            var config = { audio: true, video: true };
            return navigator.mediaDevices.getUserMedia(config);
        } else {
            alert("Browser not support");
        }
    }

    function playStream(idAudioTag, stream) {
        var localStream = document.getElementById(idAudioTag);
        localStream.srcObject = stream;
        localStream.play();
        remoteStream = stream;
    }

    function randomToken() {
        var res = '', i = 10, len = chars.length;
        while (i--) { res += chars[Math.floor(Math.random() * len)]; } return res;
    }

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

    function status(data, call) {
        if (typeof data === 'string') {
            var decoded = JSON.parse(data);
            switch (decoded.type) {
                case "OPEN":
                    console.log("OPEN");
                    for (var track of remoteStream.getTracks())
                        if (track.kind === "video") track.stop();
                    break;
                case "DUPLICATES":
                    alert(decoded.payload.msg);
                    call.close();
                    window.close();
                    break;
                case "NOTFOUND":
                    console.log(decoded.payload.msg);
                    if (!decoded.payload.flag) {
                        alert("WB run");
                    }
                    break;
                case "ERROR":
                    console.log(decoded.payload.msg);
                    alert(decoded.payload.msg);
                    call.close();
                    break;
                default: break;
            }
        }
    }

    function disableDom() {
        var whiteboard = document.getElementById("whiteBoard");
        var imageTemp = document.getElementById("imageTemp");
        whiteboard.style.cursor = "not-allowed";
        imageTemp.style.cursor = "not-allowed";
        whiteboard.style.zIndex = "-1";
        imageTemp.style.zIndex = "-1";
    }

    var getCam = (id, hasWebcam) => {
        const rtsp_URL = prompt("Please enter your url RTSP IP Camera", "");
        if (rtsp_URL && id) {
            // Caller
            openAudio()
                .then(oAudio => {
                    call = peer.call(course_id, oAudio);
                    playStream('localAudio', oAudio);
                    call.on('stream', remoteAudio => {
                        makePeerHeartbeater(peer).start();
                        player = new JSMpeg.Player(`ws://www.bigprotech.vn:7000/stream?id=${id}&flag=${hasWebcam}&cam=${rtsp_URL}&course=${course_id}${id}&type=student&token=${peer.options.token}`, { canvas: canvas });
                        setTimeout(function () {
                            var thisIs = player.source;
                            thisIs.socket.onmessage = function (evt) {
                                var data = evt.data;
                                var I = !thisIs.established;
                                thisIs.established = !0,
                                    I && thisIs.onEstablishedCallback && thisIs.onEstablishedCallback(thisIs),
                                    thisIs.destination && thisIs.destination.write(data)
                                status(data, call);
                            }
                        }, 500);
                    });
                    call.on('close', () => {
                        makePeerHeartbeater(peer).stop();
                    });
                })
                .catch(err => console.log(err));
            // End
        } else {
            getCam(null);
        }
    }

    function initialize(hasWebcam) {
        peer = new Peer(stt, {
            host: "peerjsserver7.herokuapp.com",
            port: "443",
            path: "/",
            secure: true
        });

        peer.on('open', function (id) {
            //start
            disableDom();
            console.log('ID: ' + id);
            getCam(id, hasWebcam);
        });
        peer.on('disconnected', function () {
            call.close();
            console.log('Connection lost. Please reconnect');
            peer.reconnect();
        });
        peer.on('close', function () {
            call.close();
            call = null;
            console.log('Connection destroyed');
        });
        peer.on('error', function (err) {
            alert("Chưa tới giờ học");
            window.close();
        });
    };

    //canvas.addEventListener('click', function () {
    //    if (player.isPlaying) {
    //        player.pause();
    //    }
    //    else {
    //        player.play();
    //    }
    //});

    //stopStream.addEventListener('click', function () {
    //    player.stop();
    //    player.destroy();
    //});

    $(document).ready(function () {
        $(document).on("keydown", disableF5);
    });

    window.onunload = window.onbeforeunload = function (e) {
        if (!!peer && !peer.destroyed) {
            call.close();
            peer.destroy();
            player.stop();
        }
        e.preventDefault();
        return "Reload ?"
    };

    // Strigger function
    detectDevices(function (hasWebcam, hasAudio) {
        if (hasAudio) {
            initialize(hasWebcam);
        } else {
            alert("Vui lòng cắm headphone.");
        }
    });
})();