using System;
using System.ComponentModel.DataAnnotations;

namespace GRA.Domain.Model
{
    public class Site : Abstract.IDomainEntity
    {
        [Required]
        [MaxLength(255)]
        public string Path { get; set; }
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
        [MaxLength(255)]
        public string Domain { get; set; }
    }
}
