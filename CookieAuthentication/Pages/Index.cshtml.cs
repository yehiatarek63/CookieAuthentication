﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace CookieAuthentication.Pages;

[BindProperties]
public class IndexModel : PageModel
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
    

    public void OnGet()
    {

    }
    public async Task<IActionResult> OnPostSignIn() { 
        if (ModelState.IsValid)
        {
            if(Username == "intern" && Password == "summer 2023 july")
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, Username),
                    new Claim(ClaimTypes.Role, "Intern")
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    IssuedUtc = DateTimeOffset.UtcNow,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                };
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                return RedirectToPage();
            }
        }
        ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
        TempData["ErrorMessage"] = "Invalid Login Attempt";
        return RedirectToPage();
    }


    public async Task<IActionResult> OnPostSignOut()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToPage();
    }
}




