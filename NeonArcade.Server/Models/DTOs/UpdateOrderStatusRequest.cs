using System.ComponentModel.DataAnnotations;

namespace NeonArcade.Server.Models.DTOs
{
    /// <summary>
    /// Request DTO for updating order status
    /// </summary>
    public class UpdateOrderStatusRequest
    {
        [Required(ErrorMessage = "Status is required")]
        public string Status { get; set; } = string.Empty;
    }
}