using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using KeKeSoftPlatform.Common;

namespace KeKeSoftPlatform.Core
{
    public class T_Role
    {
        public T_Role()
        {
            Id = PF.Key();
            CreateDate = DateTime.Now;
        }

        [Key]
        public Guid Id { get; set; }

        [StringLength(50)]
        [Column(TypeName="varchar")]
        [Required]
        public string RoleName { get; set; }

        [Column(TypeName = "varchar(max)")]
        public string Description { get; set; }

        public DateTime CreateDate { get; set; }

        public virtual ICollection<T_StudentRoleLink> StudentRoleLink { get; set; }
    }
}
