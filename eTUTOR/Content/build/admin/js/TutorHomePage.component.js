//tat nhan day kem
function duyetkhoahocre(id) {
    var request = new XMLHttpRequest();
    var url = "~/Tutor/registScheduleManager/" + id;
    var method = "POST";
    request.open(method, url);
    request.send();
    alert('id= ' + id);
}

// duyet khóa học của tutor
function duyetkhoahocTutor(id) {
    var request = new XMLHttpRequest();
    var url = "/Tutor/Duyetkhoahoc/" + id;
    var method = "POST";
    request.open(method, url);
    request.send();
}

// duyet view course
function duyetkhoahoc(id) {
    //alert('!!id ' + id)
    var request = new XMLHttpRequest();
    var url = "/Admin/Admin/Duyetkhoahoc/"+id;
    var method = "POST";
    request.open(method, url);
    request.send();
}
// duyet view schedule
function duyetschedule(id) {
    //alert('!!id ' + id)
    var request = new XMLHttpRequest();
    var url = "/Admin/Admin/Duyetschedule/" + id;
    var method = "POST";
    request.open(method, url);
    request.send();
}
// duyet view user bang tutor
function duyettutor(id) {
    //alert('!!id ' + id)
    var request = new XMLHttpRequest();
    var url = "/Admin/Admin/Duyettutor/" + id;
    var method = "POST";
    request.open(method, url);
    request.send();
}
// duyet view user bang parent
function duyetparent(id) {
    //alert('!!id ' + id)
    var request = new XMLHttpRequest();
    var url = "/Admin/Admin/Duyetparent/" + id;
    var method = "POST";
    request.open(method, url);
    request.send();
}
// duyet view user bang student
function duyetstudent(id) {
    //alert('!!id ' + id)
    var request = new XMLHttpRequest();
    var url = "/Admin/Admin/Duyetstudent/" + id;
    var method = "POST";
    request.open(method, url);
    request.send();
}
//KHOA
function khoatutor(id) {
    //alert('!!id ' + id)
    var request = new XMLHttpRequest();
    var url = "/Admin/Admin/Khoatutor/" + id;
    var method = "POST";
    request.open(method, url);
    request.send();
}

function khoaparent(id) {
    //alert('!!id ' + id)
    var request = new XMLHttpRequest();
    var url = "/Admin/Admin/Khoaparent/" + id;
    var method = "POST";
    request.open(method, url);
    request.send();
}

function khoastudent(id) {
    //alert('!!id ' + id)
    var request = new XMLHttpRequest();
    var url = "/Admin/Admin/Khoastudent/" + id;
    var method = "POST";
    request.open(method, url);
    request.send();
}