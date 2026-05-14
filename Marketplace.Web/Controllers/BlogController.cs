using Marketplace.Modules.Blog.Application.BlogPosts.Filters;
using Marketplace.Modules.Blog.Application.BlogPosts.Queries.GetBlogPostBySlug;
using Marketplace.Modules.Blog.Application.BlogPosts.Queries.GetBlogPostsByFilter;
using Marketplace.Web.Models.Shared;
using Marketplace.Web.Seo;
using Marketplace.Web.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Web.Controllers;

public sealed class BlogController : Controller
{
    private readonly ISender _sender;
    private const int PageSize = 12;

    public BlogController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("/{culture:regex(^uk|ru$)}/blog")]
    public async Task<IActionResult> Index(string culture, string? search, int page = 1, CancellationToken cancellationToken = default)
    {
        culture = CultureHelper.NormalizeRouteCulture(culture);
        var isUk = culture == "uk";

        var result = await _sender.Send(new GetBlogPostsByFilterQuery(new BlogPostFilter
        {
            Search = search,
            IsPublished = true,
            Page = page,
            PageSize = PageSize
        }), cancellationToken);

        ViewData["Seo"] = new PageSeoData
        {
            Title = isUk
                ? "Блог — статті, поради та новини для бізнесу"
                : "Блог — статьи, советы и новости для бизнеса",
            Description = isUk
                ? "Читайте корисні матеріали про просування бізнесу, залучення клієнтів та роботу з локальним каталогом Marketplace."
                : "Читайте полезные материалы о продвижении бизнеса, привлечении клиентов и работе с локальным каталогом Marketplace.",
            Robots = "index, follow",
            OgType = "website"
        };

        ViewData["SeoIntro"] = new SeoIntroVm
        {
            Title = isUk
                ? "Блог Marketplace — практичні поради для бізнесу"
                : "Блог Marketplace — практические советы для бизнеса",
            Text = isUk
                ? "Тут ви знайдете корисні матеріали про залучення клієнтів, просування у локальних каталогах, роботу з відгуками та ефективне використання онлайн-інструментів. Статті написані для власників малого та середнього бізнесу, які хочуть рости й розвиватися."
                : "Здесь вы найдёте полезные материалы о привлечении клиентов, продвижении в локальных каталогах, работе с отзывами и эффективном использовании онлайн-инструментов. Статьи написаны для владельцев малого и среднего бизнеса, которые хотят расти и развиваться."
        };

        ViewData["SeoBottom"] = new SeoBottomVm
        {
            Title = isUk
                ? "Часті питання про блог"
                : "Частые вопросы о блоге",
            Text = isUk
                ? "Про що пишуть у блозі Marketplace? У блозі публікуються статті про маркетинг для малого бізнесу, поради щодо розміщення у каталозі, огляди трендів та практичні кейси. Як часто виходять нові публікації? Нові матеріали з'являються регулярно — слідкуйте за оновленнями у розділі блогу. Чи можна розмістити свій бізнес на Marketplace? Так — перейдіть до розділу «Для бізнесу», щоб дізнатися про умови та плани."
                : "О чём пишут в блоге Marketplace? В блоге публикуются статьи о маркетинге для малого бизнеса, советы по размещению в каталоге, обзоры трендов и практические кейсы. Как часто выходят новые публикации? Новые материалы появляются регулярно — следите за обновлениями в разделе блога. Можно ли разместить свой бизнес на Marketplace? Да — перейдите в раздел «Для бизнеса», чтобы узнать об условиях и тарифах."
        };

        ViewData["Search"] = search;
        ViewData["Page"] = page;
        ViewData["Culture"] = culture;

        return View(result);
    }

    [HttpGet("/{culture:regex(^uk|ru$)}/blog/{slug}")]
    public async Task<IActionResult> Post(string culture, string slug, CancellationToken cancellationToken = default)
    {
        culture = CultureHelper.NormalizeRouteCulture(culture);
        var isUk = culture == "uk";

        var post = await _sender.Send(new GetBlogPostBySlugQuery(slug), cancellationToken);
        if (post is null || !post.IsPublished) return NotFound();

        ViewData["Seo"] = new PageSeoData
        {
            Title = post.Title,
            Description = post.Excerpt,
            OgType = "article",
            OgImage = post.CoverImageUrl,
            Robots = "index, follow"
        };

        ViewData["SeoTop"] = new SeoIntroVm
        {
            Title = isUk
                ? "Про цю статтю"
                : "Об этой статье",
            Text = isUk
                ? "У цьому матеріалі ви знайдете практичні поради та інсайти для розвитку бізнесу. Стаття підготовлена командою Marketplace і розрахована на власників малого та середнього бізнесу, підприємців та всіх, хто хоче ефективніше залучати клієнтів у своєму місті."
                : "В этом материале вы найдёте практические советы и инсайты для развития бизнеса. Статья подготовлена командой Marketplace и рассчитана на владельцев малого и среднего бизнеса, предпринимателей и всех, кто хочет эффективнее привлекать клиентов в своём городе."
        };

        ViewData["SeoBottom"] = new SeoBottomVm
        {
            Title = isUk
                ? "Більше корисних матеріалів"
                : "Больше полезных материалов",
            Text = isUk
                ? "Відвідайте наш блог, щоб знайти ще більше статей про бізнес, маркетинг та залучення клієнтів. Дізнайтесь, як ефективно використовувати Marketplace для просування свого бізнесу в локальному каталозі послуг вашого міста."
                : "Посетите наш блог, чтобы найти ещё больше статей о бизнесе, маркетинге и привлечении клиентов. Узнайте, как эффективно использовать Marketplace для продвижения своего бизнеса в локальном каталоге услуг вашего города."
        };

        ViewData["Culture"] = culture;

        return View(post);
    }
}
