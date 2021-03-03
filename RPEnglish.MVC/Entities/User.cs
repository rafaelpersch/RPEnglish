using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RPEnglish.MVC.Entities
{
    [Table("users")]
    public class User
    {
        [Key]
        [Required]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("name", TypeName = "text")]
        [Required]
        public string Name { get; set; }
        [Column("email", TypeName = "text")]
        [Required]
        public string Email { get; set; }

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

            if (string.IsNullOrEmpty(this.Email))
            {
                throw new ApplicationException("Name IsNull Or Empty");
            }
        }
    }
}