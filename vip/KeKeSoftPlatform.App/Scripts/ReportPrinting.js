var isChrome = window.navigator.userAgent.match(/Chrome\/[\d][\d]/);
var isFirefox = navigator.userAgent.indexOf("Firefox") > 0
var isIE = (!!window.ActiveXObject || "ActiveXObject" in window);
var isOtherBrowser = (!isChrome && !isFirefox && !isIE);
function PrintPreview() {
    if (isIE || isFirefox) {
        var webBrowser = document.getElementById("WebBrowser");
        webBrowser.ExecWB(7, 1);
        return false;
    }
    var bdhtml = window.document.body.innerHTML;
    spstr = "<!--startprint-->";
    epstr = "<!--endprint-->";
    var prnhtml = bdhtml.substring(bdhtml.indexOf(spstr) + 17);
    prnhtml = prnhtml.substring(0, prnhtml.indexOf(epstr));
    window.document.body.innerHTML = prnhtml;
    window.print();
    document.body.innerHTML = bdhtml;
}
window.onload = function () {
    var arr = document.getElementsByTagName("table");
    $.each(arr, function () {
        $(this).css("border-collapse", "inherit");
        $(this).removeAttr("cellspacing");
    });
    if (isIE) {
        var factory = document.getElementById("factory");
        //factory.printing.paperSize = "A4";
        factory.printing.header = "";
        factory.printing.footer = "";
        factory.printing.portrait = false;
        //factory.printing.leftMargin = 0;
        factory.printing.topMargin = 5;
        //factory.printing.rightMargin = 5;
        //factory.printing.bottomMargin = 5;
        //factory.DoPrint(true); //设置为false，直接打印 
    }
}
