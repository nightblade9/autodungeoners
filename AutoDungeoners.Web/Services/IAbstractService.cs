using System;
using System.Threading.Tasks;

namespace AutoDungeoners.Web.Services
{
    interface IAbstractService
    {
        Task OnTick(TimeSpan elapsedTime);
    }
}