﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3082
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Esath.Pie.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Esath.Pie.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to N/A.
        /// </summary>
        internal static string LineOfCode_NoCodeSelected {
            get {
                return ResourceManager.GetString("LineOfCode_NoCodeSelected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0}.
        /// </summary>
        internal static string LineOfCode_SelectionFormat {
            get {
                return ResourceManager.GetString("LineOfCode_SelectionFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to N/A.
        /// </summary>
        internal static string NowEditing_NoExpressionSelected {
            get {
                return ResourceManager.GetString("NowEditing_NoExpressionSelected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Param {0}:.
        /// </summary>
        internal static string ParameterFormat {
            get {
                return ResourceManager.GetString("ParameterFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to (Browse for more variables).
        /// </summary>
        internal static string VarSelector_Lurkmoar {
            get {
                return ResourceManager.GetString("VarSelector_Lurkmoar", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ------------------.
        /// </summary>
        internal static string VarSelector_Separator {
            get {
                return ResourceManager.GetString("VarSelector_Separator", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Warning.
        /// </summary>
        internal static string Warning_Caption {
            get {
                return ResourceManager.GetString("Warning_Caption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This operation will result in truncation of one or more expressions in the script being edited. The truncation will be irreversible. Do you wish to proceed?.
        /// </summary>
        internal static string Warning_IrreversibleChange {
            get {
                return ResourceManager.GetString("Warning_IrreversibleChange", resourceCulture);
            }
        }
    }
}
