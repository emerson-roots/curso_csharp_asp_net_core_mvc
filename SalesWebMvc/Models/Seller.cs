using System;
using System.Collections.Generic;
using System.Linq;

namespace SalesWebMvc.Models
{   
    // aula 251
    public class Seller
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email{ get; set; }
        public DateTime BirthDate { get; set; }
        public double BaseSalary { get; set; }
        // associacao
        public Department Department { get; set; }

        // aula 256
        // usando este padrão de nomes (nome_classe_relação+nome_chave)
        // o Entity Framework garante que o id da relação seja inserido também em novos registros
        // corrigindo assim o problema de integridade referencial (chave não nula)
        // da aula anterior
        public int DepartmentId { get; set; }

        // ICollection - responsavel pela associacao com as tabelas
        // Uma coleção genérica que aceita Lists, conjuntos HashSets, Etc
        // além disso, a coleção já inicia instanciada com o "new List<TIPO_OBJETO>()"
        public ICollection<SalesRecord> Sales { get; set; } = new List<SalesRecord>();

        public Seller()
        {
        }

        public Seller(int id, string name, string email, DateTime birthDate, double baseSalary, Department department)
        {
            Id = id;
            Name = name;
            Email = email;
            BirthDate = birthDate;
            BaseSalary = baseSalary;
            Department = department;
        }

        public void AddSales (SalesRecord sr)
        {
            Sales.Add(sr);
        }

        public void RemoveSales(SalesRecord sr)
        {
            Sales.Remove(sr);
        }

        public double TotalSales(DateTime initial, DateTime final)
        {
            // utiliza LINQ para aplicar um filtro (Where) entre datas e efetuar o calculo de soma
            return Sales.Where(salesRecord => salesRecord.Date >= initial && salesRecord.Date <= final).Sum(sr => sr.Amount);
        }

    }
}
