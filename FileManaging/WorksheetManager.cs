using System;
using System.IO;
using ClosedXML.Excel;
using WindowsFormsAppMySql.Database.Entities;
using System.Collections.Generic;
using System.Globalization;
using System.Diagnostics;
using System.Linq;
using Castle.Core.Internal;
using System.Windows.Forms;
using WindowsFormsAppMySql.Database;
using DocumentFormat.OpenXml.Spreadsheet;


namespace WindowsFormsAppMySql.FileManaging
{
    public static class WorksheetManager
    {
        private static string filePath = Path.Combine(@"G:\", "Mój dysk", "baza_danych");

        public static void createInstallationsFile(List<Installation> list)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Montaże");

                worksheet.Cell(1, 1).Value = "Data dostawy";
                worksheet.Cell(1, 2).Value = "Klient";
                worksheet.Cell(1, 3).Value = "Telefon";
                worksheet.Cell(1, 4).Value = "Adres";
                worksheet.Cell(1, 5).Value = "Stan";
                worksheet.Cell(1, 6).Value = "Towar";
                worksheet.Cell(1, 7).Value = "Ilość sztuk";
                worksheet.Cell(1, 8).Value = "Obwód (mb)";
                worksheet.Cell(1, 9).Value = "Opiekun";
                worksheet.Cell(1, 10).Value = "Opis";
                worksheet.Cell(1, 11).Value = "Data montażu";
                worksheet.Cell(1, 12).Value = "Montażysta";
                worksheet.Cell(1, 13).Value = "Protokół";


                var headerRange = worksheet.Range(1, 1, 1, 13);
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                headerRange.Style.Border.OutsideBorderColor = XLColor.Gray;

                int line = 0;

                for (int i=0; i< list.Count; i++)
                {
                    Installation installation = list[i];

                    worksheet.Cell(line + 2, 1).Value = dateTimeToString(installation.order.delivery_date);
                    worksheet.Cell(line + 2, 2).Value = installation.order.client.DisplayNameOnly;
                    worksheet.Cell(line + 2, 3).Value = installation.client.phone_number;
                    worksheet.Cell(line + 2, 4).Value = installation.order.installation_address;
                    worksheet.Cell(line + 2, 5).Value = installation.order.state;
                    worksheet.Cell(line + 2, 6).Value = installation.order.product.name;
                    worksheet.Cell(line + 2, 7).Value = installation.order.instance;
                    worksheet.Cell(line + 2, 8).Value = priceToString(installation.order.squared_meters);
                    worksheet.Cell(line + 2, 9).Value = installation.order.employee.first_name;
                    worksheet.Cell(line + 2, 10).Value = installation.notes;
                    worksheet.Cell(line + 2, 11).Value = dateTimeToString(installation.date);
                    worksheet.Cell(line + 2, 12).Value = string.Join("\n", installation.Employees.Select(e => e.first_name.ToUpper()));
                    worksheet.Cell(line + 2, 13).Value = "";
                    
                    
                    worksheet.Cell(line + 2, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(line + 2, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(line + 2, 12).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;


                    worksheet.Row(line + 2).AdjustToContents(10);

                    line++;


                    for (int j = 1; j <= 13; j++)
                    {
                        var cell = worksheet.Cell(i + 2, j);
                        cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        cell.Style.Border.OutsideBorderColor = XLColor.Gray;
                    }
                }

                worksheet.Columns().AdjustToContents();

                workbook.SaveAs(filePath + "\\montaże.xlsx");
                OpenExcelFile(filePath + "\\montaże.xlsx");
            }
        }

        public static void createInstallationsEmployeesFile(List<Installation> list, List<Employee> listE)
        {
            List<Payment> payments = new List<Payment>();

            using(var context = new MyDbConnection())
            {
                payments = context.Payments.ToList();
            }

            using (var workbook = new XLWorkbook())
            {
                for (int i = 0; i < listE.Count; i++)
                {
                    // Tworzenie nowego arkusza dla każdego pracownika
                    var worksheet = workbook.Worksheets.Add(listE[i].first_name);

                    // Dodanie nagłówków kolumn
                    worksheet.Cell(1, 1).Value = "Klient";
                    worksheet.Cell(1, 2).Value = "Adres";
                    worksheet.Cell(1, 3).Value = "Stan";
                    worksheet.Cell(1, 4).Value = "Towar";
                    worksheet.Cell(1, 5).Value = "Ilość sztuk";
                    worksheet.Cell(1, 6).Value = "Obwód (mb)";
                    worksheet.Cell(1, 7).Value = "Opiekun";
                    worksheet.Cell(1, 8).Value = "Opis";
                    worksheet.Cell(1, 9).Value = "Montaż brutto";
                    worksheet.Cell(1, 10).Value = "Stawka VAT";
                    worksheet.Cell(1, 11).Value = "Montaż netto";
                    worksheet.Cell(1, 12).Value = "Data montażu";
                    worksheet.Cell(1, 13).Value = "Montażysta";
                    worksheet.Cell(1, 14).Value = "Protokół";
                    worksheet.Cell(1, 15).Value = "Stawka za metr";

                    // Ustawienie stylu nagłówków
                    var headerRange = worksheet.Range(1, 1, 1, 15);
                    headerRange.Style.Font.Bold = true;
                    headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    headerRange.Style.Border.OutsideBorderColor = XLColor.Gray;

                    int line = 0;

                    // Filtracja instalacji dla danego pracownika
                    List<Installation> installations = list.Where(inst => inst.Employees.FirstOrDefault(e => e.id == listE[i].id) != null).OrderBy(inst => inst.date).ToList();

                    if (installations.Count != 0)
                    {
                        for (int j = 0; j < installations.Count; j++)
                        {
                            Installation installation = installations[j];

                            worksheet.Cell(line + 2, 1).Value = installation.order.client.DisplayNameOnly;
                            worksheet.Cell(line + 2, 2).Value = installation.order.installation_address;
                            worksheet.Cell(line + 2, 3).Value = installation.order.state;
                            worksheet.Cell(line + 2, 4).Value = installation.order.product.name;
                            worksheet.Cell(line + 2, 5).Value = installation.order.instance;
                            worksheet.Cell(line + 2, 6).Value = priceToString(installation.order.squared_meters);
                            worksheet.Cell(line + 2, 7).Value = installation.order.employee.first_name;
                            worksheet.Cell(line + 2, 8).Value = installation.notes;

                            Payment p = payments.FirstOrDefault(pay => pay.order_id == installation.order_id);
                            double gross = p.gross_installation_price;
                            double divide = ((double)p.var_rate/100) + 1;
                            double net = (gross / divide);

                            worksheet.Cell(line + 2, 9).Value = priceToString(gross); 
                            worksheet.Cell(line + 2, 10).Value = p.var_rate + "%";
                            worksheet.Cell(line + 2, 11).Value = priceToString(net);           

                            worksheet.Cell(line + 2, 12).Value = dateTimeToString(installation.date);
                            worksheet.Cell(line + 2, 13).Value = string.Join("\n", installation.Employees.Select(e => e.first_name.ToUpper()));
                           
                            worksheet.Cell(line + 2, 14).Value = "";
                            worksheet.Cell(line + 2, 15).Value = "";

                            worksheet.Row(line + 2).AdjustToContents(10);

                            line++;
                        }
                        line += 2;
                    }

                    worksheet.Columns().AdjustToContents();
                }

                // Zapisz plik Excela
                workbook.SaveAs(filePath + "\\montażyści.xlsx");
                OpenExcelFile(filePath + "\\montażyści.xlsx");
            }
        }


        public static void createDebtorsList(List<Bill> listB, List<Payment> listP, List<Order> listO)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Arkusz1");


                worksheet.Cell(1, 1).Value = "Wartość zadłużenia";
                worksheet.Cell(1, 2).Value = "Klient";
                worksheet.Cell(1, 3).Value = "Telefon";
                worksheet.Cell(1, 4).Value = "E-mail";
                worksheet.Cell(1, 5).Value = "Nr zamówienia";
                worksheet.Cell(1, 6).Value = "Produkt";
                worksheet.Cell(1, 7).Value = "Opis";
                worksheet.Cell(1, 8).Value = "Cena [towar]";
                worksheet.Cell(1, 9).Value = "Zaliczki [towar]";
                worksheet.Cell(1, 10).Value = "Pozostało [towar]";
                worksheet.Cell(1, 11).Value = "Cena [montaż]";
                worksheet.Cell(1, 12).Value = "Zaliczki [montaż]";
                worksheet.Cell(1, 13).Value = "Pozostało [montaż]";


                var headerRange = worksheet.Range(1, 1, 1, 13);
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                headerRange.Style.Border.OutsideBorderColor = XLColor.Gray;
                headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#B692C2");

                int rowNr = 2;

                for(int i = 0; i < listB.Count; i++)
                {
                    Bill bill = listB[i];
                    
                    Payment payment = listP.FirstOrDefault(p => p.bill_id == bill.id);
                    if (payment != null)
                    {
                        double price = ((double)bill.total_gross_price - (double)bill.sum_of_advances);
                        worksheet.Cell(i + rowNr, 1).Value = price.ToString() + "   ";
                        worksheet.Cell(i + rowNr, 2).Value = payment.order.client.DisplayNameOnly;
                        worksheet.Cell(i + rowNr, 3).Value = payment.order.client.phone_number + "   ";
                        worksheet.Cell(i + rowNr, 4).Value = payment.order.client.email + "   ";

                        for (int k = 1; k <= 13; k++)
                        {
                            var cell = worksheet.Cell(i + rowNr, k);
                            cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            cell.Style.Border.OutsideBorderColor = XLColor.Gray;
                            //cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#e9deec");
                            cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#cbb2d4");
                        }
                    }

                    List<Payment> paymentList = listP.Where(p => p.bill_id == bill.id).ToList();

                    rowNr += 1;

                    if (paymentList != null && paymentList.Any())
                    {
                        for (int j = 0; j < paymentList.Count; j++)
                        {
                            Order order = listO.FirstOrDefault(p => p.id == paymentList[j].order_id);

                            if (order != null)
                            {
                                worksheet.Cell(i + rowNr, 5).Value = order.orderCompanyNumber();

                                worksheet.Cell(i + rowNr, 6).Value = order.product.name;
                                worksheet.Cell(i + rowNr, 7).Value = order.notes;
                                worksheet.Cell(i + rowNr, 7).Style.Alignment.WrapText = true;

                                worksheet.Row(i + rowNr).Height = 50;
                                double product = listP.First(p => p.order_id == order.id).gross_product_price;
                                worksheet.Cell(i + rowNr, 8).Value = priceToString(product);
                                worksheet.Cell(i + rowNr, 9).Value = priceToString(order.product_advance);
                                worksheet.Cell(i + rowNr, 10).Value = priceToString(product - order.product_advance);        // pogrubic ?

                                double installation = listP.First(p => p.order_id == order.id).gross_installation_price;
                                worksheet.Cell(i + rowNr, 11).Value = priceToString(installation);
                                worksheet.Cell(i + rowNr, 12).Value = priceToString(order.installation_advance);
                                worksheet.Cell(i + rowNr, 13).Value = priceToString(installation - order.installation_advance);        // pogrubic ?


                                for (int k = 5; k <= 13; k++)
                                {
                                    var cell = worksheet.Cell(i + rowNr, k);
                                    cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                    cell.Style.Border.OutsideBorderColor = XLColor.Gray;


                                    if (k == 10 || k == 13)
                                    {
                                        if (cell.Value.Equals("0,00"))
                                        {
                                            cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#9ec292");
                                        }
                                        else
                                        {
                                            cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#cbb2d4");
                                        }

                                        cell.Style.Font.Bold = true;
                                    }
                                    else
                                    {
                                        cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#e9deec");
                                    }
                                   
                                    
                                }
                                rowNr += 1;
                                
                            }
                        }
                    }
                    rowNr += 1;

                }


                worksheet.Columns().AdjustToContents();
                //worksheet.Column(4).Width = 22;
                //worksheet.Rows().AdjustToContents();    

                workbook.SaveAs(filePath + "\\lista-dluznikow.xlsx");
                OpenExcelFile(filePath + "\\lista-dluznikow.xlsx");
            }
        }


        public static void createMeasurementsFile(List<Measurement> list)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Pomiary");
                worksheet.Style.Alignment.WrapText = true;

                worksheet.Cell(1, 1).Value = "Data";
                worksheet.Cell(1, 2).Value = "Klient";
                worksheet.Cell(1, 3).Value = "Telefon";
                worksheet.Cell(1, 4).Value = "Adres montażu";
                worksheet.Cell(1, 5).Value = "Stan";
                worksheet.Cell(1, 6).Value = "Opiekun";
                worksheet.Cell(1, 7).Value = "Za pomiar";
                worksheet.Cell(1, 8).Value = "Uwagi";
                worksheet.Cell(1, 9).Value = "Towar";
                worksheet.Cell(1, 10).Value = "Data pomiaru";


                var headerRange = worksheet.Range(1, 1, 1, 10);
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                headerRange.Style.Border.OutsideBorderColor = XLColor.Gray;

                int line = 0;

                for (int i = 0; i < list.Count; i++)
                {
                   Measurement measurement = list[i];

                    worksheet.Cell(line + 2, 1).Value = dateTimeToString(measurement.registration_date);
                    worksheet.Cell(line + 2, 2).Value = measurement.client.DisplayNameOnly;
                    worksheet.Cell(line + 2, 3).Value = measurement.client.phone_number;
                    worksheet.Cell(line + 2, 4).Value = measurement.measurement_address?.TrimEnd('\r', '\n');
                    worksheet.Cell(line + 2, 5).Value = measurement.status;
                    worksheet.Cell(line + 2, 6).Value = measurement.employee.first_name;
                    worksheet.Cell(line + 2, 7).Value = measurement.price;
                    worksheet.Cell(line + 2, 8).Value = measurement.notes_information?.TrimEnd('\r', '\n'); 
                    worksheet.Cell(line + 2, 9).Value = measurement.notes?.TrimEnd('\r', '\n'); 
                    worksheet.Cell(line + 2, 10).Value = dateTimeToString(measurement.measurement_date);


                    line++;


                    for (int j = 1; j <= 10; j++)
                    {
                        var cell = worksheet.Cell(i + 2, j);
                        cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        cell.Style.Border.OutsideBorderColor = XLColor.Gray;
                    }
                }


                worksheet.Column(1).Width = 12;
                worksheet.Column(2).Width = 24;
                worksheet.Column(3).Width = 24; //phone
                worksheet.Column(4).Width = 24;
                worksheet.Column(5).Width = 12;
                worksheet.Column(6).Width = 12;
                worksheet.Column(7).Width = 12;
                worksheet.Column(8).Width = 24;
                worksheet.Column(9).Width = 24;
                worksheet.Column(10).Width = 12;

                //worksheet.Rows().AdjustToContents(9);            

                for (int i = 1; i <= worksheet.RowsUsed().Count(); i++)
                {
                    worksheet.Row(i).AdjustToContents();
                    worksheet.Row(i).Height += 10;
                }

                workbook.SaveAs(filePath + "\\pomiary.xlsx");
                OpenExcelFile(filePath + "\\pomiary.xlsx");
            }
        }


        private static void OpenExcelFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
            }
            else
            {
                Console.WriteLine("Plik nie został znaleziony.");
            }
        }

        public static string dateTimeToString(DateTime? date)
        {
            if (!date.HasValue)
                return "";
            else
                return date.Value.ToString("dd.MM.yyyy");
        }

        public static string priceToString(double price)
        {
            return price.ToString("F2");
        }

        public static double stringToPrice(string price)
        {
            //if (double.TryParse(price.Replace('.', ','), out double result))
            if (double.TryParse(price, out double result))
            {
                return result;
            }

            return 0.00;
        }
    }
}
