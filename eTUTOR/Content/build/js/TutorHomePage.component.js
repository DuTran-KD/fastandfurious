// duyet view infoOftutor
function duyetstudent(id) {
    //alert('!!id ' + id)
    var request = new XMLHttpRequest();
    var url = "/Tutor/Duyetkhoahoc/"+id;
    var method = "POST";
    request.open(method, url);
    request.send();

}