using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GRA.Data.Abstract
{
    public abstract class BaseDbEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
