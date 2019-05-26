(function () {
    "use strict";
    const chars = 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789';
    const ws_url = location.origin.replace(/^http/, 'ws');
    let player = null;
    const stt = $("#stt").attr("data-id");
    const canvas = document.getElementById(`streamVideo`);

    function getParameterByName(name, url) {
        if (!url) url = window.location.href;
        name = name.replace(/[\[\]]/g, '\\$&');
        let regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'), results = regex.exec(url);
        if (!results) return null;
        if (!results[2]) return '';
        return decodeURIComponent(results[2].replace(/\+/g, ' '));
    }

    function disableF5(e) { if ((e.which || e.keyCode) == 116 || (e.which || e.keyCode) == 82) e.preventDefault(); };

    function randomToken() {
        var res = '', i = 10, len = chars.length;
        while (i--) { res += chars[Math.floor(Math.random() * len)]; } return res;
    }

    const course_id = getParameterByName('p');

    function getToken() {
        var http = new XMLHttpRequest();
        var url = 'https://peerjsserver7.herokuapp.com/peerjs/id';
        var queryString = "?ts=" + new Date().getTime() + "-" + course_id + "-" + Math.random();
        url += queryString;
        // If there's no ID we need to wait for one before trying to init socket.
        http.open("get", url, false);
        http.onerror = function (e) {
            http._abort(
                "server-error",
                "Could not get an ID from the server." + e
            );
        };
        http.onreadystatechange = function () {
            if (http.readyState !== 4) {
                return;
            }
            if (http.status !== 200) {
                http.onerror();
                return;
            }
            return http.responseText;
        };
        http.send(null);
        return http.onreadystatechange();
    }

    function initialize() {
        player = new JSMpeg.Player(`ws://www.bigprotech.vn:7000/stream?id=${randomToken()}&course=${course_id}${stt}&type=parent&token=${getToken()}`, { canvas: canvas });
    };

    $(document).ready(function () {
        $(document).on("keydown", disableF5);
    });

    window.onunload = window.onbeforeunload = function (e) {
        player.stop();
        e.preventDefault();
        return "Reload ?"
    };

    initialize();
})();