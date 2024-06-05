using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure_blob_storage_sol.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Net;

namespace Azure_blob_storage_sol.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AzureBlobController : ControllerBase
    {

        readonly BlobServiceClient _blobServiceClient;
        readonly APIResponse _aPIResponse;
        public AzureBlobController(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient =  blobServiceClient;
            _aPIResponse = new();

        }


    }
}
