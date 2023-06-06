using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IAAI.Models
{
    public class Forums
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "編號")]
        public int ForumId { get; set; }


        [Required(ErrorMessage = "{0}必填")]
        [MaxLength(30)]
        [Display(Name = "標題")]
        public string Title { get; set; }


        [Display(Name = "內容")]
        public string ContentHtml { get; set; }


        [Display(Name = "發表人")]
        public int ForumMemberId { get; set; }

        [ForeignKey("ForumMemberId")]
        public virtual ForumMember ForumMember { get; set; }



        [Display(Name = "帳號建立日期")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]  //只顯示年月日
        [DataType(DataType.DateTime)]
        public DateTime? InitDate { get; set; }
    }
}