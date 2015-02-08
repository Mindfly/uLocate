using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uLocate;

namespace uLocate.UI.App_Plugins.uLocate.Installer
{
    using System.Configuration;
    using System.IO;
    using System.Web.Configuration;

    using Umbraco.Core.IO;

    public partial class install : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            bool DbResult = InstallDatabaseTables();

            if (DbResult == true)
            {
                this.lblDbResult.Text = "Successfully installed uLocate Database tables.";
            }
            else
            {
                this.lblDbResult.Text = "Error installing uLocate Database tables. Check the umbracoLog for details.";
            }
        }
        
        protected bool InstallDatabaseTables()
        {
            return Data.Helper.InitializeDatabase();
        }

        protected bool UpdateWebConfig()
        {
            bool Result = false;
        //    string ConfigSectionsName = "configSections";
        //    try
        //    {
        //        var webConfig = WebConfigurationManager.OpenWebConfiguration("~/");
        //        if (webConfig.Sections[ConfigSectionsName] == null)
        //        {
        //            webConfig.Sections.Add(ConfigSectionsName, new ConfigSection());
        //            string configPath = string.Concat("config", Path.DirectorySeparatorChar, ConfigSectionsName, ".config");
        //            string xmlPath = IOHelper.MapPath(string.Concat("~/", configPath));
        //            string xml;
        //            using (var reader = new StreamReader(xmlPath))
        //            {
        //                xml = reader.ReadToEnd();
        //            }
        //            webConfig.Sections[ConfigSectionsName].SectionInformation.SetRawXml(xml);
        //            webConfig.Sections[ConfigSectionsName].SectionInformation.ConfigSource = configPath;
        //            webConfig.Save(ConfigurationSaveMode.Modified);
        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        string message = string.Concat("Error at install ", this.Alias(), " package action: ", ex);
        //        Log.Add(LogTypes.Error, -1, message);
        //    }
            return Result;
        }
    }

    
}