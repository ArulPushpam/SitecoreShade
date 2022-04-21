using Sitecore;
using Sitecore.ContentSearch.Utilities;
using Sitecore.Diagnostics;
using Sitecore.XA.Foundation.Search.Attributes;
using Sitecore.XA.Foundation.Search.Pipelines.ResolveSearchQueryTokens;
using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Foundation.Search.Pipelines
{
    public class ExcludePastEvents : ResolveSearchQueryTokensProcessor
    {
        protected string TokenPart { get; } = "ExcludeItemWithPreviousDateInDateField";
        private readonly string DateCompareIdentifier = "#datecompare#";

        [SxaTokenKey]
        protected override string TokenKey => FormattableString.Invariant(FormattableStringFactory.Create("{0}|FieldName", (object)this.TokenPart));

        public override void Process(ResolveSearchQueryTokensEventArgs args)
        {
            if (args.ContextItem == null)
            {
                return;
            }
            for (int index = 0; index < args.Models.Count; index++)
            {
                SearchStringModel model = args.Models[index];
                if (model.Type.Equals("sxa") && ContainsToken(model))
                {
                    string fieldName = model.Value.Replace(TokenPart, string.Empty).TrimStart('|');
                    args.Models.Insert(index, this.BuildModel(fieldName));
                    args.Models.Remove(model);
                }
            }
        }
        protected virtual SearchStringModel BuildModel(string fieldName)
        {
            try
            {
                Assert.ArgumentNotNull(fieldName, "fieldName");

                string fieldFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'";
                DateTime dateTime = DateUtil.ToUniversalTime(DateTime.UtcNow);

                string formatedDate = string.Format(arg2: "*", format: "{0}[{1} TO {2}]", arg1: dateTime.ToString(fieldFormat), arg0: DateCompareIdentifier);

                return new SearchStringModel("custom", FormattableString.Invariant(FormattableStringFactory.Create("{0}|{1}", (object)fieldName.ToLowerInvariant(), formatedDate)))
                {
                    Operation = "must"
                };
            }
            catch (Exception ex)
            {
                Log.Info(string.Format("BuildModel - error-{0}stacktrace{1}", ex.Message.ToString(), ex.StackTrace.ToString()), ex);
            }
            return new SearchStringModel();

        }

        protected override bool ContainsToken(SearchStringModel m)
        {
            return Regex.Match(m.Value, FormattableString.Invariant($"{TokenPart}\\|[a-zA-Z ]*")).Success;
        }
    }
}