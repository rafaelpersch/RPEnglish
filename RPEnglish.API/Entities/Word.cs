using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RPEnglish.API.Entities
{
    [Table("words")]
    public class Word
    {
        [Key]
        [Required]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("name", TypeName = "text")]
        [Required]
        public string Name { get; set; }
        [Column("translation", TypeName = "text")]
        public string Translation { get; set; }
        [Column("observation", TypeName = "text")]
        public string Observation { get; set; }
        [Required]
        [Column("categoryid")]
        public Guid CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public void Validate()
        {
            if (this.Id == Guid.Empty)
            {
                throw new ApplicationException("ID Empty");
            }

            if (this.CategoryId == Guid.Empty)
            {
                throw new ApplicationException("ID Empty");
            }

            if (string.IsNullOrEmpty(this.Name))
            {
                throw new ApplicationException("Name IsNull Or Empty");
            }

            if (string.IsNullOrEmpty(this.Translation))
            {
                throw new ApplicationException("Translation IsNull Or Empty");
            }
        }        
    }
}