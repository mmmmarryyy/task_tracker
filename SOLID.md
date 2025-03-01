# Применение принципов SOLID

## 1. Принцип единственной ответственности (SRP)

**Использование:** Разделение ответственности путем выделения `PasswordHasher` из `AuthService`  
`AuthService` отвечает только за бизнес-логику регистрации/авторизации, в то время как `PasswordHasher` – только за хэширование паролей

```csharp
public static class PasswordHasher
{
    public static string HashPassword(string password) {}
    public static bool VerifyPassword(string password, string hash) {}
}

public class AuthService
{
    public AuthService() {}

    public User Register(string username, string password)
    {
        var user = new User 
        {
            Username = username,
            PasswordHash = PasswordHasher.HashPassword(password) // delegating hashing
        };

        // add user to UserRepository
        return user;
    }

    public User Login(string username, string password)
    {
        var user = ???; // get user by username

        if (user != null && PasswordHasher.VerifyPassword(password, user.PasswordHash))
        {
            return user;
        }

        return null;
    }
}
```

## 2. Принцип открытости/закрытости (OCP)

**Использование:** Добавление новых способов сортировки задач без изменения существующего кода

```csharp
public interface ITaskSortStrategy
{
    IEnumerable<Task> Sort(IEnumerable<Task> tasks);
}

public class SortByPriority : ITaskSortStrategy
{
    public IEnumerable<Task> Sort(IEnumerable<Task> tasks)  
    {
        return tasks.OrderBy(t => t.Priority);
    }
}

public class SortByDeadline : ITaskSortStrategy
{
    public IEnumerable<Task> Sort(IEnumerable<Task> tasks) 
    {
        return tasks.OrderBy(t => t.Deadline);
    }
}

public class TaskService
{
    private ITaskSortStrategy _sortStrategy = new SortByPriority();

    public void SetSortStrategy(ITaskSortStrategy strategy) 
    {
        _sortStrategy = strategy;
    }

    public List<Task> GetSortedTasks(int userId)
    {
        var tasks = GetTasksByUser(userId);
        return _sortStrategy.Sort(tasks).ToList();
    }
    
    // ...
}
```

## 3. Принцип подстановки Лисков (LSP)

**Использование:** Репозитории корректно реализуют контракты интерфейсов, не усиливают предусловий, не ослабляют постусловий, не нарушают инвариантов и не генерируют типов исключений, не описанных в базовом классе

```csharp
public interface IUserRepository
{
    User Add(User user);
    User GetByUsername(string username);
    User GetById(int userId);
}

public class UserRepository : IUserRepository
{
    public User Add(User user)
    {
        // add user
        return user;
    }

    public User GetByUsername(string username) 
    {
        var user = ???; // get user by username
        return user;
    }

    public User GetById(int userId) 
    {
        var user = ???; // get user by id
        return user;
    }
}
```

## 4. Принцип разделения интерфейсов (ISP)

**Использование:** Разделим ответственность интерфейсов для операций чтения и записи задач.

```csharp
public interface ITaskReadRepository
{
    Task GetById(int taskId);
    List<Task> GetByUserId(int userId);
}

public interface ITaskWriteRepository
{
    Task Add(Task task);
    Task Update(Task task);
    void Delete(int taskId);
}

public class TaskRepository : ITaskReadRepository, ITaskWriteRepository
{
    // ...
}
```

## 5. Принцип инверсии зависимостей (DIP)

**Использование:** Сервисы зависят от абстракций, а не от конкретных реализаций.

```csharp
public class TaskService
{
    private readonly ITaskReadRepository _readRepository;
    private readonly ITaskWriteRepository _writeRepository;

    // dependency injection via constructor
    public TaskService(ITaskReadRepository readRepository, ITaskWriteRepository writeRepository)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
    }

    public Task CreateTask(Task task)
    {
        // validation...
        return _writeRepository.Add(task);
    }

    public List<Task> GetTasksByUser(int userId) {
        return _readRepository.GetByUserId(userId);
    }
}
```

---

# Итоговая схема SOLID

| Принцип       | Реализация в проекте                                                                 |
|---------------|--------------------------------------------------------------------------------------|
| **SRP**       | `AuthService` делегирует хэширование `PasswordHasher`                                |
| **OCP**       | Стратегии сортировки задач через `ITaskSortStrategy`                                 |
| **LSP**       | `UserRepository` полностью соответствует контракту `IUserRepository`                 |
| **ISP**       | Разделение `ITaskReadRepository` и `ITaskWriteRepository`                            |
| **DIP**       | Внедрение репозиториев через интерфейсы в `TaskService` и `AuthService`              |
