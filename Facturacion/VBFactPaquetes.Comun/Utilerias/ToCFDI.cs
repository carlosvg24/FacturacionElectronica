using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VBFactPaquetes.Comun.Utilerias
{
    public class ToCFDI
    {
        /*DEFINIMOS LOS VARIABLES QUE MANEJARAN (UNIDADES, DECENAS, CENTENAS)*/
        private String[] UNIDADES = { "", "un ", "dos ", "tres ", "cuatro ", "cinco ", "seis ", "siete ", "ocho ", "nueve " };
        private String[] DECENAS = {"diez ", "once ", "doce ", "trece ", "catorce ", "quince ", "dieciseis ",
        "diecisiete ", "dieciocho ", "diecinueve", "veinti","veinte ", "treinta ", "cuarenta ",
        "cincuenta ", "sesenta ", "setenta ", "ochenta ", "noventa "};
        private String[] CENTENAS = {"", "ciento ", "doscientos ", "trescientos ", "cuatrocientos ", "quinientos ", "seiscientos ",
        "setecientos ", "ochocientos ", "novecientos "};

        private Regex r;



        /* FUNCIONES PARA CONVERTIR LOS NÚMEROS A LITERALES */

        private String getUnidades(String numero)
        {   // 1 - 9            
            //SI TUVIERA ALGUN 0 ANTES SE LO QUITA -> 09 = 9 o 009=9
            String num = numero.Substring(numero.Length - 1);
            return UNIDADES[int.Parse(num)];
        }

        private String getDecenas(String num)
        {// 99                        
            int n = int.Parse(num);

            if (n < 10)
            {//PARA CASOS COMO -> 01 - 09
                return getUnidades(num);
            }
            else if (n > 19 && n < 30)
            {
                //PARA 20...99
                String u = getUnidades(num);

                if (u.Equals(""))
                {
                    //PARA 20
                    return DECENAS[int.Parse(num.Substring(0, 1)) + 9];
                }
                else
                {
                    return DECENAS[int.Parse(num.Substring(0, 1)) + 8] + u;
                }
            }
            else if (n >= 30)
            {
                //PARA 30...99
                String u = getUnidades(num);

                if (u.Equals(""))
                {
                    //PARA 30,40,50,60,70,80,90
                    return DECENAS[int.Parse(num.Substring(0, 1)) + 9];
                }
                else
                {
                    return DECENAS[int.Parse(num.Substring(0, 1)) + 9] + " y " + u;
                }
            }
            else
            {//NÚMEROS ENTRE 11 y 19
                return DECENAS[n - 10];
            }
        }

        private String getCentenas(String num)
        {// 999 o 099
            if (int.Parse(num) > 99)
            {//ES CENTENA
                if (int.Parse(num) == 100)
                {//CASO ESPECIAL
                    return " cien ";
                }
                else
                {
                    return CENTENAS[int.Parse(num.Substring(0, 1))] + getDecenas(num.Substring(1));
                }
            }
            else
            {//POR EJEM. 099 
                //SE QUITA EL 0 ANTES DE CONVERTIR A DECENAS
                return getDecenas(int.Parse(num) + "");
            }
        }

        private String getMiles(String numero)
        {// 999 999
            //OBTIENE LAS CENTENAS
            String c = numero.Substring(numero.Length - 3);
            //OBTIENE LOS MILES
            String m = numero.Substring(0, numero.Length - 3);
            String n = "";
            //SE COMPRUEBA QUE MILES TENGA VALOR ENTERO
            if (int.Parse(m) > 0)
            {
                n = getCentenas(m);
                return n + "mil " + getCentenas(c);
            }
            else
            {
                return "" + getCentenas(c);
            }

        }

        private String getMillones(String numero)
        { //000 000 000        
            //SE OBTIENE LOS MILES
            String miles = numero.Substring(numero.Length - 6);
            //SE OBTIENE LOS MILLONES
            String millon = numero.Substring(0, numero.Length - 6);
            String n = "";
            if (millon.Length > 1)
            {
                n = getCentenas(millon) + "millones ";
            }
            else
            {
                n = getUnidades(millon) + "millon ";
            }
            return n + getMiles(miles);
        }



        public string ConvertirNumLetras(String numero, bool mayusculas, String tipoMoneda)
        {
            String literal = String.Empty;
            String parte_decimal = String.Empty;

            //SI EL NÚMERO UTILIZA (.) EN LUGAR DE (,) -> SE REEMPLAZA
            numero = numero.Replace(".", ",");

            //SI EL NÚMERO NO TIENE PARTE DECIMAL, SE LE AGREGA ,00
            if (numero.IndexOf(",") == -1)
            {
                numero = numero + ",00";
            }
            //SE VALIDA FORMATO DE ENTRADA -> 0,00 y 999 999 999,00
            r = new Regex(@"\d{1,9},\d{1,2}");
            MatchCollection mc = r.Matches(numero);

            if (mc.Count > 0)
            {
                //SE DIVIDE EL NUMERO 0000000,00 -> ENTERO Y DECIMAL
                String[] Num = numero.Split(',');

                //SE DA FORMATO AL NÚMERO DECIMAL CUANDO ES PESO
                if (tipoMoneda == "MXN")
                {
                    parte_decimal = "pesos con " + Num[1].Substring(0, 2) + "/100 MXN";
                }
                //SE DA FORMATO AL NÚMERO DECIMAL CUANDO ES DOLARES
                else if (tipoMoneda == "USD")
                {
                    parte_decimal = "dolares con " + Num[1].Substring(0, 2) + "/100 USD";
                }

                //SE CONVIERTE EL NÚMERO A LITERAL
                if (int.Parse(Num[0]) == 0)
                {//SI EL VALOR ES CERO                
                    literal = "cero ";
                }
                else if (int.Parse(Num[0]) > 999999)
                {//SI ES MILLON
                    literal = getMillones(Num[0]);
                }
                else if (int.Parse(Num[0]) > 999)
                {//SI ES MILES
                    literal = getMiles(Num[0]);
                }
                else if (int.Parse(Num[0]) > 99)
                {//SI ES CENTENA
                    literal = getCentenas(Num[0]);
                }
                else if (int.Parse(Num[0]) > 9)
                {//SI ES DECENA
                    literal = getDecenas(Num[0]);
                }
                else
                {//SINO UNIDADES -> 9
                    literal = getUnidades(Num[0]);
                }
                //DEVUELVE EL RESULTADO EN MAYUSCULAS O MINUSCULAS
                if (mayusculas)
                {
                    return (literal + parte_decimal).ToUpper();
                }
                else
                {
                    return (literal + parte_decimal);
                }
            }
            else
            {//ERROR, NO SE PUEDE CONVERTIR
                return literal = null;
            }
        }

        public string GenerarQR(Dictionary<string, string> DatosFactura)
        {
            String URL = "https://verificacfdi.facturaelectronica.sat.gob.mx/default.aspx";
            StringBuilder codigoBi = new StringBuilder();

            codigoBi.Append(URL);
            codigoBi.Append("?id=" + DatosFactura["UUID"]);
            codigoBi.Append("&re=" + DatosFactura["RFCEmisor"]);
            codigoBi.Append("&rr=" + DatosFactura["RFCCliente"]);
            codigoBi.Append("&tt=" + DatosFactura["Total"]);

            string sello = DatosFactura["SelloComprobante"];
            string ult8carSello = sello.Substring(sello.Length - 8, 8);
            codigoBi.Append("&fe=" + ult8carSello);

            return codigoBi.ToString();
        }
    }
}
