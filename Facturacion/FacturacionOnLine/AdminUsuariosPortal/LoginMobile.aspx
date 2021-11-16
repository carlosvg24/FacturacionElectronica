<%@ Page Title="Iniciar Sesión" Language="C#" MasterPageFile="~/Master/VIVA.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="LoginMobile.aspx.cs" Inherits="FacturacionOnLine.AdminUsuariosPortal.LoginMobile" ValidateRequest="false" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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

        $(document).ready(function () {

            var isMobile = false; //initiate as false
            // device detection
            if (/(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|ipad|iris|kindle|Android|Silk|lge |maemo|midp|mmp|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino/i.test(navigator.userAgent)
                || /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(navigator.userAgent.substr(0, 4))) {
                isMobile = true;
            }

            if (!isMobile) {
                window.parent.location.href = '/Default.aspx';
            }
        });
    </script>


    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always" EnableViewState="true" ViewStateMode="Enabled">
        <%--<Triggers>
            <asp:AsyncPostBackTrigger ControlID="grvFacturas" />
            <asp:AsyncPostBackTrigger ControlID="btnGenerarFactura" />
       </Triggers>--%>
        <ContentTemplate>
             <div class="panel panel-success" runat="server" style="box-shadow: -1px 0px 8px 0px black;">
                <div class="panel-heading" style="font-size: 12pt;">Reservaciones pendientes de Facturar</div>
                <div class="panel-footer">Por favor selecciona las reservaciones a facturar</div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-xs-12">
                            <span runat="server"  class="error" id="txt_error"></span>
                        </div>
                    </div>
            
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
                            <asp:Button runat="server" ID="btnLogin" Text="Iniciar Sesión"
                                 CssClass="btn btn-success pull-right" OnClick="btnLogin_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    

    <%-- PROGRESS --%>
    <asp:UpdateProgress ID="UpdateProgress" runat="server">
        <ProgressTemplate>
            <div class="dv_overlay">
            </div>
            <div class="dv_progress">  
                <img alt="" src="<%= Page.ResolveUrl("~/Contents/Images/loader.gif")%>" class="img_progress" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    
</asp:Content>