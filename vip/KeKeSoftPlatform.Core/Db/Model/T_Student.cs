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
    public enum StudentStatus
    {
        [EnumValue("未审核")]
        Checking = 1,
        [EnumValue("已审核")]
        Checked
    }
    public enum Sex 
    {
        [EnumValue("男")]
        Male = 1,
        [EnumValue("女")]
        Female
    }

    /// <summary>
    /// 学生表
    /// </summary>
    public class T_Student
    {
        public T_Student()
        {
            Id = PF.Key();
            CreateDate = DateTime.Now;
            Status = StudentStatus.Checking;
        }

        [Key]
        public Guid Id { get; set; }

        [Column(TypeName = "varchar(max)")]
        public string  Name { get; set; }
        public Sex Sex { get; set; }
        public int Age { get; set; }

        public StudentStatus Status { get; set; }

        /// <summary>
        /// 是否住校生
        /// </summary>
        public bool IsZhuXiao { get; set; }
        public string Mobile { get; set; }
        public DateTime Birthday { get; set; }
        public string Code { get; set; }
        public string Password { get; set; }

        /// <summary>
        /// 缴费
        /// </summary>
        public decimal Amount { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid ClassId { get; set; }
        public virtual T_Class Class { get; set; }
        public virtual ICollection<T_StudentRoleLink> StudentRoleLink { get; set; }
    }
}
