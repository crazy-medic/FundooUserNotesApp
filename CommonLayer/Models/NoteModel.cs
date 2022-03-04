using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.Models
{
    public class NoteModel
    {
        
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime? Reminder { get; set; }
        public string Color { get; set; }
        public string BgImage { get; set; }

    }
}
