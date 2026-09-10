using System;
using System.Collections.Generic;

namespace FinanceManagementSystem
{
    // =========================================================
    // a. Transaction Record
    // =========================================================

    public record Transaction(
        int Id,
        DateTime Date,
        decimal Amount,
        string Category
    );


    // =========================================================
    // b. Transaction Processor Interface
    // =========================================================

    public interface ITransactionProcessor
    {
        void Process(Transaction transaction);
    }


    // =========================================================
    // c. Transaction Processor Classes
    // =========================================================

    public class BankTransferProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine(
                $"Bank Transfer processed: GHC {transaction.Amount:F2} for {transaction.Category}."
            );
        }
    }


    public class MobileMoneyProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine(
                $"Mobile Money processed: GHC {transaction.Amount:F2} for {transaction.Category}."
            );
        }
    }


    public class CryptoWalletProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine(
                $"Crypto Wallet processed: GHC {transaction.Amount:F2} for {transaction.Category}."
            );
        }
    }


    // =========================================================
    // d. General Account Class
    // =========================================================

    public class Account
    {
        public string AccountNumber { get; set; }

        public decimal Balance { get; protected set; }


        // Constructor
        public Account(string accountNumber, decimal initialBalance)
        {
            AccountNumber = accountNumber;
            Balance = initialBalance;
        }


        // Virtual method
        public virtual void ApplyTransaction(Transaction transaction)
        {
            Balance -= transaction.Amount;

            Console.WriteLine(
                $"Transaction applied. New balance: GHC {Balance:F2}"
            );
        }
    }


    // =========================================================
    // e. Sealed SavingsAccount Class
    // =========================================================

    public sealed class SavingsAccount : Account
    {
        public SavingsAccount(string accountNumber, decimal initialBalance)
            : base(accountNumber, initialBalance)
        {
        }


        // Override ApplyTransaction
        public override void ApplyTransaction(Transaction transaction)
        {
            if (transaction.Amount > Balance)
            {
                Console.WriteLine("Insufficient funds");
            }
            else
            {
                Balance -= transaction.Amount;

                Console.WriteLine(
                    $"Transaction of GHC {transaction.Amount:F2} applied."
                );

                Console.WriteLine(
                    $"Updated balance: GHC {Balance:F2}"
                );
            }
        }
    }


    // =========================================================
    // f. FinanceApp Class
    // =========================================================

    public class FinanceApp
    {
        private List<Transaction> _transactions = new List<Transaction>();


        public void Run()
        {
            Console.WriteLine("======================================");
            Console.WriteLine("     FINANCE MANAGEMENT SYSTEM");
            Console.WriteLine("======================================");
            Console.WriteLine();


            // i. Create SavingsAccount
            SavingsAccount account =
                new SavingsAccount("ACC-10001", 1000m);


            Console.WriteLine($"Account Number: {account.AccountNumber}");
            Console.WriteLine($"Initial Balance: GHC {account.Balance:F2}");
            Console.WriteLine();


            // ii. Create three Transaction records

            Transaction transaction1 = new Transaction(
                1,
                DateTime.Now,
                150m,
                "Groceries"
            );


            Transaction transaction2 = new Transaction(
                2,
                DateTime.Now,
                200m,
                "Utilities"
            );


            Transaction transaction3 = new Transaction(
                3,
                DateTime.Now,
                100m,
                "Entertainment"
            );


            // iii. Create processors

            ITransactionProcessor mobileMoney =
                new MobileMoneyProcessor();

            ITransactionProcessor bankTransfer =
                new BankTransferProcessor();

            ITransactionProcessor cryptoWallet =
                new CryptoWalletProcessor();


            // iv. Process and apply transactions

            Console.WriteLine("Processing Transaction 1:");
            mobileMoney.Process(transaction1);
            account.ApplyTransaction(transaction1);

            Console.WriteLine();


            Console.WriteLine("Processing Transaction 2:");
            bankTransfer.Process(transaction2);
            account.ApplyTransaction(transaction2);

            Console.WriteLine();


            Console.WriteLine("Processing Transaction 3:");
            cryptoWallet.Process(transaction3);
            account.ApplyTransaction(transaction3);

            Console.WriteLine();


            // v. Add transactions to the list

            _transactions.Add(transaction1);
            _transactions.Add(transaction2);
            _transactions.Add(transaction3);


            // Display transaction summary

            Console.WriteLine("======================================");
            Console.WriteLine("       TRANSACTION SUMMARY");
            Console.WriteLine("======================================");

            foreach (Transaction transaction in _transactions)
            {
                Console.WriteLine(
                    $"ID: {transaction.Id} | " +
                    $"Date: {transaction.Date:dd/MM/yyyy} | " +
                    $"Amount: GHC {transaction.Amount:F2} | " +
                    $"Category: {transaction.Category}"
                );
            }


            Console.WriteLine();
            Console.WriteLine("--------------------------------------");
            Console.WriteLine(
                $"Final Balance: GHC {account.Balance:F2}"
            );
            Console.WriteLine("--------------------------------------");
        }
    }


    // =========================================================
    // Main Application
    // =========================================================

    class Program
    {
        static void Main(string[] args)
        {
            FinanceApp app = new FinanceApp();

            app.Run();

            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
