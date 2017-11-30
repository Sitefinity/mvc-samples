(function () {
    window.vote = function (e) {
        e.preventDefault();

        var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
        $.post(sf_appPath + 'web-interface/books/vote/' + dataItem.Title, function (data) {
            $('#booksGrid').data('kendoGrid').dataSource.read();
            $("#booksGrid").data('kendoGrid').refresh();
        });
    };
}());