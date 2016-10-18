var context = {
     uiCulture: 'en'
 };
  
 angular.module("app", ["sfSelectors", "modalDialog"])
 .controller("MainCtrl", ["$scope", function ($scope) {
     $scope.dynamicType = "Telerik.Sitefinity.DynamicTypes.Model.TestModule.SomeType";
     $scope.context = context;
 }]);
  
 //Set a server context that will be used by the serverContextProvider.
 angular.module('sfServices').config(['serverContextProvider', function (serverContextProvider) {        
     serverContextProvider.setServerContext(context);
 }])