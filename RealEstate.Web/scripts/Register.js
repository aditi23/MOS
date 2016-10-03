var app = angular.module('BuilderRegistrationApp', ['ngStorage']);
app.controller('MyCtrl1', ['$scope', '$http', function ($scope, $http) {
    $scope.objUser = {};
    $scope.saveData = function () {
        $scope.FirstNameRequired = '';
        $scope.LastNameRequired = '';
        $scope.EmailRequired = '';
        $scope.ContactRequired = '';
        $scope.PasswordRequired = '';
        $scope.CityRequired = '';
        if (!$scope.objUser.FirstName)
            $scope.FirstNameRequired = "Required";
        if (!$scope.objUser.LastName)
            $scope.LastNameRequired = "Required";
        if (!$scope.objUser.EmailId)
            $scope.EmailIdRequired = "Required";
        if (!$scope.objUser.Contact)
            $scope.ContactRequired = "Required";
        if (!$scope.objUser.Password)
            $scope.PasswordRequired = "Required";
        if (!$scope.objUser.City)
            $scope.CityRequired = "Required";
        if ($scope.objUser.FirstName && $scope.objUser.LastName && $scope.objUser.EmailId && $scope.objUser.Contact && $scope.objUser.Password && $scope.objUser.City) {
            $http({
                method: 'POST',
                url: 'http://mosrealestate.silive.in/rest/api/User/RegisterUser',
                data: $scope.objUser, //forms user object
                headers: { 'Content-Type': 'application/json' }
            }).
      success(function (data) {
          $scope.objUser = data;
      })
        }
    };
}]);


app.controller('BuilderRegistrationController', ['$scope', '$http', '$window', function ($scope, $http, $window) {
    $scope.objBuilder = {};
    if ($scope.formBuilderRegister.$valid) {
        $scope.RegisterBuilder = function () {
            console.log($scope.objBuilder)
            $http({
                method: "POST",
                url: 'http://mosrealestate.silive.in/rest/api/Builder/Register',
                data: $scope.objBuilder, //forms builder object
                headers: { 'Content-Type': 'application/json' }
            }).
          then(function mySucces(response) {
              $scope.builerId = response.data;$scope.builerId;
              $sessionStorage.UserType = "Builder";
              $sessionStorage.UserId = $scope.builerId;
          }, function myError(response) {
              $scope.status = response.statusText;
          })
        }
    }
}]);
//Builder Registration
app.controller('RegisterBuilderController', ['$scope', '$http', '$localStorage', '$sessionStorage', '$window', function ($scope, $http, $localStorage, $sessionStorage, $window) {
    $scope.objBuilder = {};
    $scope.regUser = function () {
        if ($scope.formBuilderRegister.$valid) {
            console.log($scope.objBuilder)
            $http({
                method: "POST",
                url: 'http://mosrealestate.silive.in/rest/api/Builder/Register',
                data: $scope.objBuilder, //forms builder object
                headers: { 'Content-Type': 'application/json' }
            }).
          then(function mySucces(response) {
              $scope.builerId = response.data;
              $sessionStorage.UserType = "Builder";
              $sessionStorage.UserId = $scope.builerId;
          }, function myError(response) {
              $scope.status = response.statusText;
          })
        }
    }
}]);