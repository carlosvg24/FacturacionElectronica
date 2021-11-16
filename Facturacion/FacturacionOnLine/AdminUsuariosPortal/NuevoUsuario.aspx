<%@ Page Title="Registrarse" Language="C#"  MasterPageFile="~/Master/VIVA.Master" AutoEventWireup="true" CodeBehind="NuevoUsuario.aspx.cs" Inherits="FacturacionOnLine.NuevoUsuario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.0/css/bootstrap.min.css">--%>
    <link rel="stylesheet" href="<%= Page.ResolveUrl("~/Contents/bootstrap/css/bootstrap-modal-3.4.css")%>" />

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.0/js/bootstrap.min.js"></script>

    <script>
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
            if (control == "2")
                x = document.getElementById("<%=txtBxConfirmarContrasenia.ClientID%>");

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
    </script>

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
            $get('txt_dialogo2').innerHTML = texto;
            //console.log(ventana_dialogo);
            //ventana_dialogo.modal('show');
           
            $('#dv_dialogo').modal('toggle');
        }

        function Ocultar_Dialogo() {
            var ventana_dialogo = $('#dv_dialogo');
            ventana_dialogo.modal('hide');
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always" EnableViewState="true" ViewStateMode="Enabled">
        <ContentTemplate>
            <div class="form-horizontal">
                <div id="pnlDatosReservacion" class="panel panel-success" style="box-shadow: -1px 0px 8px 0px black;">
                    <div class="panel-heading" style="font-size: 12pt;">Registrarse</div>
                    <div class="panel-footer">
                        Por favor ingrese los siguientes datos para registrarse.
                    </div>
                    <div class="panel-body">
                        <%-- E R R O R E S --%>
                        <div class="row">
                            <div class="col-xs-12 text-center">
                                <span id="txt_dialogo"></span>
                                <asp:regularexpressionvalidator display="Dynamic" id="revtxtBxContrasenia" controltovalidate="txtBxContrasenia" runat="server" 
                                            ErrorMessage="La contraseña debe ser de 8 a 10 carácteres</br> con al menos un carácter númerico." forecolor="#DE2519" 
                                            validationexpression="(?!^[0-9]*$)(?!^[a-zA-Z]*$)^([a-zA-Z0-9]{8,10})$" ></asp:regularexpressionvalidator>
                            </div>                            
                        </div>
                        
                        <%-- E M A I L --%>
                        <div class="form-group row">
                            <label for="txtEmailAlta" class="col-xs-3 col-xs-offset-1 col-form-label text-right">e-mail</label>
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
                        <%-- C O N F I R M A R    A L T A --%>
                        <div class="form-group row">
                            <label for="txtConfirmarEmailAlta" class="col-xs-3 col-xs-offset-1 col-form-label text-right">Confirmar e-mail</label>
                            <div class="col-xs-6">
                                <asp:TextBox runat="server" ID="txtConfirmarEmailAlta" CssClass="form-control" 
                                    placeholder="confirmar e-mail" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                            </div>
                        </div>
                        <%-- C O N T R A S E Ñ A --%>
                        <div class="form-group row">
                            <label for="txtBxContrasenia" class="col-xs-3 col-xs-offset-1 col-form-label text-right">Contraseña</label>
                            <div class="col-xs-6">
                                <div class="input-group mb-3">
                                    <asp:TextBox runat="server" ID="txtBxContrasenia" CssClass="form-control" 
                                        placeholder="contraseña" TextMode="Password" onkeydown="return (event.keyCode!=13);">
                                    </asp:TextBox>
                                    <div class="input-group-append" id="viewPassword1" style="padding:0px !important;">
                                        <i class="input-group-text fas fa-eye" aria-hidden="true" onclick="viewPassword('1');"  style="font-size:x-large !important;"></i>
                                    </div>
                                    <asp:CustomValidator ID="cvtxtBxContrasenia" runat="server" ErrorMessage="*" Display="None" ValidationGroup="UsuarioAlta"
                                        ClientValidationFunction="ValidateInput" ForeColor="#DE2519" ControlToValidate="txtBxContrasenia" ValidateEmptyText="true">
                                    </asp:CustomValidator>  
                                </div>                                      
                            </div>
                        </div>
                        <%-- C O N F I R M A R   C O N T R A S E Ñ A --%>
                        <div class="form-group row">
                            <label for="txtBxConfirmarContrasenia" class="col-xs-3 col-xs-offset-1 col-form-label text-right">Confirmar Contraseña</label>
                            <div class="col-xs-6">
                                <div class="input-group mb-3">
                                    <asp:TextBox runat="server" ID="txtBxConfirmarContrasenia" CssClass="form-control"
                                            placeholder="confirmar contraseña" TextMode="Password" onkeydown="return (event.keyCode!=13);" >
                                    </asp:TextBox>
                                    <div class="input-group-append" id="viewPassword2" style="padding:0px !important;">
                                            <i class="input-group-text fas fa-eye" aria-hidden="true" onclick="viewPassword('2');"  style="font-size:x-large !important;"></i>
                                        </div>
                                </div>                                            
                            </div>
                        </div>
                        <%-- B O T O N E S --%>
                        <div class="row">
                            <div class="col-xs-12 text-center">
                                <asp:Button ID="btnCancelar" runat="server" Text="No por el momento" CssClass="btn btn-secondary" data-dismiss="modal"
                                         OnClientClick="$('#txt_dialogo').removeClass('error'); window.history.go(-1); return false;" />
                                    
                                <asp:Button ID="btnRegistrarse" runat="server" CssClass="btn btn-success" Text="Crear Usuario" ValidationGroup="UsuarioAlta"
                                    OnClick="btnRegistrarse_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
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
                  
                <!-- Modal body-->
                <div class="modal-body text-center">
                    <span id="txt_dialogo2"></span>
                </div>

                <!-- Modal footer-->
                <div class="modal-footer" style="border: none;">
                    <asp:HiddenField ID="hdn_dialogo" runat="server" ClientIDMode="Static" />
                    <div class="row">
                        <div class="col-xs-12 text-center">
                            <asp:Button ID="btnCancelarDialogo" runat="server" Text="Aceptar" class="btn btn-secondary" data-dismiss="modal" OnClientClick="window.history.back(); return false;" />
                            <asp:Button ID="btnAceptarDialogo" runat="server" Text="Si, Darme de alta" CssClass="btn btn-success" CausesValidation="false"
                                OnClientClick="window.history.back(); return false;" />
                            <asp:HiddenField ID="hdn_confirmacion" runat="server" />
                        </div>
                    </div>
                </div>  
            </div>
        </div>
    </div>
    <!-- E N D   M O D A L -->
</asp:Content>