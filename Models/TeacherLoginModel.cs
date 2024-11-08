﻿using System;
using System.ComponentModel.DataAnnotations;

namespace AuthSystem.Models
{
    public class TeacherLoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}

