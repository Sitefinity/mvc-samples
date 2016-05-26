// DevMagazine main logic
(function ($) {
    // handle ajax requests for endless paging
    function endlessPagingHandler(url, $appendContainer, resultContainerSelector) {

        $.get(url, function (data) {
            var result = (resultContainerSelector == null) ? $(data) : $(data).find(resultContainerSelector).html();

            $('#paging-wrapper').hide('slow', function () { $('#paging-wrapper').remove(); });

            // get the offset
            var targetPosition = $appendContainer.offset().top + $appendContainer.height() - $('#paging-wrapper').height() - $('nav[role="navigation"]').height();

            // append the news
            $appendContainer.append(result);

            // do the animation
            $("html body").animate({ scrollTop: targetPosition }, 500);
        });
    }

    // handle search input behaviour
    $('.Search input[type="search"]').on('keydown', function (e) {
        if (e.keyCode == 13) {
            if ($(this).val().length > 0) {
                var searchUrl = $(this).data('url') + encodeURIComponent($(this).val());
                window.location.href = searchUrl;
            }
        }
    });

    // handle articles endless paging
    $('#showMoreArticles').on('click', function (e) {
        e.preventDefault();
        var button = $(this).hide(),
        pageNum = Number(button.attr("data-page")),
        url = button.attr("data-url"),
        ajaxLoader = $('#endless-paging').show(),
        m = url + "/" + (pageNum + 1) + "/";

        endlessPagingHandler(m, $("#news-container"), "#news-container");
    });

    // handle search endless paging
    $('#showMoreResults').on('click', function (e) {
        e.preventDefault();
        var button = $(this).hide(),
        pageNum = Number(button.attr("data-page")),
        searchTerm = button.data("query")
        url = button.attr("data-url"),
        ajaxLoader = $('#endless-paging').show(),
        m = url + "?page=" + (pageNum + 1) + "&query=" + encodeURIComponent(searchTerm);

        endlessPagingHandler(m, $("#search-results"), "#search-results");
    });
	
	$('#showMoreEvents').on('click', function (e) {
        e.preventDefault();
        var button = $(this).hide(),
        pageNum = Number(button.attr("data-page")),
        url = button.attr("data-url"),
        ajaxLoader = $('#endless-paging').show(),
        m = url + "/" + (pageNum + 1) + "/";

        endlessPagingHandler(m, $("#events-container"), "#events-container");
    });

    // handle issues' endless paging
    $('#showMoreIssues').on('click', function (e) {
        e.preventDefault();
        var button = $(this).hide(),
        pageNum = Number(button.attr("data-page")),
        url = button.attr("data-url"),
        ajaxLoader = $('#endless-paging').show(),
        m = url + "/archive/" + (pageNum + 1);

        endlessPagingHandler(m, $("#issues-list"), null);
    });

    function searchToggle() {
      var $searchTerm = $(".js-search-term");
      var $searchField = $(".js-search-field")

      if ($searchTerm.length > 0) {
        $searchField.focus().val($searchTerm.html()); 
      }
    }

    searchToggle();

})(jQuery);
