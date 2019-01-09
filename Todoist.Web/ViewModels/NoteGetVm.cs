using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NotesKeeper.ViewModels
{
    public class NoteGetVm
    {
        [Display(Name = "Id")]
        public Guid Id { get; set; }

        [Display(Name = "Заголовок")]
        public string Title { get; set; }

        [Display(Name = "Заметка")]
        public string Body { get; set; }

        [Display(Name = "Дата создания (UTC)")]
        public DateTime DateCreationUTC { get; set; }
    }
}
