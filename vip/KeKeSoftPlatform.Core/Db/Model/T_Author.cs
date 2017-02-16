using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using KeKeSoftPlatform.Common;

namespace KeKeSoftPlatform.Core
{
    public class T_Author
    {
        public T_Author()
        {
            Id = PF.Key();
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public virtual ICollection<T_Book> Book { get; set; }
    }
}
