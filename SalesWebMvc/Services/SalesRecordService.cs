using SalesWebMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SalesWebMvc.Services
{
    // aula 267
    public class SalesRecordService
    {
        private readonly SalesWebMvcContext _context;

        public SalesRecordService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public async Task<List<SalesRecord>> FindByDateAsync(DateTime? minDate, DateTime? maxDate)
        {
            // consulta que retorna objeto do tipo IQueryable
            //
            // vale ressaltar que essa consulta NAO É EXECUTADA pela simples
            // definição dela
            var result = from obj in _context.SalesRecord select obj;
            if (minDate.HasValue)
            {
                // se houver dados no parametro minDate
                // adiciona uma restrição de data a query
                result = result.Where(x => x.Date >= minDate.Value);
            }

            if (maxDate.HasValue)
            {
                result = result.Where(x => x.Date <= maxDate.Value);
            }

            return await result
                       .Include(x => x.Seller)
                       .Include(x => x.Seller.Department)
                       .OrderByDescending(x => x.Date)
                       .ToListAsync();
        }

        // aula 268
        // é basicamente o mesmo metodo FindByDateAsync
        // só muda a inclusão do "GroupBy" e o retorno na assinatura do metodo para "IGrouping"
        public async Task<List<IGrouping<Department, SalesRecord>>> FindByDateGroupingAsync(DateTime? minDate, DateTime? maxDate)
        {
            // consulta que retorna objeto do tipo IQueryable
            //
            // vale ressaltar que essa consulta NAO É EXECUTADA pela simples
            // definição dela
            var result = from obj in _context.SalesRecord select obj;
            if (minDate.HasValue)
            {
                // se houver dados no parametro minDate
                // adiciona uma restrição de data a query
                result = result.Where(x => x.Date >= minDate.Value);
            }

            if (maxDate.HasValue)
            {
                result = result.Where(x => x.Date <= maxDate.Value);
            }

            return await result
                       .Include(x => x.Seller)
                       .Include(x => x.Seller.Department)
                       .OrderByDescending(x => x.Date)
                       .GroupBy(x => x.Seller.Department)
                       .ToListAsync();
        }


    }
}