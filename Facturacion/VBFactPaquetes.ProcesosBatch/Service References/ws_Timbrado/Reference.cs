﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VBFactPaquetes.ProcesosBatch.ws_Timbrado {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="urn:flo_api", ConfigurationName="ws_Timbrado.FacturaloTimbradoWSPortType")]
    public interface FacturaloTimbradoWSPortType {
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:FacturaloTimbradoWS#timbrarTXT", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc, SupportFaults=true, Use=System.ServiceModel.OperationFormatUse.Encoded)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        VBFactPaquetes.ProcesosBatch.ws_Timbrado.RespuestaTimbrado timbrar(string apikey, string request);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:FacturaloTimbradoWS#timbrarTXT", ReplyAction="*")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        System.Threading.Tasks.Task<VBFactPaquetes.ProcesosBatch.ws_Timbrado.RespuestaTimbrado> timbrarAsync(string apikey, string request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.4084.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.SoapTypeAttribute(Namespace="urn:flo_api")]
    public partial class RespuestaTimbrado : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string codigoField;
        
        private string subcoddeField;
        
        private string responseField;
        
        /// <remarks/>
        public string codigo {
            get {
                return this.codigoField;
            }
            set {
                this.codigoField = value;
                this.RaisePropertyChanged("codigo");
            }
        }
        
        /// <remarks/>
        public string subcodde {
            get {
                return this.subcoddeField;
            }
            set {
                this.subcoddeField = value;
                this.RaisePropertyChanged("subcodde");
            }
        }
        
        /// <remarks/>
        public string response {
            get {
                return this.responseField;
            }
            set {
                this.responseField = value;
                this.RaisePropertyChanged("response");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface FacturaloTimbradoWSPortTypeChannel : VBFactPaquetes.ProcesosBatch.ws_Timbrado.FacturaloTimbradoWSPortType, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class FacturaloTimbradoWSPortTypeClient : System.ServiceModel.ClientBase<VBFactPaquetes.ProcesosBatch.ws_Timbrado.FacturaloTimbradoWSPortType>, VBFactPaquetes.ProcesosBatch.ws_Timbrado.FacturaloTimbradoWSPortType {
        
        public FacturaloTimbradoWSPortTypeClient() {
        }
        
        public FacturaloTimbradoWSPortTypeClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public FacturaloTimbradoWSPortTypeClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public FacturaloTimbradoWSPortTypeClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public FacturaloTimbradoWSPortTypeClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public VBFactPaquetes.ProcesosBatch.ws_Timbrado.RespuestaTimbrado timbrar(string apikey, string request) {
            return base.Channel.timbrar(apikey, request);
        }
        
        public System.Threading.Tasks.Task<VBFactPaquetes.ProcesosBatch.ws_Timbrado.RespuestaTimbrado> timbrarAsync(string apikey, string request) {
            return base.Channel.timbrarAsync(apikey, request);
        }
    }
}