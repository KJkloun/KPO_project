
#№ Мини ДЗ по КПО

## Оглавление
1. [Использование DI-контейнера](#использование-di-контейнера)
2. [Структура репозитория](#структура-репозитория)
3. [Сборка и запуск](#сборка-и-запуск)
4. [Использование приложения](#использование-приложения)
5. [Тестирование](#тестирование)
6. [Покрытие кода (Code Coverage)](#покрытие-кода-code-coverage)
7. [Дополнительно](#дополнительно)

---

## Использование DI-контейнера
- В `Program.cs` создаётся `ServiceCollection`, где регистрируются:
  ```csharp
  services.AddSingleton<IVeterinaryClinic, VeterinaryClinic>();
  services.AddSingleton<Zoo>();
  ```
- После `BuildServiceProvider()` получаем экземпляр `Zoo` с внедрёнными зависимостями.

Это упрощает замену/тестирование компонентов, т.к. мы можем внедрять «фейковые» реализации при тестах.

---

## Структура репозитория

```plaintext
ZooERP.sln
├── ZooERP
│   ├── Program.cs
│   ├── Interfaces
│   │   ├── IAlive.cs
│   │   ├── IInventory.cs
│   │   └── IVeterinaryClinic.cs
│   ├── Managers
│   │   └── Zoo.cs
│   ├── Models
│   │   ├── Animal.cs
│   │   ├── Herbo.cs
│   │   ├── Predator.cs
│   │   ├── Monkey.cs
│   │   ├── Rabbit.cs
│   │   ├── Tiger.cs
│   │   ├── Wolf.cs
│   │   ├── Thing.cs
│   │   ├── Table.cs
│   │   └── Computer.cs
│   └── Services
│       └── VeterinaryClinic.cs
└── ZooERP.Tests
    ├── ZooTests.cs
    ├── ZooAdvancedTests.cs
    └── ZooERP.Tests.csproj
```

- **ZooERP** — основной проект (консольное приложение).
- **ZooERP.Tests** — проект с юнит-тестами (xUnit).

---

## Сборка и запуск

1. **Клонировать репозиторий** или скопировать код в локальную папку.
2. **Перейти** в папку с файлом решения (`.sln`).
3. Выполнить:
   ```bash
   dotnet build
   ```
   Убедитесь, что сборка проходит без ошибок.

4. **Запуск консольного приложения**:
   ```bash
   dotnet run --project .\ZooERP\ZooERP.csproj
   ```
   Либо перейти в папку `ZooERP` и вызвать `dotnet run` без аргументов.

---

## Использование приложения

После запуска появится **консольное меню**:

```
--- Меню ---
1. Добавить новое животное
2. Вывести отчет по животным
3. Вывести список животных для контактного зоопарка
4. Вывести список всех объектов
5. Выход
Выберите опцию:
```

1. **Добавить новое животное**  
   - Предлагается выбрать тип (обезьяна, кролик, тигр, волк).  
   - Нужно ввести имя, количество потребляемой еды (кг/сутки), для травоядных также — уровень доброты (0–10).  
   - Далее программа спросит: «здорово ли животное?» (через `Y`/`y`). Если да, животное добавится, иначе будет отклонено.

2. **Вывести отчет по животным**  
   - Показывает количество животных и суммарное количество потребляемой еды (кг в сутки).

3. **Вывести список животных для контактного зоопарка**  
   - Показывает всех травоядных с `Kindness > 5`.

4. **Вывести список всех объектов**  
   - Показывает все вещи и животных с их **инвентарными номерами**.

5. **Выход**  
   - Завершение работы приложения.

Пример **добавления вещей** (стол, компьютер) уже реализован в коде.

---

## Тестирование

### Запуск тестов

1. **Перейти** в корень решения (`.sln`).
2. Выполнить:
   ```bash
   dotnet test
   ```
3. Тесты из проекта `ZooERP.Tests` будут обнаружены автоматически и запустятся.  
4. В консоли отобразится результат (сколько тестов прошло/упало).

### Обзор тестов

- **ZooTests.cs**: базовые проверки (добавление животных, расчёт еды, фильтрация травоядных, добавление вещей).  
- **ZooAdvancedTests.cs**: расширенные проверки (граничные значения доброты, проверка корректности инвентарных номеров, отсутствие животных, отрицательные значения еды и т.д.).

Мы используем **`FakeVetClinic`**, чтобы контролировать, «здорово» животное или нет, без реального ввода с консоли.

---

## Покрытие кода (Code Coverage)

Для получения отчёта о покрытии кода:

1. **Установите** пакет `coverlet.collector` (если ещё не установлен) в проекте тестов (обычно это делается в `.csproj`):
   ```xml
   <ItemGroup>
     <PackageReference Include="coverlet.collector" Version="3.2.0" />
   </ItemGroup>
   ```
2. **Запустите** тесты с параметром покрытия:
   ```bash
   dotnet test --collect:"XPlat Code Coverage"
   ```
3. По завершении тестов в папке `TestResults/<GUID>` будет файл `coverage.cobertura.xml` (или другой формат).  

### Преобразование отчёта в HTML
Чтобы просмотреть результаты в удобном виде (графика, диаграммы):
1. **Установите ReportGenerator** как глобальный инструмент:
   ```bash
   dotnet tool install --global dotnet-reportgenerator-globaltool
   ```
2. **Выполните** команду:
   ```bash
   reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html
   ```
   - Параметр `-reports` указывает путь к файлу отчёта (используем маску `**/coverage.cobertura.xml`).
   - Параметр `-targetdir` задаёт, куда сложить файлы отчёта.
   - `-reporttypes:Html` указывает формат отчёта (HTML).

3. **Откройте** файл `index.htm` (или `index.html`) из папки `coveragereport` в браузере, чтобы посмотреть детальные результаты покрытия кода.

---

