<%@ Page Title="Recordar contraseña" Language="C#" MasterPageFile="~/Master/VIVA.Master" AutoEventWireup="true" CodeBehind="RecordarContrasenia.aspx.cs" Inherits="FacturacionOnLine.AdminUsuariosPortal.RecordarContrasenia" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
                $("#txtErrores").text("");
               $("#txtErrores").removeClass("validationMsj");
            }
            else {
                $("#txtErrores").text("Valor requerido");
                $("#txtErrores").addClass("validationMsj");
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
            $get('txtErrores2').innerHTML = texto;
           
            $('#dv_dialogo').modal('toggle');
        }

        function Ocultar_Dialogo() {
            var ventana_dialogo = $('#dv_dialogo');
            ventana_dialogo.modal('hide');
        }

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form-horizontal">
        <div id="pnlDatosReservacion" class="panel panel-success" style="box-shadow: -1px 0px 8px 0px black;">
            <div class="panel-heading" style="font-size: 12pt;">Recuperar contraseña</div>
            <div class="panel-footer">
                Por favor ingresa tu email.
            </div>

            <div class="panel-body">
                <%-- E R R O R E S --%>
                <div class="row">
                    <div class="col-xs-12 text-center">
                        <span id="txtErrores" runat="server"></span>

                        <%-- E M A I L --%>
                        <div class="form-group row">
                            <label for="txtEmail" class="col-xs-3 col-xs-offset-1 col-form-label text-right">e-mail:</label>
                            <div class="col-xs-6">
                                <asp:TextBox runat="server" ID="txtEmail" CssClass="form-control" 
                                    placeholder="e-mail" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                <asp:CustomValidator ID="cvtxtEmail" runat="server" ErrorMessage="" Display="None" ValidationGroup="ConsUsuario"
                                    ClientValidationFunction="ValidateInput" ControlToValidate="txtEmail" ValidateEmptyText="true">
                                </asp:CustomValidator>
                                <asp:RegularExpressionValidator ID="regexEmailValid" runat="server" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                                    ControlToValidate="txtEmail" ErrorMessage="Formato de e-mail incorrecto" ForeColor="#DE2519" ValidationGroup="ConsUsuario"></asp:RegularExpressionValidator>
                            </div>
                        </div>

                        <%-- B O T O N E S --%>
                        <div class="row">
                            <div class="col-xs-12 text-center">
                                <asp:Button ID="btnCancelar" runat="server" Text="Regresar" CssClass="btn btn-secondary" data-dismiss="modal"
                                         OnClientClick="$('#txtErrores').removeClass('error'); window.history.go(-1); return false;" />
                                    
                                <asp:Button ID="btnRecuperar" runat="server" CssClass="btn btn-success" Text="Recuperar contraseña" ValidationGroup="ConsUsuario"
                                         OnClick="btnRecuperar_Click" />
                            </div>
                        </div>
                    </div>                            
                </div>
            </div>
        </div>
    </div>

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
                    <span id="txtErrores2"></span>
                </div>

                <!-- Modal footer-->
                <div class="modal-footer" style="border: none;">
                    <asp:HiddenField ID="hdn_dialogo" runat="server" ClientIDMode="Static" />
                    <div class="row">
                        <div class="col-xs-12 text-center">
                            <asp:Button ID="btnCancelarDialogo" runat="server" Text="Aceptar" class="btn btn-secondary" data-dismiss="modal" OnClientClick="window.history.back(); return false;" />
                            <asp:Button ID="btnAceptarDialogo" runat="server" Text="Aceptar" CssClass="btn btn-success" CausesValidation="false"
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



