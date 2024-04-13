using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Report
    {
        public Report() { }
        public int IdReport { get; set; }
        public string Title { get; set; }
        public byte[] Image { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
    }
}
