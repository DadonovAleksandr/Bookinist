using Bookinist.DAL.Entityes;
using Bookinist.Helpers;
using Bookinist.Infrastructure.Commands;
using Bookinist.Infrastructure.DebugServices;
using Bookinist.Interfaces;
using Bookinist.Service.UserDialogService;
using Bookinist.ViewModels.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace Bookinist.ViewModels
{
    internal class BooksViewModel : BaseViewModel
    {
        private IRepository<Book> _booksRepository;
        private readonly IUserDialogService _userDialogService;
        private CollectionViewSource _booksViewSource;

        public BooksViewModel()
        {
            if(!App.IsDesighnMode)
                throw new InvalidOperationException("Данный конструктор не предназначен для использования вне дизайнера");

            _booksRepository = new DebugBooksRepository();

            OnLoadDataCommandExecuted(null);
             
        }

        public BooksViewModel(IRepository<Book> booksRepository, IUserDialogService userDialogService)
        {
            _booksRepository = booksRepository;
            this._userDialogService = userDialogService;
            _booksViewSource = new CollectionViewSource
            {
                Source = _booksRepository.Items.ToArray(),
                SortDescriptions =
                {
                    new SortDescription(nameof(Book.Name), ListSortDirection.Ascending),
                }
            };

            Books = new ObservableCollection<Book>();
            LoadDataCommand = new RelayCommand(OnLoadDataCommandExecuted, CanLoadDataCommandExecute);
            AddBookCommand = new RelayCommand(OnAddBookCommandExecuted, CanAddBookCommandExecute);
            RemoveBookCommand = new RelayCommand(OnRemoveBookCommandExecuted, CanRemoveBookCommandExecute);
            _booksViewSource.Filter += OnBooksFilter;
        }


        #region Commands

        #region LoadDataCommand
        public ICommand LoadDataCommand { get; }
        private void OnLoadDataCommandExecuted(object p)
        {
            Books.AddClear(_booksRepository.Items);
        }

        private bool CanLoadDataCommandExecute(object p) => true;
        #endregion

        #region AddBookCommand
        public ICommand AddBookCommand { get; }
        private void OnAddBookCommandExecuted(object p)
        {
            
            var newBook = new Book();
            var category = new Category { Id = 1 };
            newBook.Category = category;
            if (!_userDialogService.Edit(newBook))
                return;

            Books.Add(_booksRepository.Add(newBook));
            SelectedBook = newBook;
        }

        private bool CanAddBookCommandExecute(object p) => true;
        #endregion

        #region RemoveBookCommand
        public ICommand RemoveBookCommand { get; }
        private void OnRemoveBookCommandExecuted(object p)
        {
            var bookToRemove = (Book)p ?? SelectedBook;

            if (!_userDialogService.Confirm($"Желаете удалить книгу: {bookToRemove.Name}"))
                return;
            _booksRepository.Remove(bookToRemove.Id);
            Books.Remove(bookToRemove);
            if (ReferenceEquals(SelectedBook, bookToRemove))
                SelectedBook = null;
        }

        private bool CanRemoveBookCommandExecute(object p) => p != null || SelectedBook != null;
        #endregion

        #endregion

        private void OnBooksFilter(object sender, FilterEventArgs e)
        {
            if(!(e.Item is Book book) || string.IsNullOrEmpty(BooksFilter)) return;

            if (!book.Name.Contains(BooksFilter))
                e.Accepted = false;


        }

        public ObservableCollection<Book> Books 
        { 
            get => Get<ObservableCollection<Book>>(); 
            set
            {
                Set(value);
                _booksViewSource.Source = value;
            }
        }

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

        public Book SelectedBook { get => Get<Book>(); set => Set(value); }

    }
}
