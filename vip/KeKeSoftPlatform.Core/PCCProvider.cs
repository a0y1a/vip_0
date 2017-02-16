using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeKeSoftPlatform.Core
{
    public class PCCProvider
    {
        public const Int64 DEFAULT_PROVINCE = 11;
        public const Int64 DEFAULT_CITY = 1101;
        public const Int64 DEFAULT_COUNTY = 110101;

        private static Dictionary<Int64, PCCItemData> collection;
        public static Dictionary<Int64, PCCItemData> Collection { get { return collection; } }
        public static List<PCCItemData> GetAllProvince(Int64 selectedProvince = DEFAULT_PROVINCE)
        {
            return collection.Where(m => m.Value.ParentId == null).Select(m => m.Value).ToList();
        }

        public static string GetAreaName(Int64 areaId)
        {
            return collection[areaId].Name;
        }
        public static List<PCCItemData> GetPCCItems(Int64? parentId)
        {
            return collection.Where(m => m.Value.ParentId == parentId).Select(m => m.Value).ToList();
        }

        static PCCProvider()
        {
            collection = new Dictionary<Int64, PCCItemData>();
            using (KeKeSoftPlatformDbContext db = new KeKeSoftPlatformDbContext())
            {
                collection = db.PCC.Select(m => new PCCItemData { Id = m.Id, Name = m.Name, ParentId = m.ParentId }).ToDictionary(m => m.Id);
            }
        }
    }
}
