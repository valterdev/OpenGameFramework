using OpenGameFramework.DI;

namespace OpenGameFramework.API
{
    public partial interface API
    {
        public partial interface Domain
        {
            public interface IMatch3BoardService : IServiceDi
            {
                public void Test();
            };
        }
    }
}