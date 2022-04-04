using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Fields;
using Sitecore.Xml;
using System.Xml;

namespace Foundation.Search.ComputedFields
{
    public class BoolFieldComputedField : IComputedIndexField
    {
        public BoolFieldComputedField(XmlNode configurationNode)
        {
            ReturnType = XmlUtil.GetAttribute("returnType", configurationNode);
            FieldName = XmlUtil.GetAttribute("fieldName", configurationNode);
            SourceField = XmlUtil.GetAttribute("sourceField", configurationNode);
        }

        public string FieldName { get; set; }
        public string ReturnType { get; set; }
        public string SourceField { get; set; }

        public object ComputeFieldValue(IIndexable indexable)
        {
            var indexableItem = indexable as SitecoreIndexableItem;

            if (indexableItem == null) return null;

            if (string.IsNullOrEmpty(SourceField)) return null;

            var isChecked = (CheckboxField)indexableItem.Item.Fields[SourceField];

            if (isChecked.Checked)
            {
                return true;
            }

            else
            {
                return false;
            }

        }
    }
}