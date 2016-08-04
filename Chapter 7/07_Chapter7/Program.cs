using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace _07_Chapter7
{
    class Program
    {
        static void Main(string[] args)
        {
            //the root sequence of all user input messages
            var commandSource = new Subject<ICommand>();

            //register the diagnostic output of all messages
            commandSource.Materialize().Subscribe(Console.WriteLine);

            //register validation error output
            var validables = commandSource
                //routes only validable messages
                .OfType<IValidable>()
                //convert messages into validation results
                .Select(x => ValidableObjectHelper.Validate(x));

            //filter in search of invalid messages
            validables.Where(x => !x.IsValid)
                //notify the error on the output
                .Subscribe(x => Console.WriteLine("Validation errors: {0}", string.Join(",", x.Result)));

            //filter in search of valid messages
            validables.Where(x => x.IsValid)
                //get back the command message
                .Select(x => x.Instance as ICommand)
                //routes only invoice item messages
                .OfType<AddInvoiceItem>()
                //group items per invoice
                .GroupBy(x => x.InvoiceNumber)
                .Subscribe(group => group
                    //project the message to a new shape for getting the result
                    .Select(x => new { NewItem = x, TotalPrice = x.TotalPrice })
                    //apply the accumulator function to get the result
                    .Scan((old, x) => new { NewItem = x.NewItem, TotalPrice = old.TotalPrice + x.TotalPrice })
                    //output the result
                    .Subscribe(x => Console.WriteLine("Current total amount: {0:N2}", x.TotalPrice))
                );

            //filter in search of valid messages
            validables.Where(x => x.IsValid)
                //get back the command message
                .Select(x => x.Instance as ICommand)
                //routes only new invoices or invoice updates messages
                .Where(x => x is CreateNewInvoice || x is UpdateInvoiceCustomerAddress)
                //group items per invoice
                .GroupBy(x => x is CreateNewInvoice ? (x as CreateNewInvoice).InvoiceNumber : (x as UpdateInvoiceCustomerAddress).InvoiceNumber)
                .Subscribe(group => group
                    //apply the updates to get the last state
                    //a custom "+" operator to apply updates to the original invoice
                    //is available into the CreateNewInvoice class
                    .Scan((old, x) => x is CreateNewInvoice ? x as CreateNewInvoice : (old as CreateNewInvoice) + (x as UpdateInvoiceCustomerAddress))
                    //change type
                    .OfType<CreateNewInvoice>()
                    //output the new invoice details
                    .Subscribe(x => Console.WriteLine("Available an invoice nr: {0} to {1} living in {2}", x.InvoiceNumber, x.CustomerName, x.CustomerAddress))
                );

            Console.WriteLine("Return to start saving an invoice");
            Console.ReadLine();

            var invoicenr = new Random(DateTime.Now.GetHashCode()).Next(0, 1000);
            //create a new invoice

            commandSource.OnNext(new CreateNewInvoice { InvoiceNumber = invoicenr, Date = DateTime.Now });
            //now a validation error will flow out the sequence
            Console.WriteLine("Return to continue");
            Console.ReadLine();

            //create a valid invoice
            commandSource.OnNext(new CreateNewInvoice { InvoiceNumber = invoicenr, Date = DateTime.Now.Date, CustomerName = "Mr. Red", CustomerAddress = "1234, London Road, Milan, Italy" });
            Console.WriteLine("Return to continue");
            Console.ReadLine();

            //updates the invoice customer address
            commandSource.OnNext(new UpdateInvoiceCustomerAddress { InvoiceNumber = invoicenr, CustomerAddress = "1234, Milan Road, London, UK" });
            Console.WriteLine("Return to continue");
            Console.ReadLine();

            //adds some item
            commandSource.OnNext(new AddInvoiceItem { InvoiceNumber = invoicenr, ItemCode = "WMOUSE", Price = 44.40m, Amount = 10, Description = "Wireless Mouse" });
            Console.WriteLine("Return to continue");
            Console.ReadLine();

            commandSource.OnNext(new AddInvoiceItem { InvoiceNumber = invoicenr, ItemCode = "DMOUSE", Price = 17.32m, Amount = 5, Description = "Wired Mouse" });
            Console.WriteLine("Return to continue");
            Console.ReadLine();

            commandSource.OnNext(new AddInvoiceItem { InvoiceNumber = invoicenr, ItemCode = "USBC1MT", Price = 2.00m, Amount = 100, Description = "Usb cable 1mt" });

            Console.WriteLine("END");
            Console.ReadLine();
        }
    }

    /// <summary>
    /// Represents a command message
    /// </summary>
    public interface ICommand { }

    public class CreateNewInvoice : ICommand, IValidable
    {
        [Required, Range(1, 100000)]
        public int InvoiceNumber { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required(AllowEmptyStrings = false), StringLength(50)]
        public string CustomerName { get; set; }

        [Required(AllowEmptyStrings = false), StringLength(50)]
        public string CustomerAddress { get; set; }

        //apply updates
        public static CreateNewInvoice operator +(CreateNewInvoice invoice, UpdateInvoiceCustomerAddress updater)
        {
            if (!invoice.InvoiceNumber.Equals(updater.InvoiceNumber))
                throw new ArgumentException();

            return new CreateNewInvoice
            {
                InvoiceNumber = invoice.InvoiceNumber,
                Date = invoice.Date,
                CustomerName = invoice.CustomerName,
                CustomerAddress = updater.CustomerAddress,
            };
        }
    }

    public class UpdateInvoiceCustomerAddress : ICommand, IValidable
    {
        [Required]
        public int InvoiceNumber { get; set; }

        [Required(AllowEmptyStrings = false), StringLength(50)]
        public string CustomerAddress { get; set; }
    }

    public class AddInvoiceItem : ICommand, IValidable
    {
        [Required]
        public int InvoiceNumber { get; set; }

        [Required]
        public string ItemCode { get; set; }

        [Required(AllowEmptyStrings = false), StringLength(50)]
        public string Description { get; set; }

        [Required, Range(1, 10000)]
        public int Amount { get; set; }

        [Required, Range(-10000, 10000)]
        public decimal Price { get; set; }

        public decimal TotalPrice { get { return Amount * Price; } }
    }
}
