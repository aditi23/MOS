var app = angular.module("Home", ['ui.bootstrap', 'autocomplete', 'ngStorage', 'ngDialog', 'ngMaterial']);

app.controller("TrendingReviewsController", function ($scope, $http) {
    $http.get('http://mosrealestate.silive.in/rest/api/Search/TrendingReviews').
    then(function mySucces(response) {
        $scope.Trending = response.data;
        console.log(response.data)
    }, function myError(response) {
        alert("error");
    });
    $http.get('http://mosrealestate.silive.in/rest/api/Search/SiteStats').
   then(function mySucces(response) {
       $scope.SiteStats = response.data;
   }, function myError(response) {
       alert("error");
   });
    console.log($scope.SiteStats)
    $scope.getReviewCount = function () {
        if ($scope.SiteStats) {
            if ($scope.SiteStats.reviewCount)
                return $scope.SiteStats.reviewCount;
            else
                return 0;
        }
        else
            return 0;
    }
    $scope.getProjectCount = function () {
        if ($scope.SiteStats) {
            if ($scope.SiteStats.projectCount)
                return $scope.SiteStats.projectCount;
            else
                return 0;
        }
        else
            return 0;
    }
    $scope.getUserCount = function () {
        if ($scope.SiteStats) {
            if ($scope.SiteStats.userCount)
                return $scope.SiteStats.userCount;
            else
                return 0;
        }
        else
            return 0;
    }
    $scope.getBuilderCount = function () {
        if ($scope.SiteStats) {
            if ($scope.SiteStats.builderCount)
                return $scope.SiteStats.builderCount;
            else
                return 0;
        }
        else
            return 0;
    }
});

app.controller("NavBar", ['$scope', '$http', '$localStorage', '$sessionStorage', '$window', function ($scope, $http, $localStorage, $sessionStorage, $window) {
    if ($sessionStorage.UserId && $sessionStorage.UserType == "User") {
        console.log($sessionStorage.UserId)
        $http({
            method: 'GET',
            url: 'http://mosrealestate.silive.in/rest/api/User/GetUserFirstName?userId=' + $sessionStorage.UserId,
            data: { userId: $sessionStorage.UserId },
            headers: { 'Content-Type': 'application/json' }
        }).
        then(function mySucces(response) {
            $scope.Email = "Welcome " + response.data;
            console.log(response.data);
        }, function myError(response) {
            alert("error");
        });
    }
    else if ($sessionStorage.UserId && $sessionStorage.UserType == "Builder") {
        console.log($sessionStorage.UserId)
        $http({
            method: 'GET',
            url: 'http://mosrealestate.silive.in/rest/api/Builder/BuilderDetails?builderId=' + $sessionStorage.UserId,
            headers: { 'Content-Type': 'application/json' }
        }).
        then(function mySucces(response) {
            $scope.Email = "Welcome " + response.data.CompanyName;
            console.log(response.data);
        }, function myError(response) {
            alert("error");
        });
    }
    else {
        $scope.Email = "LOGIN"
    }
    $scope.checkLogin = function () {
        if ($scope.Email == "LOGIN")
            return true;
        else
            return false;
    }

    $scope.myFunc = function () {
        $sessionStorage.url = window.location.pathname;
        console.log($sessionStorage.url)
        if ($scope.Email == "LOGIN")
            $window.location.href = 'RegisterUser.html';
        else if ($sessionStorage.UserType == "Builder")
            $window.location.href = 'Builder.html';
        else
            $window.location.href = 'User.html';

    }
    $scope.logout = function () {
        $localStorage.$reset()
        $sessionStorage.$reset();
        $scope.Email = "LOGIN"
        $window.location.href = 'Index.html';
    }
}]);

app.controller("ProjectController", ['$scope', '$http', '$localStorage', '$sessionStorage', '$window', '$timeout', '$q', '$log', 'filterFilter', function ($scope, $http, $localStorage, $sessionStorage, $window, $timeout, $q, $log, filterFilter) {
    var CityId = {};
    $scope.AllProperty = [];
    var self = this;


    $http.get('http://mosrealestate.silive.in/rest/api/Search/GetAllCities').
   then(function mySucces(response) {
       $scope.City = response.data;
       //CityId = response.data.map(function (a) { return a.Id; });
   }, function myError(response) {
       alert("error");
       $scope.SelectedCity = $sessionStorage.SelectedCity;
   });



    $scope.changedValue = function (obj) {
        CityId = obj;
        $sessionStorage.SelectedCity = CityId;
        $http({
            method: 'GET',
            url: 'http://mosrealestate.silive.in/rest/api/Search/SearchByCity?cityId=' + CityId,
            //data: JSON.stringify(params),
            headers: { 'Content-Type': 'application/json' }
        }).
                        then(function mySucces(response) {
                            console.log(response.data)
                            if (response.data != "No data found.") {
                                console.log(response.data)
                                $scope.AllProperty = response.data;
                                self.simulateQuery = false;
                                self.isDisabled = false;
                                self.noCache = true;
                                self.states = loadStates($scope.AllProperty);
                                self.querySearch = querySearch;
                                self.selectedItemChange = selectedItemChange;
                                self.searchTextChange = searchTextChange;
                                self.newState = newState;
                                console.log($scope.AllProperty)
                            }
                        }, function myError(response) {
                            alert("error");
                        });
        $scope.SelectedCity = $sessionStorage.SelectedCity;
    }
    var projectid = "";
    var Type = "";

    function newState(state) {
        alert("This functionality is yet to be implemented!");
    }
    function querySearch(query) {
        console.log(self.states)
        var results = query ? self.states.filter(createFilterFor(query)) : self.states, deferred;
        if (self.simulateQuery) {
            deferred = $q.defer();
            $timeout(function () {
                deferred.resolve(results);
            },
           Math.random() * 1000, false);
            return deferred.promise;
        } else {
            return results;
        }
    }
    function searchTextChange(text) {
        return GetCountryService.getCountry(text);
        $log.info('Text changed to ' + text);
    }
    function selectedItemChange(item) {
        console.log('Item changed to ' + JSON.stringify(item));
    }
    //build list of states as map of key-value pairs
    function loadStates(something) {
        console.log(something.City.Id);
        var states = something;
        return states.map(function (repo) {
            repo.value = repo.Name.toLowerCase();
            return repo;
        });
    }
    console.log(self.states)

    // var Project = $scope.Projects;

    function newState(state) {
        alert("This functionality is yet to be implemented!");
    }
    function querySearch(query) {
        console.log(self.states)
        var results = query ? self.states.filter(createFilterFor(query)) : self.states, deferred;
        if (self.simulateQuery) {
            deferred = $q.defer();
            $timeout(function () {
                deferred.resolve(results);
            },
           Math.random() * 1000, false);
            return deferred.promise;
        } else {
            return results;
        }
    }
    function searchTextChange(text) {
        $log.info('Text changed to ' + text);
    }
    function selectedItemChange(item) {
        console.log('Item changed to ' + JSON.stringify(item));
    }
    //build list of states as map of key-value pairs
    function loadStates() {
        console.log($scope.AllProperty);
        var states = $scope.AllProperty;
        return states.map(function (repo) {
            repo.value = repo.Name.toLowerCase();
            return repo;
        });
    }
    //filter function for search query
    function createFilterFor(query) {
        var lowercaseQuery = angular.lowercase(query);
        return function filterFn(state) {
            return (state.value.indexOf(lowercaseQuery) === 0);
        };
    }
    $scope.Search = function (SelectedProperty) {
        console.log(SelectedProperty)
        if (SelectedProperty.Type == "Project") {
            $sessionStorage.ProjectId = {};
            $sessionStorage.ProjectId = SelectedProperty.Id;
            $window.location.href = 'ProjectPage.html';
        }
        else {
            $sessionStorage.ProjectType = SelectedProperty.Type;
            $sessionStorage.TypeId = SelectedProperty.Id;
            $window.location.href = 'Listing.html';
        }
        // alert($sessionStorage.SessionMessage);
    }
}]);

app.controller('LoginC', ['$scope', '$http', '$localstorage', '$sessionstorage', '$window', '$modal', function ($scope, $http, $localstorage, $sessionstorage, $window, $modal) {
    $scope.obj = {};
    console.log($scope.objuser)
    $scope.login = function () {
        $http({
            method: 'post',
            url: 'http://mosrealestate.silive.in/rest/api/user/login',
            data: $scope.objuser, //forms user object
            headers: { 'content-type': 'application/json' }
        }).then(function mysuccess(response) {
            $scope.id = response.data;
            if ($scope.id) {
                console.log(response.data)
                $scope.similarprojects = response.data;
                //$localstorage.pmc = 4;
                $sessionstorage.pmc = "aditi";
                $window.location.href = '/welcome.html';
            }
            else
                alert('incorrect username or password');

        }, function myerror(response) {
            $scope.similarprojects = response.statustext;
        })
        //}
    }
}]);
app.directive('starRating', function () {
    return {
        restrict: 'A',
        template: '<ul class="rating">' +
            '<li ng-repeat="star in stars" ng-class="star">' +
            '\u2605' +
            '</li>' +
            '</ul>',
        scope: {
            ratingValue: '=',
            max: '='
        },
        link: function (scope, elem, attrs) {
            scope.stars = [];
            for (var i = 0; i < scope.max; i++) {
                scope.stars.push({
                    filled: i < scope.ratingValue
                });
            }
        }
    }
});