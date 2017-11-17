(function ($) {
    var initializeBooksWidget = function (element) {
        var sf_appPath = window.sf_appPath || "/";
        var widget$ = $(element);

        widget$.find($('div[data-role=book-item]')).each(function (index, item) {
            var item$ = $(item);
            var id = item$.find('input[data-role=book-id]').val();
            var link$ = item$.find('a[data-role=vote-link]');
            var points$ = item$.find('span[data-role=points]');

            $.get(sf_appPath + 'web-interface/books/points/' + id, function (data) {
                points$.html(data);
            });

            link$.click(function () {
                $.post(sf_appPath + 'web-interface/books/vote/' + id, function (data) {
                    points$.html(data);
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
})(jQuery);
