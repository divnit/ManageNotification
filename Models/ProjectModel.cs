using System.ComponentModel.DataAnnotations;

public class ProjectModel
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public string? JsonFilePath { get; set; }
}
