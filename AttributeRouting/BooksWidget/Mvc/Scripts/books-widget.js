(function ($) {
    var initializeBooksWidget = function (element) {
        var widget = $(element);
        var currentPage = widget.find('input[data-role=current-page]').val();
        var pointSpans = widget.find('span[data-role=points]');

        $.get(sf_appPath + 'web-interface/books/points/' + currentPage, function (data) {
            for (var i = 0; i < pointSpans.length && i < data.length; i++) {
                $(pointSpans[i]).html(data[i]);
            }
        });

        widget.find('a[data-role=vote-link]').each(function (index, value) {
            var link = $(value);
            var id = index + (currentPage - 1) * 5; // PageSize is a constant 5
            var idx = index;
            link.click(function () {
                $.post(sf_appPath + 'web-interface/books/vote/' + id, function (data) {
                    $(pointSpans[idx]).html(data);
                });

                return false;
            });
        });
    };

    $(function () {
        $('div[data-role=books-widget]').each(function (index, value) {
            initializeBooksWidget(value);
        });
    });
})(jQuery)
