using Microsoft.Web.Administration;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Data.Query;
using Sitecore.Layouts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace Sc8MVC
{
    public class ItemPair
    {
        public Item SourceItem { get; private set; }
        public Item DestItem { get; private set; }

        public ItemPair(Item sourceItem, Item destItem)
        {
            SourceItem = sourceItem;
            DestItem = destItem;
        }
    }

    public partial class DuplicateItems : System.Web.UI.Page
    {
        public string ErrorMessage = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Sitecore.Security.Accounts.User.Current == null || !Sitecore.Security.Accounts.User.Current.IsAdministrator)            
                Response.Redirect("/");
            if (!IsPostBack && Request.Cookies["message"] != null && !string.IsNullOrEmpty(Request.Cookies["message"].Value))
            {
                ErrorMessage = Request.Cookies["message"].Value;
                // remove cookies
                SetMesasgeInCookie("");
            }
        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {
            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var homeItem = db.GetItem(new Sitecore.Data.ID("{110D559F-DEA5-42EA-9C1C-8A5DF7E70EF9}"));

            // duplicate new home item
            var newHomeItem = homeItem.Duplicate(homeItem.DisplayName + countryCode.Text);

            if (UpdateRendering(db, homeItem, newHomeItem) && UpdateConfig(newHomeItem) && UpdateIISSite())
            {
                if (IsPublish.Checked)
                {
                    Sitecore.Publishing.PublishOptions publishOptions =
    new Sitecore.Publishing.PublishOptions(newHomeItem.Database,
                                           Database.GetDatabase("web"),
                                           Sitecore.Publishing.PublishMode.Smart,
                                           newHomeItem.Language,
                                           System.DateTime.Now);  // Create a publisher with the publishoptions
                    Sitecore.Publishing.Publisher publisher = new Sitecore.Publishing.Publisher(publishOptions);

                    // Choose where to publish from
                    publisher.Options.RootItem = newHomeItem;

                    // Publish children as well?
                    publisher.Options.Deep = true;

                    // Do the publish!
                    publisher.Publish();
                    SetMesasgeInCookie("Done", 1);
                }
                else
                {
                    SetMesasgeInCookie("Done. Please publish new site", 1);
                }
                Response.Redirect("http://sc8mvc/duplicateitems.aspx");
            }

           
        }

        public void TestThis()
        {
            Random rand = new Random();
            if (rand.Next() == 1)
            {
                int[] a = { 1, 2, 3 };
                foreach (int item in a)
                {
                    if (rand.Next() == item)
                    {
                        int[] b = { 4, 5, 6 };
                        foreach (var item1 in b)
                        {
                            int c = item1;
                        }
                    }
                    else
                    {
                        int c = item;
                    }
                }
            }
            else
            {
                int[] a = { 1, 2, 3 };
                foreach (var item in a)
                {
                    int c = item;
                    if (rand.Next() == item)
                    {
                        int[] t = { 4, 5, 6 };
                        foreach (var item1 in t)
                        {
                            int k = item1;
                        }
                    }
                    else
                    {
                        int k = item;
                    }
                }
                int[] b = { 1, 2, 3 };
                foreach (int item in b)
                {
                    if (rand.Next() == item)
                    {
                        int[] d = { 4, 5, 6 };
                        foreach (var item1 in d)
                        {
                            int c = item1;
                        }
                    }
                    else
                    {
                        int c = item;
                    }
                }
            }
            while (rand.Next() != 0)
            {
                int a = 1;
                string b = "2";
                double c = 3;

                int d = int.Parse("22");
                int e =+ 2;
                int y =+ 2;
                int i =+ 2;
                int u =+ 2;
                int o =+ 2;
                int p =+ 2;
                int r =+ 2;
                var q =+ 2;
            }
        }

        public double divide(int divisor, int dividend)
        {
            return divisor / dividend;
        }

        public void doTheThing()
        {
            int divisor = 15;
            int dividend = 5;

            double result = divide(dividend, divisor);

            using (var fs = File.Create(""))
            {
                var bytes = Encoding.UTF8.GetBytes("");
                fs.Write(bytes, 0, bytes.Length);
            }
        }

        private bool UpdateIISSite()
        {
            try
            {
                using (ServerManager serverManager = new ServerManager())
                {
                    var currentSite = serverManager.Sites.FirstOrDefault(s => s.Id == 38);
                    string bindingInfo = string.Format(@"{0}:{1}:{2}", "*", "80", hostName.Text);

                    currentSite.Bindings.Add(bindingInfo, @"http");
                    serverManager.CommitChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                SetMesasgeInCookie(ex.Message, 1);
                return false;
            }
        }

        private bool UpdateRendering(Database db, Sitecore.Data.Items.Item homeItem, Sitecore.Data.Items.Item newHomeItem)
        {
            try
            {
                // get rendering, layout
                Sitecore.Layouts.RenderingReference[] renderings = newHomeItem.Visualization.GetRenderings(Sitecore.Context.Device, true);
                LayoutField layoutField = new LayoutField(newHomeItem.Fields[Sitecore.FieldIDs.LayoutField]);
                LayoutDefinition layoutDefinition = LayoutDefinition.Parse(layoutField.Value);
                DeviceDefinition deviceDefinition = layoutDefinition.GetDevice(Sitecore.Context.Device.ID.ToString());

                // update rendering's datasource
                foreach (RenderingReference rendering in renderings)
                {
                    string datasouceString = deviceDefinition.GetRendering(rendering.RenderingID.ToString()).Datasource;
                    if (!string.IsNullOrEmpty(datasouceString))
                    {
                        // get datasource item
                        var currentDatasouceItem = Sitecore.Data.ID.IsID(datasouceString) ? db.GetItem(new Sitecore.Data.ID(datasouceString)) : db.GetItem(datasouceString);
                        if (currentDatasouceItem != null)
                        {
                            // get new path
                            datasouceString = currentDatasouceItem.Paths.FullPath.Replace(homeItem.DisplayName, newHomeItem.DisplayName);
                            var newItem = db.GetItem(datasouceString);
                            if (newItem != null)
                            {
                                // set new datasource id
                                deviceDefinition.GetRendering(rendering.RenderingID.ToString()).Datasource = newItem.ID.ToString();
                                newHomeItem.Editing.BeginEdit();
                                layoutField.Value = layoutDefinition.ToXml();
                                newHomeItem.Editing.EndEdit();
                            }
                        }

                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                SetMesasgeInCookie(ex.Message, 1);
                return false;
            }

        }

        private bool UpdateConfig(Sitecore.Data.Items.Item newHomeItem)
        {
            try
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(Server.MapPath("~/App_Config/Include/SiteDefinition.config"));
                XmlNode newNode;
                var sitesNode = xmlDoc.SelectSingleNode("/configuration/sitecore/sites");
                if (sitesNode.ChildNodes.Count == 0)
                {
                    newNode = GetNodeFromTemplate();
                    newNode = xmlDoc.ImportNode(newNode, true);
                }
                else
                {
                    newNode = sitesNode.ChildNodes[0].CloneNode(true);
                }

                if (newNode != null)
                {
                    // set attribute for new site
                    newNode.Attributes["hostName"].Value = hostName.Text;
                    newNode.Attributes["startItem"].Value = "/" + newHomeItem.Name;
                    newNode.Attributes["name"].Value = newHomeItem.Name;
                    sitesNode.AppendChild(newNode);
                    xmlDoc.Save(Server.MapPath("~/App_Config/Include/SiteDefinition.config"));
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                SetMesasgeInCookie(ex.Message, 1);
                return false;
            }
        }

        private void SetMesasgeInCookie(string message, int expire = -1)
        {
            HttpCookie cookie = new HttpCookie("message");
            cookie.Expires = DateTime.Now.AddDays(expire);
            cookie.Value = message;
            Response.Cookies.Add(cookie);
        }

        private XmlNode GetNodeFromTemplate()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Server.MapPath("~/App_Config/Include/SiteDefinitionTemplate.xml"));

            return xmlDoc.SelectSingleNode("/configuration/sitecore/sites/site");
        }

        #region Update item fields references

        public Item source;
        public Item sourceRoot;
        public Item target;
        public Item targetRoot;

#pragma warning disable CS0649 // Field 'DuplicateItems.itemPair' is never assigned to, and will always have its default value null
        List<ItemPair> itemPair;
#pragma warning restore CS0649 // Field 'DuplicateItems.itemPair' is never assigned to, and will always have its default value null

        private IEnumerable<Field> GetFieldsToProcess(Item item)
        {
            item.Fields.ReadAll();
            return item.Fields.Where(f => f.ID != FieldIDs.LayoutField && !f.Name.StartsWith("__")).ToArray();
        }

        private IEnumerable<Item> GetVersionsToProcess(Item item)
        {
            return item.Versions.GetVersions(true);
        }

        private List<ItemPair> GetItemPairs(Item homeItem, Item newHomeItem)
        {
            List<ItemPair> itemPairLocal = new List<ItemPair>();
            // get homeItem descendants
            IEnumerable<Item> homeDescendants = Query.SelectItems("descendant-or-self::*", homeItem);
            
            foreach (Item sourceDescendant in homeDescendants)
            {
                if (!CanTranslatePath(sourceDescendant))
                    continue;

                Item equivalentTarget = sourceDescendant.Database.GetItem(TranslatePath(sourceDescendant));
                if (equivalentTarget == null)
                    continue;

                itemPairLocal.Add(new ItemPair(sourceDescendant, equivalentTarget));
            }

            return itemPairLocal;
        }

        public string TranslatePath(Item item)
        {
            if (IsDescendantOrSelf(item, source))
                return GetFullPath(target, source, item);

            if (IsDescendantOrSelf(item, sourceRoot))
                return GetFullPath(targetRoot, sourceRoot, item);
            return string.Empty;
        }


        private string GetFullPath(Item closestEquivalentAncestor, Item closestAncestor, Item item)
        {
            string startPathPart = closestEquivalentAncestor.Paths.FullPath;
            string relativePathPart = GetRelativePath(closestAncestor, item);
            return StringUtil.EnsurePostfix('/', startPathPart) + StringUtil.RemovePrefix('/', relativePathPart);
        }

        private string GetRelativePath(Item closestAncestor, Item item)
        {
            return item.Paths.FullPath.Replace(closestAncestor.Paths.FullPath, string.Empty);
        }

        public bool CanTranslatePath(Item item)
        {
            return IsDescendantOrSelf(item, source) || IsDescendantOrSelf(item, sourceRoot);
        }

        private bool IsDescendantOrSelf(Item item, Item otherItem)
        {
            return item.ID == otherItem.ID || item.Axes.IsDescendantOf(otherItem);
        }

        private string GetInitialFieldValue(Field field)
        {
            return field.GetValue(true, true);
        }

        private void ProcessField(Field field)
        {
            string initialValue = GetInitialFieldValue(field);
            if (string.IsNullOrEmpty(initialValue))
                return;

            StringBuilder value = new StringBuilder(initialValue);
            foreach (ItemPair item in itemPair)
            {
                ReplaceID(item.SourceItem, item.DestItem, value);
                ReplaceShortID(item.SourceItem, item.DestItem, value);
                ReplaceFullPath(item.SourceItem, item.DestItem, value);
                ReplaceContentPath(item.SourceItem, item.DestItem, value);
            }
            UpdateFieldValue(field, initialValue, value);
        }


        private void ReplaceID(Item item, Item otherItem, StringBuilder value)
        {
            value.Replace(item.ID.ToString(), otherItem.ID.ToString());
        }

        private void ReplaceShortID(Item item, Item otherItem, StringBuilder value)
        {
            value.Replace(item.ID.ToShortID().ToString(), otherItem.ID.ToShortID().ToString());
        }

        private void ReplaceFullPath(Item item, Item otherItem, StringBuilder value)
        {
            value.Replace(item.Paths.FullPath, otherItem.Paths.FullPath);
        }

        private void ReplaceContentPath(Item item, Item otherItem, StringBuilder value)
        {
            if (item.Paths.IsContentItem)
                value.Replace(item.Paths.ContentPath, otherItem.Paths.ContentPath);
        }

        private void UpdateFieldValue(Field field, string initialValue, StringBuilder value)
        {
            if (initialValue.Equals(value.ToString()))
                return;

            using (new EditContext(field.Item))
            {
                field.Value = value.ToString();
            }
        }

        #endregion
    }
}