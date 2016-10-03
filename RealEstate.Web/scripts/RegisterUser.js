var app = angular.module('RegisterUser', ['ngStorage']);

    function onSignIn(googleUser) {
    console.log(googleUser);
        var profile = googleUser.getBasicProfile();
        console.log(profile.getName());
        setUser(profile);
}

app.controller('socialLogin', ['$scope', '$http', '$localStorage', '$sessionStorage', '$window', function ($scope, $http, $localStorage, $sessionStorage, $window) {
    var obj = {};
    logout = function () {

        var auth2 = gapi.auth2.getAuthInstance();
        auth2.signOut().then(function () {
            console.log('User signed out.');
            alert('User signed out.');
        });
    }

    setUser = function (profile) {
        
        //obj.ProfilePicture = btoa(profile.getImageUrl());
        //obj.displayPicture = btoa(profile.getImageUrl());               //converts to base 64
        var imageUrl = profile.getImageUrl();
        //var encodedImage = {};
        //var convertFunction = convertFileToDataURLviaFileReader;
        
        //convertFunction(imageUrl, function (base64Img) {
        //    $('.output')
              
        //    var encodedImageArray = base64Img.split(",");
        //    encodedImage = encodedImageArray[1];
        //    console.log(encodedImage);
        //    sendDataThroughGoogle(profile, encodedImage);
        //});

       // event.preventDefault();

    //    function convertFileToDataURLviaFileReader(url, callback) {
    //        var xhr = new XMLHttpRequest();
    //        xhr.responseType = 'blob';
    //        xhr.onload = function () {
    //            var reader = new FileReader();
    //            reader.onloadend = function () {
    //                callback(reader.result);
    //            }
    //            reader.readAsDataURL(xhr.response);
    //        };
    //        xhr.open('GET', url);
    //        xhr.send();
    //    }
        
    //}
    //function sendDataThroughGoogle(profile,encodedImage) {
        var fullName = profile.getName();
        var name = fullName.split(" ");
        //console.log(profile.getImageUrl());
        obj.FirstName = name[0];
        obj.LastName = name[1];
        obj.EmailId = profile.getEmail();
        obj.Password = "demo";
        obj.UserTypeId = 3;
       // obj.image = encodedImage;
        obj.ProfilePictureUrl =profile.getImageUrl() ;

        $http({
            method: "POST",
            url: "http://localhost:19342/api/User/RegisterUser",
            data: obj,
            headers: { 'Content-Type': 'application/json' }
        }).then(function mySuccess(response) {
            $scope.Id = response.data;
            $sessionStorage.UserId = {};
            $sessionStorage.UserId = response.data[0];
            $sessionStorage.UserType = "User";
            $sessionStorage.UserType = response.data[2];
            $window.location.href = "Index.html";
            // $window.location.href = $sessionStorage.url;
        }, function myError(response) {
            //alert(response.statusText);
        })
    }

       

    dataFromFacebook = function (response) {
        console.log(response);
        var fullName = response.name;
        var name = fullName.split(" ");
        obj.FirstName = name[0];
        obj.LastName = name[1];
        obj.EmailId = response.email;
        obj.Password = "anotherdemo";
        obj.UserTypeId = 3;
        obj.ProfilePictureUrl = response.picture.data.url;               //converts to base 64
        console.log(obj);
        $http({
            method: "POST",
            url: "http://localhost:19342/api/User/RegisterUser",
            data: obj,
            headers: { 'Content-Type': 'application/json' }
        }).then(function mySuccess(response) {
            $sessionStorage.UserId = response.data;
            $sessionStorage.UserType = 3;
            $window.location.href = $sessionStorage.url;
        }, function myError(response) {
            //alert(response.statusText);
        })
    }
}]);
window.fbAsyncInit = function () {
    FB.init({
        appId: '558112947725248',
        status: false,
        cookie: true,
        xfbml: true,
        oauth: true,
        version: 'v2.4'
    });
};

(function (d) {
    var js, id = 'facebook-jssdk'; if (d.getElementById(id)) { return; }
    js = d.createElement('script'); js.id = id; js.async = true;
    js.src = "//connect.facebook.net/en_US/sdk.js";
    d.getElementsByTagName('head')[0].appendChild(js);

}(document));

function Facebook_login() {
    FB.getLoginStatus(function (response) {
        if (response.status === 'connected') {
            //console.log(response);
            //$sessionStorage.mode = "Facebook";
            //$window.location.href = $sessionStorage.url;
            FB.api('/me?fields=id,name,email,picture', function (response) {
                console.log(response);
                //alert("Welcome " + response. + ": Your UID is " + response.id);
                dataFromFacebook(response);
            });
        }
    });
}
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
}]).controller('RegisterUserController', ['$scope', '$http', '$localStorage', '$sessionStorage', '$window', function ($scope, $http, $localStorage, $sessionStorage, $window) {

    $scope.objForm = {};

    $http({
        method: "GET",
        url: "http://localhost:19342/api/User/GetAllUserType"
    }).then(function mySuccess(response) {
        console.log(response.data)
        $scope.Id = null;
        $scope.UserTypeList = response.data;
    }, function myError(response) {
        $scope.UserTypeList = response.statusText;
    })
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
        var image;
        if ($scope.UserRegForm.$valid) {
            var request = {
                method: 'POST',
                url: 'http://localhost:19342/api/Search/UploadFiles',
                data: formdata,
                headers: {
                    'Content-Type': undefined
                }
            };

            // SEND THE FILES.
            $http(request)
                .success(function (response) {
                    image = response;
                    if (image != "Upload Failed")
                        image = image;
                    else
                        image = null;
                    register();
                })
                .error(function () {
                });
            register = function () {
            var obj = {};
            obj.FirstName = $scope.objForm.Fname;
            obj.LastName = $scope.objForm.Lname;
            obj.EmailId = $scope.objForm.Email;
            obj.Password = $scope.objForm.Password;
            obj.Contact = $scope.objForm.Contact;
            obj.City = $scope.objForm.City;
                obj.ProfilePictureUrl = image;
            obj.UserTypeId = $scope.objForm.UserType.Id;
                obj.UserRegMode = 1;
            console.log(obj.UserTypeId)
            $http({
                method: "POST",
                url: "http://localhost:19342/api/User/RegisterUser",
                data: obj,
                headers: { 'Content-Type': 'application/json' }
            }).then(function mySuccess(response) {
                var status = response.status;
                if (status != 400) {
                    $scope.User = response.data;
                    if ($scope.User == "EMAIL ID already exist")
                        $scope.message = "Email Id already exist";
                    else {
                        console.log($scope.User)
                        $sessionStorage.UserId = {};
                        $sessionStorage.UserId = response.data[0];
                        $sessionStorage.UserType = "User";
                        $sessionStorage.UserType = response.data[2];
                        $window.location.href = "Index.html";
                    }
                }
            }, function myError(response) {
                //alert(response.statusText);
            })
        }

        }
    }
}]);

app.controller('LoginController', ['$scope', '$http', '$localStorage', '$sessionStorage', '$window', function ($scope, $http, $localStorage, $sessionStorage, $window) {
        $scope.objLogin = {};
        $scope.login = function () {
            if ($scope.loginForm.$valid) {
            $scope.EMAILRequired = '';
            $scope.PASSWORDRequired = '';

            if (!$scope.objLogin.Email)
                $scope.EMAILRequired = '*';
            if (!$scope.objLogin.Password)
                $scope.PASSWORDRequired = '*';

            var obj = {};
            obj.EmailId = $scope.objLogin.Email;
            obj.Password = $scope.objLogin.Password;
            console.log($sessionStorage.url)

            $http({
                method: "POST",
                url: "http://localhost:19342/api/User/Login",
                data: obj,
                headers: { 'Content-Type': 'application/json' }
            }).then(function mySuccess(response) {
                $scope.Id = response.data;
                $scope.status = response.status;
                $sessionStorage.mode = "Web";
                if (status != 400) {
                    if (response.data == "Invalid Username or Password" || response.data == "Invalid Request")
                        $scope.message = "Invalid Username or Password";
                    else {
                        $sessionStorage.UserId = {};
                        $sessionStorage.UserId = response.data[0];
                        $sessionStorage.UserRegType = "Web";
                        $sessionStorage.UserType = response.data[2];
                        if ($sessionStorage.url)
                        $window.location.href = $sessionStorage.url;
                        else
                            $window.location.href = "/Index.html";

                    }
                }
            }, function myError(response) {
                //alert(response.statusText);
            })
            //$window.location.href = $localStorage.url;
        }
    }
}]);