(function ($) {
    var designerModule = angular.module('designer');

    //This is basic controller for the "ManageItems" designer view.
    designerModule.controller('ManageItemsCtrl', ['$scope', 'propertyService', 'dialogFeedbackService', function ($scope, propertyService, dialogFeedbackService) {
        $scope.Feedback = dialogFeedbackService;
        $scope.Feedback.ShowLoadingIndicator = true;
        $scope.newListItem = "";

        //Adds item to list.
        $scope.AddListItem = function () {
            $scope.ListItems.push($scope.newListItem);
            $scope.Properties.ListItems.PropertyValue = JSON.stringify($scope.ListItems);
            $scope.newListItem = "";
        };

        //Deletes the selected item from the list.
        $scope.DeleteListItem = function (index) {
            $scope.ListItems.splice(index, 1);
            $scope.Properties.ListItems.PropertyValue = JSON.stringify($scope.ListItems);
        };

        //Makes call to the controlPropertyService to get the properties for the widgets.
        propertyService.get()
            .then(function (data) {
                if (data) {
                    $scope.Properties = propertyService.toAssociativeArray(data.Items);
                    $scope.ListItems = $.parseJSON($scope.Properties.ListItems.PropertyValue);
                }
            },
            function (data) {
                $scope.Feedback.ShowError = true;
                if (data)
                    $scope.Feedback.ErrorMessage = data.Detail;
            })
            .finally(function () {
                $scope.Feedback.ShowLoadingIndicator = false;
            });
    }]);
})(jQuery);