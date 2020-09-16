using DocsStorage.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using RestSharp;
using System.Net;
using System.IO;

namespace DocsStorage.Helpers
{
    public class ApiHelper
    {
        private readonly string apiAddress;

        public ApiHelper()
        {
            apiAddress = @"http://localhost:57803/api";
        }

        public Document GetDocument(int id)
        {
            var client = new RestClient($"{apiAddress}/document/{id}");
            client.Timeout = 10000;
            var request = new RestRequest(Method.GET);
            var response = client.Execute(request).Content;

            var document = JsonConvert.DeserializeObject<Document>(response);
            document.DownloadRef = $"{apiAddress}/download/{document.DocumentID}";

            return document;
        }

        public IEnumerable<Document> GetDocuments(int numPage)
        {
            var client = new RestClient($"{apiAddress}/documentsList/{numPage}");
            client.Timeout = 10000;
            var request = new RestRequest(Method.GET);
            var response = client.Execute(request).Content;

            var documents = JsonConvert.DeserializeObject<List<Document>>(response);

            foreach (var document in documents)
                document.DownloadRef = $"{apiAddress}/download/{document.DocumentID}";

            return documents;
        }

        public IEnumerable<Document> GetDocuments(string name)
        {
            var client = new RestClient($"{apiAddress}/documents/?name={name}");
            client.Timeout = 10000;
            var request = new RestRequest(Method.GET);
            var response = client.Execute(request).Content;

            var documents = JsonConvert.DeserializeObject<List<Document>>(response);

            foreach (var document in documents)
                document.DownloadRef = $"{apiAddress}/download/{document.DocumentID}";

            return documents;
        }

        public IRestResponse SaveDocument(Document document)
        {
            var client = new RestClient($"{apiAddress}/save");
            client.Timeout = 10000;
            var request = new RestRequest(Method.POST);
            byte[] data = null;
            using (var stream = new MemoryStream())
            {
                document.DocumentData.OpenReadStream().CopyTo(stream);
                data = stream.ToArray();
            }
            request.AddFile("DocumentData", data, document.DocumentData.FileName);
            request.AddParameter("Author", document.Author);
            request.AddParameter("DocumentID", document.DocumentID);
            request.AddParameter("DocumentName", document.DocumentName);
            request.AddParameter("Exstension", document.Exstension);
            request.AddParameter("LoadDate", document.LoadDate.ToString());
            request.AddParameter("Description", document.Description);
            var response = client.Execute(request);

            return response;
        }

    }
}
