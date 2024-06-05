using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure_blob_storage_sol.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Net;
using System.Xml.Linq;

namespace Azure_blob_storage_sol.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AzureContainerController : ControllerBase
    {
        readonly BlobServiceClient _blobServiceClient;
        readonly APIResponse _response;
        public AzureContainerController(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient=blobServiceClient;
            _response=new();
        }

        [HttpPost]
        public async Task<ActionResult<APIResponse>> CreateContainer(string containerName)
        {
            try
            {
                BlobContainerClient container = await _blobServiceClient.CreateBlobContainerAsync(containerName);

                if (await container.ExistsAsync())
                {
                    _response.Result = container;
                    _response.StatusCode= HttpStatusCode.OK;
                    _response.IsSuccess = true;
                    _response.Message = new List<string> { $"{containerName} Container Successfully Created " };
                }
            }
            catch (Exception e)
            {

                _response.Result = null;
                _response.StatusCode= HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.Message = new List<string> { e.Message };

            }

            return _response;
        }


        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetContainerList()
        {
            try
            {
                List<string> listofcontainer = new List<string>();

                var result = _blobServiceClient.GetBlobContainersAsync().AsPages();

                await foreach (Azure.Page<BlobContainerItem> item in result)
                {
                    foreach (BlobContainerItem blobItem in item.Values)
                    {
                        listofcontainer.Add(blobItem.Name);
                    }

                }

                _response.Result= listofcontainer;
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message =new List<string> { "List fetch Successfully " };


            }
            catch (Exception e)
            {
                _response.Result= null;
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Message =new List<string> { e.Message };
            }

            return _response;

        }


        [HttpPost]
        public async Task<ActionResult<APIResponse>> DeleteContainer(string containerName)
        {
            try
            {
                BlobContainerClient container = _blobServiceClient.GetBlobContainerClient(containerName);

                _response.Result = await container.DeleteIfExistsAsync();
                _response.IsSuccess = true;
                _response.Message = new List<string> { $"{containerName} container deleted successully "  };
                _response.StatusCode =  HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                _response.Result = null;
                _response.IsSuccess = false;
                _response.Message = new List<string> { e.Message };
                _response.StatusCode =  HttpStatusCode.InternalServerError;

            }

            return _response;
        }

        [HttpPost]
        public async Task<ActionResult<APIResponse>> RestoreContainer(string containerName)
        {
            try
            {
                await foreach (BlobContainerItem item in _blobServiceClient.GetBlobContainersAsync(BlobContainerTraits.None ,BlobContainerStates.Deleted))
                {
                    if (item.Name ==containerName)
                    {
                        try
                        {
                            _response.Result = await _blobServiceClient.UndeleteBlobContainerAsync(containerName, item.VersionId);
                            _response.StatusCode = HttpStatusCode.OK;
                            _response.IsSuccess = true;
                            _response.Message  = new List<string> { $"{containerName} is restored successfully"};
                        }
                        catch (Exception ex)
                        {
                            _response.Result = null;
                            _response.StatusCode = HttpStatusCode.InternalServerError;
                            _response.IsSuccess = false;
                            _response.Message  = new List<string> {ex.Message };

                        }
                    }
                }
            }
            catch (Exception e)
            {
                _response.Result = null;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.Message  = new List<string> { e.Message };

            }

            return _response;
        }


    }
}
