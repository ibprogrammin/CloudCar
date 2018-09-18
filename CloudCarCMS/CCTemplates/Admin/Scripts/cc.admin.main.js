var ADAPT_CONFIG = {
    // Where is your CSS?
    path: '/CCTemplates/Admin/Styles/',

    // false = Only run once, when page first loads.
    // true = Change on window resize and page tilt.
    dynamic: true,

    // First range entry is the minimum.
    // Last range entry is the maximum.
    // Separate ranges by "to" keyword.
    range: [
    '0px    to 760px  = admin.mobile.css',
    '760px  to 980px  = admin.720.css',
    '980px  to 1550px = admin.960.css',
    '1550px           = admin.1600.css'
    ]
};

var visits = false;

loaded = function (element) {
    return element.length > 0;
};

jQuery.fn.exists = function () { return this.length > 0; };

window.onload = function () {
    oo.setAPIKey(ooChart_api_key);
    oo.load(function () {

        if ($('#visits').length > 0) {
            var visits = new oo.Metric(profile_id, "30d");
            visits.setMetric("ga:visits");
            visits.draw('visits');
        }

        /*if (('.bounces').length > 0) {
        var bounce = new oo.Metric("24382466", "30d");
        bounce.setMetric("ga:bounces");
        bounce.draw('bounces');
        }*/

        if ($('#bounces').length > 0) {
            var bouncerate = new oo.Metric(profile_id, "30d");
            bouncerate.setMetric("ga:entranceBounceRate");
            bouncerate.draw('bounces', function () {
                var element = document.getElementById('bounces');
                var number = parseFloat(element.innerHTML);
                element.innerHTML = number.toFixed(2) + "%";
            });
        }

        if ($('#pageviews').length > 0) {
            var pageviews = new oo.Metric(profile_id, "30d");
            pageviews.setMetric("ga:pageviews");
            pageviews.draw('pageviews');
        }

        if ($('#visitor-overview').length > 0) {
            var timeline = new oo.Timeline(profile_id, "30d");
            timeline.addMetric("ga:visits", "Visits");
            timeline.addMetric("ga:newVisits", "New Visits");
            timeline.addMetric("ga:bounces", "Bounces");

            if ($('#visitor-overview').css('width') >= "900px") {
                timeline.setOptions({
                    backgroundColor: { stroke: "#AAAAAA", strokeWidth: 0, fill: '#F2F2F2' },
                    isStacked: true,
                    height: 160,
                    pointSize: 5,
                    lineWidth: 2,
                    chartArea: { left: 40, top: 40, width: 880 },
                    legend: { position: 'top' },
                    colors: ['#33CCFF', '#66FF66', '#FF0000'],
                    hAxis: { baselineColor: "#F2F2F2", gridlines: { color: "#F2F2F2" } },
                    vAxis: { baselineColor: "#999999", gridlines: { color: "#DDDDDD", count: 3 } }
                });
            }
            else if ($('#visitor-overview').css('width') >= "720px") {
                timeline.setOptions({
                    backgroundColor: { stroke: "#AAAAAA", strokeWidth: 0, fill: '#F2F2F2' },
                    isStacked: true,
                    height: 160,
                    pointSize: 5,
                    lineWidth: 2,
                    chartArea: { left: 40, top: 40, width: 660 },
                    legend: { position: 'top' },
                    colors: ['#33CCFF', '#66FF66', '#FF0000'],
                    hAxis: { baselineColor: "#F2F2F2", gridlines: { color: "#F2F2F2" } },
                    vAxis: { baselineColor: "#999999", gridlines: { color: "#DDDDDD", count: 3 } }
                });
            }

            timeline.draw('visitor-overview');
        }

        if ($('#browser-usage').length > 0) {
            var pie = new oo.Pie(profile_id, "30d");
            pie.setMetric("ga:visits", "Visits");
            pie.setDimension("ga:browser");
            pie.setOptions({
                is3D: true,
                height: 250,
                width: 470,
                backgroundColor: "#F2F2F2",
                sliceVisibilityThreshold: 1 / 30,
                chartArea: { left: 10, top: 10, width: 490, height: 375 },
                legend: { alignment: 'center' },
                slices: { 2: { offset: 0.2 } }
            });
            pie.draw('browser-usage');
        }

        if ($('#screen-resolution').length > 0) {
            var pie5 = new oo.Pie(profile_id, "30d");
            pie5.setMetric("ga:visits", "Visits");
            pie5.setDimension("ga:screenResolution");
            pie5.setOptions({
                is3D: true,
                height: 250,
                width: 470,
                backgroundColor: "#F2F2F2",
                sliceVisibilityThreshold: 1 / 30,
                chartArea: { left: 10, top: 10, width: 490, height: 375 },
                legend: { alignment: 'center' },
                slices: { 2: { offset: 0.2 } }
            });
            pie5.draw('screen-resolution');
        }

        if ($('#search-engines').length > 0) {
            var pie2 = new oo.Pie(profile_id, "30d");
            pie2.setMetric("ga:organicSearches", "Searches");
            pie2.setDimension("ga:source");
            pie2.setOptions({
                is3D: true,
                height: 250,
                width: 490,
                backgroundColor: "#F2F2F2",
                chartArea: { left: 10, top: 10, width: 490, height: 375 },
                legend: { alignment: 'center' },
                slices: { 1: { offset: 0.2 } }
            });
            pie2.draw('search-engines');
        }

        if ($('#keywords-overview').length > 0) {
            var table2 = new oo.Table(profile_id, "30d");
            table2.addMetric("ga:organicSearches", "Searches");
            table2.addDimension("ga:keyword", "Keywords");
            table2.setOptions({ width: "450px", sortColumn: 1, sortAscending: false, pageSize: 10, page: "enable" });
            table2.draw('keywords-overview');
        }

        if ($('#region-country').length > 0) {
            var pie3 = new oo.Pie(profile_id, "30d");
            pie3.setMetric("ga:newVisits", "New Visits");
            pie3.setDimension("ga:country");
            pie3.setOptions({
                is3D: true,
                height: 250,
                width: 470,
                backgroundColor: "#F2F2F2",
                sliceVisibilityThreshold: 1 / 50,
                chartArea: { left: 10, top: 10, width: 490, height: 375 },
                legend: { alignment: 'center' },
                slices: { 6: { offset: 0.2 } }
            });
            pie3.draw('region-country');
        }

        if ($('#region-city').length > 0) {
            var pie4 = new oo.Pie(profile_id, "30d");
            pie4.setMetric("ga:newVisits", "New Visits");
            pie4.setDimension("ga:city");
            pie4.setOptions({
                is3D: true,
                height: 250,
                width: 470,
                backgroundColor: "#F2F2F2",
                sliceVisibilityThreshold: 1 / 100,
                chartArea: { left: 10, top: 10, width: 490, height: 375 },
                legend: { alignment: 'center' },
                slices: { 4: { offset: 0.2 } }
            });
            pie4.draw('region-city');
        }

        if ($('#page-views').length > 0) {
            var table3 = new oo.Table(profile_id, "30d");
            table3.addMetric("ga:uniquePageviews", "Views");
            table3.addDimension("ga:pagePath", "Page");
            table3.setOptions({
                width: 940,
                sortColumn: 1,
                sortAscending: false,
                pageSize: 10,
                page: "enable",
                cssClassNames: { table: 'default-table', headerRow: 'default-table-header' }
            });
            table3.draw('page-views');
        }
    });
};

google.load("visualization", "1", { packages: ["corechart"] });
google.setOnLoadCallback(drawChart);
function drawChart() {
    if ($('#monthly-sales-chart').length > 0) {
        var monthlydata = google.visualization.arrayToDataTable(monthly_sales_data);
        var monthlyoptions = null;

        if ($('#monthly-sales-chart').css('width') == "940px") {
            monthlyoptions = {
                backgroundColor: { stroke: "#AAAAAA", strokeWidth: 0, fill: '#F2F2F2' },
                isStacked: true,
                /*width: 940,*/
                height: 160,
                pointSize: 6,
                lineWidth: 3,
                chartArea: { left: 60, top: 40, width: 850 },
                legend: { position: 'top' },
                colors: ['#66FF66', '#666666'],
                hAxis: { baselineColor: "#F2F2F2", gridlines: { color: "#F2F2F2" }, textPosition: 'none' },
                vAxis: { baselineColor: "#999999", gridlines: { color: "#DDDDDD", count: 3 } }
            };
        }
        else if ($('#monthly-sales-chart').css('width') == "720px") {
            monthlyoptions = {
                backgroundColor: { stroke: "#AAAAAA", strokeWidth: 0, fill: '#F2F2F2' },
                isStacked: true,
                /*width: 940,*/
                height: 160,
                pointSize: 6,
                lineWidth: 3,
                chartArea: { left: 60, top: 40, width: 640 },
                legend: { position: 'top' },
                colors: ['#66FF66', '#666666'],
                hAxis: { baselineColor: "#F2F2F2", gridlines: { color: "#F2F2F2" }, textPosition: 'none' },
                vAxis: { baselineColor: "#999999", gridlines: { color: "#DDDDDD", count: 3 } }
            };
        }

        var monthlychart = new google.visualization.AreaChart(document.getElementById('monthly-sales-chart'));
        monthlychart.draw(monthlydata, monthlyoptions);
    }

    if ($('#daily-sales-chart').length > 0) {
        var dailydata = google.visualization.arrayToDataTable(daily_sales_data);
        var dailyoptions = null;

        if ($('#daily-sales-chart').css('width') == "940px") {
            dailyoptions = {
                backgroundColor: { stroke: "#AAAAAA", strokeWidth: 0, fill: '#F2F2F2' },
                isStacked: true,
                /*width: 940,*/
                height: 160,
                pointSize: 6,
                lineWidth: 3,
                chartArea: { left: 60, top: 40, width: 850 },
                legend: { position: 'top' },
                colors: ['#66FF66', '#666666'],
                hAxis: { baselineColor: "#F2F2F2", gridlines: { color: "#F2F2F2" }, textPosition: 'none' },
                vAxis: { baselineColor: "#999999", gridlines: { color: "#DDDDDD", count: 3 } }
            };
        }
        else if ($('#daily-sales-chart').css('width') == "720px") {
            dailyoptions = {
                backgroundColor: { stroke: "#AAAAAA", strokeWidth: 0, fill: '#F2F2F2' },
                isStacked: true,
                /*width: 940,*/
                height: 160,
                pointSize: 6,
                lineWidth: 3,
                chartArea: { left: 60, top: 40, width: 640 },
                legend: { position: 'top' },
                colors: ['#66FF66', '#666666'],
                hAxis: { baselineColor: "#F2F2F2", gridlines: { color: "#F2F2F2" }, textPosition: 'none' },
                vAxis: { baselineColor: "#999999", gridlines: { color: "#DDDDDD", count: 3 } }
            };
        }

        var dailychart = new google.visualization.AreaChart(document.getElementById('daily-sales-chart'));
        dailychart.draw(dailydata, dailyoptions);
    }

    if ($('#monthly-visit-chart').length > 0) {
        var monthlydata = google.visualization.arrayToDataTable(monthly_visit_data);

        var monthlyoptions = {
            backgroundColor: { stroke: "#AAAAAA", strokeWidth: 0, fill: '#FFFFFF' },
            isStacked: true,
            width: 940,
            height: 160,
            pointSize: 6,
            lineWidth: 3,
            chartArea: { left: 40, top: 40, width: 880 },
            legend: { position: 'top' },
            colors: ['#33CCFF', '#66FF66', '#FF0000'],
            hAxis: { baselineColor: "#FFFFFF", gridlines: { color: "#FFFFFF" }, textPosition: 'none' },
            vAxis: { baselineColor: "#999999", gridlines: { color: "#EEEEEE", count: 3 } }
        };

        var monthlychart = new google.visualization.AreaChart(document.getElementById('monthly-visit-chart'));
        monthlychart.draw(monthlydata, monthlyoptions);
    }

}

jQuery(document).ready(function ($) {
    loadCKEditor();

    CreateSlideDownMenu("#SiteMenu");
    CreateSlideDownMenu("#ContentMenu");
    CreateSlideDownMenu("#StoreMenu");

    if ($('.check-password').exists()) {
        $('.check-password').passStrength({
            shortPass: "short-pass",
            badPass: "bad-pass",
            goodPass: "good-pass",
            strongPass: "strong-pass",
            baseStyle: "test-result",
            userid: "#UserNameTextBox",
            messageloc: 1
        });
    }

    if ($('.notice-panel').exists()) {
        $('.notice-panel').fadeIn(1000);

        FadeInWaitFadeOut(".notice-list", ".notice-panel label", 1000, 4700, 0);
    }
    if ($('.tip-panel').exists()) {
        $('.tip-panel').fadeIn(1000);

        FadeInWaitFadeOut(".tip-list", ".tip-panel label", 1000, 5000, 0);
    }
    if ($('.tab-container').exists()) {
        $.getScript('/CCTemplates/Shared/Scripts/EasyTabs/jquery.easytabs.min.js', function (xhr) {
            try {
                loadEasyTabs('.tab-container');
            } catch (err) {
                eval(xhr);
                loadEasyTabs('.tab-container');
            }

            $('li.tab a').click("click", function () { loadCKEditor(); });

            $('.next-tab, .prev-tab').click(function () {
                var i = parseInt($(this).attr('rel'));
                var tabSelector = $('.tab-container').data('easytabs').tabs.children('a:eq(' + i + ')').attr('href');
                $('.tab-container').easytabs('select', tabSelector);
                return false;
            });

        });
    }
    if ($('.vertical-tab-container').exists()) {
        var tabs = $('.vertical-tab-container li'); //grab tabs
        var contents = $('.vertical-tab-contents li'); //grab contents

        tabs.bind('click', function () {
            contents.hide(); //hide all contents
            tabs.removeClass('current'); //remove 'current' classes
            $(contents[$(this).index()]).show(); //show tab content that matches tab title index
            $(this).addClass('current'); //add current class on clicked tab title
        });
    }
});

function loadCKEditor() {
    if ($('.ck-editor').exists()) {
        $('.ck-editor').ckeditor({
            enterMode: CKEDITOR.ENTER_BR,
            shiftEnterMode: CKEDITOR.ENTER_P,
            height: '460px',
            scayt_autoStartup: 'true',
            allowedContent: true,
            uiColor: "#FFFFFF",
            filebrowserBrowseUrl: '/CCAdmin/EditorControls/CKEditorImageBrowser.aspx',
            filebrowserUploadUrl: '/CCAdmin/ContentManagement/UploadFiles.aspx',
            filebrowserWindowWidth: '640',
            filebrowserWindowHeight: '480',
            contentsCss: '/CCTemplates/Default/Styles/main.css'
        });

        return true;
    }
    else { return false; }
}

function loadEasyTabs($) {
    jQuery($).easytabs({
        collapsible: false,
        animate: false,
        remote: true,
        selected: -1
    });
}

function FadeInWaitFadeOut(list, label, duration, wait, index) {
    var maxIndex = $(list + " li").length - 1;

    var messageText = $(list + " li:eq(" + index + ")").html();
    $(label).html(messageText).fadeIn(duration).delay(wait).fadeOut(duration, function () {
        (index == maxIndex) ? index = 0 : index++;
        FadeInWaitFadeOut(list, label, duration, wait, index);
    });
}

function CreateSlideDownMenu(element) {

    $(element).mouseenter(function () {
        clearTimeout($(element).data('timeoutId'));

        $(element + ' ul').css({ 'z-index': "9996" });

        $(element + ' ul').slideDown("slow", function () {
            $(this).css({ 'overflow': 'visible' });
        });
    });

    $(element).mouseleave(function () {
        $(element + ' ul').css({ 'z-index': '9995' });

        var currentElement = $(this),
            timeoutId = setTimeout(function () {
                $(element + ' ul').stop().slideUp("slow", function () {
                    $(this).hide();
                });
            }, 500);

        currentElement.data('timeoutId', timeoutId);
    });
}

if (typeof (Sys.Browser.WebKit) == "undefined") {
    Sys.Browser.WebKit = {};
}
if (navigator.userAgent.indexOf("WebKit/") > -1) {
    Sys.Browser.agent = Sys.Browser.WebKit;
    Sys.Browser.version = parseFloat(navigator.userAgent.match(/WebKit\/(\d+(\.\d+)?)/)[1]);
    Sys.Browser.name = "WebKit";
}

function rotateImage(element, src) {
    element.src = src;
}

function preloader(src) {
    imageObj = new Image();
    imageObj.src = src;
}



(function ($) {
    $.fn.shortPass = 'Too short';
    $.fn.badPass = 'Weak';
    $.fn.goodPass = 'Good';
    $.fn.strongPass = 'Strong';
    $.fn.samePassword = 'Username and Password identical.';
    $.fn.resultStyle = "";

    $.fn.passStrength = function (options) {

        var defaults = {
            shortPass: "shortPass", //optional
            badPass: "badPass", 	//optional
            goodPass: "goodPass", 	//optional
            strongPass: "strongPass", //optional
            baseStyle: "testresult", //optional
            userid: "", 			//required override
            messageloc: 1				//before == 0 or after == 1
        };
        var opts = $.extend(defaults, options);

        return this.each(function () {
            var obj = $(this);

            $(obj).unbind().keyup(function () {

                var results = $.fn.teststrength($(this).val(), $(opts.userid).val(), opts);

                if (opts.messageloc === 1) {
                    $(this).next("." + opts.baseStyle).remove();
                    $(this).after("<span class=\"" + opts.baseStyle + "\"><span></span></span>");
                    $(this).next("." + opts.baseStyle).addClass($(this).resultStyle).find("span").text(results);
                }
                else {
                    $(this).prev("." + opts.baseStyle).remove();
                    $(this).before("<span class=\"" + opts.baseStyle + "\"><span></span></span>");
                    $(this).prev("." + opts.baseStyle).addClass($(this).resultStyle).find("span").text(results);
                }
            });

            //FUNCTIONS
            $.fn.teststrength = function (password, username, option) {
                var score = 0;

                //password < 4
                if (password.length < 4) { this.resultStyle = option.shortPass; return $(this).shortPass; }

                //password == user name
                if (password.toLowerCase() == username.toLowerCase()) { this.resultStyle = option.badPass; return $(this).samePassword; }

                //password length
                score += password.length * 4;
                score += ($.fn.checkRepetition(1, password).length - password.length) * 1;
                score += ($.fn.checkRepetition(2, password).length - password.length) * 1;
                score += ($.fn.checkRepetition(3, password).length - password.length) * 1;
                score += ($.fn.checkRepetition(4, password).length - password.length) * 1;

                //password has 3 numbers
                if (password.match(/(.*[0-9].*[0-9].*[0-9])/)) { score += 5; }

                //password has 2 symbols
                if (password.match(/(.*[!,@,#,$,%,^,&,*,?,_,~].*[!,@,#,$,%,^,&,*,?,_,~])/)) { score += 5; }

                //password has Upper and Lower chars
                if (password.match(/([a-z].*[A-Z])|([A-Z].*[a-z])/)) { score += 10; }

                //password has number and chars
                if (password.match(/([a-zA-Z])/) && password.match(/([0-9])/)) { score += 15; }
                //
                //password has number and symbol
                if (password.match(/([!,@,#,$,%,^,&,*,?,_,~])/) && password.match(/([0-9])/)) { score += 15; }

                //password has char and symbol
                if (password.match(/([!,@,#,$,%,^,&,*,?,_,~])/) && password.match(/([a-zA-Z])/)) { score += 15; }

                //password is just a numbers or chars
                if (password.match(/^\w+$/) || password.match(/^\d+$/)) { score -= 10; }

                //verifying 0 < score < 100
                if (score < 0) { score = 0; }
                if (score > 100) { score = 100; }

                if (score < 34) { this.resultStyle = option.badPass; return $(this).badPass; }
                if (score < 68) { this.resultStyle = option.goodPass; return $(this).goodPass; }

                this.resultStyle = option.strongPass;
                return $(this).strongPass;

            };

        });
    };
})(jQuery);


$.fn.checkRepetition = function (pLen, str) {
    var res = "";
    for (var i = 0; i < str.length; i++) {
        var repeated = true;

        for (var j = 0; j < pLen && (j + i + pLen) < str.length; j++) {
            repeated = repeated && (str.charAt(j + i) == str.charAt(j + i + pLen));
        }
        if (j < pLen) { repeated = false; }
        if (repeated) {
            i += pLen - 1;
            repeated = false;
        }
        else {
            res += str.charAt(i);
        }
    }
    return res;
};

(function (a, b, c, d) { function e() { clearTimeout(i); for (var c = a.innerWidth || b.documentElement.clientWidth || b.body.clientWidth || 0, e, f, o, p, q = m, u = m - 1; q--;) { e = l[q].split("="), f = e[0], p = e[1] ? e[1].replace(/\s/g, "") : q, e = (o = f.match("to")) ? parseInt(f.split("to")[0], 10) : parseInt(f, 10), f = o ? parseInt(f.split("to")[1], 10) : d; if (!f && q === u && c > e || c > e && c <= f) { g = k + p; break } g = "" } h ? h !== g && (h = n.href = g, j && j(q, c)) : (h = n.href = g, j && j(q, c), k && (b.head || b.getElementsByTagName("head")[0]).appendChild(n)) } function f() { clearTimeout(i), i = setTimeout(e, 100) } if (c) { var g, h, i, j = typeof c.callback == "function" ? c.callback : d, k = c.path ? c.path : "", l = c.range, m = l.length, n = b.createElement("link"); n.rel = "stylesheet", e(), c.dynamic && (a.addEventListener ? a.addEventListener("resize", f, !1) : a.attachEvent ? a.attachEvent("onresize", f) : a.onresize = f) } })(this, this.document, ADAPT_CONFIG)