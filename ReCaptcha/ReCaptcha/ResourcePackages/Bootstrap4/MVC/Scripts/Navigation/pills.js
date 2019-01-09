(function ($) {
    $('.expandable-node').on('click', function (item) {
        var target = $(item.currentTarget);
        if (window.DataIntelligenceSubmitScript && target.attr('aria-expanded') === 'false') {
            DataIntelligenceSubmitScript._client.sentenceClient.writeSentence({
                predicate: "Toggle navigation",
                object: target.text().trim(),
                objectMetadata: [{
                    'K': 'PageTitle',
                    'V': document.title
                },
                {
                    'K': 'PageUrl',
                    'V': location.href
                }
                ]
            });
        }
    })
}(jQuery));