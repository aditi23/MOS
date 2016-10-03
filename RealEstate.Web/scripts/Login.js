//var app = angular.module('mainpopup', ['ngstorage']);
//app.controller('LoginC', ['$scope', '$http', '$localstorage', '$sessionstorage', '$window', function ($scope, $http, $localstorage, $sessionstorage, $window) {
//    $scope.obj = {};
//   // console.log($scope.objuser)
//    $scope.saveData = function () {
//        $http({
//            method: 'post',
//            url: 'http://localhost:19342/api/user/login',
//            data: $scope.objuser, //forms user object
//            headers: { 'content-type': 'application/json' }
//        }).then(function mysuccess(response) {
//            $scope.id = response.data;
//            if ($scope.id) {
//                console.log(response.data)
//                $scope.similarprojects = response.data;
//                //$localstorage.pmc = 4;
//                $sessionstorage.pmc = "aditi";
//                $window.location.href = '/welcome.html';
//            }
//            else
//                alert('incorrect username or password');

//        }, function myerror(response) {
//            $scope.similarprojects = response.statustext;
//        })
//        //}
//    }
//}]);
