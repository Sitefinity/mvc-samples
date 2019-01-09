(function ($) {
    var simpleViewModule = angular.module('simpleViewModule', ['designer', 'kendo.directives', 'sfFields', 'sfSelectors']);
    angular.module('designer').requires.push('simpleViewModule');

    //basic controller for the simple designer view
    simpleViewModule.controller('SimpleCtrl', ['$scope', 'propertyService',
        function ($scope, propertyService) {
            $('.modal-dialog').scope().size = 'lg';

            // ------------------------------------------------------------------------
            // event handlers
            // ------------------------------------------------------------------------

            var onGetPropertiesSuccess = function (data) {
                $scope.properties = propertyService.toAssociativeArray(data.Items);
                kendo.bind();
            };

            // ------------------------------------------------------------------------
            // scope variables and set up
            // ------------------------------------------------------------------------

            $scope.feedback.showLoadingIndicator = true;

            propertyService.get()
                .then(onGetPropertiesSuccess)
                .catch(function (data) {
                    $scope.feedback.showError = true;
                    if (data)
                        $scope.feedback.errorMessage = data.Detail;
                })
                .finally(function () {
                    $scope.feedback.showLoadingIndicator = false;
                });
        }
    ]);
})(jQuery);
