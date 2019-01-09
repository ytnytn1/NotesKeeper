using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotesKeeper.Models
{
    public class ApplicationUser:IdentityUser<Guid>
    {
        public ICollection<Note> Notes { get; set; }
    }
}
