﻿#pragma checksum "..\..\FacebookPG.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "44A0949D1CDE19191C0AC5DE9A6444C8"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using WCCFNew;


namespace WCCFNew {
    
    
    /// <summary>
    /// FacebookPG
    /// </summary>
    public partial class FacebookPG : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 31 "..\..\FacebookPG.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblDirectionToSendFB;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\FacebookPG.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid FBwallTXTB;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\FacebookPG.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox lblInstructionsFB;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\FacebookPG.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox cbWall;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\FacebookPG.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox cbPage;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\FacebookPG.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label FBEnterMSGLBL;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\FacebookPG.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtMessageFB;
        
        #line default
        #line hidden
        
        
        #line 59 "..\..\FacebookPG.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSettings;
        
        #line default
        #line hidden
        
        
        #line 66 "..\..\FacebookPG.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnClearFB;
        
        #line default
        #line hidden
        
        
        #line 67 "..\..\FacebookPG.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSubmitFB;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/WCCFNew;component/facebookpg.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\FacebookPG.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.lblDirectionToSendFB = ((System.Windows.Controls.Label)(target));
            return;
            case 2:
            this.FBwallTXTB = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.lblInstructionsFB = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 4:
            this.cbWall = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 5:
            this.cbPage = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 6:
            this.FBEnterMSGLBL = ((System.Windows.Controls.Label)(target));
            return;
            case 7:
            this.txtMessageFB = ((System.Windows.Controls.TextBox)(target));
            return;
            case 8:
            this.btnSettings = ((System.Windows.Controls.Button)(target));
            
            #line 59 "..\..\FacebookPG.xaml"
            this.btnSettings.Click += new System.Windows.RoutedEventHandler(this.btnSettings_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.btnClearFB = ((System.Windows.Controls.Button)(target));
            return;
            case 10:
            this.btnSubmitFB = ((System.Windows.Controls.Button)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

