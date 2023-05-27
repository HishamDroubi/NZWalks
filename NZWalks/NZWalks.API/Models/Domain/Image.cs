using Microsoft.AspNetCore.Routing.Constraints;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.XPath;

namespace NZWalks.API.Models.Domain
{
    public class Image
    {
        public Guid Id { get; set; }

        [NotMapped]
        public IFormFile File { get; set; }

        public string FileName { get; set; }

        public string? FileDescription {get;set;}

        public long FileSizeInBytes { get; set; }

        public string FileExtension { get; set; }
        public string FilePath { get; set; }

    }
}
