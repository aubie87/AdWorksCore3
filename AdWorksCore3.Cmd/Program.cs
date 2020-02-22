using AdWorksCore3.Infrastructure.Context;
using System;
using System.Linq;

namespace AdWorksCore3.Cmd
{
    class Program
    {
        static void Main(string[] args)
        {
            AdWorksContext ctx = new AdWorksContext();
            foreach (var customer in ctx.Customer)
            {
                Console.WriteLine($"{customer.CustomerId}: {customer.FirstName} {customer.LastName}");
            }

            Console.WriteLine($"There are {ctx.Customer.Count()} active customers");
        }
    }
}
