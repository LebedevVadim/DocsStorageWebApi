using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace DocsStorage.Models
{
    public class Document
    { 
        public int DocumentID { get; set; }
          
        [Display(Name = "Автор")]
        public string Author { get; set; }

        [Display(Name = "Имя документа")]
        public string DocumentName { get; set; }

        public string Exstension { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Дата загрузки")]
        public DateTime LoadDate { get; set; }

        public string DownloadRef { get; set; }

        public IFormFile DocumentData { get; set; }
    }
}
