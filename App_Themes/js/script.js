$(document).ready(function(){

	// ------------------------------
	// popup click handler
	
	$('#popup-open').click(function(){
		$('#popup-wrapper').fadeIn(200);
		$('#main_content').fadeOut(200);
	});

	// ------------------------------
	// close click handler
	
	$('#popup-close').click(function(){
		$('#popup-wrapper').fadeOut(200);
		$('#main_content').fadeIn(200);
	});

});
