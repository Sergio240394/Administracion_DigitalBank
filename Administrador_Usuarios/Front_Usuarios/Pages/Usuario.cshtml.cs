using Front_Usuarios.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Front_Usuarios.Pages
{
    public class UsuarioFullModel : PageModel
    {
        [BindProperty]
        public Usuario Usuario { get; set; }

        public void OnGet() { }
    }

}
