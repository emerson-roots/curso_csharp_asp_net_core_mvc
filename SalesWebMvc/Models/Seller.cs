using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SalesWebMvc.Models
{   
    // aula 251
    public class Seller
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "{0} requerido")]
        // aula 263 - chave {0} = nome do atributo "Name"
        // chave {1} e {2} são os indexes dos parametros passados em StringLength
        [StringLength(60, MinimumLength = 3, ErrorMessage = "O tamanho do {0} deve conter entre {2} e {1} caracteres")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} requerido")]
        [EmailAddress(ErrorMessage = "Entre com um email valido.")]
        [DataType(DataType.EmailAddress)]
        public string Email{ get; set; }

        // aula 262 - anotação DISPLAY define que como sera
        // mostrado o rótulo dos atributos nas telas.
        [Display(Name = "Birth Date")]
        // aula 262 - anotação DISPLAY
        // DataType.Date define que o input no navegador
        // receba somente a data. Sem esta anotação
        // o html estava pedindo hora também
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "{0} requerido")]
        public DateTime BirthDate { get; set; }

        // aula 262 - anotação DISPLAY
        [Display(Name = "Base Salary")]
        [DisplayFormat(DataFormatString = "{0:F2}")]
        [Required(ErrorMessage = "{0} requerido")]
        [Range(100.0, 50000.0, ErrorMessage = "{0} valor mínimo deve ser {1} e o máximo {2}")]
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
