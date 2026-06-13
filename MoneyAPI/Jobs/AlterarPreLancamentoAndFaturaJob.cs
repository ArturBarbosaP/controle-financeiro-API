using MoneyAPI.Services.Interfaces;
using Quartz;

namespace MoneyAPI.Jobs
{
    public class AlterarPreLancamentoAndFaturaJob : IJob //pr_AlterarPreLancamento e pr_ResetarFatura
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public AlterarPreLancamentoAndFaturaJob(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var lancamentoService = scope.ServiceProvider.GetRequiredService<ILancamentoService>();
                    var cartaoService = scope.ServiceProvider.GetRequiredService<ICartaoService>();

                    await lancamentoService.AlterarPreLancamentoAsync();
                    await cartaoService.ResetarFatura();
                }
            }
            catch (Exception)
            {
                //logger
            }
        }
    }
}