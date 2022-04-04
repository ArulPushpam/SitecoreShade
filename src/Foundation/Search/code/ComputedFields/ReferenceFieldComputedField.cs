using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Fields;
using Sitecore.Xml;
using System;
using System.Linq;
using System.Xml;

namespace Foundation.Search.ComputedFields
{
    public class ReferenceFieldComputedField : IComputedIndexField
    {
        public ReferenceFieldComputedField(XmlNode configurationNode)
        {
            ReturnType = XmlUtil.GetAttribute("returnType", configurationNode);
            FieldName = XmlUtil.GetAttribute("fieldName", configurationNode);
            SourceField = XmlUtil.GetAttribute("sourceField", configurationNode);
            ReferenceField = XmlUtil.GetAttribute("referenceField", configurationNode);
        }

        public string FieldName { get; set; }
        public string ReturnType { get; set; }
        public string SourceField { get; set; }
        public string ReferenceField { get; set; }

        public object ComputeFieldValue(IIndexable indexable)
        {
            var indexableItem = indexable as SitecoreIndexableItem;
            if (indexableItem == null) return null;
            if (string.IsNullOrEmpty(SourceField)) return null;
            var field = (ReferenceField)indexableItem.Item.Fields[SourceField];
            if (field?.TargetItem == null) return null;
            var result = field.TargetItem[ReferenceField];
            return result;
        }
    }
}