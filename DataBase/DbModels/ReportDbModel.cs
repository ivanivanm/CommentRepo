using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.DbModels
{
    public class ReportDbModel
    {
        [Key]
        public int IdReport { get; set; }

        public UserDbModel User { get; set; }

        [Required]
        public string Title { get; set; }

        public byte[] Image { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Location { get; set; }

        [ForeignKey(nameof(UserDbModel))]
        [Required]
        public int UserId { get; set; }

        private ReportDbModel() { }

        public ReportDbModel(string title, byte[] image, string description, string location, int userId, UserDbModel user)
        {
            this.Title = title;
            this.Image = image;
            this.Description = description;
            this.Location = location;
            this.UserId = userId;
            this.User = user;
        }
        public ReportDbModel(string title, string description, string location, int userId, UserDbModel user)
        {
            this.Title = title;
            this.Description = description;
            this.Location = location;
            this.UserId = userId;
            this.User = user;
        }
    }
}
