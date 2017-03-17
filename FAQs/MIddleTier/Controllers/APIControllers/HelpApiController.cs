using Microsoft.Practices.Unity;
using Sabio.Web.Domain;
using Sabio.Web.Models.Requests;
using Sabio.Web.Models.Responses;
using Sabio.Web.Services;
using Sabio.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Web.Controllers.Api
{
    [RoutePrefix("api/help")]
    public class HelpApiController : ApiController
    {
        [Dependency]
        public IHelpService _HelpService { get; set; }

        [Route, HttpPost]
        public HttpResponseMessage InsertHelp(HelpInsertRequest model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            int id = _HelpService.AddFaq(model);

            ItemResponse<int> response = new ItemResponse<int>();
            response.Item = id;

            return Request.CreateResponse(HttpStatusCode.OK, response);

        }

        [Route("order"), HttpPut]
        public HttpResponseMessage Sort(HelpOrderRequest model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            SuccessResponse response = new SuccessResponse();

            int Order = 1;

            foreach (int Id in model.SortOrder)
            {
                _HelpService.UpdateToOrder(Id, Order);
                Order += 10;
            }

            return Request.CreateResponse(response);
        }

        [Route, HttpPut]
        public HttpResponseMessage UpdateHelp(HelpUpdateRequest model)
        {
            if (!ModelState.IsValid && model != null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            _HelpService.UpdateFaq(model);

            return Request.CreateResponse(HttpStatusCode.OK, true);
        }

        [Route(), HttpGet]
        public HttpResponseMessage GetHelp()
        {

            ItemsResponse<Help> response = new ItemsResponse<Help>();

            List<Help> HelpList = _HelpService.GetAllFaqs();

            response.Items = HelpList;

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [Route("{Id:int}"), HttpGet]
        public HttpResponseMessage GetHelpById(int Id)
        {

            ItemResponse<Help> response = new ItemResponse<Help>();

            Help FaqItem = _HelpService.GetFaqById(Id);

            response.Item = FaqItem;

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [Route("{Id:int}"), HttpDelete]
        public HttpResponseMessage DeleteHelpbyId(int Id)
        {
            SuccessResponse response = new SuccessResponse();

            _HelpService.DeleteFaqById(Id);

            return Request.CreateResponse(response);
        }

        [Route("website/{WebsiteId:int}"), HttpGet]
        public HttpResponseMessage GetHelpByWebId(int WebsiteId)
        {

            ItemsResponse<Help> response = new ItemsResponse<Help>();

            List<Help> FaqItem = _HelpService.GetAllByWebId(WebsiteId);

            response.Items = FaqItem;

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
    }
}
