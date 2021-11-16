using Facturacion.ENT.Comun;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ZXing;
using ZXing.Common;


namespace Comun.Utils
{
    public class ImpresoraTermicaEpson
    {



        #region Propiedades

        public  const string ESC = "\u001B";
        public const string GS = "\u001D";
        public const string InitializaImpresora = ESC + "@";
        public const string ActivaNegritas = ESC + "E" + "\u0001";
        public const string DesactivaNegritas = ESC + "E" + "\0";
        public const string ActivaDobleAnchoyAlto = GS + "!" + "\u0011";  // 2x sized text (double-high + double-wide)
        public const string DesactivaDobleAnchoyAlto = GS + "!" + "\0";

        public string NombreImpresora { get; }
        public int LongitudMaximaLetraNormal { get; }
        public int LongitudMaximaLetraGrande { get; }

        public string CambiarAFuentePequeña { get; }

        public string AsignarAnchoFuente10 { get; }

        public string AsignarAnchoFuente20 { get; }

        public string AsignarAnchoFuente30 { get; }

        public string AsignarAnchoFuente40 { get; }

        public string AsignarAnchoFuente50 { get; }

        public string AsignarAnchoFuente60 { get; }

        public string AsignarAnchoFuente70 { get; }
        public string AsignarAlturaFuente01 { get; }
        public string AsignarAlturaFuente02 { get; }
        public string AsignarAlturaFuente03 { get; }
        public string AsignarAlturaFuente04 { get; }
        public string AsignarAlturaFuente05 { get; }
        public string AsignarAlturaFuente06 { get; }
        public string AsignarAlturaFuente07 { get; }
        public string AsignarAlturaFuente08 { get; }
        public string AsignarAlturaFuente09 { get; }

        public string AsignaTipoFuenteGrande { get; }

        public string AsignaTipoFuenteNormal { get; }

        public string CorteHoja { get; }
        public string SaltoLineaRetorno { get; }

        public string SeparadorLineaDoble { get; }
        public string SeparadorLineaSencilla { get; }


        public string CmdActivarNegrita { get; }
        public string CmdDesactivarNegrita { get; }
        public string CmdInterlineado { get; }

        #endregion
        #region Contructor
        public ImpresoraTermicaEpson(string nombreImpresora)
        {

            NombreImpresora = nombreImpresora;
            LongitudMaximaLetraNormal = 55;
            LongitudMaximaLetraGrande = 42;
            CambiarAFuentePequeña = "\x1b\x4D\x31";
            CmdInterlineado = "\x1b\x33\x27";


            AsignarAnchoFuente10 = "\x1D\x21\x10";
            AsignarAnchoFuente20 = "\x1D\x21\x20";
            AsignarAnchoFuente30 = "\x1D\x21\x30";
            AsignarAnchoFuente40 = "\x1D\x21\x40";
            AsignarAnchoFuente50 = "\x1D\x21\x50";
            AsignarAnchoFuente60 = "\x1D\x21\x60";
            AsignarAnchoFuente70 = "\x1D\x21\x70";


            AsignarAlturaFuente01 = "\x1D\x21\x01";
            AsignarAlturaFuente02 = "\x1D\x21\x02";
            AsignarAlturaFuente03 = "\x1D\x21\x03";
            AsignarAlturaFuente04 = "\x1D\x21\x04";
            AsignarAlturaFuente05 = "\x1D\x21\x05";
            AsignarAlturaFuente06 = "\x1D\x21\x06";
            AsignarAlturaFuente07 = "\x1D\x21\x07";
            AsignarAlturaFuente08 = "\x1D\x21\x08";
            AsignarAlturaFuente09 = "\x1D\x21\x09";

            AsignaTipoFuenteGrande = "\x1b\x4D\x30";
            AsignaTipoFuenteNormal = "\x1b\x4D\x31";




            CorteHoja = "\x1B" + "m";

            SaltoLineaRetorno = "\n\r";
            SeparadorLineaDoble = "";
            SeparadorLineaDoble = SeparadorLineaDoble.PadRight(LongitudMaximaLetraNormal, '=') + SaltoLineaRetorno;
            SeparadorLineaSencilla = "";
            SeparadorLineaSencilla = SeparadorLineaSencilla.PadRight(LongitudMaximaLetraNormal, '-') + SaltoLineaRetorno;
            CmdActivarNegrita = ActivaNegritas;
            CmdDesactivarNegrita = DesactivaNegritas;
        }

        #endregion

        #region Comandos

        public void EnviarImpresion(string datos)
        {
            RawPrinterHelper.DOCINFOA doc = new Utils.RawPrinterHelper.DOCINFOA();
            //Envia texto con Formato a la impresora
            RawPrinterHelper.SendStringToPrinter(NombreImpresora, datos.ToString());
        }


        public void EjecutarCambioAFuentePequeña()
        {
            RawPrinterHelper.SendStringToPrinter(NombreImpresora, CambiarAFuentePequeña);
        }

        public void EjecutarCorteHoja()
        {
            RawPrinterHelper.SendStringToPrinter(NombreImpresora, CorteHoja);
        }

        /// <summary>
        /// Comando para incrementar tamaño de la fuente, incrementa de 10 en 10 hasta 70
        /// </summary>
        public void IncrementarAnchoFuente(int anchoFuente)
        {
            string fuenteDif = "";
            switch (anchoFuente)
            {
                case 10:
                    fuenteDif = AsignarAnchoFuente10;
                    break;
                case 20:
                    fuenteDif = AsignarAnchoFuente20;
                    break;
                case 30:
                    fuenteDif = AsignarAnchoFuente30;
                    break;
                case 40:
                    fuenteDif = AsignarAnchoFuente40;
                    break;
                case 50:
                    fuenteDif = AsignarAnchoFuente50;
                    break;
                case 60:
                    fuenteDif = AsignarAnchoFuente60;
                    break;
                case 70:
                    fuenteDif = AsignarAnchoFuente70;
                    break;

                default:
                    break;
            }

            RawPrinterHelper.SendStringToPrinter(NombreImpresora, fuenteDif);
        }



        /// <summary>
        /// Comando para incrementar el alto de la fuente, incrementa de 1 en 1 hasta 8 01,02,03,04,05,06,07,08,09
        /// </summary>
        /// <param name="altoFuente"></param>
        public void IncrementarAltoFuente(int altoFuente)
        {
            string fuenteDif = "";
            switch (altoFuente)
            {
                case 1:
                    fuenteDif = AsignarAlturaFuente01;
                    break;
                case 2:
                    fuenteDif = AsignarAlturaFuente02;
                    break;
                case 3:
                    fuenteDif = AsignarAlturaFuente03;
                    break;
                case 4:
                    fuenteDif = AsignarAlturaFuente04;
                    break;
                case 5:
                    fuenteDif = AsignarAlturaFuente05;
                    break;
                case 6:
                    fuenteDif = AsignarAlturaFuente06;
                    break;
                case 7:
                    fuenteDif = AsignarAlturaFuente07;
                    break;
                case 8:
                    fuenteDif = AsignarAlturaFuente08;
                    break;
                case 9:
                    fuenteDif = AsignarAlturaFuente09;
                    break;

                default:
                    fuenteDif = AsignarAlturaFuente01;
                    break;
            }

            RawPrinterHelper.SendStringToPrinter(NombreImpresora, fuenteDif);
        }

        public void EjecutaAjustarFuenteGrande()
        {
            RawPrinterHelper.SendStringToPrinter(NombreImpresora, AsignaTipoFuenteGrande);
        }

        public string ComandoSaltoLinea(int numLineas)
        {
            string avance = "\x1B" + "d";
            switch (numLineas)
            {
                case 1:
                    avance += "\x01";
                    break;
                case 2:
                    avance += "\x02";
                    break;
                case 3:
                    avance += "\x03";
                    break;
                case 4:
                    avance += "\x04";
                    break;
                case 5:
                    avance += "\x05";
                    break;
                case 6:
                    avance += "\x06";
                    break;
                case 7:
                    avance += "\x07";
                    break;

                case 8:
                    avance += "\x08";
                    break;

                case 9:
                    avance += "\x09";
                    break;

                default:
                    avance += "\x02";
                    break;
            }

            return avance;
        }

        #endregion


        #region Metodos Tratamiento Textos

        public string Centrar(string cadena, int longitudVal = 0, bool esTipoLetraNormal = true)
        {
            int longitud = 0;
            if (longitudVal > 0)
            {
                longitud = longitudVal;
            }
            else
            {
                if (esTipoLetraNormal)
                {
                    longitud = LongitudMaximaLetraNormal;
                }
                else
                {
                    longitud = LongitudMaximaLetraGrande;
                }
            }

            if (cadena.Length < longitud)
            {
                int diferencia = longitud - cadena.Length;
                int left = (diferencia) / 2;

                cadena = cadena.PadLeft(cadena.Length + left).PadRight(longitud);
            }

            return cadena;
        }


        public string Izquierda(string cadena, int longitudValor = 0, bool esTipoLetraNormal = true)
        {
            string result = "";
            int longitud = 0;
            if (longitudValor > 0)
            {
                longitud = longitudValor;
            }
            else
            {
                if (esTipoLetraNormal)
                {
                    longitud = LongitudMaximaLetraNormal;
                }
                else
                {
                    longitud = LongitudMaximaLetraGrande;
                }
            }


            if (cadena.Length < longitud)
            {
                cadena = cadena.PadRight(longitud);
            }

            result = cadena;
            return result;
        }

        public string Derecha(string cadena, int longitudVal = 0, bool esTipoLetraNormal = true)
        {
            int longitud = 0;
            if (longitudVal > 0)
            {
                longitud = longitudVal;
            }
            else
            {
                if (esTipoLetraNormal)
                {
                    longitud = LongitudMaximaLetraNormal;
                }
                else
                {
                    longitud = LongitudMaximaLetraGrande;
                }

            }

            if (cadena.Length < longitud)
            {
                cadena = cadena.PadLeft(longitud);
            }

            return cadena;
        }

        public string QuitarAcentos(string texto)
        {
            string result = "";

            result = texto;
            result = result.Replace("Á", "A");
            result = result.Replace("É", "E");
            result = result.Replace("Í", "I");
            result = result.Replace("Ó", "O");
            result = result.Replace("Ú", "U");

            result = result.Replace("á", "a");
            result = result.Replace("é", "e");
            result = result.Replace("í", "i");
            result = result.Replace("ó", "o");
            result = result.Replace("ú", "u");


            return result;

        }

        public byte[] GenerateBarCodeZXing(string data)
        {
            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new EncodingOptions { Width = 171, Height = 171 } //optional
            };
            var imgBitmap = writer.Write(data);
            using (var stream = new MemoryStream())
            {
                imgBitmap.Save(stream, ImageFormat.Bmp);
                return stream.ToArray();
            }
        }

        


        public void SendBytesToLocalPrinter(byte[] data, string printerName)
        {
            var size = Marshal.SizeOf(data[0]) * data.Length;
            var pBytes = Marshal.AllocHGlobal(size);
            try
            {
                RawPrinterHelper.SendBytesToPrinter(printerName, pBytes, size);
            }
            finally
            {
                Marshal.FreeCoTaskMem(pBytes);
            }
        }

        public byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }
        #endregion

        public void ImprimeQR(string valorQR)
        {

            try
            {
                /*se generan los textos para la impresión del ticket pass bording*/
                StringBuilder qr = new StringBuilder();

                var codeBar = GenerateBarCodeZXing(valorQR);

                Image redimImg = null;
                if (codeBar != null)
                {
                    redimImg = byteArrayToImage(codeBar);//resizeImage(byteArrayToImage(Pase.CodeBar), new Size(300, 100));
                }


                // se inicia el proceso de impresión
                PrintDocument pd = new PrintDocument();
                pd.DefaultPageSettings.PrinterSettings.PrinterName = NombreImpresora;

                PaperSize ps = new PaperSize("", 475, 550);
                pd.PrintController = new StandardPrintController();
                pd.DefaultPageSettings.Margins.Left = 0;
                pd.DefaultPageSettings.Margins.Right = 0;
                pd.DefaultPageSettings.Margins.Top = 0;
                pd.DefaultPageSettings.Margins.Bottom = 0;
                pd.DefaultPageSettings.PaperSize = ps;

                //se definen los saltos y tamaño de fuente
             
                int lineaWrite = 30;
                int espacio = 12;

             
                //se imprime el ticket
                pd.PrintPage += (sender, args) =>
                {
                    Rectangle m = args.MarginBounds;
                    if (redimImg != null)
                    {
                        m.Height = (int)redimImg.Height;
                        m.Width = (int)redimImg.Width;
                    }

                    if (redimImg != null)
                    {
                        m.Y = lineaWrite;
                        m.X = (int)((pd.DefaultPageSettings.PrintableArea.Width / 2) - (m.Width / 2));
                        args.Graphics.DrawImage(redimImg, m);

                        lineaWrite += m.Height + (espacio * 1);
                    }

                    
                };
                
                pd.Print();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        private Image byteArrayToImage(byte[] byteArrayIn)
        {
            try
            {
                MemoryStream ms = new MemoryStream(byteArrayIn, 0, byteArrayIn.Length);
                ms.Write(byteArrayIn, 0, byteArrayIn.Length);
                return Image.FromStream(ms, true);//Exception occurs here
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
