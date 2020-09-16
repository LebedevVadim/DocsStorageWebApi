using DocsStorageWebApi.Models;
using Ionic.Zip;
using System.IO;

namespace DocsStorageWebApi.Exstensions
{
    public static class DocumentExtionsion
    {
        public static void SaveToFile(this Document document, string pathToFolder)
        {
            var path = Path.Combine(pathToFolder, $"{document.DocumentStorageID}.zip");

            if (!File.Exists(path))
            {
                using (var zip = new ZipFile())
                {
                    zip.AddEntry($"{document.DocumentName}.{document.Exstension}", document.DocumentData.OpenReadStream());
                    zip.Save(path);
                }
            }
        }

        public static void DeleteFile(this Document document, string pathToFolder)
        {
            var path = Path.Combine(pathToFolder, $"{document.DocumentStorageID}.zip");

            if (!File.Exists(path))
                throw new FileNotFoundException("Не найден файл!", document.DocumentName);

            File.Delete(path);
        }

        public static void TryDeleteFile(this Document document, string pathToFolder)
        {
            var path = Path.Combine(pathToFolder, $"{document.DocumentStorageID}.zip");

            if (File.Exists(path))
                File.Delete(path);
        }
    }
}
