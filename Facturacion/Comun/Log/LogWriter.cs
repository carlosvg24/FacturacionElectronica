using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace ReservacionesGrupos.Class
{
    public class LogWriter
    {
        public string Serialize(object dataToSerialize)
        {
            try
            {
                if (dataToSerialize == null) return null;
                using (StringWriter stringwriter = new StringWriter())
                {
                    var serializer = new XmlSerializer(dataToSerialize.GetType());
                    serializer.Serialize(stringwriter, dataToSerialize);
                    return stringwriter.ToString();
                }

            }
            catch (Exception ex)
            {
                return ex.Message + " " + ex.InnerException;
            }
        }

        public string Serialize(object[] dataToSerialize)
        {
            try
            {
                if (dataToSerialize == null) return null;
                StringBuilder strstringwriter = new StringBuilder();

                foreach (object obj in dataToSerialize)
                {
                    strstringwriter.AppendLine(Serialize(obj));
                }
                return strstringwriter.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message + " " + ex.InnerException;
            }
        }

        public void LogVentaExitosa(string strXML, string PNR, bool EsError)
        {
            try
            {

                if (Directory.Exists(ConfigurationManager.AppSettings["PathLog"].ToString()) == false)
                {
                    Directory.CreateDirectory(ConfigurationManager.AppSettings["PathLog"].ToString());
                }

                if (Directory.Exists(ConfigurationManager.AppSettings["PathLog"].ToString() + "//" + ConfigurationManager.AppSettings["LogFiles"].ToString() + "//" + ConfigurationManager.AppSettings["LogExito"].ToString()) == false)
                {
                    Directory.CreateDirectory(ConfigurationManager.AppSettings["PathLog"].ToString() + "//" + ConfigurationManager.AppSettings["LogFiles"].ToString() + "//" + ConfigurationManager.AppSettings["LogExito"].ToString());
                }

                if (Directory.Exists(ConfigurationManager.AppSettings["PathLog"].ToString() + "//" + ConfigurationManager.AppSettings["LogFiles"].ToString() + "//" + ConfigurationManager.AppSettings["LogError"].ToString()) == false)
                {
                    Directory.CreateDirectory(ConfigurationManager.AppSettings["PathLog"].ToString() + "//" + ConfigurationManager.AppSettings["LogFiles"].ToString() + "//" + ConfigurationManager.AppSettings["LogError"].ToString());
                }


                if (EsError == false)
                {
                    using (FileStream fs = File.Create(ConfigurationManager.AppSettings["PathLog"].ToString() + "//" + ConfigurationManager.AppSettings["LogFiles"].ToString() + "//" +
                        ConfigurationManager.AppSettings["LogExito"].ToString() + "//" + PNR + ".xml"))
                    {
                        Byte[] info = new UTF8Encoding(true).GetBytes(strXML);
                        fs.Write(info, 0, info.Length);
                    }
                }
                else
                {
                    if (strXML != null)
                    {
                        using (FileStream fs = File.Create(ConfigurationManager.AppSettings["PathLog"].ToString() + "//" + ConfigurationManager.AppSettings["LogFiles"].ToString() + "//" +
                            ConfigurationManager.AppSettings["LogError"].ToString() + "//" + PNR + ".xml"))
                        {
                            Byte[] info = new UTF8Encoding(true).GetBytes(strXML);
                            fs.Write(info, 0, info.Length);
                        }
                    }
                }
            }
            catch
            {
            }
        }

        public LogWriter(string logMessage)
        {
            LogWrite(logMessage);
        }

        public void LogWrite(string logMessage)
        {
            if (Directory.Exists(ConfigurationManager.AppSettings["PathLog"].ToString()) == false)
            {
                Directory.CreateDirectory(ConfigurationManager.AppSettings["PathLog"].ToString());
            }

            if (Directory.Exists(ConfigurationManager.AppSettings["PathLog"].ToString() + "//" + ConfigurationManager.AppSettings["LogFiles"].ToString()) == false)
            {
                Directory.CreateDirectory(ConfigurationManager.AppSettings["PathLog"].ToString() + "//" + ConfigurationManager.AppSettings["LogFiles"].ToString());
            }
            try
            {
                using (StreamWriter w = File.AppendText(ConfigurationManager.AppSettings["PathLog"].ToString() + "//" +
                        ConfigurationManager.AppSettings["LogFiles"].ToString() + "\\" + DateTime.Now.ToString("yyyyMMdd")+ "_LogReservas.txt"))
                {
                    Log(logMessage, w);
                }
            }
            catch //(Exception ex)
            {
            }
        }
        public void Log(string logMessage, TextWriter txtWriter)
        {
            try
            {
                txtWriter.Write("\r\nLog Message {0} {1} :", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString());
                txtWriter.WriteLine("  :{0}", logMessage);
                txtWriter.WriteLine("-------------------------------");
            }
            catch //(Exception ex)
            {
            }
        }
    }
}
