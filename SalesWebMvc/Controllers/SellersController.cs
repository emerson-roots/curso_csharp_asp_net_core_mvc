using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services;
using SalesWebMvc.Services.Exceptions;
using System.Collections.Generic;

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
        // Delete do tipo GET para enviar os dados da lista
        // para a pagina de confirmação de deleção
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

        // aula 258
        // confirma a deleção na após confirmação na pagina Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _sellerService.Remove(id);
            return RedirectToAction(nameof(Index));
        }

        // aula 259
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obj = _sellerService.FindById(id.Value);
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        // aula 260 - GET
        // abre a tela para editar vendedor com os dados preenchidos
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obj = _sellerService.FindById(id.Value);
            if (obj == null)
            {
                return NotFound();
            }

            List<Department> departments = _departmentService.FindAll();

            // preenche o formulario de edicao com os dados do objeto buscado por id
            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = obj, Departments = departments };

            return View(viewModel);
        }

        // aula 260
        // Edit com o metodo POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Seller seller)
        {
            if (id != seller.Id)
            {
                return BadRequest();
            }

            try
            {
                _sellerService.Update(seller);
                return RedirectToAction(nameof(Index));
            }
            catch (NotFoundExceptionPersonalized)
            {
                return NotFound();
            }
            catch (DbConcurrencyExceptionPersonalized)
            {
                return BadRequest();
            }
        }

    }
}
