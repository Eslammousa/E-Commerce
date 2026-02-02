using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace E_Commerce.Core.DTO.IdentityDTO
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Name Can't be blank")]
        public string PersonName { get; set; } = null!;


        [Required(ErrorMessage = "Email Can't be blank")]
        [EmailAddress(ErrorMessage = "Email Should be in a proper email address format")]
        [Remote(action: "IsEmailAlreadyRegistered", controller: "Account", ErrorMessage = "{0} is already taken .")]
        public string Email { get; set; } = null!;


        [Required(ErrorMessage = "Phone Can't be blank")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Phone Should contain only numbers")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; } = null!;



        [Required(ErrorMessage = "Password Can't be blank")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;


        [Required(ErrorMessage = "Confirm Password Can't be blank")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and Confirm Password don't match")]
        public string ConfirmPassword { get; set; } = null!;


    }
}
