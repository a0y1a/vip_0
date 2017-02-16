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
    public enum FileType
    {
        [EnumValue("Uploadify")]
        Uploadify = 1,
        [EnumValue("Uploadifive")]
        Uploadifive,
        [EnumValue("Weui")]
        Weui
    }

    public enum SortType
    {
        [EnumValue("First")]
        First = 1,
        [EnumValue("Second")]
        Second,
        [EnumValue("Third")]
        Third,
        [EnumValue("Forth")]
        Forth,
        [EnumValue("Fifth")]
        Fifth,
        [EnumValue("Sixth")]
        Sixth,
        [EnumValue("Seventh")]
        Seventh,
        [EnumValue("Eighth")]
        Eighth,
        [EnumValue("Nineth")]
        Nineth,
        [EnumValue("Tenth")]
        Tenth
    }

    /// <summary>
    /// 文件表
    /// </summary>
    public class T_File
    {
        public T_File()
        {
            Id = PF.Key();
        }

        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// 关联其它表的Id
        /// </summary>
        public Guid? LinkId { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        [Column(TypeName = "varchar")]
        public string Name { get; set; }

        /// <summary>
        /// 文件保存路径
        /// </summary>
        [Column(TypeName = "varchar")]
        public string Url { get; set; }

        /// <summary>
        /// 文件描述
        /// </summary>
        public string MiaoShu { get; set; }

        public DateTime CreateDate { get; set; }
        public FileType FileType { get; set; }
        public SortType SortType { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; }
    }
}
