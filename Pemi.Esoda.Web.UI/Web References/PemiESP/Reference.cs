﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.17929.
// 
#pragma warning disable 1591

namespace Pemi.Esoda.Web.UI.PemiESP {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="ESPWebServiceSoap", Namespace="http://pemi.esoda.esp/")]
    public partial class ESPWebService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback GetAwaitingDocumentsOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetDocumentOperationCompleted;
        
        private System.Threading.SendOrPostCallback ConfirmDocumentReceiveOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public ESPWebService() {
            this.Url = global::Pemi.Esoda.Web.UI.Properties.Settings.Default.Pemi_Esoda_Web_UI_PemiESP_ESPWebService;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event GetAwaitingDocumentsCompletedEventHandler GetAwaitingDocumentsCompleted;
        
        /// <remarks/>
        public event GetDocumentCompletedEventHandler GetDocumentCompleted;
        
        /// <remarks/>
        public event ConfirmDocumentReceiveCompletedEventHandler ConfirmDocumentReceiveCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://pemi.esoda.esp/GetAwaitingDocuments", RequestNamespace="http://pemi.esoda.esp/", ResponseNamespace="http://pemi.esoda.esp/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetAwaitingDocuments(string ticket) {
            object[] results = this.Invoke("GetAwaitingDocuments", new object[] {
                        ticket});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetAwaitingDocumentsAsync(string ticket) {
            this.GetAwaitingDocumentsAsync(ticket, null);
        }
        
        /// <remarks/>
        public void GetAwaitingDocumentsAsync(string ticket, object userState) {
            if ((this.GetAwaitingDocumentsOperationCompleted == null)) {
                this.GetAwaitingDocumentsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetAwaitingDocumentsOperationCompleted);
            }
            this.InvokeAsync("GetAwaitingDocuments", new object[] {
                        ticket}, this.GetAwaitingDocumentsOperationCompleted, userState);
        }
        
        private void OnGetAwaitingDocumentsOperationCompleted(object arg) {
            if ((this.GetAwaitingDocumentsCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetAwaitingDocumentsCompleted(this, new GetAwaitingDocumentsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://pemi.esoda.esp/GetDocument", RequestNamespace="http://pemi.esoda.esp/", ResponseNamespace="http://pemi.esoda.esp/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetDocument(string ticket, string idDocument) {
            object[] results = this.Invoke("GetDocument", new object[] {
                        ticket,
                        idDocument});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetDocumentAsync(string ticket, string idDocument) {
            this.GetDocumentAsync(ticket, idDocument, null);
        }
        
        /// <remarks/>
        public void GetDocumentAsync(string ticket, string idDocument, object userState) {
            if ((this.GetDocumentOperationCompleted == null)) {
                this.GetDocumentOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetDocumentOperationCompleted);
            }
            this.InvokeAsync("GetDocument", new object[] {
                        ticket,
                        idDocument}, this.GetDocumentOperationCompleted, userState);
        }
        
        private void OnGetDocumentOperationCompleted(object arg) {
            if ((this.GetDocumentCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetDocumentCompleted(this, new GetDocumentCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://pemi.esoda.esp/ConfirmDocumentReceive", RequestNamespace="http://pemi.esoda.esp/", ResponseNamespace="http://pemi.esoda.esp/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string ConfirmDocumentReceive(string Ticket, string DocumentID) {
            object[] results = this.Invoke("ConfirmDocumentReceive", new object[] {
                        Ticket,
                        DocumentID});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void ConfirmDocumentReceiveAsync(string Ticket, string DocumentID) {
            this.ConfirmDocumentReceiveAsync(Ticket, DocumentID, null);
        }
        
        /// <remarks/>
        public void ConfirmDocumentReceiveAsync(string Ticket, string DocumentID, object userState) {
            if ((this.ConfirmDocumentReceiveOperationCompleted == null)) {
                this.ConfirmDocumentReceiveOperationCompleted = new System.Threading.SendOrPostCallback(this.OnConfirmDocumentReceiveOperationCompleted);
            }
            this.InvokeAsync("ConfirmDocumentReceive", new object[] {
                        Ticket,
                        DocumentID}, this.ConfirmDocumentReceiveOperationCompleted, userState);
        }
        
        private void OnConfirmDocumentReceiveOperationCompleted(object arg) {
            if ((this.ConfirmDocumentReceiveCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ConfirmDocumentReceiveCompleted(this, new ConfirmDocumentReceiveCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void GetAwaitingDocumentsCompletedEventHandler(object sender, GetAwaitingDocumentsCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetAwaitingDocumentsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetAwaitingDocumentsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void GetDocumentCompletedEventHandler(object sender, GetDocumentCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetDocumentCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetDocumentCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void ConfirmDocumentReceiveCompletedEventHandler(object sender, ConfirmDocumentReceiveCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ConfirmDocumentReceiveCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ConfirmDocumentReceiveCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591