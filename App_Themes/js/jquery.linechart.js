/*

Name: jquery.linechart.js
Description: A jQuery plugin to draw a scalable line chart using HTML, CSS and SVG.

Copyright 2012 Liquid State Ltd
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
The Software is provided "as is", without warranty of any kind, express or implied, including but not limited to the warranties of merchantability, fitness for a particular purpose and noninfringement. In no event shall the authors or copyright holders be liable for any claim, damages or other liability, whether in an action of contract, tort or otherwise, arising from, out of or in connection with the Software or the use or other dealings in the Software.

*/

(function( $ ){

	$.fn.linechart = function( options ) {

		var settings = $.extend( {

			'color': '#5A5758',
			'backgroundColor': '#fff',

			'width': 350,
			'height': 300,
			
			'headerText': 'Header text',

			// additionalText: can contain additional values, must specify the text, size, alignment 
			// and x,y position
			'additionalText': [ [ ['Additional text'], [7], ['right'], [250], [50] ], 
				  [ ['More additional text'], [6], ['right'], [250], [65] ] ], 
			
			'plotAreaLeft': 80,
			'plotAreaRight': 300,
			'plotAreaTop': 100,
			'plotAreaBottom': 220,
			
			'axisColor': '#9C9C9B',
			
			'yAxisValues': [ '0', 
				'1', 
				'2', 
				'3', 
				'4', 
				'5', 
				'6' ],
			'yLabel': 'y Label',
			'yMaxValue': 7,
	      
			'xAxisValues': [ '0', 
				'1', 
				'2', 
				'3', 
				'4', 
				'5', 
				'6', 
				'7', 
				'8', 
				'9', 
				'10' ],
			'xLabel': 'x Label',
			'xMaxValue': 24, 

			'markWidth': 3,

			'planeColors': [ ], 
			'planeLegend': [ ], 

			// imageLayers: can contain 1 or more line images, must specify the source, width, height 
			// and x,y position
			//'imageLayers': [ [ ['img/data.png'], [223], [236], [73], [60] ] ],
			'imageLayers': [ ],

			'legendAtBottom': false,
			'dropLegendDownALittle': false,
			'addMaskPadding': false,

			'fontFamily': 'SurfaceMedium', 

			'regularChartId': null, // for scaling: if any other charts should use the same data but display at different sizes, specify the chart with the default chart size here
			
		}, options);
	    
		return this.each(function() {

			// begin: main

			var id = $(this).attr('id');

			var adjustment = 1;
			// if this is the larger chart, adjust the size
			if (settings.regularChartId && id != settings.regularChartId)
			{
				var regularChartWidth = $('#' + settings.regularChartId).width();
				var largeChartWidth = $(this).width();
				adjustment = largeChartWidth / regularChartWidth;
				
				settings.width = Math.round(settings.width * adjustment);
				settings.height = Math.round(settings.height * adjustment);
				
				settings.plotAreaLeft = Math.round(settings.plotAreaLeft * adjustment);
				settings.plotAreaRight = Math.round(settings.plotAreaRight * adjustment);
				settings.plotAreaTop = Math.round(settings.plotAreaTop * adjustment);
				settings.plotAreaBottom = Math.round(settings.plotAreaBottom * adjustment);
				
				settings.markWidth = Math.round(settings.markWidth * adjustment);
				settings.barSpacing = Math.round(settings.barSpacing * adjustment);
			}

			var plotAreaWidth = settings.plotAreaRight - settings.plotAreaLeft;
			var plotAreaHeight = settings.plotAreaBottom - settings.plotAreaTop;
			var nrOfPlanes = settings.planeColors.length;

			// add the background layer, 5px padding for rounded corners and drop shadow
			var backgroundMarkup = '<div class="background" '
				+ 'style="display: block; border: 1px solid ' 
				+ settings.backgroundColor 
				+ '; background-color: ' + settings.backgroundColor + '; ' 
				+ 'width: ' + (settings.width - 10) + 'px; ' 
				+ 'height: ' + (settings.height - 10) + 'px; '
				+ 'position: absolute; top: 5px; left: 5px;'
				+ '"></div>';
			$('#' + id).append(backgroundMarkup);

			// add images just above the background layer, below everything else, and add mask layers for animation.
			for(var imageCount = 0; imageCount < settings.imageLayers.length; imageCount++) {
				// reverse the order so the first image is on top
				var currentImage = settings.imageLayers.length - imageCount - 1; 
				
				var imageMarkup = '<img class="chartimg chartimg' + (currentImage + 1) + '" ' 
					+ 'src="' + settings.imageLayers[currentImage][0][0] + '" ' 
					+ 'style="display: block; border: none; ' 
					+ 'width: ' + (settings.imageLayers[currentImage][1][0] * adjustment) + 'px; ' 
					+ 'height: ' + (settings.imageLayers[currentImage][2][0] * adjustment) + 'px; '
					+ 'position: absolute; '
					+ 'left: ' + (settings.imageLayers[currentImage][3][0] * adjustment) + 'px; '
					+ 'top: ' + (settings.imageLayers[currentImage][4][0] * adjustment) + 'px; ' 
					+ '">';
				$('#' + id).append(imageMarkup);

				// add a reveal mask
				var revealMaskTop = (settings.imageLayers[currentImage][4][0] * adjustment);
				var revealMaskRight = (settings.width - 
					((settings.imageLayers[currentImage][3][0] + settings.imageLayers[currentImage][1][0]) * adjustment));
				var revealMaskWidth = (settings.imageLayers[currentImage][1][0] * adjustment);
				var revealMaskHeight = (settings.imageLayers[currentImage][2][0] * adjustment);
				if (settings.addMaskPadding)
				{
					revealMaskTop -= 10;
					revealMaskRight += 10;
					revealMaskWidth += 20;
					revealMaskHeight += 20;
				}
				
				var revealMaskMarkup = '<div class="revealmask revealmask' + (currentImage + 1) + '" '
					+ 'style="border: 1px solid ' 
					+ settings.backgroundColor 
					+ '; background-color: ' + settings.backgroundColor + '; ' 
					+ 'width: ' + revealMaskWidth + 'px; ' 
					+ 'height: ' + revealMaskHeight + 'px; '
					+ 'position: absolute; '
					//+ 'left: ' + (settings.imageLayers[currentImage][3][0] * adjustment) + 'px; '
					// anchor the div to the right, 
					// so the mask fades to the right when animated by shrinking the width
					+ 'right: ' + revealMaskRight + 'px; ' 
					+ 'top: ' + revealMaskTop + 'px; ' 
					+ '"></div>';
				$('#' + id).append(revealMaskMarkup);
			}
		
			// add the planes legend
			// make sure the number of planes and labels 
			// are the same as we need the colours for the labels
			if (nrOfPlanes == settings.planeLegend.length) 
			{
				for(var legendCount = 0; legendCount < (nrOfPlanes); legendCount++) {
					var legendBoxLength = Math.round(3 * adjustment), 
						legendBoxWidth = Math.round(25 * adjustment), 
						legendBoxSpacing = Math.round(10 * adjustment);
					var legendItemTop = settings.plotAreaTop + legendBoxLength 
							+ (legendCount * (legendBoxLength + legendBoxSpacing));
					if (settings.legendAtBottom)
					{
						legendItemTop = settings.plotAreaBottom - Math.round(15 * adjustment) 
							- (legendBoxLength + (legendCount * (legendBoxLength + legendBoxSpacing)));
					}
					if (settings.dropLegendDownALittle)
					{
						legendItemTop = legendItemTop + Math.round(20 * adjustment);
					}
					
					var legendItemWidth = Math.round(200 * adjustment);
					
					var currentItem = settings.legendAtBottom ? nrOfPlanes - legendCount - 1 : legendCount;

					var legendBoxMarkup = '<div class="legendbox legendbox' + (currentItem + 1) + '" ' 
						+ 'style="display: block; border: ' + Math.round(2 * adjustment) + 'px solid ' 
						+ settings.planeColors[currentItem] + '; ' 
						+ 'width: ' + legendBoxWidth + 'px; ' 
						+ 'height: 0px; '
						+ 'position: absolute; '
						+ 'left: ' + (settings.plotAreaRight - legendBoxWidth - Math.round(2 * adjustment)) + 'px; '
						+ 'top: ' + legendItemTop + 'px; ' 
						+ '"></div>';
					$('#' + id).append(legendBoxMarkup);

					var legendLabelMarkup = '<div class="legendlabel legendlabel' + (currentItem + 1) + '" '
						+ 'style="display: block; border: none; ' 
						+ 'width: ' + legendItemWidth + 'px; ' 
						+ 'position: absolute; '
						+ 'left: ' + (
							settings.plotAreaRight - legendBoxWidth - legendItemWidth - Math.round(10 * adjustment)
							) + 'px; '
						+ 'top: ' + (
							legendItemTop - Math.round(3 * adjustment)
							) + 'px; ' 
						+ 'font-family: ' + settings.fontFamily + '; '
						+ 'font-size: ' + Math.round(6 * adjustment) + 'pt; '
						+ 'line-height: 1.4; text-align: right; '
						+ 'color: ' + settings.color + '; '
						+ '">' + settings.planeLegend[currentItem] + '</div>';
					$('#' + id).append(legendLabelMarkup);

				}
			}
			
	        // add the x-axis marks and labels 
			var xAxisLineMarkup = '<div style="display: block; border: none; background-color: ' 
				+ settings.axisColor + '; ' 
				+ 'width: ' + plotAreaWidth + 'px; ' 
				+ 'height: 1px; '
				+ 'position: absolute; '
				+ 'left: ' + settings.plotAreaLeft + 'px; '
				+ 'top: ' + settings.plotAreaBottom + 'px; ' 
				+ '"></div>';
			$('#' + id).append(xAxisLineMarkup);

    		// add the label
			var xLabelMarkup = '<div class="xlabel" '
				+ 'style="display: block; border: none; ' 
				+ 'position: absolute; '
				+ 'left: ' + settings.plotAreaLeft + 'px; '
				+ 'top: ' + (settings.plotAreaBottom + (20 * adjustment)) + 'px; ' 
				+ 'font-family: ' + settings.fontFamily + '; '
				+ 'font-size: ' + Math.round(8 * adjustment) + 'pt; '
				+ 'color: ' + settings.color + '; '
				+ 'width: ' + plotAreaWidth + 'px; '
				+ 'line-height: 1.4; text-align: center; '
				+ '">' + settings.xLabel + '</div>';
			$('#' + id).append(xLabelMarkup);

	        for(var n = 0; n < (settings.xAxisValues.length); n++) {
				var i = n;
				var markWidth = (plotAreaWidth / (settings.xAxisValues.length - 1));
				var markSpacing = Math.round(i * markWidth);

				// add the axis mark
				var markMarkup = '<div style="display: block; border: none; background-color: ' 
					+ settings.axisColor + '; ' 
					+ 'width: 1px; ' 
					+ 'height: ' + settings.markWidth + 'px; '
					+ 'position: absolute; '
					+ 'left: ' + (settings.plotAreaLeft + markSpacing) + 'px; '
					+ 'top: ' + settings.plotAreaBottom + 'px; ' 
					+ '"></div>';
				$('#' + id).append(markMarkup);
				
				// add the label
				var labelMarkup = '<div class="xaxislabel xaxislabel' + (i + 1) + '" '
					+ 'style="display: block; border: none; ' 
					+ 'width: ' + Math.round(20 * adjustment) + 'px; ' 
					+ 'position: absolute; '
					+ 'left: ' + (settings.plotAreaLeft + markSpacing - Math.round(10 * adjustment)) + 'px; '
					+ 'top: ' + (settings.plotAreaBottom + Math.round(5 * adjustment)) + 'px; ' 
					+ 'font-family: ' + settings.fontFamily + '; '
					+ 'font-size: ' + Math.round(6 * adjustment) + 'pt; '
					+ 'line-height: 1.4; text-align: center; '
					+ 'color: ' + settings.color + '; '
					+ '">' + settings.xAxisValues[i] + '</div>';
				$('#' + id).append(labelMarkup);

	        }

	        // add the y-axis marks and labels 
			var yAxisLineMarkup = '<div style="display: block; border: none; background-color: ' 
				+ settings.axisColor + '; ' 
				+ 'width: 1px; ' 
				+ 'height: ' + plotAreaHeight + 'px; '
				+ 'position: absolute; '
				+ 'left: ' + settings.plotAreaLeft + 'px; '
				+ 'top: ' + settings.plotAreaTop + 'px; ' 
				+ '"></div>';
			$('#' + id).append(yAxisLineMarkup);

    		// add the label
			var yLabelX = Math.round(settings.plotAreaLeft - 250 - (40 * adjustment));
			var yLabelY = Math.round(settings.plotAreaTop + (plotAreaHeight / 2) - (10 * adjustment));

			var yLabelMarkup = '<div class="ylabel" '
				+ 'style="display: block; border: none; ' 
				+ 'transform:rotate(-90deg); -ms-transform:rotate(-90deg); -moz-transform:rotate(-90deg); -webkit-transform: rotate(-90deg); -o-transform:rotate(-90deg); ' 
				+ 'position: absolute; '
				+ 'left: ' + (settings.plotAreaLeft - 250 - (40 * adjustment)) + 'px; '
				+ 'top: ' + (settings.plotAreaTop
					+ (plotAreaHeight / 2) - (10 * adjustment)
					) + 'px; ' 
				+ 'font-family: ' + settings.fontFamily + '; '
				+ 'font-size: ' + Math.round(8 * adjustment) + 'pt; '
				+ 'color: ' + settings.color + '; '
				+ 'width: 500px; '
				+ 'font-family: ' + settings.fontFamily + '; '
				+ 'line-height: 1.4; text-align: center; '
				+ '">' + settings.yLabel + '</div>';
			$('#' + id).append(yLabelMarkup);

	        for(var n = 0; n < (settings.yAxisValues.length); n++) {
				var i = n;
				var markSpacing = Math.round(i * (plotAreaHeight 
					/ (settings.yAxisValues.length - 1)));

				// add the axis mark
				var markMarkup = '<div style="display: block; border: none; background-color: ' 
					+ settings.axisColor + '; ' 
					+ 'width: ' + settings.markWidth + 'px; ' 
					+ 'height: 1px; '
					+ 'position: absolute; '
					+ 'left: ' + (settings.plotAreaLeft - settings.markWidth) + 'px; '
					+ 'top: ' + (settings.plotAreaTop + markSpacing) + 'px; ' 
					+ '"></div>';
				$('#' + id).append(markMarkup);
				
				// add the label
				var labelMarkup = '<div class="yaxislabel yaxislabel' + (i + 1) + '" '
					+ 'style="display: block; border: none; ' 
					+ 'width: ' + Math.round(20 * adjustment) + 'px; ' 
					+ 'position: absolute; '
					+ 'left: ' + (settings.plotAreaLeft - settings.markWidth - (25 * adjustment)) + 'px; '
					+ 'top: ' + (settings.plotAreaTop + markSpacing - (5 * adjustment)) + 'px; ' 
					+ 'font-family: ' + settings.fontFamily + '; '
					+ 'font-size: ' + Math.round(6 * adjustment) + 'pt; '
					+ 'line-height: 1.4; text-align: right; '
					+ 'color: ' + settings.color + '; '
					+ '">' + settings.yAxisValues[(settings.yAxisValues.length - 1) - i] + '</div>';
				$('#' + id).append(labelMarkup);

	        }

    		// add additional text
			
			// add the header
			var headerTextMarkup = '<div class="header" '
				+ 'style="display: block; border: none; ' 
				+ 'width: ' + settings.width + 'px; ' 
				+ 'position: absolute; '
				+ 'top: ' + (15 * adjustment) + 'px; ' 
				+ 'font-family: ' + settings.fontFamily + '; '
				+ 'font-size: ' + Math.round(10 * adjustment) + 'pt; '
				+ 'line-height: 1.4; text-align: center; '
				+ 'color: ' + settings.color + '; '
				+ '">' + settings.headerText + '</div>';
			$('#' + id).append(headerTextMarkup);

	        for(var n = 0; n < settings.additionalText.length; n++) {
				var i = n;

				var textMarkup = '<div class="additional-text additional-text' + (i + 1) + '" '
					+ 'style="display: block; border: none; ' 
					+ 'position: absolute; '
					+ 'left: ' + (settings.additionalText[i][3] * adjustment) + 'px; ' 
					+ 'top: ' + (settings.additionalText[i][4] * adjustment) + 'px; ' 
					+ 'font-family: ' + settings.fontFamily + '; '
					+ 'font-size: ' + Math.round(settings.additionalText[i][1] * adjustment) + 'pt; '
					+ 'line-height: 1.4; text-align: ' + settings.additionalText[i][2][0] + '; '
					+ 'color: ' + settings.color + '; '
					+ '">' + settings.additionalText[i][0][0] + '</div>';
				$('#' + id).append(textMarkup);
	        }

			// end: main
		});
		
	};

})( jQuery );