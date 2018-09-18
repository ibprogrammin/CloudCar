<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ooChartTest.aspx.vb" Inherits="CloudCar.ooChartTest" %>
<%@ Import Namespace="CloudCar.CCFramework.Commerce" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div id='query'></div>
        <div id='chart'></div>
        <div id='chart2'></div>
        <div id='visits'></div>
        <div id='bounce'></div>
        <div id='pageviews'></div>
        <div id='pie' style="float: left;"></div>
        <div id='pie2' style="float: left;"></div>
        <div id='pie3' style="float: left;"></div>
        <br style="clear: both;" />
        <div id='bar'></div>
        <div id='table' style="float: left;"></div>
        <div id='table2' style="float: left;"></div>
        <div id='table3' style="float: left;"></div>
    </div>
    
    <div id="monthly-sales-chart" style="width: 900px; height: 500px;"></div>
    <div id="daily-sales-chart" style="width: 900px; height: 500px;"></div>
    </form>
</body>

<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>
<script language="javascript" type="text/javascript" src="/CCTemplates/Shared/Scripts/ooCharts/oocharts.js"></script>
<script type="text/javascript" src="https://www.google.com/jsapi"></script>

<script type="text/javascript">
    window.onload = function () {
        oo.setAPIKey("fe677510d7739e2e735673747469a6a3fcb76ef2");
        oo.load(function () {

            /*var query = new oo.Query('24382466', '30d');
            query.addMetric('ga:visits');
            query.addDimension('ga:date');
            query.addSort('-ga:visits');
            query.execute(function (data) {
            alert(data);
            });*/

            var timeline = new oo.Timeline("24382466", "30d");
            timeline.addMetric("ga:visits", "Visits");
            timeline.addMetric("ga:newVisits", "New Visits");
            timeline.addMetric("ga:bounces", "Bounces");
            timeline.setOptions({ backgroundColor: { stroke: "#AAAAAA", strokeWidth: 0, fill: '#FFFFFF' },
                isStacked: true, /*width: 920,*/height: 160, pointSize: 5, lineWidth: 2,
                chartArea: { left: 40, top: 40 },
                legend: { position: 'top' },
                colors: ['#33CCFF', '#66FF66', '#FF0000'],
                hAxis: { baselineColor: "#FFFFFF", gridlines: { color: "#FFFFFF"} },
                vAxis: { baselineColor: "#999999", gridlines: { color: "#EEEEEE", count: 3} }
            });
            timeline.draw('chart');

            var timeline2 = new oo.Timeline("24382466", "30d");
            timeline2.addMetric("ga:visitBounceRate", "Bounce Rate");
            //timeline2.addMetric("ga:newVisits", "New Visits");
            //timeline2.query.addMetric('ga:visits', 'Visits');
            //timeline2.query.addDimension('ga:date', 'Date');
            //timeline2.query.execute();
            timeline2.setOptions({ 
                backgroundColor: { stroke: "#AAAAAA", strokeWidth: 0, fill: '#FFFFFF' },
                isStacked: true, /*width: 920,*/height: 160, pointSize: 6, lineWidth: 3,
                chartArea: { left: 40, top: 40 },
                legend: { position: 'top' },
                colors: ['#999999', '#666666'],
                hAxis: { baselineColor: "#FFFFFF", gridlines: { color: "#FFFFFF"} },
                vAxis: { baselineColor: "#999999", gridlines: { color: "#EEEEEE", count: 3} }
            });
            timeline2.draw('chart2');

            var visits = new oo.Metric("24382466", "30d");
            visits.setMetric("ga:visits");
            visits.draw('visits');

            var bounce = new oo.Metric("24382466", "30d");
            bounce.setMetric("ga:bounces");
            bounce.draw('bounce');

            var pageviews = new oo.Metric("24382466", "30d");
            pageviews.setMetric("ga:pageviews");
            pageviews.draw('pageviews');

            var pie = new oo.Pie("24382466", "30d");
            pie.setMetric("ga:visits", "Visits");
            pie.setDimension("ga:browser");
            pie.setOptions({ is3D: true, height: 270, width: 500,
                sliceVisibilityThreshold: 1 / 30,
                chartArea: { left: 10, top: 10 },
                legend: { alignment: 'center' },
                slices: { 2: { offset: 0.2} }
            });
            pie.draw('pie');

            var pie2 = new oo.Pie("24382466", "30d");
            pie2.setMetric("ga:organicSearches", "Searches");
            pie2.setDimension("ga:source");
            pie2.setOptions({ is3D: true, height: 270, width: 500,
                chartArea: { left: 10, top: 10 },
                legend: { alignment: 'center' },
                slices: { 1: { offset: 0.2} }
            });
            pie2.draw('pie2');

            var pie3 = new oo.Pie("24382466", "30d");
            pie3.setMetric("ga:newVisits", "New Visits");
            pie3.setDimension("ga:country");
            pie3.setOptions({ is3D: true, height: 270, width: 500,
                sliceVisibilityThreshold: 1 / 50,
                chartArea: { left: 10, top: 10 },
                legend: { alignment: 'center' },
                slices: { 6: { offset: 0.2} }
            });
            pie3.draw('pie3');

            var bar = new oo.Bar("24382466", "30d");
            //bar.addMetric("ga:visits", "Visits");
            bar.addMetric("ga:newVisits", "New Visits");
            bar.setDimension("ga:country");
            bar.setOptions({ sliceVisibilityThreshold: 1 / 2 });
            bar.draw('bar');

            var table = new oo.Table("24382466", "30d");
            table.addMetric("ga:visits", "Visits");
            table.addDimension("ga:city", "City");
            table.setOptions({ width: "200px", sortColumn: 1, sortAscending: false, pageSize: 10, page: "enable" });
            table.draw('table');

            var table2 = new oo.Table("24382466", "30d");
            table2.addMetric("ga:organicSearches", "Searches");
            table2.addDimension("ga:keyword", "Keyword");
            table2.setOptions({ width: "400px", sortColumn: 1, sortAscending: false, pageSize: 10, page: "enable" });
            table2.draw('table2');

            var table3 = new oo.Table("24382466", "30d");
            table3.addMetric("ga:uniquePageviews", "Views");
            table3.addDimension("ga:pagePath", "Page");
            table3.setOptions({ width: "400px", sortColumn: 1, sortAscending: false, pageSize: 10, page: "enable" });
            table3.draw('table3');

        });
    };


    google.load("visualization", "1", { packages: ["corechart"] });
    google.setOnLoadCallback(drawChart);
    function drawChart() {
        var monthlydata = google.visualization.arrayToDataTable(<%= OrderController.GetMonthlySalesFormatedForChart(12) %>);

        var monthlyoptions = {
            title: 'Monthly Sales',
            backgroundColor: { stroke: "#AAAAAA", strokeWidth: 0, fill: '#FFFFFF' },
            isStacked: true, /*width: 920,*/height: 160, pointSize: 6, lineWidth: 3,
            chartArea: { left: 40, top: 40 },
            legend: { position: 'top' },
            colors: ['#66FF66', '#666666'],
            hAxis: { baselineColor: "#FFFFFF", gridlines: { color: "#FFFFFF"} },
            vAxis: { baselineColor: "#999999", gridlines: { color: "#EEEEEE", count: 3} }
        };

        var monthlychart = new google.visualization.LineChart(document.getElementById('monthly-sales-chart'));
        monthlychart.draw(monthlydata, monthlyoptions);
        
        var dailydata = google.visualization.arrayToDataTable(<%= OrderController.GetDailySalesFormatedForChart(30) %>);

        var dailyoptions = {
            title: 'Daily Sales',
            backgroundColor: { stroke: "#AAAAAA", strokeWidth: 0, fill: '#FFFFFF' },
            isStacked: true, /*width: 920,*/height: 160, pointSize: 6, lineWidth: 3,
            chartArea: { left: 40, top: 40 },
            legend: { position: 'top' },
            colors: ['#66FF66', '#666666'],
            hAxis: { baselineColor: "#FFFFFF", gridlines: { color: "#FFFFFF"} },
            vAxis: { baselineColor: "#999999", gridlines: { color: "#EEEEEE", count: 3} }
        };

        var dailychart = new google.visualization.LineChart(document.getElementById('daily-sales-chart'));
        dailychart.draw(dailydata, dailyoptions);
    }
</script>

</html>