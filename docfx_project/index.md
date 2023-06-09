# OpenGameFramework

## Quick Start

Dependencies are resolved using own IoC container.
Use `[Inject( typeof(API.System.IService) )]` or `[Inject( typeof(API.Domain.IService) )]` attribute for classes for inject dependencies in contructor.

ForExample:
```c#
public class Test()
{
    private IService1 _service1;
    private IService2 _service2;

    public Test(IService1 service1, IService2 _service2)
    {
        _service1 = service1;
        _service2 = service2;
    }

    public void SomeMethod()
    {
        _service1.ServiceMethod();
    }
}
```

For Create new Module go to Editor menu `Window -> OpenGameFramework -> Module Editor -> Create Module`

For see all modules in project go to Editor menu `Window -> OpenGameFramework -> Module Editor -> All Modules`

## Architecture

OpenGameFramework - это легковесный фреймворк, с возможностью быстро собирать и переиспользовать различные куски кода (модульность), чтобы собирать игру с разными механиками и метой.

Задача архитектуры:
    - Разрешить зависимости
    - Пробрасывать события
    - Управлять модулями проекта
    - Сохранять структуру проекта
    - Генерировать документацию
    - Хранить Persistens дату
    - Обеспечить целостность состояний (С минимизацией сайд эффектов)
    - Обеспечить ясное и многоуровневое API.
    - Легкость аритектурного говермента.

### Maintability

- Зависимости разрешаются автоматически (есть только 1 способ это сделать и он закрыт от пользовательского кода). Соответственно напортачить и использовать его везде (надо / не надо).
- Единая точка доступа, без необходимости что-то настраивать. Это позволяет не парится о всяких забытых ссылаках. Смердженных сцен, которые могут сломать ссылки. Все что нужно кинуть App.cs на сцену.
- Продуманная последовательность инициализаций. Позволяет навсегда забыть про проблему неинициализированных переменных. Без использования Order Scripting (который не переносится от проекта к проекту). Все на уровне транзакциионных цепочек вызовов (AppTasks).
- Легкое создание / изменение / замена / удаление модулей приложения. Минимизируя человеческий фактор.
- Низкая связанность, возможность легко заменять механики, мету и прочий функционал игры (Модули).
- Документируемость (ясная структура). Легкий доступ ко всей мета информации по проекту. Обеспечивает легкий онбоардинг новых членов команды.
- Ортогональность и Строгость решения. Позволяет делать ряд вещей только одним способом. Тем самым не давай плодить креатив там, где он не нужен.
- Отказ от Zenject, UniRX в пользу своих решений. Так как эти пакеты пытаются натянуть на весь проект. И стать глобальными зависимостями, с возможностью легко плодить костыли (снижая опасность выстрелить себе в ногу).

### Scalability

- Внутри модулей может быть любая архитектура. DI контейнер в фреймворке разрешает только зависимости на этапе инициализации (в конструктор).
Это позволяет не беспокоиться, что что-то не было учтено на этапе проектирования. И модули сами разберуться. Это обеспечивает хорошую масштабируемость проекта.

### Decisitions
- Решено было не использовать Zenject, потому что он пытается натянуть DI на все системы в проекте. Став глобальной зависимостью. Позволяя очень много костылей. И тем самым повышая сложность архитектруного гавермента. Был написан просто IoC (DI) контейнер. Который автоматически строит граф зависимостей. И разрешает его на этапе преинициализации.
- Внутри модулей может быть любая архитектура. Доступ к публичному АПИ других модулей можно получить через внедрение зависимостей.
- Получать оповещения о событиях, можно подписавшись на них через `App.Hooks.OnSomething += Action`. (подробнее о Хуках можно узнать в соответствующем модулей `Hooks`)
- Использовать UniTask для решение проблем с асинхронщиной и совместимости Coroutine с async / await.
