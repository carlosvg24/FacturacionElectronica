<%@ Page Title="Facturación Electrónica" Language="C#" MasterPageFile="~/Master/OLD.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="A.aspx.cs" Inherits="FacturacionOnLine.A" ValidateRequest="false" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%= Page.ResolveUrl("~/Contents/css/bootstrap-datepicker.css")%>" rel="stylesheet" />
    <link href="<%= Page.ResolveUrl("~/Contents/css/bootstrap-select.css")%>" rel="stylesheet" />
    
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
            position: fixed;
            top: 30%;
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
        .validationMsj { color: #DE2519; }
    </style>

    

    <%--SCRIPTS NUEVOS--%>
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

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div id="fade">
    </div>
    <div id="modal" style="align-items: center; justify-content: center">
        <img src="<%= Page.ResolveUrl("~/Contents/Images/Loading.gif")%>" style="border: none; position: center" alt="Loading..." />
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" EnableViewState="true" RenderMode="Block" ViewStateMode="Enabled">
        <ContentTemplate>
            <div class="form-horizontal">
                <fieldset style="border: none!important;">
                    <%--<div class="row">
                        <div class="col-xs-12">
                            <asp:LinkButton runat="server" ID="btnRegistrarse" 
                                 CssClass="btn btn-danger pull-right" OnClick="btnRegistrarse_Click">
                                <b>Registrarse en la nueva <br /> experiencia de facturación</b>
                            </asp:LinkButton>
                        </div>
                    </div>--%>
                    <div class="form-group">
                        <div class="col-lg-12">
                            <asp:ValidationSummary ID="vlsErrores" ClientIDMode="Static" runat="server" CssClass="ValidationErrors" ValidationGroup="Guardar" ShowMessageBox="false" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-lg-6">
                            <div class="form-group">
                                <asp:Label ID="lblPNR" runat="server" Text="Clave de confirmación:" CssClass="control-label col-lg-3" AssociatedControlID="txtPNR" ClientIDMode="Static" />
                                <div class="col-lg-8">
                                    <asp:TextBox ID="txtPNR" runat="server" CssClass="form-control" ClientIDMode="Static" MaxLength="6" />
                                </div>
                                <div class="col-lg-1">
                                    <asp:RequiredFieldValidator ID="rfvPNR" runat="server" ControlToValidate="txtPNR"
                                        ErrorMessage="La clave de confirmación es un campo requerido" CssClass="help-block"
                                        ClientIDMode="Static" ValidationGroup="Guardar"
                                        Text="*" ToolTip="Agregue la clave de confirmación" Display="Dynamic" data-toggle="tooltip" />
                                    <asp:RegularExpressionValidator ID="revPNR" runat="server" ControlToValidate="txtPNR" ClientIDMode="Static" ValidationGroup="Guardar" Text="*"
                                        Display="Dynamic" data-toggle="tooltip" CssClass="help-block" ErrorMessage="La clave de confirmación tiene formato no valido" ToolTip="La clave de confirmación tiene formato no valido"
                                        ValidationExpression="^([A-Za-z0-9]{6})$" />
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-6">
                            <div class="form-group">
                                <asp:Label ID="lblRFC" runat="server" Text="RFC a Facturar:" CssClass="control-label col-lg-3" AssociatedControlID="txtRFC" ClientIDMode="Static" />
                                <div class="col-lg-8">
                                    <asp:TextBox ID="txtRFC" runat="server" CssClass="form-control" ClientIDMode="Static" MaxLength="13" />
                                </div>
                                <div class="col-lg-1">
                                    <asp:RequiredFieldValidator ID="rfvRFC" runat="server" ControlToValidate="txtRFC"
                                        ErrorMessage="El RFC es un campo requerido" CssClass="help-block"
                                        ClientIDMode="Static" ValidationGroup="Guardar"
                                        Text="*" ToolTip="Agregue el RFC" Display="Dynamic" data-toggle="tooltip" />
                                    <asp:RegularExpressionValidator ID="revRFC" runat="server" ControlToValidate="txtRFC" ClientIDMode="Static" ValidationGroup="Guardar" Text="*"
                                        Display="Dynamic" data-toggle="tooltip" CssClass="help-block" ErrorMessage="El RFC tiene formato no valido" ToolTip="El RFC tiene formato no valido"
                                        ValidationExpression="^([A-ZÑ&]{3,4}) ?(?:- ?)?(\d{2}(?:0[1-9]|1[0-2])(?:0[1-9]|[12]\d|3[01])) ?(?:- ?)?([A-Z\d]{2})([A\d])$" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-lg-6">
                            <div class="form-group">
                                <asp:Label ID="lblNombre" runat="server" Text="Nombre(s) Pasajero:" CssClass="control-label col-lg-3" AssociatedControlID="txtNombre" ClientIDMode="Static" />
                                <div class="col-lg-8">
                                    <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" ClientIDMode="Static" ToolTip ="Dato requerido para identificar la reservación, no aparecerá en la factura..."/>
                                </div>
                                <div class="col-lg-1">
                                    <asp:RequiredFieldValidator ID="rfvNombre" runat="server" ControlToValidate="txtNombre"
                                        ErrorMessage="Nombre es un campo requerido" CssClass="help-block"
                                        ClientIDMode="Static" ValidationGroup="Guardar"
                                        Text="*" ToolTip="Agregue el nombre" Display="Dynamic" data-toggle="tooltip" />
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-6">
                            <div class="form-group">
                                <asp:Label ID="lblApellidos" runat="server" Text="Apellido(s) Pasajero:" CssClass="control-label col-lg-3" AssociatedControlID="txtApellidos" ClientIDMode="Static" />
                                <div class="col-lg-8">
                                    <asp:TextBox ID="txtApellidos" runat="server" CssClass="form-control" ClientIDMode="Static" ToolTip ="Dato requerido para identificar la reservación, no aparecerá en la factura..."/>
                                </div>
                                <div class="col-lg-1">
                                    <asp:RequiredFieldValidator ID="rfvApellidos" runat="server" ControlToValidate="txtApellidos"
                                        ErrorMessage="Apellido(s) es un campo requerido" CssClass="help-block"
                                        ClientIDMode="Static" ValidationGroup="Guardar"
                                        Text="*" ToolTip="Agregue el(los) apellido(s)" Display="Dynamic" data-toggle="tooltip" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-lg-6">
                            <div class="form-group">
                                <asp:Label ID="lblUsoCFDI" runat="server" Text="Uso CFDI:" CssClass="control-label col-lg-3" AssociatedControlID="ddlUsoCFDI" ClientIDMode="Static" />
                                <div class="col-lg-8">
                                    <asp:DropDownList ID="ddlUsoCFDI" runat="server" ClientIDMode="Static" CssClass="form-control" />
                                </div>
                                <div class="col-lg-1">
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-6">
                            <div class="form-group">
                                <asp:Label ID="lblSoyExtranjero" runat="server" Text="Factura Entidad Extranjera:" CssClass="control-label col-lg-3" AssociatedControlID="chkSoyExtranjero" ClientIDMode="Static" />
                                <div class="col-lg-8">
                                    <asp:CheckBox ID="chkSoyExtranjero" runat="server" ClientIDMode="Static" Checked="false" onclick="cambiarExtra(this);" />
                                    <asp:Label ID="lblExtranjerochk" runat="server" Text="Si" AssociatedControlID="chkSoyExtranjero" ClientIDMode="Static" />
                                </div>

                            </div>
                        </div>
                    </div>
                    <div class="form-group oculto2" id="divExtranjero">
                        <div class="col-lg-6">
                            <div class="form-group">
                                <asp:Label ID="lblTaxID" runat="server" Text="TAX ID:" CssClass="control-label col-lg-3" AssociatedControlID="txtTaxID" ClientIDMode="Static" />
                                <div class="col-lg-8">
                                    <asp:TextBox ID="txtTaxID" runat="server" CssClass="form-control" ClientIDMode="Static" MaxLength="40" />
                                </div>
                                <div class="col-lg-1">
                                    <asp:RequiredFieldValidator ID="rfvTaxID" runat="server" ControlToValidate="txtTaxID"
                                        ErrorMessage="El TAX ID es un campo requerido" CssClass="help-block"
                                        ClientIDMode="Static" ValidationGroup="Guardar"
                                        Text="*" ToolTip="Agregue el TAX ID" Display="Dynamic" data-toggle="tooltip" Enabled="false" />
                                    <asp:RegularExpressionValidator ID="revTaxID" runat="server" ControlToValidate="txtTaxID" ClientIDMode="Static" ValidationGroup="Guardar" Text="*"
                                        Display="Dynamic" data-toggle="tooltip" CssClass="help-block" ErrorMessage="El TAX ID tiene formato no valido" ToolTip="El TAX ID tiene formato no valido"
                                        ValidationExpression="^([A-Za-z0-9]{1,40})$" />
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-6">
                            <div class="form-group">
                                <asp:Label ID="lblPaisReferencia" runat="server" Text="País de Residencia:" CssClass="control-label col-lg-3" AssociatedControlID="txtNombre" ClientIDMode="Static" />
                                <div class="col-lg-8">
                                    <asp:DropDownList ID="ddlPaisReferencia" runat="server" CssClass="form-control" ClientIDMode="Static" />
                                </div>
                                <div class="col-lg-1">
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-lg-6">
                            <div class="form-group">
                                <asp:Label ID="lblEMail" runat="server" Text="EMail:" CssClass="control-label col-lg-3" AssociatedControlID="txtEMail" ClientIDMode="Static" />
                                <div class="col-lg-8">
                                    <asp:TextBox ID="txtEMail" runat="server" CssClass="form-control" ClientIDMode="Static" />
                                </div>
                                <div class="col-lg-1">
                                    <asp:RequiredFieldValidator ID="rfvEMail" runat="server" ControlToValidate="txtEMail"
                                        ErrorMessage="EMail es un campo requerido" CssClass="help-block"
                                        ClientIDMode="Static" ValidationGroup="Guardar"
                                        Text="*" ToolTip="Agregue el EMail" Display="Dynamic" data-toggle="tooltip" />
                                    <asp:RegularExpressionValidator ID="revEMail" runat="server" ControlToValidate="txtEMail" ClientIDMode="Static" ValidationGroup="Guardar" Text="*"
                                        Display="Dynamic" data-toggle="tooltip" CssClass="help-block" ErrorMessage="El EMail tiene formato no valido" ToolTip="El EMail tiene formato no valido"
                                        ValidationExpression="[a-z0-9]+([-+._][a-z0-9]+){0,2}@.*?(\.(a(?:[cdefgilmnoqrstuwxz]|ero|(?:rp|si)a)|b(?:[abdefghijmnorstvwyz]iz)|c(?:[acdfghiklmnoruvxyz]|at|o(?:m|op))|d[ejkmoz]|e(?:[ceghrstu]|du)|f[ijkmor]|g(?:[abdefghilmnpqrstuwy]|ov)|h[kmnrtu]|i(?:[delmnoqrst]|n(?:fo|t))|j(?:[emop]|obs)|k[eghimnprwyz]|l[abcikrstuvy]|m(?:[acdeghklmnopqrstuvwxyz]|il|obi|useum)|n(?:[acefgilopruz]|ame|et)|o(?:m|rg)|p(?:[aefghklmnrstwy]|ro)|qa|r[eosuw]|s[abcdeghijklmnortuvyz]|t(?:[cdfghjklmnoprtvwz]|(?:rav)?el)|u[agkmsyz]|v[aceginu]|w[fs]|y[etu]|z[amw])\b){1,2}" />
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-6">
                            <div class="form-group">
                                <asp:Label ID="lblEMailConf" runat="server" Text="Confirmar EMail:" CssClass="control-label col-lg-3" AssociatedControlID="txtEmailConf" ClientIDMode="Static" />
                                <div class="col-lg-8">
                                    <asp:TextBox ID="txtEmailConf" runat="server" CssClass="form-control" ClientIDMode="Static" />
                                </div>
                                <div class="col-lg-1">
                                    <asp:RequiredFieldValidator ID="rfvEMailConf" runat="server" ControlToValidate="txtEmailConf"
                                        ErrorMessage="EMail (Confirmación) es un campo requerido" CssClass="help-block"
                                        ClientIDMode="Static" ValidationGroup="Guardar"
                                        Text="*" ToolTip="Agregue el EMail" Display="Dynamic" data-toggle="tooltip" />
                                    <asp:RegularExpressionValidator ID="revEMailConf" runat="server" ControlToValidate="txtEMailConf" ClientIDMode="Static" ValidationGroup="Guardar" Text="*"
                                        Display="Dynamic" data-toggle="tooltip" CssClass="help-block" ErrorMessage="El EMail(Confirmación) tiene formato no valido" ToolTip="El EMail(Confirmación) tiene formato no valido"
                                        ValidationExpression="[a-z0-9]+([-+._][a-z0-9]+){0,2}@.*?(\.(a(?:[cdefgilmnoqrstuwxz]|ero|(?:rp|si)a)|b(?:[abdefghijmnorstvwyz]iz)|c(?:[acdfghiklmnoruvxyz]|at|o(?:m|op))|d[ejkmoz]|e(?:[ceghrstu]|du)|f[ijkmor]|g(?:[abdefghilmnpqrstuwy]|ov)|h[kmnrtu]|i(?:[delmnoqrst]|n(?:fo|t))|j(?:[emop]|obs)|k[eghimnprwyz]|l[abcikrstuvy]|m(?:[acdeghklmnopqrstuvwxyz]|il|obi|useum)|n(?:[acefgilopruz]|ame|et)|o(?:m|rg)|p(?:[aefghklmnrstwy]|ro)|qa|r[eosuw]|s[abcdeghijklmnortuvyz]|t(?:[cdfghjklmnoprtvwz]|(?:rav)?el)|u[agkmsyz]|v[aceginu]|w[fs]|y[etu]|z[amw])\b){1,2}" />
                                    <asp:CompareValidator ID="cmvEMailConf" runat="server" ControlToValidate="txtEMail" ControlToCompare="txtEMailConf" Display="Dynamic" data-toggle="tooltip" CssClass="help-block"
                                        ClientIDMode="Static" ValidationGroup="Guardar" Text="*" ToolTip="El EMail ingesado no es igual" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-lg-6">
                            <div class="form-group">
                                <div id="divCaptcha" runat="server" class="col-lg-12 col-md-offset-3">
                                    <asp:Panel ID="pnlCaptcha" runat="server">
                                        <div id="dvCaptcha">
                                        </div>
                                        <asp:TextBox ID="txtCaptcha" runat="server" Style="display: none" ClientIDMode="Static" />
                                        <asp:RequiredFieldValidator ID="rfvCaptcha" ErrorMessage="La validación del Captcha es requerida." ControlToValidate="txtCaptcha"
                                            runat="server" ForeColor="Red" Display="Dynamic" ValidationGroup="Guardar" ClientIDMode="Static" CssClass="help-block" />
                                    </asp:Panel>
                                    <asp:Panel ID="pnlPassOTA" runat="server">
                                        <div class="form-group">
                                            <div class="col-lg-8">
                                                <asp:TextBox ID="txtPasswordOTA" runat="server" CssClass="form-control" ClientIDMode="Static" />
                                            </div>
                                            <div class="col-lg-1">
                                                <asp:RequiredFieldValidator ID="rfvPassOTA" runat="server" ControlToValidate="txtPasswordOTA"
                                                    ErrorMessage="El password es requerido..." CssClass="help-block"
                                                    ClientIDMode="Static" ValidationGroup="Guardar"
                                                    Text="*" ToolTip="Capture el password asignado" Display="Dynamic" data-toggle="tooltip" />
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-6">
                            <div class="form-group">
                                <div class="col-lg-12 col-md-offset-4">
                                    <asp:LinkButton ID="lkbBuscar" runat="server" Text="Continuar" ClientIDMode="Static" CssClass="btn btn-success" ValidationGroup="Guardar" OnClick="lkbBuscar_Click" OnClientClick="openModal();" ToolTip="Buscar pagos para facturar" data-toggle="tooltip" />
                                </div>
                            </div>
                        </div>

                    </div>
                    <asp:HiddenField ID="hdnPostback" runat="server" />
                    <asp:HiddenField ID="hdnOcultarCaptcha" runat="server" />
                </fieldset>
            </div>
            <script type="text/javascript" src="https://www.google.com/recaptcha/api.js?onload=onloadCallback&render=explicit" async defer></script>
            <script type="text/javascript">
                var onloadCallback = function () {
                    grecaptcha.render('dvCaptcha', {
                        'sitekey': '<%=ReCaptcha_Key %>',
                        'callback': function (response) {
                            $.ajax({
                                type: "POST",
                                url: "Default.aspx/VerifyCaptcha",
                                data: "{response: '" + response + "'}",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (r) {
                                    var captchaResponse = jQuery.parseJSON(r.d);
                                    if (captchaResponse.success) {
                                        $("[id*=txtCaptcha]").val(captchaResponse.success);
                                        $("[id*=rfvCaptcha]").hide();
                                    } else {
                                        $("[id*=txtCaptcha]").val("");
                                        $("[id*=rfvCaptcha]").show();
                                        var error = captchaResponse["error-codes"][0];
                                        $("[id*=rfvCaptcha]").html("RECaptcha error. " + error);
                                    }
                                }
                            });
                        }
                    });
                };



                function validaOTA(response) {
                    $.ajax({
                        type: "POST",
                        url: "Default.aspx/OcultaCaptcha",
                        data: "{response:'" + response + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (r) {
                            var resultResponse = jQuery.parseJSON(r.d);

                            if (!resultResponse) {
                                $('#<%=pnlCaptcha.ClientID%>').show();
                                $('#<%=pnlPassOTA.ClientID%>').hide();
                            }
                            else {
                                $('#<%=pnlCaptcha.ClientID%>').hide();
                                $('#<%=pnlPassOTA.ClientID%>').show();
                            }

                            ConfigureValidators();
                        }
                    });
                };

                function ConfigureValidators() {
                    if (typeof Page_Validators != 'undefined') {
                        for (i = 0; i <= Page_Validators.length; i++) {
                            if (Page_Validators[i] != null) {
                                var visible = $('#' + Page_Validators[i].controltovalidate).parent().is(':visible');
                                Page_Validators[i].enabled = visible;
                            }
                        }
                    };
                }


                $('#txtPNR').change(function () {

                    validaOTA($('#txtPNR').val());
                });


                $('button').click(function () {
                    var toAdd = $("input[name=message]").val();
                    $('#messages').append("<p>" + toAdd + "</p>");
                });


                jQuery(document).ready(
                    function () {
                        $(".oculto").hide();


                        if ($('#<%=chkSoyExtranjero.ClientID%>').prop('checked')) {
                            $(".oculto2").show();
                        }
                        else {
                            $(".oculto2").hide();
                        }


                        if ($('#<%=chkSoyExtranjero.ClientID%>').prop('disabled')) {

                            if ($('#<%=chkSoyExtranjero.ClientID%>').prop('checked')) {
                                $('#<%=lblExtranjerochk.ClientID%>').html("Sí");
                            }
                            else {
                                $('#<%=lblExtranjerochk.ClientID%>').html("No");
                            }
                        }
                        else {
                            $('#<%=lblExtranjerochk.ClientID%>').html("Sí");
                        }

                        var isOcultarCaptcha = $("#<%=hdnOcultarCaptcha.ClientID%>").val().toLowerCase() === "true";
                        if (isOcultarCaptcha) {
                            $('#<%=pnlPassOTA.ClientID%>').show();
                            $('#<%=pnlCaptcha.ClientID%>').hide();
                        }
                        else {
                            $('#<%=pnlPassOTA.ClientID%>').hide();
                            $('#<%=pnlCaptcha.ClientID%>').show();
                        }
                    }
                );

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
                        $("#txt_dialogo").removeClass("validationMsj");
                    }
                    else {
                        $("#txt_dialogo").text("Valor requerido");
                        $("#txt_dialogo").addClass("validationMsj");
                    }
                }
                
                function viewPassword(control) {
                    var x;
                    if(control == "1")
                        x = document.getElementById("<%=txtBxContrasenia.ClientID%>");
                    else
                        x = document.getElementById("<%=txtBxConfirmarContrasenia.ClientID%>");

                    $("i", "#viewPassword"+control).removeClass();

                    //console.log($("i", "#viewPassword" + control));
                    $(this).removeClass();

                    if (x.type === "password") {
                        x.type = "text";
                        $("i", "#viewPassword" + control).addClass("fas fa-eye-slash");
                    } else {
                        x.type = "password";
                        $("i", "#viewPassword" + control).addClass("fas fa-eye");
                    }
                }
            </script>
            <div id="divGrid" class="form-horizontal oculto" runat="server">
                <fieldset style="border: none!important;">
                    <div class="form-group">
                        <div class="col-lg-12">
                            <span class="titulo_pagina">Pagos pendientes para facturar</span>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-lg-12">
                            <asp:GridView ID="grvFaturas" runat="server" CssClass="table table-striped table-hover"
                                AutoGenerateColumns='False' AllowPaging='True' PageSize='25' AllowSorting='True' OnRowDataBound="grvFaturas_RowDataBound" OnPageIndexChanging="grvFaturas_PageIndexChanging" OnRowCommand="grvFaturas_RowCommand">
                                <Columns>
                                    <asp:TemplateField HeaderText='Facturar' ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkEsFacturado" runat="server" Checked="false" CssClass="CheckGrid" />
                                            <asp:Image ID="imgError" runat="server" ImageUrl="~/Contents/Images/warning.png" Height="30px" Width="30px" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="20px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="FechaPago" HeaderText="Fecha Pago" />
                                    <asp:BoundField DataField="FolioPrefactura" HeaderText="Factura">
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FechaFactura" HeaderText="Fecha Facturación" />
                                    <asp:BoundField DataField="PaymentAmount" HeaderText="Total" />
                                    <asp:BoundField DataField="CurrencyCode" HeaderText="Moneda" />
                                    <asp:TemplateField HeaderText='Descargar' Visible='True' SortExpression='' ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID='lblReenviar' runat='server' CausesValidation='false' CommandArgument='<%#DataBinder.Eval(Container.DataItem,"FolioPrefactura") %>' CommandName="Reimpresion" OnClientClick="openModal();" ToolTip="Descargar la factura" data-toggle="tooltip" Height="30px" Width="30px">
                                            <img src="Contents/Images/envio.png" />
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText='Re-enviar' Visible='True' SortExpression='' ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lblReenviarEmail" runat='server' CausesValidation='false' CommandArgument='<%#DataBinder.Eval(Container.DataItem,"FolioPrefactura") %>' CommandName="EnviarEmail" OnClientClick="openModal();" ToolTip="Re-envia CFDI por correo" data-toggle="tooltip" Height="30px" Width="30px">
                                                  <img src="Contents/Images/EnviarEmail_1.jpg" style="width: 30px;height: 30px;"/>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="EsFacturado" HeaderText="EsFacturado" />
                                    <asp:BoundField DataField="EsFacturable" HeaderText="EsFacturable" />
                                    <asp:TemplateField HeaderText='' Visible='True' SortExpression='' ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID='lblConsultar' runat='server' CausesValidation='false' CommandArgument='<%#DataBinder.Eval(Container.DataItem,"FolioPrefactura") %>' CommandName="Select" Text="Ver Detalle" ClientIDMode="Static" ToolTip="Ver detalle de pagos" data-toggle="tooltip" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="EnVigenciaParaFacturacion" HeaderText="EsVigente" />
                                    <asp:BoundField DataField="Mensaje" HeaderText="Mensaje" />
                                </Columns>
                                <AlternatingRowStyle CssClass="active" />
                                <HeaderStyle CssClass="success" HorizontalAlign="Center" />
                                <PagerStyle HorizontalAlign='Center' />
                                <EmptyDataTemplate>
                                    <div>
                                        <table class="table table-striped table-hover">
                                            <thead>
                                                <tr>
                                                    <th scope="col">Facturar</th>
                                                    <th scope="col">Fecha Pago</th>
                                                    <th scope="col">Factura</th>
                                                    <th scope="col">Fecha Facturación</th>
                                                    <th scope="col">Total</th>
                                                    <th scope="col">Moneda</th>
                                                    <th scope="col">Descargar Factura</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <td colspan="6" style="color: #000; font-size: 16px; text-align: center; font-weight: bold;">No se encontraron pagos que deban ser facturados.</td>
                                            </tbody>
                                            <tfoot>
                                                <tr>
                                                    <th scope="col" colspan="6" style="height: 10px; margin-top: 20px;"></th>
                                                </tr>
                                            </tfoot>
                                        </table>
                                    </div>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                    </div>
                    <div id="divDetalle" runat="server" class="form-group">
                        <div class="col-lg-12">
                            <asp:Label ID="lblDetNum" runat="server" AssociatedControlID="lblDetNum" ClientIDMode="Static" CssClass="control-label" />
                            <asp:GridView ID="grvDetalle" runat="server" CssClass="table table-striped table-hover"
                                AutoGenerateColumns='False' AllowPaging='True' PageSize='50' AllowSorting='True' OnRowDataBound="grvDetalle_RowDataBound">
                                <Columns>
                                    <asp:BoundField DataField="FechaPago" HeaderText="Fecha Pago">
                                        <ItemStyle Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PaymentId" HeaderText="Folio Pago">
                                        <ItemStyle Width="60px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CurrencyCode" HeaderText="Moneda">
                                        <ItemStyle Width="160px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PaymentAmount" HeaderText="Monto">
                                        <ItemStyle Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PaymentMethodCode" HeaderText="Forma Pago">
                                        <ItemStyle Width="200px" />
                                    </asp:BoundField>
                                </Columns>
                                <AlternatingRowStyle CssClass="active" />
                                <HeaderStyle CssClass="success" />
                                <PagerStyle HorizontalAlign='Center' />
                            </asp:GridView>
                        </div>
                        <div class="form-group">
                            <div class="col-lg-12 col-lg-offset-5">
                                <asp:LinkButton ID="lkbCerrar" runat="server" Text="Facturar" ClientIDMode="Static" CssClass="btn btn-default" Visible="false" OnClick="lkbCerrar_Click" ToolTip="Cerrar detalle" data-toggle="tooltip">Cerrar</asp:LinkButton>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-lg-12 col-lg-offset-5">
                            <asp:LinkButton ID="lkbFacturar" runat="server" Text="Facturar" ClientIDMode="Static" CssClass="btn btn-primary" OnClientClick="openModal();if(!FactConfirmation()) {closeModal(); return false;}else{}" OnClick="lkbFacturar_Click" data-toggle="tooltip" ToolTip="Generar facturas de pagos" />
                            <asp:LinkButton ID="lkbCancelar" runat="server" Text="Buscar por otra clave" ClientIDMode="Static" CssClass="btn btn-warning" OnClick="lkbCancelar_Click" ToolTip="Buscar por otra clave de confirmación" data-toggle="tooltip" />
                        </div>
                    </div>
                </fieldset>
            </div>
            <div class="row">
                <div class="form-group" style="background-color: #E0F8E0; margin: 10px; padding: 10px; font-size: smaller;">
                    <p>
                        Este portal no considera el <B>Complemento INE</B> para Partidos Políticos, Coaliciones y Asociaciones Civiles de conformidad con el artículo 46 del Reglamento de Fiscalización del Instituto Nacional Electoral, en caso de requerirlo agradeceremos se comunique a Call Center, donde con gusto lo atenderemos.
                    </p>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="lkbBuscar" />
            <asp:PostBackTrigger ControlID="lkbFacturar" />
            <asp:PostBackTrigger ControlID="lkbCerrar" />
            <asp:PostBackTrigger ControlID="grvFaturas" />
        </Triggers>
    </asp:UpdatePanel>

    <!-- M O D A L -->
    <div id="dv_dialogo" class="modal fade" data-backdrop="static" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">

                <!-- Modal header-->
                <div class="modal-header" style="border: none;">
                    <div id="img_dialogo"></div>
                </div>         
                    
                    <asp:UpdatePanel ID="Up_Dialogo" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                             <!-- Modal body-->
                            <div class="modal-body text-center">
                                <span id="txt_dialogo"></span>
                                <asp:regularexpressionvalidator display="Dynamic" id="revtxtBxContrasenia" controltovalidate="txtBxContrasenia" runat="server" 
                                                    ErrorMessage="La contraseña debe ser de 8 a 10 carácteres</br> con al menos un carácter númerico." forecolor="#DE2519" 
                                                    validationexpression="(?!^[0-9]*$)(?!^[a-zA-Z]*$)^([a-zA-Z0-9]{8,10})$" ></asp:regularexpressionvalidator>
                                <div class="row" id="dvRegistrar" style="display: none;">
                                    <div class="row">
                                        <div class="col-xs-12">
                                            <img src="Contents/Images/logo.png" />
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <label for="txtEmailAlta" class="col-xs-3 col-xs-offset-1 col-form-label">e-mail:</label>
                                        <div class="col-xs-6">
                                            <asp:TextBox runat="server" ID="txtEmailAlta" CssClass="form-control" 
                                                placeholder="e-mail" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                            <asp:CustomValidator ID="cvtxtEmailAlta" runat="server" ErrorMessage="" Display="None" ValidationGroup="UsuarioAlta"
                                                ClientValidationFunction="ValidateInput" ControlToValidate="txtEmailAlta" ValidateEmptyText="true">
                                            </asp:CustomValidator>
                                            <asp:RegularExpressionValidator ID="regexEmailValid" runat="server" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                                                ControlToValidate="txtEmailAlta" ErrorMessage="Formato de e-mail incorrecto" ForeColor="#DE2519" ValidationGroup="UsuarioAlta"></asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <label for="txtConfirmarEmailAlta" class="col-xs-3 col-xs-offset-1 col-form-label">Confirmar e-mail</label>
                                        <div class="col-xs-6">
                                            <asp:TextBox runat="server" ID="txtConfirmarEmailAlta" CssClass="form-control" 
                                                placeholder="confirmar e-mail" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <label for="txtBxContrasenia" class="col-xs-3 col-xs-offset-1 col-form-label">Contraseña</label>
                                        <div class="col-xs-6">
                                            <div class="input-group">
                                                <asp:TextBox runat="server" ID="txtBxContrasenia" CssClass="form-control" 
                                                    placeholder="contraseña" TextMode="Password" onkeydown="return (event.keyCode!=13);">
                                                </asp:TextBox>
                                                
                                                <asp:CustomValidator ID="cvtxtBxContrasenia" runat="server" ErrorMessage="*" Display="None" ValidationGroup="UsuarioAlta"
                                                    ClientValidationFunction="ValidateInput" ForeColor="#DE2519" ControlToValidate="txtBxContrasenia" ValidateEmptyText="true">
                                                </asp:CustomValidator>
                                                  <div class="input-group-addon" id="viewPassword1" style="padding:0px !important;">
                                                      <i class="fas fa-eye" aria-hidden="true" onclick="viewPassword('1');"  style="font-size:x-large !important;"></i>
                                                  </div>
                                            </div>                                          
                                            
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <label for="txtBxConfirmarContrasenia" class="col-xs-3 col-xs-offset-1 col-form-label">Confirmar Contraseña</label>
                                        <div class="col-xs-6">
                                            <div class="input-group">
                                                <asp:TextBox runat="server" ID="txtBxConfirmarContrasenia" CssClass="form-control"
                                                     placeholder="confirmar contraseña" TextMode="Password" onkeydown="return (event.keyCode!=13);" >
                                                </asp:TextBox>
                                                <div class="input-group-addon" id="viewPassword2" style="padding:0px !important;">
                                                      <i class="fas fa-eye" aria-hidden="true" onclick="viewPassword('2');"  style="font-size:x-large !important;"></i>
                                                  </div>
                                            </div>                                            
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-xs-12">
                                            <span style="font-size:large; color:red;">El correo de registro debe estar ligado a tus reservaciones</span>
                                        </div>
                                    </div>
                                </div>
                                <div style="display: none;">
                                    <asp:CheckBox runat="server" ID="chkQuizo" ClientIDMode="Static" Visible="true" />
                                </div>

                            </div>

                            <!-- Modal footer-->
                            <div class="modal-footer" style="border: none;">
                                <asp:HiddenField ID="hdn_dialogo" runat="server" ClientIDMode="Static" />
                                <div class="text-center">
                                    <asp:Button ID="btnCancelarDialogo" runat="server" Text="NO por el momento" class="btn btn-secondary" data-dismiss="modal"
                                         OnClientClick="$('#txt_dialogo').removeClass('error'); Ocultar_Capa('dvRegistrar'); $('#dv_dialogo').modal('hide');" />
                                    <asp:Button ID="btnAceptarDialogo" runat="server" Text="Si, Darme de alta" CssClass="btn btn-success" CausesValidation="false"
                                        OnClick="btnAceptarDialogo_Click" />
                                    <asp:Button ID="btnGuardar" runat="server" CssClass="btn btn-success" Text="Crear Usuario" ValidationGroup="UsuarioAlta" Visible="false" 
                                        OnClientClick="Mostrar_Capa('dvRegistrar');" OnClick="btnGuardar_Click" />
                                    <asp:HiddenField ID="hdn_confirmacion" runat="server" />
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                
            </div>
        </div>
    </div>
    <!-- E N D   M O D A L -->



    <script type="text/javascript">
        function openModal() {
            document.getElementById('modal').style.display = 'block';
            document.getElementById('fade').style.display = 'block';
        }

        function closeModal() {
            document.getElementById('modal').style.display = 'none';
            document.getElementById('fade').style.display = 'none';
        }

        function FactConfirmation() {
            return confirm("¿Está seguro de continuar con el proceso de facturación con la información ingresada?");
        }
    </script>
    <script>
        jQuery(document).ready(function () {
            $('[data-toggle="tooltip"]').tooltip()
            $('[rel=tooltip]').tooltip();

            var isPostback = $("#<%=hdnPostback.ClientID%>").val().toLowerCase() === "true";
            if (isPostback == true) {
                $(".oculto").show();
            }
            else {
                $(".oculto").hide();
            }

            $('#<%=chkSoyExtranjero.ClientID%>').click(function () {
                var nodo = '#divExtranjero';
                if ($(nodo).is(":visible")) {
                    $(nodo).hide();
                } else {
                    $(".oculto2").hide("slow");
                    $(nodo).fadeToggle("fast");
                }
            });
        });

        function cambiarExtra(elinput) {
            if (elinput.checked == true) {
                $('#<%=txtRFC.ClientID%>').val('XEXX010101000');
                ValidatorEnable($('#<%=rfvTaxID.ClientID%>')[0], true);
                $('#<%=txtRFC.ClientID%>').attr("ReadOnly", "True");
            }
            else {
                $('#<%=txtRFC.ClientID%>').val('');
                ValidatorEnable($('#<%=rfvTaxID.ClientID%>')[0], false);
                $('#<%=txtRFC.ClientID%>').removeAttr("ReadOnly");
            }
        }
    </script>
    <script>
        function extendedValidatorUpdateDisplay(obj) {
            if (typeof originalValidatorUpdateDisplay === "function") {
                originalValidatorUpdateDisplay(obj);
            }
            var control = document.getElementById(obj.controltovalidate);
            if (control) {
                var isValid = true;
                for (var i = 0; i < control.Validators.length; i += 1) {
                    if (!control.Validators[i].isvalid) {
                        isValid = false;
                        closeModal();
                        break;
                    }
                }
                if (isValid) {
                    $(control).closest(".form-group").removeClass("has-error");
                } else {
                    $(control).closest(".form-group").addClass("has-error");
                }
            }
        }
        var originalValidatorUpdateDisplay = window.ValidatorUpdateDisplay;
        window.ValidatorUpdateDisplay = extendedValidatorUpdateDisplay;
    </script>
</asp:Content>
