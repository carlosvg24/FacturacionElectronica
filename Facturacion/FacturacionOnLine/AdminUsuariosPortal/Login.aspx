<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="FacturacionOnLine.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" />
    <meta name="description" content="VIVA Aerobus-Facturación Electrónica" />
    <meta http-equiv="Expires" content="0" />
    <meta http-equiv="Last-Modified" content="0" />
    <meta http-equiv="Cache-Control" content="no-cache, mustrevalidate" />
    <meta http-equiv="Pragma" content="no-cache" />
    <title>facturacion.vivaaerobus.com</title>


    <link href="<%= Page.ResolveUrl("~/Contents/css/cerulean.css")%>" rel="stylesheet" />
    <link href="<%= Page.ResolveUrl("~/Contents/css/estilos.css")%>" rel="stylesheet" />
    <link href="<%= Page.ResolveUrl("~/Contents/css/bootstrap-datepicker.css")%>" rel="stylesheet" />
    <link href="<%= Page.ResolveUrl("~/Contents/css/bootstrap-select.css")%>" rel="stylesheet" />    
    <link href="<%= Page.ResolveUrl("~/Contents/bootstrap/css/bootstrap.css")%>" rel="stylesheet" />
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.8.2/css/all.css" integrity="sha384-oS3vJWv+0UjzBfQzYUhtDYW+Pj2yciDJxpsK1OYPAYjqT085Qq/1cq5FLXAZQ7Ay" crossorigin="anonymous"/>
    
    <style>
        /*--------------------------------------------
                      TIPOS DE DIALOGOS
         -------------------------------------------*/
        .dialogo_info #img_dialogo
        {
            width:60px;
            height:60px;
            background:url("../../Contents/images/img_dialogo_informacion.png") no-repeat; 
        }

        .dialogo_preg #img_dialogo
        {
            width:60px;
            height:60px;
            background:url("../../Contents/images/img_dialogo_pregunta.png") no-repeat;  
        }

        .dialogo_alert #img_dialogo
        {
            width:60px;
            height:60px;
            background:url("../../Contents/images/img_dialogo_alerta.png") no-repeat;  
        }

        .dialogo_error #img_dialogo
        {
            width:60px;
            height:60px;
            background:url("../../Contents/images/img_dialogo_error.png") no-repeat;  
        }

        /*--------------------------------------------
                   CENTRAR VENTANAS MODAL
         -------------------------------------------*/
        .modal {
          text-align: center;
          padding: 0!important;
        }

        .modal:before {
          content: '';
          display: inline-block;
          height: 100%;
          vertical-align: middle;
          margin-right: -4px;
        }

        .modal-dialog {
          display: inline-block;
          text-align: left;
          vertical-align: middle;
        }

        .oculto 
        {
            display:none !important;
        }

        .visible
        {
            display:block;
        }
        /*--------------------------------------------
                   ERRORES, VALIDACIONES
         -------------------------------------------*/
        .error {
          color: #DE2519;
          margin-left: 5px;
        }
 
        label.error {
          display: inline;
        }

        .validation { border: solid 2px #DE2519; }
        .validationMsj { color: #DE2519; font-size:medium; }

        .title{
            color: #55565a;
            font-size:medium;
            font-weight: bold;
            TEXT-DECORATION: none;
        }

        .page_title{
            font-size:20px !important;
        }

        a.title:link {  }
        a.title:visited {  }
        a.title:active { color:darkgreen; }
        a.title:hover { COLOR: red; }

        
    </style>

    <style>
        /* The switch - the box around the slider */
        .switch {
          position: relative;
          display: inline-block;
          width: 40px;
          height: 24px;
          float:right;
          margin-right:10px;
        }

        /* Hide default HTML checkbox */
        .switch input {display:none;}

        /* The slider */
        .slider {
          position: absolute;
          cursor: pointer;
          top: 0;
          left: 0;
          right: 0;
          bottom: 0;
          background-color: #ccc;
          -webkit-transition: .4s;
          transition: .4s;
        }

        .slider:before {
          position: absolute;
          content: "";
          height: 16px;
          width: 16px;
          left: 4px;
          bottom: 4px;
          background-color: white;
          -webkit-transition: .4s;
          transition: .4s;
        }

        input.default:checked + .slider {
          background-color: #444;
        }
        input.primary:checked + .slider {
          background-color: #2196F3;
        }
        input.success:checked + .slider {
          background-color: #42A841;
        }
        input.info:checked + .slider {
          background-color: #3de0f5;
        }
        input.warning:checked + .slider {
          background-color: #FFC107;
        }
        input.danger:checked + .slider {
          background-color: #f44336;
        }

        input:focus + .slider {
          box-shadow: 0 0 1px #2196F3;
        }

        input:checked + .slider:before {
          -webkit-transform: translateX(16px);
          -ms-transform: translateX(16px);
          transform: translateX(16px);
        }

        /* Rounded sliders */
        .slider.round {
          border-radius: 34px;
        }

        .slider.round:before {
          border-radius: 50%;
        }

        /* C U S T O M:   F O N T   S I Z E  C O N T R O L S  */
        .col-form-label, .form-control, .btn, td, th{
            font-size:small !important;
        }

        .btn-success{
            background: #42A841;
        }

        .btn-danger{
            color:#de2519;
            background:#FFF;
            border-color:#de2519;
        }

        .navbar{
            margin: 0px !important;
        }

        .nav-item{
            padding-bottom: 5px !important;
        }

    </style>

    <script src="https://code.jquery.com/jquery-3.2.1.slim.min.js" integrity="sha384-KJ3o2DKtIkvYIK3UENzmM7KCkRr/rE9/Qpg6aAZGJwFDMVNA/GpGFF93hXpG5KkN" crossorigin="anonymous"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.12.9/umd/popper.min.js" integrity="sha384-ApNbgh9B+Y1QKtv3Rn7W3mgPxhU9K/ScQsAP7hUibX39j7fakFPskvXusvfa0b4Q" crossorigin="anonymous"></script>
    <script src="<%= Page.ResolveUrl("~/Contents/bootstrap/js/bootstrap.js")%>"></script>
    
    <%-- M O S T R A R   /   O C U L T A R --%>
    <script>
        //MOSTRAR CAPA
        function Mostrar_Capa(id_capa) {
            var capa = document.getElementById(id_capa);
            
            if (capa.style.display !== 'block')
                capa.style.display = 'block';
        }

        //OCULTAR CAPA
        function Ocultar_Capa(id_capa) {
            var capa = document.getElementById(id_capa);

            if (capa.style.display != 'none')
                capa.style.display = 'none';
        }

        // MOSTRAR DIÁLOGO
        function Mostrar_Dialogo(tipo_dialogo, texto) {
            
            var ventana_dialogo = $('#dv_dialogo');
            var clase_anterior = ventana_dialogo.attr('class').match(/dialogo_[a-z]+/g);


            // elimina la clase anterior
            if (clase_anterior != null) ventana_dialogo.removeClass(clase_anterior.join());

            // segun el tipo de dialogo se aplica una de las clases predefinidas
            // en css StylePrincipal
            switch (tipo_dialogo) {
                case 'informacion':
                    ventana_dialogo.addClass('dialogo_info');
                    $('[id*=btnAceptarDialogo]').addClass('oculto');
                    $('#btnCancelarDialogo').addClass('btn-success');
                    $('#btnCancelarDialogo').attr('value', 'Aceptar');
                    break;
                case 'pregunta':
                    ventana_dialogo.addClass('dialogo_preg');
                    $('[id*=btnAceptarDialogo]').removeClass('oculto');
                    $('#btnCancelarDialogo').removeClass('btn-success');
                    $('#btnCancelarDialogo').addClass('btn-default');
                    $('#btnCancelarDialogo').attr('value', 'Cancelar');
                    break;
                case 'alerta':
                    ventana_dialogo.addClass('dialogo_alert');
                    $('[id*=btnAceptarDialogo]').addClass('oculto');
                    $('#btnCancelarDialogo').addClass('btn-success');
                    $('#btnCancelarDialogo').attr('value', 'Aceptar');
                    break;
                case 'error':
                    ventana_dialogo.addClass('dialogo_error');
                    $('[id*=btnAceptarDialogo]').addClass('oculto');
                    $('#btnCancelarDialogo').addClass('btn-success');
                    $('#btnCancelarDialogo').attr('value', 'Aceptar');
                    break;
            }

            // configura el texto del dialogo
            $get('txt_dialogo').innerHTML = texto;
            //console.log(ventana_dialogo);
            ventana_dialogo.modal('show');
        }

        function Ocultar_Dialogo() {
            var ventana_dialogo = $('#dv_dialogo');
            ventana_dialogo.modal('hide');
        }

    </script>


    <script>
        $(function () {
            // Setup drop down menu
            $('.dropdown-toggle').dropdown();

            // Fix input element click problem
            $('.dropdown input, .dropdown label').click(function (e) {
                e.stopPropagation();
            });

            //setNavigation();
        });

        function ValidateInput(source, args) {

            var controlName = source.controltovalidate;
            var control = $('#' + controlName);

            if (control != null && (control.is('input:text') || control.is('input:password'))) {
                if (control.val() == "") {
                    control.addClass("validation");
                    args.IsValid = false;
                }
                else {
                    control.removeClass("validation");
                    args.IsValid = true;
                }
            }
            else if (control != null && control[0].type === "select-one") {
                //console.log("valor: "+ control.val());
                if (control.val() == "") {
                    control.addClass("validation");
                    args.IsValid = false;
                }
                else {
                    control.removeClass("validation");
                    args.IsValid = true;
                }
            }

            if (args.IsValid) {
                $("#txt_dialogo").text("");
               // $("#txt_dialogo").removeClass("validationMsj");
            }
            else {
                $("#txt_dialogo").text("Valor requerido");
                //$("#txt_dialogo").addClass("validationMsj");
            }
        }

        function viewPassword(control) {
                    var x;
                    if(control == "1")
                        x = document.getElementById("<%=txtContrasenia.ClientID%>");
                    

                    $("i", "#viewPassword"+control).removeClass();

                    //console.log($("i", "#viewPassword" + control));
                    $(this).removeClass();

                    if (x.type === "password") {
                        x.type = "text";
                        $("i", "#viewPassword" + control).addClass("input-group-text fas fa-eye-slash");
                    } else {
                        x.type = "password";
                        $("i", "#viewPassword" + control).addClass("input-group-text fas fa-eye");
                    }
                }


        (function (w, d, s, l, i) {
            w[l] = w[l] || []; w[l].push({
                'gtm.start': new Date().getTime(), event: 'gtm.js'
            }); var f = d.getElementsByTagName(s)[0],
                    j = d.createElement(s), dl = l != 'dataLayer' ? '&l=' + l : ''; j.async = true;
            j.src = '//www.googletagmanager.com/gtm.js?id=' + i + dl; f.parentNode.insertBefore(j, f);
        })
            (window, document, 'script', 'dataLayer', 'GTM-P457KB');
    </script>
</head>
<body  style="background-image:none !important;">
    <script src="https://www.google.com/recaptcha/api.js?onload=renderRecaptcha&render=explicit" async defer></script>
    <script type="text/javascript">  

        var reCaptchaCallback = function (response) {
            if (response !== '') {
                $("[id*=txtCaptcha]").val(response);
                $("[id*=rfvCaptcha]").hide();
            }
        };

        var renderRecaptcha = function () {
            grecaptcha.render('ReCaptchContainer', {  
                'sitekey': '<%=ReCaptcha_Key %>',  
                'callback': reCaptchaCallback,  
                theme: 'light', //light or dark    
                //type: 'image',// image or audio    
                size: 'normal'//normal or compact    
            });  
        };  
    </script>

    <form id="form1" runat="server">
    <div class="container" style="padding-left: 0px; padding-top: 0px; padding-right: 0px;">
        <div class="form-horizontal">
            
            <%--  E - M A I L --%>
            <div class="form-group">
                <label for="txtUsuario" class="col-xs-4 col-form-label" style="padding-right:0px !important;">Usuario:</label>
                <div class="col-xs-8" style="padding-left:0px !important;">
                    <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control" placeholder="e-mail" 
                            onkeydown="return (event.keyCode!=13);" ClientIDMode="Static" />
                    <asp:CustomValidator ID="cvtxtEmailAlta" runat="server" ErrorMessage="" Display="None" ValidationGroup="Login"
                                            ClientValidationFunction="ValidateInput" ControlToValidate="txtUsuario" ValidateEmptyText="true">
                    </asp:CustomValidator>                                                                       
                </div>
            </div>

            <%-- C O N T R A S E Ñ A --%>
            <div class="form-group row">
                <label for="txtContrasenia" class="col-xs-4 col-form-label" style="padding-right:0px !important;">Contraseña:</label>
                <div class="col-xs-8" style="padding-left:0px !important;">
                    <div class="input-group mb-3">
                        <asp:TextBox ID="txtContrasenia" runat="server" CssClass="form-control" placeholder="contraseña" TextMode="Password" 
                            onkeydown="return (event.keyCode!=13);" ClientIDMode="Static" />
                        <div class="input-group-append" id="viewPassword1" style="padding:0px !important;">
                                <i class="input-group-text fas fa-eye" aria-hidden="true" onclick="viewPassword('1');"></i>
                        </div>
                        <asp:CustomValidator ID="cvtxtContrasenia" runat="server" ErrorMessage="" Display="None" ValidationGroup="Login"
                                            ClientValidationFunction="ValidateInput" ControlToValidate="txtContrasenia" ValidateEmptyText="true">                                          
                        </asp:CustomValidator> 
                    </div>                                              
                </div>
            </div>
            
            <%-- C A P T C H A --%>
            <div class="form-group row">
                <div class="col-xs-12">
                    <asp:TextBox ID="txtCaptcha" runat="server" ClientIDMode="Static" Visible="false"/> 
                    <asp:RequiredFieldValidator ID="rfvCaptcha" ErrorMessage="La validación del Captcha es requerida." ControlToValidate="txtCaptcha"
                        runat="server" ForeColor="Red" Display="Dynamic" ValidationGroup="Buscar" ClientIDMode="Static" CssClass="help-block">*</asp:RequiredFieldValidator>
                    <div id="ReCaptchContainer" class="pull-right"></div> 
                </div>
            </div>

            <%-- R E C U E R D A M E --%>
            <div class="row" style="display:none;">
                <div class="col-xs-12">
                    <label class="switch pull-left">
                        <input type="checkbox" class="success"/>
                        <span class="slider round"></span>
                    </label>
                    <label class="title">Recordarme</label>
                    <span id="txt_dialogo" runat="server" class="validationMsj pull-right"></span>
                </div>
            </div>
                            
            <%-- R E G I S T R A R  /  R E C O R D A R    C O N ... --%> 
            <div class="row">
                <div class="col-xs-12">
                    <a href="NuevoUsuario.aspx" target="_parent" class="btn btn-default title pull-left">Registrarse</a>
                    <a href="RecordarContrasenia.aspx" target="_parent" class="btn btn-default title pull-right">Olvidé mi contraseña</a>
                </div>
            </div>  

            <%-- I N I C I A R   S E S I Ó N --%>
            <div class="row">
                <div class="col-xs-12">
                    <asp:Button runat="server" ID="btnIniciarSesion" Text="Iniciar Sesión" ValidationGroup="Login"
                         CssClass="btn btn-success pull-right" OnClick="btnIniciarSesion_Click" />
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
