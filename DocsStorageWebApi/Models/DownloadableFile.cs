using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocsStorageWebApi.Models
{
    public class DownloadableFile
    {
        public string DocumentName { get; set; }

        public byte[] DocumentData { get; set; }
    }
}
