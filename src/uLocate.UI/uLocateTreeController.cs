namespace uLocate.UI
{
    using System;
    using System.Net.Http.Formatting;

    using umbraco;
    using umbraco.BusinessLogic.Actions;

    using Umbraco.Core;

    using Umbraco.Web.Models.Trees;
    using Umbraco.Web.Mvc;
    using Umbraco.Web.Trees;

    [Tree("uLocate", "uLocate", "uLocate")]
    [PluginController("uLocate")]
    public class uLocateTreeController : TreeController
    {
        protected override TreeNodeCollection GetTreeNodes(string id, FormDataCollection queryStrings)
        {
            if (id == Constants.System.Root.ToInvariantString())
            {
                var tree = new TreeNodeCollection()
                    {
                        CreateTreeNode("1", id, queryStrings, "Locations", "icon-map-loaction", "uLocate/uLocate/locations/view"),
                        CreateTreeNode("2", id, queryStrings, "Location Types", "icon-pin-location", "uLocate/uLocate/locationTypes/view"),
                        CreateTreeNode("3", id, queryStrings, "Import Locations", "icon-page-up", "uLocate/uLocate/import/edit"),
                        CreateTreeNode("4", id, queryStrings, "Export Locations", "icon-page-down", "uLocate/uLocate/export/edit")
                    };
                return tree;
            }

            //this tree doesn't support rendering more than 1 level
            throw new NotSupportedException();
        }

        protected override MenuItemCollection GetMenuForNode(string id, FormDataCollection queryStrings)
        {
            var menu = new MenuItemCollection();

            if (id == Constants.System.Root.ToInvariantString())
            {
                menu.Items.Add<RefreshNode, ActionRefresh>(ui.Text("actions", ActionRefresh.Instance.Alias));
            }
            if (id == "1" || id == "2")
            {
                menu.Items.Add<CreateChildEntity, ActionNew>(ui.Text("actions", ActionNew.Instance.Alias));
            }

            return menu;
        }
    }
}
