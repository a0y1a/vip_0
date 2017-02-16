using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace KeKeSoftPlatform.Common
{
    public class ZTreeNode
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("isParent")]
        public bool IsParent { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        public ZTreeNode()
        {
            IsParent = true;
        }
    }
}
