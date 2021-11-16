<%@ Page Title="" Language="C#" MasterPageFile="~/Master/MasterViva.Master" AutoEventWireup="true" CodeBehind="frmFacturaBase.aspx.cs" Inherits="Facturacion33.PortalAdmin.Modulos.Facturacion.frmFacturaBase" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="row">
        <div class="header-panel-viva" style="width: 100%; padding-left: 0px; padding-right: 0px;">
            <div style="width: 100%; font-size: x-large; padding-top: 7px; padding-bottom: 5px; padding-left: 15px; text-align: center;">
                Facturacion con Pago en Parcialidades o Diferido (PPD)
            </div>
        </div>
    </div>




    <div class="row" style="padding: 5px 5px 0px 5px">
        <div class="col-xs-12 col-md-3">
            <div id="pnlFiltros" class="panel panel-success">
                <div class="panel-heading">Filtros de Búsqueda</div>
                <div class="panel-body" id="pnlFiltrosBody" style="padding: 10px 15px 0px 15px">
                    <div class="form-group">
                        <div class="row" style="margin: 5px;">
                            <div class="col-md-4">
                                <label class="control-label ">Reservacion</label>
                            </div>
                            <div class="col-md-8">
                                <asp:TextBox ID="txtPNR" runat="server" CssClass="form-control" PlaceHolder="PNR" MaxLength="6"></asp:TextBox>
                            </div>
                        </div>

                    </div>
                    <div class="form-group">
                        <div class="row" style="margin: 5px;">
                            <div class="col-md-4">
                                <label class="control-label ">Organization Code</label>
                            </div>
                            <div class="col-md-8">
                                <asp:DropDownList ID="ddlOrganization" runat="server" CssClass="form-control" PlaceHolder="Organizacion" OnSelectedIndexChanged="ddlOrganization_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>



                        </div>

                    </div>

                    <div class="form-group" id="divFechaProcesoIni">
                        <div class="row" style="margin: 5px;">
                            <div class="col-md-4">
                                <label class="control-label ">Fecha PNR</label>
                            </div>
                            <div class="col-md-8">
                                <asp:TextBox ID="txtFechaProcIni" runat="server" ReadOnly="true" CssClass="form-control" PlaceHolder="Fecha Inicial"></asp:TextBox>
                            </div>


                        </div>
                    </div>
                    <div class="form-group" id="divFechaProcesoFin">
                        <div class="row" style="margin: 5px;">
                            <div class="col-md-4">
                                <label class="control-label ">Fecha PNR</label>
                            </div>
                            <div class="col-md-8">
                                <asp:TextBox ID="txtFechaProcFin" runat="server" ReadOnly="true" CssClass="form-control" PlaceHolder="Fecha Final"></asp:TextBox>
                            </div>


                        </div>
                    </div>
                    <div class="form-group">
                        <div class="row" style="margin: 5px;">
                            <div class="col-md-4">
                                <label class="control-label ">Estatus Pago</label>
                            </div>
                            <div class="col-md-8">
                                <asp:DropDownList ID="ddlEstatusPago" runat="server" CssClass="form-control" PlaceHolder="Organizacion" OnSelectedIndexChanged="ddlOrganization_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>



                        </div>

                    </div>
                    <div class="form-group">
                        <div class="row" style="margin: 5px;">
                            <div class="col-md-4">
                                <label class="control-label ">Estatus Facturación</label>
                            </div>
                            <div class="col-md-8">
                                <asp:DropDownList ID="ddlEstatusFactura" runat="server" CssClass="form-control" PlaceHolder="Organizacion" OnSelectedIndexChanged="ddlOrganization_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>



                        </div>

                    </div>
                    <br />
                    <br />
                </div>
                <div class="panel-footer" style="padding: 10px 0px 1px 15px;">
                    <div class="form-group">
                        <div class="col-xs-6">
                            <asp:Button ID="btnBuscar" class="btn btn-success" runat="server" Width="90px" Text="Buscar" TabIndex="1" OnClick="btnBuscar_Click" />
                        </div>
                        <div class="col-xs-6">
                            <asp:Button ID="btnLimpiar" class="btn btn-success" runat="server" Width="90px" Text="Limpiar" TabIndex="2" OnClick="btnLimpiar_Click" />
                        </div>
                    </div>

                </div>
            </div>
        </div>
        <div class="col-xs-12 col-md-9">

            <div id="pnlDatosFact" class="panel panel-success">

                <div class="panel-heading">Datos Facturación</div>
                <div class="panel-body">
                    <div class="col-md-12">
                        <div class="col-md-2">
                            <div class="form-group">
                                <label>Entidad Facturación:</label>
                            </div>
                            <div class="form-group ">
                                <asp:RadioButton ID="rdbNacional" runat="server" Text="Nacional" Checked="True" GroupName="EntidadFac" AutoPostBack="True" OnCheckedChanged="rdbNacional_CheckedChanged" />
                            </div>
                            <div class="form-group ">
                                <asp:RadioButton ID="rdbExtranjero" runat="server" Text="Extranjero" GroupName="EntidadFac" AutoPostBack="True" OnCheckedChanged="rdbExtranjero_CheckedChanged" />
                            </div>
                        </div>
                        <div class="col-md-5">
                            <div class="row" style="margin: 5px;">

                                <div class="col-md-4">
                                    <div class="col-md-8">
                                        <label class="control-label">RFC</label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:RequiredFieldValidator ID="rfvRFC" runat="server" ControlToValidate="txtRFC"
                                            ErrorMessage="El RFC es un campo requerido" CssClass="help-block"
                                            ClientIDMode="Static" ValidationGroup="Facturar"
                                            Text="*" ToolTip="Agregue el RFC" Display="Dynamic" data-toggle="tooltip" />
                                        <asp:RegularExpressionValidator ID="revRFC" runat="server" ControlToValidate="txtRFC" ClientIDMode="Static" ValidationGroup="Facturar" Text="*"
                                            Display="Dynamic" data-toggle="tooltip" CssClass="help-block" ErrorMessage="El RFC tiene formato no valido" ToolTip="El RFC tiene formato no valido"
                                            ValidationExpression="^([A-ZÑ&]{3,4}) ?(?:- ?)?(\d{2}(?:0[1-9]|1[0-2])(?:0[1-9]|[12]\d|3[01])) ?(?:- ?)?([A-Z\d]{2})([A\d])$" />
                                    </div>
                                </div>

                                <div class="col-md-8">
                                    <asp:TextBox ID="txtRFC" runat="server" CssClass="form-control" PlaceHolder="RFC"></asp:TextBox>
                                </div>
                            </div>


                            <div class="row" style="margin: 5px">
                                <asp:Panel ID="pnlTaxId" runat="server" Visible="False">
                                    <div class="col-md-4">
                                        <div class="col-md-8">
                                            <label class="control-label">TaxId</label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:RequiredFieldValidator ID="rfvTaxID" runat="server" ControlToValidate="txtTaxID"
                                                ErrorMessage="El TAX ID es un campo requerido" CssClass="help-block"
                                                ClientIDMode="Static" ValidationGroup="Facturar"
                                                Text="*" ToolTip="Agregue el TAX ID" Display="Dynamic" data-toggle="tooltip" />
                                            <asp:RegularExpressionValidator ID="revTaxID" runat="server" ControlToValidate="txtTaxID" ClientIDMode="Static" ValidationGroup="Facturar" Text="*"
                                                Display="Dynamic" data-toggle="tooltip" CssClass="help-block" ErrorMessage="El TAX ID tiene formato no valido" ToolTip="El TAX ID tiene formato no valido"
                                                ValidationExpression="^([A-Za-z0-9]{1,40})$" />
                                        </div>
                                    </div>
                                    <div class="col-md-8">
                                        <asp:TextBox ID="txtTaxID" runat="server" CssClass="form-control" PlaceHolder="TAX ID"></asp:TextBox>
                                    </div>
                                </asp:Panel>

                            </div>
                            <div class="row" style="margin: 5px">
                                <asp:Panel ID="pnlPaisRes" runat="server" Visible="False">
                                    <div class="col-md-4">
                                        <div class="col-md-8">
                                            <label class="control-label">País</label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:RequiredFieldValidator ID="rfvPaisResidencia" runat="server" ControlToValidate="ddlResidencia"
                                                ErrorMessage="País de Residencia es un campo requerido" CssClass="help-block"
                                                ClientIDMode="Static" ValidationGroup="Facturar"
                                                Text="*" ToolTip="Seleccione el país de residencia" Display="Dynamic" data-toggle="tooltip" />

                                        </div>
                                    </div>
                                    <div class="col-md-8">
                                        <asp:DropDownList ID="ddlResidencia" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                </asp:Panel>
                            </div>
                        </div>
                        <div class="col-md-5">
                            <div class="row" style="margin: 5px;">
                                <div class="col-md-4">
                                    <div class="col-md-8">
                                        <label class="control-label" for="correo">Email:</label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:RequiredFieldValidator ID="rfvEMail" runat="server" ControlToValidate="txtEMail"
                                            ErrorMessage="EMail es un campo requerido" CssClass="help-block"
                                            ClientIDMode="Static" ValidationGroup="Facturar"
                                            Text="*" ToolTip="Agregue el EMail" Display="Dynamic" data-toggle="tooltip" />
                                        <asp:RegularExpressionValidator ID="revEMail" runat="server" ControlToValidate="txtEMail" ClientIDMode="Static" ValidationGroup="Facturar" Text="*"
                                            Display="Dynamic" data-toggle="tooltip" CssClass="help-block" ErrorMessage="El EMail tiene formato no valido" ToolTip="El EMail tiene formato no valido"
                                            ValidationExpression="[a-z0-9]+([-+._][a-z0-9]+){0,2}@.*?(\.(a(?:[cdefgilmnoqrstuwxz]|ero|(?:rp|si)a)|b(?:[abdefghijmnorstvwyz]iz)|c(?:[acdfghiklmnoruvxyz]|at|o(?:m|op))|d[ejkmoz]|e(?:[ceghrstu]|du)|f[ijkmor]|g(?:[abdefghilmnpqrstuwy]|ov)|h[kmnrtu]|i(?:[delmnoqrst]|n(?:fo|t))|j(?:[emop]|obs)|k[eghimnprwyz]|l[abcikrstuvy]|m(?:[acdeghklmnopqrstuvwxyz]|il|obi|useum)|n(?:[acefgilopruz]|ame|et)|o(?:m|rg)|p(?:[aefghklmnrstwy]|ro)|qa|r[eosuw]|s[abcdeghijklmnortuvyz]|t(?:[cdfghjklmnoprtvwz]|(?:rav)?el)|u[agkmsyz]|v[aceginu]|w[fs]|y[etu]|z[amw])\b){1,2}" />
                                    </div>
                                </div>

                                <div class="col-md-8">
                                    <asp:TextBox ID="txtEMail" runat="server" ReadOnly="false" CssClass="form-control" PlaceHolder="Email recepción CFDI"></asp:TextBox>

                                </div>

                            </div>

                            <div class="row" style="margin: 5px;">
                                <div class="col-md-4">
                                    <div class="col-md-8">
                                        <label class="control-label" for="correo">Confirmar Email:</label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:RequiredFieldValidator ID="rfvEMailConf" runat="server" ControlToValidate="txtEmailConf"
                                            ErrorMessage="EMail (Confirmación) es un campo requerido" CssClass="help-block"
                                            ClientIDMode="Static" ValidationGroup="Facturar"
                                            Text="*" ToolTip="Agregue el EMail" Display="Dynamic" data-toggle="tooltip" />
                                        <asp:RegularExpressionValidator ID="revEMailConf" runat="server" ControlToValidate="txtEMailConf" ClientIDMode="Static" ValidationGroup="Facturar" Text="*"
                                            Display="Dynamic" data-toggle="tooltip" CssClass="help-block" ErrorMessage="El EMail(Confirmación) tiene formato no valido" ToolTip="El EMail(Confirmación) tiene formato no valido"
                                            ValidationExpression="[a-z0-9]+([-+._][a-z0-9]+){0,2}@.*?(\.(a(?:[cdefgilmnoqrstuwxz]|ero|(?:rp|si)a)|b(?:[abdefghijmnorstvwyz]iz)|c(?:[acdfghiklmnoruvxyz]|at|o(?:m|op))|d[ejkmoz]|e(?:[ceghrstu]|du)|f[ijkmor]|g(?:[abdefghilmnpqrstuwy]|ov)|h[kmnrtu]|i(?:[delmnoqrst]|n(?:fo|t))|j(?:[emop]|obs)|k[eghimnprwyz]|l[abcikrstuvy]|m(?:[acdeghklmnopqrstuvwxyz]|il|obi|useum)|n(?:[acefgilopruz]|ame|et)|o(?:m|rg)|p(?:[aefghklmnrstwy]|ro)|qa|r[eosuw]|s[abcdeghijklmnortuvyz]|t(?:[cdfghjklmnoprtvwz]|(?:rav)?el)|u[agkmsyz]|v[aceginu]|w[fs]|y[etu]|z[amw])\b){1,2}" />
                                        <asp:CompareValidator ID="cmvEMailConf" runat="server" ControlToValidate="txtEMail" ControlToCompare="txtEMailConf" Display="Dynamic" data-toggle="tooltip" CssClass="help-block"
                                            ClientIDMode="Static" ValidationGroup="Facturar" Text="*" ToolTip="El EMail ingesado no es igual" />
                                    </div>

                                </div>
                                <div class="col-md-8">
                                    <asp:TextBox ID="txtEmailConf" runat="server" ReadOnly="false" CssClass="form-control" PlaceHolder="Confirmar Email"></asp:TextBox>
                                </div>
                            </div>

                            <div class="row" style="text-align: right; margin-right: 10px;">
                                <div class="col-md-4"></div>
                                <div class="col-md-4"></div>
                                <div class="col-md-4">
                                    <asp:Button ID="btnFacturar" class="btn btn-success" runat="server" Width="90px" Text="Facturar" TabIndex="1" OnClick="btnFacturar_Click" OnClientClick="openModal();if(!FactConfirmation()) {closeModal(); return false;}else{}" ValidationGroup="Facturar" />
                                </div>

                            </div>
                        </div>
                        <%--  <div class="input-group">--%>                        <%--<asp:ValidationSummary ID="vlsErrores" ClientIDMode="Static" runat="server" CssClass="ValidationErrors" ValidationGroup="Guardar" ShowMessageBox="false" />--%>                        <%-- </div>--%>
                    </div>

                    <div class="col-md-12">
                        <asp:ValidationSummary ID="valFacturar" runat="server" CssClass="ValidationErrors" ForeColor="Red" ShowMessageBox="True" ValidationGroup="Facturar" DisplayMode="List" />
                    </div>

                    <%--                    <div class="col-md-1">
                    </div>--%>
                </div>
            </div>

            <div class="panel panel-success">
                <div class="panel-heading" style="padding-top: 2px; padding-bottom: 2px; padding-right: 2px;">
                    <div class="row">
                        <div class="col-xs-12 col-md-4">
                            <label style="margin-top: 5px; font-family: Verdana, Geneva, Tahoma, sans-serif; font-size: small;">Resultado de la búsqueda...</label>
                        </div>

                        <div class="col-xs-3 col-sm-3 col-md-3">
                            <label for="lblTotal" class="control-label" style="font-family: Verdana, Geneva, Tahoma, sans-serif; font-size: small;">Num. Filas</label>
                            <asp:Label ID="lblTotal" runat="server" Text="0" class="form-control" BackColor="White" BorderStyle="Solid" BorderWidth="1px" Width="70px" Style="text-align: center"></asp:Label>
                        </div>

                        <div class="col-xs-7 col-sm-7 col-md-4">
                            <label for="ddlPagina" class="control-label hidden-xs col-sm-4 col-md-4" style="font-family: Verdana, Geneva, Tahoma, sans-serif; font-size: small;">Página</label>
                            <asp:DropDownList ID="ddlPagina" runat="server" Width="130px" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlPagina_SelectedIndexChanged" onchange="javascript:MostrarEnProceso();">
                            </asp:DropDownList>
                        </div>
                        <div class="col-xs-2 col-sm-2 col-md-1" style="padding: 0px; margin: 0px;">
                            <asp:ImageButton ID="btnExportarAExcel" ImageUrl="~/Content/Images/exportarExcel_4.jpg" runat="server" TabIndex="3" OnClick="btnExportarAExcel_Click" Enabled="False" Style="width: 37px; padding-top: 0px; padding-bottom: 0px; border-top-width: 0px; border-bottom-width: 0px; padding-left: 0px; padding-right: 0px; border-right-width: 0px; border-left-width: 0px;" />
                        </div>
                    </div>
                </div>
                <div class="panel-body" style="height: 270px; padding: 0px;">
                    <%-- <asp:Panel ID="pnlEnc" runat="server" Style="overflow: auto; overflow-x: hidden; overflow-y: hidden;" Width="100%" Height="27px" BorderStyle="None" Wrap="true">
                        <asp:GridView ID="gvEnc" runat="server" AutoGenerateColumns="False" class="form-control"
                            Width="1750px" Font-Names="Arial" Font-Size="9pt" CssClass="table table-hover table-striped table-condensed table-bordered" ShowHeaderWhenEmpty="True" BorderStyle="None" CaptionAlign="Top" HorizontalAlign="Center">
                            <AlternatingRowStyle BackColor="#E2EFDA" />
                            <Columns>
                                <asp:BoundField DataField="QueueCode" HeaderText="Queue">
                                    <HeaderStyle BorderStyle="None" HorizontalAlign="Center" VerticalAlign="Middle" CssClass="thResultado" />
                                    <ItemStyle Width="80px" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="RecordLocator" HeaderText="PNR">
                                    <HeaderStyle BorderStyle="None" HorizontalAlign="Center" VerticalAlign="Middle" CssClass="thResultado" />
                                    <ItemStyle Width="80px" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="NombreContacto" HeaderText="Nombre Contacto">
                                    <HeaderStyle BorderStyle="None" HorizontalAlign="Center" VerticalAlign="Middle" CssClass="thResultado" />
                                    <ItemStyle HorizontalAlign="Left" Width="250px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="EmailContacto" HeaderText="Email Contacto">
                                    <HeaderStyle BorderStyle="None" HorizontalAlign="Center" VerticalAlign="Middle" CssClass="thResultado" />
                                    <ItemStyle Width="250px" HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="TelefonoContacto" HeaderText="Telefono Contacto">
                                    <HeaderStyle BorderStyle="None" HorizontalAlign="Center" VerticalAlign="Middle" CssClass="thResultado" />
                                    <ItemStyle Width="150px" HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="NombreEstatus" HeaderText="Estatus">
                                    <HeaderStyle BorderStyle="None" HorizontalAlign="Center" VerticalAlign="Middle" CssClass="thResultado" />
                                    <ItemStyle Width="90px" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="NumeroPasajeros" HeaderText="Pasajeros">
                                    <HeaderStyle BorderStyle="None" HorizontalAlign="Center" VerticalAlign="Middle" CssClass="thResultado" />
                                    <ItemStyle Width="80px" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="FechaHoraProcesamiento" HeaderText="Fecha Proceso">
                                    <HeaderStyle BorderStyle="None" HorizontalAlign="Center" VerticalAlign="Middle" CssClass="thResultado" />
                                    <ItemStyle Width="140px" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="FechaHoraConfirmacion" HeaderText="Fecha Confirmacion">
                                    <HeaderStyle BorderStyle="None" HorizontalAlign="Center" VerticalAlign="Middle" CssClass="thResultado" />
                                    <ItemStyle Width="140px" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CveDireccionVuelo" HeaderText="Sentido">
                                    <HeaderStyle BorderStyle="None" HorizontalAlign="Center" VerticalAlign="Middle" CssClass="thResultado" />
                                    <ItemStyle HorizontalAlign="Center" Width="70px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ItinerarioAnterior" HeaderText="Itinerario Anterior">
                                    <HeaderStyle BorderStyle="None" HorizontalAlign="Center" VerticalAlign="Middle" CssClass="thResultado" />
                                    <ItemStyle HorizontalAlign="Left" Width="270px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ItinerarioNuevo" HeaderText="Itinerario Nuevo">
                                    <HeaderStyle BorderStyle="None" HorizontalAlign="Center" VerticalAlign="Middle" CssClass="thResultado" />
                                    <ItemStyle HorizontalAlign="Left" Width="270px" />
                                </asp:BoundField>
                            </Columns>
                            <FooterStyle BackColor="#CCCCCC" />
                            <HeaderStyle BackColor="#3FAB37" ForeColor="White" />
                            <EmptyDataRowStyle BackColor="#3FAB37" ForeColor="White" />
                        </asp:GridView>
                    </asp:Panel>--%>
                    <asp:Panel ID="pnlGrid" runat="server" ScrollBars="Auto" Width="100%" BorderStyle="None" Wrap="true" Height="100%">
                        <asp:GridView ID="gvListaReservaciones" runat="server" AutoGenerateColumns="False" class="form-control" EmptyDataText="Sin información disponible..." Font-Names="Arial" Font-Size="9pt" CssClass="table table-hover table-striped table-condensed table-bordered" OnDataBound="gvListaReservaciones_DataBound">
                            <AlternatingRowStyle BackColor="#E2EFDA" />
                            <Columns>
                                <asp:TemplateField>
                                    <EditItemTemplate>
                                        <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("EstaMarcadoParaFacturacion") %>' />
                                    </EditItemTemplate>
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkSeleccion" runat="server" AutoPostBack="True" OnCheckedChanged="chkSeleccion_CheckedChanged" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSelFila" runat="server" Checked='<%# Bind("EstaMarcadoParaFacturacion") %>' Enabled="False" />
                                    </ItemTemplate>
                                    <ItemStyle Width="25px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="CFDI">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lblReenviar" runat="server" CausesValidation="False" CommandName="Reimpresion" ToolTip="Descargar la factura" Height="30px" Width="30px">
                                            <img src="../../Content/Images/envio.png" style="width: 30px;height: 30px;"/>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="34px" Height="30px" />
                                </asp:TemplateField>

                                <asp:BoundField DataField="PNR" HeaderText="PNR">
                                    <ItemStyle HorizontalAlign="Left" Width="70px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="NumPasajeros" HeaderText="Num Pax">
                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Moneda" HeaderText="Moneda">
                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="MontoTotal" DataFormatString="{0:c}" HeaderText="Monto Total">
                                    <ItemStyle HorizontalAlign="Right" Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="MontoPagado" DataFormatString="{0:c}" HeaderText="Monto Pagado">
                                    <ItemStyle HorizontalAlign="Right" Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Organizacion" HeaderText="Organización">
                                    <ItemStyle HorizontalAlign="Left" Width="120px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="BookingStatus" HeaderText="Estatus">
                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="EstatusPago" HeaderText="Estatus Pago">
                                    <ItemStyle HorizontalAlign="Left" Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DescEstatusFacturado" HeaderText="Estatus Factura">
                                    <ItemStyle Width="90px" />
                                </asp:BoundField>

                                <asp:BoundField DataField="FechaCreacion" DataFormatString="{0:d}" HeaderText="Fecha Reserva">
                                    <ItemStyle HorizontalAlign="Center" Width="80px" />
                                </asp:BoundField>

                            </Columns>
                            <FooterStyle BackColor="#CCCCCC" />
                            <HeaderStyle BackColor="#3FAB37" ForeColor="White" />
                            <EmptyDataRowStyle BackColor="#3FAB37" ForeColor="White" />
                        </asp:GridView>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>


    <div id="cargando" class="cargando">
        <table style="width: 100%; height: 100%;">
            <tr>
                <td></td>
                <td style="width: 50px; vertical-align: middle;">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Content/Images/loader3.gif" Style="display: table-cell; vertical-align: middle; vertical-align: middle; justify-content: center; width: 90px" />
                    Procesando...
                </td>
                <td></td>
            </tr>
        </table>
    </div>


    <asp:HiddenField ID="hfErrorBusqueda" runat="server" Value="" />
    <script src="../../scripts/Portal/FacturaBase.js"></script>


</asp:Content>
