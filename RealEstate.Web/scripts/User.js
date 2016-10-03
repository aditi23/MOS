var UserApp = angular.module('UserProfile', ['Home', 'ui.bootstrap', 'autocomplete', 'ngStorage', 'angularMoment']);
UserApp.controller("UserDetailsController", ['$scope', '$http', '$localStorage', '$sessionStorage', '$window', function ($scope, $http, $localStorage, $sessionStorage, $window) {
    var id = $sessionStorage.UserId;
    if (!id)
        $window.location.href = "/Index.html";
    $http({
        method: "GET",
        url: "http://mosrealestate.silive.in/rest/api/User/UserProfile?userId=" + id,
    }).then(function mySucces(response) {
        $scope.userDetails = response.data;
        $scope.getImages = function () {
            if ($scope.userDetails.ProfilePictureUrl)
                return $scope.userDetails.ProfilePictureUrl;
            else
                return "Images/place-holder.jpg";
        }
        console.log(response.data)
        $scope.reviewList = response.data.reviewList;
        $scope.recentlyViewed = response.data.recentlyViewed;
        $scope.followedProperty = response.data.followedProperty;
        //$localStorage.abc = 1;
        //$sessionStorage.vcb = 43;
    }, function myError(response) {
        $scope.xyz = response.statusText;
    })

}]);

app.directive('ngFiles', ['$parse', function ($parse) {

    function fn_link(scope, element, attrs) {
        var onChange = $parse(attrs.ngFiles);
        element.on('change', function (event) {
            onChange(scope, { $files: event.target.files });
        });
    };

    return {
        link: fn_link
    }
}]).controller('EditUserController', ['$scope', '$http', '$localStorage', '$sessionStorage', '$window', function ($scope, $http, $localStorage, $sessionStorage, $window) {
    var id = $sessionStorage.UserId;
    $scope.objForm = {};
    $http({
        method: "GET",
        url: "http://mosrealestate.silive.in/rest/api/User/UserProfile?userId=" + id
    }).then(function mySuccess(response) {
        console.log(response.data)
        $scope.objForm = response.data;
        $scope.getImage = function () {
            if ($scope.objForm.ProfilePictureUrl)
                return $scope.objForm.ProfilePictureUrl;
            else
                return "Images/place-holder.jpg";
        }
        something();
    }, function myError(response) {
        $scope.UserTypeList = response.statusText;
    });

    something = function () {
        console.log($scope.objForm)
        $http({
            method: "GET",
            url: "http://mosrealestate.silive.in/rest/api/User/GetAllUserType"
        }).then(function mySuccess(response) {
            console.log(response.data)
            $scope.Id = null;
            $scope.UserTypeList = response.data;
            console.log($scope.objForm.userType)
            $scope.objForm.UserType = 2;
        }, function myError(response) {
            $scope.UserTypeList = response.statusText;
        })
    }
    var formdata = new FormData();
    $scope.getProfilePic = function ($files) {
        angular.forEach($files, function (value, key) {
            formdata.append(key, value);
        });
    };
    $scope.setFile = function (element) {
        $scope.currentFile = element.files[0];
        var reader = new FileReader();

        reader.onload = function (event) {
            $scope.image_source = event.target.result
            $scope.$apply()

        }
        // when the file is read it triggers the onload event above.
        reader.readAsDataURL(element.files[0]);
    }

    $scope.regUser = function () {
        if ($scope.UserRegForm.$valid) {
            var obj = {};
            obj.Id = id;
            obj.FirstName = $scope.objForm.FirstName;
            obj.LastName = $scope.objForm.LastName;
            obj.EmailId = $scope.objForm.EmailId;
            obj.Contact = $scope.objForm.Contact;
            obj.City = $scope.objForm.City;
            obj.UserTypeId = $scope.objForm.UserType.Id;
            console.log(obj.UserTypeId)
            console.log($scope.objForm.UserType)

            $http({
                method: "PUT",
                url: "http://mosrealestate.silive.in/rest/api/User/EditProfile",
                data: obj,
                headers: { 'Content-Type': 'application/json' }
            }).then(function mySuccess(response) {
                $scope.User = response.data;
                console.log(response.data)
            }, function myError(response) {
                //alert(response.statusText);
            })
        }
        $window.location.href = "/User.html";
    }
}]);

