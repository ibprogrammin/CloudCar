﻿(function($) {



    var options = { series: { curvedLines: { active: false,
        show: false,
        fit: false,
        lineWidth: 2,
        curvePointFactor: 20,
        fitPointDist: 0.001
}}
    };


    function init(plot) {

        plot.hooks.processOptions.push(processOptions);

        //if the plugin is active register draw method
        function processOptions(plot, options) {
            if (options.series.curvedLines.active) {
                plot.hooks.draw.push(draw);
            }
        }


        //select the data sets that should be drawn with curved lines and draws them     
        function draw(plot, ctx) {
            var series;
            var axes = plot.getAxes();
            var sdata = plot.getData();
            var offset = plot.getPlotOffset();

            for (var i = 0; i < sdata.length; i++) {
                series = sdata[i];
                if (series.curvedLines.show && series.curvedLines.lineWidth > 0) {

                    axisx = axes.xaxis;
                    axisy = axes.yaxis;

                    ctx.save();
                    ctx.translate(offset.left, offset.top);
                    ctx.lineJoin = "round";
                    ctx.strokeStyle = series.color;
                    ctx.lineWidth = series.curvedLines.lineWidth;


                    var points = calculateCurvePoints(series.data, series.curvedLines);
                    plotLine(ctx, points, axisx, axisy);
                    ctx.restore();
                }
            }
        }


        //nearly the same as in the core library
        //only ps is adjusted to 2
        function plotLine(ctx, points, axisx, axisy) {

            var ps = 2;
            var prevx = null;
            var prevy = null;

            ctx.beginPath();

            for (var i = ps; i < points.length; i += ps) {
                var x1 = points[i - ps], y1 = points[i - ps + 1];
                var x2 = points[i], y2 = points[i + 1];

                if (x1 == null || x2 == null)
                    continue;

                // clip with ymin
                if (y1 <= y2 && y1 < axisy.min) {
                    if (y2 < axisy.min)
                        continue;   // line segment is outside
                    // compute new intersection point
                    x1 = (axisy.min - y1) / (y2 - y1) * (x2 - x1) + x1;
                    y1 = axisy.min;
                }
                else if (y2 <= y1 && y2 < axisy.min) {
                    if (y1 < axisy.min)
                        continue;
                    x2 = (axisy.min - y1) / (y2 - y1) * (x2 - x1) + x1;
                    y2 = axisy.min;
                }

                // clip with ymax
                if (y1 >= y2 && y1 > axisy.max) {
                    if (y2 > axisy.max)
                        continue;
                    x1 = (axisy.max - y1) / (y2 - y1) * (x2 - x1) + x1;
                    y1 = axisy.max;
                }
                else if (y2 >= y1 && y2 > axisy.max) {
                    if (y1 > axisy.max)
                        continue;
                    x2 = (axisy.max - y1) / (y2 - y1) * (x2 - x1) + x1;
                    y2 = axisy.max;
                }

                // clip with xmin
                if (x1 <= x2 && x1 < axisx.min) {
                    if (x2 < axisx.min)
                        continue;
                    y1 = (axisx.min - x1) / (x2 - x1) * (y2 - y1) + y1;
                    x1 = axisx.min;
                }
                else if (x2 <= x1 && x2 < axisx.min) {
                    if (x1 < axisx.min)
                        continue;
                    y2 = (axisx.min - x1) / (x2 - x1) * (y2 - y1) + y1;
                    x2 = axisx.min;
                }

                // clip with xmax
                if (x1 >= x2 && x1 > axisx.max) {
                    if (x2 > axisx.max)
                        continue;
                    y1 = (axisx.max - x1) / (x2 - x1) * (y2 - y1) + y1;
                    x1 = axisx.max;
                }
                else if (x2 >= x1 && x2 > axisx.max) {
                    if (x1 > axisx.max)
                        continue;
                    y2 = (axisx.max - x1) / (x2 - x1) * (y2 - y1) + y1;
                    x2 = axisx.max;
                }

                if (x1 != prevx || y1 != prevy)
                    ctx.moveTo(axisx.p2c(x1), axisy.p2c(y1));

                prevx = x2;
                prevy = y2;
                ctx.lineTo(axisx.p2c(x2), axisy.p2c(y2));
            }
            ctx.stroke();
        }


        //no real idea whats going on here code mainly from https://code.google.com/p/flot/issues/detail?id=226
        //I don't understand what nergal computes here and to be honest I didn't even try
        function calculateCurvePoints(data, curvedLinesOptions) {

            var num = curvedLinesOptions.curvePointFactor * data.length;
            var xdata = new Array;
            var ydata = new Array;

            if (curvedLinesOptions.fit) {
                //insert a point before and after the "real" data point to force the line
                //to have a max,min at the data point
                var neigh = curvedLinesOptions.fitPointDist;
                var j = 0;

                for (var i = 0; i < data.length; i++) {

                    //smooth front
                    xdata[j] = data[i][0] - 0.1;

                    if (i > 0) {
                        ydata[j] = data[i - 1][1] * neigh + data[i][1] * (1 - neigh);
                    } else {
                        ydata[j] = data[i][1];
                    }
                    j++;


                    xdata[j] = data[i][0];
                    ydata[j] = data[i][1];
                    j++;


                    //smooth back
                    xdata[j] = data[i][0] + 0.1;
                    if ((i + 1) < data.length) {
                        ydata[j] = data[i + 1][1] * neigh + data[i][1] * (1 - neigh);
                    } else {
                        ydata[j] = data[i][1];
                    }

                    j++;
                }
            } else {
                //just use the datapoints
                for (var i = 0; i < data.length; i++) {
                    xdata[i] = data[i][0];
                    ydata[i] = data[i][1];
                }
            }

            var n = xdata.length;

            var y2 = new Array();
            var delta = new Array();
            y2[0] = 0;
            y2[n - 1] = 0;
            delta[0] = 0;

            for (var i = 1; i < n - 1; ++i) {
                var d = (xdata[i + 1] - xdata[i - 1]);
                if (d == 0) {
                    return null;
                }

                var s = (xdata[i] - xdata[i - 1]) / d;
                var p = s * y2[i - 1] + 2;
                y2[i] = (s - 1) / p;
                delta[i] = (ydata[i + 1] - ydata[i]) / (xdata[i + 1] - xdata[i]) - (ydata[i] - ydata[i - 1]) / (xdata[i] - xdata[i - 1]);
                delta[i] = (6 * delta[i] / (xdata[i + 1] - xdata[i - 1]) - s * delta[i - 1]) / p;
            }

            for (var j = n - 2; j >= 0; --j) {
                y2[j] = y2[j] * y2[j + 1] + delta[j];
            }

            var step = (xdata[n - 1] - xdata[0]) / (num - 1);

            var xnew = new Array;
            var ynew = new Array;
            var result = new Array;

            xnew[0] = xdata[0];
            ynew[0] = ydata[0];


            for (j = 1; j < num; ++j) {
                xnew[j] = xnew[0] + j * step;

                var max = n - 1;
                var min = 0;

                while (max - min > 1) {
                    var k = Math.round((max + min) / 2);
                    if (xdata[k] > xnew[j]) {
                        max = k;
                    } else {
                        min = k;
                    }
                }

                var h = (xdata[max] - xdata[min]);

                if (h == 0) {
                    return null;
                }

                var a = (xdata[max] - xnew[j]) / h;
                var b = (xnew[j] - xdata[min]) / h;

                ynew[j] = a * ydata[min] + b * ydata[max] + ((a * a * a - a) * y2[min] + (b * b * b - b) * y2[max]) * (h * h) / 6;
                if (ynew[j] < 0.01) {
                    ynew[j] = 0;
                }
                result.push(xnew[j]);
                result.push(ynew[j]);
            }

            return result;
        }




    } //end init


    $.plot.plugins.push({
        init: init,
        options: options,
        name: 'curvedLines',
        version: '0.1'
    });


})(jQuery);
  