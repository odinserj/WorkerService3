using System;
using System.Transactions;

namespace WorkerService3
{
    public class MyBackgroundJob
    {
        private readonly TransactionScope _scope;

        public MyBackgroundJob(TransactionScope scope)
        {
            _scope = scope;
        }

        public void MyMethod()
        {
            Console.WriteLine(_scope.ToString());
        }
    }
}