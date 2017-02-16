using KeKeSoftPlatform.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeKeSoftPlatform.Core
{
    public enum ClassType
    {
        [EnumValue("文科")]
        WeiKe = 1,
        [EnumValue("理科")]
        LiKe,
        [EnumValue("未分")]
        UnKnown
    }

    /// <summary>
    /// 班级表
    /// </summary>
    public class T_Class
    {
        public T_Class()
        {
            Id = PF.Key();
        }

        [Key]
        public Guid Id { get; set; }

        [Column(TypeName = "varchar(max)")]
        public string Name { get; set; }
        public string Code { get; set; }
        public DateTime CreateDate { get; set; }
        public ClassType Type { get; set; }
        public string Description { get; set; }
        public virtual ICollection<T_Student> Student { get; set; }
    }
}
