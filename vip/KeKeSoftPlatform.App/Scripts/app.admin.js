//页面入口实例化方法
$(function () {

	//设置页面的main区域
	pozhu.mainSectionSetting.init();

	//初始化底部导航
	pozhu.belowNav.init();
});


//页面底部导航
pozhu.belowNav = (function () {
	var navigation = null;
	var $mainBottom = $('#main-bottom');

	init = function () {
		$mainBottom.addClass("loaded");
		navigation = $mainBottom.find('.okayNav').okayNav();
	};

	return {
		init: init
	}

})();


//初始化页面main区域高度
pozhu.mainSectionSetting = (function () {
	var $navbar = $('#pz-navbar');
	var $mainTop = $('#main-top');
	var $mainBottom = $('#main-bottom');
	var $main = $('#main-section');

	//初始化底部导航
	init = function () {
		$main.height($(window).height() - $navbar.innerHeight() - $mainTop.innerHeight());
		if ($mainBottom.size() > 0 && $mainBottom.children().size() > 0) {
			$main.height($main.height() - $mainBottom.outerHeight());
		}

		//设定顶级.row高度，以用于自应用其他列的高度
		$main.closest(".row").height($main.height() + $mainTop.height() + $mainBottom.outerHeight());
	};


	//窗口尺寸发生变化时动作
	$(window).resize(function () {
		init();
	});

	return {
		init: init
	}
})();
