using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class EmploeeController : Controller
{
    private readonly ApplicationContext _context;
    public EmploeeController(ApplicationContext context)
    {
        _context = context;
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    public ActionResult Create()//Создание пользователья
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Emploee emploee)
    {
        _context.Emploees.Add(emploee);
        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = "Работник успешно добавлен";
        return RedirectToAction("Create");

    }


    [HttpGet]
    public async Task<IActionResult> GetEmployees(string? FName, string? LName, string minstanding, SortState sortOrder = SortState.FNameAsc)
    {
        // Сохранение переданных параметров в TempData для сохранения их состояния между запросами
        TempData["FName"] = FName;
        TempData["LName"] = LName;
        TempData["minstanding"] = minstanding;

        // Получение IQueryable объектов Emploee из контекста базы данных
        IQueryable<Emploee> users = _context.Emploees;

        // Рассчитывается дата стажа, отсчитываемая от текущей даты
        DateTime standingDate = DateTime.Now.AddYears(Convert.ToInt32(minstanding) * (-1));

        // Фильтрация по имени
        if (!string.IsNullOrWhiteSpace(FName))
        {
            users = users.Where(s => s.FName.Contains(FName));
        }

        // Фильтрация по фамилии
        if (!string.IsNullOrWhiteSpace(LName))
        {
            users = users.Where(s => s.LName.Contains(LName));
        }

        // Фильтрация по стажу
        if (minstanding != "0" || minstanding != null)
        {
            users = users.Where(s => s.DateOfHire < standingDate);
        }

        // Применение сортировки
        users = sortOrder switch
        {
            SortState.FNameDesc => users.OrderByDescending(s => s.FName),
            SortState.LNameAsc => users.OrderBy(s => s.LName),
            SortState.LNameDesc => users.OrderByDescending(s => s.LName),
            SortState.EmailAsc => users.OrderBy(s => s.Email),
            SortState.EmailDesc => users.OrderByDescending(s => s.Email),
            SortState.DateOfHireAsc => users.OrderBy(s => s.DateOfHire),
            SortState.DateOfHireDesc => users.OrderByDescending(s => s.DateOfHire),
            SortState.DateOfBirthAsc => users.OrderBy(s => s.DateOfBirth),
            SortState.DateOfBirthDesc => users.OrderByDescending(s => s.DateOfBirth),
            SortState.PositionAsc => users.OrderBy(s => s.Position),
            SortState.PositionDesc => users.OrderByDescending(s => s.Position),
            SortState.AddressAsc => users.OrderBy(s => s.Address),
            SortState.AddressDesc => users.OrderByDescending(s => s.Address),
            SortState.CityAsc => users.OrderBy(s => s.City),
            SortState.CityDesc => users.OrderByDescending(s => s.City),
            SortState.RegionAsc => users.OrderBy(s => s.Region),
            SortState.RegionDesc => users.OrderByDescending(s => s.Region),
            _ => users.OrderBy(s => s.FName), // По умолчанию сортировка по имени в порядке возрастания
        };

        // Создание ViewModel, которая будет передана в представление
        IndexViewModel viewModel = new IndexViewModel
        {
            Emploees = await users.AsNoTracking().ToListAsync(), // Загрузка пользователей в список с учетом сортировки
            SortViewModel = new SortViewModel(sortOrder) // Создание модели сортировки для передачи в представление
        };

        // Возвращение представления с ViewModel
        return View(viewModel);
    }

    /*Стоит отметить, что данный метод Delete обрабатывает только запросы типа POST. Почему? Дело в том, что использование get-методов не безопасно. Например, нам могут прислать письмо с картинкой:
            <img src="http://адрес_нашего_сайта/Home/Delete/1" />
                И при открытии письма на сервер будет отправлен get-запрос. И если бы метод Delete обрабатывал бы get-запросы, то объект с id=1 был бы удален из базы данных.*/
    [HttpPost]
    public async Task<IActionResult> Delete(int? id)
    {
        // Проверка, что передан идентификатор сотрудника
        if (id != null)
        {
            // Создание нового объекта Emploee с указанным идентификатором
            Emploee emploee = new Emploee { Id = id.Value };

            // Установка состояния объекта в EntityState.Deleted,
            // это говорит Entity Framework, что этот объект должен быть удален из базы данных
            _context.Entry(emploee).State = EntityState.Deleted;

            // Сохранение изменений в базе данных
            await _context.SaveChangesAsync();

            // Перенаправление на метод GetEmployees после успешного удаления
            return RedirectToAction("GetEmployees");
        }

        // В случае отсутствия идентификатора, возвращается NotFound
        return NotFound();
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id != null)
        {
            Emploee? emploee = await _context.Emploees.FirstOrDefaultAsync(p => p.Id == id);
            if (emploee != null) return View(emploee);
        }
        return NotFound();
    }
    [HttpPost]
    public async Task<IActionResult> Edit(Emploee emploee)
    {
        _context.Emploees.Update(emploee);
        await _context.SaveChangesAsync();
        return RedirectToAction("GetEmployees");
    }

    [HttpPost]
    public async Task<IActionResult> UploadJsonFile(IFormFile file)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                @TempData["NoFileMessage"] = "Пожалуйста выберите файл";
                return RedirectToAction("GetEmployees");
            }

            using (var streamReader = new StreamReader(file.OpenReadStream()))
            {
                var jsonString = await streamReader.ReadToEndAsync();

                // Десериализация JSON
                var employees = JsonSerializer.Deserialize<List<Emploee>>(jsonString);
                if (employees != null && employees.Any())
                {
                    // Устанавливаем Id в 0 для каждого объекта
                    foreach (var employee in employees)
                    {
                        employee.Id = 0;
                    }

                    // Добавление данных в базу данных
                    _context.Emploees.AddRange(employees);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Успешно добавлено";
                    return RedirectToAction("GetEmployees");
                    //return Ok("Данные успешно добавлены в базу данных.");
                }
                else
                {
                    @TempData["ErrorMessage"] = "Ошибка";
                    return RedirectToAction("GetEmployees");
                }
            }
        }
        catch (Exception ex)
        {
            @TempData["ErrorMessage"] = "Ошибка";
            return RedirectToAction("GetEmployees");
        }
    }
    public async Task<IActionResult> DownloadFile(int? id)
    {
        try
        {
            if (id != null)
            {
                Emploee? emploee = await _context.Emploees.FirstOrDefaultAsync(p => p.Id == id);

                if (emploee != null)
                {
                    // Сериализация объекта в JSON
                    string jsonString = JsonSerializer.Serialize(new[] { emploee });

                    // Создаем уникальное имя файла
                    string fileName = $"employee_{emploee.FName}_{emploee.LName}_{DateTime.Now:yyyyMMddHHmmss}.json";

                    //// Записываем JSON в массив байтов
                    //byte[] fileContents = Encoding.UTF8.GetBytes(jsonString);

                    // Возвращаем файловый поток клиенту
                    return File(Encoding.UTF8.GetBytes(jsonString), "application/json", fileName);
                }
            }

            return NotFound("Employee not found with the provided id.");
        }
        catch (Exception ex)
        {
            // Обработка ошибок, если необходимо
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }


    [HttpPost]
    public async Task<IActionResult> DownloadAllEmploees([FromForm] IndexViewModel model)
    {
        try
        {
            // Получаем массив id из модели
            int[] employeeIds = model.EmployeeIds;

            // Получаем объекты Emploee из базы данных
            var employees = await _context.Emploees
                .Where(e => employeeIds.Contains(e.Id))
                .ToListAsync();

            if (employees.Any())
            {
                // Сериализуем объекты Emploee в JSON
                string jsonString = JsonSerializer.Serialize(employees);

                // Получаем уникальное имя файла
                string fileName = $"all_employees_forusers{DateTime.Now:yyyyMMddHHmmss}.json";

                // Составляем путь для сохранения файла на стороне клиента
                string filePath = Path.Combine("Saves", fileName);

                // Возвращаем файл для скачивания на стороне клиента
                return File(Encoding.UTF8.GetBytes(jsonString), "application/json", fileName);
            }

            return NotFound("No employees found with the provided ids.");
        }
        catch (Exception ex)
        {
            // Обработка ошибок, если необходимо
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }

}
public class ApplicationContext : DbContext
{
    public DbSet<Emploee> Emploees { get; set; } = null!;
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
        Database.EnsureCreated();   // создаем базу данных при первом обращении
    }
}

public class Emploee : Human
{
    public int Id { get; set; }

    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [Display(Name = "Hire Date")]
    [DataType(DataType.Date)]
    public DateTime? DateOfHire { get; set; }

    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [Display(Name = "Birth Date")]
    [DataType(DataType.Date)]
    public DateTime? DateOfBirth { get; set; }

    [StringLength(100)]
    public string Position { get; set; }

}

public abstract class Human
{
    [Display(Name = "First Name")]
    [Required(ErrorMessage = "Frst name is required.")]
    [MaxLength(25, ErrorMessage = "Max is 25")]
    public string FName { get; set; }

    [Display(Name = "Last Name")]
    [Required(ErrorMessage = "Last name is required.")]
    [MaxLength(25, ErrorMessage = "Max is 25")]
    public string LName { get; set; }

    [Display(Name = "Email")]
    [Required(ErrorMessage = "Email address is required.")]
    [EmailAddress(ErrorMessage = "Email address is not valid.")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    [Display(Name = "Street")]
    [StringLength(50)]
    public string Address { get; set; }

    public string City { get; set; }

    public string Region { get; set; }

}

public class IndexViewModel
{
    public IEnumerable<Emploee> Emploees { get; set; } = new List<Emploee>();
    public string? FName { get; set; }
    public string? LName { get; set; }
    public string? minstanding { get; set; }
    public SortViewModel SortViewModel { get; set; } = new SortViewModel(SortState.FNameAsc);

    public int[] EmployeeIds { get; set; }
}
public enum SortState
{
    FNameAsc,
    FNameDesc,
    LNameAsc,
    LNameDesc,
    EmailAsc,
    EmailDesc,
    DateOfHireAsc,
    DateOfHireDesc,
    DateOfBirthAsc,
    DateOfBirthDesc,
    PositionAsc,
    PositionDesc,
    AddressAsc,
    AddressDesc,
    CityAsc,
    CityDesc,
    RegionAsc,
    RegionDesc,

}

public class SortViewModel
{
    // Значения для сортировки по разным полям
    public SortState FNameSort { get; set; }
    public SortState LNameSort { get; set; }
    public SortState EmailSort { get; set; }
    public SortState DateOfHireSort { get; set; }
    public SortState DateOfBirthSort { get; set; }
    public SortState PositionSort { get; set; }
    public SortState AddressSort { get; set; }
    public SortState CitySort { get; set; }
    public SortState RegionSort { get; set; }

    public SortState Current { get; set; } // Значение свойства, выбранного для сортировки
    public bool Up { get; set; }  // Флаг сортировки по возрастанию или убыванию

    public SortViewModel(SortState sortOrder)
    {
        // Значения по умолчанию для различных полей сортировки
        FNameSort = SortState.FNameAsc;
        LNameSort = SortState.LNameAsc;
        EmailSort = SortState.EmailAsc;
        DateOfHireSort = SortState.DateOfHireAsc;
        DateOfBirthSort = SortState.DateOfBirthAsc;
        PositionSort = SortState.PositionAsc;
        AddressSort = SortState.AddressAsc;
        CitySort = SortState.CityAsc;
        RegionSort = SortState.RegionAsc;

        Up = true; // Исходно сортировка устанавливается по возрастанию

        // Проверка направления сортировки и установка флага Up соответственно
        if (sortOrder == SortState.LNameDesc || sortOrder == SortState.FNameDesc
            || sortOrder == SortState.EmailDesc || sortOrder == SortState.DateOfHireDesc
            || sortOrder == SortState.DateOfBirthDesc || sortOrder == SortState.PositionDesc
            || sortOrder == SortState.AddressDesc || sortOrder == SortState.CityDesc
            || sortOrder == SortState.RegionDesc)
        {
            Up = false;
        }

        // Установка текущего направления сортировки в соответствии с переданным sortOrder
        switch (sortOrder)
        {
            case SortState.FNameDesc:
                Current = FNameSort = SortState.FNameAsc;
                break;
            case SortState.LNameAsc:
                Current = LNameSort = SortState.LNameDesc;
                break;
            // ... аналогичные блоки для остальных полей сортировки
            default:
                Current = FNameSort = SortState.FNameDesc;
                break;
        }
    }
}

public class SortHeaderTagHelper : TagHelper
{
    public string FName { get; set; }
    public string LName { get; set; }
    public string minstanding { get; set; }

    public SortState Property { get; set; } // значение текущего свойства, для которого создается тег
    public SortState Current { get; set; }  // значение активного свойства, выбранного для сортировки
    public string? Action { get; set; }  // действие контроллера, на которое создается ссылка
    public bool Up { get; set; }    // сортировка по возрастанию или убыванию

    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext ViewContext { get; set; } = null!;

    IUrlHelperFactory urlHelperFactory;
    public SortHeaderTagHelper(IUrlHelperFactory helperFactory)
    {
        urlHelperFactory = helperFactory;
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
        output.TagName = "a";
        //string? url = urlHelper.Action(Action, new { sortOrder = Property });

        var routeValues = new
        {
            FName,
            LName,
            minstanding,
            sortOrder = Property
        };

        string url = urlHelper.Action(Action, routeValues);

        output.Attributes.SetAttribute("href", url);

        // если текущее свойство имеет значение CurrentSort
        if (Current == Property)
        {
            string arrowSymbol = Up ? "▲" : "▼";
            output.PreContent.Append(arrowSymbol);
        }

    }
}