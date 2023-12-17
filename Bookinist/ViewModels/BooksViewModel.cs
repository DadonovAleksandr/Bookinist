using Bookinist.DAL.Entityes;
using Bookinist.Interfaces;
using Bookinist.ViewModels.Base;

namespace Bookinist.ViewModels
{
    internal class BooksViewModel : BaseViewModel
    {
        private IRepository<Book> booksRepository;

        public BooksViewModel(IRepository<Book> booksRepository)
        {
            this.booksRepository = booksRepository;
        }
    }
}
