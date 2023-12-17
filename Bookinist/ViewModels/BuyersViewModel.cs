using Bookinist.DAL.Entityes;
using Bookinist.Interfaces;
using Bookinist.ViewModels.Base;

namespace Bookinist.ViewModels
{
    internal class BuyersViewModel : BaseViewModel
    {
        private IRepository<Buyer> buyersRepository;

        public BuyersViewModel(IRepository<Buyer> buyersRepository)
        {
            this.buyersRepository = buyersRepository;
        }
    }
}
