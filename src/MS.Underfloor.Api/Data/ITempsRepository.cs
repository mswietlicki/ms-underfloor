using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MS.Underfloor.Api.Data
{
    public interface ITempsRepository
    {
        Task InsertTemp(TempReport temp);
        Task<IReadOnlyCollection<TempReport>> GetTemps(DateTime from, DateTime? @to = null);
    }
}
