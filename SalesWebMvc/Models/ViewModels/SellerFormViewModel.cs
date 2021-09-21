using System.Collections.Generic;

namespace SalesWebMvc.Models.ViewModels
{
    // aula 257 - atenção ao padrão de nomes nos atributos
    // pois auxilia o framework a reconhecer os dados
    // para fazer a conversao dos dados HTTP para Objeto
    public class SellerFormViewModel
    {
        public Seller Seller { get; set; }
        public ICollection<Department> Departments { get; set; }
    }
}
