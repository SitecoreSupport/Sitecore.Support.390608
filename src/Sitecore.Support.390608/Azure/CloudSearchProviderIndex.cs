﻿namespace Sitecore.Support.ContentSearch.Azure
{
    using System;
    using Sitecore.Diagnostics;
    using Sitecore.ContentSearch;
    using Sitecore.ContentSearch.Maintenance;
    using Sitecore.ContentSearch.Security;
    using System.Reflection;
    using Sitecore.ContentSearch.Azure;

    public class CloudSearchProviderIndex : Sitecore.ContentSearch.Azure.CloudSearchProviderIndex
    {
        private static readonly MethodInfo EnsureInitializedMethodInfo =
          typeof(Sitecore.ContentSearch.Azure.CloudSearchProviderIndex).GetMethod("EnsureInitialized",
            BindingFlags.Instance | BindingFlags.NonPublic);
        public CloudSearchProviderIndex(string name, string connectionStringName, string totalParallelServices, IIndexPropertyStore propertyStore) : base(name, connectionStringName, totalParallelServices, propertyStore)
        {
        }

        public CloudSearchProviderIndex(string name, string connectionStringName, string totalParallelServices, IIndexPropertyStore propertyStore, string @group, ServiceCollectionClient serviceCollectionClient) : base(name, connectionStringName, totalParallelServices, propertyStore, @group, serviceCollectionClient)
        {
        }

        public override void Initialize()
        {
            Log.Warn(string.Format("Sitecore Support: Initializing index {0}", this.Name), this);
            try
            {
                base.Initialize();
            }
            catch (Exception e)
            {
                Log.Error(string.Format("Sitecore Support: Initializing index {0} failed", e), this);
                throw;
            }
            finally
            {
                Log.Warn(string.Format("Sitecore Support: Initializing index {0} completed", this.Name), this);
            }
        }

        public override IProviderSearchContext CreateSearchContext(SearchSecurityOptions options = SearchSecurityOptions.EnableSecurityCheck)
        {
            EnsureInitializedMethodInfo.Invoke(this, new object[0]);
            return new Sitecore.Support.ContentSearch.Azure.CloudSearchSearchContext(base.ServiceCollectionClient, options);
        }
    }
}