using Microsoft.EntityFrameworkCore;
using WebApplicationArtProgress1.Data;
using WebApplicationArtProgress1.Models;
using WebApplicationArtProgress1.DTOs;

namespace WebApplicationArtProgress1.Services
{
    public class GradesService
    {
        private readonly ArtProgressContext _context;
        private readonly NotificationService _notifications;

        public GradesService(ArtProgressContext context, NotificationService notifications)
        {
            _context = context;
            _notifications = notifications;
        }

        // Додати оцінку з валідацією та опціональним коментарем
        public async Task<Grade> AddGradeAsync(
            int studentId,
            int teacherId,
            int disciplineId,
            int value,
            string? commentText = null)
        {
            // перевірка існування студента, вчителя та дисципліни
            var studentExists = await _context.Students.AnyAsync(s => s.StudentID == studentId);
            var teacherExists = await _context.Teachers.AnyAsync(t => t.TeacherID == teacherId);
            var disciplineExists = await _context.Disciplines.AnyAsync(d => d.DisciplineID == disciplineId);

            if (!studentExists) throw new ArgumentException("Студента не знайдено.", nameof(studentId));
            if (!teacherExists) throw new ArgumentException("Викладача не знайдено.", nameof(teacherId));
            if (!disciplineExists) throw new ArgumentException("Дисципліну не знайдено.", nameof(disciplineId));
            if (value < 1 || value > 12) throw new ArgumentOutOfRangeException(nameof(value), "Оцінка має бути в діапазоні 1–12.");

            var grade = new Grade
            {
                StudentID = studentId,
                TeacherID = teacherId,
                DisciplineID = disciplineId,
                Value = value,
                Date = DateTime.UtcNow
            };

            _context.Grades.Add(grade);  // додаємо оцінку в базу

            if (!string.IsNullOrWhiteSpace(commentText))
            {
                // додаємо коментар, якщо він є
                _context.Comments.Add(new Comment
                {
                    StudentID = studentId,
                    TeacherID = teacherId,
                    Text = commentText.Trim(),
                    Date = DateTime.UtcNow
                });
            }

            await _context.SaveChangesAsync();
            await _notifications.NotifyOnNewGradeAsync(studentId, teacherId, disciplineId, value);

            return grade;
        }

        // Оновити оцінку
        public async Task<bool> UpdateGradeAsync(int gradeId, GradeDto dto)
        {
            var grade = await _context.Grades.FindAsync(gradeId);
            if (grade == null) return false;

            // Перевірка діапазону значення
            if (dto.Value < 1 || dto.Value > 12)
                throw new ArgumentOutOfRangeException(nameof(dto.Value), "Оцінка має бути в діапазоні 1–12.");

            // Перевірка існування сутностей
            var studentExists = await _context.Students.AnyAsync(s => s.StudentID == dto.StudentID);
            if (!studentExists) throw new ArgumentException("Студента не знайдено.", nameof(dto.StudentID));

            var teacherExists = await _context.Teachers.AnyAsync(t => t.TeacherID == dto.TeacherID);
            if (!teacherExists) throw new ArgumentException("Викладача не знайдено.", nameof(dto.TeacherID));

            var disciplineExists = await _context.Disciplines.AnyAsync(d => d.DisciplineID == dto.DisciplineID);
            if (!disciplineExists) throw new ArgumentException("Дисципліну не знайдено.", nameof(dto.DisciplineID));

            // Оновлення поля оцінки
            grade.Value = dto.Value;
            grade.Date = dto.Date;
            grade.StudentID = dto.StudentID;
            grade.TeacherID = dto.TeacherID;
            grade.DisciplineID = dto.DisciplineID;

            _context.Entry(grade).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;

        }

        // Видалити оцінку
        public async Task<bool> DeleteGradeAsync(int gradeId)
        {
            var grade = await _context.Grades.FindAsync(gradeId);
            if (grade == null) return false;

            _context.Grades.Remove(grade);
            await _context.SaveChangesAsync();
            return true;
        }

        // Отримати оцінку за ідентифікатором
        public async Task<Grade?> GetGradeAsync(int gradeId)
        {
            return await _context.Grades
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.GradeID == gradeId);
        }

        // Середній бал студента
        public async Task<double> GetStudentAverageAsync(int studentId)
        {
            var values = await _context.Grades
                .Where(g => g.StudentID == studentId)
                .Select(g => g.Value)
                .ToListAsync();

            return values.Count == 0 ? 0.0 : Math.Round(values.Average(), 2);
        }

        // Середні бали по групі
        public async Task<Dictionary<int, double>> GetGroupAveragesAsync(string group)
        {
            var students = await _context.Students
                .Where(s => s.Group == group)
                .Select(s => new
                {
                    s.StudentID,
                    Values = s.Grades.Select(g => g.Value)
                })
                .ToListAsync();

            var result = new Dictionary<int, double>();
            foreach (var s in students)
            {
                var avg = s.Values.Any() ? Math.Round(s.Values.Average(), 2) : 0.0;
                result[s.StudentID] = avg;
            }
            return result;
        }

        // Рейтинг групи
        public async Task<List<(int StudentId, double Average)>> GetGroupRankingAsync(string group)
        {
            var data = await _context.Students
                .Where(s => s.Group == group)
                .Select(s => new
                {
                    s.StudentID,
                    Avg = s.Grades.Any() ? s.Grades.Average(g => g.Value) : 0.0
                })
                .ToListAsync();

            return data
                .OrderByDescending(x => x.Avg)
                .Select(x => (x.StudentID, Math.Round(x.Avg, 2)))
                .ToList();
        }

        // Отримати всі оцінки
        public async Task<List<Grade>> GetAllGradesAsync()
        {
            return await _context.Grades
                .Include(g => g.Student)
                .Include(g => g.Teacher)
                .Include(g => g.Discipline)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}