using DocsStorageWebApi.Exstensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DocsStorageWebApi.Models
{
    public class DocumentsRepository : IDocRepository
    {
        private readonly AppDdContext context;

        private readonly StorageSettings storageSettings;

        public DocumentsRepository(AppDdContext context, StorageSettings storageSettings)
        {
            this.context = context;
            this.storageSettings = storageSettings;
        }

        public IQueryable<Document> Documents => context.Documents;

        public void Delete(int documentID)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var document = context.Documents.Find(documentID);
                    context.Documents.Remove(document);

                    context.SaveChanges();
                    document.DeleteFile(storageSettings.PathToFolder);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception($"При удалении документа с ИД {documentID} произошла ошибка!", ex);
                }
            }
        }

        public void Save(Document document)
        {
            if (document == null)
                throw new ArgumentNullException();

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {

                    if (document.DocumentID == 0)
                    {
                        document.DocumentStorageID = Guid.NewGuid().ToString();
                        context.Documents.Add(document);
                        document.SaveToFile(storageSettings.PathToFolder);
                    }
                    else
                    {
                        context.Entry(document).State = EntityState.Modified;
                    }

                    context.SaveChanges();
                    transaction.Commit();

                }
                catch (Exception ex)
                {
                    // Если ошибка: ошибка уникальности поля - пересоздать гуид, проверить нет ли такого же сохранить снова
                    if (ex.InnerException != null && ex.InnerException.HResult == -2146232060)
                    {
                        var newGuid = Guid.NewGuid().ToString();

                        while (context.Documents.Any(x => x.DocumentStorageID == newGuid))
                        {
                            newGuid = Guid.NewGuid().ToString();
                        }

                        context.SaveChanges();
                        transaction.Commit();
                    }
                    else
                    {
                        document.TryDeleteFile(storageSettings.PathToFolder);
                        transaction.Rollback();

                        throw new Exception($"При сохранении документа {document.DocumentName} произошла ошибка!", ex);
                    }
                }
            }
        }

        public DownloadableFile GetDownloadableFile(int documentId)
        {
            var file = new DownloadableFile();
            
            var document = context.Documents.Find(documentId);
            
            if (document == null)
                return null;

            var path = Path.Combine(storageSettings.PathToFolder, $"{document.DocumentStorageID}.zip");

            file.DocumentName = document.DocumentName;
            file.DocumentData = File.ReadAllBytes(path);

            return file;
        }

        public Document GetDocument(int id)
        {
            try
            {
                var param = new System.Data.SqlClient.SqlParameter("@id", id);
                var document = context.Documents.FromSql("GetDocument @id", param);

                return document.First();
            }
            catch (Exception ex)
            {
                throw new Exception($"При получении документа с ID: {id} произошла ошибка", ex);
            }
        }

        public IEnumerable<Document> GetDocuments(int offset, int count)
        {
            try
            {
                var countParam = new System.Data.SqlClient.SqlParameter("@count", count);
                var offsetParam = new System.Data.SqlClient.SqlParameter("@offset", offset);
                var documents = context.Documents.FromSql("GetDocuments @offset, @count", offsetParam, countParam);

                return documents.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"При получении документов произошла ошибка", ex);
            }
        }
    }
}
