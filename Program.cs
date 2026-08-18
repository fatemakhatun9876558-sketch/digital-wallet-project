using System;
using System.Collections.Generic;
List<Transaction> transactions = new List<Transaction>();
DigitalWalletSystem system = new DigitalWalletSystem();
UniqPayInfo.ShowAppName();

// create user account
Console.Write("Enter User ID: ");
int userId = Convert.ToInt32(Console.ReadLine());

Console.Write("Enter Name: ");
string name = Console.ReadLine();
Console.Write("Enter Phone Number: ");
string phoneNumber = Console.ReadLine();
Console.Write("Enter Email: ");
string email = Console.ReadLine();
Console.Write("Enter Password: ");
string password = Console.ReadLine();
User user1 = new User(userId, name, phoneNumber, email, password);
User copiedUser = new User(user1);
Console.WriteLine("\nAccount Created Successfully!");

//create wallet
Console.Write("Enter Wallet ID: ");
int walletId = Convert.ToInt32(Console.ReadLine());
Wallet wallet1 = system.CreateWallet(walletId, user1.UserId);
Console.WriteLine("Wallet Created Successfully!");
Console.WriteLine("Wallet ID: " + wallet1.WalletId);
Console.WriteLine("User ID: " + user1.UserId);
Console.WriteLine("Balance: " + wallet1.Balance);

bool running = true;
while (running)
{
    Console.WriteLine("\n-------  MENU ---------");
    Console.WriteLine("1. Add Money");
    Console.WriteLine("2. Transfer Money");
    Console.WriteLine("3. Merchant Payment");
    Console.WriteLine("4. Check Balance");
    Console.WriteLine("5. Transaction History");
    Console.WriteLine("6. Report");
    Console.WriteLine("7. Exit");
    Console.Write("Choose an option: ");
    int choice = Convert.ToInt32(Console.ReadLine());
    switch (choice)
    {
        case 1:
            Console.Write("Enter amount to add: ");
            double amount = Convert.ToDouble(Console.ReadLine());
            wallet1.AddMoney(amount);
            Console.WriteLine("Money Added Successfully!");
            Console.WriteLine("New Balance: " + wallet1.Balance);
            break;

        case 2:
            Console.WriteLine("\nCreate Second User");
            Console.Write("Enter User ID: ");
            int userId2 = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter Name: ");
            string name2 = Console.ReadLine();
            User user2 = new User(userId2, name2);
            Console.Write("Enter Wallet ID: ");
            int walletId2 = Convert.ToInt32(Console.ReadLine());
            Wallet wallet2 = system.CreateWallet(walletId2, user2.UserId);
            Console.WriteLine("Second User and Wallet Created Successfully!");
            Console.Write("\nEnter amount to transfer: ");
            double transferAmount = Convert.ToDouble(Console.ReadLine());
            double charge = Wallet.CalculateCharge(transferAmount);
            double totalAmount = transferAmount + charge;
            if (wallet1.Balance >= totalAmount)
            {
                wallet1.Balance = wallet1.Balance - totalAmount;
                wallet2 = wallet2 + transferAmount; // Operator Overloading
                Transaction transaction1 = new Transaction();
                transaction1.TransactionId = 1;
                transaction1.Type = "Money Transfer";
                transaction1.Amount = transferAmount;
                transaction1.Charge = charge;
                transaction1.Total = totalAmount;
                transaction1.Status = "Successful";
                transactions.Add(transaction1);
                transaction1.GenerateReceipt();
                Console.WriteLine("Money Transferred Successfully!");
                Console.WriteLine("Sender Balance: " + wallet1.Balance);
                Console.WriteLine("Receiver Balance: " + wallet2.Balance);
            }
            else
            {
                Console.WriteLine("Transaction Failed! Insufficient funds.");
            }
            break;

        case 3:
            Console.WriteLine("\nCreate Merchant");
            Console.Write("Enter Merchant ID: ");
            int merchantId = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter Merchant Name: ");
            string merchantName = Console.ReadLine();
            Merchant merchant1 = new Merchant(merchantId, merchantName);
            merchant1.ShowInfo(); // Overriding
            Console.WriteLine("Merchant Created Successfully!");
            Console.Write("\nEnter payment amount: ");
            double paymentAmount = Convert.ToDouble(Console.ReadLine());
            MerchantPayment merchantPayment = new MerchantPayment();
            if (wallet1.Balance >= paymentAmount)
            {
                wallet1.Balance = wallet1.Balance - paymentAmount;
                merchant1.Balance = merchant1.Balance + paymentAmount;
                merchantPayment.MakePayment();
                Transaction transaction2 = new Transaction();
                transaction2.TransactionId = 2;
                transaction2.Type = "Merchant Payment";
                transaction2.Amount = paymentAmount;
                transaction2.Charge = 0;
                transaction2.Total = paymentAmount;
                transaction2.Status = "Successful";
                transactions.Add(transaction2);
                transaction2.GenerateReceipt();
                Console.WriteLine("Payment Successful!");
                Console.WriteLine("Your Balance: " + wallet1.Balance);
                Console.WriteLine("Merchant Balance: " + merchant1.Balance);
            }
            else
            {
                Console.WriteLine("Payment Failed! Insufficient funds.");
            }
            break;
        case 4:
            Console.WriteLine("\n--- Current Balance ---");
            Console.WriteLine("Balance: " + wallet1.Balance);
            break;

        case 5:
            Console.WriteLine("\n--- Transaction History ---");
            foreach (Transaction t in transactions)
            {
                Console.WriteLine("Transaction ID: " + t.TransactionId);
                Console.WriteLine("Type: " + t.Type);
                Console.WriteLine("Amount: " + t.Amount);
                Console.WriteLine("--------------------");
            }
            break;

        case 6:
           double totalTransactionAmount = 0;
           foreach (Transaction t in transactions)
           {
                totalTransactionAmount = totalTransactionAmount + t.Amount;
           }
           Console.WriteLine("\n--- UniqPay Report ---");
           Console.WriteLine("Total Transactions: " + transactions.Count);
           Console.WriteLine("Total Transaction Amount: " + totalTransactionAmount);
            break;

        case 7:
            running = false;
            Console.WriteLine("Thank you for using UniqPay!");
            break;

        default:
            Console.WriteLine("Invalid option! Please try again.");
            break;
    }
}
//classes
public class User
{
    public int UserId;
    public string Name;
    public string PhoneNumber;
    public string Email;
    public string Password;

    // Constructor
    public User(int userId, string name, string phoneNumber, string email, string password)
    {
        UserId = userId;
        Name = name;
        PhoneNumber = phoneNumber;
        Email = email;
        Password = password;
    }
    // Constructor Overloading
    public User(int userId, string name)
    {
        UserId = userId;
        Name = name;
        PhoneNumber = "";
        Email = "";
        Password = "";
    }
     // Copy Constructor
    public User(User oldUser)
    {
        UserId = oldUser.UserId;
        Name = oldUser.Name;
        PhoneNumber = oldUser.PhoneNumber;
        Email = oldUser.Email;
        Password = oldUser.Password;
    }
    //Virtual Method
    public virtual void ShowInfo()
    {
        Console.WriteLine("User Name: " + Name);
    }
}
public class Wallet
{
    public int WalletId;
    public int UserId;
    public double Balance;

    // Static Field
    public static double TransferChargeRate = 0.02;
    // Method
    public void AddMoney(double amount)
    {
        Balance = Balance + amount;
    }

    // Method Overloading
    public void AddMoney(int amount)
    {
        Balance = Balance + amount;
    }

    // Static Method
    public static double CalculateCharge(double amount)
    {
        return amount * TransferChargeRate;
    }
    // Operator Overloading
    public static Wallet operator +(Wallet wallet, double amount)
    {
        wallet.Balance = wallet.Balance + amount;
        return wallet;
    }
}
public class Merchant : User
{
    public int MerchantId;
    public string MerchantName;
    public double Balance;

    // Constructor
    public Merchant(int merchantId, string merchantName)
        : base(0, merchantName) // Parent User constructor call
    {
        MerchantId = merchantId;
        MerchantName = merchantName;
        Balance = 0;
    }
    // Method Overriding
    public override void ShowInfo()
    {
        Console.WriteLine("Merchant Name: " + MerchantName);
    }
}
public class Transaction : IReceipt
{
    public int TransactionId;
    public string Type;
    public double Amount;
    public double Charge;
    public double Total;
    public string Status;

// Interface Method
    public void GenerateReceipt()
    {
        Console.WriteLine("\n--- UniqPay Receipt ---");
        Console.WriteLine("Transaction ID: " + TransactionId);
        Console.WriteLine("Type: " + Type);
        Console.WriteLine("Amount: " + Amount);
        Console.WriteLine("Charge: " + Charge);
        Console.WriteLine("Total: " + Total);
        Console.WriteLine("Status: " + Status);
        Console.WriteLine("-----------------------");
    }
}
// Abstract Class
public abstract class Payment
{
    public abstract void MakePayment();
}
//Inheritance from Abstract Class
public class MerchantPayment : Payment
{
    public override void MakePayment()
    {
        Console.WriteLine("Merchant Payment Processed.");
    }
}
//Interface
public interface IReceipt
{
    void GenerateReceipt();
 }
 public class DigitalWalletSystem
{
     public Wallet CreateWallet(int walletId, int userId)
    {
        Wallet wallet = new Wallet();

        wallet.WalletId = walletId;
        wallet.UserId = userId;
        wallet.Balance = 0;

        return wallet;
    }
}
// Static Class
public static class UniqPayInfo
{
    public static void ShowAppName()
    {
        Console.WriteLine("UniqPay Digital Wallet");
    }
}