using SalesWebMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Services.Exceptions;
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
        //
        // alterado na aula 264 p/ async
        public async Task<List<Seller>> FindAllAsync()
        {
            // alterado na aula 264 p/ async
            return await _context.Seller.ToListAsync();
        }

        // aula 255
        //
        // alterado na aula 264 p/ async
        public async Task InsertAsync(Seller obj)
        {
            _context.Add(obj);
            await _context.SaveChangesAsync();
        }

        // aula 258
        //
        // alterado na aula 264 p/ async
        public async Task<Seller> FindByIdAsync(int id)
        {
            // metodo alterado na aula 259 - 6:00 aplicando o cenceito de EAGER LOADING
            // com o Include para incluir o departamento associado ao vendedor
            //
            // alterado na aula 264 p/ async
            return await _context.Seller.Include(obj => obj.Department).FirstOrDefaultAsync(obj => obj.Id == id);
        }

        // aula 258
        //
        // alterado na aula 264 p/ async
        public async Task RemoveAsync(int id)
        {
            var obj = await _context.Seller.FindAsync(id);
            _context.Seller.Remove(obj);
            await _context.SaveChangesAsync();
        }

        // aula 260
        //
        // alterado na aula 264 p/ async
        public async Task UpdateAsync(Seller obj)
        {
            bool hasAny = await _context.Seller.AnyAsync(x => x.Id == obj.Id);

            // verifica se o id NAO EXISTE no banco
            if (!hasAny)
            {
                throw new NotFoundExceptionPersonalized("Id inexistente");
            }

            try
            {
                _context.Update(obj);
                await _context.SaveChangesAsync();
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
