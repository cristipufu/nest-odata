﻿using Nest.OData.Tests.Common;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Nest.OData.Tests
{
    public class OrderByTests
    {
        [Fact]
        public void SkipTopOrderByDesc()
        {
            var queryOptions = "$skip=10&$top=20&$orderby=CreatedDate desc".GetODataQueryOptions<Product>();

            var elasticQuery = queryOptions.ToElasticQuery();

            Assert.NotNull(elasticQuery);

            var queryJson = elasticQuery.ToJson();

            var expectedJson = @"
            {
              ""from"": 10,
              ""size"": 20,
              ""sort"": [
                {
                  ""CreatedDate"": {
                    ""order"": ""desc""
                  }
                }
              ]
            }";

            var actualJObject = JObject.Parse(queryJson);
            var expectedJObject = JObject.Parse(expectedJson);

            Assert.True(JToken.DeepEquals(expectedJObject, actualJObject), "Expected and actual JSON do not match.");
        }

        [Fact]
        public void MultipleOrderBy()
        {
            var queryOptions = "$orderby=CreatedDate desc,Category".GetODataQueryOptions<Product>();

            var elasticQuery = queryOptions.ToElasticQuery();

            Assert.NotNull(elasticQuery);

            var queryJson = elasticQuery.ToJson();

            var expectedJson = @"
            {
              ""sort"": [
                {""CreatedDate"": {""order"": ""desc""}},
                {""Category"": {""order"": ""asc""}}
              ]
            }";

            var actualJObject = JObject.Parse(queryJson);
            var expectedJObject = JObject.Parse(expectedJson);

            Assert.True(JToken.DeepEquals(expectedJObject, actualJObject), "Expected and actual JSON do not match.");
        }

        [Fact]
        public void OrderByNested()
        {
            var queryOptions = "$orderby=ProductDetail/Id desc".GetODataQueryOptions<Product>();

            var elasticQuery = queryOptions.ToElasticQuery();

            Assert.NotNull(elasticQuery);

            var queryJson = elasticQuery.ToJson();

            var expectedJson = @"
            {
              ""sort"": [
                {
                  ""ProductDetail.Id"": {
                    ""nested"": {
                      ""path"": ""ProductDetail""
                    },
                    ""order"": ""desc""
                  }
                }
              ]
            }";

            var actualJObject = JObject.Parse(queryJson);
            var expectedJObject = JObject.Parse(expectedJson);

            Assert.True(JToken.DeepEquals(expectedJObject, actualJObject), "Expected and actual JSON do not match.");
        }

        [Fact]
        public void MultipleNestedOrderBy()
        {
            var queryOptions = "$orderby=ProductDetail/Info desc,ProductFeature/Id".GetODataQueryOptions<Product>();

            var elasticQuery = queryOptions.ToElasticQuery();

            Assert.NotNull(elasticQuery);

            var queryJson = elasticQuery.ToJson();

            var expectedJson = @"
            {
              ""sort"": [
                {
                  ""ProductDetail.Info"": {
                    ""nested"": {
                      ""path"": ""ProductDetail""
                    },
                    ""order"": ""desc""
                  }
                },
                {
                  ""ProductFeature.Id"": {
                    ""nested"": {
                      ""path"": ""ProductFeature""
                    },
                    ""order"": ""asc""
                  }
                }
              ]
            }";

            var actualJObject = JObject.Parse(queryJson);
            var expectedJObject = JObject.Parse(expectedJson);

            Assert.True(JToken.DeepEquals(expectedJObject, actualJObject), "Expected and actual JSON do not match.");
        }

        [Fact]
        public void OrderByKeyword()
        {
            var queryOptions = "$orderby=Key desc".GetODataQueryOptions<Product>();

            var elasticQuery = queryOptions.ToElasticQuery();

            Assert.NotNull(elasticQuery);

            var queryJson = elasticQuery.ToJson();

            var expectedJson = @"
            {
              ""sort"": [
                {
                  ""Key.keyword"": {
                    ""order"": ""desc""
                  }
                }
              ]
            }";

            var actualJObject = JObject.Parse(queryJson);
            var expectedJObject = JObject.Parse(expectedJson);

            Assert.True(JToken.DeepEquals(expectedJObject, actualJObject), "Expected and actual JSON do not match.");
        }
    }
}
