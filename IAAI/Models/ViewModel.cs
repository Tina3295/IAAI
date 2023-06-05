using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IAAI.Models
{
    public class ViewModel
    {
        public class ForumLogin
        {
            [Required(ErrorMessage = "{0}必填")]
            [MaxLength(30)]
            [Display(Name = "帳號")]
            public string Account { get; set; }


            [Required(ErrorMessage = "{0}必填")]
            [StringLength(100, ErrorMessage = "{0}長度至少必須為 {2} 個字元。", MinimumLength = 8)]
            [Display(Name = "密碼")]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }


    }
}