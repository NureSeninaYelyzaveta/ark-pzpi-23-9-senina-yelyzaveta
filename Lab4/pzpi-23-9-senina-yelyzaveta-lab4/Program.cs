using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Linq;

namespace IoTClientArtProgress
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //кодування консолі для правильного відображення українських символів
            Console.OutputEncoding = System.Text.Encoding.UTF8;


            var client = new HttpClient();

            // Налаштування IoT-клієнта: вибір адреси сервера
            Console.Write("Введіть адресу сервера (або Enter для localhost): ");
            string serverAddress = Console.ReadLine();

            client.BaseAddress = string.IsNullOrEmpty(serverAddress)
                ? new Uri("https://localhost:7033/api/")
                : new Uri(serverAddress);

            // Перевірка підключення після встановлення адреси
            Console.WriteLine("Перевіряю підключення до сервера...");

            try
            {
                var test = await client.GetAsync("parents");

                if (test.IsSuccessStatusCode)
                {
                    Console.WriteLine(" Підключення успішне!");
                }
                else
                {
                    Console.WriteLine($"Сервер відповідає з помилкою: {test.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Не вдалося підключитися до сервера:");
                Console.WriteLine(ex.Message);
            }


            // Основне меню клієнта
            bool running = true;

            while (running)
            {
                Console.WriteLine(" IoT Клієнт Art Progress ");
                Console.WriteLine("1 - Показати всiх батькiв");
                Console.WriteLine("2 - Додати нового батька");
                Console.WriteLine("3 - Показати всі оцінки");
                Console.WriteLine("4 - Показати всіх викладачів");
                Console.WriteLine("5 - Додати нового викладача");
                Console.WriteLine("6 - Показати всі дисципліни викладачів");
                Console.WriteLine("7 - Показати всі сповіщення");
                Console.WriteLine("8 - Додати нове сповіщення");
                Console.WriteLine("9 - Бізнес-логіка: середній бал студентів");
                Console.WriteLine("10 - Бізнес-логіка: кількість сповіщень по батьках");
                Console.WriteLine("0 - Вийти");
                Console.Write("Оберіть дію: ");
                string choice = Console.ReadLine();

                // Обробка вибору користувача
                switch (choice)
                {
                    case "1":
                        await ShowParents(client);
                        break;
                    case "2":
                        await AddParent(client);
                        break;
                    case "3":
                        await ShowGrades(client);
                        break;
                    case "4":
                        await ShowTeachers(client);
                        break;
                    case "5":
                        await AddTeacher(client);
                        break;
                    case "6":
                        await ShowTeacherDisciplines(client);
                        break;
                    case "7":
                        await ShowNotifications(client);
                        break;
                    case "8":
                        await AddNotification(client);
                        break;
                    case "9":
                        await ShowAverageGrades(client);
                        break;
                    case "10":
                        await ShowNotificationsCountByParent(client);
                        break;
                    case "0":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Невірний вибір. Спробуйте ще раз.");
                        break;
                }

                Console.WriteLine();
            }
        }

        // Батьки
        static async Task ShowParents(HttpClient client) // Показати всіх батьків
        {
            try
            {
                var parents = await client.GetFromJsonAsync<List<Parent>>("parents");
                Console.WriteLine("Список батьків:");
                foreach (var p in parents)
                {
                    Console.WriteLine($"ID: {p.ParentID}, Ім'я: {p.Name}, Контакт: {p.ContactInfo}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка при отриманні батьків: " + ex.Message);
            }
        }

        static async Task AddParent(HttpClient client)  // Додати нового батька
        {
            Console.Write("Введіть ім'я батька: ");
            string name = Console.ReadLine();
            Console.Write("Введіть контактну інформацію: ");
            string contact = Console.ReadLine();

            var newParent = new ParentDto
            {
                Name = name,
                ContactInfo = contact
            };

            try
            {
                var response = await client.PostAsJsonAsync("parents", newParent);
                if (response.IsSuccessStatusCode)
                    Console.WriteLine("Батька успішно додано!");
                else
                    Console.WriteLine("Помилка при додаванні батька: " + response.StatusCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка при додаванні батька: " + ex.Message);
            }
        }

        // Оцінки
        static async Task ShowGrades(HttpClient client)  // Показати всі оцінки
        {
            try
            {
                var grades = await client.GetFromJsonAsync<List<Grade>>("grades");
                Console.WriteLine("Список оцінок:");
                foreach (var g in grades)
                {
                    Console.WriteLine($"ID: {g.GradeID}, StudentID: {g.StudentID}, TeacherID: {g.TeacherID}, DisciplineID: {g.DisciplineID}, Оцінка: {g.Value}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка при отриманні оцінок: " + ex.Message);
            }
        }

        // Викладачі 
        static async Task ShowTeachers(HttpClient client)  // Показати всіх викладачів
        {
            try
            {
                var teachers = await client.GetFromJsonAsync<List<Teacher>>("teachers");
                Console.WriteLine("Список викладачів:");
                foreach (var t in teachers)
                {
                    Console.WriteLine($"ID: {t.TeacherID}, Ім'я: {t.Name}, Посада: {t.Position}, Контакт: {t.ContactInfo}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка при отриманні викладачів: " + ex.Message);
            }
        }

        static async Task AddTeacher(HttpClient client)  // Додати нового викладача
        {
            Console.Write("Введіть ім'я викладача: ");
            string name = Console.ReadLine();
            Console.Write("Введіть посаду: ");
            string position = Console.ReadLine();
            Console.Write("Введіть контактну інформацію: ");
            string contact = Console.ReadLine();

            var newTeacher = new TeacherDto
            {
                Name = name,
                Position = position,
                ContactInfo = contact
            };

            try
            {
                var response = await client.PostAsJsonAsync("teachers", newTeacher);
                if (response.IsSuccessStatusCode)
                    Console.WriteLine("Викладача успішно додано!");
                else
                    Console.WriteLine("Помилка при додаванні викладача: " + response.StatusCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка при додаванні викладача: " + ex.Message);
            }
        }

        // Дисципліни викладачів 
        static async Task ShowTeacherDisciplines(HttpClient client) // Показати дисципліни викладачів
        {
            try
            {
                var tds = await client.GetFromJsonAsync<List<TeacherDiscipline>>("TeacherDisciplines");
                Console.WriteLine("Список дисциплін викладачів:");
                foreach (var td in tds)
                {
                    Console.WriteLine($"TeacherID: {td.TeacherID}, DisciplineID: {td.DisciplineID}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка при отриманні дисциплін: " + ex.Message);
            }
        }

        // Сповіщення 
        static async Task ShowNotifications(HttpClient client)  // Показати всі сповіщення
        {
            try
            {
                var notifications = await client.GetFromJsonAsync<List<Notification>>("notifications");
                Console.WriteLine("Список сповіщень:");
                foreach (var n in notifications)
                {
                    Console.WriteLine($"ID: {n.NotificationID}, Текст: {n.Text}, StudentID: {n.StudentID}, ParentID: {n.ParentID}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка при отриманні сповіщень: " + ex.Message);
            }
        }

        static async Task AddNotification(HttpClient client)  // Додати нове сповіщення
        {
            Console.Write("Введіть текст сповіщення: ");
            string text = Console.ReadLine();
            Console.Write("Введіть StudentID (якщо є, інакше 0): ");
            int studentId = int.Parse(Console.ReadLine() ?? "0");
            Console.Write("Введіть ParentID (якщо є, інакше 0): ");
            int parentId = int.Parse(Console.ReadLine() ?? "0");

            var newNotification = new NotificationDto
            {
                Text = text,
                StudentID = studentId == 0 ? (int?)null : studentId,
                ParentID = parentId == 0 ? (int?)null : parentId
            };

            try
            {
                var response = await client.PostAsJsonAsync("notifications", newNotification);
                if (response.IsSuccessStatusCode)
                    Console.WriteLine("Сповіщення успішно додано!");
                else
                    Console.WriteLine("Помилка при додаванні сповіщення: " + response.StatusCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка при додаванні сповіщення: " + ex.Message);
            }
        }

        // Середній бал студентів
        static async Task ShowAverageGrades(HttpClient client)
        {
            try
            {
                var grades = await client.GetFromJsonAsync<List<Grade>>("grades");
                if (grades.Count == 0) { Console.WriteLine("Оцінок немає"); return; }
                double avg = grades.Average(g => g.Value);
                Console.WriteLine($"Середній бал студентів: {avg:F2}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка при обчисленні середнього балу: " + ex.Message);
            }
        }

        //Кількість сповіщень по батьках
        static async Task ShowNotificationsCountByParent(HttpClient client)
        {
            try
            {
                var notifications = await client.GetFromJsonAsync<List<Notification>>("notifications");
                var grouped = notifications.GroupBy(n => n.ParentID);
                foreach (var group in grouped)
                {
                    Console.WriteLine($"ParentID: {group.Key}, кількість сповіщень: {group.Count()}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка при підрахунку сповіщень: " + ex.Message);
            }
        }
    }

    // Моделі для клієнта
    public class Parent
    {
        public int ParentID { get; set; }
        public string Name { get; set; }
        public string ContactInfo { get; set; }
    }

    public class ParentDto
    {
        public string Name { get; set; }
        public string ContactInfo { get; set; }
    }

    public class Grade
    {
        public int GradeID { get; set; }
        public int StudentID { get; set; }
        public int TeacherID { get; set; }
        public int DisciplineID { get; set; }
        public int Value { get; set; }
    }

    public class Teacher
    {
        public int TeacherID { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string ContactInfo { get; set; }
    }

    public class TeacherDto
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public string ContactInfo { get; set; }
    }

    public class TeacherDiscipline
    {
        public int TeacherID { get; set; }
        public int DisciplineID { get; set; }
    }

    public class Notification
    {
        public int NotificationID { get; set; }
        public string Text { get; set; }
        public int? StudentID { get; set; }
        public int? ParentID { get; set; }
    }

    public class NotificationDto
    {
        public string Text { get; set; }
        public int? StudentID { get; set; }
        public int? ParentID { get; set; }
    }
}
