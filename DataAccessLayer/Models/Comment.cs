using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Comment
    {
        public Comment(int id, int reportId, int userId, DateTime postedOn, string commentContent)
        {
            Id = id;
            ReportId = reportId;
            UserId = userId;
            PostedOn = postedOn;
            CommentContent = commentContent;
        }
        public Comment()
        {
            
        }

        public int Id { get; set; }
        public int ReportId { get; set; }
        public int UserId { get; set; }
        public DateTime PostedOn { get; set; }
        public string CommentContent { get; set; }
    }
}
