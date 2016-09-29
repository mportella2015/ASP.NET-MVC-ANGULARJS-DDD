using System.Web.Mvc;

namespace Agnus.Interface.Web.Controllers
{
    public class DashboardController : BaseController
    {
        public DashboardController()
        {
            ViewBag.Descricao = "Página Inicial";
        }

        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }

       
    }
}