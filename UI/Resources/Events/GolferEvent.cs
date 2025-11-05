using Core.Models;

namespace InterfaceControls.Events
{
    public class GolferEvent : EventArgs
    {
        public Golfer Golfer { get; set;  }

        public GolferEvent(Golfer golfer)
        {
            Golfer = golfer;
        }
    }
}
