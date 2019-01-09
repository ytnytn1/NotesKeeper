using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NotesKeeper.ViewModels
{
    public class AddEditNoteVm
    {
     
        [Required]
        [Display(Name = "Заголовок")]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Заметка")]
        public string Body { get; set; }

    }
}
