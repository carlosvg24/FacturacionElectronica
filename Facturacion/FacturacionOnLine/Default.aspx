<%@ Page Title="Facturación Electrónica" Language="C#" MasterPageFile="~/Master/VIVA.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="~/Default.aspx.cs" Inherits="FacturacionOnLine.Default" ValidateRequest="false" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <script type="text/javascript">
        // https://forums.asp.net/t/1991430.aspx?how+to+validate+at+least+one+check+box+is+checked+in+grid+view+check+box+column+
        function CheckBoxValidation() {
            var valid = false;
            //Varibale to hold the checked checkbox count
            var chkselectcount = 0;
            //Get the gridview object
            var gridview = document.getElementById('<%= grvFacturas.ClientID %>');

            //Loop thorugh items
            for (var i = 0; i < gridview.getElementsByTagName("input").length; i++) {
                //Get the object of input type
                var node = gridview.getElementsByTagName("input")[i];
                //check if object is of type checkbox and checked or not
                if (node != null && node.type == "checkbox" && node.checked) {
                    valid = true;
                    //Increase the count of check box
                    chkselectcount = chkselectcount + 1;
                }
            }

            // Checking the count is zero , this means no checkbox is selected
            if (chkselectcount == 0) {
                Mostrar_Dialogo("informacion", "Por favor elija al menos un pago a facturar");
                $('#<%=hfPuedeFacturar.ClientID%>').val("");
                return false;
            }
            else {
                $('#<%=hfPuedeFacturar.ClientID%>').val("1");
                return false;
            }

        }
    </script>
    <script type="text/javascript">
        function showpreview(input) {
            var x = document.getElementById("txtPNR").value.length;
            if (x == 7) {
                document.getElementById('<%=lbl_vldPNR.ClientID %>').style.display = "";
            }
            else if (x == 0)
            {
                document.getElementById('<%=lbl_vldPNR.ClientID %>').style.display = "none";
            }
        }
    </script>
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
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always" EnableViewState="true" ViewStateMode="Enabled">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="grvFacturas" />
            <asp:AsyncPostBackTrigger ControlID="btnGenerarFactura" />
        </Triggers>
        <ContentTemplate>
            <!-- R E S E R V A S   P E N D    F A C T -->
            <div class="panel panel-success" id="pnlReservaPendFac" runat="server" visible="false" style="box-shadow: -1px 0px 8px 0px black;">
                <div class="panel-heading" style="font-size: 12pt;">Reservaciones pendientes de Facturar</div>
                <div class="panel-footer">Por favor selecciona las reservaciones a facturar</div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-md-4 col-xs-12 center-block">
                            <asp:GridView ID="grvReservaPendFac" runat="server" CssClass="table table-striped table-hover"
                                AutoGenerateColumns="False" AllowPaging="True" PageSize="5" AllowSorting="True"
                                OnRowDataBound="grvReservaPendFac_RowDataBound" OnPageIndexChanging="grvReservaPendFac_PageIndexChanging"
                                OnRowCommand="grvReservaPendFac_RowCommand" EmptyDataText="No se encontraron reservaciones que puedan ser facturados."
                                ShowHeaderWhenEmpty="True" BorderStyle="Solid" BorderColor="Transparent">
                                <Columns>
                                    <%-- 1: P O R   F A C T U R A R --%>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkPorFacturar" runat="server" CssClass="CheckGrid" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="10px" VerticalAlign="Top" />
                                    </asp:TemplateField>
                                    <%-- 2: V E R   P A G O S --%>
                                    <asp:TemplateField HeaderText='Pagos' ItemStyle-Width="10px" ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID='lblVerPagos' runat='server' CausesValidation='false' CommandArgument='<%#DataBinder.Eval(Container.DataItem,"RecordLocator") %>'
                                                CommandName="PNR" ClientIDMode="Static" ToolTip="Ver detalle de los pagos" data-toggle="tooltip">
                                                        <i class="glyphicon glyphicon-list-alt" style="font-size:large !important;"></i>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%-- 3: P N R --%>
                                    <asp:TemplateField HeaderText="Clave de Confirmación" ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblPNR" Text="" ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center" ForeColor="#55565a" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <AlternatingRowStyle CssClass="active" />
                                <HeaderStyle CssClass="success" HorizontalAlign="Center" />
                                <PagerStyle HorizontalAlign='Center' />
                            </asp:GridView>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4 col-xs-12 center-block">
                            <asp:Button runat="server" ID="btnFacturarPNRs" Text="Facturar Seleccionados"
                                OnClick="btnFacturarPNRs_Click" CssClass="btn btn-success pull-right" />
                        </div>
                    </div>
                </div>
            </div>

            <!-- D A T O S   V U E L O -->
            <%--<div class="form-horizontal">--%>
            <div class="panel panel-success" id="pnlDatosReservacion" runat="server" visible="false" style="box-shadow: -1px 0px 8px 0px black;">

                <div class="panel-heading" style="font-size: 12pt;">Datos de la Reservación a Facturar</div>
                <div class="panel-footer">
                    Por favor ingrese los siguientes datos para identificar su reservación.
                </div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-md-5 col-md-offset-1">
                            <!-- C L A V E   C O N F I R M A C I Ó N -->
                            <div class="form-group row">
                                <label for="txtPNR" class="col-sm-5 col-form-label">Clave Confirmación:</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtPNR" runat="server" CssClass="form-control" ClientIDMode="Static" MinLength="6" MaxLength="8" onchange="showpreview(this);" />
                                    <asp:Label ID="lbl_vldPNR" CssClass="help-block" runat="server" Text="Clave de reservacion invalida" style="display:none;" ForeColor="#FF0000" ></asp:Label>
                                    <asp:RequiredFieldValidator ID="rfvPNR" runat="server" ControlToValidate="txtPNR"
                                        ErrorMessage="La clave de confirmación es un campo requerido" CssClass="help-block"
                                        ClientIDMode="Static" ValidationGroup="Buscar"
                                        Text="*" ToolTip="Agregue la clave de confirmación" Display="Dynamic" data-toggle="tooltip" ForeColor="#FF3300" />
                                    <asp:RegularExpressionValidator ID="revPNR" runat="server" ControlToValidate="txtPNR" ClientIDMode="Static" ValidationGroup="Buscar" Text="*"
                                        Display="Dynamic" data-toggle="tooltip" CssClass="help-block" ErrorMessage="La clave de confirmación tiene formato no valido" ToolTip="La clave de confirmación tiene formato no valido"
                                        ValidationExpression="^([A-Za-z0-9]{6,8})$" ForeColor="#FF3300" />
                                    <%--ValidationExpression="^([A-Za-z0-9]{6})$" ForeColor="#FF3300" />--%>
                                </div>
                            </div>
                            <!--  N O M B R E   D E   P A S A J E R O -->
                            <div class="form-group row">
                                <label for="txtNombre" class="col-sm-5 col-form-label">Nombre(s) Pasajero:</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" ClientIDMode="Static" ToolTip="Dato requerido para identificar la reservación, no aparecerá en la factura..." />
                                    <asp:RequiredFieldValidator ID="rfvNombre" runat="server" ControlToValidate="txtNombre"
                                        ErrorMessage="Nombre es un campo requerido" CssClass="help-block"
                                        ClientIDMode="Static" ValidationGroup="Buscar"
                                        Text="*" ToolTip="Agregue el nombre" Display="Dynamic" data-toggle="tooltip" ForeColor="#FF3300" />
                                </div>
                            </div>
                            <!-- A P E L L I D O S   P A S A J E R O -->
                            <div class="form-group row">
                                <label for="txtApellidos" class="col-sm-5 col-form-label">Apellido(s) Pasajero:</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtApellidos" runat="server" CssClass="form-control" ClientIDMode="Static" ToolTip="Dato requerido para identificar la reservación, no aparecerá en la factura..." />
                                    <asp:RequiredFieldValidator ID="rfvApellidos" runat="server" ControlToValidate="txtApellidos"
                                        ErrorMessage="Apellido(s) es un campo requerido" CssClass="help-block"
                                        ClientIDMode="Static" ValidationGroup="Buscar"
                                        Text="*" ToolTip="Agregue el(los) apellido(s)" Display="Dynamic" data-toggle="tooltip" ForeColor="#FF3300" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <!-- C A P T C H A -->
                            <asp:Panel ID="pnlCaptcha" runat="server" HorizontalAlign="Center">
                                <div class="form-group row">
                                    <div class="col-lg-10 col-lg-pull-1 col-xs-12">
                                        <asp:TextBox ID="txtCaptcha" runat="server" ClientIDMode="Static" Style="display: none" />
                                        <asp:RequiredFieldValidator ID="rfvCaptcha" ErrorMessage="La validación del Captcha es requerida." ControlToValidate="txtCaptcha"
                                            runat="server" ForeColor="Red" Display="Dynamic" ValidationGroup="Buscar" ClientIDMode="Static" CssClass="help-block">*</asp:RequiredFieldValidator>
                                        <div id="ReCaptchContainer" class="pull-right"></div>
                                    </div>
                                </div>
                            </asp:Panel>
                            <!-- B U S C A R   R E S E R V A -->
                            <div class="form-group row">
                                <div class="col-lg-10 col-lg-pull-1 col-xs-12">
                                    <asp:Button ID="btnBuscarReserva" class="btn btn-success pull-right" runat="server" Width="90px" Text="Buscar" TabIndex="1" ValidationGroup="Buscar" OnClick="btnBuscarReserva_Click" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-11 col-md-offset-1 col-md-pull-1" style="margin-top: 15px;">
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="ValidationErrors" ForeColor="Red" ShowMessageBox="False" ValidationGroup="Buscar" DisplayMode="BulletList" BackColor="#FFE3E3" Font-Bold="False" HeaderText="Por favor revise la siguiente información para poder continuar..." />
                        </div>
                    </div>
                </div>
            </div>
            <%--</div>--%>

            <!-- P A G O S   F A C T U R A B L E S -->
            <asp:Panel ID="pnlPagos" runat="server" Visible="False">
                <div class="panel panel-success" style="box-shadow: -1px 0px 8px 0px black;">

                    <div class="panel-heading" runat="server" id="pnlPagosHeader" style="font-size: 12pt;">Pagos Facturables</div>
                    <div class="panel-footer">
                        Por favor marque los pagos que desea facturar.
                    </div>
                    <div class="panel-body">
                        <%-- G R I D --%>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Panel ID="pnlContGrid" runat="server" Visible="True" ScrollBars="Auto" Height="100%" Width="100%">
                                    <asp:GridView ID="grvFacturas" runat="server" CssClass="table table-striped table-hover"
                                        AutoGenerateColumns='False' AllowPaging='True' PageSize='25' AllowSorting='True'
                                        OnRowDataBound="grvFacturas_RowDataBound" OnPageIndexChanging="grvFacturas_PageIndexChanging"
                                        OnRowCommand="grvFacturas_RowCommand" EmptyDataText="No se encontraros pagos que puedan ser facturados."
                                        ShowHeaderWhenEmpty="True" BorderStyle="Solid" BorderWidth="1px" BorderColor="Transparent">
                                        <Columns>

                                            <%-- 1 : E S   F A C T U R A D O --%>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkEsFacturado" runat="server" Checked="false" CssClass="CheckGrid" />
                                                    <asp:Image ID="imgError" runat="server" ImageUrl="~/Contents/Images/warning.png" Width="16px" Height="16px" CssClass="iconoGrid" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="40px" VerticalAlign="Top" />
                                            </asp:TemplateField>
                                            <%-- 2 : R E E N V I A R --%>
                                            <asp:TemplateField Visible="true" ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center" HeaderText="Reenvió">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lblReenviar" runat="server" CausesValidation="false"
                                                        CommandArgument='<%#DataBinder.Eval(Container.DataItem,"FolioPrefactura") %>' CommandName="Reenviar"
                                                        ClientIDMode="Static" ToolTip="Reenviar CFDI por correo" data-toggle="tooltip">
                                                        <i class="glyphicon glyphicon-share-alt" style="font-size:large !important;"></i>
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="30px" />
                                            </asp:TemplateField>
                                            <%-- 3 : D E S C A R G A R --%>
                                            <asp:TemplateField Visible="true" ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center" HeaderText="Descarga">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lblDescargar" runat="server" CausesValidation="false"
                                                        CommandArgument='<%#DataBinder.Eval(Container.DataItem,"FolioPrefactura") %>' CommandName="Reimpresion"
                                                        ToolTip="Descargar la factura" data-toggle="tooltip">                                                            
                                                            <img src="Contents/Images/envio.png" />
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="40px" />
                                            </asp:TemplateField>
                                            <%-- 4 : V E R   D E T A L L E --%>
                                            <asp:TemplateField HeaderText='Detalle' SortExpression='' ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID='lblConsultar' runat='server' CausesValidation='false' CommandArgument='<%#DataBinder.Eval(Container.DataItem,"FolioPrefactura") %>'
                                                        CommandName="Select" OnClick="lblConsultar_Click"
                                                        ClientIDMode="Static" ToolTip="Ver detalle de pagos" data-toggle="tooltip">
                                                        <i class="glyphicon glyphicon-list-alt" style="font-size:large !important;"></i>
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="40px" />
                                            </asp:TemplateField>
                                            <%-- 5 : F E C H A   P A G O --%>
                                            <asp:BoundField DataField="FechaPago" HeaderText="Fecha Pago" DataFormatString="{0:d}">
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="80px" />
                                            </asp:BoundField>
                                            <%-- 6 : F O R M A   P A G O --%>
                                            <asp:TemplateField HeaderText="Forma Pago">
                                                <ItemTemplate>
                                                    <%--# Bind("PaymentMethodCode") --%>
                                                    <asp:Label ID="lblFormaPago" runat="server" Width="100%" Text='' ForeColor="#55565a" Font-Bold="false"></asp:Label>

                                                    <asp:DropDownList ID="ddlFormaPago" runat="server" CssClass="form-control" Width="180px"
                                                        Visible="False" OnSelectedIndexChanged="ddlFormaPago_SelectedIndexChanged" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="140px" />
                                            </asp:TemplateField>
                                            <%-- 7 : F O L I O   F A C T U R A --%>
                                            <asp:BoundField DataField="FolioPrefactura" HeaderText="Folio Factura">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="70px" />
                                            </asp:BoundField>
                                            <%-- 8 : F E C H A   F A C T U R A --%>
                                            <asp:BoundField DataField="FechaFactura" HeaderText="Fecha Facturación" DataFormatString="{0:d}">
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="80px" />
                                            </asp:BoundField>
                                            <%-- 9 : C U R R E N C Y   C O D E --%>
                                            <asp:BoundField DataField="CurrencyCode" HeaderText="Moneda">
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="40px" />
                                            </asp:BoundField>
                                            <%-- 10 : P A Y M E N T   A M O U N T --%>
                                            <asp:BoundField DataField="PaymentAmount" HeaderText="Monto Total" DataFormatString="{0:C}">
                                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="120px" />
                                            </asp:BoundField>
                                            <%-- 11 : L U G A R   E X P E D I C I O N --%>
                                            <asp:BoundField DataField="LugarExpedicion" HeaderText="Lugar Exp.">
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="80px" />
                                            </asp:BoundField>
                                            <%-- 12 : I D   P A G O S   C A B --%>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblIdPagosCab" Visible="false"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%-- 13 : PNR --%>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblFactPNR" Visible="false"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
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
                                </asp:Panel>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-12 text-right">
                                <asp:Button ID="btnCancelarFacturar" class="btn btn-secondary" runat="server" Width="110px" Text="Cancelar" TabIndex="1"
                                    OnClick="btnCancelarFacturar_Click" />
                                <asp:Button ID="btnFacturar" class="btn btn-success" runat="server" Width="110px" Text="Facturar" TabIndex="1"
                                    ValidationGroup="Facturar" OnClick="btnFacturar_Click" OnClientClick="CheckBoxValidation();" />
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <asp:HiddenField runat="server" ID="hfPuedeFacturar" />

            <!-- D E T A L L E   P A G O S -->
            <asp:Panel ID="pnlDetallePagos" runat="server" Visible="False">
                <div class="panel panel-success" style="box-shadow: -1px 0px 8px 0px black;">

                    <div class="panel-heading" style="font-size: 12pt; padding-right: 5px !important;">
                        <asp:Label ID="lblDetNum" runat="server" AssociatedControlID="lblDetNum" ClientIDMode="Static" CssClass="control-label" Text="Detalle de la Factura" />

                        <div class="btn-group pull-right" style="top: -6px !important;">
                            <asp:LinkButton ID="btnCerrarDetallesPago" runat='server' CssClass="btn btn-danger btn-sm"
                                ToolTip="Cerrar" data-toggle="tooltip" OnClick="btnCerrarDetallesPago_Click">
                                <i class="glyphicon glyphicon-remove" style="font-size:large !important;"></i>
                            </asp:LinkButton>
                        </div>
                    </div>
                    <div class="panel-footer">
                        A continuación se listan los pagos considerados en la factura.
                    </div>
                    <div class="panel-body">
                        <div class="col-md-12">
                            <asp:Panel ID="pnlGridDetalleFac" runat="server" Visible="True" ScrollBars="Auto" Height="100%" Width="100%">

                                <asp:GridView ID="grvDetalle" runat="server" CssClass="table table-striped table-hover" BorderColor="Transparent"
                                    AutoGenerateColumns='False' AllowPaging='True' PageSize='50' AllowSorting='True' OnRowDataBound="grvDetalle_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="FechaPago" HeaderText="Fecha Pago">
                                            <ItemStyle Width="150px" HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PaymentId" HeaderText="Folio Pago">
                                            <ItemStyle Width="60px" HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CurrencyCode" HeaderText="Moneda">
                                            <ItemStyle Width="160px" HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PaymentAmount" HeaderText="Monto">
                                            <ItemStyle Width="150px" HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PaymentMethodCode" HeaderText="Forma Pago" Visible="true">
                                            <ItemStyle Width="200px" HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                    </Columns>
                                    <AlternatingRowStyle CssClass="active" />
                                    <HeaderStyle CssClass="success" />
                                    <PagerStyle HorizontalAlign='Center' />
                                </asp:GridView>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </asp:Panel>

            <!-- D A T O S   F A C T U R A -->
            <asp:Panel ID="pnlDatosFact" runat="server" Visible="False">
                <div class="panel panel-success" style="box-shadow: -1px 0px 8px 0px black;">

                    <div class="panel-heading" style="font-size: 12pt; padding-right: 5px !important;">
                        Datos de Facturación
                        <div class="btn-group pull-right" style="top: -6px !important;">
                            <asp:LinkButton ID='btnCerrarDatosFactura' runat='server' CssClass="btn btn-danger btn-sm"
                                ToolTip="Cerrar" data-toggle="tooltip" OnClick="btnCerrarDatosFactura_Click">
                                <i class="glyphicon glyphicon-remove" style="font-size:large !important;"></i>
                            </asp:LinkButton>
                        </div>
                    </div>
                    <div class="panel-footer">
                        Por favor ingrese la siguiente información fiscal para la generación de facturas.
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-xs-12">
                                <asp:RadioButton ID="rdbPersMoral" runat="server" CssClass="radio-inline" Text="Moral" GroupName="TipoPersona" AutoPostBack="True" Visible="false" />
                                <asp:RadioButton ID="rdbPersFisica" runat="server" CssClass="radio-inline" Text="Fisica" Checked="True" GroupName="TipoPersona" AutoPostBack="True" Visible="false" />

                                <asp:RadioButton ID="rdbNacional" runat="server" CssClass="radio-inline" Text="Nacional" Checked="True" GroupName="EntidadFac" AutoPostBack="True" Visible="false" />
                                <asp:RadioButton ID="rdbExtranjero" runat="server" CssClass="radio-inline" Text="Extranjero" GroupName="EntidadFac" AutoPostBack="True" Visible="false" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-5 col-md-offset-1">
                                <%-- E M A I L --%>
                                <div class="form-group row">
                                    <label for="txtEMail" class="col-sm-5 col-form-label">Email:</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txtEMail" runat="server" CssClass="form-control" ClientIDMode="Static" />
                                        <asp:RequiredFieldValidator ID="rfvEMail" runat="server" ControlToValidate="txtEMail"
                                            ErrorMessage="EMail es un campo requerido" CssClass="help-block"
                                            ClientIDMode="Static" ValidationGroup="Guardar"
                                            Text="*" ToolTip="Agregue el EMail" Display="Dynamic" data-toggle="tooltip" ForeColor="#FF3300" />
                                        <asp:RegularExpressionValidator ID="revEMail" runat="server" ControlToValidate="txtEMail" ClientIDMode="Static" ValidationGroup="Guardar" Text="*"
                                            Display="Dynamic" data-toggle="tooltip" CssClass="help-block" ErrorMessage="El EMail tiene formato no valido" ToolTip="El EMail tiene formato no valido"
                                            ValidationExpression="[a-z0-9]+([-+._][a-z0-9]+){0,2}@.*?(\.(a(?:[cdefgilmnoqrstuwxz]|ero|(?:rp|si)a)|b(?:[abdefghijmnorstvwyz]iz)|c(?:[acdfghiklmnoruvxyz]|at|o(?:m|op))|d[ejkmoz]|e(?:[ceghrstu]|du)|f[ijkmor]|g(?:[abdefghilmnpqrstuwy]|ov)|h[kmnrtu]|i(?:[delmnoqrst]|n(?:fo|t))|j(?:[emop]|obs)|k[eghimnprwyz]|l[abcikrstuvy]|m(?:[acdeghklmnopqrstuvwxyz]|il|obi|useum)|n(?:[acefgilopruz]|ame|et)|o(?:m|rg)|p(?:[aefghklmnrstwy]|ro)|qa|r[eosuw]|s[abcdeghijklmnortuvyz]|t(?:[cdfghjklmnoprtvwz]|(?:rav)?el)|u[agkmsyz]|v[aceginu]|w[fs]|y[etu]|z[amw])\b){1,2}" ForeColor="#FF3300" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-5" runat="server" id="EmailConfirmacion">
                                <%-- C O N F I R M A R   E M A I L --%>
                                <div class="form-group row">
                                    <label for="txtEmailConf" class="col-sm-5 col-form-label">Confirmar Email:</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txtEmailConf" runat="server" CssClass="form-control" ClientIDMode="Static" />
                                        <asp:RequiredFieldValidator ID="rfvEMailConf" runat="server" ControlToValidate="txtEmailConf"
                                            ErrorMessage="EMail (Confirmación) es un campo requerido" CssClass="help-block"
                                            ClientIDMode="Static" ValidationGroup="Guardar"
                                            Text="*" ToolTip="Agregue el EMail" Display="Dynamic" data-toggle="tooltip" ForeColor="#FF3300" />
                                        <asp:RegularExpressionValidator ID="revEMailConf" runat="server" ControlToValidate="txtEMailConf" ClientIDMode="Static" ValidationGroup="Guardar" Text="*"
                                            Display="Dynamic" data-toggle="tooltip" CssClass="help-block" ErrorMessage="El EMail(Confirmación) tiene formato no valido" ToolTip="El EMail(Confirmación) tiene formato no valido"
                                            ValidationExpression="[a-z0-9]+([-+._][a-z0-9]+){0,2}@.*?(\.(a(?:[cdefgilmnoqrstuwxz]|ero|(?:rp|si)a)|b(?:[abdefghijmnorstvwyz]iz)|c(?:[acdfghiklmnoruvxyz]|at|o(?:m|op))|d[ejkmoz]|e(?:[ceghrstu]|du)|f[ijkmor]|g(?:[abdefghilmnpqrstuwy]|ov)|h[kmnrtu]|i(?:[delmnoqrst]|n(?:fo|t))|j(?:[emop]|obs)|k[eghimnprwyz]|l[abcikrstuvy]|m(?:[acdeghklmnopqrstuvwxyz]|il|obi|useum)|n(?:[acefgilopruz]|ame|et)|o(?:m|rg)|p(?:[aefghklmnrstwy]|ro)|qa|r[eosuw]|s[abcdeghijklmnortuvyz]|t(?:[cdfghjklmnoprtvwz]|(?:rav)?el)|u[agkmsyz]|v[aceginu]|w[fs]|y[etu]|z[amw])\b){1,2}" ForeColor="#FF3300" />
                                        <asp:CompareValidator ID="cmvEMailConf" runat="server" ControlToValidate="txtEMail" ControlToCompare="txtEMailConf" Display="Dynamic" data-toggle="tooltip" CssClass="help-block"
                                            ClientIDMode="Static" ValidationGroup="Guardar" Text="*" ToolTip="El EMail ingesado no es igual" ForeColor="#FF3300" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-5 col-md-offset-1">
                                <%--  R F C --%>
                                <div class="form-group row">
                                    <label for="txtRFC" class="col-sm-5 col-form-label">RFC:</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txtRFC" runat="server" CssClass="form-control" MaxLength="13"
                                            AutoPostBack="true" OnTextChanged="txtRFC_TextChanged" />
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-5">
                                <%--  U S O   D E   C F D I --%>
                                <div class="form-group row">
                                    <label for="ddlUsoCFDI" class="col-sm-5 col-form-label">Uso de CFDI:</label>
                                    <div class="col-sm-7">
                                        <asp:DropDownList ID="ddlUsoCFDI" runat="server" ClientIDMode="Static" CssClass="form-control" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <%-- F A C T U R A   E X T R A N J E R O --%>
                        <div class="row" id="dvFacExtranjero" runat="server" visible="false">

                            <div class="col-md-5 col-md-offset-1">
                                <%--  T A X   I D  --%>
                                <div class="form-group row">
                                    <label for="" class="col-sm-5 col-form-label">TAX ID:</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txtTaxID" runat="server" CssClass="form-control" ClientIDMode="Static" MaxLength="40" />
                                        <asp:RequiredFieldValidator ID="rfvTaxID" runat="server" ControlToValidate="txtTaxID"
                                            ErrorMessage="El TAX ID es un campo requerido" CssClass="help-block"
                                            ClientIDMode="Static" ValidationGroup="Guardar"
                                            Text="*" ToolTip="Agregue el TAX ID" Display="Dynamic" data-toggle="tooltip" Enabled="false" ForeColor="#FF3300" />
                                        <asp:RegularExpressionValidator ID="revTaxID" runat="server" ControlToValidate="txtTaxID" ClientIDMode="Static" ValidationGroup="Guardar" Text="*"
                                            Display="Dynamic" data-toggle="tooltip" CssClass="help-block" ErrorMessage="El TAX ID tiene formato no valido" ToolTip="El TAX ID tiene formato no valido"
                                            ValidationExpression="^([A-Za-z0-9]{1,40})$" ForeColor="#FF3300" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-5">
                                <%--  P A Í S   D E   R E S I D E N C I A --%>
                                <div class="form-group row">
                                    <label for="ddlPaisReferencia" class="col-sm-5 col-form-label">País de residencia:</label>
                                    <div class="col-sm-7">
                                        <asp:DropDownList ID="ddlPaisReferencia" runat="server" CssClass="form-control" ClientIDMode="Static" />
                                    </div>
                                </div>
                            </div>

                        </div>


                        <%-- B O T Ó N   G E N E R A R   F A C T U R A --%>
                        <div class="row">
                            <div class="col-md-5 col-md-offset-1">
                                <asp:Button runat="server" ID="btnSoyExtranjero" CssClass="btn btn-secondary pull-right"
                                    Text="Soy Extranjero" OnClick="btnSoyExtranjero_Click" />
                            </div>
                            <div class="col-md-5">
                                <asp:Button runat="server" ID="btnGenerarFactura" CssClass="btn btn-success pull-right"
                                    Text="Generar Factura" OnClick="btnGenerarFactura_Click" />
                            </div>
                        </div>
                    </div>

                    <div class="panel-footer panel-warning">
                        Importante: Con base en las nuevas disposiciones de la miscelanea fiscal el Nombre o Razón Social y la Dirección Fiscal ya no son datos obligatorios, por tal motivo no se incluyen en este apartado.
                    </div>
                </div>
            </asp:Panel>

            <!-- E R R O R E S -->
            <div class="form-group">
                <div class="col-lg-12">
                    <asp:ValidationSummary ID="vlsErrores" ClientIDMode="Static" runat="server" CssClass="ValidationErrors" ValidationGroup="Guardar" ShowMessageBox="false" ForeColor="#FF3300" />
                </div>
            </div>

            <!-- F O O T E R -->
            <div class="row">
                <div class="form-group" style="background-color: #E0F8E0; margin: 10px; padding: 10px; font-size: smaller;">
                    <p>
                        Este portal no considera el <b>Complemento INE</b> para Partidos Políticos, Coaliciones y Asociaciones Civiles de conformidad con el artículo 46 del Reglamento de Fiscalización del Instituto Nacional Electoral, en caso de requerirlo agradeceremos se comunique a Call Center, donde con gusto lo atenderemos.
                    </p>
                </div>
            </div>

            <asp:HiddenField ID="hdnPostback" runat="server" />
            <asp:HiddenField ID="hdnOcultarCaptcha" runat="server" />

            <%--</div>   --%>
        </ContentTemplate>
    </asp:UpdatePanel>


    <!-- M O D A L -->
    <div id="dv_dialogo" class="modal fade" data-backdrop="static" role="dialog" style="z-index: 999999999 !important;">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">

                <!-- Modal header-->
                <div class="modal-header" style="border: none;">
                    <div id="img_dialogo"></div>
                </div>

                <!-- Modal body-->
                <div class="modal-body text-center">
                    <span id="txt_dialogo"></span>
                </div>

                <!-- Modal footer-->
                <div class="modal-footer" style="border: none;">
                    <asp:UpdatePanel ID="Up_Dialogo" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:HiddenField ID="hdn_dialogo" runat="server" ClientIDMode="Static" />
                            <div class="text-center">
                                <input id="btnCancelarDialogo" type="button" value="Cancelar" class="btn btn-secondary" data-dismiss="modal" onclick="Ocultar_Capa('dv_dialogo');" />
                                <asp:Button ID="btnAceptarDialogo" runat="server" Text="Aceptar" CssClass="btn btn-success" CausesValidation="false" OnClientClick="$('#dv_dialogo').modal('hide');" />
                                <asp:HiddenField ID="hdn_confirmacion" runat="server" />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>
            </div>
        </div>
    </div>
    <!-- E N D   M O D A L -->



    <%-- PROGRESS --%>
    <asp:UpdateProgress ID="UpdateProgress" runat="server">
        <ProgressTemplate>
            <div class="dv_overlay">
            </div>
            <div class="dv_progress">
                <%--style="z-index:99 !important; position: relative;"--%>
                <img alt="" src="<%= Page.ResolveUrl("~/Contents/Images/loader.gif")%>" class="img_progress" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

</asp:Content>
