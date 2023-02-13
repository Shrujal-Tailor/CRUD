using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CRUD.Pages.Account
{
    [Authorize]
    public class LogoutModel : PageModel
    {
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync() 
        {
			await HttpContext.SignOutAsync("MyCookieAuth");
            
            return RedirectToPage("/Index");
		}
    }
}
