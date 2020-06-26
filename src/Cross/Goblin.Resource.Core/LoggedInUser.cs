using Goblin.Core.Web.Utils;

namespace Goblin.Resource.Core
{
    public class LoggedInUser
    {
        public LoggedInUser()
        {
        }

        public LoggedInUser(long id)
        {
            Id = id;
        }

        public static LoggedInUser Current
        {
            get => Get();
            set => Set(value);
        }

        private static LoggedInUser Get()
        {
            return SingletonPerHttpRequest<LoggedInUser>.Current;
        }

        private static void Set(LoggedInUser value)
        {
            SingletonPerHttpRequest<LoggedInUser>.Current = value;
        }

        public long Id { get; set; }
    }
}