using Microsoft.AspNetCore.Mvc;

namespace DevelManager.Controllers
{
    /// <summary>
    /// 에러 컨트롤러
    /// </summary>
    public class ErrorController : Controller
    {
        /// <summary>
        /// 뷰
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
            => View();
    }
}
