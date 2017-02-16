using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using KeKeSoftPlatform.Common;

namespace KeKeSoftPlatform.Core
{
    public class T_Test
    {
        public T_Test()
        {
            Id = PF.Key();
        }
        public Guid Id { get; set; }

        public string ImgUrl { get; set; }
    }
}
