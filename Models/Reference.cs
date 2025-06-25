using System.ComponentModel.DataAnnotations;

namespace ManageNotification.Models
{
    public class Reference
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MainProjectId { get; set; }

        [Required]
        public int SubProjectId { get; set; }

        public bool IsAssigned { get; set; }
    }
}
