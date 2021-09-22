using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services;

namespace SalesWebMvc.Controllers
{
    //aula 253
    public class SellersController : Controller
    {
        // aula 254
        private readonly SellerService _sellerService;

        private readonly DepartmentService _departmentService;

        public SellersController(SellerService sellerService, DepartmentService departmentService)
        {
            _sellerService = sellerService;
            _departmentService = departmentService;
        }

        public IActionResult Index()
        {

            var list = _sellerService.FindAll();
            return View(list);
        }

        // aula 255
        public IActionResult Create()
        {   
            // alteracoes da aula 257 - 6:40
            var departments = _departmentService.FindAll();
            var viewModel = new SellerFormViewModel { Departments = departments };
            return View(viewModel);
        }

        // aula  255
        [HttpPost]
        [ValidateAntiForgeryToken] // previne a aplicação contra ataques CSRF
        public IActionResult Create(Seller seller)
        {
            _sellerService.Insert(seller);
            return RedirectToAction(nameof(Index));
        }

        // aula 258
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                // retorno provisorio - instancia resposta basica
                // posteriormente será personalizado c/ pagina de erro
                return NotFound();
            }

            // foi passado o id.value pq o parametro é "nullable" (opcional)
            var obj = _sellerService.FindById(id.Value);
            if (obj == null)
            {
                return NotFound();
            }

            // se tudo correr corretamente, envia o objeto para view
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _sellerService.Remove(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
