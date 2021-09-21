using System;
using System.Collections.Generic;
using System.Linq;

// aula 245
namespace SalesWebMvc.Models
{
    public class Department
    {
        public int  Id { get; set; }
        public string Name { get; set; }

        // ICollection - responsavel pela associacao com as tabelas
        // Uma coleção genérica que aceita Lists, conjuntos HashSets, Etc
        // além disso, a coleção já inicia instanciada com o "new List<TIPO_OBJETO>()"
        public ICollection<Seller> Sellers { get; set; } = new List<Seller>();

        public Department()
        {

        }

        public Department(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public void AddSeller(Seller seller)
        {
            Sellers.Add(seller);
        }

        public double TotalSales(DateTime initial, DateTime final)
        {
            return Sellers.Sum(seller => seller.TotalSales(initial, final));
        }

    }
}
