// Copyright © Microsoft Corporation.  All Rights Reserved.
// This code released under the terms of the 
// Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)
//
//Copyright (C) Microsoft Corporation.  All rights reserved.

using System;
using System.Linq;
using SampleSupport;
using Task.Data;

// Version Mad01

namespace SampleQueries
{
    [Title("LINQ Module")]
    [Prefix("Linq")]
    public class LinqSamples : SampleHarness
    {

        private DataSource dataSource = new DataSource();

        [Category("Restriction Operators")]
        [Title("Where - Task 1")]
        [Description("This sample uses the where clause to find all elements of an array with a value less than 5.")]
        public void Linq1()
        {
            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            var lowNums =
                from num in numbers
                where num < 5
                select num;

            Console.WriteLine("Numbers < 5:");
            foreach (var x in lowNums)
            {
                Console.WriteLine(x);
            }
        }

        [Category("Restriction Operators")]
        [Title("Where - Task 2")]
        [Description("This sample return return all presented in market products")]

        public void Linq2()
        {
            var products =
                from p in dataSource.Products
                where p.UnitsInStock > 0
                select p;

            foreach (var p in products)
            {
                ObjectDumper.Write(p);
            }
        }

        [Category("Homework")]
        [Title("Task-1")]
        [Description("Выдайте список всех клиентов, чей суммарный оборот (сумма всех заказов) превосходит некоторую величину X")]
        public void Linq001()
        {
            var x = 5000;
            var customers =
                from client in dataSource.Customers
                where client.Orders.Sum(order => order.Total) > x
                select new
                {
                    Name = client.CompanyName,
                    Sum = client.Orders.Sum(order => order.Total)
                };

            foreach (var client in customers)
            {
                Console.WriteLine(String.Format("{0}\t:\t{1}", client.Name, client.Sum));
            }
        }

        [Category("Homework")]
        [Title("Task-1-Ext")]
        [Description("Выдайте список всех клиентов, чей суммарный оборот (сумма всех заказов) превосходит некоторую величину X")]
        public void Linq001e()
        {
            var x = 5000;
            var customers = dataSource.Customers
                .Where(c => c.Orders.Sum(o => o.Total) > x)
                .Select(c => new
                {
                    Name = c.CompanyName,
                    Sum = c.Orders.Sum(o => o.Total)
                });

            foreach (var client in customers)
            {
                Console.WriteLine(String.Format("{0}\t:\t{1}", client.Name, client.Sum));
            }
        }

        [Category("Homework")]
        [Title("Task-2 without group")]
        [Description("Для каждого клиента составьте список поставщиков, находящихся в той же стране и том же городе. Сделайте задания с использованием группировки и без.")]
        public void Linq002_1()
        {
            var suppliers =
                        from client in dataSource.Customers
                        from supplier in dataSource.Suppliers
                        where supplier.Country == client.Country && supplier.City == client.City
                        select new
                        {
                            Client = client.CompanyName,
                            Supplier = supplier.SupplierName,
                            City = client.City
                        };
            foreach (var supplier in suppliers)
            {
                Console.WriteLine(String.Format("{0}\t:\t{1}:\t{2}", supplier.City, supplier.Client, supplier.Supplier));
            }
        }

        [Category("Homework")]
        [Title("Task-2 without group Ext")]
        [Description("Для каждого клиента составьте список поставщиков, находящихся в той же стране и том же городе. Сделайте задания с использованием группировки и без.")]
        public void Linq002_1e()
        {
            var suppliers = dataSource.Customers
               .Select(c => new
               {
                   Client = c,
                   Supplier = dataSource.Suppliers.Where(s => s.City == c.City && s.Country == c.Country).FirstOrDefault(),
                   City = c.City
               }).Where(x => x.Supplier != null);

            foreach (var supplier in suppliers)
            {
                Console.WriteLine(String.Format("{0}\t:\t{1}:\t{2}", supplier.City, supplier.Client.CompanyName, supplier.Supplier.SupplierName));
            }
        }

        [Category("Homework")]
        [Title("Task-2 with group")]
        [Description("Для каждого клиента составьте список поставщиков, находящихся в той же стране и том же городе. Сделайте задания с использованием группировки и без.")]
        public void Linq002_2()
        {
            var cities =
                        from client in dataSource.Customers
                        from supplier in dataSource.Suppliers
                        where supplier.Country == client.Country && supplier.City == client.City
                        group new { Client = client.CompanyName, Supplier = supplier.SupplierName } by client.City into groupped
                        select new
                        {
                            City = groupped.Key,
                            Clients = groupped.ToList()
                        };
            foreach (var city in cities)
            {
                Console.WriteLine(String.Format("{0}", city.City));
                foreach (var client in city.Clients)
                {
                    Console.WriteLine(String.Format("\t{0}:\t{1}", client.Client, client.Supplier));
                }
            }
        }

        [Category("Homework")]
        [Title("Task-2 with group Ext")]
        [Description("Для каждого клиента составьте список поставщиков, находящихся в той же стране и том же городе. Сделайте задания с использованием группировки и без.")]
        public void Linq002_2e()
        {
            var cities = dataSource.Suppliers.GroupJoin(dataSource.Customers,
                s => new { s.City, s.Country },
                c => new { c.City, c.Country },
                (s, c) => new { Supplier = s, Customers = c });
            foreach (var city in cities)
            {
                Console.WriteLine(String.Format("{0}", city.Supplier.City));
                foreach (var client in city.Customers)
                {
                    Console.WriteLine(String.Format("\t{0}:\t{1}", client.CompanyName, city.Supplier.SupplierName));
                }
            }
        }

        [Category("Homework")]
        [Title("Task-3")]
        [Description("Найдите всех клиентов, у которых были заказы, превосходящие по сумме величину X")]
        public void Linq003()
        {
            var x = 5000;
            var customers =
                from client in dataSource.Customers
                where client.Orders.Sum(order => order.Total) > x
                select new
                {
                    Name = client.CompanyName,
                    Sum = client.Orders.Sum(order => order.Total)
                };

            foreach (var client in customers)
            {
                Console.WriteLine(String.Format("{0}\t:\t{1}", client.Name, client.Sum));
            }
        }

        [Category("Homework")]
        [Title("Task-3 Ext")]
        [Description("Найдите всех клиентов, у которых были заказы, превосходящие по сумме величину X")]
        public void Linq003e()
        {
            var x = 5000;
            var customers = dataSource.Customers.Select(c => new
            {
                Name = c.CompanyName,
                Sum = c.Orders.Sum(order => order.Total)
            }).Where(c => c.Sum > x);

            foreach (var client in customers)
            {
                Console.WriteLine(String.Format("{0}\t:\t{1}", client.Name, client.Sum));
            }
        }

        [Category("Homework")]
        [Title("Task-4")]
        [Description("Выдайте список клиентов с указанием, начиная с какого месяца какого года они стали клиентами (принять за таковые месяц и год самого первого заказа)")]
        public void Linq004()
        {
            var customers =
                from client in dataSource.Customers
                from order in client.Orders
                orderby order.OrderDate
                group new { Client = client.CompanyName, Date = order.OrderDate } by client.CompanyName into groupped
                select new
                {
                    Name = groupped.Key,
                    Date = groupped.First().Date
                };

            foreach (var client in customers)
            {
                Console.WriteLine(String.Format("{0}\t:{1}:{2}", client.Name, client.Date.Month, client.Date.Year));
            }
        }

        [Category("Homework")]
        [Title("Task-4 Ext")]
        [Description("Выдайте список клиентов с указанием, начиная с какого месяца какого года они стали клиентами (принять за таковые месяц и год самого первого заказа)")]
        public void Linq004e()
        {
            var customers = dataSource.Customers.Where(c => c.Orders.Any())
                .Select(c => new
                {
                    Name = c.CompanyName,
                    Date = c.Orders.OrderBy(o => o.OrderDate).Select(o => o.OrderDate).First()
                });

            foreach (var client in customers)
            {
                Console.WriteLine(String.Format("{0}\t:{1}:{2}", client.Name, client.Date.Month, client.Date.Year));
            }
        }


        [Category("Homework")]
        [Title("Task-5")]
        [Description("Сделайте предыдущее задание, но выдайте список отсортированным по году, месяцу, оборотам клиента (от максимального к минимальному) и имени клиента")]
        public void Linq005()
        {
            var customers =
                from client in dataSource.Customers
                from order in client.Orders
                orderby order.OrderDate descending, client.Orders.Sum(o => o.Total) descending, client.CompanyName
                group new { Client = client.CompanyName, Date = order.OrderDate, Sum = client.Orders.Sum(o => o.Total) } by client.CompanyName into groupped
                select new
                {
                    Name = groupped.Key,
                    Date = groupped.Last().Date
                };

            foreach (var client in customers)
            {
                Console.WriteLine(String.Format("{0}\t:{1}:{2}", client.Name, client.Date.Month, client.Date.Year));
            }
        }

        [Category("Homework")]
        [Title("Task-5 Ext")]
        [Description("Сделайте предыдущее задание, но выдайте список отсортированным по году, месяцу, оборотам клиента (от максимального к минимальному) и имени клиента")]
        public void Linq005e()
        {
            var customers = dataSource.Customers.Where(c => c.Orders.Any())
                .Select(c => new
                {
                    Name = c.CompanyName,
                    Date = c.Orders.OrderBy(o => o.OrderDate).Select(o => o.OrderDate).First(),
                    Sum = c.Orders.Sum(o => o.Total)
                }).OrderByDescending(c => c.Date.Year)
                .ThenByDescending(c => c.Date.Month)
                .ThenByDescending(c => c.Sum)
                .ThenByDescending(c => c.Name);


            foreach (var client in customers)
            {
                Console.WriteLine(String.Format("{0}\t:{1}:{2}", client.Name, client.Date.Month, client.Date.Year));
            }
        }

        [Category("Homework")]
        [Title("Task-6")]
        [Description("Укажите всех клиентов, у которых указан нецифровой почтовый код или не заполнен регион или в телефоне не указан код оператора (для простоты считаем, что это равнозначно «нет круглых скобочек в начале»")]
        public void Linq006()
        {
            var customers =
                from client in dataSource.Customers
                where client.Phone[0] != '(' || IsDigitsOnly(client.PostalCode) || string.IsNullOrWhiteSpace(client.Region)
                select client.CompanyName;

            foreach (var client in customers)
            {
                Console.WriteLine(client);
            }
        }

        [Category("Homework")]
        [Title("Task-6 Ext")]
        [Description("Укажите всех клиентов, у которых указан нецифровой почтовый код или не заполнен регион или в телефоне не указан код оператора (для простоты считаем, что это равнозначно «нет круглых скобочек в начале»")]
        public void Linq006e()
        {
            var customers = dataSource.Customers
                .Where(client => client.Phone[0] != '(' || IsDigitsOnly(client.PostalCode) || string.IsNullOrWhiteSpace(client.Region))
                .Select(client => client.CompanyName);

            foreach (var client in customers)
            {
                Console.WriteLine(client);
            }
        }

        private bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

        [Category("Homework")]
        [Title("Task-7")]
        [Description("Сгруппируйте все продукты по категориям, внутри – по наличию на складе, внутри последней группы отсортируйте по стоимости")]
        public void Linq007()
        {
            var products =
                from product in dataSource.Products
                orderby product.UnitPrice
                group new { product.Category, InStock = product.UnitsInStock > 0 ? "In stock" : "Out of stock", product.ProductName, product.UnitPrice } by new { product.Category, IsInStock = product.UnitsInStock > 0 } into groupped
                from grouppedProduct in groupped
                select new
                {
                    grouppedProduct.ProductName,
                    grouppedProduct.InStock,
                    grouppedProduct.Category,
                    grouppedProduct.UnitPrice

                };

            foreach (var product in products)
            {
                Console.WriteLine(String.Format("{0}\t:{1}\t:{2}:{3}", product.Category, product.ProductName, product.InStock, product.UnitPrice));
            }
        }

        [Category("Homework")]
        [Title("Task-7 Ext")]
        [Description("Сгруппируйте все продукты по категориям, внутри – по наличию на складе, внутри последней группы отсортируйте по стоимости")]
        public void Linq007e()
        {
            var products = dataSource.Products
                .GroupBy(p => p.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    ProductsByStock = g.GroupBy(p => p.UnitsInStock > 0)
                        .Select(a => new
                        {
                            InStock = a.Key ? "In stock" : "Out of stock",
                            Products = a.OrderBy(prod => prod.UnitPrice)
                        })
                });

            foreach (var category in products)
            {
                foreach(var stock in category.ProductsByStock)
                {
                    foreach (var product in stock.Products)
                    {
                        Console.WriteLine(String.Format("{0}\t:{1}\t:{2}:{3}", product.Category, product.ProductName, stock.InStock, product.UnitPrice));
                    }              
                }             
            }
        }


        [Category("Homework")]
        [Title("Task-8")]
        [Description("Сгруппируйте товары по группам «дешевые», «средняя цена», «дорогие». Границы каждой группы задайте сами")]
        public void Linq008()
        {
            var cheap = 30;
            var normal = 100;
            var products =
                from product in dataSource.Products
                group new { product.ProductName, Price = product.UnitPrice > cheap ? (product.UnitPrice > normal ? "Expensive" : "Normal") : "Cheap" } by product.UnitPrice > cheap ? (product.UnitPrice > normal ? "Expensive" : "Normal") : "Cheap" into groupped
                from grouppedProduct in groupped
                select new
                {
                    grouppedProduct.ProductName,
                    grouppedProduct.Price

                };

            foreach (var product in products)
            {
                Console.WriteLine(String.Format("{0}\t:{1}", product.ProductName, product.Price));
            }
        }

        [Category("Homework")]
        [Title("Task-8 Ext")]
        [Description("Сгруппируйте товары по группам «дешевые», «средняя цена», «дорогие». Границы каждой группы задайте сами")]
        public void Linq008e()
        {
            var cheap = 30;
            var normal = 100;
            var products = dataSource.Products
                .GroupBy(p => p.UnitPrice < cheap ? "Cheap"
                    : p.UnitPrice < normal ? "Normal" : "Expensive");

            foreach (var group in products)
            {
                foreach (var product in group)
                {
                    Console.WriteLine(String.Format("{0}\t:{1}", product.ProductName, product.UnitPrice));
                }
            }
        }

        [Category("Homework")]
        [Title("Task-9")]
        [Description("Рассчитайте среднюю прибыльность каждого города (среднюю сумму заказа по всем клиентам из данного города) и среднюю интенсивность (среднее количество заказов, приходящееся на клиента из каждого города)")]
        public void Linq009()
        {
            var cities =
                from customer in dataSource.Customers
                from order in customer.Orders
                group new { order, count = customer.Orders.Count() } by customer.City into orders
                select new
                {
                    orders.Key,
                    Average = orders.Average(x => x.order.Total),
                    Intensity = orders.Average(x => x.count)
                };
            foreach (var city in cities)
            {
                Console.WriteLine(String.Format("{0}\t:{1}\t:{2}", city.Key, city.Average, city.Intensity));
            }
        }

        [Category("Homework")]
        [Title("Task-9 Ext")]
        [Description("Рассчитайте среднюю прибыльность каждого города (среднюю сумму заказа по всем клиентам из данного города) и среднюю интенсивность (среднее количество заказов, приходящееся на клиента из каждого города)")]
        public void Linq009e()
        {
            var cities = dataSource.Customers
                .GroupBy(c => c.City)
                .Select(c => new
                {
                    City = c.Key,
                    Intensity = c.Average(p => p.Orders.Length),
                    Average = c.Average(p => p.Orders.Average(o => o?.Total))
                });
            foreach (var city in cities)
            {
                Console.WriteLine(String.Format("{0}\t:{1}\t:{2}", city.City, city.Average, city.Intensity));
            }
        }

        [Category("Homework")]
        [Title("Task-10")]
        [Description("Сделайте среднегодовую статистику активности клиентов по месяцам (без учета года), статистику по годам, по годам и месяцам (т.е. когда один месяц в разные годы имеет своё значение)")]
        public void Linq010()
        {
            var statistics =
                from customer in dataSource.Customers
                select new
                {
                    customer.CustomerID,
                    MonthsStatistic =
                        from order in customer.Orders
                        group order by order.OrderDate.Month into byMonths
                        select new
                        {
                            Month = byMonths.Key,
                            OrdersCount = byMonths.Count()
                        },

                    YearsStatistic =
                        from order in customer.Orders
                        group order by order.OrderDate.Year into byYears
                        select new
                        {
                            Year = byYears.Key,
                            OrdersCount = byYears.Count()
                        },

                    YearMonthStatistic =
                        from order in customer.Orders
                        group order by new
                        {
                            order.OrderDate.Year,
                            order.OrderDate.Month
                        } into byYearMonth
                        select new
                        {
                            byYearMonth.Key.Year,
                            byYearMonth.Key.Month,
                            OrdersCount = byYearMonth.Count()
                        }
                };
            foreach (var record in statistics)
            {
                Console.WriteLine(String.Format("{0}", record.CustomerID));
                Console.WriteLine("\tMonths statistic:\n");
                foreach (var ms in record.MonthsStatistic)
                {
                    Console.WriteLine(String.Format("\t\t:{0}:{1}", ms.Month, ms.OrdersCount));
                }
                Console.WriteLine("\tYears statistic:\n");
                foreach (var ys in record.YearsStatistic)
                {
                    Console.WriteLine(String.Format("\t\t:{0}:{1}", ys.Year, ys.OrdersCount));
                }
                Console.WriteLine("\tYear and month statistic:\n");
                foreach (var ym in record.YearMonthStatistic)
                {
                    Console.WriteLine(String.Format("\t\t:{0}-{1}:{2}", ym.Year, ym.Month, ym.OrdersCount));
                }
            }
        }

        [Category("Homework")]
        [Title("Task-10 Ext")]
        [Description("Сделайте среднегодовую статистику активности клиентов по месяцам (без учета года), статистику по годам, по годам и месяцам (т.е. когда один месяц в разные годы имеет своё значение)")]
        public void Linq010e()
        {
            var statistics = dataSource.Customers
                           .Select(c => new
                           {
                               c.CustomerID,
                               MonthsStatistic = c.Orders.GroupBy(o => o.OrderDate.Month)
                                                   .Select(g => new { Month = g.Key, OrdersCount = g.Count() }),
                               YearsStatistic = c.Orders.GroupBy(o => o.OrderDate.Year)
                                                   .Select(g => new { Year = g.Key, OrdersCount = g.Count() }),
                               YearMonthStatistic = c.Orders
                                                   .GroupBy(o => new { o.OrderDate.Year, o.OrderDate.Month })
                                                   .Select(g => new { g.Key.Year, g.Key.Month, OrdersCount = g.Count() })
                           });
            foreach (var record in statistics)
            {
                Console.WriteLine(String.Format("{0}", record.CustomerID));
                Console.WriteLine("\tMonths statistic:\n");
                foreach (var ms in record.MonthsStatistic)
                {
                    Console.WriteLine(String.Format("\t\t:{0}:{1}", ms.Month, ms.OrdersCount));
                }
                Console.WriteLine("\tYears statistic:\n");
                foreach (var ys in record.YearsStatistic)
                {
                    Console.WriteLine(String.Format("\t\t:{0}:{1}", ys.Year, ys.OrdersCount));
                }
                Console.WriteLine("\tYear and month statistic:\n");
                foreach (var ym in record.YearMonthStatistic)
                {
                    Console.WriteLine(String.Format("\t\t:{0}-{1}:{2}", ym.Year, ym.Month, ym.OrdersCount));
                }
            }
        }

    }
}
