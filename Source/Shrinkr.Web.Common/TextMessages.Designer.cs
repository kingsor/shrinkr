﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30128.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Shrinkr.Web {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class TextMessages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal TextMessages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Shrinkr.Web.TextMessages", typeof(TextMessages).Assembly);
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
        ///   Looks up a localized string similar to Invalid OpenID user name..
        /// </summary>
        internal static string InvalidOpenIDUserName {
            get {
                return ResourceManager.GetString("InvalidOpenIDUserName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to OpenID user name cannot be blank..
        /// </summary>
        internal static string OpenIDUserNameCannotBeBlank {
            get {
                return ResourceManager.GetString("OpenIDUserNameCannotBeBlank", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Something unholy going on!!!!.
        /// </summary>
        internal static string SomethingUnholyGoingOn {
            get {
                return ResourceManager.GetString("SomethingUnholyGoingOn", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to login with your preferred OpenID provider..
        /// </summary>
        internal static string UnableToLoginWithYourPreferredOpenIDProvider {
            get {
                return ResourceManager.GetString("UnableToLoginWithYourPreferredOpenIDProvider", resourceCulture);
            }
        }
    }
}
