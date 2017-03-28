using Microsoft.Practices.Unity;
using App.Web.Domain;
using App.Web.Models.Requests;
using App.Web.Models.Responses;
using App.Web.Services;
using App.Web.Services.Interfaces;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace App.Web.Controllers.Api
{
    [RoutePrefix("api/useraddress")]
    public class UserAddressApiController : ApiController
    {
        [Dependency]
        public IUserAddressService _UserAddressService { get; set; }

        [Route, HttpGet]
        public HttpResponseMessage GetAddressBook()
        {
            ItemsResponse<UserAddress> response = new ItemsResponse<UserAddress>();

            string UserId = UserService.GetCurrentUserId();

            response.Items = _UserAddressService.GetUserAddresses(UserId);

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [Route("{AddressId:int}"), HttpDelete]
        public HttpResponseMessage DeleteHelpbyId(int AddressId)
        {
            string UserId = UserService.GetCurrentUserId();

            SuccessResponse response = new SuccessResponse();

            _UserAddressService.DeleteAddress(UserId, AddressId);

            return Request.CreateResponse(response);
        }

        [Route, HttpPost]
        public HttpResponseMessage InsertUserCredits(AddressAddRequest model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            SuccessResponse response = new SuccessResponse();

            string UserId = UserService.GetCurrentUserId();

            _UserAddressService.InsertAddress(UserId, model);
            
            return Request.CreateResponse(HttpStatusCode.OK, response);

        }

        [Route("{AddressId:int}"), HttpPut]
        public HttpResponseMessage UpdateDefaultAddress(int AddressId)
        {
            string UserId = UserService.GetCurrentUserId();

            SuccessResponse response = new SuccessResponse();
            
            _UserAddressService.UpdateCustomerDefaultAddress(UserId, AddressId);

            return Request.CreateResponse(response);
        }

        [Route("default"), HttpGet]
        public HttpResponseMessage GetDefaultAddress()
        {
            ItemResponse<UserAddress> response = new ItemResponse<UserAddress>();

            string UserId = UserService.GetCurrentUserId();

            response.Item = _UserAddressService.GeDefaultAddress(UserId);

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
    }
}
