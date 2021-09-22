using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services;
using SalesWebMvc.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

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

        // alterado na aula 264 p/ async
        public async Task<IActionResult> Index()
        {

            var list = await _sellerService.FindAllAsync();
            return View(list);
        }

        // aula 255
        //
        // alterado na aula 264 p/ async
        public async Task<IActionResult> Create()
        {
            // alteracoes da aula 257 - 6:40
            var departments = await _departmentService.FindAllAsync();
            var viewModel = new SellerFormViewModel { Departments = departments };
            return View(viewModel);
        }

        // aula  255
        //
        // alterado na aula 264 p/ async
        [HttpPost]
        [ValidateAntiForgeryToken] // previne a aplicação contra ataques CSRF
        public async Task<IActionResult> Create(Seller seller)
        {
            // aula 263 - validacao
            // verifica se o form NAO ESTA VALIDO
            // se estiver invalido, retorna para
            // a mesma view para corrigir os dados
            if (!ModelState.IsValid)
            {

                var departments = await _departmentService.FindAllAsync();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(viewModel);
            }
            await _sellerService.InsertAsync(seller);
            return RedirectToAction(nameof(Index));
        }

        // aula 258
        // Delete do tipo GET para enviar os dados da lista
        // para a pagina de confirmação de deleção
        //
        // alterado na aula 264 p/ async
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                // alterado na aula 261
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            // foi passado o id.value pq o parametro é "nullable" (opcional)
            var obj = await _sellerService.FindByIdAsync(id.Value);
            if (obj == null)
            {
                // alterado na aula 261
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            // se tudo correr corretamente, envia o objeto para view
            return View(obj);
        }

        // aula 258
        // confirma a deleção na após confirmação na pagina Delete
        //
        // alterado na aula 264 p/ async
        //
        // alterado na aula 265 ajustando exceção personalizada
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _sellerService.RemoveAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (IntegrityExceptionPersonalized e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        // aula 259
        //
        // alterado na aula 264 p/ async
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                // alterado na aula 261
                return RedirectToAction(nameof(Error), new { message = "Id not Provided" });
            }

            var obj = await _sellerService.FindByIdAsync(id.Value);
            if (obj == null)
            {
                // alterado na aula 261
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            return View(obj);
        }

        // aula 260 - GET
        // abre a tela para editar vendedor com os dados preenchidos
        //
        // alterado na aula 264 p/ async
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                // alterado na aula 261
                return RedirectToAction(nameof(Error), new { message = "Id not Provided" });
            }

            var obj = await _sellerService.FindByIdAsync(id.Value);
            if (obj == null)
            {
                // alterado na aula 261
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            List<Department> departments = await _departmentService.FindAllAsync();

            // preenche o formulario de edicao com os dados do objeto buscado por id
            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = obj, Departments = departments };

            return View(viewModel);
        }

        // aula 260
        // Edit com o metodo POST
        //
        // alterado na aula 264 p/ async
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Seller seller)
        {

            // aula 263 - validacao
            // verifica se o form NAO ESTA VALIDO
            // se estiver invalido, retorna para
            // a mesma view para corrigir os dados
            if (!ModelState.IsValid)
            {
                var departments = await _departmentService.FindAllAsync();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(viewModel);
            }

            if (id != seller.Id)
            {
                // alterado na aula 261
                return RedirectToAction(nameof(Error), new { message = "Id mismatch" });
            }

            try
            {
                await _sellerService.UpdateAsync(seller);
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException e)
            {
                // alterado na aula 261
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        // aula 261 - retorna a view html de erro
        public IActionResult Error(string message)
        {
            // instancia o objeto html ErrorViewModel.cs
            var viewModel = new ErrorViewModel
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(viewModel);
        }

    }
}
