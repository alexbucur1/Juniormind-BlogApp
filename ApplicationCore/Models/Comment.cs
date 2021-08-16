using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApp.Dotnet.ApplicationCore.Models
{
    public class Comment
    {
        public int ID { get; set; }

        [ForeignKey("User")]
        public string UserID { get; set; }
        public virtual User User { get; set; }

        [ForeignKey("BlogPost")]
        public int PostID { get; set; }
        public virtual BlogPost BlogPost { get; set; }

        [StringLength(900, MinimumLength = 1)]
        [Required]
        public string Content { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        public  int? ParentID { get; set; }
    }
}
