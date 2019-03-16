using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace bbwm
{
    public class DocumentClientReal : IDocumentClientMockable//, IDocumentClient
    {
        private Uri uri;
        private string primaryKey;
        private DocumentClient client;

        public DocumentClientReal(Uri uri, string primaryKey)
        {
            this.uri = uri;
            this.primaryKey = primaryKey;
            this.client = new DocumentClient(uri, primaryKey);
        }

        public async Task CreateDocumentAsync<T>(Uri uri, T document) where T : new()
        {
            await client.CreateDocumentAsync(uri, document);
        }

        public async Task CreateDocumentCollectionIfNotExistsAsync(Uri uri, DocumentCollection documentCollection)
        {
            await client.CreateDocumentCollectionIfNotExistsAsync(uri, documentCollection);
        }

        public async Task OpenAsync()
        {
            await client.OpenAsync();
        }

        public async Task<DocumentResponse<T>> ReadDocumentAsync<T>(Uri documentUri)
            where T : new()
        {
            var resp = await client.ReadDocumentAsync<T>(documentUri);

            return resp;
        }

        public async Task ReplaceDocumentAsync<T>(Uri documentUri, T document) where T : new()
        {
            await client.ReplaceDocumentAsync(documentUri, document);
        }
    }

    public interface IDocumentClientMockable
    {
        Task OpenAsync();
        Task<DocumentResponse<T>> ReadDocumentAsync<T>(Uri documentUri)
            where T : new();
        Task CreateDocumentCollectionIfNotExistsAsync(Uri uri, DocumentCollection documentCollection);
        Task ReplaceDocumentAsync<T>(Uri documentUri, T document)
            where T : new();
        Task CreateDocumentAsync<T>(Uri uri, T document)
            where T : new();
    }
}
