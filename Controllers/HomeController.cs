using Microsoft.AspNetCore.Mvc;
using ProjetoDron_ee.Models;
using System.Diagnostics;

namespace ProjetoDron_ee.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewData["SuccessMessage"] = HttpContext.Session.GetString("SuccessMessage");
            HttpContext.Session.Remove("SuccessMessage");

            ViewData["LogoutMessage"] = TempData["LogoutMessage"];
            return View();
        }

        public IActionResult Contato() => View();
        public IActionResult Cadastro() => View();
        public IActionResult Who() => View();
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Adicionar(string nome, string email, string senha, string senha2, string cep, string endereco, string bairro, string cidade)
        {
            var sessionEmail = HttpContext.Session.GetString("Email");

            if (!string.IsNullOrEmpty(sessionEmail) && sessionEmail == email)
            {
                ViewData["ErrorMessage"] = "Usuário já cadastrado. Faça login.";
                return View("Cadastro");
            }

            if (senha != senha2)
            {
                ViewData["ErrorMessage"] = "As senhas não coincidem.";
                return View("Cadastro");
            }

            HttpContext.Session.SetString("Email", email);
            HttpContext.Session.SetString("Senha", senha);
            HttpContext.Session.SetString("SuccessMessage", "Cadastro realizado com sucesso!");

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult LoginUser(string user, string senha)
        {
            var sessionEmail = HttpContext.Session.GetString("Email");
            var sessionSenha = HttpContext.Session.GetString("Senha");

            if (string.IsNullOrEmpty(sessionEmail) || string.IsNullOrEmpty(sessionSenha))
            {
                ViewData["ErrorMessage"] = "Usuário não encontrado. Cadastre-se.";
                return View("Index");
            }

            if (sessionEmail != user || sessionSenha != senha)
            {
                ViewData["ErrorMessage"] = "Email ou senha incorretos.";
                return View("Index");
            }

            TempData["LoginSuccess"] = "Bem-vindo(a)!";
            HttpContext.Session.SetString("IsLoggedIn", "true"); // Marca o usuário como logado
            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Limpa todos os dados da sessão
            TempData["LogoutMessage"] = "Desconectado com sucesso.";
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() =>
            View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}