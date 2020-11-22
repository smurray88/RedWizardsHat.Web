﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RedWizardHat.Web.Models.Accounts
{
    public class ResetPasswordModel
    {
        [Required(ErrorMessage = "Password is Required.")]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Code is Required.")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Email is Required.")]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
