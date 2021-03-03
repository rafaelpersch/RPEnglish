using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RPEnglish.API.Entities
{
    [Table("categories")]
    public class Category
    {
        [Key]
        [Required]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("name", TypeName = "text")]
        [Required]
        public string Name { get; set; }

        public void Validate()
        {
            if (this.Id == Guid.Empty)
            {
                throw new ApplicationException("ID Empty");
            }

            if (string.IsNullOrEmpty(this.Name))
            {
                throw new ApplicationException("Name IsNull Or Empty");
            }
        }        
    }
}