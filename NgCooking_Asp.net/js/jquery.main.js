$(function(){
	$("[data-popin-open]").click(function(){
		var popin = $(this).attr("data-popin-open");
		$("[data-popin='"+popin+"']").addClass("displayed");
	});
	$("[data-popin] .popin-backlayer, [data-popin] .popin-close").click(function(){
		$(this).parents(".popin").removeClass("displayed");
	});
	$("#mobile-menu-btn, #mobile-close-btn, #mobile-back-layer").click(function(){
		$("body").toggleClass("mobile-menu-open");
	});
});