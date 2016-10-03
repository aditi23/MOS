var app = angular.module("PropertyApp", ['ngStorage', 'ngMaterial', 'hm.readmore']);
app.controller("PropertyController",[ '$scope', '$localStorage', '$sessionStorage', '$window', '$http', function ($scope, $localStorage, $sessionStorage, $window, $http) {
    $sessionStorage.url = {};
    $scope.type = $sessionStorage.UserType;
    console.log($sessionStorage.UserId, $sessionStorage.ProjectId)
    var userId = $sessionStorage.UserId;
    var projectId = $sessionStorage.ProjectId;
    $scope.checkBuilder = function()
    {
        if ($scope.type == "Builder") { 
            return true;
        }
        else {
            return false;
        }
    }
    $scope.login = function () {
        $sessionStorage.url = window.location.pathname;
        alert($sessionStorage.url);
        $window.location.href = "/RegisterUser.html";
    }
    $scope.UserProfile = function (userId) {
        if(userId!=null)
        {
            $sessionStorage.ViewUserId = {};
            $sessionStorage.ViewUserId = userId;
            $window.location.href = "/ViewUser.html";
        }
        else
        {
            $window.location.href = "/ProjectPage.html";
        }
    }
    $scope.ViewProperty = function () {

    }
    $scope.WriteReview = function () {
        if (userId != null) {
            $sessionStorage.url = window.location.pathname;
            $window.location.href = "/Review.html";
        }
        else {
            $window.location.href = "/RegisterUser.html";
        }
    }
    var showDays = "";
    var showMonths = "";
    var showYears = "";
    $http({
        method: "GET",
        url: "http://mosrealestate.silive.in/rest/api/Search/ProjectDetails?projectId=" + projectId + "&&userId=" + userId,
    }).then(function mySuccess(response) {
        console.log(response.data)
        $scope.propertyDetails = response.data;
        $scope.Reviews = response.data.Reviews;
        $scope.Duration = response.data.Duration;
        $scope.Image = response.data.Images;
    }, function myError(response) {
        $scope.propertyDetails = response.statusText;
    })
    $scope.getProfilePic = function () {
            return "Images/place-holder.jpg"
    }
    $scope.getUserProfilePic = function () {
        if ($scope.propertyDetails.ProfilePictureUrl)
            return $scope.propertyDetails.ProfilePictureUrl;
        else
            return "Images/place-holder.jpg"

    }


    $http({
        method: "GET",
        url: "http://mosrealestate.silive.in/rest/api/Search/SimilarProjects?projectId=" +projectId,
    }).then(function mySuccess(response) {
        $scope.similarProjects = response.data;
    }, function myError(response) {
        $scope.similarProjects = response.statusText;
    })

    $scope.ViewFullDetails = function () {
        $http({
            method: "GET",
            url: "http://mosrealestate.silive.in/rest/api/Search/ProjectDetails?projectId=" + projectId + "&&userId=" + userId
        }).then(function mySuccess(response) {
        console.log(response.data)
            $scope.propertyDetails = response.data;
            $scope.Amenities = response.data.Amenities;
            $scope.ApartmentBuildQuality = response.data.ApartmentBuildQuality;
            $scope.ConstructionQualityParameter = response.data.ConstructionQualityParameter;
            $scope.Inventory = response.data.Inventory;
            $scope.LegalClarity = response.data.LegalClarity;
            $scope.Livability = response.data.Livability;
            $scope.ProjectInformation = response.data.ProjectInformation;


        }, function myError(response) {
            $scope.propertyDetails = response.statusText;
        })

        $scope.showPropertyFullDetails = true;
    }
    $scope.Get = function () {
        $window.alert($localStorage.abc + "\n" + $sessionStorage.vcb);
    }

    $scope.Follow = function () {
        if (userId != null) {
            FollowProject();
        }
        else {
            $window.location.href = "/RegisterUser.html";
    }
    }
    FollowProject = function () {
        $http({
            method: 'POST',
            url: 'http://mosrealestate.silive.in/rest/api/User/FollowProject',
            headers: { 'Content-Type': 'application / json' },
            data: {
                ProjectId: projectId,
                UserId: userId
            }
        }).then(function mySucces(response) {
            $scope.FollowedStatus = response.data;
        }, function myError(response) {
            $scope.FollowedStatus = response.statusText;
        })
        $scope.showFollowProperty = true;
    };
    MarkHelpful = function (Id) {
        $http({
            method: 'POST',
            url: 'http://mosrealestate.silive.in/rest/api/Review/MarkHelpful',
            headers: { 'Content-Type': 'application / json' },
            data: {
                ReviewId: Id,
                ReviewedUserId: userId,
                Helpful: 1,
                SayThanks: 0
            }
        }).then(function mySucces(response) {
            $scope.HelpfulStatus = response.data;
        }, function myError(response) {
            $scope.HelpfulStatus = response.statusText;
        })
        $scope.showHelpful = true;
    };

    $scope.Helpful = function (Id) {
        if (userId != null) {
            MarkHelpful(Id);
        }
        else {
            $window.location.href = "/RegisterUser.html";
        }
    }
    MarkSayThanks = function (Id) {
        $http({
            method: 'POST',
            url: 'http://mosrealestate.silive.in/rest/api/Review/MarkSayThanks',
            headers: { 'Content-Type': 'application / json' },
            data: {
                ReviewId: Id,
                ReviewedUserId: userId,
                Helpful: 0,
                SayThanks: 1
            }
        }).then(function mySucces(response) {
            $scope.SayThanksStatus = response.data;
        }, function myError(response) {
            $scope.SayThanksStatus = response.statusText;
        })
        $scope.showSayThanks = true;
    };
    $scope.SayThanks = function (Id) {
        if (userId != null) {
            MarkSayThanks(Id);
        }
        else {
            $window.location.href = "/RegisterUser.html";
        }
    }
    
     reviewId = {};
    $scope.abc = function (Id) {
        $localStorage.reviewId = Id;
    }
    $scope.Inappropriate = function () {
        if (userId != null) {
            MarkInappropriate($localStorage.reviewId);
        }
        else {
            $window.location.href = "/RegisterUser.html";
        }
    }
   
    MarkInappropriate = function(Id) {
        $http({
            method: 'POST',
            url: 'http://mosrealestate.silive.in/rest/api/Review/AddInappropriate',
            headers: { 'Content-Type': 'application / json' },
            data: {
                ReviewId: Id,
                UserId: userId,
                Reason: $scope.Reason
            }
        }).then(function mySucces(response) {
            $scope.InappropriateStatus = response.data;
            alert($scope.InappropriateStatus);
        }, function myError(response) {
            $scope.InappropriateStatus = response.statusText;
        })
        $scope.showInappropriate = true;
    };
    $scope.IsConvinced = function (Id) {
        if (userId != null) {
            MarkIsConvinced(Id);
        }
        else {
            $window.location.href = "/RegisterUser.html";
        }
    }
   
    $scope.MarkIsConvinced = function (Id) {
        $http({
            method: 'POST',
            url: 'http://mosrealestate.silive.in/rest/api/Review/MarkConvinced?reviewId=' + Id + '&&IsConvinced=' + true + '&&userId=' + userId,
            headers: { 'Content-Type': 'application / json' },
            data: {
                ReviewId: 3,
                ReviewedUserId: 3,
                Helpful: 0,
                SayThanks: 1
            }
        }).then(function mySucces(response) {
            $scope.SayIsConvinced = response.data;
        }, function myError(response) {
            $scope.IsConvincedStatus = response.statusText;
        })
        $scope.showIsConvinced = true;
    };
    $scope.UnFollow = function () {
        if (userId != null) {
            UnFollowProject();
        }
        else {
            $window.location.href = "/RegisterUser.html";
        }
    }
    UnFollowProject = function () {                                                         //change method, bind isconvinced
        $http({
            method: 'PUT',
            url: 'http://mosrealestate.silive.in/rest/api/User/UnfollowProject',
            headers: { 'Content-Type': 'application / json' },
            data: {
                ProjectId: projectId,
                UserId: userId,
            }
        }).then(function mySucces(response) {
            $scope.UnFollowStatus = response.data;
        }, function myError(response) {
            $scope.UnFollowStatus = response.statusText;
        })
        $scope.showUnFollowProperty = true;
    };
}]);

app.controller('QueryUsController', ['$scope', '$http', function ($scope, $http) {

    $scope.objQuuery = {};
    $http({
        method: "GET",
        url: "http://mosrealestate.silive.in/rest/api/Search/GetAllCities"
    }).then(function mySuccess(response) {
        $scope.Id = null;
        $scope.CityList = response.data;
    }, function myError(response) {
        $scope.CItyList = response.statusText;
    })

    $scope.queryUs = function () {

        $scope.NameRequired = '';
        $scope.ContactRequired = '';
        $scope.EmailRequired = '';
        $scope.CityRequired = '';
        $scope.MessageRequired = '';
        if (!$scope.objQuery.Name)
            $scope.NameRequired = '*';
        if (!$scope.objQuery.Contact)
            $scope.ContactRequired = '*';
        //if (!$scope.objQuery.Email)
        //    $scope.EmailRequired = '*';
        if (!$scope.objQuery.CityId)
            $scope.CityIdRequired = '*';
        if (!$scope.objQuery.Message)
            $scope.MessageRequired = '*';
        console.log($scope.objQuery)

        var obj = {};
        obj.Name = $scope.objQuery.Name;
        obj.EmailId = $scope.objQuery.Email;
        obj.ContactNumber = $scope.objQuery.Contact;
        obj.CityId = 1; //$scope.objQuery.CityId;
        obj.Message = $scope.objQuery.Message;
        $http({
            method: "PUT",
            url: "http://mosrealestate.silive.in/rest/api/User/Query",
            data: obj,
            headers: { 'Content-Type': 'application/json' }
        }).then(function mySuccess(response) {
            $scope.propertyDetails = response.data;
            $scope.Reviews = response.data.Reviews;


        }, function myError(response) {
            $scope.propertyDetails = response.statusText;
        })
    }
}]);

app.controller("ProjectPageAutoComplete", ['$scope', '$http', '$localStorage', '$sessionStorage', '$window', '$timeout', '$q', '$log', 'filterFilter', function ($scope, $http, $localStorage, $sessionStorage, $window, $timeout, $q, $log, filterFilter) {
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
            url: 'http://mosrealestate.silive.in/rest/api/Search/FinalAutoComplete',
            //data: JSON.stringify(params),
            headers: { 'Content-Type': 'application/json' }
        }).
                        then(function mySucces(response) {
                            console.log(response.data)
                            if (response.data != "No data found.") {
                                console.log(response.data)
                                $scope.AllProperty = response.data;
                                $scope.AllProperty = filterFilter(response.data, { CityId: CityId });
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

app.controller("NavBarProjectPage", ['$scope', '$http', '$localStorage', '$sessionStorage', '$window', function ($scope, $http, $localStorage, $sessionStorage, $window) {
    if ($sessionStorage.UserId && $sessionStorage.UserType == "User") {
        console.log($sessionStorage.UserId)
        $http({
            method: 'GET',
            url: 'http://mosrealestate.silive.in/rest/api/User/GetUserFirstName?userId=' + $sessionStorage.UserId,
            data: { userId: $sessionStorage.UserId },
            headers: { 'Content-Type': 'application/json' }
        }).
        then(function mySucces(response) {
            $scope.Email = response.data;
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
            $scope.Email = response.data.CompanyName;
            console.log(response.data);
        }, function myError(response) {
            alert("error");
        });
    }
    else {
        $scope.Email = "Login"
    }
    $scope.checkLogin = function () {
        if ($scope.Email == "Login")
            return true;
        else
            return false;
    }
    $scope.myFunc = function () {
        $sessionStorage.url = window.location.pathname;
        console.log($sessionStorage.url)
        if ($scope.Email == "Login")
            $window.location.href = 'RegisterUser.html';
        else if ($sessionStorage.UserType == "Builder")
            $window.location.href = 'Builder.html';
        else
            $window.location.href = 'User.html';

    }
    $scope.logout = function () {
        $localStorage.$reset()
        $sessionStorage.$reset();
        $scope.Email = "Login"
        $window.location.href = 'Index.html';
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