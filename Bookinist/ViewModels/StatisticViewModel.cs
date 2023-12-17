using Bookinist.DAL.Entityes;
using Bookinist.Interfaces;
using Bookinist.ViewModels.Base;

namespace Bookinist.ViewModels
{
    internal class StatisticViewModel : BaseViewModel
    {
        private IRepository<Book> booksRepository;
        private IRepository<Buyer> buyersRepository;
        private IRepository<Seller> sellersRepository;

        public StatisticViewModel(IRepository<Book> booksRepository, IRepository<Buyer> buyersRepository, IRepository<Seller> sellersRepository)
        {
            this.booksRepository = booksRepository;
            this.buyersRepository = buyersRepository;
            this.sellersRepository = sellersRepository;
        }
    }
}
