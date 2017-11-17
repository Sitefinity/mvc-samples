(function ($) {
    var initializeBooksWidget = function (element) {
        var sf_appPath = window.sf_appPath || "/";
        var widget = $(element);
        var pointSpans = widget.find('span[data-role=points]');

        widget.find($('div[data-role=book-item]')).each(function (index, item) {
            var item$ = $(item);
            var id = item$.find('input[data-role=book-id]').val();
            var link = item$.find('a[data-role=vote-link]');

            link.click(function () {
                $.post(sf_appPath + 'web-interface/books/vote/' + id, function (data) {
                    $(pointSpans[index]).html(data);
                });

                return false;
            });
        });

        var currentPage = widget.find('input[data-role=current-page]').val();
        if (currentPage) {
            $.get(sf_appPath + 'web-interface/books/points/' + currentPage, function (data) {
                for (var i = 0; i < pointSpans.length && i < data.length; i++) {
                    $(pointSpans[i]).html(data[i]);
                }
            });
        }
    };

    $(function () {
        $('div[data-role=books-widget]').each(function (index, value) {
            initializeBooksWidget(value);
        });
    });
})(jQuery);
