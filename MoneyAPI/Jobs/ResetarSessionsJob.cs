using MoneyAPI.Data;
using Quartz;

namespace MoneyAPI.Jobs
{
    public class ResetarSessionsJob : IJob
    {
        private readonly Session _session;
        private readonly ILogger<ResetarSessionsJob> _logger;

        public ResetarSessionsJob(ILogger<ResetarSessionsJob> logger, Session session)
        {
            _session = session;
            _logger = logger;
        }

        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                _logger.LogInformation("Job ResetarSessionsJob iniciado: {Hora}", DateTime.Now);

                _session.LimparSessoes();

                _logger.LogInformation("Job ResetarSessionsJob iniciado: {Hora}", DateTime.Now);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no Job ResetarSessionsJob");
            }

            return Task.CompletedTask;
        }
    }
}