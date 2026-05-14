using Marketplace.Modules.Appointments.Domain.Entities;
using Marketplace.Modules.Appointments.Domain.Enums;
using Marketplace.Modules.Appointments.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Web.Data;

public sealed class AppointmentSeeder
{
    private readonly AppointmentsDbContext _db;

    public AppointmentSeeder(AppointmentsDbContext db)
    {
        _db = db;
    }

    public async Task SeedAsync()
    {
        if (await _db.Appointments.AnyAsync())
            return;

        var now = DateTime.UtcNow;

        var appointments = new List<Appointment>
        {
            new()
            {
                BusinessName = "Стоматологія «Посмішка»",
                ContactName  = "Марина Коваленко",
                Phone        = "+380 44 123 45 67",
                Email        = "info@posmishka-dental.ua",
                CategoryName = "Медицина",
                CityName     = "Київ",
                Address      = "вул. Хрещатик, 12",
                Website      = "https://posmishka-dental.ua",
                Description  = "Сімейна стоматологічна клініка. Працюємо 10 років, 5 лікарів у штаті.",
                Status       = AppointmentStatus.New,
                CreatedAtUtc = now.AddDays(-2),
                UpdatedAtUtc = now.AddDays(-2)
            },
            new()
            {
                BusinessName = "IT-компанія DevCraft",
                ContactName  = "Олег Савченко",
                Phone        = "+380 67 987 65 43",
                Email        = "oleg@devcraft.ua",
                CategoryName = "IT-послуги",
                CityName     = "Львів",
                Address      = "вул. Городоцька, 55",
                Website      = "https://devcraft.ua",
                Description  = "Розробка веб-додатків та мобільних застосунків на замовлення. Команда 20+ розробників.",
                Status       = AppointmentStatus.InReview,
                AdminNotes   = "Перевірити портфоліо. Є кілька цікавих кейсів.",
                CreatedAtUtc = now.AddDays(-5),
                UpdatedAtUtc = now.AddDays(-4)
            },
            new()
            {
                BusinessName = "Автосервіс «Мотор»",
                ContactName  = "Василь Бондаренко",
                Phone        = "+380 50 321 11 22",
                Email        = "motor.service@ukr.net",
                CategoryName = "Автопослуги",
                CityName     = "Харків",
                Address      = "проспект Науки, 78",
                Website      = null,
                Description  = "СТО повного циклу: ремонт, діагностика, шиномонтаж. 15 підйомників.",
                Status       = AppointmentStatus.Converted,
                AdminNotes   = "Лістинг створено: ID 142. Клієнт задоволений.",
                CreatedAtUtc = now.AddDays(-20),
                UpdatedAtUtc = now.AddDays(-18)
            },
            new()
            {
                BusinessName = "Школа іноземних мов «Lingua»",
                ContactName  = "Ірина Мельник",
                Phone        = "+380 73 555 00 11",
                Email        = "lingua.school.od@gmail.com",
                CategoryName = "Освіта",
                CityName     = "Одеса",
                Address      = "вул. Дерибасівська, 3, офіс 201",
                Website      = "https://lingua-school.com.ua",
                Description  = "Курси англійської, польської та іспанської мов. Онлайн та офлайн формат.",
                Status       = AppointmentStatus.New,
                CreatedAtUtc = now.AddDays(-1),
                UpdatedAtUtc = now.AddDays(-1)
            },
            new()
            {
                BusinessName = "Юридична фірма «Захист»",
                ContactName  = "Андрій Петренко",
                Phone        = "+380 44 777 88 99",
                Email        = "a.petrenko@zahyst-law.ua",
                CategoryName = "Юридичні послуги",
                CityName     = "Київ",
                Address      = "бул. Лесі Українки, 34",
                Website      = "https://zahyst-law.ua",
                Description  = "Супровід бізнесу, корпоративне право, судові спори. 12 адвокатів.",
                Status       = AppointmentStatus.Rejected,
                AdminNotes   = "Дублікат. Схожа компанія вже є в базі під ID 88.",
                CreatedAtUtc = now.AddDays(-10),
                UpdatedAtUtc = now.AddDays(-9)
            },
            new()
            {
                BusinessName = "Салон краси «Аура»",
                ContactName  = "Тетяна Яценко",
                Phone        = "+380 96 444 33 22",
                Email        = "aura.beauty.dp@gmail.com",
                CategoryName = "Краса та здоров'я",
                CityName     = "Дніпро",
                Address      = "вул. Короленка, 9",
                Website      = null,
                Description  = "Перукарня, манікюр, педикюр, косметологія. Прийом щодня з 9:00 до 20:00.",
                Status       = AppointmentStatus.InReview,
                AdminNotes   = "Зателефонувати в четвер для уточнення деталей.",
                CreatedAtUtc = now.AddDays(-3),
                UpdatedAtUtc = now.AddDays(-3)
            },
            new()
            {
                BusinessName = "Фітнес-клуб «Форма»",
                ContactName  = "Роман Гриценко",
                Phone        = "+380 63 200 10 30",
                Email        = "forma.fitness.lv@ukr.net",
                CategoryName = "Фітнес",
                CityName     = "Львів",
                Address      = "вул. Стрийська, 101",
                Website      = "https://forma-fitness.lviv.ua",
                Description  = "Тренажерний зал, групові заняття, персональні тренери. Площа 800 м².",
                Status       = AppointmentStatus.New,
                CreatedAtUtc = now.AddHours(-6),
                UpdatedAtUtc = now.AddHours(-6)
            },
            new()
            {
                BusinessName = "Клінінгова компанія «Чисто»",
                ContactName  = "Світлана Кравченко",
                Phone        = "+380 99 111 22 33",
                Email        = "chysto.clean@gmail.com",
                CategoryName = "Послуги для дому",
                CityName     = "Вінниця",
                Address      = "вул. Соборна, 22",
                Website      = "https://chysto.com.ua",
                Description  = "Прибирання квартир, офісів та після ремонту. Власний інвентар, досвід 7 років.",
                Status       = AppointmentStatus.Converted,
                AdminNotes   = "Лістинг ID 201 активний.",
                CreatedAtUtc = now.AddDays(-30),
                UpdatedAtUtc = now.AddDays(-28)
            },
            new()
            {
                BusinessName = "Фотостудія «Кадр»",
                ContactName  = "Денис Шевченко",
                Phone        = "+380 50 900 80 70",
                Email        = "kadr.photo.kh@gmail.com",
                CategoryName = "Івент-послуги",
                CityName     = "Харків",
                Address      = null,
                Website      = "https://kadr-photo.com",
                Description  = "Фотосесії, репортажна зйомка, оренда студії. Досвід 8 років, 500+ проєктів.",
                Status       = AppointmentStatus.New,
                CreatedAtUtc = now.AddHours(-14),
                UpdatedAtUtc = now.AddHours(-14)
            },
            new()
            {
                BusinessName = "Електромонтажна компанія «Струм»",
                ContactName  = "Ігор Тимченко",
                Phone        = "+380 67 333 44 55",
                Email        = "strum.electric@ukr.net",
                CategoryName = "Послуги для дому",
                CityName     = "Київ",
                Address      = "вул. Борщагівська, 154",
                Website      = null,
                Description  = "Монтаж електропроводки, встановлення щитків, підключення техніки. Ліцензія СРО.",
                Status       = AppointmentStatus.InReview,
                AdminNotes   = null,
                CreatedAtUtc = now.AddDays(-4),
                UpdatedAtUtc = now.AddDays(-4)
            }
        };

        _db.Appointments.AddRange(appointments);
        await _db.SaveChangesAsync();
    }
}
