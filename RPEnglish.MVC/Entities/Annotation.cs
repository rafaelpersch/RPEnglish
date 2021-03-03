using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RPEnglish.MVC.Entities
{
    [Table("annotations")]
    public class Annotation
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Column("text", TypeName = "text")]
        [Required]
        public string Text { get; set; }

        public void Validate()
        {
            if (this.Id == Guid.Empty)
            {
                throw new ApplicationException("ID Empty");
            }

            if (string.IsNullOrEmpty(this.Text))
            {
                throw new ApplicationException("Text IsNull Or Empty");
            }
        }
    }
}
