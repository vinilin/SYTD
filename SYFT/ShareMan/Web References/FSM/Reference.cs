﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:2.0.50727.3603
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

// 
// 此源代码是由 Microsoft.VSDesigner 2.0.50727.3603 版自动生成。
// 
#pragma warning disable 1591

namespace ShareMan.FSM {
    using System.Diagnostics;
    using System.Web.Services;
    using System.ComponentModel;
    using System.Web.Services.Protocols;
    using System;
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="FileSetManSoap", Namespace="http://localhost/FSM")]
    public partial class FileSetMan : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback CreateFileSetOperationCompleted;
        
        private System.Threading.SendOrPostCallback QureyFileSetOperationCompleted;
        
        private System.Threading.SendOrPostCallback DeleteFileSetOperationCompleted;
        
        private System.Threading.SendOrPostCallback CommitFileSetOperationCompleted;
        
        private System.Threading.SendOrPostCallback ProgressNotifyOperationCompleted;
        
        private System.Threading.SendOrPostCallback CommitFileOperationCompleted;
        
        private System.Threading.SendOrPostCallback CommitErrorOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public FileSetMan() {
            this.Url = global::ShareMan.Properties.Settings.Default.ShareMan_FSM_FileSetMan;
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
        public event CreateFileSetCompletedEventHandler CreateFileSetCompleted;
        
        /// <remarks/>
        public event QureyFileSetCompletedEventHandler QureyFileSetCompleted;
        
        /// <remarks/>
        public event DeleteFileSetCompletedEventHandler DeleteFileSetCompleted;
        
        /// <remarks/>
        public event CommitFileSetCompletedEventHandler CommitFileSetCompleted;
        
        /// <remarks/>
        public event ProgressNotifyCompletedEventHandler ProgressNotifyCompleted;
        
        /// <remarks/>
        public event CommitFileCompletedEventHandler CommitFileCompleted;
        
        /// <remarks/>
        public event CommitErrorCompletedEventHandler CommitErrorCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://localhost/FSM/CreateFileSet", RequestNamespace="http://localhost/FSM", ResponseNamespace="http://localhost/FSM", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public long CreateFileSet(FileSet fset, bool needProgress) {
            object[] results = this.Invoke("CreateFileSet", new object[] {
                        fset,
                        needProgress});
            return ((long)(results[0]));
        }
        
        /// <remarks/>
        public void CreateFileSetAsync(FileSet fset, bool needProgress) {
            this.CreateFileSetAsync(fset, needProgress, null);
        }
        
        /// <remarks/>
        public void CreateFileSetAsync(FileSet fset, bool needProgress, object userState) {
            if ((this.CreateFileSetOperationCompleted == null)) {
                this.CreateFileSetOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCreateFileSetOperationCompleted);
            }
            this.InvokeAsync("CreateFileSet", new object[] {
                        fset,
                        needProgress}, this.CreateFileSetOperationCompleted, userState);
        }
        
        private void OnCreateFileSetOperationCompleted(object arg) {
            if ((this.CreateFileSetCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.CreateFileSetCompleted(this, new CreateFileSetCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://localhost/FSM/QureyFileSet", RequestNamespace="http://localhost/FSM", ResponseNamespace="http://localhost/FSM", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public FileSet QureyFileSet(long id) {
            object[] results = this.Invoke("QureyFileSet", new object[] {
                        id});
            return ((FileSet)(results[0]));
        }
        
        /// <remarks/>
        public void QureyFileSetAsync(long id) {
            this.QureyFileSetAsync(id, null);
        }
        
        /// <remarks/>
        public void QureyFileSetAsync(long id, object userState) {
            if ((this.QureyFileSetOperationCompleted == null)) {
                this.QureyFileSetOperationCompleted = new System.Threading.SendOrPostCallback(this.OnQureyFileSetOperationCompleted);
            }
            this.InvokeAsync("QureyFileSet", new object[] {
                        id}, this.QureyFileSetOperationCompleted, userState);
        }
        
        private void OnQureyFileSetOperationCompleted(object arg) {
            if ((this.QureyFileSetCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.QureyFileSetCompleted(this, new QureyFileSetCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://localhost/FSM/DeleteFileSet", RequestNamespace="http://localhost/FSM", ResponseNamespace="http://localhost/FSM", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void DeleteFileSet(long id) {
            this.Invoke("DeleteFileSet", new object[] {
                        id});
        }
        
        /// <remarks/>
        public void DeleteFileSetAsync(long id) {
            this.DeleteFileSetAsync(id, null);
        }
        
        /// <remarks/>
        public void DeleteFileSetAsync(long id, object userState) {
            if ((this.DeleteFileSetOperationCompleted == null)) {
                this.DeleteFileSetOperationCompleted = new System.Threading.SendOrPostCallback(this.OnDeleteFileSetOperationCompleted);
            }
            this.InvokeAsync("DeleteFileSet", new object[] {
                        id}, this.DeleteFileSetOperationCompleted, userState);
        }
        
        private void OnDeleteFileSetOperationCompleted(object arg) {
            if ((this.DeleteFileSetCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.DeleteFileSetCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://localhost/FSM/CommitFileSet", RequestNamespace="http://localhost/FSM", ResponseNamespace="http://localhost/FSM", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public int CommitFileSet(long id) {
            object[] results = this.Invoke("CommitFileSet", new object[] {
                        id});
            return ((int)(results[0]));
        }
        
        /// <remarks/>
        public void CommitFileSetAsync(long id) {
            this.CommitFileSetAsync(id, null);
        }
        
        /// <remarks/>
        public void CommitFileSetAsync(long id, object userState) {
            if ((this.CommitFileSetOperationCompleted == null)) {
                this.CommitFileSetOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCommitFileSetOperationCompleted);
            }
            this.InvokeAsync("CommitFileSet", new object[] {
                        id}, this.CommitFileSetOperationCompleted, userState);
        }
        
        private void OnCommitFileSetOperationCompleted(object arg) {
            if ((this.CommitFileSetCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.CommitFileSetCompleted(this, new CommitFileSetCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://localhost/FSM/ProgressNotify", RequestNamespace="http://localhost/FSM", ResponseNamespace="http://localhost/FSM", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public int ProgressNotify(long id, long total, long downloaded) {
            object[] results = this.Invoke("ProgressNotify", new object[] {
                        id,
                        total,
                        downloaded});
            return ((int)(results[0]));
        }
        
        /// <remarks/>
        public void ProgressNotifyAsync(long id, long total, long downloaded) {
            this.ProgressNotifyAsync(id, total, downloaded, null);
        }
        
        /// <remarks/>
        public void ProgressNotifyAsync(long id, long total, long downloaded, object userState) {
            if ((this.ProgressNotifyOperationCompleted == null)) {
                this.ProgressNotifyOperationCompleted = new System.Threading.SendOrPostCallback(this.OnProgressNotifyOperationCompleted);
            }
            this.InvokeAsync("ProgressNotify", new object[] {
                        id,
                        total,
                        downloaded}, this.ProgressNotifyOperationCompleted, userState);
        }
        
        private void OnProgressNotifyOperationCompleted(object arg) {
            if ((this.ProgressNotifyCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ProgressNotifyCompleted(this, new ProgressNotifyCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://localhost/FSM/CommitFile", RequestNamespace="http://localhost/FSM", ResponseNamespace="http://localhost/FSM", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void CommitFile(long fsid, long fid) {
            this.Invoke("CommitFile", new object[] {
                        fsid,
                        fid});
        }
        
        /// <remarks/>
        public void CommitFileAsync(long fsid, long fid) {
            this.CommitFileAsync(fsid, fid, null);
        }
        
        /// <remarks/>
        public void CommitFileAsync(long fsid, long fid, object userState) {
            if ((this.CommitFileOperationCompleted == null)) {
                this.CommitFileOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCommitFileOperationCompleted);
            }
            this.InvokeAsync("CommitFile", new object[] {
                        fsid,
                        fid}, this.CommitFileOperationCompleted, userState);
        }
        
        private void OnCommitFileOperationCompleted(object arg) {
            if ((this.CommitFileCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.CommitFileCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://localhost/FSM/CommitError", RequestNamespace="http://localhost/FSM", ResponseNamespace="http://localhost/FSM", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void CommitError(string msg) {
            this.Invoke("CommitError", new object[] {
                        msg});
        }
        
        /// <remarks/>
        public void CommitErrorAsync(string msg) {
            this.CommitErrorAsync(msg, null);
        }
        
        /// <remarks/>
        public void CommitErrorAsync(string msg, object userState) {
            if ((this.CommitErrorOperationCompleted == null)) {
                this.CommitErrorOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCommitErrorOperationCompleted);
            }
            this.InvokeAsync("CommitError", new object[] {
                        msg}, this.CommitErrorOperationCompleted, userState);
        }
        
        private void OnCommitErrorOperationCompleted(object arg) {
            if ((this.CommitErrorCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.CommitErrorCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3082")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://localhost/FSM")]
    public partial class FileSet {
        
        private long idField;
        
        private string pathField;
        
        private bool readyField;
        
        private System.Nullable<long> totalField;
        
        private System.Nullable<long> downloadedField;
        
        private File[] fileField;
        
        /// <remarks/>
        public long ID {
            get {
                return this.idField;
            }
            set {
                this.idField = value;
            }
        }
        
        /// <remarks/>
        public string Path {
            get {
                return this.pathField;
            }
            set {
                this.pathField = value;
            }
        }
        
        /// <remarks/>
        public bool Ready {
            get {
                return this.readyField;
            }
            set {
                this.readyField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<long> total {
            get {
                return this.totalField;
            }
            set {
                this.totalField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<long> downloaded {
            get {
                return this.downloadedField;
            }
            set {
                this.downloadedField = value;
            }
        }
        
        /// <remarks/>
        public File[] File {
            get {
                return this.fileField;
            }
            set {
                this.fileField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3082")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://localhost/FSM")]
    public partial class File {
        
        private long idField;
        
        private long fileSetIDField;
        
        private string fileNameField;
        
        private long sizeField;
        
        private int showIndexField;
        
        /// <remarks/>
        public long ID {
            get {
                return this.idField;
            }
            set {
                this.idField = value;
            }
        }
        
        /// <remarks/>
        public long FileSetID {
            get {
                return this.fileSetIDField;
            }
            set {
                this.fileSetIDField = value;
            }
        }
        
        /// <remarks/>
        public string FileName {
            get {
                return this.fileNameField;
            }
            set {
                this.fileNameField = value;
            }
        }
        
        /// <remarks/>
        public long Size {
            get {
                return this.sizeField;
            }
            set {
                this.sizeField = value;
            }
        }
        
        /// <remarks/>
        public int ShowIndex {
            get {
                return this.showIndexField;
            }
            set {
                this.showIndexField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    public delegate void CreateFileSetCompletedEventHandler(object sender, CreateFileSetCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CreateFileSetCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal CreateFileSetCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public long Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((long)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    public delegate void QureyFileSetCompletedEventHandler(object sender, QureyFileSetCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class QureyFileSetCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal QureyFileSetCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public FileSet Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((FileSet)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    public delegate void DeleteFileSetCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    public delegate void CommitFileSetCompletedEventHandler(object sender, CommitFileSetCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CommitFileSetCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal CommitFileSetCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public int Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((int)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    public delegate void ProgressNotifyCompletedEventHandler(object sender, ProgressNotifyCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ProgressNotifyCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ProgressNotifyCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public int Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((int)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    public delegate void CommitFileCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    public delegate void CommitErrorCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
}

#pragma warning restore 1591