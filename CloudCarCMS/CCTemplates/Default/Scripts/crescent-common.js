$(document).ready(function() {

    $('.contact-button').click(function () {
        $("html, body").animate({ scrollTop: 0 }, "slow", function () {
            $("#contact-overlay").fadeIn("slow");
            $("#contact-form").fadeIn("slow");
        });
    });
    $("#CancelContactButton").click(function () {
        $("#contact-overlay").fadeOut("slow");
        $("#contact-form").fadeOut("slow");
    });
    
/* NEW INCLUDES STARTS HERE */
	
	/* Ajax Cart initiates the load plus slideDown event */
	$('#cart').hover(
		function() {
			$('#cart .content').slideDown(700);
			
			$.ajax({
				url: 'index.php?route=module/cart',
				dataType: 'load',
				success: function(load) {
					if (load['output']) {
						$('#cart > *').html(load['output']);
					}
				}
			});
		},
		
		function () {
			$('#cart .content').hide();
		}	
	);
	
	/* Ajax Cart maintains the load */
	$('#cart > .heading a').live('mouseover', function() {
		$('#cart').addClass('active');
		
		$('#cart').load('index.php?route=module/cart #cart > *');
		
		$('#cart').live('mouseleave', function() {
			$(this).removeClass('active');
		});
	});
	
	// Responsive Menu
	$(function() {
		var pull 		= $('#pull');
			menu 		= $('nav ul');
			menuHeight	= menu.height();

		$(pull).on('click', function(e) {
			e.preventDefault();
			menu.slideToggle();
		});

		$(window).resize(function(){
			var w = $(window).width();
			if(w > 320 && menu.is(':hidden')) {
				menu.removeAttr('style');
			}
		});
	});
	
	// Responsive Top Nav
	$(document).ready(function(){
		$('#account-trigger').click(function(){
			$(this).next('#account-content').slideToggle();
			$(this).toggleClass('active');					
			
			if ($(this).hasClass('active')) $(this).find('span').html('')
				else $(this).find('span').html('')
		})

		$('#links-trigger').click(function(){
			$(this).next('#links-content').slideToggle();
			$(this).toggleClass('active');					
			
			if ($(this).hasClass('active')) $(this).find('span').html('')
				else $(this).find('span').html('')
		})
	});

	/* Poshytip */
	$('.poshytip').poshytip({
    	className: 'tip-twitter',
		showTimeout: 1,
		alignTo: 'target',
		alignX: 'center',
		offsetY: 5,
		allowTipHover: false
    });
	
	/* Menu DARK hover animations */
		$('#menu > ul > li').hover(function() {
			$("#menu li").removeClass("active");
			$("#menu li div").css("display", "none");

		}); 
		$("#menu > ul > li").hover(function () {
			$(this).children('div').slideDown(300, function() {
			});
		},
		function () {
			$(this).children('div').slideUp('medium', function() {
			});  
		}); 

		/* Sub-category hover animations */
		$("#menu > ul > li > div > ul > li").hover(function () {
			$(this).children('div').slideDown(300, function() {
			});
		},
		function () {
			$(this).children('div').slideUp('medium', function() {
			});  
		});  		
		
	/* NEW INCLUDES ENDS HERE */
	
	/* Search */
	$('.button-search').bind('click', function() {
		url = $('base').attr('href') + 'index.php?route=product/search';
				 
		var filter_name = $('input[name=\'filter_name\']').attr('value');
		
		if (filter_name) {
			url += '&filter_name=' + encodeURIComponent(filter_name);
		}
		
		location = url;
	});
	
	$('#header input[name=\'filter_name\']').bind('keydown', function(e) {
		if (e.keyCode == 13) {
			url = $('base').attr('href') + 'index.php?route=product/search';
			 
			var filter_name = $('input[name=\'filter_name\']').attr('value');
			
			if (filter_name) {
				url += '&filter_name=' + encodeURIComponent(filter_name);
			}
			
			location = url;
		}
	});
	
	/* Ajax Cart 
	$('#cart > .heading a').live('click', function() {
		$('#cart').addClass('active');
		
		$('#cart').load('index.php?route=module/cart #cart > *');
		
		$('#cart').live('mouseleave', function() {
			$(this).removeClass('active');
		});
	});
	*/ 
	
	if ($.browser.msie && $.browser.version < 9) {
	} else {
		/* Box product hover animations */
		$('.box-product > div').hover(
			function(){
				$(this).siblings().stop().animate({opacity:0.6}, 600);
			},
			function(){
				$(this).siblings().stop().animate({opacity:1}, 600);
			}
		);
		
		/* Product grid hover animations */
		$('.product-grid > div, .product-list > div').hover(
			function(){
				$(this).siblings().stop().animate({opacity:0.6}, 600);
			},
			function(){
				$(this).siblings().stop().animate({opacity:1}, 600);
			}
		);
	}
	
	/* Mega Menu DARK */
	$('#menu ul > li > a + div').each(function(index, element) {
		// IE6 & IE7 Fixes
		if ($.browser.msie && ($.browser.version == 7 || $.browser.version == 6)) {
			var category = $(element).find('a');
			var columns = $(element).find('ul').length;
			
			$(element).css('width', (columns * 143) + 'px');
			$(element).find('ul').css('float', 'left');
		}		
		
		var menu = $('#menu').offset();
		var dropdown = $(this).parent().offset();
		
		i = (dropdown.left + $(this).outerWidth()) - (menu.left + $('#menu').outerWidth());
		
		if (i > 0) {
			$(this).css('margin-left', '-' + (i + 5) + 'px');
		}
	});
	
	/* Mega Menu LIGHT */
	$('#menu-light ul > li > a + div').each(function(index, element) {
		// IE6 & IE7 Fixes
		if ($.browser.msie && ($.browser.version == 7 || $.browser.version == 6)) {
			var category = $(element).find('a');
			var columns = $(element).find('ul').length;
			
			$(element).css('width', (columns * 143) + 'px');
			$(element).find('ul').css('float', 'left');
		}		
		
		var menu = $('#menu-light').offset();
		var dropdown = $(this).parent().offset();
		
		i = (dropdown.left + $(this).outerWidth()) - (menu.left + $('#menu-light').outerWidth());
		
		if (i > 0) {
			$(this).css('margin-left', '-' + (i + 5) + 'px');
		}
	});

	// IE6 & IE7 Fixes
	if ($.browser.msie) {
		if ($.browser.version <= 6) {
			$('#column-left + #column-right + #content, #column-left + #content').css('margin-left', '195px');
			
			$('#column-right + #content').css('margin-right', '195px');
		
			$('.box-category ul li a.active + ul').css('display', 'block');	
		}
		
		if ($.browser.version <= 7) {
			$('#menu > ul > li').bind('mouseover', function() {
				$(this).addClass('active');
			});
				
			$('#menu > ul > li').bind('mouseout', function() {
				$(this).removeClass('active');
			});	
		}
		
		if ($.browser.version <= 7) {
			$('#menu-light > ul > li').bind('mouseover', function() {
				$(this).addClass('active');
			});
				
			$('#menu-light > ul > li').bind('mouseout', function() {
				$(this).removeClass('active');
			});	
		}
	}
	
	$('.success img, .warning img, .attention img, .information img').live('click', function() {
		$(this).parent().fadeOut('slow', function() {
			$(this).remove();
		});
	});	
});

function getURLVar(urlVarName) {
	var urlHalves = String(document.location).toLowerCase().split('?');
	var urlVarValue = '';
	
	if (urlHalves[1]) {
		var urlVars = urlHalves[1].split('&');

		for (var i = 0; i <= (urlVars.length); i++) {
			if (urlVars[i]) {
				var urlVarPair = urlVars[i].split('=');
				
				if (urlVarPair[0] && urlVarPair[0] == urlVarName.toLowerCase()) {
					urlVarValue = urlVarPair[1];
				}
			}
		}
	}
	
	return urlVarValue;
} 

function addToCart(product_id, quantity) {
	quantity = typeof(quantity) != 'undefined' ? quantity : 1;

	$.ajax({
		url: 'index.php?route=checkout/cart/add',
		type: 'post',
		data: 'product_id=' + product_id + '&quantity=' + quantity,
		dataType: 'json',
		success: function(json) {
			$('.success, .warning, .attention, .information, .error').remove();
			
			if (json['redirect']) {
				location = json['redirect'];
			}
			
			if (json['success']) {
				$('#notification').html('<div class="success" style="display: none;">' + json['success'] + '<img src="catalog/view/theme/crescent/image/close.png" alt="" class="close" /></div>');
				
				$('.success').fadeIn('slow');
				
				$('#cart-total').html(json['total']);
				
				$('html, body').animate({ scrollTop: 0 }, 'slow'); 
			}	
		}
	});
}
function addToWishList(product_id) {
	$.ajax({
		url: 'index.php?route=account/wishlist/add',
		type: 'post',
		data: 'product_id=' + product_id,
		dataType: 'json',
		success: function(json) {
			$('.success, .warning, .attention, .information').remove();
						
			if (json['success']) {
				$('#notification').html('<div class="success" style="display: none;">' + json['success'] + '<img src="catalog/view/theme/crescent/image/close.png" alt="" class="close" /></div>');
				
				$('.success').fadeIn('slow');
				
				$('#wishlist-total').html(json['total']);
				
				$('html, body').animate({ scrollTop: 0 }, 'slow');
			}	
		}
	});
}

function addToCompare(product_id) { 
	$.ajax({
		url: 'index.php?route=product/compare/add',
		type: 'post',
		data: 'product_id=' + product_id,
		dataType: 'json',
		success: function(json) {
			$('.success, .warning, .attention, .information').remove();
						
			if (json['success']) {
				$('#notification').html('<div class="success" style="display: none;">' + json['success'] + '<img src="catalog/view/theme/crescent/image/close.png" alt="" class="close" /></div>');
				
				$('.success').fadeIn('slow');
				
				$('#compare-total').html(json['total']);
				
				$('html, body').animate({ scrollTop: 0 }, 'slow'); 
			}	
		}
	});
}