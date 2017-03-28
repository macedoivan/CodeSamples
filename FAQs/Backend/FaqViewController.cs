using App.Web.Models.ViewModels;
using System.Web.Mvc;

namespace App.Web.Controllers
{
    [RoutePrefix ("website")]
    public class HelpController : Controller
    {
         //GET: Help
         [Route("{WebsiteId:int}/help")] // e.g. /22/help
        public ActionResult Index(int? WebsiteId = null)
        {
            ItemViewModel<int?> vm = new ItemViewModel<int?>();
            vm.Item = WebsiteId;

            return View("IndexNg", vm);
        }


        [Route("{WebsiteId:int}/how-it-works")]
        public ActionResult HowItWorks(int? WebsiteId = null)
        {
            ItemViewModel<int?> vm = new ItemViewModel<int?>();
            vm.Item = WebsiteId;
            return View("HowItWorksNg", vm);
        }
    }
}