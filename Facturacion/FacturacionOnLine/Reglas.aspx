<%@ Page Title="Reglas de Facturación" Language="C#" MasterPageFile="~/Master/VIVA.Master" AutoEventWireup="true" CodeBehind="Reglas.aspx.cs" Inherits="FacturacionOnLine.Reglas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel panel-success">
        <div class="panel-body">
            <div class="row">
                <div class="col-xs-12">
                    <p class="titulo_recuadro">Estimado pasajero:</p>
                    <ul  class="text-justify"> 
                        <li>Se tienen  30 días naturales para generar la factura electrónica a partir de la fecha de compra,  
                                una vez pasado este tiempo no se podrá facturar.<br />
                            La fecha que debe tomar en cuenta para la generación de su solicitud es la fecha de pago.</li>
                        <li>El portal es para la facturación de compras por boletos de avión, se excluyen las reservaciones de hoteles, 
                                 autos y compras de alimentos y servicios abordo. Dado que estos servicios se ofrecen a través de agencias o terceros 
                                 la factura electrónica se debe solicitar al prestador de servicios.</li>
                        <li>Adicional al portal de facturación http://facturacion.vivaaerobus.com la factura la puede solicitar a través de los 
                                 puntos de ventas en aeropuertos o bien en el call center.</li>
                        <li>La factura es por el total de la compra del itinerario; el sistema no permite facturación parcial ya sea por pasajero o 
                                 por algunos servicios en específico como excesos de equipaje o cambios.</li>
                        <li>En caso de que requiera facturar por pasajero deberá realizar reservaciones individuales. En caso de separaciones o splits,
                                 la reservación original es la que prevalece para facturar, solo cuando se agreguen servicios adicionales a la reservación 
                                 Split, se puede facturar ese importe.</li>
                        <li>Para reservaciones con forma de pago en Oxxo, Sucursales Banamex y Banamex Bancanet, el pago se ve reflejado en un 
                                 máximo de 72 horas, por lo tanto deberá esperar ese tiempo para contar los 30 días que se tienen para facturar.</li>
                        <li>Una vez facturada la compra no se permiten refacturaciones o cancelaciones, solo cuando la factura contenga error en el 
                                 importe, se podrá refacturar. No se permiten correcciones de RFC o datos de Razón Social.</li>
                        <li>Para procesar la solicitud se requiere contar con su clave de confirmación, nombre(s) y apellido(s) tal cual aparece 
                                 en su confirmación de reservación. Para reservaciones con más de un pasajero se puede utilizar cualquier nombre y apellido del itinerario.</li>
                        <li>Deberá capturar toda su información fiscal requerida como RFC, SIN UTILIZAR ACENTOS, CARACTERES ESPECIALES COMO ¨ *, °, |, / o etc.</li>
                        <li>En caso de que no le llegue el correo electrónico con el CFDI, favor de utilizar la opción de descarga desde el portal.</li>
                        <li>En caso de que tenga dudas relacionadas a la emisión del CFDI favor de enviar sus comentarios (incluyendo los datos de su 
                                 reserva y folio de factura) a través del correo electrónico 
                                 <a href="mailto:facturacion.electronica@vivaaerobus.com">facturacion.electronica@vivaaerobus.com</a></li>
                    </ul>
                </div>

            </div>
            
        </div>
        <div class="separacion_20"></div>
    </div>
</asp:Content>
