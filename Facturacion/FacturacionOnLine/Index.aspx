<%@ Page Title="Facturación Electrónica" Language="C#" MasterPageFile="~/Master/VIVA.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="FacturacionOnLine.Index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <noscript>
        <iframe src="//www.googletagmanager.com/ns.html?id=GTM-P457KB" height="0" width="0" style="display: none; visibility: hidden"></iframe>
    </noscript>
    <script>
        (function (w, d, s, l, i) {
            w[l] = w[l] || []; w[l].push({
                'gtm.start': new Date().getTime(), event: 'gtm.js'
            }); var f = d.getElementsByTagName(s)[0],
                    j = d.createElement(s), dl = l != 'dataLayer' ? '&l=' + l : ''; j.async = true;
            j.src = '//www.googletagmanager.com/gtm.js?id=' + i + dl; f.parentNode.insertBefore(j, f);
        })
            (window, document, 'script', 'dataLayer', 'GTM-P457KB');
    </script>
    <div class="contenido">
        <table width="960" border="0" cellspacing="0" cellpadding="0" style="margin-bottom: 20px">
            <tr>
                <td width="320">
                    <img src="../Contents/Images/avance_1_1.png" width="320" height="60" /></td>
                <td width="320">
                    <img src="../Contents/Images/avance_2_1.png" width="320" height="60" /></td>
                <td width="320">
                    <img src="../Contents/Images/avance_3_1.png" width="317" height="60" /></td>
            </tr>
        </table>

        <form action="" method="get">
            <div class="contenedor_f6f6f6">
                <table width="924" border="0" cellspacing="0" cellpadding="0" style="margin-bottom: 10px;">
                    <tr>
                        <td width="453" valign="top">
                            <span class="texto_campos">
                                <%: Html.LabelFor(m => m.RFC) %>
                            </span>
                            <br />
                            <%: Html.TextBoxFor(m => m.RFC, new { @onkeyup = "InputToUpper(this)",@class="input_text_453"})%>
                            <%: Html.ValidationMessageFor(m => m.RFC)%>
                        </td>
                        <td width="18" valign="top">&nbsp;</td>
                        <td width="453" valign="top">
                            <span class="texto_campos">
                                <%: Html.LabelFor(m => m.Clave) %>
                            </span>
                            <a href="#" class="tooltip_2">
                                <img src="../Contents/Images/icono_info_1.png" width="18" height="16" border="0" />
                                <span>
                                    <img class="callout_2" src="../Contents/Images/callout_black.gif" />
                                    La clave de confirmación es la que recibió por correo o en su itinerario, 
                                                esta compuesta por 6 digitos alfanuméricos, vea los siguientes ejemplos:
                                            <img src="../Contents/Images/ejemplos_codigo.png" width="856" height="211" />
                                </span>
                            </a>
                            <br />
                            <%: Html.TextBoxFor(m => m.Clave, new { @onkeyup = "InputToUpper(this)",@class="input_text_453"}) %>
                            <%: Html.ValidationMessageFor(m => m.Clave) %>
                        </td>
                    </tr>
                </table>
                <span class="nota_campos">
                    <span class=" texto_rojo">Nota:</span>
                    Capturar RFC que desea facturar sin guiones, comas o espacios.
                </span>
                <hr class="hr_division" />
                <table width="924" border="0" cellspacing="0" cellpadding="0" style="margin-bottom: 10px;">
                    <tr>
                        <td width="453" valign="top">
                            <span class="texto_campos">
                                <%: Html.LabelFor(m => m.Nombre) %>
                            </span>
                            <br />
                            <%: Html.TextBoxFor(m => m.Nombre, new { @onkeyup = "InputToUpper(this)",@class="input_text_453"}) %>
                            <%: Html.ValidationMessageFor(m => m.Nombre) %>
                        </td>
                        <td width="18" valign="top">&nbsp;</td>
                        <td width="453" valign="top">
                            <span class="texto_campos">
                                <%: Html.LabelFor(m => m.Ape) %>
                            </span>
                            <br />
                            <%: Html.TextBoxFor(m => m.Ape, new { @onkeyup = "InputToUpper(this)",@class="input_text_453"}) %>
                            <%: Html.ValidationMessageFor(m => m.Ape) %>
                        </td>
                    </tr>
                </table>
                <span class="nota_campos">
                    <span class=" texto_rojo">Nota:</span>
                    Capture el nombre(s) y apellido(s) tal y como aparece en su correo de confirmación. 
                            Importante no use caracteres especiales como ¨ o *
                </span>
            </div>

            <table width="960" border="0" cellspacing="0" cellpadding="0" style="margin-bottom: 30px;">
                <tr>
                    <td width="723" valign="top">
                        <div>
                            <table width="670" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td width="40"></td>
                                    <td width="630">
                                        <%: Html.ValidationSummary(true)%>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                    <td width="237">
                        <input id="btnFacturar" value="" type="submit" onclick="btnBuscar_onclick(this, event)" style="width: 200px; height: 95px; background: url(../Contents/Images/boton_buscar.png)" />
                    </td>
                </tr>
            </table>
        </form>
    </div>
    <div id="cargando" class="cargando">
        <table id="TablaLoading">
            <tr id="FilaLoading">
                <td id="Celda1"></td>
                <td id="Celda2">
                    <table id="TblGif">
                        <tr id="Row1">
                            <td></td>
                        </tr>
                        <tr id="Row2">
                            <td id="CellGif">
                                <img id="MainContent_Image1" alt="CARGANDO" src="../Imagenes/loader.gif" />
                            </td>
                        </tr>
                        <tr id="Row3">
                            <td id="CellTxt">L O A D I N G</td>
                        </tr>
                        <tr id="Row4">
                            <td></td>
                        </tr>
                    </table>
                </td>
                <td id="Celda3" style="width: 33%;"></td>
            </tr>
        </table>
    </div>

    <script type="text/javascript">
        $(function () {
            $('[data-toggle="tooltip"]').tooltip()
            $('[rel=tooltip]').tooltip();
        })

        function InputToUpper(obj) {
            if (obj.value != "") {
                obj.value = obj.value.toUpperCase();
            }
        }

        window.mobilecheck = function () {
            var check = false;
            (function (a) { if (/(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino/i.test(a) || /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(a.substr(0, 4))) check = true; })(navigator.userAgent || navigator.vendor || window.opera);
            return check;
        };

        window.mobileAndTabletcheck = function () {
            var check = false;
            (function (a) { if (/(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino|android|ipad|playbook|silk/i.test(a) || /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(a.substr(0, 4))) check = true; })(navigator.userAgent || navigator.vendor || window.opera);
            return check;
        };

        function btnBuscar_onclick(ctrl, e) {
            var alturaMonitor = $(window).height();
            var alturaMain = $("#DivMain").height();

            if (window.mobilecheck() || window.mobileAndTabletcheck()) {
                var anchoMain = $("#DivMain").width();
                var padding = $("#DivMain").css("padding");
                $("#cargando").css("width", anchoMain.toString() + "px");
                $("#cargando").css("left", padding);
            }

            $("html, body").animate({ scrollTop: alturaMain }, 400);
            $("#cargando").css("top", (alturaMain - alturaMonitor + 30) + "px");
            $("#cargando").css("height", alturaMonitor.toString() + "px");
            $("#cargando").css("display", "block");
        }

    </script>
</asp:Content>
