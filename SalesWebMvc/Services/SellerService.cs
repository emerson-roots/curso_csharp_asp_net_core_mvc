using SalesWebMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMvc.Services
{   
    // aula 254 - necessário registrar o serviço no sistema de injeção de dependencia
    // no Startup.cs
    public class SellerService
    {
        // READONLY - boa prática da comunidade dotnet 
        // previne a dependência de ser alterada
        private readonly SalesWebMvcContext _context;

        public SellerService(SalesWebMvcContext context)
        {
            _context = context;
        }

        // aula 254 - de primeiro momento foi implementado o metodo
        // deixando a operação como SÍNCRONA, o que significa que o metodo
        // ira rodar a operação e a aplicação ficara aguardando a operação terminar
        //
        // posteriormente será abordado o conceito ASSÍNCRONO, tornando a aplicação mais
        // performatica
        public List<Seller> FindAll()
        {
            return _context.Seller.ToList();
        }

        // aula 255
        public void Insert(Seller obj)
        {
            _context.Add(obj);
            _context.SaveChanges();
        }

        // aula 258
        public Seller FindById(int id)
        {
            return _context.Seller.FirstOrDefault(obj => obj.Id == id);
        }

        // aula 258
        public void Remove(int id)
        {
            var obj = _context.Seller.Find(id);
            _context.Seller.Remove(obj);
            _context.SaveChanges();
        }

    }
}
