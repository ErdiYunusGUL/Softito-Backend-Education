using GymApp.NTier.Business.Services;
using GymApp.NTier.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace GymApp.NTier.Web.Controllers
{
    public class MembersController : Controller
    {
        private readonly IMemberService _memberService;

        public MembersController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        public async Task<IActionResult> Index()
        {
            var members = await _memberService.GetAllMembersAsync();
            return View(members);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Member member)
        {
            // Simple validation bypass for navigation properties like Plan
            ModelState.Remove("Plan");
            ModelState.Remove("Workouts");

            if (ModelState.IsValid)
            {
                member.JoinDate = DateTime.Now;
                await _memberService.AddMemberAsync(member);
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }
    }
}
