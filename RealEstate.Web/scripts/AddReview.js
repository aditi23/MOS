var app = angular.module('AddReview', ['ngStorage','awesome-rating']);
app.controller('AddReviewController', ['$scope', '$localStorage', '$sessionStorage', '$window', '$http', '$filter', function ($scope, $localStorage, $sessionStorage, $window, $http, $filter) {
    $scope.type = $sessionStorage.UserType;
    var userId = $sessionStorage.UserId;
    var projectId = $sessionStorage.ProjectId;

    $scope.rating = 0;
    $scope.ratings = [{
        label: "Connectivity",
        current: 3,
        max: 5
    }, {
        label: "Livability",
        current: 3,
        max: 5
    }, {
        label: "Lifestyle",
        current: 3,
        max: 5
    }, {
        label: "Amenities",
        current: 3,
        max: 5
    }];
    var arr = [];
    $scope.getSelectedRating = function (rating) {
       console.log(rating);
        var a = rating;
        arr.push(a);
        //console.log(arr);
        
    }
    
    $scope.objReview = {};
    
    $scope.addReview = function () {
        if ($scope.SelectedMonth == 1) {
                       var someDate = new Date();
                        var dd = someDate.getDate() - 1;
                        var mm = someDate.getMonth() - 1;
                        var y = someDate.getFullYear();
                        var someFormattedDate = dd + '/' + mm + '/' + y;
                        $scope.SelectedMonth = someFormattedDate;
                    }
                if ($scope.SelectedMonth == 2) {
                        var someDate = new Date();
                        var dd = someDate.getDate() - 3;
                        var mm = someDate.getMonth() - 1;
                        var y = someDate.getFullYear();
                        var someFormattedDate = dd + '/' + mm + '/' + y;
                        $scope.SelectedMonth = someFormattedDate;
                    }
                if ($scope.SelectedMonth == 3) {
                        var someDate = new Date();
                        var dd = someDate.getDate();
                        var mm = someDate.getMonth()-2;
                        var y = someDate.getFullYear();
                        var someFormattedDate = dd + '/' + mm + '/' + y;
                        $scope.SelectedMonth = someFormattedDate;
                    }
                if ($scope.SelectedMonth == 4) {
                        var someDate = new Date();
                        var dd = someDate.getDate();
                        var mm = someDate.getMonth() -4;
                        var y = someDate.getFullYear();
                        var someFormattedDate = dd + '/' + mm + '/' + y;
                        $scope.SelectedMonth = someFormattedDate;
                    }
                if ($scope.SelectedMonth == 5) {
                        var someDate = new Date();
                        var dd = someDate.getDate();
                        var mm = someDate.getMonth() - 1;
                        var y = someDate.getFullYear()-1;
                        var someFormattedDate = dd + '/' + mm + '/' + y;
                        $scope.SelectedMonth = someFormattedDate;
                    }
                if ($scope.SelectedMonth == 6) {
                        var someDate = new Date();
                        var dd = someDate.getDate()-5;
                        var mm = someDate.getMonth() - 2;
                        var y = someDate.getFullYear() - 1;
                        var someFormattedDate = dd + '/' + mm + '/' + y;
                        $scope.SelectedMonth = someFormattedDate;
                    }

            var recaptcha = $("#g-recaptcha-response").val();
            if (recaptcha === "") {
                event.preventDefault();
                alert("Please check the recaptcha");
            }
        $scope.TextRequired = '';
        $scope.headingRequired = '';
        $scope.MonthRequired = '';
        $scope.LivabilityRequired = '';
        console.log(rating);
        console.log($scope.rating1);

        if (!$scope.objReview.Text)
            $scope.TextRequired = "*";
        if (!$scope.objReview.Heading)
            $scope.HeadingRequired = "*";
        if ($scope.SelectedMonth)
            $scope.MonthRequired = "*";
       
        //if (!arr[0])
        //    $scope.ConnectivityRequired = "*";
        //if (!arr[])
        //    $scope.LivabilityRequired = "*";
        //if (!$scope.objReview.Lifestyle)
        //    $scope.LifestyleRequired = "*";
        //if (!$scope.objReview.Amenity)
        //    $scope.AmenityRequired = "*";
        if ($scope.objReview.Text && $scope.objReview.Heading && $scope.SelectedMonth) {
            var obj = {};
            obj.ProjectId=projectId;
            obj.UserId=userId;
            obj.Text=$scope.objReview.Text;
            obj.lastVisited = $scope.SelectedMonth;
            obj.Heading=$scope.objReview.Heading;
            obj.reviewRatingDetails=[
            { MasterReviewId: 1, Value:arr[0] },
            { MasterReviewId: 2, Value: arr[1] },
            { MasterReviewId: 3, Value: arr[2] },
            { MasterReviewId: 4, Value: arr[3] }
            ];
            $http({
                method: 'POST',
                url: 'http://localhost:19342/api/Review/AddReview',
                data: obj, //forms user object
                headers: { 'Content-Type': 'application/json' }
            }).
      then(function mySucces(response) {
          $scope.objUser = data;
      }, function myError(response) {
          $scope.LastVisited = response.statusText;
      })
        }
    };
}]);

app.directive('starRating', function () {
    return {
        restrict: 'A',
        template: '<ul class="rating">' +
            '<li ng-repeat="star in stars" ng-class="star" ng-click="toggle($index)">' +
            '\u2605' +
            '</li>' +
            '</ul>',
        scope: {
            ratingValue: '=',
            max: '=',
            onRatingSelected: '&'
        },
        link: function (scope, elem, attrs) {

            var updateStars = function () {
                scope.stars = [];
                for (var i = 0; i < scope.max; i++) {
                    scope.stars.push({
                        filled: i < scope.ratingValue
                    });
                }
            };

            scope.toggle = function (index) {
                
                scope.ratingValue = index + 1;
                scope.onRatingSelected({
                    rating: index + 1
                });
            };

            scope.$watch('ratingValue', function (oldVal, newVal) {
                if (newVal) {
                    updateStars();
                }
            });
        }
    }
});
app.controller('RecentReviewController', ['$scope', '$localStorage', '$sessionStorage', '$window', '$http', function ($scope, $localStorage, $sessionStorage, $window, $http) {
    var projectId = $sessionStorage.ProjectId;
    $http({
        method: "GET",
        url: "http://localhost:19342/api/Review/RecentReviews?projectId=" + projectId
    }).then(function mySucces(response) {
        $scope.RecentReviews = response.data;
    }, function myError(response) {
        $scope.LastVisited = response.statusText;
    })
}]);