using AutoSchoolExam.Components;
using AutoSchoolExam.Data;
using AutoSchoolExam.Data.Repositories;
using AutoSchoolExam.Models;
using AutoSchoolExam.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Добавляем Blazor Server
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Подключаем SQLite базу данных
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=exams.db"));

// Регистрируем репозитории (Dependency Injection)
builder.Services.AddScoped<ITicketRepository, TicketRepository>();

// Регистрируем сервис состояния экзамена
builder.Services.AddScoped<ExamState>();

var app = builder.Build();

// Инициализация базы данных
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // Применяем миграции (создаем БД если нет)
    db.Database.Migrate();

    // Если билетов нет, создаем их
    if (!db.Tickets.Any())
    {
        CreateTickets(db);
        db.SaveChanges();
    }
}

// Настраиваем HTTP pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

// =====================================================
// МЕТОД СОЗДАНИЯ БИЛЕТОВ С РЕАЛЬНЫМИ ВОПРОСАМИ ПДД
// =====================================================
void CreateTickets(AppDbContext db)
{
    var tickets = new List<Ticket>();

    // ==================== БИЛЕТ 1 ====================
    var ticket1 = new Ticket
    {
        Name = "Билет №1",
        Description = "Экзаменационный билет по ПДД категории B"
    };

    ticket1.Questions = new List<Question>
    {
        new Question
        {
            Text = "В каком случае водитель совершит вынужденную остановку?",
            Options = new List<Option>
            {
                new Option { Text = "Остановившись на проезжей части из-за технической неисправности ТС", IsCorrect = true },
                new Option { Text = "Остановившись для высадки пассажиров", IsCorrect = false },
                new Option { Text = "Остановившись перед светофором", IsCorrect = false },
                new Option { Text = "Остановившись для разгрузки груза", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Разрешен ли Вам съезд на дорогу с грунтовым покрытием?",
            Options = new List<Option>
            {
                new Option { Text = "Разрешен", IsCorrect = true },
                new Option { Text = "Запрещен", IsCorrect = false },
                new Option { Text = "Разрешен только при отсутствии встречных ТС", IsCorrect = false },
                new Option { Text = "Разрешен только с разрешения инспектора", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Обязаны ли Вы уступить дорогу автомобилю с включенными спецсигналами?",
            Options = new List<Option>
            {
                new Option { Text = "Обязаны уступить дорогу", IsCorrect = true },
                new Option { Text = "Не обязаны", IsCorrect = false },
                new Option { Text = "Обязаны только на перекрестке", IsCorrect = false },
                new Option { Text = "Обязаны только в темное время суток", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Что означает мигающий зеленый сигнал светофора?",
            Options = new List<Option>
            {
                new Option { Text = "Время действия сигнала истекает", IsCorrect = true },
                new Option { Text = "Светофор неисправен", IsCorrect = false },
                new Option { Text = "Разрешает движение с осторожностью", IsCorrect = false },
                new Option { Text = "Запрещает движение", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "С какой максимальной скоростью можно двигаться в жилой зоне?",
            Options = new List<Option>
            {
                new Option { Text = "20 км/ч", IsCorrect = true },
                new Option { Text = "40 км/ч", IsCorrect = false },
                new Option { Text = "60 км/ч", IsCorrect = false },
                new Option { Text = "70 км/ч", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Разрешается ли обгон на перекрестке?",
            Options = new List<Option>
            {
                new Option { Text = "Запрещается на равнозначных перекрестках", IsCorrect = true },
                new Option { Text = "Разрешается всегда", IsCorrect = false },
                new Option { Text = "Запрещается всегда", IsCorrect = false },
                new Option { Text = "Разрешается только на регулируемых", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Кто имеет преимущество на нерегулируемом перекрестке?",
            Options = new List<Option>
            {
                new Option { Text = "Трамвай в равнозначных условиях", IsCorrect = true },
                new Option { Text = "Легковой автомобиль", IsCorrect = false },
                new Option { Text = "Грузовой автомобиль", IsCorrect = false },
                new Option { Text = "Мотоцикл", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Что означает знак «Движение без остановки запрещено»?",
            Options = new List<Option>
            {
                new Option { Text = "Обязательная остановка перед перекрестком", IsCorrect = true },
                new Option { Text = "Запрет на остановку", IsCorrect = false },
                new Option { Text = "Разрешение на движение", IsCorrect = false },
                new Option { Text = "Ограничение скорости", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Можно ли парковаться на тротуаре?",
            Options = new List<Option>
            {
                new Option { Text = "Только если есть соответствующий знак", IsCorrect = true },
                new Option { Text = "Можно всегда", IsCorrect = false },
                new Option { Text = "Нельзя никогда", IsCorrect = false },
                new Option { Text = "Можно только ночью", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Какой сигнал подает водитель поднятой вверх рукой?",
            Options = new List<Option>
            {
                new Option { Text = "Снижение скорости, остановка", IsCorrect = true },
                new Option { Text = "Поворот направо", IsCorrect = false },
                new Option { Text = "Поворот налево", IsCorrect = false },
                new Option { Text = "Движение прямо", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Разрешен ли разворот на железнодорожном переезде?",
            Options = new List<Option>
            {
                new Option { Text = "Запрещен", IsCorrect = true },
                new Option { Text = "Разрешен", IsCorrect = false },
                new Option { Text = "Разрешен если нет поезда", IsCorrect = false },
                new Option { Text = "Разрешен только днем", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "С какой скоростью двигаться вне населенного пункта на легковом авто?",
            Options = new List<Option>
            {
                new Option { Text = "Не более 90 км/ч", IsCorrect = true },
                new Option { Text = "Не более 60 км/ч", IsCorrect = false },
                new Option { Text = "Не более 110 км/ч", IsCorrect = false },
                new Option { Text = "Не более 70 км/ч", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Кому нужно уступать при повороте направо?",
            Options = new List<Option>
            {
                new Option { Text = "Пешеходам и велосипедистам", IsCorrect = true },
                new Option { Text = "Никому", IsCorrect = false },
                new Option { Text = "Только пешеходам", IsCorrect = false },
                new Option { Text = "Только велосипедистам", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Что такое «Главная дорога»?",
            Options = new List<Option>
            {
                new Option { Text = "Дорога с твердым покрытием относительно грунтовой", IsCorrect = true },
                new Option { Text = "Дорога с большим количеством полос", IsCorrect = false },
                new Option { Text = "Дорога с интенсивным движением", IsCorrect = false },
                new Option { Text = "Дорога в городе", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Можно ли двигаться по полосе встречного движения?",
            Options = new List<Option>
            {
                new Option { Text = "Только при обгоне если это разрешено", IsCorrect = true },
                new Option { Text = "Можно всегда", IsCorrect = false },
                new Option { Text = "Нельзя никогда", IsCorrect = false },
                new Option { Text = "Можно только ночью", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Что означает желтый сигнал светофора?",
            Options = new List<Option>
            {
                new Option { Text = "Запрещает движение, кроме экстренного торможения", IsCorrect = true },
                new Option { Text = "Разрешает движение", IsCorrect = false },
                new Option { Text = "Предупреждает о неисправности", IsCorrect = false },
                new Option { Text = "Разрешает движение с осторожностью", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Разрешена ли стоянка на мосту?",
            Options = new List<Option>
            {
                new Option { Text = "Запрещена", IsCorrect = true },
                new Option { Text = "Разрешена", IsCorrect = false },
                new Option { Text = "Разрешена если нет помех", IsCorrect = false },
                new Option { Text = "Разрешена только ночью", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Какое расстояние должно быть между ТС при остановке?",
            Options = new List<Option>
            {
                new Option { Text = "Не менее 3 метров до сплошной линии", IsCorrect = true },
                new Option { Text = "Не менее 1 метра", IsCorrect = false },
                new Option { Text = "Не менее 5 метров", IsCorrect = false },
                new Option { Text = "Любое расстояние", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Кто имеет преимущество на перекрестке с круговым движением?",
            Options = new List<Option>
            {
                new Option { Text = "ТС движущиеся по кругу (если нет знаков)", IsCorrect = true },
                new Option { Text = "Въезжающие на круг", IsCorrect = false },
                new Option { Text = "Легковые автомобили", IsCorrect = false },
                new Option { Text = "Грузовые автомобили", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Что необходимо сообщить при вызове скорой помощи?",
            Options = new List<Option>
            {
                new Option { Text = "Место, количество пострадавших, их состояние", IsCorrect = true },
                new Option { Text = "Только место ДТП", IsCorrect = false },
                new Option { Text = "Только количество машин", IsCorrect = false },
                new Option { Text = "Только свои данные", IsCorrect = false }
            }
        }
    };

    tickets.Add(ticket1);

    // ==================== БИЛЕТ 2 ====================
    var ticket2 = new Ticket
    {
        Name = "Билет №2",
        Description = "Экзаменационный билет по ПДД категории B"
    };

    ticket2.Questions = new List<Question>
    {
        new Question
        {
            Text = "Разрешается ли движение задним ходом на перекрестке?",
            Options = new List<Option>
            {
                new Option { Text = "Запрещается", IsCorrect = true },
                new Option { Text = "Разрешается", IsCorrect = false },
                new Option { Text = "Разрешается если нет помех", IsCorrect = false },
                new Option { Text = "Разрешается только днем", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Что означает прерывистая линия разметки?",
            Options = new List<Option>
            {
                new Option { Text = "Разрешает перестроение и обгон", IsCorrect = true },
                new Option { Text = "Запрещает перестроение", IsCorrect = false },
                new Option { Text = "Разделяет встречные потоки", IsCorrect = false },
                new Option { Text = "Обозначает край проезжей части", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "С какой стороны разрешен обгон на дороге с двусторонним движением?",
            Options = new List<Option>
            {
                new Option { Text = "С левой стороны", IsCorrect = true },
                new Option { Text = "С правой стороны", IsCorrect = false },
                new Option { Text = "С любой стороны", IsCorrect = false },
                new Option { Text = "Обгон запрещен", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Обязаны ли водители уступать дорогу пешеходам на нерегулируемом переходе?",
            Options = new List<Option>
            {
                new Option { Text = "Обязаны всегда", IsCorrect = true },
                new Option { Text = "Не обязаны", IsCorrect = false },
                new Option { Text = "Обязаны только если пешеход на проезжей части", IsCorrect = false },
                new Option { Text = "Обязаны только в темное время", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Разрешена ли остановка на остановке общественного транспорта?",
            Options = new List<Option>
            {
                new Option { Text = "Запрещена ближе 15м до и после знака", IsCorrect = true },
                new Option { Text = "Разрешена", IsCorrect = false },
                new Option { Text = "Разрешена только для посадки", IsCorrect = false },
                new Option { Text = "Разрешена только ночью", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Что означает знак «Главная дорога»?",
            Options = new List<Option>
            {
                new Option { Text = "Преимущество проезда перекрестка", IsCorrect = true },
                new Option { Text = "Запрет на въезд", IsCorrect = false },
                new Option { Text = "Ограничение скорости", IsCorrect = false },
                new Option { Text = "Конец главной дороги", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Можно ли парковаться на газоне?",
            Options = new List<Option>
            {
                new Option { Text = "Запрещено", IsCorrect = true },
                new Option { Text = "Разрешено", IsCorrect = false },
                new Option { Text = "Разрешено если нет знака", IsCorrect = false },
                new Option { Text = "Разрешено только ночью", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Какой сигнал поворота нужно включить при перестроении направо?",
            Options = new List<Option>
            {
                new Option { Text = "Правый поворот", IsCorrect = true },
                new Option { Text = "Левый поворот", IsCorrect = false },
                new Option { Text = "Аварийную сигнализацию", IsCorrect = false },
                new Option { Text = "Никакой не нужен", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Разрешен ли обгон на пешеходном переходе?",
            Options = new List<Option>
            {
                new Option { Text = "Запрещен", IsCorrect = true },
                new Option { Text = "Разрешен", IsCorrect = false },
                new Option { Text = "Разрешен если нет пешеходов", IsCorrect = false },
                new Option { Text = "Разрешен только днем", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "С какой минимальной глубиной протектора разрешена эксплуатация шин?",
            Options = new List<Option>
            {
                new Option { Text = "1.6 мм", IsCorrect = true },
                new Option { Text = "2.0 мм", IsCorrect = false },
                new Option { Text = "3.0 мм", IsCorrect = false },
                new Option { Text = "4.0 мм", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Что означает сплошная линия разметки?",
            Options = new List<Option>
            {
                new Option { Text = "Запрещает пересечение", IsCorrect = true },
                new Option { Text = "Разрешает обгон", IsCorrect = false },
                new Option { Text = "Разрешает перестроение", IsCorrect = false },
                new Option { Text = "Обозначает парковку", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Разрешается ли разворот на мосту?",
            Options = new List<Option>
            {
                new Option { Text = "Запрещается", IsCorrect = true },
                new Option { Text = "Разрешается", IsCorrect = false },
                new Option { Text = "Разрешается если нет помех", IsCorrect = false },
                new Option { Text = "Разрешается только днем", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Кто должен уступить дорогу при одновременном перестроении?",
            Options = new List<Option>
            {
                new Option { Text = "Водитель при перестроении на соседнюю полосу", IsCorrect = true },
                new Option { Text = "Водитель на главной полосе", IsCorrect = false },
                new Option { Text = "Оба водителя", IsCorrect = false },
                new Option { Text = "Никто", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Разрешено ли движение с включенными противотуманными фарами днем?",
            Options = new List<Option>
            {
                new Option { Text = "Разрешено", IsCorrect = true },
                new Option { Text = "Запрещено", IsCorrect = false },
                new Option { Text = "Разрешено только в туман", IsCorrect = false },
                new Option { Text = "Разрешено только ночью", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Что означает знак «Уступи дорогу»?",
            Options = new List<Option>
            {
                new Option { Text = "Уступить ТС на пересекаемой дороге", IsCorrect = true },
                new Option { Text = "Полная остановка", IsCorrect = false },
                new Option { Text = "Запрет на въезд", IsCorrect = false },
                new Option { Text = "Преимущество проезда", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Разрешена ли буксировка на автомагистрали?",
            Options = new List<Option>
            {
                new Option { Text = "Запрещена", IsCorrect = true },
                new Option { Text = "Разрешена", IsCorrect = false },
                new Option { Text = "Разрешена только днем", IsCorrect = false },
                new Option { Text = "Разрешена если скорость меньше 60", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "С какой скоростью можно двигаться в населенном пункте?",
            Options = new List<Option>
            {
                new Option { Text = "Не более 60 км/ч", IsCorrect = true },
                new Option { Text = "Не более 40 км/ч", IsCorrect = false },
                new Option { Text = "Не более 80 км/ч", IsCorrect = false },
                new Option { Text = "Не более 90 км/ч", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Обязательно ли использование ремней безопасности?",
            Options = new List<Option>
            {
                new Option { Text = "Обязательно для всех", IsCorrect = true },
                new Option { Text = "Только для водителя", IsCorrect = false },
                new Option { Text = "Только на трассе", IsCorrect = false },
                new Option { Text = "Не обязательно", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Разрешен ли обгон через сплошную линию?",
            Options = new List<Option>
            {
                new Option { Text = "Запрещен", IsCorrect = true },
                new Option { Text = "Разрешен", IsCorrect = false },
                new Option { Text = "Разрешен если нет встречных", IsCorrect = false },
                new Option { Text = "Разрешен только днем", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Что означает красный сигнал светофора?",
            Options = new List<Option>
            {
                new Option { Text = "Запрещает движение", IsCorrect = true },
                new Option { Text = "Разрешает движение", IsCorrect = false },
                new Option { Text = "Предупреждает", IsCorrect = false },
                new Option { Text = "Разрешает поворот", IsCorrect = false }
            }
        }
    };

    tickets.Add(ticket2);

    // ==================== БИЛЕТ 3 ====================
    var ticket3 = new Ticket
    {
        Name = "Билет №3",
        Description = "Экзаменационный билет по ПДД категории B"
    };

    ticket3.Questions = new List<Question>
    {
        new Question
        {
            Text = "Разрешается ли остановка на трамвайных путях?",
            Options = new List<Option>
            {
                new Option { Text = "Запрещена", IsCorrect = true },
                new Option { Text = "Разрешена", IsCorrect = false },
                new Option { Text = "Разрешена если нет трамвая", IsCorrect = false },
                new Option { Text = "Разрешена только для посадки", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Что означает знак «Въезд запрещен»?",
            Options = new List<Option>
            {
                new Option { Text = "Запрещает въезд всех ТС", IsCorrect = true },
                new Option { Text = "Разрешает въезд", IsCorrect = false },
                new Option { Text = "Запрещает только грузовым", IsCorrect = false },
                new Option { Text = "Запрещает остановку", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Разрешен ли разворот на автомагистрали?",
            Options = new List<Option>
            {
                new Option { Text = "Запрещен", IsCorrect = true },
                new Option { Text = "Разрешен", IsCorrect = false },
                new Option { Text = "Разрешен через разрыв", IsCorrect = false },
                new Option { Text = "Разрешен только ночью", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "С какой стороны нужно обходить трамвай?",
            Options = new List<Option>
            {
                new Option { Text = "Спереди", IsCorrect = true },
                new Option { Text = "Сзади", IsCorrect = false },
                new Option { Text = "С любой стороны", IsCorrect = false },
                new Option { Text = "Трамвай обходить нельзя", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Разрешена ли стоянка с работающим двигателем в жилой зоне?",
            Options = new List<Option>
            {
                new Option { Text = "Запрещена более 5 минут", IsCorrect = true },
                new Option { Text = "Разрешена", IsCorrect = false },
                new Option { Text = "Разрешена только зимой", IsCorrect = false },
                new Option { Text = "Разрешена только ночью", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Что означает знак «Опасный поворот»?",
            Options = new List<Option>
            {
                new Option { Text = "Предупреждает о повороте", IsCorrect = true },
                new Option { Text = "Запрещает поворот", IsCorrect = false },
                new Option { Text = "Разрешает поворот", IsCorrect = false },
                new Option { Text = "Обозначает направление", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Обязаны ли вы пропустить машину со спецсигналами?",
            Options = new List<Option>
            {
                new Option { Text = "Обязаны уступить дорогу", IsCorrect = true },
                new Option { Text = "Не обязаны", IsCorrect = false },
                new Option { Text = "Обязаны только на перекрестке", IsCorrect = false },
                new Option { Text = "Обязаны только если она сзади", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Разрешен ли обгон на мосту?",
            Options = new List<Option>
            {
                new Option { Text = "Запрещен", IsCorrect = true },
                new Option { Text = "Разрешен", IsCorrect = false },
                new Option { Text = "Разрешен если нет помех", IsCorrect = false },
                new Option { Text = "Разрешен только днем", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Что означает двойная сплошная линия?",
            Options = new List<Option>
            {
                new Option { Text = "Запрещает пересечение с обеих сторон", IsCorrect = true },
                new Option { Text = "Разрешает обгон", IsCorrect = false },
                new Option { Text = "Разрешает перестроение", IsCorrect = false },
                new Option { Text = "Обозначает парковку", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Разрешается ли движение задним ходом на автомагистрали?",
            Options = new List<Option>
            {
                new Option { Text = "Запрещается", IsCorrect = true },
                new Option { Text = "Разрешается", IsCorrect = false },
                new Option { Text = "Разрешается для разворота", IsCorrect = false },
                new Option { Text = "Разрешается если нет помех", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Кто имеет преимущество на перекрестке с круговым движением?",
            Options = new List<Option>
            {
                new Option { Text = "ТС на круге (если нет знаков)", IsCorrect = true },
                new Option { Text = "Въезжающие на круг", IsCorrect = false },
                new Option { Text = "Трамвай", IsCorrect = false },
                new Option { Text = "Грузовые ТС", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Разрешена ли парковка на краю тротуара?",
            Options = new List<Option>
            {
                new Option { Text = "Только если есть знак", IsCorrect = true },
                new Option { Text = "Разрешена", IsCorrect = false },
                new Option { Text = "Запрещена", IsCorrect = false },
                new Option { Text = "Разрешена только мотоциклам", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Что означает знак «Движение запрещено»?",
            Options = new List<Option>
            {
                new Option { Text = "Запрещает движение всех ТС", IsCorrect = true },
                new Option { Text = "Разрешает движение", IsCorrect = false },
                new Option { Text = "Запрещает остановку", IsCorrect = false },
                new Option { Text = "Запрещает только грузовым", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Разрешен ли обгон на железнодорожном переезде?",
            Options = new List<Option>
            {
                new Option { Text = "Запрещен", IsCorrect = true },
                new Option { Text = "Разрешен", IsCorrect = false },
                new Option { Text = "Разрешен если нет поезда", IsCorrect = false },
                new Option { Text = "Разрешен только днем", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "С какой скоростью можно двигаться по автомагистрали?",
            Options = new List<Option>
            {
                new Option { Text = "Не более 110 км/ч", IsCorrect = true },
                new Option { Text = "Не более 90 км/ч", IsCorrect = false },
                new Option { Text = "Не более 130 км/ч", IsCorrect = false },
                new Option { Text = "Не более 70 км/ч", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Обязаны ли вы включить фары в тоннеле?",
            Options = new List<Option>
            {
                new Option { Text = "Обязаны", IsCorrect = true },
                new Option { Text = "Не обязаны", IsCorrect = false },
                new Option { Text = "Только если темно", IsCorrect = false },
                new Option { Text = "Только габариты", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Разрешается ли буксировка с неисправными тормозами?",
            Options = new List<Option>
            {
                new Option { Text = "Запрещается", IsCorrect = true },
                new Option { Text = "Разрешается", IsCorrect = false },
                new Option { Text = "Разрешается на тросе", IsCorrect = false },
                new Option { Text = "Разрешается на жесткой сцепке", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Что означает знак «Пешеходный переход»?",
            Options = new List<Option>
            {
                new Option { Text = "Место перехода пешеходов", IsCorrect = true },
                new Option { Text = "Запрет на движение", IsCorrect = false },
                new Option { Text = "Разрешение на парковку", IsCorrect = false },
                new Option { Text = "Опасный участок", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Разрешен ли разворот на пешеходном переходе?",
            Options = new List<Option>
            {
                new Option { Text = "Запрещен", IsCorrect = true },
                new Option { Text = "Разрешен", IsCorrect = false },
                new Option { Text = "Разрешен если нет пешеходов", IsCorrect = false },
                new Option { Text = "Разрешен только днем", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Кто должен уступить дорогу при выезде с прилегающей территории?",
            Options = new List<Option>
            {
                new Option { Text = "Водитель выезжающий с территории", IsCorrect = true },
                new Option { Text = "Водитель на дороге", IsCorrect = false },
                new Option { Text = "Оба водителя", IsCorrect = false },
                new Option { Text = "Никто", IsCorrect = false }
            }
        }
    };

    tickets.Add(ticket3);

    // ==================== БИЛЕТ 4 ====================
    var ticket4 = new Ticket
    {
        Name = "Билет №4",
        Description = "Экзаменационный билет по ПДД категории B"
    };

    ticket4.Questions = new List<Question>
    {
        new Question
        {
            Text = "Разрешена ли остановка на велосипедной дорожке?",
            Options = new List<Option>
            {
                new Option { Text = "Запрещена", IsCorrect = true },
                new Option { Text = "Разрешена", IsCorrect = false },
                new Option { Text = "Разрешена только ночью", IsCorrect = false },
                new Option { Text = "Разрешена для посадки", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Что означает знак «Стоянка запрещена»?",
            Options = new List<Option>
            {
                new Option { Text = "Запрещает стоянку", IsCorrect = true },
                new Option { Text = "Запрещает остановку", IsCorrect = false },
                new Option { Text = "Разрешает парковку", IsCorrect = false },
                new Option { Text = "Запрещает движение", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Разрешен ли обгон на регулируемом перекрестке?",
            Options = new List<Option>
            {
                new Option { Text = "Запрещен", IsCorrect = true },
                new Option { Text = "Разрешен", IsCorrect = false },
                new Option { Text = "Разрешен если зеленый", IsCorrect = false },
                new Option { Text = "Разрешен только днем", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "С какой стороны нужно объезжать островок безопасности?",
            Options = new List<Option>
            {
                new Option { Text = "Слева", IsCorrect = true },
                new Option { Text = "Справа", IsCorrect = false },
                new Option { Text = "С любой стороны", IsCorrect = false },
                new Option { Text = "Объезжать нельзя", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Разрешена ли стоянка ближе 50м от переезда?",
            Options = new List<Option>
            {
                new Option { Text = "Запрещена", IsCorrect = true },
                new Option { Text = "Разрешена", IsCorrect = false },
                new Option { Text = "Разрешена если нет поезда", IsCorrect = false },
                new Option { Text = "Разрешена только днем", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Что означает знак «Движение без остановки запрещено»?",
            Options = new List<Option>
            {
                new Option { Text = "Обязательная остановка", IsCorrect = true },
                new Option { Text = "Разрешает движение", IsCorrect = false },
                new Option { Text = "Запрещает въезд", IsCorrect = false },
                new Option { Text = "Предупреждает", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Обязаны ли вы пропустить пешехода переходящего дорогу?",
            Options = new List<Option>
            {
                new Option { Text = "Обязаны", IsCorrect = true },
                new Option { Text = "Не обязаны", IsCorrect = false },
                new Option { Text = "Только на переходе", IsCorrect = false },
                new Option { Text = "Только по зеленому", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Разрешен ли разворот на остановке общественного транспорта?",
            Options = new List<Option>
            {
                new Option { Text = "Запрещен", IsCorrect = true },
                new Option { Text = "Разрешен", IsCorrect = false },
                new Option { Text = "Разрешен если нет автобуса", IsCorrect = false },
                new Option { Text = "Разрешен только днем", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Что означает прерывистая линия с длинными штрихами?",
            Options = new List<Option>
            {
                new Option { Text = "Разрешает обгон и перестроение", IsCorrect = true },
                new Option { Text = "Запрещает обгон", IsCorrect = false },
                new Option { Text = "Запрещает перестроение", IsCorrect = false },
                new Option { Text = "Обозначает край дороги", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Разрешается ли движение по обочине?",
            Options = new List<Option>
            {
                new Option { Text = "Запрещается", IsCorrect = true },
                new Option { Text = "Разрешается", IsCorrect = false },
                new Option { Text = "Разрешается для обгона", IsCorrect = false },
                new Option { Text = "Разрешается для объезда", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Кто имеет преимущество на нерегулируемом перекрестке равнозначных дорог?",
            Options = new List<Option>
            {
                new Option { Text = "Трамвай", IsCorrect = true },
                new Option { Text = "Легковой автомобиль", IsCorrect = false },
                new Option { Text = "Грузовой автомобиль", IsCorrect = false },
                new Option { Text = "Тот кто справа", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Разрешена ли парковка на проезжей части?",
            Options = new List<Option>
            {
                new Option { Text = "Разрешена если нет запрещающих знаков", IsCorrect = true },
                new Option { Text = "Разрешена всегда", IsCorrect = false },
                new Option { Text = "Запрещена", IsCorrect = false },
                new Option { Text = "Разрешена только ночью", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Что означает знак «Ограничение скорости»?",
            Options = new List<Option>
            {
                new Option { Text = "Максимальная разрешенная скорость", IsCorrect = true },
                new Option { Text = "Минимальная скорость", IsCorrect = false },
                new Option { Text = "Рекомендуемая скорость", IsCorrect = false },
                new Option { Text = "Конец ограничения", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Разрешен ли обгон в тоннеле?",
            Options = new List<Option>
            {
                new Option { Text = "Запрещен", IsCorrect = true },
                new Option { Text = "Разрешен", IsCorrect = false },
                new Option { Text = "Разрешен если есть освещение", IsCorrect = false },
                new Option { Text = "Разрешен только днем", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "С какой скоростью можно двигаться в жилой зоне ночью?",
            Options = new List<Option>
            {
                new Option { Text = "Не более 20 км/ч", IsCorrect = true },
                new Option { Text = "Не более 40 км/ч", IsCorrect = false },
                new Option { Text = "Не более 60 км/ч", IsCorrect = false },
                new Option { Text = "Ограничений нет", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Обязаны ли вы включить аварийную сигнализацию при ДТП?",
            Options = new List<Option>
            {
                new Option { Text = "Обязаны", IsCorrect = true },
                new Option { Text = "Не обязаны", IsCorrect = false },
                new Option { Text = "Только если есть пострадавшие", IsCorrect = false },
                new Option { Text = "Только ночью", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Разрешается ли буксировка мотоцикла без коляски?",
            Options = new List<Option>
            {
                new Option { Text = "Запрещается", IsCorrect = true },
                new Option { Text = "Разрешается", IsCorrect = false },
                new Option { Text = "Разрешается на тросе", IsCorrect = false },
                new Option { Text = "Разрешается на жесткой сцепке", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Что означает знак «Направление поворота»?",
            Options = new List<Option>
            {
                new Option { Text = "Разрешенное направление движения", IsCorrect = true },
                new Option { Text = "Запрещенный поворот", IsCorrect = false },
                new Option { Text = "Объезд препятствия", IsCorrect = false },
                new Option { Text = "Конец дороги", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Разрешен ли разворот на автомагистрали?",
            Options = new List<Option>
            {
                new Option { Text = "Запрещен", IsCorrect = true },
                new Option { Text = "Разрешен", IsCorrect = false },
                new Option { Text = "Разрешен через разрыв", IsCorrect = false },
                new Option { Text = "Разрешен только ночью", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Кто должен уступить дорогу при перестроении?",
            Options = new List<Option>
            {
                new Option { Text = "Водитель перестраивающийся", IsCorrect = true },
                new Option { Text = "Водитель на своей полосе", IsCorrect = false },
                new Option { Text = "Оба водителя", IsCorrect = false },
                new Option { Text = "Никто", IsCorrect = false }
            }
        }
    };

    tickets.Add(ticket4);

    // ==================== БИЛЕТ 5 ====================
    var ticket5 = new Ticket
    {
        Name = "Билет №5",
        Description = "Экзаменационный билет по ПДД категории B"
    };

    ticket5.Questions = new List<Question>
    {
        new Question
        {
            Text = "Разрешена ли остановка на проезжей части вне населенного пункта?",
            Options = new List<Option>
            {
                new Option { Text = "Разрешена на обочине", IsCorrect = true },
                new Option { Text = "Разрешена на полосе", IsCorrect = false },
                new Option { Text = "Запрещена", IsCorrect = false },
                new Option { Text = "Разрешена только ночью", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Что означает знак «Стоянка запрещена с работой двигателя»?",
            Options = new List<Option>
            {
                new Option { Text = "Запрещает стоянку с работающим двигателем", IsCorrect = true },
                new Option { Text = "Разрешает стоянку", IsCorrect = false },
                new Option { Text = "Запрещает остановку", IsCorrect = false },
                new Option { Text = "Запрещает движение", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Разрешен ли обгон на подъеме?",
            Options = new List<Option>
            {
                new Option { Text = "Запрещен на крутом подъеме", IsCorrect = true },
                new Option { Text = "Разрешен", IsCorrect = false },
                new Option { Text = "Разрешен если нет встречных", IsCorrect = false },
                new Option { Text = "Разрешен только днем", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "С какой стороны нужно обходить автобус на остановке?",
            Options = new List<Option>
            {
                new Option { Text = "Сзади", IsCorrect = true },
                new Option { Text = "Спереди", IsCorrect = false },
                new Option { Text = "С любой стороны", IsCorrect = false },
                new Option { Text = "Автобус обходить нельзя", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Разрешена ли стоянка на газоне?",
            Options = new List<Option>
            {
                new Option { Text = "Запрещена", IsCorrect = true },
                new Option { Text = "Разрешена", IsCorrect = false },
                new Option { Text = "Разрешена если нет знака", IsCorrect = false },
                new Option { Text = "Разрешена только ночью", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Что означает знак «Искусственная неровность»?",
            Options = new List<Option>
            {
                new Option { Text = "Предупреждает о лежачем полицейском", IsCorrect = true },
                new Option { Text = "Запрещает движение", IsCorrect = false },
                new Option { Text = "Разрешает объезд", IsCorrect = false },
                new Option { Text = "Обозначает яму", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Обязаны ли вы уступить дорогу трамваю?",
            Options = new List<Option>
            {
                new Option { Text = "Обязаны в равнозначных условиях", IsCorrect = true },
                new Option { Text = "Не обязаны", IsCorrect = false },
                new Option { Text = "Обязаны только на перекрестке", IsCorrect = false },
                new Option { Text = "Обязаны только если он слева", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Разрешен ли обгон на эстакаде?",
            Options = new List<Option>
            {
                new Option { Text = "Запрещен", IsCorrect = true },
                new Option { Text = "Разрешен", IsCorrect = false },
                new Option { Text = "Разрешен если нет помех", IsCorrect = false },
                new Option { Text = "Разрешен только днем", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Что означает разметка «Зебра»?",
            Options = new List<Option>
            {
                new Option { Text = "Пешеходный переход", IsCorrect = true },
                new Option { Text = "Велосипедная дорожка", IsCorrect = false },
                new Option { Text = "Остановка транспорта", IsCorrect = false },
                new Option { Text = "Опасный участок", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Разрешается ли движение по трамвайным путям?",
            Options = new List<Option>
            {
                new Option { Text = "Разрешается попутного направления", IsCorrect = true },
                new Option { Text = "Разрешается всегда", IsCorrect = false },
                new Option { Text = "Запрещается", IsCorrect = false },
                new Option { Text = "Разрешается для обгона", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Кто имеет преимущество при выезде из двора?",
            Options = new List<Option>
            {
                new Option { Text = "ТС на дороге", IsCorrect = true },
                new Option { Text = "Выезжающий из двора", IsCorrect = false },
                new Option { Text = "Оба водителя", IsCorrect = false },
                new Option { Text = "Никто", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Разрешена ли парковка на тротуаре?",
            Options = new List<Option>
            {
                new Option { Text = "Только если есть знак", IsCorrect = true },
                new Option { Text = "Разрешена", IsCorrect = false },
                new Option { Text = "Запрещена", IsCorrect = false },
                new Option { Text = "Разрешена только мотоциклам", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Что означает знак «Конец зоны ограничения скорости»?",
            Options = new List<Option>
            {
                new Option { Text = "Отменяет ограничение", IsCorrect = true },
                new Option { Text = "Устанавливает ограничение", IsCorrect = false },
                new Option { Text = "Запрещает движение", IsCorrect = false },
                new Option { Text = "Предупреждает", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Разрешен ли обгон на перекрестке?",
            Options = new List<Option>
            {
                new Option { Text = "Запрещен", IsCorrect = true },
                new Option { Text = "Разрешен", IsCorrect = false },
                new Option { Text = "Разрешен на регулируемом", IsCorrect = false },
                new Option { Text = "Разрешен только днем", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "С какой скоростью можно двигаться в населенном пункте на мотоцикле?",
            Options = new List<Option>
            {
                new Option { Text = "Не более 60 км/ч", IsCorrect = true },
                new Option { Text = "Не более 40 км/ч", IsCorrect = false },
                new Option { Text = "Не более 80 км/ч", IsCorrect = false },
                new Option { Text = "Не более 90 км/ч", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Обязаны ли вы включить фары в темное время суток?",
            Options = new List<Option>
            {
                new Option { Text = "Обязаны", IsCorrect = true },
                new Option { Text = "Не обязаны", IsCorrect = false },
                new Option { Text = "Только габариты", IsCorrect = false },
                new Option { Text = "Только в городе", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Разрешается ли буксировка с неисправным рулевым управлением?",
            Options = new List<Option>
            {
                new Option { Text = "Запрещается", IsCorrect = true },
                new Option { Text = "Разрешается", IsCorrect = false },
                new Option { Text = "Разрешается на тросе", IsCorrect = false },
                new Option { Text = "Разрешается на жесткой сцепке", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Что означает знак «Дорожные работы»?",
            Options = new List<Option>
            {
                new Option { Text = "Предупреждает о работах", IsCorrect = true },
                new Option { Text = "Запрещает движение", IsCorrect = false },
                new Option { Text = "Разрешает объезд", IsCorrect = false },
                new Option { Text = "Обозначает конец дороги", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Разрешен ли разворот на остановке?",
            Options = new List<Option>
            {
                new Option { Text = "Запрещен", IsCorrect = true },
                new Option { Text = "Разрешен", IsCorrect = false },
                new Option { Text = "Разрешен если нет транспорта", IsCorrect = false },
                new Option { Text = "Разрешен только днем", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Кто должен уступить дорогу при одновременном проезде нерегулируемого перекрестка?",
            Options = new List<Option>
            {
                new Option { Text = "Водитель уступающий справа", IsCorrect = true },
                new Option { Text = "Водитель справа", IsCorrect = false },
                new Option { Text = "Оба водителя", IsCorrect = false },
                new Option { Text = "Никто", IsCorrect = false }
            }
        }
    };

    tickets.Add(ticket5);

    // Добавляем все билеты в базу
    db.Tickets.AddRange(tickets);
}