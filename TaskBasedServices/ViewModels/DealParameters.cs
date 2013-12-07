using System.Threading.Tasks;
using System.Windows.Input;
using AutoMapper;
using TaskBasedServices.Common;
using TaskBasedServices.Models;
using TaskBasedServices.Services;

namespace TaskBasedServices.ViewModels
{
    public class DealParameters
    {
        private readonly ServiceExecutor services;

        public DealParameters()
        {
            services = new ServiceExecutor();
            BuildDealCommand = new RelayCommand(OnBuildCommand);
        }

        public MoneyMarketRate MoneyMarketRate { get; set; }
        public InvestmentBoundaries InvestmentBoundaries { get; set; }
        public TradingDate TradingDate { get; set; }
        public Spot Spot { get; set; }

        public ICommand BuildDealCommand { get; private set; }

        private void OnBuildCommand()
        {
            var tasks = new Task[]
            {
                services.GetAsTask<DTOs.MoneyMarketRate>()
                        .ContinueWith(t => MoneyMarketRate = Mapper.Map<DTOs.MoneyMarketRate, MoneyMarketRate>(t.Result)),

                services.GetAsTask<DTOs.InvestmentBoundaries>()
                        .ContinueWith(t => InvestmentBoundaries = Mapper.Map<DTOs.InvestmentBoundaries, InvestmentBoundaries>(t.Result)),

                services.GetAsTask<DTOs.TradingDate>()
                        .ContinueWith(t => TradingDate = Mapper.Map<DTOs.TradingDate, TradingDate>(t.Result)),
            };

            Task.Factory.ContinueWhenAll(
                tasks,
                ts => services.GetAsTask<DTOs.Spot>()
                                .ContinueWith(t => Spot = Mapper.Map<DTOs.Spot, Spot>(t.Result)));
        }
    }
}