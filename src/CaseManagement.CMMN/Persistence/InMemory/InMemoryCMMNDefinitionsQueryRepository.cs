using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.CMMN.Persistence.Responses;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence.InMemory
{
    public class InMemoryCMMNDefinitionsQueryRepository : ICMMNDefinitionsQueryRepository
    {
        private ICollection<tDefinitions> _tDefinitions;

        public InMemoryCMMNDefinitionsQueryRepository(ICollection<tDefinitions> definitions)
        {
            _tDefinitions = definitions;
        }

        public Task<tDefinitions> FindDefinitionById(string id)
        {
            return Task.FromResult(_tDefinitions.FirstOrDefault(d => d.id == id));
        }

        public Task<ICollection<tDefinitions>> GetAll()
        {
            return Task.FromResult(_tDefinitions);
        }

        public Task<FindResponse<tDefinitions>> Find(BaseFindParameter parameter)
        {
            IQueryable<tDefinitions> result = _tDefinitions.AsQueryable();
            switch(parameter.Order)
            {
                case FindOrders.ASC:
                    if (parameter.OrderBy == "id")
                    {
                        result = result.OrderBy(r => r.id);
                    }

                    if(parameter.OrderBy == "name")
                    {
                        result = result.OrderBy(r => r.name);
                    }

                    if (parameter.OrderBy == "create_datetime")
                    {
                        result = result.OrderBy(r => r.creationDate);
                    }
                    break;
                case FindOrders.DESC:
                    if (parameter.OrderBy == "id")
                    {
                        result = result.OrderByDescending(r => r.id);
                    }

                    if (parameter.OrderBy == "name")
                    {
                        result = result.OrderByDescending(r => r.name);
                    }

                    if (parameter.OrderBy == "create_datetime")
                    {
                        result = result.OrderByDescending(r => r.creationDate);
                    }

                    break;
            }

            int totalLength = result.Count();
            result = result.Skip(parameter.StartIndex).Take(parameter.Count);
            return Task.FromResult(new FindResponse<tDefinitions>
            {
                StartIndex = parameter.StartIndex,
                Count = parameter.Count,
                TotalLength = totalLength,
                Content = (ICollection<tDefinitions>)result.ToList()
            });
        }
    }
}