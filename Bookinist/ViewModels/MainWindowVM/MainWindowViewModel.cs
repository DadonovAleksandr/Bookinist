using Bookinist.DAL.Entityes;
using Bookinist.Infrastructure.Commands;
using Bookinist.Interfaces;
using Bookinist.Model.AppSettings.AppConfig;
using Bookinist.Service.Interfaces;
using Bookinist.Service.UserDialogService;
using Bookinist.ViewModels.Base;
using ProjectVersionInfo;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Bookinist.ViewModels.MainWindowVm
{
    internal class MainWindowViewModel : BaseViewModel
    {
        private readonly IAppConfig _appConfig;
        private readonly IUserDialogService _userDialogService;
        private readonly IRepository<Book> _booksRepository;
        private readonly IRepository<Seller> _sellersRepository;
        private readonly IRepository<Buyer> _buyersRepository;
        private readonly ISalesService _salesService;



        /* ------------------------------------------------------------------------------------------------------------ */
        public MainWindowViewModel(IUserDialogService userDialogService,
            IRepository<Book> booksRepository,
            IRepository<Seller> sellersRepository,
            IRepository<Buyer> buyersRepository,
            ISalesService salesService
            )
        {
            _log.Debug($"Вызов конструктора {GetType().Name}");
            _appConfig = AppConfig.GetConfigFromDefaultPath();
            _userDialogService = userDialogService;
            
            var prjVersion = new ProjectVersion(Assembly.GetExecutingAssembly());
            Title = $"{AppConst.Get().AppDesciption} {prjVersion.Version}";

            _booksRepository = booksRepository;
            _sellersRepository = sellersRepository;
            _buyersRepository = buyersRepository;
            _salesService = salesService;
            var books = _booksRepository.Items.Take(10).ToArray();

            #region Commands
            Exit = new RelayCommand(OnExitExecuted, CanExitExecute);
            ShowBooksViewCommand = new RelayCommand(OnShowBooksViewCommandExecuted, CanShowBooksViewCommandExecute);
            ShowBuyersViewCommand = new RelayCommand(OnShowBuyersViewCommandExecuted, CanShowBuyersViewCommandExecute);
            ShowStatisticViewCommand = new RelayCommand(OnShowStatisticViewCommandExecuted, CanShowStatisticViewCommandExecute);
            #endregion

        }

        private async Task Test()
        {
            var dealsCount = _salesService.Deals.Count();

            var book = await _booksRepository.GetAsync(5);
            var buyer = await _buyersRepository.GetAsync(3);
            var seller = await _sellersRepository.GetAsync(7);

            var deal = _salesService.MakeDeal(book.Name, seller, buyer, 100m);

            var dealsCount2 = _salesService.Deals.Count();

        }

        /// <summary>
        /// Действия выполняемые при закрытии основной формы
        /// </summary>
        public void OnExit()
        {
            //_projectConfigurationRepository?.Save();
        }
        /* ------------------------------------------------------------------------------------------------------------ */
        #region Commands

        #region ShowBooksViewCommand
        public ICommand ShowBooksViewCommand { get; }
        private void OnShowBooksViewCommandExecuted(object p) => CurrentModel = new BooksViewModel(_booksRepository);
        private bool CanShowBooksViewCommandExecute(object p) => true;
        #endregion

        #region ShowBuyersViewCommand
        public ICommand ShowBuyersViewCommand { get; }
        private void OnShowBuyersViewCommandExecuted(object p) => CurrentModel = new BuyersViewModel(_buyersRepository);
        private bool CanShowBuyersViewCommandExecute(object p) => true;
        #endregion

        #region ShowStatisticViewCommand
        public ICommand ShowStatisticViewCommand { get; }
        private void OnShowStatisticViewCommandExecuted(object p) => CurrentModel = new StatisticViewModel(
            _booksRepository,
            _buyersRepository,
            _sellersRepository);
        private bool CanShowStatisticViewCommandExecute(object p) => true;
        #endregion


        #region Exit
        public ICommand Exit { get; }
        private void OnExitExecuted(object p) => Application.Current.Shutdown();
        private bool CanExitExecute(object p) => true;
        #endregion

        #endregion

        /* ------------------------------------------------------------------------------------------------------------ */

        /// <summary>
        /// Заголовок окна
        /// </summary>
        public string Title { get => Get<string>(); set => Set(value); }

        /// <summary>
        /// Текущая дочерняя модель-представления
        /// </summary>
        public BaseViewModel CurrentModel { get => Get<BaseViewModel>(); set => Set(value); }

    }
}