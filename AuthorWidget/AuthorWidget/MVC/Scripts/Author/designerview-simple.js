(function ($) {
    var simpleViewModule = angular.module('simpleViewModule', ['expander', 'designer', 'sfFields', 'sfSelectors']);
    angular.module('designer').requires.push('simpleViewModule');
    angular.module('designer').value('cssClasses', {
        'Default': [
            { 'value': 'blue', 'title': 'Blue box Default' },
            { 'value': 'red', 'title': 'Red box Default' }
        ],
        'djvadjva': [
            { 'value': 'blue', 'title': 'Blue box djvadjva' },
            { 'value': 'red', 'title': 'Red box djvadjva' }
        ]
    });
})(jQuery);