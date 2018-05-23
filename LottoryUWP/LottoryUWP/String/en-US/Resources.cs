//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// --------------------------------------------------------------------------------------------------
// <auto-generatedInfo>
// 	This code was generated by ResW File Code Generator (http://bit.ly/reswcodegen)
// 	ResW File Code Generator was written by Christian Resma Helle
// 	and is under GNU General Public License version 2 (GPLv2)
// 
// 	This code contains a helper class exposing property representations
// 	of the string resources defined in the specified .ResW file
// 
// 	Generated: 05/23/2018 19:37:42
// </auto-generatedInfo>
// --------------------------------------------------------------------------------------------------
namespace LottoryUWP.Strings
{
    using Windows.ApplicationModel.Resources;
    
    
    public sealed partial class Resources
    {
        
        private static ResourceLoader resourceLoader;
        
        static Resources()
        {
            string executingAssemblyName;
            executingAssemblyName = Windows.UI.Xaml.Application.Current.GetType().AssemblyQualifiedName;
            string[] executingAssemblySplit;
            executingAssemblySplit = executingAssemblyName.Split(',');
            executingAssemblyName = executingAssemblySplit[1];
            string currentAssemblyName;
            currentAssemblyName = typeof(Resources).AssemblyQualifiedName;
            string[] currentAssemblySplit;
            currentAssemblySplit = currentAssemblyName.Split(',');
            currentAssemblyName = currentAssemblySplit[1];
            if (executingAssemblyName.Equals(currentAssemblyName))
            {
                resourceLoader = ResourceLoader.GetForCurrentView("Resources");
            }
            else
            {
                resourceLoader = ResourceLoader.GetForCurrentView(currentAssemblyName + "/Resources");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Let��s Lucky Draw, who is the next winner?"
        /// </summary>
        public static string AppDescription
        {
            get
            {
                return resourceLoader.GetString("AppDescription");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Let��s Lucky Draw"
        /// </summary>
        public static string AppName
        {
            get
            {
                return resourceLoader.GetString("AppName");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Starting {0} | {1}"
        /// </summary>
        public static string InfoBaar_Starting
        {
            get
            {
                return resourceLoader.GetString("InfoBaar_Starting");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Applied Group Capacity {0}"
        /// </summary>
        public static string InfoBar_Capacity_Applied
        {
            get
            {
                return resourceLoader.GetString("InfoBar_Capacity_Applied");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Free Draw"
        /// </summary>
        public static string InfoBar_Capacity_FreeDraw
        {
            get
            {
                return resourceLoader.GetString("InfoBar_Capacity_FreeDraw");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "{0} | {1}"
        /// </summary>
        public static string InfoBar_Default
        {
            get
            {
                return resourceLoader.GetString("InfoBar_Default");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "{0} out of {1} Lucky Winner(s)"
        /// </summary>
        public static string InfoBar_DrawInfo_Capacity
        {
            get
            {
                return resourceLoader.GetString("InfoBar_DrawInfo_Capacity");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "{0} Lucky Winner(s)"
        /// </summary>
        public static string InfoBar_DrawInfo_FreeDraw
        {
            get
            {
                return resourceLoader.GetString("InfoBar_DrawInfo_FreeDraw");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "{0} | {1} | {2}"
        /// </summary>
        public static string InfoBar_Running
        {
            get
            {
                return resourceLoader.GetString("InfoBar_Running");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Next: {0} | {1}"
        /// </summary>
        public static string InfoBar_Stopped
        {
            get
            {
                return resourceLoader.GetString("InfoBar_Stopped");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Visual Appearance"
        /// </summary>
        public static string SettingTitle_Event_Appearance
        {
            get
            {
                return resourceLoader.GetString("SettingTitle_Event_Appearance");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Data Source"
        /// </summary>
        public static string SettingTitle_Event_Data
        {
            get
            {
                return resourceLoader.GetString("SettingTitle_Event_Data");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Misc"
        /// </summary>
        public static string SettingTitle_Event_Support
        {
            get
            {
                return resourceLoader.GetString("SettingTitle_Event_Support");
            }
        }
    }
}
