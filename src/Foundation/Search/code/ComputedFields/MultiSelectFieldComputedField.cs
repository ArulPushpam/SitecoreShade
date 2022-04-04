using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Fields;
using Sitecore.Xml;
using System.Linq;
using System.Xml;
namespace Foundation.Search.ComputedFields
{
    public class MultiSelectFieldComputedField : IComputedIndexField
    {
        public MultiSelectFieldComputedField(XmlNode configurationNode)
        {
            FieldName = XmlUtil.GetAttribute("fieldName", configurationNode);
            SourceField = XmlUtil.GetAttribute("sourceField", configurationNode);
            ReferenceField = XmlUtil.GetAttribute("referenceField", configurationNode);
            ReturnType = XmlUtil.GetAttribute("returnType", configurationNode);
        }

        public string FieldName { get; set; }
        public string ReturnType { get; set; }
        public string SourceField { get; set; }
        public string ReferenceField { get; set; }

        public object ComputeFieldValue(IIndexable indexable)
        {
            var indexableItem = indexable as SitecoreIndexableItem;
            if (indexableItem == null) return null;
            var field = (MultilistField)indexableItem.Item.Fields[SourceField];
            if (field == null) return null;
            else
            {
                var multilist = field.GetItems();
                if (multilist == null || multilist.Length == 0)
                    return null;
                else
                    return multilist.Select(t => t[ReferenceField]);
            }

        }
    }
}