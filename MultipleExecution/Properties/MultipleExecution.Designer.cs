﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.488
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MultipleExecution.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
    public sealed partial class MultipleExecution : global::System.Configuration.ApplicationSettingsBase {
        
        private static MultipleExecution defaultInstance = ((MultipleExecution)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new MultipleExecution())));
        
        public static MultipleExecution Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int Fast_Changing_Variable_Initial {
            get {
                return ((int)(this["Fast_Changing_Variable_Initial"]));
            }
            set {
                this["Fast_Changing_Variable_Initial"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int Slow_Changing_Variable_Initial {
            get {
                return ((int)(this["Slow_Changing_Variable_Initial"]));
            }
            set {
                this["Slow_Changing_Variable_Initial"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("N")]
        public string Fast_Changing_Variable {
            get {
                return ((string)(this["Fast_Changing_Variable"]));
            }
            set {
                this["Fast_Changing_Variable"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("50")]
        public int Fast_Changing_Variable_Minimum {
            get {
                return ((int)(this["Fast_Changing_Variable_Minimum"]));
            }
            set {
                this["Fast_Changing_Variable_Minimum"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("150")]
        public int Fast_Changing_Variable_Maximum {
            get {
                return ((int)(this["Fast_Changing_Variable_Maximum"]));
            }
            set {
                this["Fast_Changing_Variable_Maximum"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("10")]
        public int Fast_Changing_Variable_Increment {
            get {
                return ((int)(this["Fast_Changing_Variable_Increment"]));
            }
            set {
                this["Fast_Changing_Variable_Increment"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<ArrayOfString xmlns:xsi=\"http://www.w3." +
            "org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <s" +
            "tring>50 100 150 200</string>\r\n</ArrayOfString>")]
        public global::System.Collections.Specialized.StringCollection Slow_Changing_Variable_Values {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["Slow_Changing_Variable_Values"]));
            }
            set {
                this["Slow_Changing_Variable_Values"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string Slow_Changing_Variable {
            get {
                return ((string)(this["Slow_Changing_Variable"]));
            }
            set {
                this["Slow_Changing_Variable"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int Fast_Changing_Variable_Current {
            get {
                return ((int)(this["Fast_Changing_Variable_Current"]));
            }
            set {
                this["Fast_Changing_Variable_Current"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int Slow_Changing_Variable_Current {
            get {
                return ((int)(this["Slow_Changing_Variable_Current"]));
            }
            set {
                this["Slow_Changing_Variable_Current"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("10")]
        public int Repetitions_Per_Single_Execution {
            get {
                return ((int)(this["Repetitions_Per_Single_Execution"]));
            }
            set {
                this["Repetitions_Per_Single_Execution"] = value;
            }
        }
    }
}
