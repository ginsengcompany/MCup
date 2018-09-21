using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCup.FormsVideoLibrary
{
   public interface IVideoPlayerController
    {
        VideoStatus Status { set; get; }
        TimeSpan Duration { set; get; }
    }
}
