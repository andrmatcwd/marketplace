using Marketplace.Modules.Listings.Domain.Entities;

namespace Marketplace.Web.Data.Seeders;

internal static class VacancySeedData
{
    internal static List<ListingVacancy> Build(string categorySlug, string cityName, Random random)
    {
        var pool = GetPool(categorySlug, cityName);
        return [.. pool.OrderBy(_ => random.Next()).Take(random.Next(1, Math.Min(pool.Length, 3) + 1))];
    }

    private static ListingVacancy[] GetPool(string categorySlug, string cityName) => categorySlug switch
    {
        "medicine" =>
        [
            new() { Title = "Лікар загальної практики", Description = "Ведення прийому, консультування пацієнтів, ведення медичних карток.", EmploymentType = "Повна зайнятість", SalaryText = "від 25 000 грн", LocationText = cityName },
            new() { Title = "Медична сестра", Description = "Підготовка кабінету, виконання процедур, супровід лікаря.", EmploymentType = "Позмінно", SalaryText = "від 16 000 грн", LocationText = cityName },
            new() { Title = "Адміністратор клініки", Description = "Запис пацієнтів, зустріч гостей, робота з касою та документами.", EmploymentType = "Повна зайнятість", SalaryText = "від 14 000 грн", LocationText = cityName },
            new() { Title = "Санітар", Description = "Підтримка чистоти у приміщеннях, дезінфекція кабінетів.", EmploymentType = "Часткова зайнятість", SalaryText = "від 10 000 грн", LocationText = cityName },
        ],
        "beauty" =>
        [
            new() { Title = "Косметолог", Description = "Проведення косметологічних процедур, консультування клієнтів.", EmploymentType = "Повна зайнятість", SalaryText = "від 20 000 грн", LocationText = cityName },
            new() { Title = "Перукар-стиліст", Description = "Стрижки, фарбування, укладання для жінок та чоловіків.", EmploymentType = "Повна зайнятість", SalaryText = "від 18 000 грн", LocationText = cityName },
            new() { Title = "Масажист", Description = "Релаксаційний та лікувальний масаж за записом.", EmploymentType = "Позмінно", SalaryText = "від 17 000 грн", LocationText = cityName },
            new() { Title = "Адміністратор салону", Description = "Запис клієнтів, робота з касою, зустріч відвідувачів.", EmploymentType = "Повна зайнятість", SalaryText = "від 13 000 грн", LocationText = cityName },
        ],
        "home-services" =>
        [
            new() { Title = "Прибиральник / Прибиральниця", Description = "Прибирання квартир, будинків та офісів за графіком.", EmploymentType = "Позмінно", SalaryText = "від 12 000 грн", LocationText = cityName },
            new() { Title = "Сантехнік", Description = "Монтаж і ремонт сантехніки, усунення аварійних ситуацій.", EmploymentType = "Повна зайнятість", SalaryText = "від 18 000 грн", LocationText = cityName },
            new() { Title = "Електрик", Description = "Монтаж і ремонт електропроводки, підключення обладнання.", EmploymentType = "Повна зайнятість", SalaryText = "від 18 000 грн", LocationText = cityName },
            new() { Title = "Менеджер замовлень", Description = "Прийом заявок, координація бригад, спілкування з клієнтами.", EmploymentType = "Повна зайнятість", SalaryText = "від 15 000 грн", LocationText = cityName },
        ],
        "education" =>
        [
            new() { Title = "Викладач англійської мови", Description = "Проведення індивідуальних та групових уроків англійської.", EmploymentType = "Часткова зайнятість", SalaryText = "від 20 000 грн", LocationText = cityName },
            new() { Title = "Репетитор з математики", Description = "Підготовка учнів до ЗНО та ДПА з математики.", EmploymentType = "Часткова зайнятість", SalaryText = "від 18 000 грн", LocationText = cityName },
            new() { Title = "Тренер з програмування", Description = "Навчання основ Python, Web-розробки, алгоритмів.", EmploymentType = "Повна зайнятість", SalaryText = "від 25 000 грн", LocationText = cityName },
            new() { Title = "Менеджер курсів", Description = "Запис студентів, ведення розкладу, комунікація з викладачами.", EmploymentType = "Повна зайнятість", SalaryText = "від 14 000 грн", LocationText = cityName },
        ],
        "legal" =>
        [
            new() { Title = "Юрист-консультант", Description = "Правові консультації фізичних та юридичних осіб.", EmploymentType = "Повна зайнятість", SalaryText = "від 28 000 грн", LocationText = cityName },
            new() { Title = "Нотаріус / Помічник нотаріуса", Description = "Засвідчення документів, ведення нотаріальних дій.", EmploymentType = "Повна зайнятість", SalaryText = "від 22 000 грн", LocationText = cityName },
            new() { Title = "Юридичний асистент", Description = "Ведення документів, підготовка договорів, підтримка юристів.", EmploymentType = "Повна зайнятість", SalaryText = "від 16 000 грн", LocationText = cityName },
            new() { Title = "Адміністратор офісу", Description = "Запис клієнтів, зустріч гостей, ведення архіву документів.", EmploymentType = "Повна зайнятість", SalaryText = "від 13 000 грн", LocationText = cityName },
        ],
        "auto-services" =>
        [
            new() { Title = "Автомеханік", Description = "Діагностика та ремонт автомобілів різних марок.", EmploymentType = "Повна зайнятість", SalaryText = "від 22 000 грн", LocationText = cityName },
            new() { Title = "Майстер шиномонтажу", Description = "Шиномонтаж, балансування, продаж шин.", EmploymentType = "Позмінно", SalaryText = "від 15 000 грн", LocationText = cityName },
            new() { Title = "Детейлер", Description = "Хімчистка, полірування, нанесення захисних покриттів.", EmploymentType = "Повна зайнятість", SalaryText = "від 18 000 грн", LocationText = cityName },
            new() { Title = "Адміністратор автосервісу", Description = "Прийом замовлень, запис клієнтів, видача авто.", EmploymentType = "Повна зайнятість", SalaryText = "від 14 000 грн", LocationText = cityName },
        ],
        "event-services" =>
        [
            new() { Title = "Фотограф", Description = "Зйомка весіль, корпоративів, дитячих свят та портретів.", EmploymentType = "Фріланс / за заявками", SalaryText = "від 2 500 грн / захід", LocationText = cityName },
            new() { Title = "Менеджер заходів", Description = "Планування та організація заходів, координація команди.", EmploymentType = "Повна зайнятість", SalaryText = "від 20 000 грн", LocationText = cityName },
            new() { Title = "Кейтеринг-менеджер", Description = "Організація харчування на заходах, робота з постачальниками.", EmploymentType = "Повна зайнятість", SalaryText = "від 18 000 грн", LocationText = cityName },
            new() { Title = "Декоратор / Флорист", Description = "Оформлення залів квітами, тканинами, декоративними елементами.", EmploymentType = "Фріланс / за заявками", SalaryText = "від 1 500 грн / захід", LocationText = cityName },
        ],
        _ => // fitness
        [
            new() { Title = "Персональний тренер", Description = "Розробка програм тренувань та індивідуальна робота з клієнтами.", EmploymentType = "Повна зайнятість", SalaryText = "від 22 000 грн", LocationText = cityName },
            new() { Title = "Інструктор групових занять", Description = "Проведення занять з йоги, пілатесу, аеробіки.", EmploymentType = "Часткова зайнятість", SalaryText = "від 16 000 грн", LocationText = cityName },
            new() { Title = "Адміністратор залу", Description = "Запис клієнтів, контроль входу, допомога відвідувачам.", EmploymentType = "Позмінно", SalaryText = "від 13 000 грн", LocationText = cityName },
            new() { Title = "Тренер з бойових мистецтв", Description = "Ведення груп з боксу, MMA або самооборони.", EmploymentType = "Часткова зайнятість", SalaryText = "від 18 000 грн", LocationText = cityName },
        ]
    };
}
