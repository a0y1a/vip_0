using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeKeSoftPlatform.Common;

namespace KeKeSoftPlatform.Core
{
    public class CurrencyHandler
    {
        public class CurrencyItem
        {
            private int _Id;
            public int Id { get { return this._Id; } }

            private string _Name;
            public string Name { get { return this._Name; } }

            public CurrencyItem(int id,string name)
            {
                this._Id = id;
                this._Name = name;

                //注册到LevelHandler集合中，供外部访问
                CurrencyHandler.Register(this);
            }
        }

        private static List<CurrencyItem> _Data;

        private static void Register(CurrencyItem item)
        {
            if (_Data == null)
            {
                _Data = new List<CurrencyItem>();
            }
            _Data.Add(item);
        }

        public static List<CurrencyItem> Collection
        {
            get { return _Data.OrderBy(m => m.Id).ToList(); }
        }

        public static CurrencyItem Element(int id)
        {
            var result = _Data.FirstOrDefault(m => m.Id == id);
            if (result == null)
            {
                throw new Exception("没有找到该货币：{0}".FormatString(id));
            }
            return result;
        }

        /// <summary>
        /// 储值
        /// </summary>
        public static CurrencyItem ChuZhi = new CurrencyItem(1, "储值");
        /// <summary>
        /// 积分
        /// </summary>
        public static CurrencyItem JiFen = new CurrencyItem(2, "积分");
        
    }
}
