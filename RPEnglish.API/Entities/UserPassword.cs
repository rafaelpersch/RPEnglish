using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RPEnglish.API.Entities
{
    [Table("users_passwords")]
    public class UserPassword
    {
        [Key]
        [Required]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("password", TypeName = "text")]
        [Required]
        public string Password { get; set; }
        [Required]
        [Column("userid")]
        public Guid UserId { get; set; }
        public virtual User User { get; set; }

        public void Validate()
        {
            if (this.Id == Guid.Empty)
            {
                throw new ApplicationException("ID Empty");
            }

            if (this.UserId == Guid.Empty)
            {
                throw new ApplicationException("UserId Empty");
            }

            if (string.IsNullOrEmpty(this.Password))
            {
                throw new ApplicationException("Password IsNull Or Empty");
            }
        }        
    }
}