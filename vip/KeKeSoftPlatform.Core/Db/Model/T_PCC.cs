using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeKeSoftPlatform.Core
{
    public class T_PCC
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Int64 Id { get; set; }

        public Int64? ParentId { get; set; }
        public virtual T_PCC Parent { get; set; }

        [Required]
        [StringLength(500)]
        [Column(TypeName = "varchar")]
        public string Name { get; set; }
        public virtual ICollection<T_PCC> Children { get; set; }
    }
}
