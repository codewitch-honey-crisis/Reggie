<!DOCTYPE html>
<html>
<head>
    <title>Login</title>
    <link href="/Content/themes/base/minified/jquery.ui.all.min.css" rel="stylesheet" type="text/css" />
    <link href="/Content/Site.css?1.0.0.2" rel="stylesheet" type="text/css" />
    

    <script src="/Scripts/modernizr-2.0.6.chirp.js" type="text/javascript"></script>
    

    <script src=/Scripts/jquery-3.6.0.min.js type="text/javascript"></script>

    <!-- Latest compiled and minified CSS -->
    
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.4/css/bootstrap.min.css">
    

    <!-- Optional theme -->
    <!--<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.4/css/bootstrap-theme.min.css">-->
    
    <script src="/Scripts/bootstrap/bootstrap.min.js"></script>
</head>
<body>
    <nav id="main-navbar" class="navbar navbar-inverse navbar-fixed-top">
        <div class="container-fluid">
            <div class="navbar-header">
                <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#dm-navbar-collapse">
                    <span class="sr-only">Toggle navigation</span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                </button>
                <a class="navbar-brand" href="/">
                    <img src="/Content/Images/developermedia-logo-blue-white.png"
                         srcset="/Content/Images/developermedia-logo-blue-white.png 1x, /Content/Images/developermedia-logo-blue-white@2x.png 2x"
                         width="222" height="30" style="max-height:30px;height:auto;"
                         alt="Developer Media Logo"
                         data-retina_logo_url="https://developermedia.com/wp-content/uploads/2017/10/developermedia-logo-blue-white@2x.png" />
                </a>
            </div>
            <div class="collapse navbar-collapse" id="dm-navbar-collapse">
                <ul class="nav navbar-nav menuItems">
                    <li>
                        <a href="/">
                            Home
                        </a>
                    </li>
                                                                                                        <li>
                            <a href="/CodePlex/Home/Faq">
                                FAQ
                            </a>
                        </li>
                        <li>
                            <a href="/CodePlex/Home/ContactUs">
                                Contact Us
                            </a>
                        </li>
                </ul>
                <ul class="nav navbar-nav navbar-right">
                                            <li>
                            <a href="/Account/Logon">Sign In</a>
                        </li>
                </ul>
                </div>
        </div>
    </nav>
    
    <div class="page container-fluid">
        
        <div id="main">
            


<div class="login-body">
    <div class="login-container col-lg-3 col-md-4 col-sm-6 col-xs-12">
        <!--<div class="widget-header">
            Login

            <div class="widget-header-button">
                    
<a href="/">Back</a>
            </div>
        </div>
        <p>
            Please enter your user name and password. <a href="/Account/Register" id="Register">Register</a> if you don't have an account.
        </p>
            -->



        

<form action="/Account/Logon?ReturnUrl=%2f" method="post">            <div class="login-form">
                <div class="dm-logo col-sm-12">
                    <img src="/Content/Images/developermedia-logo-blue-white.png" />
                </div>
                <!--<div class="editor-label">
                    <label for="UserName">User name</label>
                </div>-->
                <div class="editor-field">
                    <input class="form-control" data-val="true" data-val-required="The User name field is required." id="UserName" name="UserName" placeholder="User Name" type="text" value="" />
                    <span class="field-validation-valid" data-valmsg-for="UserName" data-valmsg-replace="true"></span>
                </div>

                <!--<div class="editor-label">
                    <label for="Password">Password</label>
                </div>-->
                <div class="editor-field">
                    <input class="form-control" data-val="true" data-val-required="The Password field is required." id="Password" name="Password" placeholder="Password" type="password" />
                    <span class="field-validation-valid" data-valmsg-for="Password" data-valmsg-replace="true"></span>
                </div>

                <div class="login-button-container">
                    <div class="remember-me">
                        <input data-val="true" data-val-required="The Remember me? field is required." id="RememberMe" name="RememberMe" type="checkbox" value="true" /><input name="RememberMe" type="hidden" value="false" />
                        <label for="RememberMe">Remember me?</label>
                    </div>
                    <input type="submit" class="btn login-button" id="LogOnBtn" value="Sign in" />
                </div>
                <div>
                    <p>

                    </p>

                    <p>
                        I forgot my password!
                        <a href="/Account/ResetPassword" id="SendPassword">Please reset it.</a>
                    </p>
                </div>
            </div>
</form>    </div>
</div>
<style>
    body {
        color: white;
        background-color: #337ab7;
    }

    #main {
        background-color: #337ab7;
    }

    .dm-logo {
        text-align: center;
        margin-bottom: 25px;
    }

    .btn {
        color: black;
    }

    .login-body {
        margin-top: 40px;
        display: flex;
        flex-direction: column;
        align-content: center;
    }

    .login-container {
        margin-left: auto;
        margin-right: auto;
    }
    .login-form {
        display: flex;
        flex-direction: column;
    }
    
    .login-button {
        margin-bottom: 10px;
    }

    .login-button-container {
        margin-top: 15px;
        display: flex; 
        flex-direction: row;
        justify-content: space-between;
    }

    .remember-me {
        margin-top: 5px;
    }

    a:link, a:visited {
        color: lightskyblue;
    }

    .navbar-brand {
        display: none;
    }

    .editor-field {
        width: 100%;
    }

    .form-control {
        width: 100% !important;
    }
</style>
        </div>
        <div id="footer">
            <a href="http://developermedia.com">Developer Media</a> &copy; 2021
        </div>
    </div>

    <script src="/Scripts/jquery-ui-1.12.1.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.ui.autocomplete-select.js" type="text/javascript"></script>
    <script src="/Scripts/common.js" type="text/javascript"></script>
    <!-- Latest compiled and minified JavaScript -->
    

    
	<script src="/Scripts/jquery.validate.min.js" type="text/javascript"></script>
	<script src="/Scripts/jquery.validate.unobtrusive.min.js" type="text/javascript"></script>

    
</body>
</html>
