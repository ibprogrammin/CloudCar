var ADAPT_CONFIG = {
    path: '/CCTemplates/Default/Styles/',
    dynamic: true,
    range: [
      '0px    to 760px  = main-mobile.css',
      '760px  to 980px  = main-720.css',
      '980px  to 1280px = main-960.css',
      '1280px to 1600px = main-1200.css',
      '1600px to 1900px = main-1600.css',
      '1901px           = main-1600.css'
    ]
};

jQuery.fn.extend({
    countMenu: function (args) {
        var hiddenControlsHtml = "";
        var elementName = '.' + this.attr('class');
        var totalElements = $(elementName + ' nav a').length;
        
        $(elementName + ' nav a').each(function () {
            if ($(this).index() + 1 >= args.count) {
                if (($(this).index() + 1) == args.count) {
                    if (args.count != totalElements) {
                        hiddenControlsHtml += $(this).clone().wrap('<p>').parent().html();
                        $(this).css("display", "none");
                    }
                } else {
                    hiddenControlsHtml += $(this).clone().wrap('<p>').parent().html();
                    $(this).css("display", "none");
                }
            };
        });
        
        if (totalElements > args.count) {
            $(elementName + ' nav').html($(elementName + ' nav').html() + '<a href="#" class="more-link">' + args.moreText + '</a>');
        }
        $(elementName + ' nav .more-link').click(function () {
            if ($(elementName + ' div.hidden-links').css("display") == "none") {
                $(elementName + ' div.hidden-links').show(args.transitionDuration);
            } else {
                $(elementName + ' div.hidden-links').hide(args.transitionDuration);
            }
        });
        
        $(elementName + ' div.hidden-links').mouseleave(function () { $(this).hide(args.transitionDuration); });
        $(elementName + ' div.hidden-links').html('<nav>' + hiddenControlsHtml + '</nav>');
        $(elementName + ' div.hidden-links').delay(args.transitionDuration).hide(args.transitionDuration);
    }
});

jQuery(document).ready(function ($) {
    if ($('.store-navigation').exists()) {
        $('.store-navigation').countMenu({"count": 6, "transitionDuration": 1000, "moreText": "More >>"});
    }

    if ($('#slider').exists()) {
        $.getScript('/CCTemplates/Default/Scripts/NivoSlider/jquery.nivo.slider.pack.js', function (xhr) {
            try {
                loadNivoSlider('#slider');
            } catch (err) {
                eval(xhr);
                loadNivoSlider('#slider');
            }
        });
    }

    if ($('#accordion').exists()) {
        $("#accordion").slideUp("slow");

        $("#toggle").on("click", function() {
            if ($("#accordion").is(":hidden")) {
                $('html, body').animate({
                    scrollTop: $("#toggle").offset().top - 80
                }, 2000);
                $("#accordion").slideDown("slow");
            } else {
                $("#accordion").slideUp("slow");
            }

            return false;
        });
    }

    if ($('#multi-zoom').exists()) {
        $.getScript('/CCTemplates/Default/Scripts/jMultiZoom/multizoom.js', function (xhr) {
            try {
                loadMultiZoom('#multi-zoom');
            } catch (err) {
                eval(xhr);
                loadMultiZoom('#multi-zoom');
            }
        });
    }

    if ($('[class^=image-zoom-]').exists()) {
        $.getScript('/CCTemplates/Default/Scripts/jLensView/jlensview.js', function (xhr) {
            try {
                loadLensView('[class^=image-zoom-]');
            } catch (err) {
                eval(xhr);
                loadLensView('[class^=image-zoom-]');
            }
        });
    }

    if ($('.tab-container').exists()) {
        $.getScript('/CCTemplates/Default/Scripts/EasyTabs/jquery.easytabs.min.js', function (xhr) {
            try {
                loadEasyTabs('.tab-container');
            } catch (err) {
                eval(xhr);
                loadEasyTabs('.tab-container');
            }
        });
    }

    if ($('.load-image').exists()) {
        preloader($('.load-image').attr('data-preload-image'));
    }

    if ($('#JShowOffRotator').exists()) {
        $.getScript('/CCTemplates/Default/Scripts/jShowOff/jquery.jshowoff.min.js', function (xhr) {
            try {
                loadJShowOff('#JShowOffRotator');
            } catch (err) {
                eval(xhr);
                loadJShowOff('#JShowOffRotator');
            }
        });
    }

    if ($('.Qtip').exists()) {
        $.getScript('/CCTemplates/Default/Scripts/qTip/jquery.qtip-1.0.0-rc3.min.js', function (xhr) {
            try {
                loadQTip('.Qtip');
            } catch (err) {
                eval(xhr);
                loadQTip('.Qtip');
            }
        });
    }

    $("#MenuControl").mouseenter(function () {
        $("#MenuLinks").show().stop().animate({ "height": "230" }, "slow", function () {
            $("#MenuLinks").css({ 'overflow': 'visible' });
        });
    });
    $("#MenuControl").mouseleave(function () {
        $("#MenuLinks").stop().animate({ "height": "0" }, "slow", function () {
            $(this).hide();
        });
    });

    $("#MenuLinks li").mouseenter(function () {
        $(this).find("ul").show().stop().animate({ "height": "260" }, "slow");
    });

    $("#MenuLinks li").mouseleave(function () {
        $(this).find("ul").stop().animate({ "height": "0" }, "slow", function () {
            $(this).hide();
        });
    });


    $('.contact-button').click(function() {
        $("html, body").animate({ scrollTop: 0 }, "slow", function () {
            $("#contact-overlay").fadeIn("slow");
            $("#contact-form").fadeIn("slow");
        });
    });
    $("#CancelContactButton").click(function () {
        $("#contact-overlay").fadeOut("slow");
        $("#contact-form").fadeOut("slow");
    });

    jQuery(window).bind('scroll', function () {
        if ($(window).scrollTop() > 260) {
            $('.top-pop-menu').addClass('top-menu-fixed').css({ 'display': 'block' }).stop().animate({ 'height': '45' }, 'slow');
        }
        else {
            $('.top-pop-menu').stop().animate({ 'height': '0' }, 'slow', function () {
                $(this).css({ 'display': 'none' }).removeClass('top-menu-fixed'); ;
            });
        }
    });

    $('.Hover').hover(function () {
        var fade = $('#PortfolioNextButton');

        if (fade.is(':animated')) {
            fade.stop().fadeTo(500, 1);
        } else {
            fade.fadeOut(0);
            fade.fadeIn(500);
        }
    }, function () {
        var fade = $('#PortfolioNextButton');
        if (fade.is(':animated')) {
            fade.stop().fadeTo(500, 1);
        } else {
            fade.fadeOut(0);
            fade.fadeIn(500);
        }
    });

    $(".Fade").hover(
			function () {
			    var show = $('> div', this);
			    show.stop().animate({ "opacity": "1" }, "slow").animate({ "height": "420" }, "fast").css({ 'z-index': '9' });
			},
			function () {
			    var show = $('> div', this);
			    show.stop().animate({ "opacity": "0" }, "slow").animate({ "height": "0" }, "slow").css({ 'z-index': '1' });
			});

    $("#WatchThis").hover(
			function () {
			    $(this).stop().animate({ "width": "520" }, "slow");
			},
			function () {
			    var show = $('> div', this);
			    $(this).stop().animate({ "width": "40" }, "slow");
			});

    $(".portfolio-next-button").hover(
			function () {
			    $(this).stop().animate({ "opacity": "1" }, "fast");
			},
			function () {
			    $(this).stop().animate({ "opacity": "0.7" }, "slow");
			});

    $('tr.parent')
		.css("cursor", "pointer")
		.attr("title", "Click to expand/collapse")
		.click(function () {
		    $(this).siblings('tr.child-' + this.id).toggle('slow');
		});

    $('tr[class^=child-]').hide().children('td');

    $('.SelectContent').click(function () { $(this).next().toggle("slow"); return false; }).next().hide();
    $('.Accordion h2').click(function () { $(this).next().toggle("slow"); return false; }).next().hide();

    //scrollMore();

    if ($('#switcher-panel').exists()) {
        jcps.fader(300, '#switcher-panel');
    }

    parallax();
    //fixHeader();
});

jQuery.fn.exists = function () { return this.length > 0; };

if (typeof (Sys.Browser.WebKit) == "undefined") {
    Sys.Browser.WebKit = {};
}
if (navigator.userAgent.indexOf("WebKit/") > -1) {
    Sys.Browser.agent = Sys.Browser.WebKit;
    Sys.Browser.version = parseFloat(navigator.userAgent.match(/WebKit\/(\d+(\.\d+)?)/)[1]);
    Sys.Browser.name = "WebKit";
}

function parallax() {
    var scrolled = $(window).scrollTop();
    $('.bg').css('top', -(scrolled * 0.2) + 'px');
}

function fixHeader() {
    var stickyHeaderTop = $('header').offset().top;

    $(window).scroll(function () {
        if ($(window).scrollTop() > stickyHeaderTop) {
            $('header').css({ position: 'fixed', top: '0px' });
            $('#stickyalias').css('display', 'block');
        } else {
            $('header').css({ position: 'static', top: '0px' });
            $('#stickyalias').css('display', 'none');
        }
    });
}

function rotateImage(element, src) {
    element.src = src;
}

function preloader(src) {
    var imageObj = new Image();
    imageObj.src = src;
}

/*
var xPos, yPos;
var prm = Sys.WebForms.PageRequestManager.getInstance();

function BeginRequestHandler(sender, args) {
    if (!$get('.BodyWrapper') == null) {
        xPos = $get('.BodyWrapper').scrollLeft;
        yPos = $get('.BodyWrapper').scrollTop;
    }
}
function EndRequestHandler(sender, args) {
    if (!$get('.BodyWrapper') == null) {
        $get('.BodyWrapper').scrollLeft = xPos;
        $get('.BodyWrapper').scrollTop = yPos;
    }
}

prm.add_beginRequest(BeginRequestHandler);
prm.add_endRequest(EndRequestHandler);
*/

function EndRequestHandler(sender, args) {
    if (Sys.Extended.UI.TextBoxWrapper._originalValidatorGetValue.toString() == Sys.Extended.UI.TextBoxWrapper.validatorGetValue.toString()) {

        Sys.Extended.UI.TextBoxWrapper._originalValidatorGetValue = function (id) {
            var control;
            control = document.getElementById(id);
            if (typeof (control.value) == "string") {
                return control.value;
            }
            return ValidatorGetValueRecursive(control);
        }

        return true;
    }
}

function UnloadHandler() {
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    
    return true
}



/*
var galleries = $('.ad-gallery').adGallery({
    //loader_image: 'loader.gif',
    width: 840, height: 400, thumb_opacity: 0.7, start_at_index: 0, description_wrapper: false, 
    animate_first_image: false, animation_speed: 400, display_next_and_prev: false, 
    display_back_and_forward: false, scroll_jump: 0, 
    slideshow: { enable: true, autostart: true, speed: 5000, start_label: 'Start', stop_label: 'Stop',
        stop_on_scroll: true, countdown_prefix: '(', countdown_sufix: ')',
        onStart: function() {}, onStop: function() {}
    },
    effect: 'slide-hori', enable_keyboard_move: true, cycle: true, 
    callbacks: {
        init: function() { this.preloadAll(); },
        afterImageVisible: false,
        beforeImageVisible: false
    }
});*/

function scrollMore() {
    jQuery("body").prepend('<span id="ScrollMore"><h3>Scroll for More</h3></span>');

    if (jQuery('#ScrollMore').css('position') == 'absolute') {
        jQuery('#ScrollMore').hide();
    }
    else {
        jQuery(window).scroll(function() {
            var opacity = 1.3 - (jQuery(window).scrollTop() / 1000);
            if (opacity > 1) opacity = 1;
            if (opacity < 0) opacity = 0;
            jQuery('#ScrollMore').fadeTo(75, opacity);
        });

        jQuery('#ScrollMore').click(function() {
            jQuery('html,body').animate({ scrollTop: jQuery(window).scrollTop() + 400 + 'px' }, 300);
        }).hover(function() {
            jQuery(this).fadeTo(200, 1);
        });
    }
}

function loadMultiZoom($) {
    jQuery($).addimagezoom({
        descArea: '#description',
        speed: 1500,
        descpos: false,
        imagevertcenter: false,
        magvertcenter: true,
        zoomrange: [3, 10],
        magnifiersize: [250, 250],
        magnifierpos: 'right',
        cursorshadecolor: '#fdffd5',
        cursorshade: true
    });
}

function loadLensView($) {
    jQuery($).lensView({
        srcInRel: true,
        size: 200
    });
}

function loadNivoSlider($) {
    jQuery($).nivoSlider({
        effect: 'boxRandom', // Specify sets like: 'fold,fade,sliceDown'
        slices: 15, // For slice animations
        boxCols: 8, // For box animations
        boxRows: 4, // For box animations
        animSpeed: 1500, // Slide transition speed
        pauseTime: 7000, // How long each slide will show
        startSlide: 0, // Set starting Slide (0 index)
        directionNav: true, // Next & Prev navigation
        directionNavHide: true, // Only show on hover
        controlNav: false, // 1,2,3... navigation
        controlNavThumbs: false, // Use thumbnails for Control Nav
        keyboardNav: true, // Use left & right arrows
        pauseOnHover: true, // Stop animation while hovering
        manualAdvance: false, // Force manual transitions
        captionOpacity: 0.0, // Universal caption opacity
        prevText: 'Prev', // Prev directionNav text
        nextText: 'Next', // Next directionNav text
        beforeChange: function () { }, // Triggers before a slide transition
        afterChange: function () { }, // Triggers after a slide transition
        slideshowEnd: function () { }, // Triggers after all slides have been shown
        lastSlide: function () { }, // Triggers when last slide is shown
        afterLoad: function () { } // Triggers when slider has loaded
    });
}

function loadQTip($) {
    jQuery($).mouseover(function () {
        // Destroy currrent tooltip if present
        if (jQuery(this).data("qtip")) jQuery(this).qtip("destroy");

        jQuery(this).qtip({
            content: {
                text: jQuery(this).find("div").html(),
                title: { text: jQuery(this).attr("title") }
            }, // Set the tooltip content to the current corner
            position: {
                corner: {
                    tooltip: 'topMiddle', // Use the corner...
                    target: 'bottomMiddle' // ...and opposite corner
                }
            },
            show: {
                when: false, // Don't specify a show event
                ready: true // Show the tooltip when ready
            },
            hide: "mouseout", // Don't specify a hide event
            style: {
                border: {
                    width: 1,
                    radius: 2
                },
                width: 520,
                padding: 10,
                textAlign: 'left',
                tip: true, // Give it a speech bubble tip with automatic corner detection
                name: 'green' // Style it according to the preset 'cream' style
            }
        });

        i++; // Increase the counter
    });
}

function loadJShowOff($) {
    jQuery($).jshowoff({
        controls: true,
        controlText: { previous: '', next: '' },
        effect: 'fade',
        changeSpeed: 3000,
        speed: 7500,
        links: false
    });
    jQuery($).css("display", "block");
}

function loadEasyTabs($) {
    jQuery($).easytabs({
        collapsible: false,
        animate: true,
        remote: true,
        selected: -1
    });
}

/*Adapt.js*/
(function (w, d, config, undefined) {
    // If no config, exit.
    if (!config) {
        return;
    }

    // Empty vars to use later.
    var url, url_old, timer;

    // Alias config values.
    var callback = config.callback || function () { };
    var path = config.path ? config.path : '';
    var range = config.range;
    var range_len = range.length;

    // Create empty link tag:
    // <link rel="stylesheet" />
    var css = d.createElement('link');
    css.rel = 'stylesheet';
    css.media = 'screen';

    // Called from within adapt().
    function change(i, width) {
        // Set the URL.
        css.href = url;
        url_old = url;

        // Fire callback.
        callback(i, width);
    }

    // Adapt to width.
    function adapt() {
        // This clearTimeout is for IE.
        // Really it belongs in react(),
        // but doesn't do any harm here.
        clearTimeout(timer);

        // Parse viewport width.
        var width = d.documentElement ? d.documentElement.clientWidth : 0;

        // While loop vars.
        var arr, arr_0, val_1, val_2, is_range, file;

        // How many ranges?
        var i = range_len;
        var last = range_len - 1;

        // Start with blank URL.
        url = '';

        while (i--) {
            // Turn string into array.
            arr = range[i].split('=');

            // Width is to the left of "=".
            arr_0 = arr[0];

            // File name is to the right of "=".
            // Presuppoes a file with no spaces.
            // If no file specified, make empty.
            file = arr[1] ? arr[1].replace(/\s/g, '') : undefined;

            // Assume max if "to" isn't present.
            is_range = arr_0.match('to');

            // If it's a range, split left/right sides of "to",
            // and then convert each one into numerical values.
            // If it's not a range, turn maximum into a number.
            val_1 = is_range ? parseInt(arr_0.split('to')[0], 10) : parseInt(arr_0, 10);
            val_2 = is_range ? parseInt(arr_0.split('to')[1], 10) : undefined;

            // Check for maxiumum or range.
            if ((!val_2 && i === last && width > val_1) || (width > val_1 && width <= val_2)) {
                // Build full URL to CSS file.
                file && (url = path + file);

                // Exit the while loop. No need to continue
                // if we've already found a matching range.
                break;
            }
        }

        // Was it created yet?
        if (!url_old) {
            // Apply changes.
            change(i, width);

            // Add the CSS, only if path is defined.
            // Use faster document.head if possible.
            path && (d.head || d.getElementsByTagName('head')[0]).appendChild(css);
        }
        else if (url_old !== url) {
            // Apply changes.
            change(i, width);
        }
    }

    // Fire off once.
    adapt();

    // Slight delay.
    function react() {
        // Clear the timer as window resize fires,
        // so that it only calls adapt() when the
        // user has finished resizing the window.
        clearTimeout(timer);

        // Start the timer countdown.
        timer = setTimeout(adapt, 16);
        // -----------------------^^
        // Note: 15.6 milliseconds is lowest "safe"
        // duration for setTimeout and setInterval.
        //
        // http://www.nczonline.net/blog/2011/12/14/timer-resolution-in-browsers
    }

    // Do we want to watch for
    // resize and device tilt?
    if (config.dynamic) {
        // Event listener for window resize,
        // also triggered by phone rotation.
        if (w.addEventListener) {
            // Good browsers.
            w.addEventListener('resize', react, false);
        }
        else if (w.attachEvent) {
            // Legacy IE support.
            w.attachEvent('onresize', react);
        }
        else {
            // Old-school fallback.
            w.onresize = react;
        }
    }

    // Pass in window, document, config, undefined.
})(this, this.document, ADAPT_CONFIG);

(function (a, b, c, d) { function e() { clearTimeout(i); for (var c = a.innerWidth || b.documentElement.clientWidth || b.body.clientWidth || 0, e, f, o, p, q = m, u = m - 1; q--; ) { e = l[q].split("="), f = e[0], p = e[1] ? e[1].replace(/\s/g, "") : q, e = (o = f.match("to")) ? parseInt(f.split("to")[0], 10) : parseInt(f, 10), f = o ? parseInt(f.split("to")[1], 10) : d; if (!f && q === u && c > e || c > e && c <= f) { g = k + p; break } g = "" } h ? h !== g && (h = n.href = g, j && j(q, c)) : (h = n.href = g, j && j(q, c), k && (b.head || b.getElementsByTagName("head")[0]).appendChild(n)) } function f() { clearTimeout(i), i = setTimeout(e, 100) } if (c) { var g, h, i, j = typeof c.callback == "function" ? c.callback : d, k = c.path ? c.path : "", l = c.range, m = l.length, n = b.createElement("link"); n.rel = "stylesheet", e(), c.dynamic && (a.addEventListener ? a.addEventListener("resize", f, !1) : a.attachEvent ? a.attachEvent("onresize", f) : a.onresize = f) } })(this, this.document, ADAPT_CONFIG)

jQuery.loadScript = function (url, arg1, arg2) {
  var cache = false, callback = null;
  if ($.isFunction(arg1)){
    callback = arg1;
    cache = arg2 || cache;
  } else {
    cache = arg1 || cache;
    callback = arg2 || callback;
  }
               
  var load = true;
  jQuery('script[type="text/javascript"]')
    .each(function () { 
      return load = (url != $(this).attr('src')); 
    });
  if (load){
    jQuery.ajax({
      type: 'GET',
      url: url,
      success: callback,
      dataType: 'script',
      cache: cache
    });
  } else {
    if (jQuery.isFunction(callback)) {
      callback.call(this);
    };
  };
};


/* jQuery Content Panel Switcher JS - v1.1 */
var jcps = {};
var prevtarget = null;
jcps.fader = function (speed, target, panel) {
    jcps.show(target, panel);
    
    if (panel == null) {
        panel = '';
    }
    
    $('.switcher' + panel).click(function () {
        var _targetId = $(this).attr('id');
        var _contentId = '#' + $(this).attr('id') + '-content';
        var _content = $(_contentId).html();
        
        if (speed == 0) {
            $(target).html(_content);

            $('#' + _targetId).parent().addClass('selected');
            $('#' + prevtarget).parent().removeClass('selected');
        }
        else {
            $(target).fadeToggle(speed, function() {
                $(this).html(_content);
            }).fadeToggle(speed);

            $('#' + _targetId).parent().addClass('selected');
            $('#' + prevtarget).parent().removeClass('selected');
        }

        prevtarget = _targetId;
        
    });
    
};
jcps.slider = function (speed, target, panel) {
    jcps.show(target, panel);
    if (panel == null) { panel = '' };
    $('.switcher' + panel).click(function () {
        var _contentId = '#' + $(this).attr('id') + '-content';
        var _content = $(_contentId).html();
        if (speed == 0) {
            $(target).html(_content);
        }
        else {
            $(target).slideToggle(speed, function () { $(this).html(_content); }).slideToggle(speed);
        }
    });
};
jcps.show = function (target, panel) {
    $('.show').each(function () {
        if (panel == null) {
            $(target).html($(this).html());

            prevtarget = $(this).attr("id").replace('-content', '');
        }
        else {
            var trimPanel = panel.replace('.', '');
            if ($(this).hasClass(trimPanel) == true) {
                 $(target).append($(this).html() + '');
            }
        }
    });
}
/* End jQuery Content Panel Switcher JS - v1.1 */