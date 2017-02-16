using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using KeKeSoftPlatform.Common;

namespace KeKeSoftPlatform.Core
{
    public class T_Book
    {
        public T_Book()
        {
            Id = PF.Key();
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Page { get; set; }
        public decimal Price { get; set; }
        public Guid AuthorId { get; set; }
        public virtual T_Author Author { get; set; }
    }
}
