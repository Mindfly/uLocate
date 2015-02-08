namespace uLocate.Package
{
    using System;
    using System.Linq;
    using System.Xml;
    using umbraco.interfaces;

    class uLocateInstallerPackageAction : IPackageAction
    {

        //Copied from https://github.com/Shazwazza/Articulate/blob/master/Articulate/ArcticulateInstallerPackageAction.cs

        public bool Execute(string packageName, XmlNode xmlData)
        {
            bool Result = uLocate.Data.Helper.InitializeDatabase();

            return Result;
        }

        public string Alias()
        {
            return "uLocateInstaller";
        }

        public bool Undo(string packageName, XmlNode xmlData)
        {
            return true;
        }

        public XmlNode SampleXml()
        {
            var xml = "<Action runat=\"install\" undo=\"false\" alias=\"" + "uLocateInstaller" + "\" />";
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            return doc.DocumentElement;
        }

    }
}
