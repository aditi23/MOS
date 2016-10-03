var app = angular.module("ProjectListing", ['Home', 'ui.bootstrap', 'autocomplete', 'ngStorage', 'ngDialog']);

app.controller("PropertiesController",['$scope', '$http', '$localStorage', '$sessionStorage', '$window', function ($scope, $http, $localStorage, $sessionStorage, $window) {
    var Type = $sessionStorage.ProjectType;
    var TypeId = $sessionStorage.TypeId;
    console.log(Type,TypeId)
    var id = 2;
    var abc = "";
    if (Type == "City")
        abc = "http://mosrealestate.silive.in/rest/api/Search/ProjectByCity?cityId=" + TypeId;
    if (Type == "Builder")
        abc = "http://mosrealestate.silive.in/rest/api/Search/ProjectByBuilder?builderId=" + TypeId;
    if (Type == "Location")
        abc = "http://mosrealestate.silive.in/rest/api/Search/ProjectByLocation?locationId=" + TypeId;
    $http({
        method: 'GET',
        url: abc,
        //data: id,
        headers: { 'Content-Type': 'application/json' }
    }).then(function mySuccess(response) {
        $scope.AllProjects = response.data;
        console.log($scope.AllProjects);
    }, function myError(response) {
        $scope.similarProjects = response.statusText;
    })
    //$scope.currentPage = 1;
    //$scope.pageSize = 5;
    //$scope.numberOfPages = function () {
    //    return Math.ceil($scope.Properties.length / $scope.pageSize);
    //}

    var list = {};
    $scope.BHK = [{ name: '1BHK' },
{ name: '2BHK' },
{ name: '3BHK' },
{ name: '4BHK' },
{ name: '5BHK' }];

    $scope.demo = [{ name: '1BHK' },
{ name: '2BHK' },
{ name: '3BHK' },
{ name: '4BHK' },
{ name: '5BHK' }];
    $scope.selectionBHK = [];
    $scope.selectiondemo = [];
    filterProjects = function () {
        list = {
            bhk: $scope.selectionBHK,
            demo: $scope.selectiondemo,
        }
 
    }
    // toggle selection for a given employee by name
    $scope.toggleSelectionBHk = function toggleSelectionBHk(SelectedBHk) {
        var idx = $scope.selectionBHK.indexOf(SelectedBHk);

        // is currently selected
        if (idx > -1) {
            $scope.selectionBHK.splice(idx, 1);
            filterProjects();
        }

            // is newly selected
        else {
            $scope.selectionBHK.push(SelectedBHk);
            filterProjects();
        }
    };
    $scope.toggleSelectiondemo = function toggleSelectiondemo(Selecteddemo) {
        var idx = $scope.selectiondemo.indexOf(Selecteddemo);

        // is currently selected
        if (idx > -1) {
            $scope.selectiondemo.splice(idx, 1);
            filterProjects();
        }

            // is newly selected
        else {
            $scope.selectiondemo.push(Selecteddemo);
            filterProjects();
        }
    };

    $scope.filterProjects = function () {
        list = {
            bhk: $scope.selectionBHK,
            demo: $scope.selectiondemo,
        }
        console.log(list)
    }




    $scope.RedirectToProject = function (projectId) {
        $sessionStorage.ProjectId = {};
        $sessionStorage.ProjectId = projectId;
        $window.location.href = 'ProjectPage.html';
    }

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
}] );
