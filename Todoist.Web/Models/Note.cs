using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Todoist.Web.Models
{
    public class Note
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public ApplicationUser Author { get; set; }

        [Required]
        public Guid AuthorId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Body { get; set; }

        [Required]
        public DateTime DateCreationUTC => DateTime.UtcNow;
    }
}
