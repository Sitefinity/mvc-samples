(function ($) {
    var designerModule = angular.module('designer');

    //This is basic controller for the "ManageItems" designer view.
    designerModule.controller('ManageItemsCtrl', ['$scope', 'propertyService', function ($scope, propertyService) {
        $scope.feedback.showLoadingIndicator = true;
        $scope.newListItem = "";

        //Adds item to list.
        $scope.addListItem = function () {
            $scope.listItems.push($scope.newListItem);
            $scope.properties.ListItems.PropertyValue = JSON.stringify($scope.listItems);
            $scope.newListItem = "";
        };

        //Deletes the selected item from the list.
        $scope.deleteListItem = function (index) {
            $scope.listItems.splice(index, 1);
            $scope.properties.ListItems.PropertyValue = JSON.stringify($scope.listItems);
        };

        //Makes call to the controlPropertyService to get the properties for the widgets.
        propertyService.get()
            .then(function (data) {
                if (data) {
                    $scope.properties = propertyService.toAssociativeArray(data.Items);
                    $scope.listItems = $.parseJSON($scope.properties.ListItems.PropertyValue);
                }
            },
            function (data) {
                $scope.feedback.showError = true;
                if (data)
                    $scope.feedback.errorMessage = data.Detail;
            })
            .finally(function () {
                $scope.feedback.showLoadingIndicator = false;
            });
    }]);
})(jQuery);