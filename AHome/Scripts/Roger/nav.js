
function setph(n) {
    var tli = document.getElementById("ph1").getElementsByTagName("a");
    var mli = document.getElementById("ph2").getElementsByTagName("ul");
    for (i = 0; i < tli.length; i++) {
        tli[i].className = i == n ? "ph_hover" : "";
        mli[i].style.display = i == n ? "block" : "none";
    }
}