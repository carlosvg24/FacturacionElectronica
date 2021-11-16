<%@ Page Title="Facturación Electrónica" Language="C#" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="frmLanding.aspx.cs" Inherits="FacturacionOnLine.frmLanding" %>

<!DOCTYPE html>

<html lang="es-MX" xml:lang="es-MX">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" />
    <meta name="description" content="VIVA Aerobus-Facturación Electrónica" />
    <meta http-equiv="Expires" content="0" />
    <meta http-equiv="Last-Modified" content="0" />
    <meta http-equiv="Cache-Control" content="no-cache, mustrevalidate" />
    <meta http-equiv="Pragma" content="no-cache" />

    <title>facturación.vivaaerobus.com</title>

    <link href='https://fonts.googleapis.com/css?family=Lato:300,400,700,900,300italic,400italic,700italic,900italic'
        rel='stylesheet' type='text/css' />
    <link href='https://fonts.googleapis.com/css?family=Varela+Round' rel='stylesheet' type='text/css' />
    <link href="../Contents/CSS/estilos.css" rel="stylesheet" />
    <link href="../Contents/CSS/cerulean.css" rel="stylesheet" />
    <link href="../Contents/CSS/bootstrap-datepicker.css" rel="stylesheet" />
    <link href="../Contents/CSS/bootstrap-select.css" rel="stylesheet" />

    <script src="../Contents/Scripts/jquery-1.12.4.min.js"></script>
    <script src="../Contents/Scripts/jquery-3.0.0.min.js"></script>
    <script src="../Contents/Scripts/jquery.js"></script>
    <script src="../Contents/Scripts/jquery-ui.js"></script>
    <script src="../Contents/Scripts/bootstrap.js"></script>
    <script src="../Contents/Scripts/bootstrap-datepicker.js"></script>
    <script src="../Contents/Scripts/datepickerlang/bootstrap-datepicker.es.min.js"></script>
    <script src="../Contents/Scripts/bootstrap-select.js"></script>
    <script src="../Contents/Scripts/datepickerlang/bootstrap-datepicker.es.min.js"></script>
    <script src="../Contents/Scripts/sweetalert.min.js"></script>
   <%-- <asp:ContentPlaceHolder ID="head" runat="server">
    </asp:ContentPlaceHolder>--%>
</head>
<body>
    <div id="DivMain" class="siteWrapper">
        <table style="border: none; border-spacing: 0px; border-collapse: collapse; margin-bottom: 5px;">
            <tr>
                <td style="width: 480px; height: 100px">
                    <a href="../Default.aspx">
                        <span class="logo">
                            <img src="../Contents/Images/vivaaerobus-logo.png" alt="Logo" width="480" height="100" border="0" />
                        </span>
                    </a>
                </td>
                <td style="width: 480px; height: 100px; vertical-align: top; text-align: right;">
                    <div class="absoluteTop">
                        <div class="headerBookmarks">
                            <a class="socialBookmark markFacebook" target="_blank" href="https://www.facebook.com/VivaAerobus"></a>
                            <a class="socialBookmark markTwitter" target="_blank" href="https://twitter.com/vivaaerobus"></a>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="height: 25px; text-align: right;" colspan="2">
                    <%--<a href="../Default.aspx" class="menu_nav">Factura Electrónica</a>--%>
                    <a href="../Reglas.aspx" class="menu_nav" style="margin-right: 15px;">Reglas de Facturación</a></td>
            </tr>
        </table>
        <hr class="Separador" />
        <table style="border: none; border-spacing: 0; border-collapse: collapse;">
            <tr>
                <td style="width: 724px; height: 60px; vertical-align: bottom;" class="titulo_pagina">
                    <%=Page.Title %>
                </td>
                <td style="width: 236px; height: 60px; vertical-align: bottom; text-align: right;">
                    <table style="margin-bottom: 10px; margin-right: 15px; border: none; border-collapse: collapse; border-spacing: 0px;">
                        <tr>
                            <td style="width: 90px;" class="descargas_guia">Descargas -</td>
                            <td style="width: 27px; text-align: right;">
                                <a href="Contents/GuiasPDF/Guía%20para%20proceso%20de%20facturación.pdf" target="_blank" title="Guía para proceso de Facturación." data-toggle="tooltip" style="margin:3px;">
                                    <img src="../Contents/Images/guia_facturacion.jpg" width="17" height="16" border="0" alt="Guía de facturación" />
                                </a>
                            </td>
<%--                            <td style="width: 27px; text-align: right;">
                                <a href="Contents/GuiasPDF/GUIA_ACTUALIZACION.pdf" target="_blank" title="Guía para Actualizar Datos de Facturación." data-toggle="tooltip">
                                    <img src="../Contents/Images/guia_actualizacion.jpg" width="17" height="16" border="0" alt="Guía de actualización" />
                                </a>
                            </td>
                            <td style="width: 34px; text-align: right;">
                                <a href="Contents/GuiasPDF/GUIA_REENVIO.pdf" target="_blank" title="Guía para Reenvío de Facturas." data-toggle="tooltip">
                                    <img src="../Contents/Images/guia_reenvio.jpg" width="24" height="16" border="0"  alt="Guía de reenvío" />
                                </a>
                            </td>--%>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <div class="separacion_20"></div>
        <div>
            <form id="form1" runat="server">
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
                <div>
                    <table style="margin-bottom: 10px; margin-right: 15px; border: none; border-collapse: collapse; border-spacing: 0px; width:100%" >
                        <tr>
                            <td style="width: 27px; text-align: center;">
                               <asp:Image ID="imgMantenimiento" runat="server" ImageUrl="~/Imagenes/mantenimiento5.png" Width="50%" />
                            </td>
                        </tr>
                    </table>

                </div>
            </form>
        </div>
        <div class="separacion_20"></div>
        <div class="footerPage">
            <a style="border-right: solid 1px #30b131;" href="https://www.vivaaerobus.com/mx" title="VivaAerobus, La Aerol&#237;nea de bajo costo de M&#233;xico. Los mejores precios, 
                                                                    ofertas, promociones y descuentos." target="_blank" data-toggle="tooltip">VivaAerobus
            </a>
            <a style="border-right: solid 1px #30b131;" href="https://www.vivaaerobus.com/mx/promociones/promociones-de-vuelos" title="VivaAerobus, La Aerol&#237;nea de bajo costo de 
                                            M&#233;xico. Los mejores precios, ofertas, promociones y descuentos." target="_blank" data-toggle="tooltip">La Aerol&#237;nea de bajo costo de M&#233;xico. Los mejores precios, ofertas, promociones y descuentos.
            </a>
            <a href="https://www.vivaaerobus.com/upload/mx/legales/Vivaaerobus-AvisoPrivacidadMayo2013.pdf" title="Aviso de Privacidad."
                id="footerLinksultimo" target="_blank" data-toggle="tooltip">Aviso Privacidad
            </a>
        </div>
    </div>
</body>
</html>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        #content {
            padding: 25px;
        }

        #fade {
            display: none;
            position: fixed;
            top: 0%;
            left: 0%;
            width: 100vw;
            height: 100vh;
            background-color: #ababab;
            z-index: 1001;
            -moz-opacity: 0.8;
            opacity: .70;
            filter: alpha(opacity=70);
            -ms-filter: "progid:DXImageTransform.Microsoft.Alpha(Opacity=70)";
        }

        #modal {
            display: none;
            position: absolute;
            top: 45%;
            left: 40%;
            padding: 30px 15px 0px;
            border: none;
            background-color: transparent;
            z-index: 1002;
            text-align: center;
            overflow: auto;
        }

        .CheckGrid [type="checkbox"] {
            display: inline-block !important;
        }
    </style>
    <div id="fade">
    </div>
    <div id="modal">
        <img src="../Contents/Images/Loading.gif" style="border: none;" alt="Loading..." />
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" EnableViewState="true" RenderMode="Block" ViewStateMode="Enabled">
        <ContentTemplate>
            <div class="form-horizontal">
               
            </div>
        </ContentTemplate>
     
    </asp:UpdatePanel>
   
</asp:Content>--%>
