using System.Threading;
using System.Threading.Tasks;
using Application;
using Microsoft.Azure.WebJobs;

namespace AzureFunctionApplication
{
    public class ScheduledRandomApiRequest
    {
        public ScheduledRandomApiRequest(IRandomApiRequestManager randomApiRequestManager)
        {
            _randomApiRequestManager = randomApiRequestManager;
        }

        [FunctionName("ScheduledRandomApiRequest")]
        public async Task RunAsync([TimerTrigger("0 */1 * * * *", RunOnStartup = true)] TimerInfo myTimer, CancellationToken cancellationToken)
        {
            await _randomApiRequestManager.ProcessRequestAsync(cancellationToken);
        }

        #region Fields
        private readonly IRandomApiRequestManager _randomApiRequestManager;
        #endregion
    }
}