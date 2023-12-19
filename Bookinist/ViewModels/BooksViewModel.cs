using Bookinist.DAL.Entityes;
using Bookinist.Helpers;
using Bookinist.Infrastructure.DebugServices;
using Bookinist.Interfaces;
using Bookinist.ViewModels.Base;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace Bookinist.ViewModels
{
    internal class BooksViewModel : BaseViewModel
    {
        private IRepository<Book> _booksRepository;
        private CollectionViewSource _booksViewSource;

        public BooksViewModel()
        {
            if(!App.IsDesighnMode)
                throw new InvalidOperationException("Данный конструктор не предназначен для использования вне дизайнера");

            _booksRepository = new DebugBooksRepository();
            Books.AddClear(_booksRepository.Items);
            
             
        }

        public BooksViewModel(IRepository<Book> booksRepository)
        {
            _booksRepository = booksRepository;
            Books.AddClear(_booksRepository.Items);

            _booksViewSource = new CollectionViewSource
            {
                Source = _booksRepository.Items.ToArray(),
                SortDescriptions =
                {
                    new SortDescription(nameof(Book.Name), ListSortDirection.Ascending),
                }
            };

            _booksViewSource.Filter += OnBooksFilter;
        }

        private void OnBooksFilter(object sender, FilterEventArgs e)
        {
            if(!(e.Item is Book book) || string.IsNullOrEmpty(BooksFilter)) return;

            if (!book.Name.Contains(BooksFilter))
                e.Accepted = false;


        }

        public ObservableCollection<Book> Books { get; set; } = new ObservableCollection<Book>();
        public string BooksFilter 
        { 
            get => Get<string>();
            set
            {
                Set(value);
                _booksViewSource.View.Refresh();
            }
        }
        public ICollectionView BooksView => _booksViewSource.View;

    }
}
