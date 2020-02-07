using Sitecore.ContentSearch.Azure.Http;
using Sitecore.ContentSearch.Azure.Query;
using Sitecore.ContentSearch.Azure.Schema;
using Sitecore.ContentSearch.Linq.Common;
using Sitecore.ContentSearch.Linq.Helpers;
using Sitecore.ContentSearch.Linq.Nodes;
using Sitecore.ContentSearch.Linq.Parsing;
using System;
using System.Reflection;

namespace Sitecore.Support.ContentSearch.Azure.Query
{
    public class CloudQueryMapper : Sitecore.ContentSearch.Azure.Query.CloudQueryMapper
    {
        public CloudQueryMapper(CloudIndexParameters parameters) : base(parameters)
        {

        }

        public override CloudQuery MapQuery(IndexQuery query)
        {
            var mappingState = new CloudQueryMapperState();
            var nativeQuery = this.HandleCloudQuery(query.RootNode, mappingState);

            //// If the returned query equal to wildcard string only, the wildcard expression hasn't been constructed yet.
            //// Hence, fire the wildcard construction

            if (nativeQuery == null)
            {
                nativeQuery = string.Empty;
            }

            if (string.IsNullOrEmpty(nativeQuery) && mappingState.AdditionalQueryMethods.Count == 0 && mappingState.FacetQueries.Count == 0)
            {
                nativeQuery = "&search=*";
            }

            // beginfix 

            string[] SearchAndFilter = nativeQuery.Split(new string[] { "&$filter=" }, StringSplitOptions.None);
            if (SearchAndFilter.Length == 2)
            {
                SearchAndFilter[1] = SearchAndFilter[1].Replace("\\'", "''");
                nativeQuery = SearchAndFilter[0] + "&$filter=" + SearchAndFilter[1];
            }

            //endfix

            return new CloudQuery(nativeQuery, mappingState.AdditionalQueryMethods, mappingState.FacetQueries, mappingState.VirtualFieldProcessors, this.Parameters.ExecutionContexts);
        }
    }
}