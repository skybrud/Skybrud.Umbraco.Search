using System;
using Newtonsoft.Json;
using Umbraco.Core.Models;

namespace Skybrud.Umbraco.Search.Models
{
    public partial class SiteSearchResult
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        //[JsonProperty("teaser")]
        //public string Teaser { get; set; }

        [JsonProperty("created")]
        public DateTime Created { get; set; }

        [JsonProperty("updated")]
        public DateTime Updated { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        //[JsonIgnore]
        //public bool HideInNavigation { get; set; }

	    public SiteSearchResult(IPublishedContent a)
	    {
		    Id = a.Id;
		    Name = a.Name;
		    //Teaser = a.HasValue("contentTeasertext") ? a.GetPropertyValue<string>("contentTeasertext") : "";
		    Created = a.CreateDate;
		    Updated = a.UpdateDate;
		    Url = a.Url;
		    //HideInNavigation = a.HasValue("umbracoNaviHide") && a.GetPropertyValue<bool>("umbracoNaviHide")
	    }

	    public static SiteSearchResult GetFromContent(IPublishedContent a)
	    {
		    return new SiteSearchResult(a);
        }
    }
}