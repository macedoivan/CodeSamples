using Microsoft.Practices.Unity;
using App.Web.Domain;
using App.Web.Models.Requests;
using App.Web.Models.Responses;
using App.Web.Services;
using App.Web.Services.Interfaces;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace App.Web.Controllers.Api
{
    [RoutePrefix("api/admin/usercredits")]
    public class UserCreditsApiController : ApiController
    {
        [Dependency]
        public IUserCreditsService _CreditsService { get; set; }

        [Route, HttpPost]
        public HttpResponseMessage InsertUserCredits(UserCreditsRequest model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            int id = _CreditsService.InsertUserCredits(model);

            ItemResponse<int> response = new ItemResponse<int>();
            response.Item = id;

            return Request.CreateResponse(HttpStatusCode.OK, response);

        }

        [Route(), HttpGet]
        public HttpResponseMessage GetUserCreditsPaginated([FromUri]PaginatedRequest model)
        {

            PaginatedItemsResponse<UserCredits> response = _CreditsService.GetTransactionsPaginated(model);


            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [Route("{UserId}"), HttpGet]
        public HttpResponseMessage GetCreditsByUserId(string UserId)
        {
            ItemResponse<UserCreditsBalance> response = new ItemResponse<UserCreditsBalance>();

            //   UserCreditsService UserCreditsService = new UserCreditsService();

            UserCreditsBalance Balance = _CreditsService.GetBalance(UserId);

            response.Item = Balance;

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }


        [Authorize]
        [Route(), HttpGet]
        public HttpResponseMessage GetCreditsForJobPage()
        {
            ItemsResponse<UserCreditsForJobs> response = new ItemsResponse<UserCreditsForJobs>();

            string gotUserId = UserService.GetCurrentUserId();

            if (gotUserId != null)
            {
                response.Items = UserCreditsService.GetCreditsForJobPage(gotUserId);
            }

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

    }
}
