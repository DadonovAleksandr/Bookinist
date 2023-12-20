using Bookinist.DAL.Entityes;
using Bookinist.ViewModels.Base;

namespace Bookinist.ViewModels
{
    class BookEditorWindowViewModel : BaseViewModel
    {
        public BookEditorWindowViewModel(Book book)
        {
            Name = book.Name;
            Id = book.Id;
        }

        public string Name { get => Get<string>(); set => Set(value); }
        public int Id { get; }
    }
}
