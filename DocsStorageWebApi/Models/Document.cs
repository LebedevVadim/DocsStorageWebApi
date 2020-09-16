using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocsStorageWebApi.Models
{
    [Table(name: "Documents")]
    public class Document
    {
        [Key]
        public int DocumentID { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        public string DocumentName { get; set; }

        [Required]
        public string Exstension { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime LoadDate { get; set; }

        public string DocumentStorageID { get; set; }

        [NotMapped]
        public IFormFile DocumentData { get; set; }
    }
}
