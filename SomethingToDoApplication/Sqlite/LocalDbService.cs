using SomethingToDoApp.Models;
using SomethingToDoApplication.Models;
using SQLite;

namespace SomethingToDoApp.Sqlite
{
    public class LocalDbService
    {
        private const string DB_Name = "SomethingToDoTasks.db3";
        private readonly SQLiteAsyncConnection _connection;
        private bool initialized = false;

        public LocalDbService()
        {
            _connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, DB_Name));
            _connection.CreateTableAsync<TaskModel>();
            _connection.CreateTableAsync<ShoppingTaskModel>();
        }

        async Task Init()
        {
            if (initialized) { return; }

            await _connection.CreateTableAsync<TaskModel>();
            await _connection.CreateTableAsync<ShoppingTaskModel>();

            var anyTasks = await _connection.Table<TaskModel>().CountAsync() > 0;
            if (!anyTasks)
            {
                var demoTask = new TaskModel
                {
                    TaskName = "Demo Task",
                    TaskDescription = "This is a demo task description"
                };
                await _connection.InsertAsync(demoTask);
            }

            var anyShopping = await _connection.Table<ShoppingTaskModel>().CountAsync() > 0;
            if (!anyShopping)
            {
                var demoShopping = new ShoppingTaskModel
                {
                    ShoppingName = "Milk",
                    ShoppingDescription = "1L whole milk",
                    ShoppingQuantity = 1
                };
                await _connection.InsertAsync(demoShopping);
            }

            initialized = true;
        }

        // TaskModel methods
        public async Task<List<TaskModel>> GetTasks()
        {
            await Init();
            return await _connection.Table<TaskModel>().ToListAsync();
        }

        public async Task<TaskModel> GetRandomTask()
        {
            await Init();
            var allTasks = await GetTasks();
            if (!allTasks.Any()) return null;

            var random = new Random();
            return allTasks[random.Next(allTasks.Count)];
        }

        public async Task<TaskModel> GetTaskById(int id)
        {
            await Init();
            return await _connection.Table<TaskModel>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateTaskIfNotExists(TaskModel task)
        {
            if (task == null) return;
            if (string.IsNullOrWhiteSpace(task.TaskName)) return;

            await Init();
            var normalized = task.TaskName.Trim().ToLowerInvariant();

            var all = await _connection.Table<TaskModel>()
                .Where(x => x.TaskName != null)
                .ToListAsync();
            var count = all.Count(t => t.TaskName.Trim().ToLowerInvariant() == normalized);

            if (count == 0)
            {
                task.Id = 0;
                await _connection.InsertAsync(task);
            }
        }

        public async Task CreateTask(TaskModel task)
        {
            await Init();
            await _connection.InsertAsync(task);
        }

        public async Task UpdateTask(TaskModel task)
        {
            await Init();
            await _connection.UpdateAsync(task);
        }

        public async Task DeleteTask(TaskModel task)
        {
            await Init();
            await _connection.DeleteAsync(task);
        }

        // ShoppingTaskModel methods
        public async Task<List<ShoppingTaskModel>> GetShoppingTasks()
        {
            await Init();
            return await _connection.Table<ShoppingTaskModel>().ToListAsync();
        }

        public async Task<ShoppingTaskModel> GetShoppingTaskById(int id)
        {
            await Init();
            return await _connection.Table<ShoppingTaskModel>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateShoppingTaskIfNotExists(ShoppingTaskModel task)
        {
            if (task == null) return;
            if (string.IsNullOrWhiteSpace(task.ShoppingName)) return;

            await Init();
            var normalized = task.ShoppingName.Trim().ToLowerInvariant();

            var all = await _connection.Table<ShoppingTaskModel>()
                .Where(x => x.ShoppingName != null)
                .ToListAsync();
            var count = all.Count(t => t.ShoppingName.Trim().ToLowerInvariant() == normalized);

            if (count == 0)
            {
                task.Id = 0;
                await _connection.InsertAsync(task);
            }
        }

        public async Task CreateShoppingTask(ShoppingTaskModel task)
        {
            await Init();
            await _connection.InsertAsync(task);
        }

        public async Task UpdateShoppingTask(ShoppingTaskModel task)
        {
            await Init();
            await _connection.UpdateAsync(task);
        }

        public async Task DeleteShoppingTask(ShoppingTaskModel task)
        {
            await Init();
            await _connection.DeleteAsync(task);
        }
    }
}
