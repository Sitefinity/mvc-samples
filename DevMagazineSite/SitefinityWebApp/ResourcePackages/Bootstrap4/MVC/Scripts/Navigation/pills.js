(function () {
    /* Polyfills */

    if (window.NodeList && !NodeList.prototype.forEach) {
        NodeList.prototype.forEach = Array.prototype.forEach;
    }

    document.addEventListener('DOMContentLoaded', function () {
        document.querySelectorAll('.expandable-node').forEach(function (x) {
            x.addEventListener('click', function (e) {
                if (window.DataIntelligenceSubmitScript && e.currentTarget.hasAttribute('aria-expanded') === 'false') {
                    DataIntelligenceSubmitScript._client.sentenceClient.writeSentence({
                        predicate: "Toggle navigation",
                        object: e.currentTarget.innerText.trim(),
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
            });
        });
    });

}());