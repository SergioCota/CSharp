<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Nomina.WS.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>

    <link rel="stylesheet" type="text/css" href="css/toastr.css">
    <link rel="stylesheet" type="text/css" href="/css/bootstrap.min.css" />

</head>
<body>

    <div class="modal-body" style="margin-top: 15%; margin-left: 33%">
        
        <h1 style="margin-left:80px; margin-bottom:50px" >Login sistema nómina</h1>

        <form class="form-horizontal">
            <div class="control-group">
                <label class="control-label ui-span" for="inputUser">*Email:</label>
                <div class="controls">
                    <input type="text" id="txtEmail" placeholder="Email">
                </div>
            </div>
            <div class="control-group">
                <label class="control-label ui-span" for="inputPassword">*Contrase&ntilde;a:</label>
                <div class="controls">
                    <input type="password" id="txtPassword" placeholder="Contrase&ntilde;a">
                </div>
            </div>
            <div class="control-group">
                <div class="controls">
                    <input type="button" class="btn btn-primary" id="btnLogin" value="Log In" />
                </div>
            </div>

        </form>
    </div>

    <script src="Scripts/libs/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="Scripts/libs/bootstrap-multiselect.js" type="text/javascript"></script>
    <script src="Scripts/libs/bootstrap.js" type="text/javascript"></script>
    <script src="Scripts/libs/bootstrap.min.js" type="text/javascript"></script>
    <script src="Scripts/libs/toastr.min.js" type="text/javascript"></script>
    <script src="Scripts/Login.js" type="text/javascript"></script>
</body>
</html>
