using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using KeKeSoftPlatform.Common;
using System.Threading.Tasks;

namespace KeKeSoftPlatform.Core
{
    public enum NewsType
    {
        [EnumValue("体育")]
        Sport = 1,
        [EnumValue("音乐")]
        Music,
        [EnumValue("艺术")]
        Art
    }
    public class T_News
    {
        public T_News()
        {
            Id = PF.Key();
        }

        [Key]
        public Guid Id { get; set; }

        public string Content { get; set; }

        public NewsType Type { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? LastModifyDate { get; set; }
    }
}
