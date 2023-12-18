using Bookinist.DAL.Entityes;
using Bookinist.Infrastructure.Commands;
using Bookinist.Interfaces;
using Bookinist.Model;
using Bookinist.ViewModels.Base;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Bookinist.ViewModels
{
    internal class StatisticViewModel : BaseViewModel
    {
        private IRepository<Book> _booksRepository;
        private IRepository<Buyer> _buyersRepository;
        private IRepository<Seller> _sellersRepository;
        private IRepository<Deal> _dealsRepository;

        public StatisticViewModel(IRepository<Book> booksRepository, 
            IRepository<Buyer> buyersRepository, 
            IRepository<Seller> sellersRepository,
            IRepository<Deal> dealsRepository)
        {
            _booksRepository = booksRepository;
            _buyersRepository = buyersRepository;
            _sellersRepository = sellersRepository;
            _dealsRepository = dealsRepository;
            ComputeStatisticCommand = new RelayCommand(OnComputeStatisticCommandExecuted, CanComputeStatisticCommandExecute);
        }

        public ObservableCollection<BestSellerInfo> BestSellers { get; } = new ObservableCollection<BestSellerInfo>();

        /* ------------------------------------------------------------------------------------------------------------ */

        #region Commands

        #region ComputeStatisticCommand
        public ICommand ComputeStatisticCommand { get; }
        private async void OnComputeStatisticCommandExecuted(object p)
        {
            BooksCount = await _booksRepository.Items.CountAsync();
            await ComputeDealsStatistiAsync();
        }

        private bool CanComputeStatisticCommandExecute(object p) => true;
        #endregion

        #endregion

        private async Task ComputeDealsStatistiAsync()
        {
            var deals = _dealsRepository.Items;
            var bestSellers = await deals.GroupBy(d => d.Book.Id)
                .Select(d => new
                {
                    BookId = d.Key,
                    Count = d.Count()
                })
                .OrderByDescending(deals => deals.Count)
                .Take(5)
                    .Join(_booksRepository.Items,
                        deals => deals.BookId,
                        book => book.Id,
                        (deals, book) => new BestSellerInfo { Book = book, SellCount = deals.Count })
                .ToArrayAsync();

            BestSellers.Clear();
            foreach (var item in bestSellers)
                BestSellers.Add(item);

        }

        /* ------------------------------------------------------------------------------------------------------------ */

        /// <summary>
        /// Число книг
        /// </summary>
        public int BooksCount { get => Get<int>(); set => Set(value); }



    }
}
