using System.Collections.Generic;
using System.Linq;

namespace DocsStorageWebApi.Models
{
    public interface IDocRepository
    {
        IQueryable<Document> Documents { get; }

        void Save(Document document);

        void Delete(int documentID);

        DownloadableFile GetDownloadableFile(int documentId);
        
        Document GetDocument(int id);

        IEnumerable<Document> GetDocuments(int offset, int count);
    }
}
