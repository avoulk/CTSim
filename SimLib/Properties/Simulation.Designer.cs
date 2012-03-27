﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.261
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SimLib.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
    public sealed partial class Simulation : global::System.Configuration.ApplicationSettingsBase {
        
        private static Simulation defaultInstance = ((Simulation)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Simulation())));
        
        public static Simulation Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("100")]
        public int Nodes {
            get {
                return ((int)(this["Nodes"]));
            }
            set {
                this["Nodes"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("30")]
        public int Field {
            get {
                return ((int)(this["Field"]));
            }
            set {
                this["Field"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("4")]
        public int Representatives {
            get {
                return ((int)(this["Representatives"]));
            }
            set {
                this["Representatives"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("5")]
        public int Sigma {
            get {
                return ((int)(this["Sigma"]));
            }
            set {
                this["Sigma"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("5")]
        public int Range {
            get {
                return ((int)(this["Range"]));
            }
            set {
                this["Range"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1000")]
        public double Energy {
            get {
                return ((double)(this["Energy"]));
            }
            set {
                this["Energy"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("10")]
        public double Energy_To_Send {
            get {
                return ((double)(this["Energy_To_Send"]));
            }
            set {
                this["Energy_To_Send"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("8")]
        public double Energy_To_Receive {
            get {
                return ((double)(this["Energy_To_Receive"]));
            }
            set {
                this["Energy_To_Receive"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("95")]
        public int QoS {
            get {
                return ((int)(this["QoS"]));
            }
            set {
                this["QoS"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("50")]
        public double Data_Initial_Mean {
            get {
                return ((double)(this["Data_Initial_Mean"]));
            }
            set {
                this["Data_Initial_Mean"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("30")]
        public double Data_Initial_Sigma {
            get {
                return ((double)(this["Data_Initial_Sigma"]));
            }
            set {
                this["Data_Initial_Sigma"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("10")]
        public int Data_Changes {
            get {
                return ((int)(this["Data_Changes"]));
            }
            set {
                this["Data_Changes"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0.3")]
        public double Data_Change_Maximum_Percentage {
            get {
                return ((double)(this["Data_Change_Maximum_Percentage"]));
            }
            set {
                this["Data_Change_Maximum_Percentage"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("10")]
        public int FF_Scale {
            get {
                return ((int)(this["FF_Scale"]));
            }
            set {
                this["FF_Scale"] = value;
            }
        }
    }
}
