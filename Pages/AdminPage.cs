﻿using ConsoleTables;
using Market_App.Enums;
using Market_App.IRepository;
using Market_App.Service;
using System;
using System.Data;
using System.Linq;
using Market_App.Registration;
using Market_App.Extensions;
using System.Threading;
using Market_App.Repositories;
using System.Collections.Generic;

namespace Market_App.Models
{
    internal class AdminPage
    {
        private static IProductRepository productRepo = new ProductRepository();

        private static IUserRepository userRepo = new UserRepository();

        private static IHistoryRepository historyRepo = new HistoryRepository();

        private static Sales sales = new Sales();

        public void Execute()
        {
            while (true)
            {
                MainMenu regist = new MainMenu();
                
                Console.Clear();

                Console.WriteLine("1. Browse all products | 2. Sales information | 3. Create Admin | 4. Show all users | 5. Log out | 6. Exit");
                
                Console.Write("\n> ");
                
                string choose = Console.ReadLine();

                switch (choose)
                {
                    case "1":
                        ShowProducts();
                        break;
                    case "2":
                        SalesInformation();
                        break;
                    case "3":
                        AddAdmin();
                        break;
                    case "4":
                        ShowUsers();
                        break;
                    case "5":
                        regist.Menu();
                        break;
                    case "6":
                        Environment.Exit(0);
                        break;
                    default:
                        CatchErrors.InputError();
                        Execute();
                        break;
                }
            }
        }

        private void AddAdmin()
        {
            try
            {

                Console.Write("Enter First name: ");
                string firstName = Console.ReadLine().Capitalize();

                Console.Write("Enter Last name: ");
                string lastName = Console.ReadLine().Capitalize();

                Console.Write("Enter username: ");
                string username = Console.ReadLine();

                if (!AdminInspection(username))
                {

                    Console.Write("Enter password: ");
                    string password = MethodService.HashPassword(Console.ReadLine());
                    User admin = new User
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        Role = UserRole.Admin,
                        Login = username,
                        Password = password
                    };
                    userRepo.Create(admin);

                    Console.Clear();

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Succes!\n");
                    Console.ForegroundColor = ConsoleColor.White;

                    Console.Write("Do you want add admin again? [y, n]: ");
                    string choose = Console.ReadLine();

                    if (choose == "Y" || choose == "y")
                        AddAdmin();
                    else Execute();
                }
                else
                {
                    Console.Clear();

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Such a user exists!");
                    Console.ForegroundColor = ConsoleColor.White;

                    Console.Write("\nWould you like to try again? [y, n]: ");
                    string choose = Console.ReadLine();

                    if (choose == "y" || choose == "Y") AddAdmin();
                    else Execute();
                }
            }
            catch
            {
                Console.ForegroundColor= ConsoleColor.Red;
                Console.WriteLine("Input error! Please try again.");
                Console.ForegroundColor = ConsoleColor.White;

                Thread.Sleep(1300);

                ShowUsers();
            }
        }
  
        private void ShowProducts()
        {

            Console.Clear();

            var products = productRepo.Get();

            var table = new ConsoleTable("№", "Product Name", "Price", "Unit", "Residue", "Type");

            foreach (var product in products)
            {
                table.AddRow(product.Id, product.Name, product.Price, product.Unit, product.Residue, product.Type);
            }

            products.Clear();

            OptionMenu(table);
        }
        
        private void AddProduct()
        {
            try
            {


                Console.Write("\nEnter product name: ");
                string name = Console.ReadLine().Capitalize();
                var product = productRepo.Get(name);

                if (product.Name != null)
                {
                    Console.Write("Enter price: ");
                    product.Price = decimal.Parse(Console.ReadLine());

                    Console.Write("Enter residue: ");
                    product.Residue = float.Parse(Console.ReadLine());

                    productRepo.Update(product);

                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nProduct edited.\n");
                    Console.ForegroundColor = ConsoleColor.White;
                    Thread.Sleep(1000);

                }
                else
                {
                    Product product1 = new Product();
                    
                    product1.Name = name;

                    Console.Write("Enter price: ");
                    product1.Price = decimal.Parse(Console.ReadLine());

                    Console.WriteLine("\n1. pcs\n2. kgs\n");
                    Console.Write("Choose unit: ");
                    switch (Console.ReadLine())
                    {
                        case "1":
                            product1.Unit = Unit.pcs;
                            break;
                        case "2":
                            product1.Unit = Unit.kgs;
                            break;
                        default:
                            AddProduct();
                            break;
                    }

                    Console.Write("Enter residue: ");
                    product1.Residue = float.Parse(Console.ReadLine());

                    Console.WriteLine("\n1. Fruit\n2. Vegetables\n3. Meat\n4. Drink\n5. Sweets");
                    Console.Write("Choose type: ");
                    switch (Console.ReadLine())
                    {
                        case "1":
                            product1.Type = ProductsType.fruit;
                            break;
                        case "2":
                            product1.Type = ProductsType.vegetables;
                            break;
                        case "3":
                            product1.Type = ProductsType.meat;
                            break;
                        case "4":
                            product1.Type = ProductsType.drink;
                            break;
                        case "5":
                            product1.Type = ProductsType.sweets;
                            break;
                        default:
                            AddProduct();
                            break;
                    }

                    product1.Status = ProductStatus.in_stock;

                    productRepo.Create(product1);

                    Console.WriteLine("Product added.\n");
                }

                ShowProducts();
            }
            catch
            {
                CatchErrors.InputError();
                ShowProducts();
            }
        }
        
        private void UpdateProduct()
        {

            Console.Clear();

            var products = productRepo.Get();

            var table = new ConsoleTable("№", "Product Name", "Price", "Unit", "Residue", "Type");

            foreach (var product in products)
            {
                table.AddRow(product.Id, product.Name, product.Price, product.Unit, product.Residue, product.Type);
            }

            table.Write();

            Console.WriteLine("\n1. Delete product");
            Console.WriteLine("2. Edit product");
            Console.WriteLine("3. Back to Menu");

            Console.Write("\nEnter option: ");

            int choose = int.Parse(Console.ReadLine());
            if (choose == 1)
            {
                DeleteProduct(table);
            }
            else if (choose == 2)
            {
                EditProduct(table);
            }
            else if (choose == 3)
                Execute();
            else
            {
                Console.WriteLine("You entered incorrectly! Please enter again.");
                UpdateProduct(table);
            }

        }
        
        private void UpdateProduct(ConsoleTable table)
        {
            table.Write();

            Console.WriteLine("\n1. Delete product");
            Console.WriteLine("2. Edit product");
            Console.WriteLine("3. Back to Menu");

            Console.Write("\n> ");

            string choose = Console.ReadLine();
            switch(choose)
            {
                case "1":
                    DeleteProduct(table);
                    break;
                case "2":
                    EditProduct(table);
                    break;
                case "3":
                    Execute();
                    break;
                default:
                    CatchErrors.InputError();
                    UpdateProduct(table);
                    break;
            }
        }
        
        private void SearchProduct()
        {
            try
            {

                Console.Write("\nEnter the product name: ");

                string nameProduct = Console.ReadLine();

                var products = sales.GetProductsForSelling().Where(x => x.Name.Contains(nameProduct.Capitalize())).ToList();

                if (products == null)
                {
                    products = sales.GetProductsForSelling().Where(x => x.Name.Contains(nameProduct)).ToList();
                }

                var table = new ConsoleTable("№", "Product Name", "Price", "Unit", "Residue", "Type");

                foreach (var product in products)
                {
                    table.AddRow(product.Id, product.Name, product.Price, product.Unit, product.Residue, product.Type);
                }

                OptionMenu("Search product", table);
            }
            catch
            {
                CatchErrors.InputError();

                ShowProducts();
            }
        }
        
        private void DeleteProduct(ConsoleTable table)
        {
            try
            {

                Console.Clear();

                table.Write();

                Console.Write("Enter №: ");

                var product = productRepo.Get(int.Parse(Console.ReadLine()));

                if (product != null)
                {
                    productRepo.Remove(product.Id);
                    ShowProducts();
                }
                else
                {
                    Console.WriteLine("Such a product is not available in the basket!");
                    DeleteProduct(table);
                }
            }
            catch
            {
                CatchErrors.InputError();

                UpdateProduct(table);
            }
        }
        
        private void OptionMenu(ConsoleTable table)
        {
            Console.Clear();

            table.Write();

            Console.WriteLine("\n1. Add product");
            Console.WriteLine("2. Update product");
            Console.WriteLine("3. Search product");
            Console.WriteLine("4. Back to Menu");
            Console.Write("\n> ");

            string choose = Console.ReadLine();

            switch (choose)
            {
                case "1":
                    AddProduct();
                    break;
                case "2":
                    UpdateProduct(table);
                    break;
                case "3":
                    SearchProduct();
                    break;
                case "4":
                    Execute();
                    break;
                default:
                    CatchErrors.InputError();
                    ShowProducts();
                    break;
            }
        }
        
        private void OptionMenu(string option, ConsoleTable table)
        {
            if (option == "Search product")
            {
                Console.Clear();

                table.Write();

                Console.WriteLine("\n1. Show all products");
                Console.WriteLine("2. Update product");
                Console.WriteLine("3. Delete product");
                Console.WriteLine("4. Back to Menu");
                Console.Write("\n> ");

                string choose = Console.ReadLine();

                switch (choose)
                {
                    case "1":
                        ShowProducts();
                        break;
                    case "2":
                        UpdateProduct(table);
                        break;
                    case "3":
                        DeleteProduct(table);
                        break;
                    case "4":
                        Execute();
                        break;
                    default:
                        CatchErrors.InputError();
                        OptionMenu(option, table);
                        break;
                }
            }

            else if (option == "Show all users")
            {
                Console.Clear();

                table.Write();

                Console.WriteLine("\n1. Create admin");
                Console.WriteLine("2. Edit");
                Console.WriteLine("3. Delete");
                Console.WriteLine("4. Back to Menu");
                Console.Write("\n> ");

                string choose = Console.ReadLine();

                switch (choose)
                {
                    case "1":
                        AddAdmin();
                        break;
                    case "2":
                        EditUser(table);
                        break;
                    case "3":
                        DeleteUser(table);
                        break;
                    case "4":
                        Execute();
                        break;
                    default:
                        CatchErrors.InputError();
                        OptionMenu(option, table);
                        break;
                }
            }
        }
        
        private void EditProduct(ConsoleTable table)
        {
            try
            {

                Console.Clear();

                table.Write();

                Console.Write("\nEnter №: ");

                int id = int.Parse(Console.ReadLine());

                var product = productRepo.Get().FirstOrDefault(x => x.Id.Equals(id));

                Console.Clear();

                table.Write();

                Console.WriteLine("\n1. Name | 2. Price | 3. Unit | 4. Residue | 5. Type | 6. Back ");

                Console.Write("\n> ");

                int choose = int.Parse(Console.ReadLine());

                if (product != null)
                {
                    switch (choose)
                    {
                        case 1:
                            Console.Write("Enter name: ");
                            product.Name = Console.ReadLine().Capitalize();
                            break;
                        case 2:
                            Console.Write("Enter price: ");
                            product.Price = int.Parse(Console.ReadLine());
                            break;
                        case 3:
                            Console.WriteLine("1. pcs\n2. kgs\n");
                            Console.Write("Choose unit: ");
                            switch (Console.ReadLine())
                            {
                                case "1":
                                    product.Unit = Unit.pcs;
                                    break;
                                case "2":
                                    product.Unit = Unit.kgs;
                                    break;
                                default:
                                    AddProduct();
                                    break;
                            }
                            break;
                        case 4:
                            Console.Write("Enter residue: ");
                            product.Residue = int.Parse(Console.ReadLine());
                            break;
                        case 5:
                            Console.WriteLine("1. Fruit\n2. Vegetables\n3. Meat\n4. Drink\n5. Sweets");
                            Console.Write("Choose type: ");
                            switch (Console.ReadLine())
                            {
                                case "1":
                                    product.Type = ProductsType.fruit;
                                    break;
                                case "2":
                                    product.Type = ProductsType.vegetables;
                                    break;
                                case "3":
                                    product.Type = ProductsType.meat;
                                    break;
                                case "4":
                                    product.Type = ProductsType.drink;
                                    break;
                                case "5":
                                    product.Type = ProductsType.sweets;
                                    break;
                                default:
                                    AddProduct();
                                    break;
                            }
                            break;
                        case 6:
                            UpdateProduct(table);
                            break;
                        default:
                            EditProduct(table);
                            break;
                    }

                    productRepo.Update(product);

                    UpdateProduct();

                }
            }
            catch
            {
                CatchErrors.InputError();

                UpdateProduct();
            }
        }
        
        private void ShowUsers()
        {
            var users = userRepo.GetAllUsers();

            var table = new ConsoleTable("№", "First Name", "Last Name", "Role", "Login", "Password");

            foreach (var user in users)
            {
                table.AddRow(user.Id, user.FirstName, user.LastName, user.Role, user.Login, user.Password);
            }

            OptionMenu("Show all users", table);
        }
        
        private void EditUser(ConsoleTable table)
        {
            try
            {

                Console.Clear();

                table.Write();

                Console.Write("\nEnter №: ");

                int id = int.Parse(Console.ReadLine());

                var admin = userRepo.GetUser(id);

                Console.Clear();

                table.Write();

                Console.WriteLine("\n1. First Name | 2. Last Name | 3. Login | 4. Password | 5. Back ");

                Console.Write("\n> ");

                int choose = int.Parse(Console.ReadLine());

                if (admin != null)
                {
                    switch (choose)
                    {
                        case 1:
                            Console.Write("First name: ");
                            admin.FirstName = Console.ReadLine().Capitalize();
                            break;
                        case 2:
                            Console.Write("Last name: ");
                            admin.LastName = Console.ReadLine().Capitalize();
                            break;
                        case 3:
                            Console.Write("Login: ");
                            admin.Login = Console.ReadLine();
                            break;
                        case 4:
                            Console.Write("Password: ");
                            admin.Password = MethodService.HashPassword(Console.ReadLine());
                            break;
                        default:
                            EditUser(table);
                            break;
                    }
                    userRepo.EditUser(admin);
                    ShowUsers();
                }
            }
            catch
            {
                CatchErrors.InputError();

                ShowUsers();
            }
        }
        
        private void DeleteUser(ConsoleTable table)
        {
            try
            {

                Console.Clear();

                table.Write();

                Console.Write("Enter №: ");

                int id = int.Parse(Console.ReadLine());

                User admin = userRepo.GetUser(id);

                if (admin != null)
                {
                    userRepo.RemoveUser(admin);
                    ShowUsers();
                }
                else
                {
                    Console.WriteLine("Such a product is not available in the basket!");
                    DeleteUser(table);
                }
            }
            catch
            {
                CatchErrors.InputError();

                ShowUsers();
            }
        }
        
        private bool AdminInspection(string username)
        {
            return userRepo.GetAllUsers().Any(x => x.Login == username);
        }

        private void SalesInformation()
        {
            ConsoleTable table = new ConsoleTable("#", "Customer", "Product name", "Product price", "Product residue", "Summ", "Date");
            var histories = historyRepo.GetHistories();
            decimal totalSumm = 0;

            for (int i = 0; i < histories.Count; i++)
            {
                if (histories[i].CustomerId.Equals(histories[i].CustomerId))
                {
                    table.AddRow(
                        i + 1,
                    histories[i].CustomerFullName,
                    histories[i].ProductName,
                    histories[i].ProductPrice,
                    histories[i].Quantity,
                    histories[i].ProductPrice * (decimal)histories[i].Quantity,
                    histories[i].Date);
                }
                else
                {
                    table.AddRow(
                  " ",
                  "" ,
                  histories[i].ProductName,
                  histories[i].ProductPrice,
                  histories[i].Quantity,
                  histories[i].ProductPrice * (decimal)histories[i].Quantity, " ");
                }

                totalSumm += histories[i].ProductPrice * (decimal)histories[i].Quantity;
            }

            table.AddRow(" ", " ", " ", " ", "Total summ: ", totalSumm, " ");

            Console.Clear();

            table.Write();

            Console.WriteLine("\n0. Back\n");

            Console.Write("> ");

            Console.ReadLine();
        }

    }
}
