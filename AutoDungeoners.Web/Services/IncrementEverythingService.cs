using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoDungeoners.Web.DataAccess.Repositories;
using AutoDungeoners.Web.Models;

namespace AutoDungeoners.Web.Services
{
    class IncrementEverythingService : AbstractService
    {
        private readonly IGenericRepository genericRepository;
        private IEnumerable<User> users;

        public IncrementEverythingService(IGenericRepository genericRepository)
        {
            this.genericRepository = genericRepository;
        }

        public override async Task OnTick(TimeSpan elapsedTime)
        {
            // Because order matters, update every user, aspect by aspect
            await Task.Run(() => {
                this.users = genericRepository.All<User>();

                this.UpdateGold(elapsedTime);

                this.SaveAllUsers();
            });
        }

        private void UpdateGold(TimeSpan elapsedTime)
        {
            foreach (var user  in users)
            {
                user.Gold += (int)Math.Floor(elapsedTime.TotalSeconds);
            }
        }

        private void SaveAllUsers()
        {
            foreach (var user in this.users)
            {
                this.genericRepository.Update(user);
            }
        }

    }
}