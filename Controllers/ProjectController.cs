using ManageNotification.Data;
using ManageNotification.Models;
using Microsoft.AspNetCore.Mvc;

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

            string projectName = string.Join("_", name.Split(Path.GetInvalidFileNameChars()));
            string originalFileName = Path.GetFileName(jsonFile.FileName);
            string fileName = $"{projectName}_{originalFileName}";

            var filePath = Path.Combine(uploadDir, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await jsonFile.CopyToAsync(stream);
            }

            var project = new ProjectModel
            {
                Name = name,
                JsonFilePath = "/uploads/" + fileName
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
            // Only try to delete if JsonFilePath is not null or empty
            if (!string.IsNullOrWhiteSpace(project.JsonFilePath))
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", project.JsonFilePath.TrimStart('/'));

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            _context.Projects.Remove(project);
            _context.SaveChanges();
        }

        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult GetSubProjects(int mainProjectId)
    {
        var subProjects = (from r in _context.References
                           join p in _context.Projects on r.SubProjectId equals p.Id
                           where r.MainProjectId == mainProjectId
                           select new
                           {
                               id = r.SubProjectId,
                               name = p.Name,
                               isAssigned = r.IsAssigned
                           }).ToList();

        return Json(subProjects);
    }



    [HttpPost]
    public async Task<JsonResult> SaveSubProjectsWithNew([FromBody] SaveSubProjectsModel model)
    {
        foreach (var sp in model.SubProjects)
        {
            var subProj = _context.Projects.FirstOrDefault(p => p.Name == sp.Name);
            if (subProj == null)
            {
                subProj = new ProjectModel { Name = sp.Name };
                _context.Projects.Add(subProj);
                _context.SaveChanges();
            }

            var existing = _context.References.FirstOrDefault(r =>
                r.MainProjectId == model.MainProjectId &&
                r.SubProjectId == subProj.Id);

            if (existing == null)
            {
                var reference = new Reference
                {
                    MainProjectId = model.MainProjectId,
                    SubProjectId = subProj.Id,
                    IsAssigned = sp.IsAssigned
                };
                _context.References.Add(reference);
            }
            else
            {
                existing.IsAssigned = sp.IsAssigned;
            }
        }

        _context.SaveChanges();

        return Json(new { success = true });
    }

}
