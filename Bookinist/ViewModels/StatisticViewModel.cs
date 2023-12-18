using Bookinist.DAL.Entityes;
using Bookinist.Infrastructure.Commands;
using Bookinist.Interfaces;
using Bookinist.ViewModels.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq;
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

        /* ------------------------------------------------------------------------------------------------------------ */

        #region Commands

        #region ComputeStatisticCommand
        public ICommand ComputeStatisticCommand { get; }
        private async void OnComputeStatisticCommandExecuted(object p)
        {
            BooksCount = await _booksRepository.Items.CountAsync();
            var deals = _dealsRepository.Items;
            var bestSellers = await deals.GroupBy(d => d.Book)
                .Select(d => new 
                {
                    Book = d.Key,
                    Count = d.Count()
                })
                .OrderByDescending(b => b.Count)
                .Take(5)
                .ToArrayAsync();

        }
        private bool CanComputeStatisticCommandExecute(object p) => true;
        #endregion

        #endregion

        /* ------------------------------------------------------------------------------------------------------------ */

        /// <summary>
        /// Число книг
        /// </summary>
        public int BooksCount { get => Get<int>(); set => Set(value); }



    }
}
