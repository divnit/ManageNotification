using ManageNotification.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public class ProjectController : Controller
{
    private readonly AppDbContext _context;

    public ProjectController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var projects = _context.Projects.ToList();
        return View(projects);
    }

    [HttpPost]
    public async Task<IActionResult> Add(string name, IFormFile jsonFile)
    {
        if (jsonFile != null && jsonFile.Length > 0)
        {
            var uploadDir = Path.Combine("wwwroot", "uploads");
            if (!Directory.Exists(uploadDir))
            {
                Directory.CreateDirectory(uploadDir);
            }

            var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(jsonFile.FileName)}";
            var filePath = Path.Combine(uploadDir, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await jsonFile.CopyToAsync(stream);
            }

            var project = new ProjectModel
            {
                Name = name,
                JsonFilePath = "/uploads/" + uniqueFileName
            };

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        var project = _context.Projects.FirstOrDefault(p => p.Id == id);
        if (project != null)
        {
            _context.Projects.Remove(project);
            _context.SaveChanges();
        }

        return RedirectToAction("Index");
    }

}
