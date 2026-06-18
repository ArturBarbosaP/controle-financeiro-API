using MoneyAPI.Services.Interfaces;
using Quartz;

namespace MoneyAPI.Jobs
{
    public class AlterarPreLancamentoAndFaturaJob : IJob //pr_AlterarPreLancamento e pr_ResetarFatura
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<AlterarPreLancamentoAndFaturaJob> _logger;

        public AlterarPreLancamentoAndFaturaJob(IServiceScopeFactory scopeFactory, ILogger<AlterarPreLancamentoAndFaturaJob> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Job iniciado: {Hora}", DateTime.Now);

            using (var scope = _scopeFactory.CreateScope())
            {
                try
                {
                    var lancamentoService = scope.ServiceProvider.GetRequiredService<ILancamentoService>();
                    await lancamentoService.AlterarPreLancamentoAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro no Job AlterarPreLancamentoAndFaturaJob, AlterarPreLancamentoAsync");
                }
            }
            //escopos diferentes para um service nao alterar em outro
            using (var scope = _scopeFactory.CreateScope())
            {
                try
                {
                    var cartaoService = scope.ServiceProvider.GetRequiredService<ICartaoService>();
                    await cartaoService.ResetarFatura();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro no Job AlterarPreLancamentoAndFaturaJob, ResetarFatura");
                }
            }

            _logger.LogInformation("Job finalizado: {Hora}", DateTime.Now);
        }
    }
}