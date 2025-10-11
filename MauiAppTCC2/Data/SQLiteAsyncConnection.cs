
namespace MauiAppTCC2.Data
{
    internal class SQLiteAsyncConnection
    {
        private string databasePath;
        private object value;

        public SQLiteAsyncConnection(string databasePath, object value)
        {
            this.databasePath = databasePath;
            this.value = value;
        }

        internal async Task CreateTableAsync<T>()
        {
            throw new NotImplementedException();
        }

        internal object Table<T>()
        {
            throw new NotImplementedException();
        }
    }
}