using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VBFactPaquetes.Comun.Utilerias;

namespace VBFactPaquetes.Comun.Log
{
    /// <summary>
    /// Loguea las excepciones y devuelve mensajes personalizados
    /// </summary>
    public class Excepciones : Exception
    {

        /// <summary>
        /// Identifica los Tipos de Portal que loguean errores
        /// </summary>
        public enum TipoPortal : int
        {
            Ninguno = 0,
            VivaPaquetes = 1,
            Juniper = 2,
            Facturacion = 3
        }


        #region "PROPIEDADES"
        /// <summary>
        /// Identificador del error logueado
        /// </summary>
        public static Guid IdEx { get; set; }
        public object Param { get; set; }
        private static String Codigo { get; set; }
        private static String Desc { get; set; }

        /// <summary>
        /// Clase dónde se generó el error
        /// </summary>
        private static String Clase { get; set; }

        /// <summary>
        /// Método dónde se generó el error
        /// </summary>
        private static String Metodo { get; set; }

        /// <summary>
        /// Mensaje del error generado
        /// </summary>
        private static String Mensaje { get; set; }

        /// <summary>
        /// Origen del error
        /// </summary>
        private static String Source { get; set; }

        /// <summary>
        /// Nombre completo de la Excepción
        /// </summary>
        private static String NombreCompEx { get; set; }
        private static String NombreExecpcion { get; set; }

        /// <summary>
        /// Rastro pila del error
        /// </summary>
        private static String StackTrace { get; set; }

        /// <summary>
        /// Código del error
        /// </summary>
        private static String HResult { get; set; }
        #endregion


        private static Logger dbLogger = LogManager.GetLogger("dbLog");
        private static NLog.Logger fileLogger = LogManager.GetLogger("File");
        private static NLog.Logger fileLoggerNoLayout = LogManager.GetLogger("NoLayout");

        /// <summary>
        /// Código generico del error
        /// </summary>
        public const String CODIGO_DEFAULT = "50000";

        /// <summary>
        /// Mensaje por defecto que se mostrará al usuario por del error 
        /// </summary>
        public const String MENSAJE_DEFAULT = "Ha ocurrido un error inesperado.";

        /// <summary>
        /// Descripción del mensaje que se mostrará al usuario
        /// </summary>
        public const String DESCRIPCION_DEFAULT = "Si el problema persiste, contáctese con Soporte Técnico.";
        private const String SEPARADOR = "====================================================================================";


        public override string Message
        {
            get
            {
                return Excepciones.MENSAJE_DEFAULT + " " +
                    Excepciones.DESCRIPCION_DEFAULT + " " +
                    ", Código de error: " +
                    IdEx.ToString();
            }
        }

        public Excepciones() { }


        /// <summary>
        /// Captura la excepción real para ser logueada, detalla el origen y seguimiento de pila. El mensaje de error se asigna a dtEstado.
        /// </summary>
        /// <param name="origen">clase:metodo</param>
        /// <param name="excepcion">Exception</param>
        public Excepciones(string clase, string metodo, Dictionary<string, object> parametros, Exception excepcion, TipoPortal tipoPortal)
        : base(clase, excepcion)
        {
            ConexionBD connection = new ConexionBD();
            SqlConnection con = new SqlConnection();
            con = connection.Conexion();

            ((NLog.Targets.DatabaseTarget)new List<NLog.Targets.Target>(dbLogger.Factory.Configuration.AllTargets)[0]).ConnectionString = con.ConnectionString;
            ((NLog.Targets.DatabaseTarget)new List<NLog.Targets.Target>(dbLogger.Factory.Configuration.AllTargets)[0]).CommandText = "INSERT INTO Facturacion.VBFactPaqLogs " +
                            " (GuidLogging, Aplicacion, Fecha, UserName, Thread, Source, Clase, Metodo, HResult, ExceptionFullName, Exception, StackTrace) " +
                            " VALUES(@GuidLogging, @IdTipoPortal, @Fecha, @UserName, @Thread, @Source, @Clase, @Metodo, @HResult, @ExceptionFullName, @Exception, @StackTrace); ";

            IdEx = Guid.NewGuid();
            Source = excepcion.Source;
            Clase = clase;
            Metodo = metodo;
            NombreCompEx = excepcion.GetType().FullName;
            NombreExecpcion = excepcion.GetType().Name;
            Mensaje = excepcion.Message;
            StackTrace = excepcion.StackTrace;
            HResult = excepcion.HResult.ToString();

            // Escribir Log en BD
            if (true)
            {
                dbLogger.WithProperty("GuidLogging", IdEx.ToString())
                        .WithProperty("Aplicacion", tipoPortal.ToString())
                        .WithProperty("Source", Source)
                        .WithProperty("Clase", Clase)
                        .WithProperty("Metodo", Metodo)
                        .WithProperty("HResult", HResult)
                        .WithProperty("ExceptionFullName", NombreCompEx)
                        .WithProperty("Exception", Mensaje)
                        .WithProperty("StackTrace", StackTrace).Error("");

                if (parametros != null && parametros.Count > 0)
                    EscribirParametros(parametros);

            }
            else
            {
                fileLoggerNoLayout.Info(SEPARADOR);
                EscribirExcepcionEnLog();

                if (parametros != null && parametros.Count > 0)
                    EscribirParametros(parametros);

                fileLoggerNoLayout.Info(SEPARADOR);
            }

            // Enviar correo 
            //EnvioCorreo.EmailSendTo = "daniel.hernandez@enitma.com";
            //Dictionary<string, string> datosSMTP = new Dictionary<string, string>();
            //DataTable dtResultado = new DataTable();
            //ConexionBD conn = new ConexionBD();

            //// Obtiene Valores SMTP para envío de correo
            //dtResultado = conn.ConsultarConfiguracion("YAVAS").Select("TipoParametro = 'ClienteSMTP'").CopyToDataTable();
            //datosSMTP = Convertidor.DataTableToDiccionario(dtResultado, "Nombre", "Valor");
            //datosSMTP.Add("guid", IdEx.ToString());
            //datosSMTP.Add("error", excepcion.Message);
            //datosSMTP.Add("stacktrace", excepcion.StackTrace);

            //EnvioCorreo.EnviarCorreo(datosSMTP, EnvioCorreo.TipoCorreo.NotificarError);


        }

        private string ValorParametro(KeyValuePair<string, object> obj)
        {
            String valor = String.Empty;
            switch (obj.Value.GetType().FullName)
            {
                case "System.Enum":
                    // https://stackoverflow.com/questions/17495941/store-enum-as-comma-separated-list
                    valor = "No se a implementado ENUM";
                    break;

                case "System.DBNull":
                case "System.Empty":
                    valor = obj.Value.GetType().FullName;
                    break;

                case "System.Int64":
                case "System.Object":
                case "System.String":
                case "System.DateTime":
                case "System.Boolean":
                case "System.Byte":
                case "System.Char":
                case "System.Currency":
                case "System.Decimal":
                case "System.Double":
                case "System.Guid":
                case "System.Int16":
                case "System.Number":
                case "System.TimeSpan":
                case "System.UInt16":
                case "System.UInt32":
                case "System.UInt64":
                    valor = obj.Value.ToString();
                    break;

                case "System.Array":
                    valor = string.Join(",", ((Array)obj.Value));
                    break;

                case "System.IO.File":
                case "System.IO.FileAccess":
                case "System.IO.FileInfo":
                case "System.IO.FileMode":
                case "System.IO.FileStream":
                case "System.IO.MemoryStream":
                case "System.IO.Stream":
                case "System.IO.StreamReader":
                case "System.IO.StreamWriter":
                case "System.IO.StringReader":
                case "System.IO.StringWriter":
                case "System.IO.TextReader":
                case "System.IO.TextWriter":
                    valor = "El tipo : " + obj.Value.GetType().FullName + " no está considerado.";
                    break;
                case "System.Text.StringBuilder":
                    valor = obj.Value.ToString();
                    break;

                //case "":
                //case "":
                //case "":
                //case "":
                //case "":
                //case "":
                //case "":
                //case "":
                //case "":
                //case "":
                //    break;
                case "System.Data.DataRow":                
                    var dt = (((System.Data.DataRow)(obj.Value))).Table;
                    valor = Convertidor.DataTableToJSON(dt);
                    break;
                case "System.Data.DataTable":
                    var dt2 = ((System.Data.DataTable)(obj.Value));
                    valor = Convertidor.DataTableToJSON(dt2);
                    break;
                default:
                    valor = obj.Value.GetType().FullName + " No existe en la opción múltiple";
                    if (obj.Value.GetType().FullName.Contains("List") || obj.Value.GetType().FullName.Contains("VBFactPaquetes.Model"))
                        valor = JsonConvert.SerializeObject(obj.Value);

                    break;
            }

            return valor;
        }

        private void EscribirParametros(Dictionary<string, object> parametros)
        {
            String valor = String.Empty;
            List<String> tipos = new List<string>();
            ConexionBD connection = new ConexionBD();
            int numPar = 1;

            try
            {
                //Assembly mscorlib = typeof(string).Assembly;
                //foreach (Type type in mscorlib.GetTypes())
                //{
                //    tipos.Add(type.FullName);
                //}


                foreach (KeyValuePair<string, object> obj in parametros)
                {
                    String tipoDato = String.Empty;

                    valor = ValorParametro(obj);
                    //fileLogger.Log(LogLevel.Error, obj.Value.GetType().Name + " - " + obj.Key + " => " + valor);

                    String query = "INSERT INTO [Facturacion].[VBFactPaqLogsParametros] (GuidLogging,IdParametro,TipoDato,ParametroNombre,ParametroValor) " +
                    " VALUES (@GuidLogging,@IdParametro,@TipoDato,@ParametroNombre,@ParametroValor)";

                    using (SqlConnection con = connection.Conexion())
                    {
                        using (SqlCommand command = new SqlCommand(query, con))
                        {
                            command.Parameters.AddWithValue("@GuidLogging", IdEx);
                            command.Parameters.AddWithValue("@IdParametro", numPar);
                            command.Parameters.AddWithValue("@TipoDato", obj.Value.GetType().FullName);
                            command.Parameters.AddWithValue("@ParametroNombre", obj.Key);
                            command.Parameters.AddWithValue("@ParametroValor", valor);

                            con.Open();
                            int resultado = command.ExecuteNonQuery();

                            // Verifica sí se inertó
                            if (resultado < 0)
                                Console.WriteLine("Error al insertar en la BD");
                        }
                    }

                    numPar += 1;
                }
            }
            catch (Exception ex)
            {
                //fileLoggerNoLayout.Error(ex.Message);
                //throw ex;

                dbLogger.WithProperty("GuidLogging", Guid.NewGuid().ToString())
                        .WithProperty("Aplicacion", "PortalInterno")
                        .WithProperty("Source", ex.Source)
                        .WithProperty("Clase", MethodBase.GetCurrentMethod().DeclaringType.Name)
                        .WithProperty("Metodo", MethodBase.GetCurrentMethod().Name)
                        .WithProperty("HResult", ex.HResult)
                        .WithProperty("ExceptionFullName", ex.GetType().FullName)
                        .WithProperty("Exception", ex.Message)
                        .WithProperty("StackTrace", ex.StackTrace)
                        .Error("");
            }

        }

        /// <summary>
        /// Guarda la excepción para fines de correción del issue
        /// </summary>
        /// <param name="clase"></param>
        /// <param name="metodo"></param>
        /// <param name="nombreCompEx"></param>
        /// <param name="nombreExecpcion"></param>
        /// <param name="mensaje"></param>
        /// <param name="stackTrace"></param>
        /// <param name="hResult"></param>
        private static void EscribirExcepcionEnLog()
        {

            fileLoggerNoLayout.Info(Environment.NewLine + Environment.NewLine);
            //                          "      123456789012345678901234567890123456       "
            fileLogger.Log(LogLevel.Info, "-------------------------------------------------");
            fileLogger.Log(LogLevel.Info, "      " + IdEx.ToString() + "       ");
            fileLogger.Log(LogLevel.Info, "-------------------------------------------------");

            fileLogger.Log(LogLevel.Error, "SOURCE:             " + Source);
            fileLogger.Log(LogLevel.Error, "CLASE:              " + Clase);
            fileLogger.Log(LogLevel.Error, "METODO:             " + Metodo);
            if (!String.IsNullOrEmpty(HResult))
                fileLogger.Log(LogLevel.Error, "HRESULT:            " + HResult);
            fileLogger.Log(LogLevel.Error, "EXCEPTION FULLNAME: " + NombreCompEx);
            fileLogger.Log(LogLevel.Error, "EXCEPTION NAME:     " + NombreExecpcion);
            fileLogger.Log(LogLevel.Error, "DESCRIPCIÓN:        " + Mensaje);
            fileLogger.Log(LogLevel.Error, "STACK TRACE:        " + StackTrace);



        }





        //   Configuración para loguear en archivos     //
        /*
          
         <nlog>
            <targets>
              <target name="NoLayout" type="File" fileName="${basedir}/logs/LOG_${shortdate}.log" layout="${message}" />
              <target name="File" type="File" 
                      layout="${date:format=dd-MM-yyyy HH\:mm\:ss.fff}  | ${pad:padding=5:inner=${level:uppercase=true}} | ${message} ${onexception:${newline} ${exception:format=ToString}" 
                      fileName="${basedir}/logs/LOG_${shortdate}.log" />
         
            </targets>
            <rules>
              <logger writeTo="NoLayout" levels="Info" name="NoLayout" final="true" />
              <logger writeTo="File" minlevel="Debug" name="*" />
            </rules>
        </nlog>
         
         */

    }
}
