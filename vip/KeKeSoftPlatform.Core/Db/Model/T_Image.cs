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
    /// <summary>
    /// 图片表
    /// </summary>
    public class T_Image
    {
        public T_Image()
        {
            Id = PF.Key();
            CreateDate = DateTime.Now;
        }

        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// 图片名称
        /// </summary>
        [Column(TypeName = "varchar")]
        public string Name { get; set; }

        /// <summary>
        /// 图片保存路径
        /// </summary>
        [Column(TypeName = "varchar")]
        public string Url { get; set; }

        /// <summary>
        /// 图片二进制形式
        /// </summary>
        [Column(TypeName = "image")]
        public byte[] ImageByte { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
