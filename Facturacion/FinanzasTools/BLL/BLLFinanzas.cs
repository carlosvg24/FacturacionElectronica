using Facturacion.BLL;
using Facturacion.BLL.ProcesoFacturacion;
using Facturacion.ENT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanzasTools
{
    public class BLLFinanzas
    {
        public List<ENTPagosCambiaFormaPago> RecuperarPagosPorFacturar(long bookingId)
        {
            List<ENTPagosCambiaFormaPago> listResult = new List<ENTPagosCambiaFormaPago>();

            BLLPagosCab bllPagos = new BLLPagosCab();
            List<ENTPagosCab> listaPagosPorBooking = new List<ENTPagosCab>();
            listaPagosPorBooking = bllPagos.RecuperarPagosCabBookingid(bookingId);

            //Recupera Catalogo de Formas de Pago
            BLLFormapagoCat bllForma = new BLLFormapagoCat();
            List<ENTFormapagoCat> listCatFormaPago = new List<ENTFormapagoCat>();
            listCatFormaPago = bllForma.RecuperarTodo().ToList();

            //Recupera el catalogo de descripciones de las formas de pago definidas por el SAT
            BLLGendescripcionesCat bllDescSAT = new BLLGendescripcionesCat();
            List<ENTGendescripcionesCat> listaDescripciones = new List<ENTGendescripcionesCat>();
            listaDescripciones = bllDescSAT.RecuperarGendescripcionesCatCvetabla("FRMPAG");

            foreach (ENTPagosCab pago in listaPagosPorBooking)
            {
                long paymentId = 0;
                int idFormaPago = 0;

                paymentId = pago.PaymentID;
                idFormaPago = pago.IdFormaPago;
                ENTFormapagoCat entFormaPago = listCatFormaPago.Where(x => x.IdFormaPago == idFormaPago).FirstOrDefault();

                if (entFormaPago != null)
                {
                    if ((entFormaPago.EsFacturable && (pago.FolioPrefactura == pago.PaymentID))
                        ||
                        (
                        entFormaPago.EsFacturable && pago.EsFacturable == false
                        && (pago.MontoPorAplicar > 0)
                        && ((pago.EsPagoDividido == false) || (pago.EsPagoDividido == true && pago.EsPagoPadre == true))
                        ))
                    {
                        //En caso de que la forma de pago sea facturable en el catalogo entonces se muestra en la lista
                        ENTPagosCambiaFormaPago entPagoFact = new FinanzasTools.ENTPagosCambiaFormaPago();
                        entPagoFact.IdPagosCab = pago.IdPagosCab;
                        entPagoFact.PaymentID = pago.PaymentID;
                        entPagoFact.FechaPago = pago.FechaPago;
                        entPagoFact.FechaPagoUTC = pago.FechaPagoUTC;
                        entPagoFact.IdFormaPago = pago.IdFormaPago;
                        entPagoFact.PaymentMethodCode = pago.PaymentMethodCode;
                        entPagoFact.CurrencyCode = pago.CurrencyCode;
                        entPagoFact.FolioFactura = pago.FolioFactura;
                        entPagoFact.FechaFactura = pago.FechaFactura;
                        entPagoFact.FechaFacturaUTC = pago.FechaFacturaUTC;
                        entPagoFact.VersionFacturacion = pago.VersionFacturacion;
                        entPagoFact.EsFacturable = pago.EsFacturable;
                        entPagoFact.EsFacturado = pago.EsFacturado;
                        entPagoFact.FolioPrefactura = pago.FolioPrefactura;




                        //Se busca la descripcion de la forma de pago que se enviara al SAT
                        entPagoFact.CveFormaPagoSAT = entFormaPago.CveFormaPagoSAT;
                        entPagoFact.DescripcionFormaPagoNavitaire = entFormaPago.Descripcion;

                        ENTGendescripcionesCat entDesSAT = listaDescripciones.Where(x => x.CveValor == entPagoFact.CveFormaPagoSAT).FirstOrDefault();

                        if (entDesSAT != null)
                        {
                            entPagoFact.DescripcionFormaPagoSAT = entDesSAT.Descripcion;
                        }
                        //Se agrega el pago a la lista final
                        listResult.Add(entPagoFact);
                    }
                }


            }




            return listResult;
        }


        public void ReprocesarReserva(string pnr)
        {
            //Se reprocesara la reservacion
            BLLReservaCab bllRes = new BLLReservaCab();
            List<ENTReservaCab> listaReserva = new List<ENTReservaCab>();

            listaReserva = bllRes.RecuperarReservaCabRecordlocator(pnr);
            if (listaReserva.Count == 1)
            {
                //Si solo se recupero un registro de la reservacion entonces se actualiza la fecha de ultima modificacion
                bllRes.ModifiedDate = bllRes.ModifiedDate.AddMinutes(-1);
                bllRes.Actualizar();

            }
            //Se invoca la Distribucion de pagos
            BLLDistribucionPagos bllDistribucion = new BLLDistribucionPagos();
            //bllDistribucion.DistribuirPagosEnReservacion(pnr);


        }
    }
}
