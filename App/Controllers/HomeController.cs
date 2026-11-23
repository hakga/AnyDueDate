using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.DB;
using App.Domain.Repositories;
using App.Models;
using System.Diagnostics;

namespace App.Controllers {
    public class HomeController : Controller {
        private readonly SampleDbContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly IMemberReader _loaderMembers;
        private readonly IConfiguration _config;

        public HomeController(ILogger<HomeController> logger, SampleDbContext context, IMemberReader loaderMembers, IConfiguration config) {
            _logger = logger;
            _context = context;
            _loaderMembers = loaderMembers;
            _config = config;
        }

        public IActionResult Index() {
            return View();
        }

        public IActionResult Privacy() {
            ViewBag.MySetting = _config["MySetting"];
            return View();
        }

        public async Task<IActionResult> Sample(int? id) {
            var member = await _context.Members.FirstOrDefaultAsync();     // DB����1���擾�i�T���v���p�j
            if (id != null) {
            member =await _context.Members.FirstOrDefaultAsync(r => r.Id == id);
            }
            return View(member);
        }
        public async Task<IActionResult> Samples() {
            _logger.LogInformation("LogInformation.");
            var members = _loaderMembers.Load();        // �ˑ��������ς݂̃f�[�^�ǂݏo��
            return View(members);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            _logger.LogCritical("LogCritical!!");
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
