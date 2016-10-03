var UserApp = angular.module('BuilderProfile', ['Home', 'ui.bootstrap', 'autocomplete', 'ngStorage']);

UserApp.controller("BuilderDetailsController", ['$scope', '$http', '$localStorage', '$sessionStorage', function ($scope, $http, $localStorage, $sessionStorage) {
    var id = $sessionStorage.UserId;
    $http({
        method: "GET",
        url: "http://localhost:19342/api/Builder/BuilderDetails?builderId=" + id,
    }).then(function mySucces(response) {
        $scope.builderDetails = response.data;
        console.log(response.data)
        $scope.reviewList = response.data.Reviews;
        $scope.recentlyViewed = response.data.Projects;
    }, function myError(response) {
        $scope.xyz = response.statusText;
    })
    // add builder comment
    $scope.AddComment = function (reviewId) {
        $http({
            method: "POST",
            url: "http://localhost:19342/api/Builder/AddComment",
            data: {
                ReviewId: reviewId,
                Text: $scope.builderComment,
                ProjectId: $scope.review.projectId
            }
        }).then(function mySucces(response) {
            $scope.builderDetails = response.data;
            console.log(response.data)
            $scope.reviewList = response.data.Reviews;
            $scope.recentlyViewed = response.data.Projects;
        }, function myError(response) {
            $scope.xyz = response.statusText;
        })
    }
}]);

app.controller('EditBuilderController', ['$scope', '$http', '$localStorage', '$sessionStorage', function ($scope, $http, $localStorage, $sessionStorage) {
    var id = $sessionStorage.UserId;
    $http({
        method: "GET",
        url: "http://localhost:19342/api/Builder/BuilderDetails?builderId=" + id,
    }).then(function mySucces(response) {
        $scope.objBuilder = response.data;
        console.log(response.data)
    }, function myError(response) {
        $scope.xyz = response.statusText;
    })
    var objBuilder = {};
    $scope.EditBuilder = function () {
        if ($scope.formBuilderRegister.$valid) {

            $http({
                method: "POST",
                url: 'http://localhost:19342/api/Builder/EditProfile',
                data: $scope.objBuilder, //forms builder object
                headers: { 'Content-Type': 'application/json' }
            }).
          then(function mySucces(response) {
              $scope.status = response.statusText;
          }, function myError(response) {
              $scope.status = response.statusText;
          })
        }
    }
}]);


