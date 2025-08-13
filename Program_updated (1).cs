using System;
using RailwayReservation.DataAccessClass;
using RailwayReservation.Models;

namespace RailwayReservation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Railway Reservation System (v7)") ;
            while (true)
            {
                Console.WriteLine("1-Admin Login 2-Customer Login 3-Register 4-Exit");
                Console.Write("Choice: "); var ch = Console.ReadLine();
                if (ch=="1") AdminFlow();
                else if (ch=="2") CustomerFlow();
                else if (ch=="3") RegisterCustomer();
                else break;
            }
        }

        static void AdminFlow()
        {
            Console.Write("Admin username: "); var u = Console.ReadLine();
            Console.Write("Password: "); var p = Console.ReadLine();
            var ada = new AdminDataAccess();
            if (!ada.Login(u,p)) { Console.WriteLine("Invalid admin."); return; }
            Console.WriteLine("Admin logged in.");
            var rda = new ReportDataAccess();
            while (true)
            {
                Console.WriteLine("Admin Menu: 1-Report 2-Delete Booking & Refund 3-Logout");
                Console.Write("Choice: "); var c = Console.ReadLine();
                if (c=="1") rda.ShowReport();
                else if (c=="2") 
                {
                    Console.Write("BookingId to delete: "); int bid = int.Parse(Console.ReadLine()??"0");
                    ada.DeleteBookingAndRefund(bid);
                    Console.WriteLine("Booking deleted and refunded."); 
                }
                else break;
            }
        }

        static void RegisterCustomer()
        {
            var c = new Customer();
            Console.Write("Name: "); c.CustName = Console.ReadLine();
            Console.Write("Phone: "); c.CustPhone = Console.ReadLine();
            Console.Write("Email: "); c.CustEmail = Console.ReadLine();
            Console.Write("Password: "); c.CustPassword = Console.ReadLine();
            var cda = new CustomerDataAccess();
            int id = cda.Register(c);
            Console.WriteLine($"Registered CustId: {id}"); 
        }

        static void CustomerFlow()
        {
            Console.Write("Email: "); string e = Console.ReadLine();
            Console.Write("Password: "); string p = Console.ReadLine();
            var cda = new CustomerDataAccess();
            if (!cda.Login(e,p)) { Console.WriteLine("Invalid customer."); return; }
            Console.WriteLine("Customer logged in.");
            var tda = new TrainDataAccess();
            var rda = new ReservationDataAccess();
            var cnda = new CancellationDataAccess();
            while (true)
            {
                Console.WriteLine("Customer Menu: 1-view trains+availability 2-view classes 3-book 4-cancel 5-print 6-PNR status 7-logout");
                Console.Write("Choice: "); var ch = Console.ReadLine();
                if (ch=="1")
                {
                    var av = tda.GetAvailabilityView();
                    foreach (var a in av)
                    {
                        Console.WriteLine($"{a.TrainNumber} - {a.TrainName} ({a.Source}->{a.Destination}) | Sleeper:{a.SleeperAvailable} 2ndAC:{a.SecondACAvailable} 3rdAC:{a.ThirdACAvailable}");
                    }
                } - {t.TrainName} ({t.Source}->{t.Destination})");
                }
                else if (ch=="2")
                {
                    Console.Write("TrainNumber: "); int tn = int.Parse(Console.ReadLine()??"0");
                    var cls = tda.GetClassesForTrain(tn);
                    foreach(var cl in cls) Console.WriteLine($"{cl.TrainClassId} - {cl.ClassType} Seats:{cl.AvailableSeats}/{cl.MaxSeats} Price:{cl.Price}"); 
                }
                else if (ch=="3")
                {
                    Console.Write("CustId: "); int cid = int.Parse(Console.ReadLine()??"0");
                    Console.Write("TrainClassId: "); int tc = int.Parse(Console.ReadLine()??"0");
                    Console.Write("Travel Date (yyyy-mm-dd): "); var dt = DateTime.Parse(Console.ReadLine());
// Read class type from user
Console.Write("Enter class type (Sleeper/2nd AC/3rd AC or short code sl/2a/3a): ");
string classInput = Console.ReadLine().Trim().ToLower();

// Map short codes and variations to full class names
switch (classInput)
{
    case "sl":
    case "sleeper":
        classInput = "Sleeper";
        break;
    case "2a":
    case "2nd ac":
    case "second ac":
        classInput = "2nd AC";
        break;
    case "3a":
    case "3rd ac":
    case "third ac":
        classInput = "3rd AC";
        break;
    default:
        classInput = System.Globalization.CultureInfo.CurrentCulture.TextInfo
            .ToTitleCase(classInput);
        break;
}

// Lookup TrainClassId from mapped class name
var classRow = tda.GetClassesForTrain(trainNumber).FirstOrDefault(c => c.ClassType == classInput);
if (classRow == null)
{
    Console.WriteLine("Invalid class type.");
    return;
}
int tc = classRow.TrainClassId;

                    var bid = rda.Book(cid, tc, dt);
                    if (bid>0) Console.WriteLine($"Booked. BookingId: {bid}"); else Console.WriteLine("Booking failed (no seats).");
                }
                else if (ch=="4")
                {
                    Console.Write("BookingId to cancel: "); int bid = int.Parse(Console.ReadLine()??"0");
                    var refund = cnda.CancelByUser(bid);
                    Console.WriteLine($"Cancellation processed. Refund: {refund:C}"); 
                }
                else if (ch=="5")
                {
                    Console.Write("BookingId: "); int bid = int.Parse(Console.ReadLine()??"0");
                    using (var conn = DatabaseConnection.getConnection())
                    {
                        var cmd = new System.Data.SqlClient.SqlCommand("SELECT r.BookingId,c.CustName,t.TrainName,tc.ClassType,r.TravelDate FROM Reservations r JOIN Customers c ON r.CustId=c.CustId JOIN TrainClasses tc ON r.TrainClassId=tc.TrainClassId JOIN TrainMaster t ON tc.TrainNumber=t.TrainNumber WHERE r.BookingId=@b", conn);
                        cmd.Parameters.AddWithValue("@b", bid);
                        var rd = cmd.ExecuteReader();
                        if (rd.Read())
                        {
                            int bookingId = Convert.ToInt32(rd[0]);
                            string cust = rd[1].ToString();
                            string trainn = rd[2].ToString();
                            string cl = rd[3].ToString();
                            DateTime dt = (DateTime)rd[4];
                            TicketGenerator.SavePdfTicket(bookingId, cust, trainn, cl, dt);
                        }
                        else Console.WriteLine("Booking not found."); 
                    }
                }
                else break;
            }
        }
    }
}