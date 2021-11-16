<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Facturacion33.PortalAdmin.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta name="viewport" content="width=device-width, user-scalable=no, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0" />

    <link href="<%= ResolveClientUrl("~/Content/bootstrap.css") %>" rel="stylesheet" />
    <link href="<%= ResolveClientUrl("~/Content/bootstrap-theme.min.css") %>" rel="stylesheet" />
    <link href="<%= ResolveClientUrl("~/Content/bootstrap-dialog.min.css") %>" rel="stylesheet" />
    <link href="<%= ResolveClientUrl("~/Content/VivaAerobus.css") %>" rel="stylesheet" />
    <link href="<%= ResolveClientUrl("~/Content/jquery-ui.min.css") %>" rel="stylesheet" />
    <link href="<%= ResolveClientUrl("~/Content/combined.css") %>" rel="stylesheet" />
    <link href="<%= ResolveClientUrl("~/Content/LayoutLogin.css") %>" rel="stylesheet" />

    <script src="<%= ResolveClientUrl("~/scripts/jquery-3.1.0.min.js") %>"></script>
    <script src="<%= ResolveClientUrl("~/scripts/bootstrap.min.js") %>"></script>
    <script src="<%= ResolveClientUrl("~/scripts/bootstrap-dialog.min.js") %>"></script>
    <script src="<%= ResolveClientUrl("~/scripts/jquery-ui.min.js") %>"></script>


</head>
<body>
    <form id="formMaster" runat="server" class="form-horizontal" style="background-color: teal;">
        <div class="container principal">

            <div class="headWrapper">
                <h1 class="startpage-header">VivaAerobus</h1>
                <div class="row" style="margin: 10px;">
                    <div class="col-md-4 head col-xs-4 hidden-sm">
                        <a href="/mx">
                            <div class="logo " style="background: url('/Content/Images/vivaaerobus-logo-responsive.png') no-repeat">
                            </div>
                        </a>
                    </div>
                    <div class="hidden-md head hidden-xs col-sm-4">
                        <a href="/mx">
                            <div class="img-responsive branding " style="background: url('/content/images/vivaaerobus-logo-mobile.png') no-repeat">
                            </div>
                        </a>
                    </div>
                    <div class="col-md-5 hidden-xs hidden-sm">
                        <div class="clear">
                        </div>
                    </div>
                    <div class="col-md-3 hidden-xs hidden-sm" style="margin-top: 10px;">
                    </div>
                </div>
                <div class="row" style="margin-bottom: 40px;">
                    <div class="col-md-12 menu" style="padding-left: 0px; padding-right: 0px;">
                        <div style="width: 100%; margin-bottom: 5px;">
                            <div class="container" style="margin-left: 0px; padding-left: 0px; margin-right: 0px; padding-right: 0px;">
                                <div class="navbar-default side-collapse in">
                                    <nav class="navbar-collapse">
                                    </nav>
                                </div>
                            </div>
                            <div class="clear"></div>
                        </div>
                    </div>
                </div>
            </div>


            <div class="container" style="height: 80%; margin-left: 0px; margin-right: 0px; width: 100%;">
                <div class="header-panel-viva" style="width: 100%; padding-left: 0px; padding-right: 0px;">
                    <div style="width: 100%; font-size: x-large; padding-top: 7px; padding-bottom: 5px; padding-left: 15px; text-align: center;">
                        Portal Administrativo de Facturacion 3.3
                    </div>
                </div>

                <div class="row" style="vertical-align: bottom; margin-bottom: 0px;">
                    <div class="col-md-2 " style="padding-left: 0px; padding-right: 0px;">
                    </div>
                    <div class="col-md-8 " style="padding-left: 0px; padding-right: 0px;">
                        <div class="main">
                            <div class="form-2">
                                <h1><span class="sign-up">Iniciar Sesión</span> </h1>
                                <div class="col-md-6 " style="padding-left: 0px; padding-right: 0px;">
                                    <asp:Image ID="imgLogin" runat="server" ImageUrl="~/Content/Images/login.jpg" />
                                </div>
                                 <div class="col-md-1">
                                </div>
                                <div class="col-md-5">

                                    <div class="row">
                                        <label for="login"><i class="icon-user"></i>Usuario</label>
                                        <input id="txtUserName" type="text" runat="server" name="login" placeholder="Usuario" />
                                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtUserName"
                                            CssClass="text-danger" ErrorMessage="El usuario es requerido..." ValidationGroup="GrupoLogin" ForeColor="Red" Display="None">*</asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Usuario no cumple estandar..." ControlToValidate="txtUserName" ValidationExpression="^[a-zA-Z0-9._-]{3,15}$" ValidationGroup="GrupoLogin" ForeColor="Red" Display="None">*</asp:RegularExpressionValidator>
                                    </div>

                                    <div class="row">
                                        <label for="password"><i class="icon-lock"></i>Contraseña</label>
                                        <input id="txtPassword" runat="server" type="password" name="password" placeholder="Contraseña" class="showpassword" />
                                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPassword"
                                            CssClass="text-danger" ErrorMessage="La contraseña es requerida..." ValidationGroup="GrupoLogin" ForeColor="Red" Display="None">*</asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="La contraseña no cumple estandar..." ControlToValidate="txtPassword" ValidationExpression="^(?=\w*\d)(?=\w*[A-Z])(?=\w*[a-z])\S{5,16}$" ValidationGroup="GrupoLogin" ForeColor="Red" Display="None">*</asp:RegularExpressionValidator>

                                    </div>

                                    <div class="row">
                                        <p class="clearfix" style="align-self: center">
                                            <asp:Button ID="btnLogin" CssClass="btnViva" runat="server" Text="Ingresar" OnClick="btnLogin_Click" ValidationGroup="GrupoLogin" />
                                        </p>
                                    </div>

                                    <div class="row">
                                        <asp:Label ID="lblMensaje" runat="server" Text=""></asp:Label>
                                    </div>

                                    <div class="row">
                                        <asp:ValidationSummary ID="ResumenErrores" runat="server" ValidationGroup="GrupoLogin" DisplayMode="List" BackColor="#FFF0F0" BorderStyle="None" Font-Names="Verdana" ForeColor="Red" />
                                    </div>

                                </div>
                                
                            </div>
                        </div>
                    </div>
                    <div class="col-md-2 " style="padding-left: 0px; padding-right: 0px;">
                    </div>

                </div>
            </div>
            <div class="footerPage">
                <a style="border-right: solid 1px #30b131;" href="https://www.vivaaerobus.com/mx" title="VivaAerobus, La Aerol&#237;nea de bajo costo de M&#233;xico. Los mejores precios,ofertas, promociones y descuentos."
                    target="_blank" data-toggle="tooltip">VivaAerobus
            </a>
                <a style="border-right: solid 1px #30b131;" href="https://www.vivaaerobus.com/mx/promociones/promociones-de-vuelos" title="VivaAerobus, La Aerol&#237;nea de bajo costo de M&#233;xico. Los mejores precios, ofertas, promociones y descuentos."
                    target="_blank" data-toggle="tooltip">La Aerol&#237;nea de bajo costo de M&#233;xico. Los mejores precios, ofertas, promociones y descuentos.
            </a>
                <a href="https://www.vivaaerobus.com/upload/mx/legales/Vivaaerobus-AvisoPrivacidadMayo2013.pdf" title="Aviso de Privacidad."
                    id="footerLinksultimo" target="_blank" data-toggle="tooltip">Aviso Privacidad
            </a>
            </div>


        </div>
        <script src="scripts/Login.js"></script>
    </form>

</body>
</html>
