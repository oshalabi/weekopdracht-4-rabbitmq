using System.Threading;

namespace ValidateCreditCardPublisherDotnet
{
    public static class ValidateCreditCard
    {
        public static void Main(string[] argv)
        {
            Run(argv);
        }

        private static void Run(string[] argv)
        {
            var validateCreditCardSubscriber = new ValidateCreditCardSubscriber();
            validateCreditCardSubscriber.Run();

            // Keep the service running
            while (true)
            {
                Thread.Sleep(1000);
            }
        }
    }
}