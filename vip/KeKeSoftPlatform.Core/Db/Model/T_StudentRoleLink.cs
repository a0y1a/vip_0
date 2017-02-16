using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KeKeSoftPlatform.Common;

namespace KeKeSoftPlatform.Core
{
    public class T_StudentRoleLink
    {
        public T_StudentRoleLink()
        {
            Id = PF.Key();
        }

        [Key]
        public Guid Id { get; set; }

        public Guid StudentId { get; set; }
        public virtual T_Student Student { get; set; }

        public Guid RoleId { get; set; }
        public virtual T_Role Role { get; set; }
    }
}
