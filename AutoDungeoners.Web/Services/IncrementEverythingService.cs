using System;
using System.Threading.Tasks;
using AutoDungeoners.Web.DataAccess.Repositories;
using AutoDungeoners.Web.Models;

namespace AutoDungeoners.Web.Services
{
    class IncrementEverythingService : AbstractService
    {
        private readonly IGenericRepository genericRepository;

        public IncrementEverythingService(IGenericRepository genericRepository)
        {
            this.genericRepository = genericRepository;
        }

        public override async Task OnTick(TimeSpan elapsedTime)
        {
            // Because order matters, update every user, aspect by aspect
            await Task.Run(() => {
                this.UpdateGold(elapsedTime);
            });
        }

        private void UpdateGold(TimeSpan elapsedTime)
        {
            var users = genericRepository.All<User>();
            foreach (var user  in users)
            {
                user.Gold += (int)Math.Floor(elapsedTime.TotalSeconds);
            }
        }
    }
}