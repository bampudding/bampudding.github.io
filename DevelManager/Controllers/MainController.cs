using DevelManager.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DevelManager.Controllers
{
    /// <summary>
    /// 메인 컨트롤러
    /// </summary>
    public class MainController : Controller
    {
        /// <summary>
        /// 로거
        /// </summary>
        private readonly ILogger<MainController> _logger;

        /// <summary>
        /// 컨트롤러 정의
        /// </summary>
        /// <param name="logger"></param>
        public MainController(ILogger<MainController> logger)
            => _logger = logger;

        /// <summary>
        /// 에러
        /// </summary>
        /// <returns></returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
            => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

        /// <summary>
        /// 메인 페이지
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

    }
}