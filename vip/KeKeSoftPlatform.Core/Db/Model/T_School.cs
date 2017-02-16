using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using KeKeSoftPlatform.Common;

namespace KeKeSoftPlatform.Core
{
    public class T_School
    {
        public T_School()
        {
            Id = PF.Key();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        [Column(TypeName = "varchar")]
        [StringLength(50)]
        [Index(IsUnique = true)]
        public string Code { get; set; }

        [Required]
        [Column(TypeName = "varchar(max)")]
        public string Pwd { get; set; }

        public string Name { get; set; }
        public DateTime CreateDate { get; set; }

    }
}
