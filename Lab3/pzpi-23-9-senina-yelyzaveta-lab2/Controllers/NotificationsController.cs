using Microsoft.AspNetCore.Mvc;
using WebApplicationArtProgress1.DTOs;
using WebApplicationArtProgress1.Models;
using WebApplicationArtProgress1.Services;

namespace WebApplicationArtProgress1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly NotificationService _notificationService;

        public NotificationsController(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        /// <summary>
        /// Отримати всі сповіщення в системі.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllNotifications()
        {
            var notifications = await _notificationService.GetAllNotificationsAsync();
            return Ok(notifications);
        }

        /// <summary>
        /// Отримати сповіщення для конкретного студента.
        /// </summary>
        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetStudentNotifications([FromRoute] int studentId)
        {
            var notifications = await _notificationService.GetStudentNotificationsAsync(studentId);
            return Ok(notifications);
        }

        /// <summary>
        /// Отримати сповіщення для конкретного батька.
        /// </summary>
        [HttpGet("parent/{parentId}")]
        public async Task<IActionResult> GetParentNotifications([FromRoute] int parentId)
        {
            var notifications = await _notificationService.GetParentNotificationsAsync(parentId);
            return Ok(notifications);
        }

        /// <summary>
        /// Створити нове сповіщення вручну.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Notification>> PostNotification(NotificationDto dto)
        {
            var notification = await _notificationService.AddNotificationAsync(dto);
            return CreatedAtAction(nameof(GetAllNotifications), new { id = notification.NotificationID }, notification);
        }

        /// <summary>
        /// Оновити сповіщення за ідентифікатором.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNotification(int id, NotificationDto dto)
        {
            var updated = await _notificationService.UpdateNotificationAsync(id, dto);
            if (!updated) return NotFound();
            return NoContent();
        }

        /// <summary>
        /// Видалити сповіщення за ідентифікатором.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            var deleted = await _notificationService.DeleteNotificationAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}