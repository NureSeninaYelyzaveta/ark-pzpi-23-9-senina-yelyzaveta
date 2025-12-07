using Microsoft.EntityFrameworkCore;
using WebApplicationArtProgress1.Data;
using WebApplicationArtProgress1.Models;
using WebApplicationArtProgress1.DTOs;

namespace WebApplicationArtProgress1.Services
{
    public class NotificationService
    {
        private readonly ArtProgressContext _context;

        public NotificationService(ArtProgressContext context)
        {
            _context = context;
        }

        // Створити сповіщення вручну
        public async Task<Notification> AddNotificationAsync(NotificationDto dto)
        {
            var notification = new Notification
            {
                Text = dto.Text,
                Date = dto.Date,
                StudentID = dto.StudentID,
                ParentID = dto.ParentID,
                TeacherID = dto.TeacherID
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
            return notification;
        }

        // Оновити сповіщення
        public async Task<bool> UpdateNotificationAsync(int id, NotificationDto dto)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null) return false;

            notification.Text = dto.Text;
            notification.Date = dto.Date;
            notification.StudentID = dto.StudentID;
            notification.ParentID = dto.ParentID;
            notification.TeacherID = dto.TeacherID;

            _context.Entry(notification).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        // Видалити сповіщення
        public async Task<bool> DeleteNotificationAsync(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null) return false;

            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
            return true;
        }

        // Сповіщення про нову оцінку
        public async Task NotifyOnNewGradeAsync(int studentId, int teacherId, int disciplineId, int value)
        {
            var discipline = await _context.Disciplines
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.DisciplineID == disciplineId);

            var disciplineName = discipline?.Name ?? "невідома дисципліна";

            // Сповіщення для студента
            _context.Notifications.Add(new Notification
            {
                StudentID = studentId,
                TeacherID = teacherId,
                Text = $"Нова оцінка: {value} з дисципліни \"{disciplineName}\"",
                Date = DateTime.UtcNow
            });

            // Отримати всіх валідних батьків студента (тільки тих, що реально існують у Parent)
            var parentIds = await _context.ParentStudents
                .Where(ps => ps.StudentID == studentId)
                .Join(_context.Parents,
                      ps => ps.ParentID,
                      p => p.ParentID,
                      (ps, p) => p.ParentID)
                .ToListAsync();

            foreach (var parentId in parentIds)
            {
                _context.Notifications.Add(new Notification
                {
                    ParentID = parentId,
                    StudentID = studentId,
                    TeacherID = teacherId,
                    Text = $"Дитина отримала оцінку: {value} з дисципліни \"{disciplineName}\"",
                    Date = DateTime.UtcNow
                });
            }

            await _context.SaveChangesAsync();
        }


        // Отримати всі сповіщення для студента
        public async Task<List<Notification>> GetStudentNotificationsAsync(int studentId)
        {
            return await _context.Notifications
                .AsNoTracking()
                .Where(n => n.StudentID == studentId)
                .OrderByDescending(n => n.Date)
                .ToListAsync();
        }

        // Отримати всі сповіщення для батька
        public async Task<List<Notification>> GetParentNotificationsAsync(int parentId)
        {
            return await _context.Notifications
                .AsNoTracking()
                .Where(n => n.ParentID == parentId)
                .OrderByDescending(n => n.Date)
                .ToListAsync();
        }

        // Отримати всі сповіщення з навігаційними властивостями
        public async Task<List<Notification>> GetAllNotificationsAsync()
        {
            return await _context.Notifications
                .Include(n => n.Student)
                .Include(n => n.Parent)
                .Include(n => n.Teacher)
                .AsNoTracking()
                .OrderByDescending(n => n.Date)
                .ToListAsync();
        }
    }
}