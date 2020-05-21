using System;
using System.Threading.Tasks;

namespace AutoDungeoners.Web.Services
{
    interface IAbstractService
    {
        Task OnTick(int elapsedSeconds);
    }
}