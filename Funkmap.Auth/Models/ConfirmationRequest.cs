﻿using Microsoft.Build.Framework;

namespace Funkmap.Module.Auth.Models
{
    public class ConfirmationRequest
    {
        [Required]
        public string Login { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Code { get; set; }
    }
}
