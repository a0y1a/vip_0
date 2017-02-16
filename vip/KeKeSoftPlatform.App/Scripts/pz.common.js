
// 声明一个全局对象Namespace，用来注册命名空间
Namespace = new Object();
// 全局对象仅仅存在register函数，参数为名称空间全路径，如"pozhu"
Namespace.register = function (fullNS) {
	// 将命名空间切成N部分, 比如pozhu、me等
	var nsArray = fullNS.split('.');
	var sEval = "";
	var sNS = "";
	for (var i = 0; i < nsArray.length; i++) {
		if (i != 0) sNS += ".";
		sNS += nsArray[i];
		// 依次创建构造命名空间对象（假如不存在的话）的语句
		// 比如先创建pozhu，然后创建pozhu.gk，依次下去
		sEval += "if (typeof(" + sNS + ") == 'undefined') " + sNS + " = new Object();"
	}
	if (sEval != "") eval(sEval);
}
Namespace.register("pozhu");
Namespace.register("pozhu.apps");

/* 解决windows phone 8 中媒体查询的BUG */
if (navigator.userAgent.match(/IEMobile\/10\.0/)) {
	var msViewportStyle = document.createElement("style")
	msViewportStyle.appendChild(
	  document.createTextNode(
		"@-ms-viewport{width:auto!important}"
	  )
	)
	document.getElementsByTagName("head")[0].appendChild(msViewportStyle)
}
