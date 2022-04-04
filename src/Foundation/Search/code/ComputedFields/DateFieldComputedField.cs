using Sitecore;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Fields;
using Sitecore.Xml;
using System;
using System.Xml;

namespace Foundation.Search.ComputedFields
{
    public class DateFieldComputedField : IComputedIndexField
    {
        public DateFieldComputedField(XmlNode configurationNode)
        {
            ReturnType = XmlUtil.GetAttribute("returnType", configurationNode);
            FieldName = XmlUtil.GetAttribute("fieldName", configurationNode);
            SourceField = XmlUtil.GetAttribute("sourceField", configurationNode);
            StringFormat = XmlUtil.GetAttribute("stringFormat", configurationNode);
        }

        public string FieldName { get; set; }
        public string ReturnType { get; set; }
        public string SourceField { get; set; }
        public string StringFormat { get; set; }
        public object ComputeFieldValue(IIndexable indexable)
        {
            var indexableItem = indexable as SitecoreIndexableItem;
            if (indexableItem == null) return null;
            if (string.IsNullOrEmpty(SourceField)) return null;
            var field = indexableItem.Item.Fields[SourceField];
            if (field == null) return null;
            var result = Getvalue(field);

            if (string.IsNullOrEmpty(StringFormat))
                return result;

            return result.ToString(StringFormat);

        }

        private DateTime Getvalue(Field fieldValue)
        {
            DateTime serverTime = DateUtil.IsoDateToDateTime(fieldValue.Value, DateTime.MinValue);
            return DateUtil.ToUniversalTime(serverTime);
        }
    }
}