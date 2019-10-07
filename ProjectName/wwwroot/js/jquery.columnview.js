/**
 *  jQuery Column Navigation Plugin
 *	
 *	version 1.0.0
 *	
 *	Written by Sam Clark
 *	http://sam.clark.name
 *	
 *
 *	!!! NOTICE !!!
 *	This library and related library requires jQuery 1.2.6 or higher
 *	http://www.jquery.com
 *
 *	This library requires the ScrollTo plugin for jQuery by Flesler
 *	http://plugins.jquery.com/project/ScrollTo
 *
 *	The MIT License
 *
 *	Copyright (c) 2008 Polaris Digital Limited
 *
 *	Permission is hereby granted, free of charge, to any person obtaining a copy
 *	of this software and associated documentation files (the "Software"), to deal
 *	in the Software without restriction, including without limitation the rights
 *	to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *	copies of the Software, and to permit persons to whom the Software is
 *	furnished to do so, subject to the following conditions:
 *
 *	The above copyright notice and this permission notice shall be included in
 *	all copies or substantial portions of the Software.
 *
 *	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *	IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *	FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *	AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *	LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *	OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 *	THE SOFTWARE.
 *
 **/

(function($) {
    $.fn.columnNavigation = function(configuration) {
        // Setup the column navigation object with configuration settings
        // Overright existing settings where applicable
        configuration = $.extend({
            containerPosition: "absolute",
            containerPadding: "0",
            containerMargin: "0",
            containerWidth: "91%",
            containerHeight: "74%",
            containerBackgroundColor: "",
            columnFontFamily: "'Segoe UI Light', 'Lucida Grande', Verdana, Arial, Helvetica, sans-serif",
            columnFontSize: "100%",
            columnSeperatorStyle: "3px solid gainsboro",
            columnDeselectFontWeight: "normal",
            columnDeselectColor: "#666",
            columnDeselectBackgroundColor: "",
            columnSelectFontWeight: "normal",
            columnSelectColor: "black",
            columnSelectBackgroundColor: "#C3E7F7",
            columnItemPadding: "5px",
            columnItemMarginLeft: "5px",
            columnItemMarginRight: "2px",
            columnScrollVelocity: 200,
        }, configuration);

        // Setup the container space using the settings
        $(this).css({
            position: configuration.containerPosition,
            padding: configuration.containerPadding,
            margin: configuration.containerMargin,
            width: configuration.containerWidth,
            height: configuration.containerHeight,
            backgroundColor: configuration.containerBackgroundColor,
            overflowX: "auto",
            overflowY: "hidden",
            fontSize: "14px",
            paddingTop: "20px"
        });

        // LI element deselect state
        var liDeselect = {
            backgroundColor: configuration.containerBackgroundColor,
            borderLeft: "0",
            paddingLeft: "5px",
            backgroundImage: "none"
        };

        // LI element select state
        var liSelect = {
            backgroundColor: configuration.columnSelectBackgroundColor,
            color: configuration.columnSelectColor,
            paddingLeft: "5px",
            backgroundImage: "none"
        };

        // A element deselect state
        var aDeselect = {
            color: configuration.columnDeselectColor,
            fontFamily: configuration.columnFontFamily,
            fontSize: configuration.columnFontSize,
            textDecoration: "none",
            fontWeight: "normal",
            outline: "none",
            width: "100%",
            backgroundImage: "none"
        };

        // A element select state
        var aSelect = {
            color: configuration.columnSelectColor,
            textDecoration: "none",
            backgroundImage: "none"
        };

        // Setup the column width as a string (for CSS)
        var columnWidth = (($(this).width() / 3) - 3) + "px";

        // Hide and layout children beneath the first level
        $(this).find("ul li").find("ul").css({
            left: columnWidth,
            top: "0px",
            position: "absolute"
        }).hide();

        // Style the columns
        $(this).find("ul").css({
            position: "absolute",
            width: columnWidth,
            height: "100%",
            borderRight: configuration.columnSeperatorStyle,
            padding: "0",
            margin: "0"
        });

        // Ensure each level can scroll within the container
        $(this).find("ul div").css({
            height: "100%",
            overflowX: "hidden",
            overflowY: "auto"
        });

        // Style the internals
        $(this).find("ul li").css({
            listStyle: "none",
            padding: configuration.columnItemPadding,
            marginLeft: configuration.columnItemMarginLeft,
            marginRight: configuration.columnItemMarginRight,
            backgroundColor: configuration.columnDeselectBackgroundColor,
            cursor: "pointer"
        });

        // Style the unselected links (this overrides specific CSS styles on the PageInfo)
        $(this).find("ul li a").css(
            aDeselect
        );

        // Setup the onclick function for each link within the tree
        $(this).find("ul li").on("click", function() {

            // Hide lower levels
            $(this).siblings().find("ul").hide();

            // Deselect other levels
            $(this).siblings().css(liDeselect);

            // Deselect other levels children
            $(this).siblings().find("li").css(liDeselect);

            // Deselect other a links
            $(this).siblings().find("a").css(aDeselect);

            // Show child menu
            $(this).find("ul:first").show();

            // Select this level
            $(this).css(liSelect);

            if ($(this).parent().find("ul:first").attr('id') === "right-menu") {
                $(this).parent().find("ul").css({ borderRight: "0" });
            }
            
            $("#display-search").focus();

            return false;
        });

        $(this).find("ul li a").click(function () {
            if ($(this).attr("href") !== undefined) {
                window.location = $(this).attr("href");
            }
        });
    };
})(jQuery);