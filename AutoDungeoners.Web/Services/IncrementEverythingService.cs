using System;
using System.Threading.Tasks;
using AutoDungeoners.Web.DataAccess.Repositories;

namespace AutoDungeoners.Web.Services
{
    class IncrementEverythingService : AbstractService
    {
        private readonly IGenericRepository genericRepository;

        public IncrementEverythingService(IGenericRepository genericRepository)
        {
            this.genericRepository = genericRepository;
        }

        public override Task OnTick(TimeSpan elapsedTime)
        {
            throw new NotImplementedException();
        }
    }
}