using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IAAI.Models
{
    public class ForumReply
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "編號")]
        public int ForumReplyId { get; set; }


        [Display(Name = "內容")]
        public string ReplyContentHtml { get; set; }


        [Display(Name = "對應文章")]
        public int ForumId { get; set; }

        [ForeignKey("ForumId")]
        public virtual Forums Forums { get; set; }


        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "發表人")]
        public int ForumMemberId { get; set; }


        [Display(Name = "帳號建立日期")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]  //只顯示年月日
        [DataType(DataType.DateTime)]
        public DateTime? InitDate { get; set; }
    }
}