﻿using System.ComponentModel.DataAnnotations;

namespace DistributedLogging.Web.Api.Auth.Models
{
    public class LoginRequestModel
    {
        [Required(ErrorMessage = "Email is required.")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string? Password { get; set; }
    }
}
