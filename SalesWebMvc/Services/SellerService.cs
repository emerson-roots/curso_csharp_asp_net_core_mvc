using SalesWebMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Services.Exceptions;

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
            // metodo alterado na aula 259 - 6:00 aplicando o cenceito de EAGER LOADING
            // com o Include para incluir o departamento associado ao vendedor
            return _context.Seller.Include(obj => obj.Department).FirstOrDefault(obj => obj.Id == id);
        }

        // aula 258
        public void Remove(int id)
        {
            var obj = _context.Seller.Find(id);
            _context.Seller.Remove(obj);
            _context.SaveChanges();
        }

        // aula 260
        public void Update(Seller obj)
        {
            // verifica se o id NAO EXISTE no banco
            if (!_context.Seller.Any(x => x.Id == obj.Id))
            {
                throw new NotFoundExceptionPersonalized("Id inexistente");
            }

            try
            {
                _context.Update(obj);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException e)
            {
                // IMPORTANTE: segregação de camadas
                // captura uma exceção a nivel de acesso a dados do entity framework
                // e relança a exceção utilizando nossa exceção personalizada a nível de serviço
                // sendo assim, Controllers lidam somente com excessões da camada de Serviço
                // respeitando a arquitetura proposta no curso
                throw new DbConcurrencyExceptionPersonalized(e.Message);
            }

        }

        }
    }
