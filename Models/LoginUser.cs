using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginReg.Models
{
    public class LoginUser
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}