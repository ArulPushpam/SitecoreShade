using Scriban.Runtime;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.XA.Foundation.Scriban.Pipelines.GenerateScribanContext;
using Sitecore.XA.Foundation.Search.Models;
using Sitecore.XA.Foundation.Search.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Search.Pipelines
{
    public class GetSearchItem : IGenerateScribanContextProcessor
    {
        protected ISearchService SearchService { get; }
        protected ISortingService SortingService { get; }

        public GetSearchItem(ISearchService searchService, ISortingService sortingService)
        {
            SearchService = searchService;
            SortingService = sortingService;
        }
        private delegate IEnumerable<Item> SearchItemsDelegate(string s, int p = 0, int o = 0, string of = null, string f = null);

        /// <summary>
        /// Search API
        /// </summary>
        /// <param name="s">ScopeID</param>
        /// <param name="p">NumberOfResults</param>
        /// <param name="o">Order</param>
        /// <param name="of">OrderBy FieldName</param>
        /// <param name="f">FilterBy FieldName</param>
        /// <returns>Result Items</returns>
        public IEnumerable<Item> SearchItemsImpl(string s, int p = 10, int o = 0, string of = null, string f = null)
        {
            try
            {
                var scopesIDs = s?.Split(',', '|').Where(ID.IsID).Select(ID.Parse);

                var query = this.SearchService.GetQuery(new SearchQueryModel
                {
                    ScopesIDs = scopesIDs,
                }, out var indexName);

                //if o=1 then ascending
                if (o == 1)
                    query = query.OrderBy((ContentPage i) => i.get_Item<double>(of));
                else
                    query = query.OrderByDescending((ContentPage i) => i.get_Item<double>(of));

                //filter by passed date
                if (!string.IsNullOrEmpty(f))
                    query = query.Where((ContentPage i) => i.get_Item<DateTime>(f) >= DateTime.Now);

                if (p > 0)
                {
                    query = query.Take(p);
                }

                return query.Select(i => i.Uri).AsEnumerable()
                    .Select(u => Factory.GetDatabase(u.DatabaseName)
                        .GetItem(u.ItemID, u.Language, u.Version));
            }
            catch (Exception ex)
            {
                Log.Info(string.Format("SearchItemsImpl-error-{0}stacktrace{1}", ex.Message.ToString(), ex.StackTrace.ToString()), ex);
            }
            return new List<Item>();
        }

        public void Process(GenerateScribanContextPipelineArgs args)
        {
            args.GlobalScriptObject.Import("sc_searchitems", new SearchItemsDelegate(SearchItemsImpl));
        }
    }
}