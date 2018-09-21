using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCup.FormsVideoLibrary
{
   public interface IVideoPicker
    {
        Task<string> GetVideoFileAsync();
    }
}
