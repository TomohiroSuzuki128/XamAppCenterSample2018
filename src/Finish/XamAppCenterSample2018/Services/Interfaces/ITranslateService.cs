using System;
using System.Threading.Tasks;

namespace XamAppCenterSample2018.Services.Interfaces
{
    public interface ITranslateService
    {
        Task<string> Translate(string text);
    }
}
