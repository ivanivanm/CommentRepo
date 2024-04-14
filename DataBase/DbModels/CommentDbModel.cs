using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Microsoft.EntityFrameworkCore;

namespace DataBase.DbModels
{
    public class CommentDbModel
    {
        private CommentDbModel()
        {
            
        }
        public CommentDbModel(int userId, int reportId, DateTime postedOn, string commentContent, UserDbModel user)
        {
            UserId = userId;
            ReportId = reportId;
            PostedOn = postedOn;
            CommentContent = commentContent;
            User = user;           

        }

        public CommentDbModel(UserDbModel user, int userId, ReportDbModel report, int reportId, DateTime postedOn, string commentContent)
        {
            User = user;
            UserId = userId;
            Report = report;
            ReportId = reportId;
            PostedOn = postedOn;
            CommentContent = commentContent;
        }

        [Key]
        public int Id { get; set; }

        public UserDbModel User { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        [Required]
        public ReportDbModel Report { get; set; }
        [ForeignKey(nameof(Report))]


        public int ReportId { get; set; }
        [Required]

        public DateTime PostedOn { get; set; }

        [Required]
        public string CommentContent { get; set; }
       
    }
}
