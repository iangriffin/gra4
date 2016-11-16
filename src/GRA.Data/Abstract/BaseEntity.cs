using System.ComponentModel.DataAnnotations;

namespace GRA.Data.Abstract
{
    public abstract class BaseDbEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
