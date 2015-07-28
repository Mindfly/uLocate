namespace uLocate.Search
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;

    using Examine;

    using Lucene.Net.QueryParsers;

    using Umbraco.Core;

    /// <summary>
    /// This class constructs the actual lucene query from a bunch of 
    /// parameters and returns umbraco examine ISearchResults
    /// </summary>
    public class ExamineSearch
    {
        /// <summary>
        /// There's a lot of parameters, this is the container object
        /// </summary>
        protected SearchParameters Parameters;
        
        const string IndexTypePropertyName = "__IndexType";

        public ExamineSearch(SearchParameters parameters)
        {
            this.Parameters = parameters;
        }

        /// <summary>
        /// "multi relevance" search does the following... roughly
        /// The index is searched for, in order of decreasing relevance
        /// 1) the exact phrase entered in any of the title properties
        /// 2) any of the terms entered in any of the title properties
        /// 3) a fuzzy match for any of the terms entered in any of the title properties
        /// 4) the exact phrase entered in any of the body properties
        /// 5) any of the terms entered in any of the body properties
        /// 6) a fuzzy match for any of the terms entered in any of the body properties
        /// </summary>
        /// <returns></returns>
        public ISearchResults ResultsMultiRelevance()
        {
            var query = new StringBuilder();
            // We formulate the query differently depending on the input.
            if (this.Parameters.SearchTerm.Contains('"'))
            {
                // If the user has enetered double quotes we don't bother 
                // searching for the full string
                query.Append(this.QueryAllPropertiesOr(SearchUtilities.GetSearchTermsSplit(this.Parameters.SearchTerm), 1));
            }
            else if (!this.Parameters.SearchTerm.Contains('"') && !this.Parameters.SearchTerm.Contains(' '))
            {
                // if there's no spaces or quotes we don't need to get the quoted term and boost it
                query.Append(this.QueryAllPropertiesOr(SearchUtilities.GetSearchTermsSplit(this.Parameters.SearchTerm), 1));
            }
            else
            {
                // otherwise we search first for the entire query in quotes, 
                // then for each term in the query OR'd together.
                query.AppendFormat("({0} OR {1})",
                    this.QueryAllPropertiesOr(SearchUtilities.GetSearchTermQuoted(this.Parameters.SearchTerm), 2)
                    , this.QueryAllPropertiesOr(SearchUtilities.GetSearchTermsSplit(this.Parameters.SearchTerm), 1)
                );
            }

            return this.ExecuteSearch(this.WrapQuery(query));
        }

        /// <summary>
        /// Execute a search requiring all terms in the query to be present, not necessarily in 
        /// the order entered, though that will boost the relevance. Pretty much the same
        /// as SearchMultiRelevance, except terms are AND'd rather than OR'd 
        /// </summary>
        /// <returns></returns>
        public ISearchResults ResultsMultiAnd()
        {
            var query = new StringBuilder();

            if (this.Parameters.SearchTerm.Contains('"'))
            {
                // If the user has entered double quotes we don't bother 
                // searching for the full string
                query.Append(this.QueryAllPropertiesAnd(SearchUtilities.GetSearchTermsSplit(this.Parameters.SearchTerm), 1.0));
            }
            else if (!this.Parameters.SearchTerm.Contains('"') && !this.Parameters.SearchTerm.Contains(' '))
            {
                // if there's no spaces or quotes we don't need to get the quoted term and boost it
                query.Append(this.QueryAllPropertiesAnd(SearchUtilities.GetSearchTermsSplit(this.Parameters.SearchTerm), 1));
            }
            else
            {
                // otherwise we search first for the entire query in quotes, 
                // then for each term in the query OR'd together.
                query.AppendFormat("{0} OR {1}",
                    this.QueryAllPropertiesAnd(SearchUtilities.GetSearchTermQuoted(this.Parameters.SearchTerm), 2)
                    , this.QueryAllPropertiesAnd(SearchUtilities.GetSearchTermsSplit(this.Parameters.SearchTerm), 1)
                );
            }

            return this.ExecuteSearch(this.WrapQuery(query));
        }

        /// <summary>
        /// Simple search for any term in the query. Make this simpler so it executes faster
        /// </summary>
        /// <returns></returns>
        public ISearchResults ResultsSimpleOr()
        {
            var query = new StringBuilder();
            query.Append(this.QueryAllProperties(SearchUtilities.GetSearchTermsSplit(this.Parameters.SearchTerm),1.0,"OR",true));
            return this.ExecuteSearch(this.WrapQuery(query));
        }
        /// <summary>
        /// Run a quoted query 
        /// </summary>
        /// <returns></returns>
        public ISearchResults ResultsAsEntered()
        {
            var query = new StringBuilder();
            query.Append(this.QueryAllPropertiesAnd(SearchUtilities.GetSearchTermQuoted(this.Parameters.SearchTerm), 1.0));
            return this.ExecuteSearch(this.WrapQuery(query));
        }

        /// <summary>
        /// Pre-pend the query with the necessary queries for index type and LocationType
        /// </summary>
        /// <param name="toWrap">Query to wrap</param>
        /// <returns>query</returns>
        protected StringBuilder WrapQuery(StringBuilder toWrap)
        {
            var query = new StringBuilder();
            // first add the required index type to the query
            var indexTypesQuery = this.QueryIndexTypes();
            if (indexTypesQuery != null)
                query.AppendFormat("{0} AND ", indexTypesQuery );
            
            //TODO: Update to use LocationType rather than root node
            // now check the node has a parent in the supplied root node list, blank means search all 
            //var rootNodeQuery = this.QueryRootNodes();
            var rootNodeQuery = "";

            if (rootNodeQuery != null && rootNodeQuery.Length > 0)
            {
                query.AppendFormat("{0} AND (", rootNodeQuery);
            }
            else
            {
                query.Append("(");
            }

            query.Append(toWrap);
            query.Append(")");
            return query;
        }

        /// <summary>
        /// Get the lucene query to pick out only nodes that are a matching location type
        /// </summary>
        /// <returns></returns>
        protected StringBuilder QueryLocationTypes()
        {
            //None specified, so assume all
            if (this.Parameters.LocationTypes == null || this.Parameters.LocationTypes.Count < 1)
            {
                return null;
            }

            //Otherwise, construct query addition
            var query = new StringBuilder();
            
            //Determine whether LocationType Names or Keys have been entered
            string testLt = this.Parameters.LocationTypes[0];
            bool isKey = testLt.IsGuid(true);
            string fieldName = isKey ? "LocationTypeKey" : "LocationTypeName";
            
            query.Append("+(");
            var i = 0;
            
            foreach (var item in this.Parameters.LocationTypes)
            {
                if (i++ > 0)
                    query.Append(" OR ");
                query.AppendFormat("{0}:{1}", fieldName, item);
            }

            query.Append(")");

            return query;
        }

        protected StringBuilder QueryIndexTypes()
        {
            if (this.Parameters.IndexTypes == null || this.Parameters.IndexTypes.Count < 1)
                return null;
            var query = new StringBuilder();

            query.Append("+(");
            var i = 0;
            foreach (var indexType in this.Parameters.IndexTypes)
            {
                if (i++ > 0)
                    query.Append(" OR ");
                query.AppendFormat("{0}:\"{1}\"", IndexTypePropertyName, QueryParser.Escape(indexType));
            }
            query.Append(")");
            return query;
        }

        /// <summary>
        /// OR's together all the passed search terms into a query
        /// for each property in the properties list
        /// </summary>
        /// <param name="searchTerms">A list of fully escaped search terms</param>
        /// <param name="boostAll">all terms are boosted by this amount, multiplied by the amount in the property/boost dictionary</param>
        /// <returns>a query fragment</returns>
        protected StringBuilder QueryAllPropertiesOr(ICollection<string> searchTerms, double boostAll)
        {
            if (searchTerms == null || searchTerms.Count < 1)
                return new StringBuilder();

            return this.QueryAllProperties(searchTerms, boostAll, "OR");
        }

        /// <summary>
        /// AND's together all the passed search terms into a query
        /// for each property in the properties list
        /// </summary>
        /// <param name="searchTerms">A list of fully escaped search terms</param>
        /// <param name="boostAll">all terms are boosted by this amount, multiplied by the amount in the property/boost dictionary</param>
        /// <returns>a query fragment</returns>
        protected StringBuilder QueryAllPropertiesAnd(ICollection<string> searchTerms, double boostAll)
        {
            if (searchTerms == null || searchTerms.Count < 1)
                return new StringBuilder();

            return this.QueryAllProperties(searchTerms, boostAll, "AND");
        }

        /// <summary>
        /// Called by queryAllPropertiesOr, queryAllPropertiesAnd
        /// Creates a somewhat convoluted lucene query string.
        /// Each search term is applied to each property in the umbracoProperties list, 
        /// boosted by the boost value associated with the property, multiplied by
        /// the boost value passed to the function. 
        /// The global fuzziness level is applied, multiplied by the fuzziness value 
        /// associated with the relevant property.
        /// Terms are ether OR'd or AND'd (or theoretically anything else
        /// you stick into joinWith'd, though I can't think of much that would 
        /// actually be useful) according to the contents of joinWith
        /// </summary>
        /// <param name="searchTerms">A list of fully escaped search terms</param>
        /// <param name="boostAll">Boost all terms by this amount</param>
        /// <param name="joinWith">Join terms with this string, should be AND/OR</param>
        /// <param name="simplify"></param>
        /// <returns></returns>
        protected StringBuilder QueryAllProperties(ICollection<string> searchTerms, double boostAll, string joinWith, bool simplify = false)
        {
            var queryBuilder = new List<StringBuilder>();
            foreach (var term in searchTerms)
            {
                var termQuery = new StringBuilder();
                foreach (var property in this.Parameters.SearchProperties)
                {
                    termQuery.Append(simplify
                                         ? this.QuerySingleItemSimple(term, property)
                                         : this.QuerySingleItem(term, property, boostAll));
                }
                if (termQuery.Length > 0)
                    queryBuilder.Add(termQuery);
            }
            var query = new StringBuilder();
            var count = queryBuilder.Count;
            if (count < 1)
                return query;
            var i = 0;
            for (; ; )
            {
                query.AppendFormat(" ({0}) ", queryBuilder[i]);
                if (++i >= count)
                    break;
                query.AppendFormat("{0} ", joinWith);
            }
            return query;
        }

        protected string QuerySingleItem(string term, SearchProperty property, double boostAll)
        {
            var boost = property.BoostMultiplier * boostAll;
            var boostString = string.Empty;
            if (boost != 1.0)
            {
                boostString = "^" + boost;
            }
            var fuzzyString = string.Empty;
            var wildcardQuery = string.Empty;
            if (!term.Contains('"'))
            {
                // wildcard queries get lower relevance than exact matches, and ignore fuzzieness
                if (property.Wildcard)
                {
                    wildcardQuery = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}:{1}*^{2} ", property.PropertyName, term, boost * 0.5);
                }
                else
                {
                    double fuzzyLocal = property.FuzzyMultiplier;
                    if (fuzzyLocal < 1.0 && fuzzyLocal > 0.0)
                    {
                        fuzzyString = "~" + fuzzyLocal;
                    }
                }
            }

            return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}:{1}{2}{3} {4}", property.PropertyName, term, fuzzyString, boostString, wildcardQuery);
        }

        protected string QuerySingleItemSimple(string term, SearchProperty property)
        {
            var fuzzyString = string.Empty;
            var wildcard = string.Empty;
            if (!term.Contains('"'))
            {
                if (property.Wildcard)
                {
                    wildcard = "*";
                }
                else
                {
                    var fuzzyLocal = property.FuzzyMultiplier;
                    if (fuzzyLocal < 1.0 && fuzzyLocal > 0.0)
                    {
                        fuzzyString = "~" + fuzzyLocal;
                    }
                }
            }

            return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}:{1}{2}{3} ", property.PropertyName, term, fuzzyString, wildcard);
        }

        /// <summary>
        /// Call Examine to execute the generated query
        /// </summary> 
        /// <param name="query">query to execute</param>
        /// <returns>ISearchResults object or null</returns>
        protected ISearchResults ExecuteSearch(StringBuilder query)
        {
            var provider = ExamineManager.Instance.SearchProviderCollection[this.Parameters.SearchProvider];
            if (provider == null)
                throw new ArgumentException("Supplied search provider not found. Check FullTextSearch.config");
            var filter = provider.CreateSearchCriteria().RawQuery(query.ToString());
            return filter != null ? provider.Search(filter) : null;
        }
    }
}