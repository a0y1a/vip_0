using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeKeSoftPlatform.Common;

namespace KeKeSoftPlatform.Core
{

    public class Navigation
    {
        public const string TOP_KEY = "__top_key__";
        public const string ITEM_KEY = "__item_key__";

        public class NavigationTop
        {
            internal static int IdIndex = 0;

            private string _Name;
            public string Name { get { return _Name; } }

            public int Id { get; internal set; }

            private List<NavigationItem> _NavigationItemCollection;

            public NavigationTop(string name)
            {
                this._NavigationItemCollection = new List<NavigationItem>();
                this._Name = name;
                this.Id = ++IdIndex;
            }


            public NavigationTop Item(NavigationItem item)
            {
                if (item.Url.Contains("?"))
                {
                    item.Url += "&";
                }
                else
                {
                    item.Url += "?";
                }
                item.Url += "{0}={1}&{2}={3}".FormatString(TOP_KEY, Id, ITEM_KEY, item.Id);
                this._NavigationItemCollection.Add(item);
                return this;
            }

            public IEnumerable<NavigationItem> NavigationItemCollection()
            {
                return this._NavigationItemCollection.Select(m => m);
            }
        }

        public class NavigationItem
        {
            internal static int IdIndex = 0;

            private string _Name;
            public string Name { get { return _Name; } }

            public int Id { get; internal set; }

            private string _Url;
            public string Url { get { return _Url; } internal set { _Url = value; } }

            public Func<T_Admin, bool> Enable { get; set; }

            public NavigationItem(string name, string url, Func<T_Admin, bool> enable = null)
            {
                this._Name = name;
                this._Url = url;
                this.Enable = enable ?? (u => true);
                this.Id = ++IdIndex;
            }

        }

        private static Navigation _Instance;
        public static Navigation Instance { get { return _Instance; } }
        static Navigation()
        {
            _Instance = new Navigation();

            _Instance.Top(new NavigationTop("用户管理")
                            .Item(new NavigationItem("用户列表", "/system/UserList"))
                            .Item(new NavigationItem("消费记录", "/system/ConsumptionRecordList")));
        }

        private List<NavigationTop> _NavigationTopCollection;

        private Navigation()
        {
            this._NavigationTopCollection = new List<NavigationTop>();
        }

        private Navigation Top(NavigationTop top)
        {
            this._NavigationTopCollection.Add(top);
            return this;
        }

        public IEnumerable<NavigationTop> NavigationTopCollection()
        {
            return this._NavigationTopCollection.Select(m => m);
        }
    }
}
