using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

namespace OpenGameFramework
{
    public interface IAppTask
    {
        public UniTask Run();
    }
}
