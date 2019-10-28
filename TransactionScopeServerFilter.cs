using System;
using System.Transactions;
using Hangfire.Server;
using Microsoft.Extensions.DependencyInjection;

namespace WorkerService3
{
    public class TransactionScopeServerFilter : IServerFilter
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public TransactionScopeServerFilter(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public void OnPerforming(PerformingContext context)
        {
            var serviceScope = _serviceScopeFactory.CreateScope();

            context.Items["App_ServiceScope"] = serviceScope;
            context.Items["App_TransactionScope"] = serviceScope.ServiceProvider.GetRequiredService<TransactionScope>();
        }

        public void OnPerformed(PerformedContext context)
        {
            if (context.Items.TryGetValue("App_TransactionScope", out var scope1) &&
                scope1 is TransactionScope transactionScope)
            {
                if (context.Exception != null)
                {
                    // transactionScope.RollBack();
                }
            }
            else
            {
                throw new InvalidOperationException("FilterContext does not contain 'App_TransactionScope' object of TransactionScope type");
            }

            if (context.Items.TryGetValue("App_ServiceScope", out var scope2) && 
                scope2 is IServiceScope serviceScope)
            {
                serviceScope.Dispose();
            }
            else
            {
                throw new InvalidOperationException("FilterContext does not contain 'App_ServiceScope' object of IServiceScope type");
            }
        }
    }
}