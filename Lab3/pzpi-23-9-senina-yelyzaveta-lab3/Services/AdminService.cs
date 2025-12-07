using Microsoft.EntityFrameworkCore;
using WebApplicationArtProgress1.Data;
using WebApplicationArtProgress1.Models;

namespace WebApplicationArtProgress1.Services
{
    public class AdminService
    {
        private readonly ArtProgressContext _context;

        public AdminService(ArtProgressContext context)
        {
            _context = context;
        }

        // Додати дисципліну
        public async Task<Discipline> AddDisciplineAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Назва дисципліни не може бути порожньою.", nameof(name));

            var exists = await _context.Disciplines.AnyAsync(d => d.Name == name.Trim());
            if (exists)
                throw new InvalidOperationException("Дисципліна з такою назвою вже існує.");

            var discipline = new Discipline { Name = name.Trim() };
            _context.Disciplines.Add(discipline);  // додаємо в базу
            await _context.SaveChangesAsync();  // зберігаємо зміни
            return discipline;
        }

        // Видалити дисципліну за айди
        public async Task<bool> RemoveDisciplineAsync(int disciplineId)
        {
            var discipline = await _context.Disciplines.FindAsync(disciplineId);
            if (discipline == null) return false;

            _context.Disciplines.Remove(discipline);
            await _context.SaveChangesAsync();
            return true;
        }

        // Системна статистика
        public async Task<SystemStatsDto> GetSystemStatsAsync()
        {
            var studentsCount = await _context.Students.CountAsync();
            var teachersCount = await _context.Teachers.CountAsync();
            var disciplinesCount = await _context.Disciplines.CountAsync();
            var gradesCount = await _context.Grades.CountAsync();

            // Обчислення середнього балу
            var grades = await _context.Grades.ToListAsync();
            double averageGrade = grades.Count > 0
                ? Math.Round(grades.Average(g => g.Value), 2)
                : 0.0;


            return new SystemStatsDto
            {
                Students = studentsCount,
                Teachers = teachersCount,
                Disciplines = disciplinesCount,
                Grades = gradesCount,
                AverageGrade = averageGrade
            };
        }
    }

    // DTO для статистики (кількість студентів, вчителів і тд)
    public class SystemStatsDto
    {
        public int Students { get; set; }
        public int Teachers { get; set; }
        public int Disciplines { get; set; }
        public int Grades { get; set; }
        public double AverageGrade { get; set; }
    }
}